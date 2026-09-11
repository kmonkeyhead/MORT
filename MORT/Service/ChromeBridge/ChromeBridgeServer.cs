using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.Service.ChromeBridge
{
    /// <summary>
    /// 127.0.0.1 에만 붙는 최소 HTTP + WebSocket 서버. MORT 프로세스 안에서 직접 돈다.
    ///
    /// HttpListener 는 비관리자 계정에서 URL ACL 등록을 요구할 때가 있어서 TcpListener 로 직접 받는다.
    /// WebSocket 은 핸드셰이크만 여기서 하고 프레임 처리는 WebSocket.CreateFromStream 에 넘긴다.
    ///
    /// MORT 자신은 번역할 때 이 HTTP를 거치지 않고 허브를 바로 부른다.
    /// /translate 는 커스텀 API 프리셋으로 붙이거나 손으로 확인할 때 쓰는 통로다.
    /// </summary>
    public class ChromeBridgeServer
    {
        private const string WebSocketGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private const int MaxHeadBytes = 16 * 1024;
        private const int MaxBodyBytes = 1024 * 1024;
        private const string PageResourceName = "MORT.Service.ChromeBridge.Web.bridge.html";

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        private readonly ChromeBridgeHub _hub;
        private readonly string _page;

        private TcpListener? _listener;
        private CancellationTokenSource? _cts;

        public int Port { get; private set; }
        public bool IsRunning => _listener != null;

        /// <summary>
        /// 브릿지 페이지 주소. MORT의 UI 언어를 lang 으로 실어 보낸다.
        ///
        /// 페이지가 자기 문구를 스스로 고르는 데 쓴다. 붙고 나서 WebSocket으로 알려 주면 창이 뜬 뒤에
        /// 문구가 한 번 갈아 끼워지므로, 처음 그려질 때부터 맞도록 주소에 실는다.
        /// 브릿지 문구는 페이지 안에 ko/en 두 벌로 들고 있어서 localize.csv 에는 키를 두지 않는다.
        /// </summary>
        public string PageUrl => $"http://127.0.0.1:{Port.ToString(CultureInfo.InvariantCulture)}/?lang={PageLanguage}";

        /// <summary>페이지가 아는 언어는 ko 와 en 뿐이다. 나머지 UI 언어는 en 으로 보낸다.</summary>
        private static string PageLanguage =>
            MORT.LocalizeManager.LocalizeManager.Language == MORT.LocalizeManager.AppLanguage.Korea ? "ko" : "en";

        public ChromeBridgeServer(ChromeBridgeHub hub)
        {
            _hub = hub;
            _page = LoadEmbeddedPage();
        }

        public bool Start(int port, out string error)
        {
            error = "";

            if (IsRunning && Port == port)
            {
                return true;
            }

            Stop();

            try
            {
                _listener = new TcpListener(IPAddress.Loopback, port);
                _listener.Start();
                Port = port;

                _cts = new CancellationTokenSource();
                CancellationToken token = _cts.Token;

                _ = Task.Run(() => AcceptLoopAsync(token));

                Util.ShowLog($"[ChromeBridge] 로컬 서버를 열었습니다 : {PageUrl}");
                return true;
            }
            catch (SocketException ex)
            {
                error = $"포트 {port.ToString(CultureInfo.InvariantCulture)} 을 열지 못했습니다 : {ex.Message}";
                _listener = null;
                return false;
            }
        }

        public void Stop()
        {
            try
            {
                _cts?.Cancel();
                _cts?.Dispose();
            }
            catch (Exception)
            {
            }

            _cts = null;

            try
            {
                _listener?.Stop();
            }
            catch (Exception)
            {
            }

            if (_listener != null)
            {
                Util.ShowLog("[ChromeBridge] 로컬 서버를 닫았습니다.");
            }

            _listener = null;
        }

        private async Task AcceptLoopAsync(CancellationToken token)
        {
            TcpListener? listener = _listener;

            while (listener != null && !token.IsCancellationRequested)
            {
                TcpClient client;

                try
                {
                    client = await listener.AcceptTcpClientAsync(token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (SocketException)
                {
                    return;
                }

                _ = Task.Run(() => HandleClientAsync(client, token));
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            try
            {
                client.NoDelay = true;

                using (NetworkStream stream = client.GetStream())
                {
                    ChromeBridgeHttpRequest? head = await ReadHeadAsync(stream, token).ConfigureAwait(false);

                    if (head == null)
                    {
                        return;
                    }

                    if (head.IsWebSocketUpgrade && head.Path == "/ws")
                    {
                        await HandleWebSocketAsync(stream, head, token).ConfigureAwait(false);
                        return;
                    }

                    await HandleHttpAsync(stream, head, token).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (IOException)
            {
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] {ex.Message}");
            }
            finally
            {
                try
                {
                    client.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        private async Task HandleWebSocketAsync(NetworkStream stream, ChromeBridgeHttpRequest head, CancellationToken token)
        {
            string accept = ComputeAcceptKey(head.GetHeader("sec-websocket-key") ?? "");

            StringBuilder response = new StringBuilder();
            response.Append("HTTP/1.1 101 Switching Protocols\r\n");
            response.Append("Upgrade: websocket\r\n");
            response.Append("Connection: Upgrade\r\n");
            response.Append("Sec-WebSocket-Accept: ").Append(accept).Append("\r\n");
            response.Append("\r\n");

            byte[] bytes = Encoding.ASCII.GetBytes(response.ToString());
            await stream.WriteAsync(bytes, 0, bytes.Length, token).ConfigureAwait(false);
            await stream.FlushAsync(token).ConfigureAwait(false);

            using (WebSocket socket = WebSocket.CreateFromStream(stream, true, null, TimeSpan.FromSeconds(30)))
            {
                await _hub.RunSessionAsync(socket, token).ConfigureAwait(false);
            }
        }

        private async Task HandleHttpAsync(NetworkStream stream, ChromeBridgeHttpRequest head, CancellationToken token)
        {
            if (head.Method == "GET" && (head.Path == "/" || head.Path == "/index.html"))
            {
                await WriteTextAsync(stream, 200, "OK", "text/html; charset=utf-8", _page, token).ConfigureAwait(false);
                return;
            }

            if (head.Method == "GET" && head.Path == "/status")
            {
                string json = JsonSerializer.Serialize(new
                {
                    connected = _hub.IsPageConnected,
                    status = _hub.PageStatus,
                    port = Port,
                }, _jsonOptions);

                await WriteTextAsync(stream, 200, "OK", "application/json; charset=utf-8", json, token).ConfigureAwait(false);
                return;
            }

            if (head.Path == "/translate")
            {
                await HandleTranslateAsync(stream, head, token).ConfigureAwait(false);
                return;
            }

            await WriteTextAsync(stream, 404, "Not Found", "text/plain; charset=utf-8", "not found", token).ConfigureAwait(false);
        }

        private async Task HandleTranslateAsync(NetworkStream stream, ChromeBridgeHttpRequest head, CancellationToken token)
        {
            if (head.Method != "POST")
            {
                await WriteErrorAsync(stream, 405, "Method Not Allowed", "POST만 받습니다.", token).ConfigureAwait(false);
                return;
            }

            string? body = await ReadBodyAsync(stream, head, token).ConfigureAwait(false);

            if (body == null)
            {
                await WriteErrorAsync(stream, 413, "Payload Too Large", "요청 본문이 너무 큽니다.", token).ConfigureAwait(false);
                return;
            }

            string? text;
            string? source;
            string? target;
            string? mode;

            try
            {
                using (JsonDocument document = JsonDocument.Parse(body))
                {
                    JsonElement root = document.RootElement;
                    text = ReadFirstString(root, "text", "q", "OCR_TEXT", "content");
                    source = ReadFirstString(root, "source", "sourceLanguage", "from", "SOURCE_CODE");
                    target = ReadFirstString(root, "target", "targetLanguage", "to", "RESULT_CODE");
                    mode = ReadFirstString(root, "mode", "engine");
                }
            }
            catch (JsonException ex)
            {
                await WriteErrorAsync(stream, 400, "Bad Request", $"요청 본문이 JSON이 아닙니다 : {ex.Message}", token).ConfigureAwait(false);
                return;
            }

            if (text == null)
            {
                await WriteErrorAsync(stream, 400, "Bad Request", "번역할 text 값이 없습니다.", token).ConfigureAwait(false);
                return;
            }

            string normalizedSource = ChromeBridgeLanguage.Normalize(source);
            string normalizedTarget = ChromeBridgeLanguage.Normalize(target);

            if (normalizedTarget == ChromeBridgeLanguage.Auto)
            {
                normalizedTarget = "ko";
            }

            ChromeBridgeResult result = await _hub.TranslateAsync(text, normalizedSource, normalizedTarget,
                ChromeBridgeEngineMode.Normalize(mode), AdvencedOptionManager.ChromeBridgeTimeout, token).ConfigureAwait(false);

            if (!result.IsSuccess)
            {
                await WriteErrorAsync(stream, 503, "Service Unavailable", result.Error, token).ConfigureAwait(false);
                return;
            }

            string payload = JsonSerializer.Serialize(new
            {
                result = result.Text,
                engine = result.Engine,
            }, _jsonOptions);

            await WriteTextAsync(stream, 200, "OK", "application/json; charset=utf-8", payload, token).ConfigureAwait(false);
        }

        private static async Task<ChromeBridgeHttpRequest?> ReadHeadAsync(Stream stream, CancellationToken token)
        {
            byte[] buffer = new byte[MaxHeadBytes];
            int used = 0;
            int end = -1;

            while (used < buffer.Length)
            {
                int read = await stream.ReadAsync(buffer.AsMemory(used, buffer.Length - used), token).ConfigureAwait(false);

                if (read <= 0)
                {
                    break;
                }

                int searchFrom = used > 3 ? used - 3 : 0;
                used += read;
                end = FindHeadEnd(buffer, searchFrom, used);

                if (end >= 0)
                {
                    break;
                }
            }

            if (end < 0)
            {
                return null;
            }

            string headText = Encoding.UTF8.GetString(buffer, 0, end - 4);
            byte[] leftover = new byte[used - end];
            Array.Copy(buffer, end, leftover, 0, leftover.Length);

            return ChromeBridgeHttpRequest.Parse(headText, leftover);
        }

        private static int FindHeadEnd(byte[] buffer, int from, int used)
        {
            for (int i = from; i + 3 < used; i++)
            {
                if (buffer[i] == (byte)'\r' && buffer[i + 1] == (byte)'\n' && buffer[i + 2] == (byte)'\r' && buffer[i + 3] == (byte)'\n')
                {
                    return i + 4;
                }
            }

            return -1;
        }

        private static async Task<string?> ReadBodyAsync(Stream stream, ChromeBridgeHttpRequest head, CancellationToken token)
        {
            int length = head.ContentLength;

            if (length <= 0)
            {
                return "{}";
            }

            if (length > MaxBodyBytes)
            {
                return null;
            }

            byte[] body = new byte[length];
            int copied = Math.Min(head.Leftover.Length, length);
            Array.Copy(head.Leftover, body, copied);

            while (copied < length)
            {
                int read = await stream.ReadAsync(body.AsMemory(copied, length - copied), token).ConfigureAwait(false);

                if (read <= 0)
                {
                    break;
                }

                copied += read;
            }

            return Encoding.UTF8.GetString(body, 0, copied);
        }

        private static string? ReadFirstString(JsonElement root, params string[] names)
        {
            foreach (string name in names)
            {
                JsonElement value;

                if (root.TryGetProperty(name, out value) && value.ValueKind == JsonValueKind.String)
                {
                    return value.GetString();
                }
            }

            return null;
        }

        private static async Task WriteErrorAsync(Stream stream, int status, string reason, string message, CancellationToken token)
        {
            string json = JsonSerializer.Serialize(new { error = message }, _jsonOptions);
            await WriteTextAsync(stream, status, reason, "application/json; charset=utf-8", json, token).ConfigureAwait(false);
        }

        private static async Task WriteTextAsync(Stream stream, int status, string reason, string contentType, string body, CancellationToken token)
        {
            byte[] payload = Encoding.UTF8.GetBytes(body ?? "");

            StringBuilder head = new StringBuilder();
            head.Append("HTTP/1.1 ").Append(status.ToString(CultureInfo.InvariantCulture)).Append(' ').Append(reason).Append("\r\n");
            head.Append("Content-Type: ").Append(contentType).Append("\r\n");
            head.Append("Content-Length: ").Append(payload.Length.ToString(CultureInfo.InvariantCulture)).Append("\r\n");
            head.Append("Cache-Control: no-store\r\n");
            head.Append("Connection: close\r\n");
            head.Append("\r\n");

            byte[] headBytes = Encoding.ASCII.GetBytes(head.ToString());

            await stream.WriteAsync(headBytes, 0, headBytes.Length, token).ConfigureAwait(false);
            await stream.WriteAsync(payload, 0, payload.Length, token).ConfigureAwait(false);
            await stream.FlushAsync(token).ConfigureAwait(false);
        }

        private static string ComputeAcceptKey(string key)
        {
            byte[] hash = SHA1.HashData(Encoding.ASCII.GetBytes(key + WebSocketGuid));
            return Convert.ToBase64String(hash);
        }

        private static string LoadEmbeddedPage()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream? stream = assembly.GetManifestResourceStream(PageResourceName))
            {
                if (stream == null)
                {
                    return "<!doctype html><meta charset=\"utf-8\"><p>브릿지 페이지 리소스를 찾지 못했습니다.</p>";
                }

                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
