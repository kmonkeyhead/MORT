using MORT.Service.Plamo;
using System.Text.Json;

void Check(bool ok, string message) { if (!ok) throw new Exception(message); Console.WriteLine("PASS " + message); }
async Task MustFail(Func<Task> run, string message) { try { await run(); } catch { Console.WriteLine("PASS " + message); return; } throw new Exception(message); }
MORT.LocalizeManager.LocalizeManager.Init("", MORT.LocalizeManager.AppLanguage.English);
Check(PlamoService.InitializationNotice.StartsWith("Loading model"), "embedded English initialization notice");
string root = Path.Combine(Path.GetTempPath(), "mort-plamo-test-" + Guid.NewGuid().ToString("N"));
try {
    using var service = new PlamoService(root);
    Check(PlamoService.NormalizeLanguage("zh-CN") == "zh-hans", "Chinese mapping");
    Check(PlamoService.NormalizeLanguage("JA") == "ja", "Japanese mapping");
    await MustFail(() => Task.FromResult(PlamoService.NormalizeLanguage("pt")), "unsupported language rejected");
    await MustFail(() => service.StartAsync(default), "missing installation rejected");
    await MustFail(() => service.TranslateAsync("hello", "en", "ko", default), "stopped engine rejected");
    service.Configure("2b", false);
    using var reopened = new PlamoService(root);
    Check(reopened.Settings.Model == "2b" && !reopened.Settings.UseCuda, "settings persisted");
    using var cancelled = new CancellationTokenSource(); cancelled.Cancel();
    await MustFail(() => service.InstallAsync(cancelled.Token), "cancelled download rejected");
    await service.StopAsync(); await service.StopAsync();
    Check(!service.IsReady, "stop idempotent");
} finally { Directory.Delete(root, true); }
if (args.Contains("--live")) {
    string liveRoot = args.Contains("--root") ? args[Array.IndexOf(args,"--root") + 1] : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MORT", "Plamo");
    using var service = new PlamoService(liveRoot);
    service.Configure(args.Contains("--2b") ? "2b" : "8b", true);
    service.StatusChanged += Console.WriteLine;
    using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(10));
    await service.InstallAsync(timeout.Token);
    Check(!service.IsReady, "not started before translation");
    var cases = new[] { ("Reinforcements will arrive in 30 seconds.","en"), ("Support arrives in {seconds} seconds. Available units: {current}/{maximum}.","en"), ("敵の増援が到着しました。撤退地点に向かってください。","ja") };
    foreach (var (input, source) in cases) {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        string output = await service.TranslateAsync(input, source,"ko",timeout.Token);
        Check(output.Any(c => c >= '가' && c <= '힣'), "Korean output: " + output);
        if (input.Contains("{seconds}")) Check(output.Contains("{seconds}") && output.Contains("{current}") && output.Contains("{maximum}"), "placeholders preserved");
        Console.WriteLine(JsonSerializer.Serialize(new { input, output, seconds=watch.Elapsed.TotalSeconds }));
    }
    Check(service.IsReady,"translation automatically started server");
    var inFlight = service.TranslateAsync("The first dropship has arrived. Keep all units near the extraction zone and wait for further orders.", "en", "ko", timeout.Token);
    await Task.WhenAll(inFlight, service.ReconfigureAsync("2b", true, timeout.Token));
    Check(service.IsReady && service.Settings.Model == "2b", "running translation completed before switching to 2B");
    string switched = await service.TranslateAsync("Reinforcements have arrived.", "en", "ko", timeout.Token);
    Check(switched.Any(c => c >= '가' && c <= '힣'), "translation after switch: " + switched);
    await service.ReconfigureAsync("8b", true, timeout.Token);
    Check(service.IsReady && service.Settings.Model == "8b", "switch back to 8B restarts automatically");
    using (var nextApp = new PlamoService(service.Root)) {
        Check(nextApp.CleanupPreviousServers() >= 1, "next app cleaned previous managed server");
        Check(!service.IsReady, "previous server exited");
    }
    await service.StopAsync(); Check(!service.IsReady,"real server stopped");
    await service.StartAsync(timeout.Token); Check(service.IsReady,"real server restarted");
    service.Dispose(); Check(!service.IsReady,"app disposal stopped server");
}