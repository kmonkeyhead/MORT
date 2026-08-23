using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MORT.VersionCheck
{
    internal class VersionCheckLogic
    {
        //개발용 버전인가?
        public readonly bool IsDevVersion = false;
        private readonly IMainFormContract contract;

        //현재 버전
        private int nowVersion = 1171;

        public VersionCheckLogic(IMainFormContract contract)
        {
            this.contract = contract;
        }

        #region :::::::::::::::::::::::::::::::::::::::::::버전 확인 관련 :::::::::::::::::::::::::::::::::::::::::

        public bool GetCheckUpdate()
        {
            bool isCheckUpdate = true;


            //구버전 파일은 없앤다.
            if (File.Exists(GlobalDefine.CHECK_UPDATE_FILE))
            {
                File.Delete(GlobalDefine.CHECK_UPDATE_FILE);
            }

            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@USE_UPDATE ", '[', ']');


            if (!Boolean.TryParse(result, out isCheckUpdate))
            {
                isCheckUpdate = true;
            }


            return isCheckUpdate;
        }

        public void SetCheckUpdate(bool isUse)
        {
            Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@USE_UPDATE ", isUse.ToString());
        }

        #endregion

        //version.txt 등 외부에서 받은 URL은 평문 http로 올 수 있으므로 다운로드 전 https로 강제한다(MITM 방지)
        private static string ForceHttps(string url)
        {
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                return "https://" + url.Substring("http://".Length);
            }
            return url;
        }

        //업데이트 파일 옆 .sha256 다운로드. 실패/없음 -> 빈 문자열 반환(호출 측에서 "준비 안 됨"으로 해석).
        private static string TryDownloadHash(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string content = client.DownloadString(url);
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return "";
                    }
                    //파일 안에 해시만 있는 경우, sha256sum 호환 포맷("hash *filename") 모두 첫 토큰만 취한다
                    string first = content.Trim().Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    return first.Trim();
                }
            }
            catch (Exception e)
            {
                Util.ShowLog("TryDownloadHash failed: " + e.Message);
                return "";
            }
        }

        //쿨다운 마커의 키. Updater.exe도 동일 키/포맷(@KEY [value])으로 기록한다.
        private const string HASH_FAIL_TIME_KEY = "@HASH_FAIL_TIME ";

        //다운로드용 exe 옆의 MORT.dll.config를 받아 MORT_VERSION_VALUE를 읽고 version.txt 버전과 같은지 확인한다.
        //같으면 -> exe/config 배포가 끝난 것으로 보고 true. 받기/파싱 실패하거나 다르면 -> 아직 준비 안 됨으로 보고 false.
        private bool IsRemoteConfigVersionMatched(string fileUrl, string expectedVersion)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl) || string.IsNullOrEmpty(expectedVersion))
                {
                    return false;
                }

                //Updater가 받는 것과 동일 규칙: exe와 같은 위치의 MORT.dll.config. URL은 다운로드 직전 https로 강제(MITM 방지).
                string configUrl = ForceHttps(fileUrl).Replace("MORT.exe", "MORT.dll.config");

                using (WebClient client = new WebClient())
                {
                    string configContent = client.DownloadString(configUrl);
                    string configVersion = ParseConfigVersionValue(configContent);

                    Util.ShowLog("Remote config MORT_VERSION_VALUE : " + configVersion + " / version.txt : " + expectedVersion);

                    return int.TryParse(configVersion, out int remote)
                        && int.TryParse(expectedVersion, out int expected)
                        && remote == expected;
                }
            }
            catch (Exception e)
            {
                Util.ShowLog("IsRemoteConfigVersionMatched failed: " + e.Message);
                return false;
            }
        }

        //MORT.dll.config(XML)에서 <setting name="MORT_VERSION_VALUE"><value>1312</value></setting> 값을 읽는다. 실패 시 빈 문자열.
        private static string ParseConfigVersionValue(string configContent)
        {
            try
            {
                XDocument doc = XDocument.Parse(configContent);
                XElement setting = doc.Descendants("setting")
                    .FirstOrDefault(e => (string)e.Attribute("name") == "MORT_VERSION_VALUE");

                return setting?.Element("value")?.Value?.Trim() ?? "";
            }
            catch (Exception e)
            {
                Util.ShowLog("ParseConfigVersionValue failed: " + e.Message);
                return "";
            }
        }

        //업데이트 파일 SHA256 검증 실패 직후 10분간은 업데이트로 진입하지 않는다(손상/변조 파일 재다운로드 루프 방지).
        //쿨다운 마커는 Updater.exe가 @HASH_FAIL_TIME [ticks](UTC) 포맷으로 기록하고 여기서 읽는다.
        private static bool IsInHashFailCooldown()
        {
            try
            {
                string raw = Util.ParseStringFromFile(GlobalDefine.UPDATE_HASH_FAIL_COOLDOWN_FILE, HASH_FAIL_TIME_KEY);

                //마커 없음/형식 깨짐 -> 쿨다운 없음
                if (!long.TryParse(raw, out long ticks))
                {
                    return false;
                }

                DateTime failTime = new DateTime(ticks, DateTimeKind.Utc);
                TimeSpan elapsed = DateTime.UtcNow - failTime;

                if (elapsed >= TimeSpan.Zero && elapsed < TimeSpan.FromMinutes(10))
                {
                    Util.ShowLog($"Hash-fail cooldown active (about {10 - (int)elapsed.TotalMinutes} min left), skipping update");
                    return true;
                }

                //만료됐거나 미래 시각(시계 조작 등)이면 통과. 마커는 다음 실패 때 덮어써진다.
                return false;
            }
            catch (Exception e)
            {
                Util.ShowLog("IsInHashFailCooldown failed: " + e.Message);
                return false;
            }
        }

        //버전 확인.
        public void CheckVersion()
        {
            if (GetCheckUpdate())
            {
                try
                {
                    nowVersion = Properties.Settings.Default.MORT_VERSION_VALUE;
                    //http://kmonkeyhead.github.io/MORT_VERSION/version.txt
                    using(WebClient client = new WebClient())
                    {
                        Stream stream = client.OpenRead("https://kmonkeyhead.github.io/MORT_VERSION/version.txt");
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            String content = reader.ReadToEnd();
                            content = content.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                            Util.ShowLog("--- Version : " + content);

                            CheckMortVersion(content);

                            if (!Program.IS_FORCE_QUITE)
                            {
                                CheckDicVersion(content, "@MORT_DIC_ENG", @".\\DIC\\dic.txt");
                                CheckDicVersion(content, "@MORT_DIC_JPN", @".\\DIC\\dicJpn.txt");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Util.ShowLog(e.ToString());
                }
            }

        }

        public void CheckDefaultSetting()
        {
            try
            {
                //http://kmonkeyhead.github.io/MORT_VERSION/default_setting.txt
                using(WebClient client = new WebClient())
                {
                    Stream stream = client.OpenRead("https://kmonkeyhead.github.io/MORT_VERSION/default_setting.txt");
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        String content = reader.ReadToEnd();
                        content = content.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                        Util.ShowLog("--- default setting : " + content);

                        string naver = Util.ParseString(content, "@SPLITE_NAVER_TOKEN", '{', '}');
                        string google = Util.ParseString(content, "@SPLITE_GOOGLE_TOEKN ", '{', '}');
                        string isUseAdvence = Util.ParseString(content, "@ENABLE_ADVENCED_TOKEN", '{', '}');

                        string deeplUrl = Util.ParseString(content, "@DEEPL_URL", '{', '}');
                        string deeplFormat = Util.ParseString(content, "@DEEPL_FORMAT", '"', '"');
                        string deeplElementTarget = Util.ParseString(content, "@DEEPL_TARGET", '{', '}');
                        if (naver != "" && google != "")
                        {
                            Util.SetSpliteToken(naver, google, isUseAdvence);
                        }

                        if(deeplUrl != "" && deeplFormat != "")
                        {
                            GlobalDefine.DeeplFrontUrl = deeplUrl;
                            GlobalDefine.DeeplFormat = deeplFormat;
                            GlobalDefine.DeeplElementTarget = deeplElementTarget;

                            //TODO : 현재 {}로 하기 때문에 아래 문구를 넣을 수 없다, 방식을 바꿔야 한다
                            GlobalDefine.DeeplElementTarget = @"
    (function() {
        var mainDiv = document.getElementsByClassName('relative flex flex-1 flex-col')[0];
        if (!mainDiv) {
            return '';
        }

        var alternativesPanel = mainDiv.querySelector('[data-testid=""translator-target-result-alternatives-panel""]');
        if (alternativesPanel) {
            alternativesPanel.remove();
        }

        return mainDiv.innerText;
    })();
    ";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Util.ShowLog(e.ToString());
            }
        }

        /// <summary>
        /// MORT 버전 확인
        /// </summary>
        /// <param name="content"></param>
        private void CheckMortVersion(String content)
        {
            try
            {
                //직전 업데이트의 SHA256 검증이 실패했다면 일정 시간 동안은 버전이 달라도 업데이트로 진입하지 않는다.
                if (IsInHashFailCooldown())
                {
                    return;
                }

                if (content != null)
                {
                    string versionKey = "@MORT_VERSION ";
                    string minorKey = "@MORT_MINOR_VERSION ";
                    string bitUpdateKey = "@MORT_64_BIT_MESSSAGE ";
                    string newVersionString = "";
                    string minorVersionString = "";
                    string bitMessageString = "";
                    string downloadPage = "";

                    if (IsDevVersion)
                    {
                        versionKey = "@DEV_VERSION ";
                        minorKey = "@DEV_MINOR_VERSION ";
                    }

                    newVersionString = Util.ParseString(content, versionKey, '[', ']');
                    downloadPage = Util.ParseString(content, versionKey, '{', '}');
                    minorVersionString = Util.ParseString(content, minorKey, '[', ']');

                    //1.230까지만 쓴다
                    bitMessageString = Util.ParseString(content, bitUpdateKey, '[', ']');


                    UpdateType updateType = Util.GetUpdateType(nowVersion, newVersionString, minorVersionString);

                    Util.ShowLog("------------------" + System.Environment.NewLine + "Now : " + nowVersion + " / New : " + newVersionString + " / Minor : " + minorVersionString + " / Result : " + updateType.ToString());
                    
                    //1. 버전 비교를 한다.
                    if (updateType == UpdateType.Major)
                    {
                        string nowVersionString = nowVersion.ToString();
                        nowVersionString = nowVersionString.Insert(1, ".");
                        newVersionString = newVersionString.Insert(1, ".");

                        if (string.IsNullOrEmpty(bitMessageString) || nowVersion >= 1240)
                        {
                            bitMessageString = LocalizeString("New Update Message", true);
                        }

                        string checkMessageSubtitle = "(" + LocalizeManager.LocalizeManager.GetLocalizeString("Major Update") + nowVersionString + " -> " + newVersionString + ")";
                        if (DialogResult.OK == MessageBox.Show(bitMessageString, checkMessageSubtitle, MessageBoxButtons.OKCancel))
                        {
                            Logo.SetTopmost(false);
                            try
                            {
                                Logo.SetTopmost(true);
                                contract.ForceDisableTopMost();
                                Util.OpenURL(downloadPage);
                            }
                            catch { }
                        }
                        else
                        {

                        }
                    }
                    else if (updateType == UpdateType.Minor)
                    {

                        string fileUrl = Util.ParseString(content, $"{minorKey}[{minorVersionString}]", '{', '}');

                        //배포 시간차 가드: version.txt와 다운로드용 exe는 서로 다른 곳에 올라가므로 시간차가 생길 수 있다.
                        //exe 옆 MORT.dll.config의 MORT_VERSION_VALUE가 version.txt 버전과 같아야 양쪽 배포가 끝난 것으로 보고 진행한다.
                        //다르거나 받기 실패 -> 아직 배포 중으로 간주, 사용자에게 묻지 않고 조용히 종료(다음 실행 때 재시도).
                        if (!IsRemoteConfigVersionMatched(fileUrl, newVersionString))
                        {
                            Util.ShowLog("Remote MORT.dll.config version not matched yet (deploy in progress), skipping update");
                            return;
                        }

                        //업데이트 사전 검증: 바이너리 옆 .sha256 파일을 먼저 받아 본다. 해시 검증은 필수다(fail-closed).
                        //- URL 미기재(@MORT_MINOR_HASH 없음) -> 검증 불가이므로 진행하지 않음(에러로 간주)
                        //- URL은 있는데 다운 실패 -> 해시를 못 받음(준비 안 됨/네트워크 오류), 검증 불가이므로 진행하지 않음(에러로 간주)
                        string hashUrl = Util.ParseString(content, $"@MORT_MINOR_HASH ", '{', '}');

                        if (string.IsNullOrEmpty(hashUrl))
                        {
                            Util.ShowLog("Error: @MORT_MINOR_HASH not in version.txt, cannot verify update -> skipping update");
                            return;
                        }

                        string expectedHash = TryDownloadHash(ForceHttps(hashUrl));
                        if (string.IsNullOrEmpty(expectedHash))
                        {
                            Util.ShowLog("Error: failed to download update sha256, cannot verify update -> skipping update: " + hashUrl);
                            return;
                        }

                        string nowVersionString = nowVersion.ToString();
                        nowVersionString = nowVersionString.Insert(1, ".");
                        newVersionString = newVersionString.Insert(1, ".");

                        string checkMessageSubtitle = "(" + LocalizeManager.LocalizeManager.GetLocalizeString("Minor Update") + nowVersionString + " -> " + newVersionString + ")";
                        if (DialogResult.OK == MessageBox.Show(LocalizeString("Minor Update Message", true), checkMessageSubtitle, MessageBoxButtons.OKCancel))
                        {
                            string str = GetUpdaterLocalizeString();
                            Program.IS_FORCE_QUITE = true;

                            ProcessStartInfo psi = new ProcessStartInfo();
                            psi.WorkingDirectory = System.Environment.CurrentDirectory;
                            psi.FileName = "Updater.exe";

                            psi.ArgumentList.Add(newVersionString);
                            psi.ArgumentList.Add(fileUrl);
                            psi.ArgumentList.Add(downloadPage);
                            psi.ArgumentList.Add(str);
                            psi.ArgumentList.Add(expectedHash);
                            //psi.Arguments = $"{newVersionString} {fileUrl} {downloadPage} {str} {expectedHash}";
                            Process.Start(psi);

                            if (Application.MessageLoop)
                                Application.Exit();
                            else
                                Environment.Exit(1);

                            return;
                        }
                        else
                        {

                        }

                    }
                    else if (updateType == UpdateType.None)
                    {
                        if (IsDevVersion)
                        {
                            int finishedVersion = Util.ParseInt(content, "@MORT_DEV_FINISHED_VERSION ");

                            if (finishedVersion >= nowVersion)
                            {
                                string checkMessageSubtitle = LocalizeManager.LocalizeManager.GetLocalizeString("Release Update Title");
                                if (DialogResult.OK == MessageBox.Show(LocalizeManager.LocalizeManager.GetLocalizeString("Release Update Message"), checkMessageSubtitle, MessageBoxButtons.OKCancel))
                                {
                                    Logo.SetTopmost(false);
                                    try
                                    {
                                        Logo.SetTopmost(true);
                                        contract.ForceDisableTopMost();
                                        downloadPage = Util.ParseString(content, "@MORT_VERSION ", '{', '}');

                                        Util.OpenURL(downloadPage);
                                    }
                                    catch { }
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                
            }
        }

        private string GetUpdaterLocalizeString()
        {
            List<string> dataList = new List<string>();
            
            dataList.Add(LocalizeString("Updater Prepare"));
            dataList.Add(LocalizeString("Updater DownloadError"));
            dataList.Add(LocalizeString("Updater Complete"));
            dataList.Add(LocalizeString("Updater CompleteMessage"));
            dataList.Add(LocalizeString("Updater CompleteTitle"));
            dataList.Add(LocalizeString("Updater ErrorMessage"));
            dataList.Add(LocalizeString("Updater ErrorTitle"));
            dataList.Add(LocalizeString("Updater DownloadConfig"));
            dataList.Add(LocalizeString("Updater Downloading"));
            dataList.Add(LocalizeString("Updater HashMismatch"));   // keys[9] - 스프레드시트에서 키 추가 시 자동 적용

            return dataList.Aggregate((text, next) => text + "_" + next);
        }

        private void UpdateDic(string fileName, String data)
        {
            //Util.ShowLog("Dic data : " + data);
            Encoding utf8WithoutBom = new UTF8Encoding(false);
            if (!string.IsNullOrEmpty(data))
            {
                try
                {
                    using (StreamWriter file = new StreamWriter(fileName, false, utf8WithoutBom))
                    {
                        file.WriteLine(data);
                        file.Write(System.Environment.NewLine);

                        Util.ShowLog("Dic update complete");
                    }

                }
                catch (FileNotFoundException)
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                    {
                        fs.Close();
                        fs.Dispose();
                        using (StreamWriter file = new StreamWriter(fileName, true, utf8WithoutBom))
                        {
                            file.WriteLine(data);
                            file.Write(System.Environment.NewLine);
                        }
                    }
                }
            }
        }

        private void CheckDicVersion(String content, string checkType, string fileName)
        {
            try
            {
                if (content != null)
                {
                    int dicVersion = 0;
                    string result = Util.ParseStringFromFile(GlobalDefine.DATA_VERSION_FILE, checkType, '[', ']');

                    if (!string.IsNullOrEmpty(result))
                    {
                        dicVersion = Convert.ToInt32(result);
                    }


                    string newVersionString = "";
                    string downloadPage = "";

                    newVersionString = Util.ParseString(content, checkType, '[', ']');
                    downloadPage = Util.ParseString(content, checkType, '{', '}');
                    //var path = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
                    Util.ShowLog(checkType + " : " + dicVersion + " / " + newVersionString + " / download : " + downloadPage);

                    if (dicVersion < Convert.ToInt32(newVersionString))
                    {
                        using (WebClient client = new WebClient())
                        {
                            Stream stream = client.OpenRead(ForceHttps(downloadPage));
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                String dicData = reader.ReadToEnd();
                                dicData = dicData.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                                UpdateDic(fileName, dicData);
                                Util.ChangeFileData(GlobalDefine.DATA_VERSION_FILE, checkType, newVersionString, '[', ']');

                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        //TODO : 인터페이스로 변경하자
        private string LocalizeString(string key, bool replaceLine = false)
        {
            return LocalizeManager.LocalizeManager.GetLocalizeString(key).Replace("[]", "");
        }

    }
}
