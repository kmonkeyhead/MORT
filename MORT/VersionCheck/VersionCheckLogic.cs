using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        //버전 확인.
        public void CheckVersion()
        {
            if (GetCheckUpdate())
            {
                try
                {
                    nowVersion = Properties.Settings.Default.MORT_VERSION_VALUE;
                    //http://kmonkeyhead.github.io/MORT_VERSION/version.txt
                    using (WebClient client = new WebClient())
                    {
                        Stream stream = client.OpenRead("http://kmonkeyhead.github.io/MORT_VERSION/version.txt");
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
                using (WebClient client = new WebClient())
                {
                    Stream stream = client.OpenRead("http://kmonkeyhead.github.io/MORT_VERSION/default_setting.txt");
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

                        if(deeplUrl != "" && deeplFormat != "" && deeplElementTarget != "")
                        {
                            GlobalDefine.DeeplFrontUrl = deeplUrl;
                            GlobalDefine.DeeplFormat = deeplFormat;
                            GlobalDefine.DeeplElementTarget = deeplElementTarget;
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
                if (content != null)
                {
                    string versionKey = "@MORT_32BIT_VERSION ";
                    string minorKey = "@MORT_32BIT_MINOR_VERSION ";
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
                                System.Diagnostics.Process.Start(downloadPage);
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


                        string nowVersionString = nowVersion.ToString();
                        nowVersionString = nowVersionString.Insert(1, ".");
                        newVersionString = newVersionString.Insert(1, ".");

                        string checkMessageSubtitle = "(" + LocalizeManager.LocalizeManager.GetLocalizeString("Minor Update") + nowVersionString + " -> " + newVersionString + ")";
                        if (DialogResult.OK == MessageBox.Show(LocalizeString("Minor Update Message", true), checkMessageSubtitle, MessageBoxButtons.OKCancel))
                        {
                            var updater = new MORT.Updater.Updater();
                            updater.Show();
                            updater.DoDownload(newVersionString, fileUrl, downloadPage);


                            Program.IS_FORCE_QUITE = true;

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

                                        System.Diagnostics.Process.Start(downloadPage);
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
                            Stream stream = client.OpenRead(downloadPage);
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
