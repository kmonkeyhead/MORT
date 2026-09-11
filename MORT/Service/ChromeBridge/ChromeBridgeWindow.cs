using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;

namespace MORT.Service.ChromeBridge
{
    /// <summary>
    /// 이미 떠 있는 브릿지 창을 찾아 앞으로 꺼낸다.
    ///
    /// 크롬은 같은 주소를 --app 으로 다시 실행하면 창을 하나 더 띄운다. 창이 둘이면 서로 밀어내다가
    /// 한쪽이 죽은 채로 화면에 남으므로, 새로 띄우기 전에 있는 창을 먼저 찾아 쓴다.
    /// 창 제목은 bridge.html 의 title 이 그대로 올라온다.
    /// </summary>
    internal static class ChromeBridgeWindow
    {
        /// <summary>
        /// bridge.html 이 다는 제목. 페이지가 MORT의 UI 언어에 맞춰 제목을 바꾸므로 두 벌을 다 본다.
        /// 창을 찾는 유일한 실마리가 제목이라, 페이지의 STRINGS.*.appTitle 을 고치면 여기도 같이 고쳐야 한다.
        /// </summary>
        private static readonly string[] Titles = { "MORT 크롬 브릿지", "MORT Chrome Bridge" };

        private const int SW_RESTORE = 9;
        private const int SW_MINIMIZE = 6;
        private const uint WM_CLOSE = 0x0010;

        private delegate bool EnumWindowsProc(IntPtr handle, IntPtr param);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr param);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr handle, out uint processId);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr handle);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr handle, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int command);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr handle, uint message, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 브릿지 창이 뜨면 최소화한다.
        ///
        /// 이 창은 사람이 볼 일이 거의 없다. 번역을 시작할 때마다 앞에 떠서 게임에서 포커스를
        /// 가져가면 쓸 수 없고, 전체 화면 게임이면 그것만으로 창 모드로 튕기기도 한다.
        ///
        /// 최소화해도 번역은 그대로 된다. 최소화 3분 뒤에도 4ms 로 응답하는 것을 확인했다.
        /// 크롬의 백그라운드 지연은 타이머를 늦추는 것이고, 번역은 WebSocket 수신으로 깨어나
        /// 네이티브 호출을 하므로 해당되지 않는다.
        ///
        /// 완전히 숨기지(SW_HIDE) 않는 이유는 되찾을 방법을 남겨 두기 위해서다. 숨긴 창은 작업
        /// 표시줄에도 없어서, MORT가 비정상 종료하면 사용자가 그 창을 닫을 길이 없다.
        /// </summary>
        /// <summary>
        /// 최소화를 취소한다. 창이 뜨자마자 "모델을 받으시겠습니까?"를 물어야 하는 경우가 있는데,
        /// 그때 예약된 최소화가 그대로 걸리면 사용자가 눌러야 할 대화상자를 작업 표시줄로 내려 버린다.
        /// 물어볼 일이 생기는 것은 창이 뜬 직후라 이 경합이 실제로 난다.
        /// </summary>
        private static int _cancelMinimize;

        public static void KeepVisible()
        {
            Interlocked.Exchange(ref _cancelMinimize, 1);
            TryFocus();
        }

        public static void MinimizeAfterLaunch()
        {
            try
            {
                Interlocked.Exchange(ref _cancelMinimize, 0);
                IntPtr handle = IntPtr.Zero;

                //창이 실제로 뜰 때까지 기다린다. 뜨기 전에 최소화를 걸면 아무 일도 일어나지 않는다.
                for (int i = 0; i < 40; i++)
                {
                    if (Interlocked.CompareExchange(ref _cancelMinimize, 0, 0) == 1)
                    {
                        return;
                    }

                    handle = Find();

                    if (handle != IntPtr.Zero)
                    {
                        break;
                    }

                    Thread.Sleep(150);
                }

                if (handle == IntPtr.Zero)
                {
                    return;
                }

                //창이 자리를 잡을 짬을 준다. 곧바로 최소화하면 크롬이 다시 펼치는 경우가 있다.
                Thread.Sleep(400);

                if (Interlocked.CompareExchange(ref _cancelMinimize, 0, 0) == 1)
                {
                    Util.ShowLog("[ChromeBridge] 브릿지 창에서 확인할 것이 있어 최소화하지 않습니다.");
                    return;
                }

                ShowWindow(handle, SW_MINIMIZE);
                Util.ShowLog("[ChromeBridge] 브릿지 창을 최소화했습니다. 볼 일이 있으면 [설정 열기]로 꺼냅니다.");
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 브릿지 창을 최소화하지 못했습니다 : {ex.Message}");
            }
        }

        /// <summary>
        /// 브릿지 창을 닫는다. MORT가 꺼지면 서버가 사라져 그 창은 아무것도 못 하므로 같이 정리한다.
        ///
        /// 제목으로 찾은 창에만 WM_CLOSE 를 보낸다. 크롬 프로세스를 죽이면 사용자가 보던 다른 탭까지
        /// 날아가므로 절대 그렇게 하지 않는다.
        /// </summary>
        public static void TryClose()
        {
            IntPtr handle = Find();

            if (handle == IntPtr.Zero)
            {
                return;
            }

            try
            {
                PostMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                Util.ShowLog("[ChromeBridge] 브릿지 창을 닫았습니다.");
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 브릿지 창을 닫지 못했습니다 : {ex.Message}");
            }
        }

        /// <summary>
        /// 브릿지 창을 찾으면 앞으로 꺼내고 true를 준다.
        ///
        /// 반환값은 "찾았는가"지 "앞으로 꺼냈는가"가 아니다. SetForegroundWindow 는 부르는 쪽이
        /// 포그라운드가 아니면 윈도우가 거부한다. 그걸 실패로 보면 창이 멀쩡히 있는데도
        /// 호출부가 새 창을 띄우게 되고, 결국 중복 창이 생긴다. 앞으로 꺼내는 건 부가 기능이다.
        /// </summary>
        public static bool TryFocus()
        {
            IntPtr handle = Find();

            if (handle == IntPtr.Zero)
            {
                return false;
            }

            try
            {
                if (IsIconic(handle))
                {
                    ShowWindow(handle, SW_RESTORE);
                }

                if (!SetForegroundWindow(handle))
                {
                    Util.ShowLog("[ChromeBridge] 브릿지 창이 이미 있습니다. 앞으로 꺼내지는 못했으니 직접 찾아 주세요.");
                }
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 브릿지 창을 앞으로 꺼내지 못했습니다 : {ex.Message}");
            }

            return true;
        }

        /// <summary>크롬은 제목 뒤에 " - Chrome" 을 붙이기도 해서 앞부분만 본다.</summary>
        private static bool IsBridgeTitle(string text)
        {
            foreach (string title in Titles)
            {
                if (text.StartsWith(title, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static IntPtr Find()
        {
            HashSet<uint> chromeProcessIds = GetChromeProcessIds();

            if (chromeProcessIds.Count == 0)
            {
                return IntPtr.Zero;
            }

            IntPtr found = IntPtr.Zero;

            try
            {
                EnumWindows((handle, param) =>
                {
                    if (!IsWindowVisible(handle))
                    {
                        return true;
                    }

                    uint processId;
                    GetWindowThreadProcessId(handle, out processId);

                    if (!chromeProcessIds.Contains(processId))
                    {
                        return true;
                    }

                    StringBuilder text = new StringBuilder(512);
                    GetWindowText(handle, text, text.Capacity);

                    if (!IsBridgeTitle(text.ToString()))
                    {
                        return true;
                    }

                    found = handle;
                    return false;
                }, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 창 목록을 훑지 못했습니다 : {ex.Message}");
                return IntPtr.Zero;
            }

            return found;
        }

        private static HashSet<uint> GetChromeProcessIds()
        {
            HashSet<uint> result = new HashSet<uint>();

            try
            {
                foreach (Process process in Process.GetProcessesByName("chrome"))
                {
                    result.Add((uint)process.Id);
                    process.Dispose();
                }
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 크롬 프로세스를 찾지 못했습니다 : {ex.Message}");
            }

            return result;
        }
    }
}
