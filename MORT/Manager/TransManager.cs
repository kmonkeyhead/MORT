using MORT.TransAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace MORT
{
    class TransManager : IDisposable
    {
        public class TransCodeData
        {
            public string Title
            {
                get
                {
                    if (_customCode)
                    {
                        return title;
                    }

                    return LocalizeManager.LocalizeManager.GetLocalizeString(Key, title);
                }
            }

            public string Key { get; private set; }
            public string DeepLCode { get; }

            private string title;
            public string languageCode;
            public string googleCode;
            public string naverCode;
            private readonly bool _customCode;

            public TransCodeData(string key, string title, string languageCode, string deepLCode, string googleCode, string naverCode, bool customCode)
            {
                Key = key;
                this.title = title;
                this.languageCode = languageCode;
                DeepLCode = deepLCode;
                this.googleCode = googleCode;
                this.naverCode = naverCode;
                this._customCode = customCode;
            }
        }

        private class TransData
        {
            public int index;
            public string text = "";
            public string result = "";
        }

        //TODO : 모두 DI로 옮겨야 함
        private static TransManager _instance;

        public static TransManager Instace => _instance;
        public void TempInitalizeInstance()
        {
            _instance = this;
        }

        public class NaverKeyData
        {
            public enum eState
            {
                Normal,
                Error,
                Limit,
            }

            public string id;
            public string secret;
            public eState eNMTstate = NaverKeyData.eState.Normal;
            public bool isPaid => true;


            public NaverKeyData(string id, string secret, bool isPaid = false)
            {
                this.id = id;
                this.secret = secret;
                //this.isPaid = isPaid;
            }

            public void SetState(NaverKeyData.eState state, string apiType)
            {
                this.eNMTstate = state;
            }
        }

        public static bool isSaving = false;
        public static bool s_CheckedGoogleBasicWarning;
        public static bool s_CheckedPapagoWebWarning;
        public static bool s_CheckedDeeplWarning;

        public const int MAX_NAVER = 20;
        public GSTrans.Sheets sheets;

        public string googleKey;

        //TODO : 다음 업데이트 때 시트 안에서 처리로 바꿔야함.
        //백업용 구글 시트 번역 언어
        private string _backupSheetSource = "";
        private string _backupSheetTarget = "";

        public int currentNaverIndex;
        public List<NaverKeyData> naverKeyList = new List<NaverKeyData>();


        private const int MAX_FORMER = 10000;

        public Dictionary<string, string> TempDbDic = new Dictionary<string, string>();
        private Dictionary<SettingManager.TransType, Dictionary<string, string>> resultDic = new Dictionary<SettingManager.TransType, Dictionary<string, string>>();

        private Dictionary<SettingManager.TransType, List<KeyValuePair<string, string>>> saveResultDic =
            new Dictionary<SettingManager.TransType, List<System.Collections.Generic.KeyValuePair<string, string>>>();

        private Dictionary<string, string> userTranslationDic = new Dictionary<string, string>();

        private bool isTranslationDbStyle = false;

        private readonly CustomAPI _customAPI;
        private DeepLAPITranslateAPI _deeplapiranslateAPI = new DeepLAPITranslateAPI();
        private DeepLTranslateAPI _deepLTranslateAPI = new DeepLTranslateAPI();
        private PipeServer.PipeServer _ezTransPipeServer = new PipeServer.PipeServer();
        private PapagoWebTranslateAPI _papagoWebAPI = new PapagoWebTranslateAPI();
        private readonly ChromeBridgeTranslateAPI _chromeBridgeAPI;
        private readonly GeminiTranslatorAPI _geminiTranslatorAPI;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public TransManager(GeminiTranslatorAPI geminiTranslatorAPI, CustomAPI customAPI, ChromeBridgeTranslateAPI chromeBridgeAPI)
        {
            _customAPI = customAPI;
            _geminiTranslatorAPI = geminiTranslatorAPI;
            _chromeBridgeAPI = chromeBridgeAPI;
        }

        public bool InitEzTrans()
        {
            return _ezTransPipeServer.InitPipe();
        }

        public void InitCustomApi(string url, string source, string target, string presetName)
        {
            _customAPI.Init(url, source, target, presetName);
        }

        public void InitChromeBridge(string source, string target, string mode)
        {
            _chromeBridgeAPI.Init(source, target, mode);
        }

        public void InitDeeplApiKey(string apiKey) => _deeplapiranslateAPI.InitApiKey(apiKey);

        public void InitDeepLAPI(string source, string target, SettingManager.DeepLAPIEndpointType endpointType)
        {
            _deeplapiranslateAPI.Init(source, target, endpointType);
        }

        public void InitDeepL(string transCode, string resultCode, string frontUrl, string urlFormat, string elementTarget)
        {
            _deepLTranslateAPI.Init(transCode, resultCode, frontUrl, urlFormat, elementTarget);
        }

        public void InitializeGeminiModel(string model, string apiKey)
        {
            _geminiTranslatorAPI.InitializeModel(model, apiKey, model != "custom");
        }

        public void InitGemini(string transCode, string resultCode)
        {
            _geminiTranslatorAPI.Initialize(transCode, resultCode);
        }

        public void InitGeminiCustom(string customModel, string command, bool disableDefaultCommand)
        {
            _geminiTranslatorAPI.InitializeCustom(customModel, command, disableDefaultCommand);
        }

        public void InitPapagoWeb(string transCode, string resultCode)
        {
            _papagoWebAPI.Init(transCode, resultCode);
        }

        public void ShowDeeplWebView()
        {
            _deepLTranslateAPI.ShowWebview();
        }

        public void InitDeepLContract(IDeeplAPIContract contract) => _deepLTranslateAPI.InitContract(contract);

        public void LoadUserTranslation(List<string> files)
        {
            var transType = FormManager.Instace.MyMainForm.MySettingManager.NowTransType;

            if (transType != SettingManager.TransType.db)
            {
                isTranslationDbStyle = false;
                userTranslationDic.Clear();
                var languageType = FormManager.Instace.MyMainForm.MySettingManager.GetOcrLanguage();
                if (AdvencedOptionManager.IsTranslationDbStyle && (languageType == OcrLanguageType.English || languageType == OcrLanguageType.Japen))
                {
                    string data = "";
                    foreach (var obj in files)
                    {
                        string path = GlobalDefine.ADVENCED_TRANSRATION_PATH + obj + ".txt";
                        var stream = Util.OpenFile(path);
                        if (stream != null)
                        {
                            data += stream.ReadToEnd();
                            data += System.Environment.NewLine;
                        }
                    }

                    Util.SaveFile(GlobalDefine.DB_PATH + AdvencedOptionManager.TEMP_USER_TRANSLATION_DB_FILE, data);

                    bool isStringUpper = AdvencedOptionManager.IsTranslationStringUpper;


                    Form1.SetIsStringUpper(isStringUpper);
                    Form1.setUseDB(true, false, AdvencedOptionManager.TEMP_USER_TRANSLATION_DB_FILE);
                    isTranslationDbStyle = true;
                }
                else
                {
                    foreach (var obj in files)
                    {
                        string path = GlobalDefine.ADVENCED_TRANSRATION_PATH + obj + ".txt";
                        userTranslationDic = Util.LoadDBFile(path, userTranslationDic);
                    }

                    isTranslationDbStyle = false;
                }
            }
        }

        /// <summary>
        /// 이전에 기록한 결과 제거.
        /// </summary>
        public void ClearFormerDic()
        {
            foreach (var obj in resultDic)
            {
                obj.Value.Clear();
                ClearFormerResultFile(obj.Key);
            }

            foreach (var obj in saveResultDic)
            {
                obj.Value.Clear();
            }
        }

        /// <summary>
        /// 이전에 기록한 결과 사전 초기화.
        /// </summary>
        public void InitFormerDic()
        {
            // 1. 딕셔너리 만듬.
            // 2. 파일 불러옴.
            // 3. 만개 이상이면 반띵함.

            MakeFormerDic(resultDic);
            LoadFormerResultFile(SettingManager.TransType.google);
            LoadFormerResultFile(SettingManager.TransType.google_url);
            LoadFormerResultFile(SettingManager.TransType.naver);
            LoadFormerResultFile(SettingManager.TransType.deepl);
            LoadFormerResultFile(SettingManager.TransType.customApi);
            LoadFormerResultFile(SettingManager.TransType.papago_web);
            LoadFormerResultFile(SettingManager.TransType.deeplApi);
            LoadFormerResultFile(SettingManager.TransType.gemini);
            LoadFormerResultFile(SettingManager.TransType.chromeBridge);
        }

        private void MakeFormerDic(Dictionary<SettingManager.TransType, Dictionary<string, string>> dic)
        {
            if (dic == null)
            {
                dic = new Dictionary<SettingManager.TransType, Dictionary<string, string>>();
            }
            else
            {
                dic.Clear();
            }

            Dictionary<string, string> naverDic = new Dictionary<string, string>();
            Dictionary<string, string> googleDic = new Dictionary<string, string>();
            Dictionary<string, string> basicDic = new Dictionary<string, string>();
            Dictionary<string, string> deeplDic = new Dictionary<string, string>();
            Dictionary<string, string> customDic = new Dictionary<string, string>();
            Dictionary<string, string> papagoWebDic = new Dictionary<string, string>();
            Dictionary<string, string> deeplapiDic = new Dictionary<string, string>();
            Dictionary<string, string> geminiDic = new Dictionary<string, string>();
            Dictionary<string, string> chromeBridgeDic = new Dictionary<string, string>();

            dic.Add(SettingManager.TransType.google, googleDic);
            dic.Add(SettingManager.TransType.naver, naverDic);
            dic.Add(SettingManager.TransType.google_url, basicDic);
            dic.Add(SettingManager.TransType.deepl, deeplDic);
            dic.Add(SettingManager.TransType.customApi, customDic);
            dic.Add(SettingManager.TransType.papago_web, papagoWebDic);
            dic.Add(SettingManager.TransType.deeplApi, deeplapiDic);
            dic.Add(SettingManager.TransType.gemini, geminiDic);
            dic.Add(SettingManager.TransType.chromeBridge, chromeBridgeDic);

            if (saveResultDic == null)
            {
                saveResultDic = new Dictionary<SettingManager.TransType, List<KeyValuePair<string, string>>>();
            }
            else
            {
                saveResultDic.Clear();
            }


            saveResultDic.Add(SettingManager.TransType.google, new List<KeyValuePair<string, string>>());
            saveResultDic.Add(SettingManager.TransType.naver, new List<KeyValuePair<string, string>>());
            saveResultDic.Add(SettingManager.TransType.google_url, new List<KeyValuePair<string, string>>());
            saveResultDic.Add(SettingManager.TransType.deepl, new List<KeyValuePair<string, string>>());
            saveResultDic.Add(SettingManager.TransType.customApi, new List<KeyValuePair<string, string>>());
            saveResultDic.Add(SettingManager.TransType.papago_web, new List<KeyValuePair<string, string>>());
            saveResultDic.Add(SettingManager.TransType.deeplApi, new List<KeyValuePair<string, string>>());
            saveResultDic.Add(SettingManager.TransType.gemini, new List<KeyValuePair<string, string>>());
            saveResultDic.Add(SettingManager.TransType.chromeBridge, new List<KeyValuePair<string, string>>());
        }

        private void LoadFormerResultFile(SettingManager.TransType transType)
        {
            resultDic[transType].Clear();
            saveResultDic[transType].Clear();

            Dictionary<string, string> dic = resultDic[transType];
            string path = string.Format(GlobalDefine.FORMER_TRANS_FILE, transType.ToString());

            dic = Util.LoadDBFile(path, dic);
        }

        public void SaveFormerResultFile(SettingManager.TransType transType)
        {
            if (!isSaving)
            {
                Task task = RunSaveFormerResultFile(transType);
            }
        }

        private async Task RunSaveFormerResultFile(SettingManager.TransType transType)
        {
            isSaving = true;

            await Task.Run(() =>
            {
                try
                {
                    string saveData = "";
                    string path = string.Format(GlobalDefine.FORMER_TRANS_FILE, transType.ToString());

                    List<KeyValuePair<string, string>> list = null;

                    if (saveResultDic.ContainsKey(transType))
                    {
                        list = saveResultDic[transType];
                    }


                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            string data = "/s";
                            data += Environment.NewLine + list[i].Key + Environment.NewLine + "/t" + Environment.NewLine + list[i].Value + Environment.NewLine + "/e";

                            saveData += data + Environment.NewLine + Environment.NewLine;
                        }

                        Util.SaveFile(path, saveData, true);
                        list.Clear();
                    }
                }
                catch
                {
                }

                isSaving = false;
            }).ConfigureAwait(true);
        }

        public void ClearFormerResultFile(SettingManager.TransType transType)
        {
            if (!isSaving)
            {
                string saveData = "";
                string path = string.Format(GlobalDefine.FORMER_TRANS_FILE, transType.ToString());
                Util.SaveFile(path, saveData);
            }
        }

        private string GetUserTransResult(string ocrText)
        {
            string result = null;

            if (isTranslationDbStyle)
            {
                StringBuilder sb = new StringBuilder(ocrText, 8192);
                StringBuilder sb2 = new StringBuilder(8192);
                Form1.ProcessGetDBText(sb, sb2);

                result = sb2.ToString();

                if (result == "not thing")
                {
                    result = null;
                }
            }
            else
            {
                if (userTranslationDic.TryGetValue(ocrText, out result))
                {
                }
            }

            return result;
        }

        /// <summary>
        /// 유저 사전과 이전 결과를 이용해 가져온다.
        /// </summary>
        /// <param name="transType"></param>
        /// <param name="ocrValue"></param>
        /// <returns></returns>
        private string GetFormerResult(SettingManager.TransType transType, string ocrValue)
        {
            string result = GetUserTransResult(ocrValue);

            if (result == null && !isSaving)
            {
                bool isFound = false;
                if (!resultDic.ContainsKey(transType))
                {
                    resultDic.Add(transType, new Dictionary<string, string>());
                }

                if (resultDic[transType].TryGetValue(ocrValue, out result))
                {
                    isFound = true;
                }
            }

            return result;
        }

        private void AddFormerResult(SettingManager.TransType transType, string ocrValue, string result)
        {
            if (!isSaving)
            {
                if (resultDic[transType].Count >= MAX_FORMER)
                {
                    resultDic[transType].Clear();
                    saveResultDic[transType].Clear();
                    //파일을 열고 다 지운다.

                    ClearFormerResultFile(transType);
                }

                resultDic[transType][ocrValue] = result;
                saveResultDic[transType].Add(new KeyValuePair<string, string>(ocrValue, result));
            }
        }

        public void InitGtrans(string sheetID, string clientID, string secretKey, string source, string result)
        {
            Console.WriteLine("InitGtrans");
            if (sheets == null)
            {
                sheets = new GSTrans.Sheets();
            }

            googleKey = sheetID;
            sheets.spreadsheetId = @sheetID; // @"1k4dlDiXjuJnIS0K1EYuMxB40f_cZP3t0sGtS5cv3J3I";


            bool isComplete = sheets.Init(@sheetID, clientID, secretKey);
            Console.WriteLine("InitGtrans - complete");
            //sheets.RowCount = 30;
            //초기화 - 반드시 해줘야 함 - 시트가 제대로 준비되었는지 확인하고, 준비되지 않았을 경우 셋팅합니다.
            //sheets.Initialize();

            //초기화 후 언제나 변경 가능한 변수 설정
            sheets.source = source;
            sheets.target = result;

            //백업
            _backupSheetSource = source;
            _backupSheetTarget = result;

            if (!isComplete && clientID != "")
            {
                SettingManager.IsErrorEmptyGoogleToken = true;
            }
        }

        public void InitGTransToken()
        {
            if (sheets != null)
            {
                sheets.InitToken();
            }
        }

        public void DeleteAllGsTransToken()
        {
            Util.ShowLog("Delete google token");
            sheets.DeleteToken();
        }

        public async Task<string> StartTrans(string text, SettingManager.TransType trasType, List<string> textList = null)
        {
            if (text == "")
            {
                return "";
            }

            _cts.Cancel();
            _cts.Dispose();

            _cts = new CancellationTokenSource();

            Task<string> task1 = null;

            if (textList == null)
            {
                textList = new List<string>();
                textList.Add(text);
            }

            task1 = Task<string>.Run(() => GetTransLinesAsync(textList, trasType));
            string result = await task1;

            return result;
        }

        public void StopTrans()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        private async Task<string> GetTransLinesAsync(List<string> textList, SettingManager.TransType transType)
        {
            int removeLength = System.Environment.NewLine.Length;
            Dictionary<int, TransData> textDic = new Dictionary<int, TransData>();

            for (int i = 0; i < textList.Count; i++)
            {
                TransData data = new TransData();
                data.index = i;
                data.text = textList[i].TrimEnd();

                data.result = "";

                textDic.Add(data.index, data);
            }

            try
            {
                bool isError = false;
                bool isContain = false;
                bool isUseMemoryDic = true;
                string formerResult = null;
                bool splitTranslation = transType == SettingManager.TransType.gemini;
                splitTranslation = false;
                string spliteToken = textList.Count > 1 ? Util.GetSpliteToken(transType) : "";
                List<string> ocrTexts = new();

                if (transType == SettingManager.TransType.db || transType == SettingManager.TransType.ezTrans)
                {
                    isUseMemoryDic = false;
                }


                if (isUseMemoryDic)
                {
                    string ocrText = "";
                    bool requireTranslate = false;
                    foreach (var obj in textDic)
                    {
                        obj.Value.result = GetFormerResult(transType, obj.Value.text);

                        if (string.IsNullOrEmpty(obj.Value.result))
                        {
                            //기억에 없는 텍스트만 번역한다

                            if (!splitTranslation)
                            {
                                ocrText += spliteToken + obj.Value.text + System.Environment.NewLine;
                            }
                            else
                            {
                                ocrTexts.Add(obj.Value.text);
                            }

                            obj.Value.result = "";
                            requireTranslate = true;
                        }
                        else
                        {
                            if (Form1.IsDebugShowFormerResultLog)
                            {
                                obj.Value.result = "[기억 결과 " + resultDic[transType].Count.ToString() + " ] " + obj.Value.result;
                            }
                        }
                    }

                    if (requireTranslate)
                    {
                        string transResult = "";
                        if (transType == SettingManager.TransType.naver)
                        {
                            transResult = NaverTranslateAPI.instance.GetResult(ocrText, ref isError);
                            transResult = transResult.Replace("\r\n ", System.Environment.NewLine);
                        }
                        else if (transType == SettingManager.TransType.papago_web)
                        {
                            var papagoResult = await _papagoWebAPI.TranslateAsync(ocrText);
                            isError = papagoResult.IsError;
                            transResult = papagoResult.Result;
                        }
                        else if (transType == SettingManager.TransType.google)
                        {
                            transResult = DoGoogleSheetTranslate(ocrText, ref isError);
                            transResult = transResult.Replace("\r\n ", System.Environment.NewLine);
                        }
                        else if (transType == SettingManager.TransType.google_url)
                        {
                            transResult = GoogleBasicTranslateAPI.instance.DoTrans(ocrText, ref isError);
                        }
                        else if (transType == SettingManager.TransType.customApi)
                        {
                            transResult = _customAPI.GetResult(ocrText, ref isError);
                            transResult = transResult.Replace("\r\n", "\n");
                            transResult = transResult.Replace("\n", System.Environment.NewLine);
                        }
                        else if (transType == SettingManager.TransType.chromeBridge)
                        {
                            transResult = await _chromeBridgeAPI.GetResultAsync(ocrText, _cts.Token);
                            isError = _chromeBridgeAPI.IsError;
                            transResult = transResult.Replace("\r\n", "\n");
                            transResult = transResult.Replace("\n", System.Environment.NewLine);
                        }
                        else if (transType == SettingManager.TransType.gemini)
                        {
                            var geminiResult = await _geminiTranslatorAPI.TranslateTextAsync(ocrText, _cts.Token);
                            transResult = geminiResult.Result;
                            isError = geminiResult.Error == GeminiErrorType.NormalError;

                            if (geminiResult.Error is GeminiErrorType.ProhibitedContent)
                            {
                                transResult = GoogleBasicTranslateAPI.instance.DoTrans(ocrText, ref isError);
                            }
                            else
                            {
                                transResult = transResult.Replace("\r\n", System.Environment.NewLine);
                                transResult = transResult.Replace("\n", System.Environment.NewLine);
                            }
                        }
                        else if (transType == SettingManager.TransType.deepl)
                        {
                            transResult = _deepLTranslateAPI.DoTrans(ocrText, ref isError, _cts.Token);

                            if (isError && AdvencedOptionManager.UseDeeplAltOption)
                            {
                                isError = false;
                                transResult = GoogleBasicTranslateAPI.instance.DoTrans(ocrText, ref isError);
                            }
                            else
                            {
                                transResult = transResult.Replace("\\r\\n", System.Environment.NewLine);
                                transResult = transResult.Replace("\\n", System.Environment.NewLine);
                            }
                        }
                        else if (transType == SettingManager.TransType.deeplApi)
                        {
                            transResult = _deeplapiranslateAPI.GetResult(ocrText, ref isError);
                            transResult = transResult.Replace("\\r\\n", System.Environment.NewLine);
                            transResult = transResult.Replace("\\n", System.Environment.NewLine);
                        }

                        if (isError)
                        {
                            //문제가 있으면 바로 끝낸다.
                            return transResult;
                        }
                        else
                        {
                            //string[] separatingStrings = { Util.GetSpliteToken(transType) };
                            //string[] words = transResult.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                            string[] words = Util.GetSpliteByToken(transResult, transType);
                            int index = 0;
                            foreach (var obj in textDic)
                            {
                                if (string.IsNullOrEmpty(obj.Value.result))
                                {
                                    if (words.Length > index)
                                    {
                                        obj.Value.result = words[index++];

                                        AddFormerResult(transType, obj.Value.text, obj.Value.result);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (transType == SettingManager.TransType.db)
                {
                    foreach (var obj in textDic)
                    {
                        if(TempDbDic.TryGetValue(obj.Value.text, out var dbResult))
                        {
                            obj.Value.result = dbResult;
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder(obj.Value.text, 8192);
                            StringBuilder sb2 = new StringBuilder(8192);
                            Form1.ProcessGetDBText(sb, sb2);

                            obj.Value.result = sb2.ToString();

                            if(obj.Value.result == "not thing")
                            {
                                obj.Value.result = "";
                            }
                        }
                       
                    }
                }
                else if (transType == SettingManager.TransType.ezTrans)
                {
                    foreach (var obj in textDic)
                    {
                        if (_ezTransPipeServer.InitResponse)
                        {
                            obj.Value.result = GetUserTransResult(obj.Value.text);
                            if (obj.Value.result == null)
                            {
                                obj.Value.result = await _ezTransPipeServer.DoTransAsync(obj.Value.text);
                            }
                        }
                        else
                        {
                            obj.Value.result = "이지트랜스를 사용할 수 없습니다.";
                        }
                    }
                }

                string result = "";

                foreach (var obj in textDic)
                {
                    result += spliteToken + obj.Value.result + System.Environment.NewLine;
                }

                return result;
            }
            catch (Exception e)
            {
                return "Error " + e;
            }
        }

        public string DoGoogleSheetTranslate(string original, ref bool isError)
        {
            string result = "";

            sheets.target = _backupSheetTarget;
            sheets.source = _backupSheetSource;

            if (sheets.source != "ja" && AdvencedOptionManager.IsExecutive)
            {
                sheets.target = "ja";
                original = sheets.Translate(original, ref isError);
                result = original;


                Util.ShowLog("Ori : " + original);
                if (!isError)
                {
                    sheets.source = "ja";
                    sheets.target = _backupSheetTarget;
                    result = sheets.Translate(original, ref isError);
                }
            }
            else
            {
                result = sheets.Translate(original, ref isError);
            }

            return result;
        }

        public static bool GetIsRemain()
        {
            bool isRemain = true;

            if (_instance == null)
            {
                isRemain = false;
            }

            return isRemain;
        }

        private void AddTransCode(string key, string title, string ocrCode, string naverCode, string googleCode, string deepLCode, bool customCode = false)
        {
            if (codeDataList.Any(r => r.Key == key))
            {
                //중복 방지
                return;
            }

            TransCodeData data = new TransCodeData(key, title, ocrCode, deepLCode, googleCode, naverCode, customCode);
            codeDataList.Add(data);
        }

        private void InitCustomTransCode()
        {
            try
            {
                string path = Util.ToCurrentFilePath(GlobalDefine.CUSTOM_TRANSCODE_FILE);
                StreamReader r = new StreamReader(path);
                string line;

                while ((line = r.ReadLine()) != null)
                {
                    if (line != null)
                    {
                        line = line.Trim();
                        string[] keys = line.Split(',');

                        if (keys.Length == 2 && !keys[0].Contains("//"))
                        {
                            string code = keys[0].Trim();
                            string title = keys[1].Trim();
                            //codeDataList
                            AddTransCode(code, title, "", code, code, code, customCode: true);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                r.Close();
                r.Dispose();
            }
            catch
            {
            }
        }

        public TransCodeData GetTransCodeData(string ocrCode)
        {
            TransCodeData data = null;


            for (int i = 0; i < codeDataList.Count; i++)
            {
                if (Util.GetIsEqualMainOcrCode(ocrCode, codeDataList[i].languageCode))
                {
                    data = codeDataList[i];
                    break;
                }
            }

            return data;
        }

        List<TransCodeData> codeDataList = new List<TransCodeData>();

        public void InitTransCode(System.Windows.Forms.ComboBox cbNaver, System.Windows.Forms.ComboBox cbNaverResult,
            System.Windows.Forms.ComboBox cbGoogle, System.Windows.Forms.ComboBox cbGoogleResult,
            System.Windows.Forms.ComboBox cbDeepL, System.Windows.Forms.ComboBox cbDeepLResult,
            System.Windows.Forms.ComboBox cbGoogleOcr)
        {
            //TODO : 코드와 콤보박스 모두 설정할 수 있도록 변경해야 한다.


            codeDataList.Clear();
            AddTransCode("en", "영어", "en", "en", "en", "en");
            AddTransCode("ja", "일본어", "ja", "ja", "ja", "ja");

            AddTransCode("zh-CN", "중국어 간체", "zh-Hans-CN", "zh-CN", "zh-CN", "ZH-HANS");
            AddTransCode("zh-TW", "중국어 번체", "zh-Hant-TW", "zh-TW", "zh-TW", "ZH-HANT");
            AddTransCode("es", "스페인어", "es", "es", "es", "es");
            AddTransCode("fr", "프랑스어", "fr-FR", "fr", "fr", "fr");
            AddTransCode("vi", "베트남어", "vi", "vi", "vi", "vi");
            AddTransCode("th", "태국어", "th", "th", "th", "th");
            AddTransCode("id", "인도네시아어", "id", "id", "id", "id");
            AddTransCode("ko", "한국어", "ko", "ko", "ko", "ko");
            AddTransCode("ru", "러시아어", "ru", "ru", "ru", "ru");
            AddTransCode("de", "독일어", "de-DE", "de", "de", "de");
            AddTransCode("pt-BR", "브라질어", "pt-BR", "", "pt-BR", "pt-BR");
            AddTransCode("pt-PT", "포르투갈어", "pt-PT", "pt", "pt-PT", "pt-PT");
            AddTransCode("tr", "터키어", "tr", "", "tr", "tr");
            AddTransCode("it", "이탈리아어", "it", "", "it", "it");

            AddTransCode("pl", "폴란드어", "pl", "", "pl", "pl");
            AddTransCode("ar", "아랍어", "ar", "ar", "ar", "ar");
            AddTransCode("hu", "헝가리어", "hu", "", "hu", "hu");
            AddTransCode("uk", "우크라이나어", "uk", "", "uk", "uk");
            AddTransCode("cs", "체코어", "cs", "", "cs", "cs");
            AddTransCode("fa", "페르시아어", "fa", "", "fa", "");
            AddTransCode("el", "그리스어", "el", "", "el", "el");
            AddTransCode("nl", "네덜란드어", "nl", "", "nl", "nl");

            InitCustomTransCode();

            cbNaver.Items.Clear();
            cbNaverResult.Items.Clear();
            cbGoogle.Items.Clear();
            cbGoogleResult.Items.Clear();
            cbDeepL.Items.Clear();
            cbDeepLResult.Items.Clear();


            cbGoogleOcr.Items.Clear();
            cbGoogleOcr.Items.Add(LocalizeManager.LocalizeManager.GetLocalizeString("AUTO", "자동"));


            foreach (var obj in codeDataList)
            {
                //네이버 설정
                if (obj.naverCode != "")
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = obj.Title;
                    item.Value = obj;

                    cbNaver.Items.Add(item);

                    //네이버 결과
                    if (obj.naverCode == "ko")
                    {
                        cbNaverResult.Items.Insert(0, item);
                    }
                    else
                    {
                        cbNaverResult.Items.Add(item);
                    }
                }

                //구글
                if (obj.googleCode != "")
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = obj.Title;
                    item.Value = obj;

                    cbGoogle.Items.Add(item);

                    if (obj.googleCode == "ko")
                    {
                        cbGoogleResult.Items.Insert(0, item);
                    }
                    else
                    {
                        cbGoogleResult.Items.Add(item);
                    }
                }

                //디플
                if (obj.DeepLCode != "")
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = obj.Title;
                    item.Value = obj;

                    cbDeepL.Items.Add(item);

                    if (obj.DeepLCode == "ko")
                    {
                        cbDeepLResult.Items.Insert(0, item);
                    }
                    else
                    {
                        cbDeepLResult.Items.Add(item);
                    }
                }

                //구글 OCR
                if (obj.languageCode != "")
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = obj.Title;
                    item.Value = obj;

                    cbGoogleOcr.Items.Add(item);
                }
            }
        }

        public void SetState(NaverKeyData.eState state)
        {
            if (naverKeyList.Count == 0)
            {
            }
            else
            {
                if (naverKeyList.Count > currentNaverIndex)
                {
                    naverKeyList[currentNaverIndex].SetState(state, NaverTranslateAPI.instance.GetAPIType());
                }
            }
        }

        public NaverKeyData GetNextNaverKey()
        {
            NaverKeyData data = null;

            if (naverKeyList.Count == 0)
            {
                data = new NaverKeyData("", "");
            }
            else
            {
                currentNaverIndex++;
                if (currentNaverIndex >= naverKeyList.Count || currentNaverIndex == MAX_NAVER + 1)
                {
                    currentNaverIndex = 0;
                }

                data = naverKeyList[currentNaverIndex];
            }

            return data;
        }

        public List<NaverKeyData> SortNaverKeyList()
        {
            List<NaverKeyData> list = naverKeyList;

            List<NaverKeyData> freeList = new List<NaverKeyData>();
            List<NaverKeyData> paidList = new List<NaverKeyData>();

            foreach (var obj in list)
            {
                if (obj.isPaid)
                {
                    paidList.Add(obj);
                }
                else
                {
                    freeList.Add(obj);
                }
            }

            list.Clear();

            foreach (var obj in freeList)
            {
                list.Add(obj);
            }

            foreach (var obj in paidList)
            {
                list.Add(obj);
            }

            currentNaverIndex = 0;


            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].eNMTstate == NaverKeyData.eState.Normal)
                {
                    currentNaverIndex = i;
                    break;
                }
            }

            return list;
        }

        public void OpenNaverKeyFile()
        {
            currentNaverIndex = 0;
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = System.IO.Path.Combine(baseDir, GlobalDefine.NAVER_ACCOUNT_FILE);
                StreamReader r = new StreamReader(fullPath);

                string line;

                Dictionary<string, NaverKeyData> dataDic = new Dictionary<string, NaverKeyData>();

                for (int i = 0; i < naverKeyList.Count; i++)
                {
                    if (!dataDic.ContainsKey(naverKeyList[i].id))
                    {
                        dataDic.Add(naverKeyList[i].id, naverKeyList[i]);
                    }
                }

                naverKeyList.Clear();

                while ((line = r.ReadLine()) != null)
                {
                    string id = line;
                    string secret = "";
                    bool isPaid = false;
                    line = r.ReadLine();
                    if (line != null)
                    {
                        string[] keys = line.Split('\t');

                        if (keys.Length == 1)
                        {
                            secret = line;
                        }
                        else
                        {
                            if (keys.Length >= 2)
                            {
                                secret = keys[0];
                                isPaid = Convert.ToBoolean(keys[1]);
                            }
                        }


                        if (dataDic.ContainsKey(id) && dataDic[id].secret == secret)
                        {
                            naverKeyList.Add(dataDic[id]);
                        }
                        else
                        {
                            NaverKeyData data = new NaverKeyData(id, secret, isPaid);
                            naverKeyList.Add(data);
                        }
                    }
                    else
                    {
                        break;
                    }
                }


                for (int i = 0; i < naverKeyList.Count; i++)
                {
                    Util.ShowLog("id : " + naverKeyList[i].id + " / secret : " + naverKeyList[i].secret + " / isPaid : " + naverKeyList[i].isPaid);
                }

                r.Close();
                r.Dispose();
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.NAVER_ACCOUNT_FILE))
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }


        public void SaveNaverKeyFile(string id, string secret, bool isPaid = true)
        {
            id = id.Replace(" ", "");
            secret = secret.Replace(" ", "");
            string path = Util.ToCurrentFilePath(GlobalDefine.NAVER_ACCOUNT_FILE);

            try
            {
                using (StreamWriter newTask = new StreamWriter(path, false))
                {
                    newTask.WriteLine(id);
                    newTask.WriteLine(secret + "\t" + isPaid.ToString());

                    //첫 번째는 넘김.
                    for (int i = 1; i < naverKeyList.Count; i++)
                    {
                        newTask.WriteLine(naverKeyList[i].id);
                        newTask.WriteLine(naverKeyList[i].secret + "\t" + naverKeyList[i].isPaid.ToString());
                    }

                    if (naverKeyList.Count > 0)
                    {
                        naverKeyList[0].id = id;
                        naverKeyList[0].secret = secret;
                    }
                    else
                    {
                        naverKeyList.Add(new NaverKeyData(id, secret));
                    }

                    newTask.Close();
                }
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(path))
                {
                    fs.Close();
                    fs.Dispose();
                    using (StreamWriter newTask = new StreamWriter(path, false))
                    {
                        newTask.WriteLine(id);
                        newTask.WriteLine(secret + "\t" + isPaid.ToString());

                        for (int i = 1; i < naverKeyList.Count; i++)
                        {
                            newTask.WriteLine(naverKeyList[i].id);
                            newTask.WriteLine(naverKeyList[i].secret + "\t" + naverKeyList[i].isPaid.ToString());
                        }

                        if (naverKeyList.Count > 0)
                        {
                            naverKeyList[0].id = id;
                            naverKeyList[0].secret = secret;
                        }
                        else
                        {
                            naverKeyList.Add(new NaverKeyData(id, secret, isPaid));
                        }

                        newTask.Close();
                    }
                }
            }
        }

        public NaverKeyData GetNaverKey()
        {
            NaverKeyData data = null;

            if (naverKeyList.Count > 0)
            {
                data = naverKeyList[0];
            }
            else
            {
                data = new NaverKeyData("", "");
            }

            return data;
        }

        public void Dispose()
        {
            if (_ezTransPipeServer != null)
            {
                _ezTransPipeServer.Close();
            }
        }
    }
}