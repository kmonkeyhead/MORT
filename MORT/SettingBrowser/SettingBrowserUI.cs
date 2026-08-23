using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace MORT.SettingBrowser
{
    public partial class SettingBrowserUI : Form
    {
        private const string DATA_PATH = "https://kmonkeyhead.github.io/MORT_VERSION/Data/SETTING/";
        private const string MAIN_PATH = "Main";
        private class ListData
        {
            public string path;
            public string title;
            public string korTitle;

            public Dictionary<string, SettingData> dataList = new Dictionary<string, SettingData>();
            public bool isInit = false;

            public ListData()
            {

            }
            //메인 기준으로 먼저 설정한다.
            public ListData(string content)
            {
                string[] keys = content.Split('\t');

                if(keys.Length >= 2)
                {
                    path = keys[0];
                    title = keys[1];

                    if(keys.Length >= 3)
                    {
                        korTitle = keys[2];
                    }
                    isInit = true;


                    SettingData data = new SettingData();
                    data.path = MAIN_PATH;
                    dataList.Add(data.path, data);

                }
                else
                {
                    isInit = false;
                }
            }

        }

        private class SettingData
        {
            public string path;
            public string gamePath;
            private string title;
            private string korTitle;
            private string shopLink;
            private string extraLink;
            private string information;
            private string content;

            private string dbPath;          //db 주소
            private string settingPath;     //설정 주소
            private string downloadLink;    //다운로드 페이지 (대체)

            public bool isInit;

            public SettingData()
            {

            }

            public void Init(string path, string gamePath, string content)
            {
                isInit = false;

                this.path = path;
                this.gamePath = gamePath;
                title = Util.ParseString(content, "@TITLE", '[', ']');
                korTitle = Util.ParseString(content, "@KOR_TITLE", '[', ']');
                shopLink = Util.ParseString(content, "@STORE", '[', ']');
                extraLink = Util.ParseString(content, "@LINK", '[', ']');

                information = Util.ParseString(content, "@INFO", '[', ']');

                information = information.Replace("\r\n", "\n");
                information = information.Replace("\n", System.Environment.NewLine);

                dbPath = Util.ParseString(content, "@DB", '[', ']');
                settingPath = Util.ParseString(content, "@SETTING", '[', ']');

                downloadLink = Util.ParseString(content, "@DOWNLOAD_LINK", '[', ']');

                isInit = true;
            }

            public string GetTitle()
            {
                string title = this.title;

                if(!string.IsNullOrEmpty(korTitle))
                {
                    title += " / " + korTitle;
                }


                return title;
            }

            public string GetShop()
            {
                return shopLink;
            }

            public string GetExtraLink()
            {
                return extraLink;
            }

            public string GetInformation()
            {
                return information;
            }

            public string GetDBPath()
            {
                string result = "";

                if(!string.IsNullOrEmpty(dbPath))
                {
                    result = DATA_PATH + gamePath + "/" + path + "/" + dbPath;
                }

                return result;
            }

            public string GetDownloadLink()
            {
                return downloadLink;
            }

            public string GetDBFileName()
            {
                return dbPath;
            }

            public string GetSettingPath()
            {
                string result = "";

                if(!string.IsNullOrEmpty(settingPath))
                {
                    result = DATA_PATH + gamePath + "/" + path + "/" + settingPath;
                }

                Util.ShowLog(result);
                return result;
            }

            public string GetSettingFileName()
            {
                return settingPath;
            }

        }

        private List<ListData> searchDataList = new List<ListData>();
        private List<ListData> gameDataList = new List<ListData>();
        private SettingData selectedData = null;
        private ListData selectedListData = null;

        private bool isInit = false;
        public SettingBrowserUI()
        {
            InitializeComponent();
        }

        public void Init()
        {
            tbInformation.SelectionHangingIndent = 11;
            listView1.FullRowSelect = true;
            listView1.BeginUpdate();

            listView1.Items.Clear();
            searchDataList.Clear();

            DownloadList();
            for(int i = 0; i < gameDataList.Count; i++)
            {
                ListData data = gameDataList[i];
                ListViewItem item = new ListViewItem(data.title);
                item.SubItems.Add(data.korTitle);
                item.Tag = data;
                listView1.Items.Add(item);
            }

            listView1.EndUpdate();

            lbTitleResult.Text = "";
            SetLinkLabel(lbLinkShop, "");
            SetLinkLabel(lbLinkExtra, "");


            btApplay.Enabled = false;
            isInit = true;
        }


        private void DownloadList()
        {
            using(WebClient client = new WebClient())
            {
                Stream stream = client.OpenRead("https://kmonkeyhead.github.io/MORT_VERSION/Data/SETTING/list.txt");
                using(StreamReader reader = new StreamReader(stream))
                {
                    gameDataList.Clear();

                    while(!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        ListData data = new ListData(line);

                        if(data.isInit)
                        {
                            gameDataList.Add(data);

                        }

                    }
                }

            }
        }

        private void ShowInformation(ListViewItem item, string key)
        {
            selectedData = null;
            selectedListData = null;
            ListData data = item.Tag as ListData;
            selectedListData = data;

            bool isRequireDownload = false;
            SettingData settingData = null;
            //추후에 
            if(data.dataList.ContainsKey(key))
            {
                settingData = data.dataList[key];

                if(!settingData.isInit)
                {
                    isRequireDownload = true;
                }
            }
            else
            {
                isRequireDownload = true;
            }



            if(isRequireDownload)
            {
                using(WebClient client = new WebClient())
                {
                    Stream stream = client.OpenRead(DATA_PATH + data.path + "/" + key + "/info.txt");
                    using(StreamReader reader = new StreamReader(stream))
                    {

                        if(settingData == null)
                        {
                            settingData = new SettingData();
                        }
                        settingData.Init(key, data.path, reader.ReadToEnd());
                    }
                }
            }


            if(settingData != null && settingData.isInit)
            {
                lbTitleResult.Text = settingData.GetTitle();
                SetLinkLabel(lbLinkShop, settingData.GetShop());
                SetLinkLabel(lbLinkExtra, settingData.GetExtraLink());

                tbInformation.Text = settingData.GetInformation();
                selectedData = settingData;


                //DB나 설정 파일이 없으면 링크로 대체

                string settingPath = selectedData.GetSettingPath();
                string dbPath = selectedData.GetDBPath();

                if(string.IsNullOrEmpty(settingPath) && string.IsNullOrEmpty(dbPath))
                {
                    btApplay.Text = "다운로드 사이트로 이동";
                }
                else
                {
                    btApplay.Text = "적용";
                }


            }
            else
            {
                selectedData = null;
                selectedListData = null;
                lbTitleResult.Text = "에러!";
            }
        }

        private void SetLinkLabel(LinkLabel lbLink, string text)
        {
            lbLink.Text = text;
            lbLink.Links.Clear();
            if(!string.IsNullOrEmpty(text))
            {
                lbLink.Links.Add(0, text.Length, text);
            }
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = null;


            if(listView1.Items.Count > 0)
            {
                items = listView1.SelectedItems;

                if(items.Count > 0)
                {
                    ListViewItem item = items[0];
                    ShowInformation(item, MAIN_PATH);

                    btApplay.Enabled = true;
                }
            }
            else
            {
                btApplay.Enabled = false;
            }

        }

        private void lbLinkShop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string url = e.Link.LinkData as string;
                Util.OpenURL(url);
            }
            catch { }
        }

        private void lbLinkExtra_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string url = e.Link.LinkData as string;
                Util.OpenURL(url);
            }
            catch { }
        }

        private void tbInformation_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                string url = e.LinkText;
                Util.OpenURL(e.LinkText);
            }
            catch { }
        }

        private void btAppaly_Click(object sender, EventArgs e)
        {
            if(selectedData != null)
            {
                string settingPath = selectedData.GetSettingPath();
                string dbPath = selectedData.GetDBPath();


                if(string.IsNullOrEmpty(settingPath) && string.IsNullOrEmpty(dbPath))
                {
                    Util.OpenURL(selectedData.GetDownloadLink());
                }
                else
                {
                    bool isDownloadComplete = false;
                    if(settingPath != "")
                    {
                        using(WebClient client = new WebClient())
                        {
                            Stream stream = client.OpenRead(settingPath);
                            using(StreamReader reader = new StreamReader(stream))
                            {
                                string data = reader.ReadToEnd();
                                data = data.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                                Util.SaveFile(GlobalDefine.SETTING_PATH + selectedData.GetSettingFileName(), data);

                                isDownloadComplete = true;
                            }

                        }
                    }

                    if(dbPath != "")
                    {
                        using(WebClient client = new WebClient())
                        {
                            Stream stream = client.OpenRead(dbPath);
                            using(StreamReader reader = new StreamReader(stream))
                            {
                                string data = reader.ReadToEnd();
                                data = data.Replace("\r\n", "\n").Replace("\n", System.Environment.NewLine);
                                Util.SaveFile(GlobalDefine.DB_PATH + selectedData.GetDBFileName(), data);
                            }

                        }
                    }

                    if(isDownloadComplete)
                    {
                        settingPath = GlobalDefine.SETTING_PATH + selectedData.GetSettingFileName();

                        FormManager.Instace.MyMainForm.OpenSettingFile(settingPath);
                        string message = "적용 및 다운로드 완료!" + System.Environment.NewLine + "OCR 영역 및 번역 설정은 다시 확인해 주세요" +
                            System.Environment.NewLine + System.Environment.NewLine +

                            "[다운 받은 파일]" + System.Environment.NewLine + "설정파일 : " + settingPath;

                        if(dbPath != "")
                        {
                            dbPath = GlobalDefine.DB_PATH + selectedData.GetDBFileName();
                            message += System.Environment.NewLine + "DB파일 : " + dbPath;
                        }
                        MessageBox.Show(message, "MORT");
                    }
                    else
                    {
                        MessageBox.Show("적용 실패 - 재시도해 주세요", "MORT");
                    }
                }




            }
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            string text = tbSearch.Text.ToLower();
            SearchGame(text);
        }

        private void SearchGame(string text)
        {
            if(isInit)
            {
                searchDataList.Clear();
                for(int i = 0; i < gameDataList.Count; i++)
                {
                    if(gameDataList[i].title.ToLower().Contains(text) || gameDataList[i].korTitle.ToLower().Contains(text))
                    {
                        searchDataList.Add(gameDataList[i]);
                    }
                }


                listView1.BeginUpdate();

                listView1.Items.Clear();

                int foundIndex = -1;

                for(int i = 0; i < searchDataList.Count; i++)
                {
                    ListData data = searchDataList[i];
                    ListViewItem item = new ListViewItem(data.title);
                    item.SubItems.Add(data.korTitle);
                    item.Tag = data;
                    listView1.Items.Add(item);

                    if(selectedListData != null && selectedListData == searchDataList[i])
                    {
                        foundIndex = i;
                    }


                }

                if(foundIndex != -1)
                {
                    listView1.Items[foundIndex].Selected = true;
                }

                listView1.EndUpdate();
            }

        }

        private void SettingBrowserUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormManager.Instace.DestorySettingBrowserUI();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            SearchGame(tbSearch.Text);
        }
    }
}
