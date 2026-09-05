using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace MORT.Service.ChromeBridge
{
    /// <summary>
    /// 브릿지 페이지는 반드시 크롬에서 열려야 한다. 기본 브라우저가 크롬이 아닌 사용자가 많아서
    /// 실행 파일 경로를 직접 찾고, 못 찾을 때만 기본 브라우저로 넘긴다.
    /// </summary>
    public static class ChromeLauncher
    {
        public static void Launch(string url, bool useAppWindow)
        {
            string? chromePath = FindChrome();

            if(chromePath == null)
            {
                Util.ShowLog("[ChromeBridge] 크롬 실행 파일을 찾지 못했습니다. 기본 브라우저로 엽니다. 크롬이 아니면 내장 AI를 쓸 수 없습니다.");

                try
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                catch(Exception ex)
                {
                    Util.ShowLog("[ChromeBridge] 브라우저를 열지 못했습니다 : " + ex.Message);
                }

                return;
            }

            // --app 은 주소창 없는 창으로 띄운다. 사용자가 실수로 다른 주소로 이동해 연결이 끊기는 걸 줄인다.
            string arguments = useAppWindow ? "--app=" + url : url;

            try
            {
                Process.Start(new ProcessStartInfo(chromePath, arguments) { UseShellExecute = false });
                Util.ShowLog("[ChromeBridge] 크롬을 열었습니다 : " + url);
            }
            catch(Exception ex)
            {
                Util.ShowLog("[ChromeBridge] 크롬을 실행하지 못했습니다 : " + ex.Message);
            }
        }

        private static string? FindChrome()
        {
            string? fromRegistry = ReadAppPath(Registry.CurrentUser) ?? ReadAppPath(Registry.LocalMachine);

            if(fromRegistry != null)
            {
                return fromRegistry;
            }

            List<string> candidates = new List<string>
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google", "Chrome", "Application", "chrome.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Google", "Chrome", "Application", "chrome.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google", "Chrome", "Application", "chrome.exe"),
            };

            foreach(string candidate in candidates)
            {
                if(File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static string? ReadAppPath(RegistryKey root)
        {
            try
            {
                using(RegistryKey? key = root.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"))
                {
                    if(key == null)
                    {
                        return null;
                    }

                    string? path = key.GetValue(null) as string;

                    if(string.IsNullOrWhiteSpace(path))
                    {
                        return null;
                    }

                    path = path.Trim('"');

                    return File.Exists(path) ? path : null;
                }
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
