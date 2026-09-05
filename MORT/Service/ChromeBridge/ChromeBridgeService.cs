using System;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.Service.ChromeBridge
{
    /// <summary>
    /// 크롬 번역기의 바깥 창구. 로컬 서버와 크롬 창의 수명을 여기서 관리한다.
    ///
    /// 별도 실행 파일 없이 MORT 프로세스 안에서 서버를 띄운다. 그래서 MORT가 꺼지면 서버도 같이 꺼진다.
    /// 크롬 창은 MORT가 죽여도 남지만, 서버가 없으면 그 창은 아무것도 하지 못한다.
    /// </summary>
    public class ChromeBridgeService
    {
        public const int DefaultPort = 16899;
        public const string DefaultMode = ChromeBridgeEngineMode.Auto;
        public const int DefaultTimeout = 30;


        /// <summary>창을 띄운 뒤 연결될 때까지 기다려 주는 시간. 이 사이의 요청은 무시한다.</summary>
        private static readonly TimeSpan LaunchCooldown = TimeSpan.FromSeconds(12);

        private readonly ChromeBridgeHub _hub = new ChromeBridgeHub();
        private readonly ChromeBridgeServer _server;
        private readonly object _openGate = new object();

        private DateTime _lastLaunchUtc = DateTime.MinValue;

        /// <summary>이 서버가 도는 동안 크롬 창이 한 번이라도 붙은 적이 있는지.</summary>
        private bool _pageConnectedBefore;

        public ChromeBridgeService()
        {
            _server = new ChromeBridgeServer(_hub);
            _hub.ConnectionChanged += OnConnectionChanged;
        }

        private void OnConnectionChanged()
        {
            if (_hub.IsPageConnected)
            {
                _pageConnectedBefore = true;
            }
        }

        public bool IsServerRunning => _server.IsRunning;
        public bool IsPageConnected => _hub.IsPageConnected;
        public string PageUrl => _server.PageUrl;

        /// <summary>크롬 창이 "쓸 모델이 없다"고 알려 온 상태. 내려받기는 그 창에서만 시작할 수 있다.</summary>
        public bool NeedsModel => _hub.PageStatus == "need-model";

        /// <summary>크롬 창이 붙거나 끊길 때 알린다. 서버 스레드에서 올라온다.</summary>
        public event Action ConnectionChanged
        {
            add { _hub.ConnectionChanged += value; }
            remove { _hub.ConnectionChanged -= value; }
        }

        /// <summary>
        /// 번역을 시작할 때 부른다. 서버를 올리고 크롬 창을 준비한다.
        /// </summary>
        public bool Prepare(int port, bool openBrowser, out string error)
        {
            if (!_server.Start(port, out error))
            {
                return false;
            }

            if (openBrowser)
            {
                OpenPage(false);
            }

            return true;
        }

        /// <summary>
        /// 브릿지 창을 확보한다. 창은 항상 하나만 둔다.
        ///
        /// 이미 붙어 있으면 새로 띄우지 않는다. 크롬은 같은 주소를 --app 으로 다시 실행하면 창을
        /// 하나 더 띄우고, 그러면 두 창이 서로 밀어내다가 한쪽이 죽은 채로 남는다.
        ///
        /// 아직 안 붙었더라도 방금 띄웠으면 기다린다. 창이 떠서 연결될 때까지 몇 초 걸리는데
        /// 그 사이에 번역 시작과 [설정 열기]가 겹치면 창이 두 개가 된다.
        /// </summary>
        /// <param name="requestedByUser">
        /// 사용자가 버튼으로 부른 경우에만 true.
        ///
        /// 이 값이 창을 앞으로 꺼낼지를 가른다. 번역 시작은 매번 이 함수를 지나가는데, 그때마다
        /// 창을 앞으로 꺼내면 게임에서 포커스를 빼앗는다. 화면을 번역하는 도구가 할 짓이 아니다.
        /// 자동 경로는 창이 살아 있는지만 확인하고 조용히 지나간다.
        /// </param>
        public void OpenPage(bool requestedByUser = true)
        {
            if (!_server.IsRunning)
            {
                string error;

                if (!_server.Start(AdvencedOptionManager.ChromeBridgePort, out error))
                {
                    Util.ShowLog($"[ChromeBridge] {error}");
                    return;
                }
            }

            lock (_openGate)
            {
                if (_hub.IsPageConnected)
                {
                    if (!requestedByUser)
                    {
                        //창이 이미 붙어 있다. 띄울 것도 앞으로 꺼낼 것도 없다.
                        return;
                    }

                    if (ChromeBridgeWindow.TryFocus())
                    {
                        return;
                    }

                    Util.ShowLog("[ChromeBridge] 연결된 브릿지 창을 찾지 못해 새로 띄웁니다.");
                }
                else if (!requestedByUser && _pageConnectedBefore)
                {
                    //붙었다가 끊긴 것이니 사용자가 창을 닫은 것이다. 번역할 때마다 다시 띄우면
                    //게임 위로 창이 계속 올라온다. 다시 쓰려면 [설정 열기]를 누르게 둔다.
                    return;
                }
                else if (DateTime.UtcNow - _lastLaunchUtc < LaunchCooldown)
                {
                    Util.ShowLog("[ChromeBridge] 브릿지 창을 여는 중입니다. 잠시 기다려 주세요.");
                    return;
                }

                _lastLaunchUtc = DateTime.UtcNow;
            }

            //주소창 없는 앱 창으로 띄운다. 사용자가 다른 주소로 옮겨 가 연결이 끊기는 일을 줄인다.
            ChromeLauncher.Launch(_server.PageUrl, true);

            if (!requestedByUser)
            {
                //사용자가 부른 게 아니면 띄우자마자 최소화한다. 이 창은 볼 일이 거의 없고,
                //번역을 시작할 때마다 앞에 뜨면 게임에서 포커스를 가져간다.
                //창이 뜰 때까지 기다려야 해서 번역 시작을 붙잡지 않도록 뒤에서 처리한다.
                Task.Run(() => ChromeBridgeWindow.MinimizeAfterLaunch());
            }
        }

        /// <summary>쓸 모델이 없다고 크롬 창이 알려 올 때. 사유 문자열을 함께 준다.</summary>
        public event Action<string> ModelMissing
        {
            add { _hub.ModelMissing += value; }
            remove { _hub.ModelMissing -= value; }
        }

        /// <summary>MORT에 설정된 번역 언어와 엔진을 크롬 창에 알린다.</summary>
        public void SetConfig(string source, string target, string mode)
        {
            _hub.SetConfig(source, target, mode);
        }

        /// <summary>브릿지 창을 앞으로 꺼낸다. 사용자가 그쪽에서 뭔가 해야 할 때 부른다.</summary>
        public void FocusPage()
        {
            ChromeBridgeWindow.TryFocus();
        }

        public void Stop()
        {
            //서버가 사라지면 브릿지 창은 아무것도 못 하고 재접속만 되풀이한다. 같이 닫는다.
            ChromeBridgeWindow.TryClose();

            _server.Stop();

            lock (_openGate)
            {
                _pageConnectedBefore = false;
                _lastLaunchUtc = DateTime.MinValue;
            }
        }

        public Task<ChromeBridgeResult> TranslateAsync(string text, string source, string target, string mode,
            int timeoutSeconds, CancellationToken token)
        {
            return _hub.TranslateAsync(text, source, target, mode, timeoutSeconds, token);
        }
    }
}
