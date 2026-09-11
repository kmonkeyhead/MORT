using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.Service.ChromeBridge
{
    public readonly struct ChromeBridgeResult
    {
        public bool IsSuccess { get; }
        public string Text { get; }
        public string Engine { get; }
        public string Error { get; }

        private ChromeBridgeResult(bool isSuccess, string text, string engine, string error)
        {
            IsSuccess = isSuccess;
            Text = text;
            Engine = engine;
            Error = error;
        }

        public static ChromeBridgeResult Success(string text, string engine)
        {
            return new ChromeBridgeResult(true, text, engine, "");
        }

        public static ChromeBridgeResult Failure(string error)
        {
            return new ChromeBridgeResult(false, "", "", error);
        }
    }

    /// <summary>
    /// 브릿지 페이지(크롬 탭) 한 개와의 연결을 들고 있으면서 번역 요청을 그 페이지로 넘기고 결과를 기다린다.
    ///
    /// 크롬 내장 AI는 Web Worker에서 쓸 수 없고 모델 첫 내려받기는 create() 직전에 사용자 조작이 있어야 한다.
    /// 그래서 MORT가 모델을 직접 부를 수 없고, 살아 있는 크롬 문서를 반드시 거쳐야 한다.
    /// </summary>
    public class ChromeBridgeHub
    {
        /// <summary>
        /// 자리를 내준 창이 재접속을 멈추도록 알리는 종료 사유. bridge.html 의 CLOSE_REASON_REPLACED 와 같아야 한다.
        /// 양쪽 다 재접속하면 3초마다 서로 밀어내며 그 사이 요청이 전부 실패한다.
        /// </summary>
        public const string ReplacedCloseReason = "another tab connected";

        private const int MaxMessageBytes = 4 * 1024 * 1024;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        private readonly ConcurrentDictionary<long, TaskCompletionSource<ChromeBridgeResult>> _pending
            = new ConcurrentDictionary<long, TaskCompletionSource<ChromeBridgeResult>>();
        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);

        private WebSocket? _socket;
        private long _nextId;
        private string _pageStatus = "";

        //창이 닫힌 채로 번역이 돌면 OCR 회차마다 같은 실패가 난다. 그걸 전부 적으면 로그가 그 말로만 찬다.
        //끊긴 뒤 첫 번째 요청에만 남기고, 다시 붙으면 이 표시를 푼다.
        private int _loggedDisconnected;

        //MORT에 설정된 번역 언어. 페이지가 붙을 때와 설정이 바뀔 때 알려 준다.
        //이걸 안 보내면 페이지는 첫 번역이 올 때까지 어떤 언어 쌍을 쓸지 몰라서
        //모델 준비도 상태 표시도 엉뚱한 쌍(시험 칸 기본값)을 기준으로 하게 된다.
        private string _configSource = "";
        private string _configTarget = "";
        private string _configMode = "";

        /// <summary>
        /// 연결이 붙거나 끊길 때 알린다. 서버 스레드에서 올라오므로 UI에서 받을 때는 받는 쪽이 스레드를 맞춰야 한다.
        /// 크롬 창이 붙은 걸 MORT 패널이 바로 반영하지 못하면 패널을 볼 이유가 없어진다.
        /// </summary>
        public event Action? ConnectionChanged;

        /// <summary>
        /// 크롬 창이 "쓸 모델이 없다"고 알려 올 때. 사유 문자열을 함께 준다.
        /// 모델 없이 계속 돌리면 auto 모드가 조용히 LLM으로 떨어져 품질이 나쁜 결과가 쌓이므로
        /// MORT는 번역을 멈추고 사용자에게 알린다.
        /// </summary>
        public event Action<string>? ModelMissing;

        public bool IsPageConnected
        {
            get
            {
                WebSocket? socket = _socket;
                return socket != null && socket.State == WebSocketState.Open;
            }
        }

        public string PageStatus => _pageStatus;

        /// <summary>
        /// MORT에 설정된 번역 언어와 엔진을 페이지에 알린다. 설정 적용 때와 페이지가 붙을 때 부른다.
        /// </summary>
        public void SetConfig(string source, string target, string mode)
        {
            _configSource = source ?? "";
            _configTarget = target ?? "";
            _configMode = mode ?? "";

            _ = SendConfigAsync();
        }

        /// <summary>
        /// 크롬이 받아 둔 모델 목록을 페이지에 보낸다. 페이지는 파일을 볼 수 없어서 MORT가 대신 읽어 준다.
        /// 디스크를 훑으므로 붙을 때와 사용자가 새로 고칠 때만 보낸다.
        /// </summary>
        public async Task SendModelsAsync()
        {
            WebSocket? socket = _socket;

            if (socket == null || socket.State != WebSocketState.Open)
            {
                return;
            }

            try
            {
                ChromeModelSnapshot snapshot = await Task.Run(() => ChromeModelInfo.Read()).ConfigureAwait(false);

                string payload = JsonSerializer.Serialize(new
                {
                    type = "models",
                    llm = snapshot.Llm == null ? null : new
                    {
                        name = snapshot.Llm.Name,
                        version = snapshot.Llm.Version,
                        size = ChromeModelInfo.ToSizeText(snapshot.Llm.Bytes),
                        //LLM 모델이 실제로 어디 있는지. 페이지는 웹페이지라 경로를 알 길이 없고,
                        //지우려면 폴더를 찾아가야 하는데 크롬 설정에는 그 위치가 안 적혀 있다.
                        path = snapshot.Llm.Path,
                    },
                    packs = snapshot.Packs.Select(p => new
                    {
                        name = p.Name,
                        version = p.Version,
                        size = ChromeModelInfo.ToSizeText(p.Bytes),
                        path = p.Path,
                    }).ToList(),
                    packRoot = snapshot.PackRoot,
                }, _jsonOptions);

                await SendAsync(socket, payload, CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 모델 목록을 보내지 못했습니다 : {ex.Message}");
            }
        }

        private async Task SendConfigAsync()
        {
            WebSocket? socket = _socket;

            if (socket == null || socket.State != WebSocketState.Open || string.IsNullOrEmpty(_configTarget))
            {
                return;
            }

            try
            {
                string payload = JsonSerializer.Serialize(new
                {
                    type = "config",
                    source = _configSource,
                    target = _configTarget,
                    mode = _configMode,
                }, _jsonOptions);

                await SendAsync(socket, payload, CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 설정을 페이지에 알리지 못했습니다 : {ex.Message}");
            }
        }

        private void RaiseModelMissing(string detail)
        {
            try
            {
                ModelMissing?.Invoke(detail);
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 모델 없음 알림 실패 : {ex.Message}");
            }
        }

        private void RaiseConnectionChanged()
        {
            try
            {
                ConnectionChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 상태 알림 실패 : {ex.Message}");
            }
        }

        public async Task RunSessionAsync(WebSocket socket, CancellationToken token)
        {
            WebSocket? previous = Interlocked.Exchange(ref _socket, socket);

            if (previous != null)
            {
                Util.ShowLog("[ChromeBridge] 이전 브릿지 페이지 연결을 닫습니다. 창은 하나만 열어 두세요.");
                await CloseQuietlyAsync(previous, ReplacedCloseReason).ConfigureAwait(false);
            }

            _pageStatus = "connected";
            Interlocked.Exchange(ref _loggedDisconnected, 0);
            Util.ShowLog("[ChromeBridge] 브릿지 페이지가 연결되었습니다.");
            RaiseConnectionChanged();

            //붙자마자 지금 설정을 알려 준다. 첫 번역을 기다리지 않고 바로 맞는 언어 쌍을 쓰게 한다.
            await SendConfigAsync().ConfigureAwait(false);
            await SendModelsAsync().ConfigureAwait(false);

            try
            {
                await ReceiveLoopAsync(socket, token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
            catch (WebSocketException ex)
            {
                Util.ShowLog($"[ChromeBridge] 연결이 끊어졌습니다 : {ex.Message}");
            }
            finally
            {
                Interlocked.CompareExchange(ref _socket, null, socket);

                if (!IsPageConnected)
                {
                    _pageStatus = "";
                    FailAllPending("브릿지 페이지 연결이 끊어졌습니다.");
                    Util.ShowLog("[ChromeBridge] 브릿지 페이지 연결이 종료되었습니다.");
                    RaiseConnectionChanged();
                }

                socket.Dispose();
            }
        }

        public async Task<ChromeBridgeResult> TranslateAsync(string text, string source, string target, string mode,
            int timeoutSeconds, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return ChromeBridgeResult.Success("", "empty");
            }

            WebSocket? socket = _socket;

            if (socket == null || socket.State != WebSocketState.Open)
            {
                if (Interlocked.Exchange(ref _loggedDisconnected, 1) == 0)
                {
                    Util.ShowLog("[ChromeBridge] 창이 연결되어 있지 않아 번역 요청을 처리하지 못했습니다."
                        + " 번역 설정의 [설정 열기]로 창을 여세요.");
                }

                return ChromeBridgeResult.Failure("크롬 브릿지 창이 연결되어 있지 않습니다. 번역 설정의 [설정 열기]로 창을 여세요.");
            }

            long id = Interlocked.Increment(ref _nextId);
            TaskCompletionSource<ChromeBridgeResult> completion
                = new TaskCompletionSource<ChromeBridgeResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            _pending[id] = completion;

            try
            {
                string payload = JsonSerializer.Serialize(new
                {
                    type = "translate",
                    id = id,
                    text = text,
                    source = source,
                    target = target,
                    mode = mode,
                }, _jsonOptions);

                await SendAsync(socket, payload, token).ConfigureAwait(false);

                using (CancellationTokenSource timeout = CancellationTokenSource.CreateLinkedTokenSource(token))
                {
                    Task delay = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds), timeout.Token);
                    Task finished = await Task.WhenAny(completion.Task, delay).ConfigureAwait(false);

                    timeout.Cancel();

                    if (finished != completion.Task)
                    {
                        //제한 시간을 넘긴 건 화면에만 뜨고 로그에는 안 남았다. 나중에 왜 끊겼는지 볼 수가 없다.
                        Util.ShowLog($"[ChromeBridge] 번역 응답 없음 : {source} → {target} / {mode} / {text.Length}자 "
                            + $"/ {timeoutSeconds.ToString(CultureInfo.InvariantCulture)}초 초과");

                        return ChromeBridgeResult.Failure(
                            $"크롬 브릿지가 {timeoutSeconds.ToString(CultureInfo.InvariantCulture)}초 안에 응답하지 않았습니다. 모델을 내려받는 중이거나 창이 절전 상태일 수 있습니다.");
                    }

                    return await completion.Task.ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                return ChromeBridgeResult.Failure(ex.Message);
            }
            finally
            {
                TaskCompletionSource<ChromeBridgeResult>? removed;
                _pending.TryRemove(id, out removed);
            }
        }

        private async Task ReceiveLoopAsync(WebSocket socket, CancellationToken token)
        {
            byte[] buffer = new byte[16 * 1024];

            while (!token.IsCancellationRequested && socket.State == WebSocketState.Open)
            {
                using (MemoryStream message = new MemoryStream())
                {
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), token).ConfigureAwait(false);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await CloseQuietlyAsync(socket, "bye").ConfigureAwait(false);
                            return;
                        }

                        if (message.Length + result.Count > MaxMessageBytes)
                        {
                            Util.ShowLog("[ChromeBridge] 페이지가 보낸 메시지가 너무 커서 버립니다.");
                            return;
                        }

                        message.Write(buffer, 0, result.Count);
                    }
                    while (!result.EndOfMessage);

                    HandleMessage(Encoding.UTF8.GetString(message.GetBuffer(), 0, (int)message.Length));
                }
            }
        }

        private void HandleMessage(string json)
        {
            try
            {
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    JsonElement root = document.RootElement;
                    string type = GetString(root, "type") ?? "";

                    switch (type)
                    {
                        case "ready":
                        {
                            string previous = _pageStatus;
                            _pageStatus = GetString(root, "status") ?? "ready";
                            string detail = GetString(root, "detail") ?? "";
                            Util.ShowLog($"[ChromeBridge] 페이지 상태 : {_pageStatus} / {detail}");
                            //모델 없음 같은 상태도 패널에 바로 비쳐야 한다.
                            RaiseConnectionChanged();

                            //같은 상태가 이어지는 동안에는 한 번만 알린다. 요청마다 팝업이 뜨면 못 쓴다.
                            if (_pageStatus == "need-model" && previous != "need-model")
                            {
                                RaiseModelMissing(detail);
                            }

                            break;
                        }

                        case "log":
                        {
                            Util.ShowLog($"[ChromeBridge][page] {GetString(root, "message")}");
                            break;
                        }

                        case "refresh-models":
                        {
                            //모델을 받고 나면 목록이 달라진다. 페이지가 요청할 때 다시 읽어 보낸다.
                            _ = SendModelsAsync();
                            break;
                        }

                        case "result":
                        {
                            Complete(GetInt64(root, "id"),
                                ChromeBridgeResult.Success(GetString(root, "text") ?? "", GetString(root, "engine") ?? "unknown"));
                            break;
                        }

                        case "error":
                        {
                            long id = GetInt64(root, "id");
                            string message = GetString(root, "message") ?? "크롬 브릿지에서 알 수 없는 오류가 발생했습니다.";

                            if (id > 0)
                            {
                                //로그는 페이지가 언어 쌍까지 붙여 log 메시지로 따로 보낸다. 여기서 또 적으면 두 줄이 된다.
                                Complete(id, ChromeBridgeResult.Failure(message));
                            }
                            else
                            {
                                Util.ShowLog($"[ChromeBridge][page] 오류 : {message}");
                            }

                            break;
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                Util.ShowLog($"[ChromeBridge] 페이지 메시지를 해석하지 못했습니다 : {ex.Message}");
            }
        }

        private void Complete(long id, ChromeBridgeResult result)
        {
            TaskCompletionSource<ChromeBridgeResult>? completion;

            if (_pending.TryRemove(id, out completion))
            {
                completion.TrySetResult(result);
            }
        }

        private void FailAllPending(string reason)
        {
            foreach (long id in _pending.Keys)
            {
                Complete(id, ChromeBridgeResult.Failure(reason));
            }
        }

        private async Task SendAsync(WebSocket socket, string payload, CancellationToken token)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(payload);

            await _sendLock.WaitAsync(token).ConfigureAwait(false);

            try
            {
                await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, token).ConfigureAwait(false);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private static async Task CloseQuietlyAsync(WebSocket socket, string reason)
        {
            try
            {
                if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
                {
                    await socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, reason, CancellationToken.None).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
            }
        }

        private static string? GetString(JsonElement root, string name)
        {
            JsonElement value;

            if (root.TryGetProperty(name, out value) && value.ValueKind == JsonValueKind.String)
            {
                return value.GetString();
            }

            return null;
        }

        private static long GetInt64(JsonElement root, string name)
        {
            JsonElement value;

            if (!root.TryGetProperty(name, out value))
            {
                return 0;
            }

            if (value.ValueKind == JsonValueKind.Number)
            {
                long number;

                if (value.TryGetInt64(out number))
                {
                    return number;
                }
            }

            return 0;
        }
    }
}
