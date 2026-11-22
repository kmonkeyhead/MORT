using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.Service.Plamo;

public sealed record PlamoModel(string Id, string Title, long Bytes, string Sha256, string Revision)
{
    public string FileName => $"plamo-3-translate-{Id}-Q4_K_M.gguf";
    public string Url => $"https://huggingface.co/pfnet/plamo-3-translate-{Id}-GGUF/resolve/{Revision}/{FileName}";
    public override string ToString() => Title;
}
public sealed class PlamoSettings
{
    public const string DefaultModel = "2b";
    public string Model { get; set; } = DefaultModel;
    public bool UseCuda { get; set; } = true;
}

/// <summary>Owns only the server it starts. Downloads are verified before becoming executable/usable.</summary>
public sealed class PlamoService : IDisposable
{
    public static readonly PlamoModel[] Models = {
        new("0.8b", "PLaMo 3 Translate 0.8B · 573 MB", 573126848, "70891ec70dc7ab4205745a38307867c2881b99239f20327927633abfe4bc9ec6", "7de4409f84a9d70e3a5e87b81e84883d49f77b58"),
        new("2b", "PLaMo 3 Translate 2B · 1.81 GB", 1807203040, "1ffd9073afcc70e35ccc942428aa2b73d7aa2b9ea779bf504b775369e0276c13", "4f672106b847200bfdc0f6af6c321cd94eba1aed"),
        new("8b", "PLaMo 3 Translate 8B · 5.28 GB", 5280445728, "94e5d3f91b421f7c8900ddaac27b11a19f7441426c954bbb25a2fd60a1640187", "c67efa4eefdc14d3c97bf8f4f35ee58e2ecd2f42")
    };
    private const string Release = "b12405";
    private readonly HttpClient _http = new() { Timeout = Timeout.InfiniteTimeSpan };
    private readonly SemaphoreSlim _gate = new(1);
    private readonly CancellationTokenSource _lifetime = new();
    private readonly object _processLock = new();
    private Process? _process;
    private string? _baseUrl;
    private string? _apiKey;
    private bool _ready;
    private bool _disposed;
    private bool _restartAfterInstall;
    public string Root { get; }
    public PlamoSettings Settings { get; private set; }
    public static string InitializationNotice => MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO NOTICE");
    private volatile bool _initializing;
    public bool IsInitializing => _initializing;
    private volatile bool _installing;
    public string DisplayStatus => $"PLaMo 3 {SelectedModel.Id.ToUpperInvariant()} - {(_installing ? MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO DOWNLOADING") : _initializing ? MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO INITIALIZING") : IsReady ? MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO READY") : MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO NEEDS INIT"))}";
    public string Status { get; private set; } = MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO STOPPED");
    public event Action<string>? StatusChanged;
    public bool IsReady { get { lock (_processLock) return _ready && _process is { HasExited: false }; } }
    public PlamoModel SelectedModel => Models.First(m => m.Id == Settings.Model);
    private string SettingsPath => Path.Combine(Root, "settings.json");
    private string RuntimePath => Path.Combine(Root, Release + (Settings.UseCuda ? "-cuda" : "-cpu"));
    public bool IsInstalled => File.Exists(ModelPath) && File.Exists(Path.Combine(RuntimePath, "llama-server.exe"));
    public string ModelPath => Path.Combine(Root, SelectedModel.FileName);

    public PlamoService() : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MORT", "Plamo")) { }
    public PlamoService(string root)
    {
        Root = Path.GetFullPath(root);
        Directory.CreateDirectory(Root);
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("MORT-Plamo/1.0");
        try { Settings = JsonSerializer.Deserialize<PlamoSettings>(File.ReadAllText(SettingsPath)) ?? new(); }
        catch { Settings = new(); }
        if (!Models.Any(m => m.Id == Settings.Model)) Settings.Model = PlamoSettings.DefaultModel;
    }
    private void Report(string value) { Status = value; StatusChanged?.Invoke(value); }
    public void Configure(string model, bool cuda)
    {
        if (!Models.Any(m => m.Id == model)) throw new ArgumentException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO UNKNOWN"));
        if (!_gate.Wait(0)) throw new InvalidOperationException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO WAIT"));
        try {
            if (Settings.Model == model && Settings.UseCuda == cuda) return;
            if (IsReady || _process != null) throw new InvalidOperationException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO STOP MODEL"));
            Settings = new() { Model = model, UseCuda = cuda };
            File.WriteAllText(SettingsPath + ".tmp", JsonSerializer.Serialize(Settings));
            File.Move(SettingsPath + ".tmp", SettingsPath, true);
        } finally { _gate.Release(); }
    }
    private static async Task<bool> Matches(string path, string hash, CancellationToken token)
    {
        if (!File.Exists(path)) return false;
        using var file = File.OpenRead(path);
        return Convert.ToHexString(await SHA256.HashDataAsync(file, token).ConfigureAwait(false)).Equals(hash, StringComparison.OrdinalIgnoreCase);
    }
    private async Task Download(string url, string path, string hash, CancellationToken token)
    {
        if (await Matches(path, hash, token).ConfigureAwait(false)) return;
        string partial = path + ".partial";
        try {
            using var response = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            long total = response.Content.Headers.ContentLength ?? 0, done = 0;
            using (var input = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false))
            using (var output = new FileStream(partial, FileMode.Create, FileAccess.Write, FileShare.None, 131072, true)) {
                byte[] buffer = new byte[131072];
                var clock = Stopwatch.StartNew();
                int n;
                while ((n = await input.ReadAsync(buffer, token).ConfigureAwait(false)) > 0) {
                    await output.WriteAsync(buffer.AsMemory(0,n), token).ConfigureAwait(false); done += n;
                    if (clock.ElapsedMilliseconds > 250) { Report(string.Format(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO PROGRESS"), Path.GetFileName(path), done/1000000, total/1000000)); clock.Restart(); }
                }
            }
            Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO CHECK DOWNLOAD"));
            if (!await Matches(partial, hash, token).ConfigureAwait(false)) throw new IOException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO BAD HASH"));
            File.Move(partial, path, true);
        } finally { if (File.Exists(partial)) File.Delete(partial); }
    }
    public async Task InstallAsync(CancellationToken cancellationToken)
    {
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _lifetime.Token);
        var token = linked.Token;
        await _gate.WaitAsync(token).ConfigureAwait(false);
        try {
            if (_process != null) throw new InvalidOperationException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO STOP MODEL"));
            _installing = true;
            Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO CHECK MODEL"));
            var model = SelectedModel;
            if (!await Matches(ModelPath, model.Sha256, token).ConfigureAwait(false)) {
                // Reuse a verified Ollama blob without requiring Ollama to run or another download.
                string ollama = Environment.GetEnvironmentVariable("OLLAMA_MODELS") ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ollama", "models");
                string blob = Path.Combine(ollama, "blobs", "sha256-" + model.Sha256);
                if (await Matches(blob, model.Sha256, token).ConfigureAwait(false)) {
                    Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO COPY"));
                    using (var input = File.OpenRead(blob))
                    using (var output = File.Create(ModelPath + ".partial")) await input.CopyToAsync(output, token).ConfigureAwait(false);
                    File.Move(ModelPath + ".partial", ModelPath, true);
                } else await Download(model.Url, ModelPath, model.Sha256, token).ConfigureAwait(false);
            }
            string platform = Settings.UseCuda ? "cuda-12.4" : "cpu";
            string archive = $"llama-{Release}-bin-win-{platform}-x64.zip";
            string hash = Settings.UseCuda ? "f069a0ecf89a89be67c0af097b9278baa8d2629becaf6c897defff0059dd70d2" : "6a382b6e7fd07a6fe4d0d888dcce7e1f10471b53554e0f47631f780c8edf51e9";
            await Download($"https://github.com/ggml-org/llama.cpp/releases/download/{Release}/{archive}", Path.Combine(Root,archive), hash, token).ConfigureAwait(false);
            string? cudaArchive = null;
            if (Settings.UseCuda) {
                cudaArchive = "cudart-llama-bin-win-cuda-12.4-x64.zip";
                await Download($"https://github.com/ggml-org/llama.cpp/releases/download/{Release}/{cudaArchive}", Path.Combine(Root,cudaArchive), "8c79a9b226de4b3cacfd1f83d24f962d0773be79f1e7b75c6af4ded7e32ae1d6", token).ConfigureAwait(false);
            }
            Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO EXTRACT"));
            foreach (var active in Process.GetProcessesByName("llama-server"))
            using (active) {
                string? executable;
                try { executable = active.MainModule?.FileName; }
                catch (System.ComponentModel.Win32Exception) { continue; }
                catch (InvalidOperationException) { continue; }
                if (string.Equals(executable, Path.Combine(RuntimePath, "llama-server.exe"), StringComparison.OrdinalIgnoreCase))
                    throw new IOException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO IN USE"));
            }
            string staging = RuntimePath + ".staging";
            if (Directory.Exists(staging)) Directory.Delete(staging,true);
            Directory.CreateDirectory(staging);
            ZipFile.ExtractToDirectory(Path.Combine(Root,archive), staging, true);
            if (cudaArchive != null) ZipFile.ExtractToDirectory(Path.Combine(Root,cudaArchive), staging, true);
            token.ThrowIfCancellationRequested();
            if (!File.Exists(Path.Combine(staging,"llama-server.exe"))) throw new IOException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO NO ENGINE"));
            if (Directory.Exists(RuntimePath)) Directory.Delete(RuntimePath,true);
            Directory.Move(staging, RuntimePath);
            _installing = false;
            Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO DONE"));
            if (_restartAfterInstall) { await StartCoreAsync(token).ConfigureAwait(false); _restartAfterInstall = false; }
        } catch (OperationCanceledException) { _installing = false; Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO CANCELED")); throw; }
        catch (Exception ex) { _installing = false; Report(ex.Message); throw; }
        finally { _gate.Release(); }
    }
    public async Task StartAsync(CancellationToken token)
    {
        await _gate.WaitAsync(token).ConfigureAwait(false);
        try { await StartCoreAsync(token).ConfigureAwait(false); }
        finally { _gate.Release(); }
    }
    public async Task ReconfigureAsync(string model, bool cuda, CancellationToken token)
    {
        if (!Models.Any(m => m.Id == model)) throw new ArgumentException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO UNKNOWN"));
        await _gate.WaitAsync(token).ConfigureAwait(false);
        try {
            token.ThrowIfCancellationRequested();
            if (Settings.Model == model && Settings.UseCuda == cuda) return;
            bool restart = _process != null || _restartAfterInstall;
            var settings = new PlamoSettings { Model = model, UseCuda = cuda };
            // Persist before touching the running process so save errors leave it usable.
            File.WriteAllText(SettingsPath + ".tmp", JsonSerializer.Serialize(settings));
            File.Move(SettingsPath + ".tmp", SettingsPath, true);
            Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO CHANGE"));
            StopProcess();
            Settings = settings;
            _restartAfterInstall = restart;
            if (restart && IsInstalled) {
                await StartCoreAsync(token).ConfigureAwait(false);
                _restartAfterInstall = false;
            } else Report(restart ? MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO RESTART") : MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO SAVED"));
        } catch (Exception ex) { Report(ex.Message); throw; }
        finally { _gate.Release(); }
    }
    private async Task StartCoreAsync(CancellationToken cancellationToken)
    {
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _lifetime.Token);
        linked.CancelAfter(TimeSpan.FromMinutes(3));
        var token = linked.Token;

        try {
            if (IsReady) return;
            StopProcess();
            string executable = Path.Combine(RuntimePath, "llama-server.exe");
            if (!File.Exists(executable) || !File.Exists(ModelPath)) throw new InvalidOperationException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO INSTALL FIRST"));
            _initializing = true;
            Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO VERIFY"));
            if (!await Matches(ModelPath, SelectedModel.Sha256, token).ConfigureAwait(false)) throw new IOException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO BAD MODEL"));
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start(); int port = ((IPEndPoint)listener.LocalEndpoint).Port; listener.Stop();
            _baseUrl = $"http://127.0.0.1:{port}";
            _apiKey = Convert.ToHexString(RandomNumberGenerator.GetBytes(24));
            var info = new ProcessStartInfo(executable) { WorkingDirectory = RuntimePath, UseShellExecute = false, CreateNoWindow = true, RedirectStandardError = true, RedirectStandardOutput = true };
            foreach (var arg in new[] { "-m", ModelPath, "-c", "4096", "-np", "1", "-ngl", Settings.UseCuda ? "99" : "0", "--host", "127.0.0.1", "--port", port.ToString(), "--alias", "plamo-3-translate", "--jinja", "--no-prefill-assistant", "--api-key", _apiKey }) info.ArgumentList.Add(arg);
            Report(InitializationNotice);
            var process = new Process { StartInfo = info };
            // Drain both streams, but do not persist OCR text or credentials in logs.
            process.OutputDataReceived += (_,_) => { };
            process.ErrorDataReceived += (_,_) => { };
            lock (_processLock) { token.ThrowIfCancellationRequested(); _process = process; process.Start(); process.BeginOutputReadLine(); process.BeginErrorReadLine(); }
            while (true) {
                token.ThrowIfCancellationRequested();
                if (process.HasExited) throw new IOException(string.Format(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO EXIT"), process.ExitCode));
                try {
                    using var check = new HttpRequestMessage(HttpMethod.Get, _baseUrl + "/health");
                    check.Headers.Authorization = new("Bearer", _apiKey);
                    using var response = await _http.SendAsync(check, token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode && !process.HasExited) break;
                } catch (HttpRequestException) { }
                await Task.Delay(400, token).ConfigureAwait(false);
            }
            lock (_processLock) { token.ThrowIfCancellationRequested(); if (_process != process || process.HasExited) throw new IOException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO ENDED")); _ready = true; }
            _initializing = false;
            Report(string.Format(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO RUNNING"), SelectedModel.Title));
        } catch (Exception ex) { _initializing = false; StopProcess(); Report(ex is OperationCanceledException ? MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO TIMEOUT") : ex.Message); throw; }

    }
    public async Task<string> TranslateAsync(string text, string source, string target, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";
        await _gate.WaitAsync(token).ConfigureAwait(false);
        try {
        await StartCoreAsync(token).ConfigureAwait(false);
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(token, _lifetime.Token);
        linked.CancelAfter(TimeSpan.FromSeconds(90));
        using var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/v1/chat/completions");
        request.Headers.Authorization = new("Bearer", _apiKey);
        var placeholders = Regex.Matches(text, @"\{[A-Za-z_][A-Za-z0-9_]*\}").Select(m => m.Value).ToArray();
        request.Content = JsonContent.Create(new { model = "plamo-3-translate", messages = new[] { new { role = "user", content = text } }, stream = false, temperature = 0, max_tokens = 1536, stop = new[] { "<|plamo:tag|>", "<|plamo:bos|>" }, chat_template_kwargs = new { source_lang = NormalizeLanguage(source), target_lang = NormalizeLanguage(target), glossary = placeholders.Distinct().Select(p => new { source = p, target = p }).ToArray(), context = "Game text. Preserve all placeholders in braces exactly as written, including their spelling and case." } });
        using var response = await _http.SendAsync(request, linked.Token).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(linked.Token).ConfigureAwait(false));
        var choice = json.RootElement.GetProperty("choices")[0];
        if (choice.GetProperty("finish_reason").GetString() == "length") throw new IOException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO LENGTH"));
        string result = choice.GetProperty("message").GetProperty("content").GetString() ?? "";
        if (string.IsNullOrWhiteSpace(result)) throw new IOException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO EMPTY"));
        foreach (var placeholder in placeholders.Distinct())
            if (Regex.Matches(result, Regex.Escape(placeholder)).Count != placeholders.Count(p => p == placeholder))
                throw new IOException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO VARIABLE"));
        return result.Trim();
        } finally { _gate.Release(); }
    }
    private static readonly string[] Languages = "ja en zh-hans zh-hant ko ar az be bg bn ca ceb de es et eu fa fr gu he hi hr hu hy id it jv ka kk km lt lv ml mr ms my ne nl pa pl ru si sk sl sr sw ta te th tl ur uz vi".Split(' ');
    public static string NormalizeLanguage(string language)
    {
        string code = language.ToLowerInvariant() switch {
            "zh-cn" => "zh-hans", "zh-tw" => "zh-hant", _ => language.ToLowerInvariant()
        };
        if (!Languages.Contains(code)) throw new ArgumentException(string.Format(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO LANG"), language));
        return code;
    }
    // Only MORT-managed runtime paths are eligible, never a process name alone.
    public int CleanupPreviousServers()
    {
        int count = 0;
        string prefix = Path.TrimEndingDirectorySeparator(Path.GetFullPath(Root)) + Path.DirectorySeparatorChar;
        foreach (var process in Process.GetProcessesByName("llama-server"))
        using (process)
        {
            try
            {
                string? path = process.MainModule?.FileName;
                if (path == null || !Path.GetFullPath(path).StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;
                if (_process != null && process.Id == _process.Id) continue;
                process.Kill(true);
                if (!process.WaitForExit(5000)) throw new IOException(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO CLEANUP"));
                count++;
            }
            catch (System.ComponentModel.Win32Exception) { }
            catch (ArgumentException) { }
            catch (InvalidOperationException) { }
        }
        if (count > 0) Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO CLEANED"));
        return count;
    }
    private void StopProcess()
    {
        lock (_processLock) {
            _ready = false;
            if (_process != null) {
                try { if (!_process.HasExited) { _process.Kill(true); _process.WaitForExit(5000); } }
                catch (InvalidOperationException) { }
                finally { _process.Dispose(); _process = null; }
            }
        }
    }
    public async Task StopAsync()
    {
        await _gate.WaitAsync().ConfigureAwait(false);
        try { StopProcess(); Report(MORT.LocalizeManager.LocalizeManager.GetLocalizeString("PLAMO RELEASED")); } finally { _gate.Release(); }
    }
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true; _lifetime.Cancel(); StopProcess();
    }
}
