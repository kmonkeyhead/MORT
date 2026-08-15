using MORT.SettingData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using MORT.Service.CustomApi;

namespace MORT
{
    public class HotKeyData
    {
        public int index;
        public KeyInputLabel.KeyType keyType;
        public List<Keys> keyList = new List<Keys>();
        public string extraData = "";
        public string keyResult = "";

        public HotKeyData()
        {

        }

        public HotKeyData(MORT.CustomControl.CtHotKey ctHotKey, string keyResult)
        {
            this.index = 0;
            this.keyType = ctHotKey.keyType;
            this.keyList = new List<Keys>();

            foreach (var obj in ctHotKey.GetKeys())
            {
                this.keyList.Add(obj);
            }

            this.keyResult = keyResult;
            this.extraData = "";
        }

        public HotKeyData(int index, KeyInputLabel.KeyType key, List<Keys> keyList, string keyResult, string extraData = "")
        {
            this.index = index;
            this.keyType = key;
            this.keyList = new List<Keys>();

            foreach(var obj in keyList)
            {
                this.keyList.Add(obj);
            }

            this.extraData = extraData;
            this.keyResult = keyResult;
        }

        public HotKeyData(int index, KeyInputLabel.KeyType key, string keyResult, string extraData = "")
        {
            this.index = index;
            this.keyType = key;
            this.keyList = new List<Keys>();
            this.keyResult = keyResult;

            char[] token = new char[] { ' ' };
            string[] keys = keyResult.Split(token, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    Keys test = (Keys)System.Enum.Parse(typeof(Keys), keys[i]);
                    keyList.Add(test);
                }

            }
            catch
            {
                keyList.Clear();
                this.keyResult = "";
            }


            this.extraData = extraData;

        }

    }

    public class AdvencedOptionManager
    {

        public const string TEMP_USER_TRANSLATION_DB_FILE = @"TEMP_USER_TRANSLATION.txt";

        public const int OPEN_SETTING_MAX = 4;

        public const string KEY_HOTKEY_FORMAT = "@HOTKEY_{0} ";
        public const string KEY_HOTKEY_OPEN_SETTING = "@HOTKEY_OPEN_SETTING_{0} ";
        public const string KEY_HOTKEY_FILE_PATH = "@HOTKEY_OPEN_FILE_PATH_{0} ";

        private const string KEY_USE_TOP_MOST_WHEN_TRANSLATE = "@USE_TOP_MOST_OPTION_WHEN_TRANSLATE ";
        private const string KEY_USE_IGONORE_EMPTY_TRANSLATE = "@USE_IGONORE_EMPTY_TRANSLATE ";
        private const string KeyEnableAdvencedHideTransform = "@EnableAdvencedHideTransform ";

        public const string KeyOverlayAutoMerge = "@OverlayAutoMerge ";
        public const string KeyOverlayKeepSourceDirection = "@OverlayKeepSourceDirection ";
        public const string KeyOverlayAutoColor = "@OverlayAutoColor ";
        public const string KeyOverlayAutoBackgroundColor = "@OverlayAutoBackgroundColor ";
        public const string KeyOverlayAutoFontColor = "@OverlayAutoFontColor ";
        public const string KeyOverlayUseFontOutline = "@OverlayUseFontOutline ";
        public const string KeyOverlayUseBackgroundAlpha = "@OverlayUseBackgroundAlpha ";
        public const string KEY_FONT_AUTO_SIZE = "@OVERLAY_FONT_AUTO_SIZE ";
        public const string KEY_FONT_AUTO_MIN_SIZE = "@OVERLAY_FONT_AUTO_MIN_SIZE ";
        public const string KEY_FONT_AUTO_MAX_SIZE = "@OVERLAY_FONT_AUTO_MAX_SIZE ";
        public const string KEY_SNAP_SHOP_REMAIN_TIEM = "@SNAP_SHOP_REMAIN_TIME ";
        public const string KeyLayerTextAlignmentBottom = "@LayerTextAlignmentBottom";
        public const string KeyLayerTextAlignmentRight = "@LayerTextAlignmentRight";


        public const string KEY_BASIC_FONT = "@BASIC_FONT ";

        public const string KEY_TRANSLATION_LIST = "@TRANSLATION_FILES ";   // <>로 해야한다
        public const string KEY_TRANSLATION_SEARCH_TYPE = "@TRANSLATION_SEARCH_TYPE ";      //번역집 검색 방식

        public const string KEY_TRANSLATOR_EXECUTIVE = "@TRANSLATOR_EXECUTIVE ";      //번역기 중역 사용

        public const string KEY_TRANSLATION_PARTIAL = "@TRANSLATION_PARTIAL ";              //번역집 부붕 검색
        public const string KEY_TRANSLATION_STRING_UPPER = "@TRANSLATION_STRING_UPPER ";    //번역집 대소문자 무시

        public const string KEY_DIC_REPROCESS_COUNT = "@DIC_REPROCESS_COUNT ";   //교정사전 추가 횟수

        public const string KEY_IS_USE_CLIPBOARD_TRANS = "@IS_USE_CLIPBOARD_TRANS ";
        public const string KEY_IS_SHOW_CLIPBOARD_ORIGINAL = "@IS_SHOW_CLIPBOARD_ORIGINAL ";
        public const string KEY_IS_SHOW_CLIPBOARD_PROICESSING = "@IS_SHOW_CLIPBOARD_PROICESSING ";  //클립보드 번역중 표시

        public const string KEY_USE_GOOGLE_OCR_PRIORTY = "@USE_GOOGLE_OCR_PRIORTY ";  //구글 OCR 우선 사용
        public const string KEY_GOOGLE_OCR_LIMIT = "@GOOGLE_OCR_LIMIT ";  //구글 OCR 한도

        public const string KEY_USE_DEEPL_ALT_OPTION = "@USE_DEEPL_ALT_OPTION";

        public const string KeyRttTopMost = "@EnableRttTopMost ";  //리모컨 최상단
        public const string KEY_ENABLE_SYSTEM_TRAY_MODE = "@ENABLE_SYSTEM_TRAY_MODE ";  //클립보드 번역중 표시
        public const string KEY_ENABLE_YELLOW_BORADER = "@ENABLE_YELLOW_BORADER ";  //활성화 된 윈도우에서 테두리 표시
        public const string KeyEnableRTL = "@EnableRTL ";
        public const string KeyUseLegacyOcrAreaFollowMouse = "@UseLegacyOcrAreaFollowMouse ";
        public const string KeyUseOnlyOcrAreaFollowMouse = "@UseOnlyOcrAreaFollowMouse ";
        public const string KeySelectOcrAreaBackgroundColor = "@SelectOcrAreaBackgroundColor";  //OCR 영역 선택 배경색
        public const string KeySelectOcrAreaColor = "@SelectOcrAreaColor";  //OCR 영역 선택 영역색

        public const string KeyClipboardSaveType = "@ClipboardSaveType ";

        public const string KeyEnableUseGoogleLanguageCode = "@ENABLE_USE_GOOGLE_LANGUAGE_CODE ";   //커스텀 api - 언어 코드 구글과 동일하게 처리
        public const string KeyCustomApiLanguageSource = "@CUSTOM_API_LANGUAGE_SOURCE ";
        public const string KeyCustomApiLanguageTarget = "@CUSTOM_API_LANGUAGE_TARGET ";
        public const string KeyCustomApiUrl = "@CUSTOM_API_URL ";

        public const string KeyGeminiCommand = @"GEMINI_COMMAND "; //Gemini API 명령어
        public const string KeyGeminiModel = @"GEMINI_MODEL "; //Gemini API 모델명
        public const string KeyGeminiIncludeDefaultCommand = @"GEMINI_INCLUDE_DEFAULT_COMMAND "; //Gemini API 기본 명령어 포함 여부
        public const string KeyGeminiPreset = @"GEMINI_PREST ";

        //번역 결과 기억하기
        public const string KeyEnableMemory = "@EnableMemory ";
        public const string KeyMemoryLimit = "@MemoryLimit ";
        public const string KeyMemoryRemainTime = "@MemoryRemainTime ";


        public class Data
        {
            public List<ISettingDataParse> ParseList = new List<ISettingDataParse>();
            public ISettingData<bool> BoolSettingData;

            //고급 단축키
            public ISettingData<List<HotKeyData>> hotKeyList;

            //번역창
            public ISettingData<bool> UseAutoSizeFont;
            public ISettingData<int> MinAutoSizeFont;
            public ISettingData<int> MaxAutoSizeFont;
            public ISettingData<int> SnapShotRemainTime;
            public ISettingData<bool> UseAutoMerge;
            public ISettingData<bool> OverlayKeepSourceDirection;
            public ISettingData<bool> OverlayAutoColor;
            public ISettingData<bool> OverlayAutoBackgroundColor;
            public ISettingData<bool> OverlayAutoFontColor;
            public ISettingData<bool> OverlayUseFontOutline;
            public ISettingData<bool> OverlayUseBackgroundAlpha;
            public ISettingData<bool> EnableAdvencedHideTransform;

            public ISettingData<string> BasicFont;

            //레이어 번역창
            public ISettingData<bool> LayerTextAlignmentBottom;
            public ISettingData<bool> LayerTextAlignmentRight;


            /// <summary>
            /// 번역할 때 만 탑 모스트 적용
            /// </summary>
            public ISettingData<bool> UseTopMostOptionWhenTranslate;
            /// <summary>
            /// 빈 번역 결과 무시
            /// </summary>
            public ISettingData<bool> UseIgnoreEmptyTranslate;

            //번역집
            public ISettingData<List<string>> translationFileList;
            public ISettingData<bool> isTranslationDbStyle;

            public ISettingData<bool> TranslationStringUpper;

            //일본어 중역
            public ISettingData<bool> IsExecutive;

            //딥플 - 에러시 구글 번역기 사용
            public ISettingData<bool> UseDeeplAltOption;

            //교정사전 추가 횟수
            public ISettingData<int> DicReProcessCount;

            //클립보드 번역 사용
            public ISettingData<bool> UseClipboardTrans;
            public ISettingData<bool> ShowClipboardOriginal;
            public ISettingData<bool> ShowClipboardProcessing;

            //OCR 설정
            public ISettingData<bool> UseGoogleOCRPriority;
            public ISettingData<int> GoogleOcrLimit;

            //클립보드 저장 타입
            public ISettingData<int> ClipboardSaveType;

            //앱 설정
            public ISettingData<bool> EnableRttTopMost;
            public ISettingData<bool> EnableSystemTrayMode;
            public ISettingData<bool> EnableYellowBorder;
            public ISettingData<string> SelectOcrAreaBackgroundColor;
            public ISettingData<string> SelectOcrAreaBackColor;
            public ISettingData<bool> EnableRTL;
            public ISettingData<bool> UseLegacyOcrAreaFollowMouse;
            public ISettingData<bool> UseOnlyOcrAreaFollowMouse;

            //커스터 api 설정
            public ISettingData<bool> UseGoogleLanguageCode;
            public ISettingData<string> CustomApiLanguageSource;
            public ISettingData<string> CustomApiLanguageTarget;
            public ISettingData<string> CustomApiUrl;

            //Gemini API 설정
            public ISettingData<string> GeminiCommand;
            public ISettingData<string> GeminiModel;
            public ISettingData<bool> GeminiDisableDefaultCommand;
            public ISettingData<GeminiPresetValue> GeminiPreset;
 

            //번역 결과 기억하기
            public ISettingData<bool> EnableTranslateMemory;
            public ISettingData<int> TranslateMemoryLimit;
            public ISettingData <int> TranslateMemoryRemainTime;

        }

        public static Data data = new Data();

        public static bool UseGoogleOCRPriority
        {
            set { data.UseGoogleOCRPriority.Value = value; }
            get { return data.UseGoogleOCRPriority.Value; }
        }

        public static bool EnableRttTopMost
        {
            set { data.EnableRttTopMost.Value = value; }
            get { return data.EnableRttTopMost.Value; }
        }

        public static bool EnableSystemTrayMode
        {
            set { data.EnableSystemTrayMode.Value = value; }
            get { return data.EnableSystemTrayMode.Value; }
        }

        public static bool EnableRTL
        {
            get { return data.EnableRTL?.Value ?? false; }
            set { data.EnableRTL.Value = value;}
        }

        public static bool UseLegacyOcrAreaFollowMouse
        {
            get { return data.UseLegacyOcrAreaFollowMouse?.Value ?? false; }
            set { data.UseLegacyOcrAreaFollowMouse.Value = value; }
        }

        public static bool UseOnlyOcrAreaFollowMouse
        {
            get { return data.UseOnlyOcrAreaFollowMouse?.Value ?? true; }
            set { data.UseOnlyOcrAreaFollowMouse.Value = value; }
        }


        public static bool EnableYellowBorder
        {
            set { data.EnableYellowBorder.Value = value; }
            get { return data.EnableYellowBorder.Value; }
        }

        public static Color SelectOcrAreaBackgroundColor
        {
            get
            {
                if (data.SelectOcrAreaBackgroundColor == null || string.IsNullOrEmpty(data.SelectOcrAreaBackgroundColor.Value))
                {
                    return Color.White;
                }

                string color = data.SelectOcrAreaBackgroundColor.Value;


                return Util.ParseColor(color, Color.White);
            }

            set
            {
                data.SelectOcrAreaBackgroundColor.Value = Util.ColorToString(value);
            }
        }

        public static Color SelectOcrAreaColor
        {
            get
            {
                if (data.SelectOcrAreaBackColor == null || string.IsNullOrEmpty(data.SelectOcrAreaBackColor.Value))
                {
                    return Color.Black;
                }

                string color = data.SelectOcrAreaBackColor.Value;

                return Util.ParseColor(color, Color.Black);
            }

            set
            {
                data.SelectOcrAreaBackColor.Value = Util.ColorToString(value);
            }
        }


        public static bool IsExecutive
        {
            set { data.IsExecutive.Value = value; }
            get { return data.IsExecutive.Value; }
        }

        public static bool UseDeeplAltOption
        {
            set { data.UseDeeplAltOption.Value = value; }
            get { return data.UseDeeplAltOption.Value; }
        }

        public static bool IsTranslationStringUpper
        {
            set { data.TranslationStringUpper.Value = value; }
            get { return data.TranslationStringUpper.Value ; }
        }

        public static List<string> TranslationFileList
        {
            set { data.translationFileList.Value = value; }
            get { return data.translationFileList.Value; }
        }

        public static bool IsTranslationDbStyle
        {
            set { data.isTranslationDbStyle.Value = value; }
            get { return data.isTranslationDbStyle.Value; }
        }

        public static int GoogleOcrLimit
        {
            set { data.GoogleOcrLimit.Value = value; }
            get { return data.GoogleOcrLimit.Value; }
        }

        public static bool LayerTextAlignmentBottom => data.LayerTextAlignmentBottom?.Value ?? false;
        public static bool LayerTextAlignmentRight => data.LayerTextAlignmentRight?.Value ?? false;
        public static bool EnableAdvencedHideTransform => data.EnableAdvencedHideTransform?.Value ?? false;

        public static bool UseTopMostOptionWhenTranslate => data.UseTopMostOptionWhenTranslate.Value;
        public static bool UseIgonoreEmptyTranslate => data.UseIgnoreEmptyTranslate.Value;

        public static string BasicFontData => data.BasicFont.Value;

        public static bool OverlayAutoColor => data.OverlayAutoColor.Value;
        public static bool OverlayAutoBackgroundColor => data.OverlayAutoBackgroundColor.Value;
        public static bool OverlayAutoFontColor => data.OverlayAutoFontColor.Value;
        public static bool OverlayUseFontOutline => data.OverlayUseFontOutline.Value;
        public static bool OverlayUseBackgroundAlpha => data.OverlayUseBackgroundAlpha.Value;
        public static bool UseAutoMerge => data.UseAutoMerge.Value;
        public static bool OverlayKeepSourceDirection => data.OverlayKeepSourceDirection.Value;
        public static bool IsAutoFontSize
        {
            get { return data.UseAutoSizeFont.Value; }
        }

        public static int MinAutoFontSize
        {
            get { return data.MinAutoSizeFont.Value; }
        }

        public static int MaxAutoFontSize
        {
            get { return data.MaxAutoSizeFont.Value; }
        }

        public static int SnapShopRemainTime
        {
            get => data.SnapShotRemainTime.Value;
            set => data.SnapShotRemainTime.Value = value;
        }


        public static float GetResultAutoFontSize(float fontSize)
        {
            float result = fontSize;
            if(fontSize > data.MaxAutoSizeFont.Value)
            {
                result = data.MaxAutoSizeFont.Value;
            }
            else if(fontSize < data.MinAutoSizeFont.Value)
            {
                result = data.MinAutoSizeFont.Value;
            }

            return result;
        }

        //커스텀 api 설정
        public static bool UseGoogleLanguageCode => data.UseGoogleLanguageCode.Value;
        public static string CustomApiLanguageSource => data.CustomApiLanguageSource.Value;
        public static string CustomApiLanguageTarget => data.CustomApiLanguageTarget.Value;
        public static string CustomApiUrl => data.CustomApiUrl.Value;

        //Gemini API 설정
        public static string GeminiCommand => data.GeminiCommand.Value;
        public static string GeminiModel => data.GeminiModel.Value;
        public static bool GeminiDisableDefaultCommand => data.GeminiDisableDefaultCommand.Value;
        public static GeminiPresetValue GeminiPreset => data.GeminiPreset.Value;
        


        //번역 결과 기억하기
        public static bool EnableTranslateMemory => data.EnableTranslateMemory.Value;
        public static int TranslateMemoryLimit => data.TranslateMemoryLimit.Value;
        public static int TranslateMemoryRemainTime => data.TranslateMemoryRemainTime.Value;

        public static void SetOverLay(bool isAutoSize, int minSize, int maxSize, int snapShotRemainTime, bool autoMerge, bool keepSourceDirection, bool autoColor, bool autoFontColor, bool autoBackgroundColor, bool useBackgroundAlpha)
        {
            data.UseAutoSizeFont.Value = isAutoSize;
            data.MinAutoSizeFont.Value = minSize;
            data.MaxAutoSizeFont.Value = maxSize;
            data.SnapShotRemainTime.Value = snapShotRemainTime;
            data.UseAutoMerge.Value = autoMerge;
            data.OverlayKeepSourceDirection.Value = keepSourceDirection;
            data.OverlayAutoColor.Value = autoColor;
            data.OverlayAutoBackgroundColor.Value = autoBackgroundColor;
            data.OverlayAutoFontColor.Value = autoFontColor;
            data.OverlayUseBackgroundAlpha.Value = useBackgroundAlpha;
        }

        public static void SetOverlayFontOutline(bool useFontOutline)
        {
            data.OverlayUseFontOutline.Value = useFontOutline;
        }

        public static void SetLayer(bool isAlignmentBottom, bool alignmentRight)
        {
            data.LayerTextAlignmentBottom.Value = isAlignmentBottom;
            data.LayerTextAlignmentRight.Value = alignmentRight;
        }

        public static void SetTranslationFormSetting(bool useTopMostOptionWhenTranslate, bool igonoreEmptyTranslate, string fontData, bool enableAdvencedHideTransform)
        {
            data.UseTopMostOptionWhenTranslate.Value = useTopMostOptionWhenTranslate;
            data.UseIgnoreEmptyTranslate.Value = igonoreEmptyTranslate;
            data.BasicFont.Value = fontData;
            data.EnableAdvencedHideTransform.Value = enableAdvencedHideTransform;
        }

        public static void SetTranslateMemory(bool enable, int limit, int time)
        {
            data.EnableTranslateMemory.Value = enable;
            data.TranslateMemoryLimit.Value = limit;
            data.TranslateMemoryRemainTime.Value = time;
        }

        /// <summary>
        /// 일본어 중역 사용
        /// </summary>
        /// <param name="isUse"></param>
        public static void SetExecutive(bool isUse)
        {
            data.IsExecutive.Value = isUse;
        }

        public static void SetDeeplOption(bool useAltOption)
        {
            data.UseDeeplAltOption.Value = useAltOption;
        }

        public static void SetCustomApiOption(bool useGoogleLanguageCode, string source, string target, string url)
        {
            data.UseGoogleLanguageCode.Value = useGoogleLanguageCode;
            data.CustomApiLanguageSource.Value = source;
            data.CustomApiLanguageTarget.Value = target;
            data.CustomApiUrl.Value = url;
        }

        public static void SetGeminiOption(string command, string model, bool disableDefaultCommand)
        {
            data.GeminiCommand.Value = command;
            data.GeminiModel.Value = model;
            data.GeminiDisableDefaultCommand.Value = disableDefaultCommand;
        }

        public static void SetGeminiPreset(GeminiPresetValue preset)
        {
            data.GeminiPreset.Value = preset;
        }


        public static int DicReProcessCount => data.DicReProcessCount.Value;

        public static void SetDicReProcessCount(int count)
        {
            if(count < 0)
            {
                count = 0;
            }
            else if(count > 3)
            {
                count = 3;
            }

            data.DicReProcessCount.Value = count;
        }

        //클립보드 설정
        public static bool IsUseClipboardTrans => data.UseClipboardTrans.Value;
        public static bool IsShowClipboardOriginal => data.ShowClipboardOriginal.Value;
        public static bool IsShowClipboardProcessing => data.ShowClipboardProcessing.Value;

        //클립보드에 저장
        public static int ClipboardSaveType => data.ClipboardSaveType.Value;


        public static void SetClipboardTrans(bool isUse, bool isShowOriginal, bool isShowProcessing)
        {
            data.UseClipboardTrans.Value = isUse;
            data.ShowClipboardOriginal.Value = isShowOriginal;
            data.ShowClipboardProcessing.Value = isShowProcessing;
        }

        public static void SetClipboardSave(int saveType)
        {
            data.ClipboardSaveType.Value = saveType;
        }


        public static void SetHotKey(List<HotKeyData> list)
        {
            data.hotKeyList.Value.Clear();
            data.hotKeyList.Value = list;
        }

        public static List<HotKeyData> GetHotKeyList()
        {
            return data.hotKeyList.Value;
        }

        public static HotKeyData GetHotKeyResult(List<Keys> inputList)
        {
            HotKeyData result = null;

            for(int i = 0; i < data.hotKeyList.Value.Count; i++)
            {
                if(KeyInputLabel.GetResult(inputList, data.hotKeyList.Value[i].keyList))
                {
                    result = data.hotKeyList.Value[i];
                    break;
                }
            }
            return result;
        }


        //파일에서 초기화 -> 한 번만 한다.
        public static void Init()
        {
            data.ParseList.Clear();
            data.hotKeyList = SettingDataFactory.CreateList<HotKeyData, HotKeySettingData>("", data.ParseList);

            //TODO : Load 가 맞나? Init가 더 적절해보임
            //번역창 설정
            LoadTranslationFormSetting();

            //앱 설정
            LoadAppSetting();

            //번역집
            LoadTranslationFileList();

            //번역기
            LoadTranslatorSetting();

            //교정사전
            LoadDicSetting();

            //클립보드 설정
            LoadClipboardSetting();

            //OCR 설정
            LoadOcrSetting();

            //클립보드 저장
            LoadClipboardSaveSetting();

            using (StreamReader r = Util.OpenFile(GlobalDefine.ADVENCED_SETTING_FILE))
            {
                //TODO : 파일이 없으면 초기화 루틴을 따로 불러줘야 한다
                string fileData = "";
                if (r != null)
                {
                    fileData = r.ReadToEnd();
                }

                r.Close();

                if(string.IsNullOrEmpty(fileData))
                {
                    Reset();
                }
                else
                {
                    foreach(var obj in data.ParseList)
                    {
                        obj.LoadValue(fileData);
                    }
                }
            }
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = System.IO.Path.Combine(baseDir, GlobalDefine.ADVENCED_TRANSRATION_PATH);

            if(!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        private static void LoadTranslationFileList()
        {
            data.translationFileList  = SettingDataFactory.CreateList<string, TranslationFileSettingData>(KEY_TRANSLATION_LIST, data.ParseList);
            data.isTranslationDbStyle = SettingDataFactory.Create<bool>(KEY_TRANSLATION_SEARCH_TYPE, data.ParseList, true);
            data.TranslationStringUpper = SettingDataFactory.Create<bool>(KEY_TRANSLATION_STRING_UPPER, data.ParseList, true);
        }

        private static void LoadTranslatorSetting()
        {
            data.IsExecutive = SettingDataFactory.Create<bool>(KEY_TRANSLATOR_EXECUTIVE, data.ParseList, false);
            data.UseDeeplAltOption = SettingDataFactory.Create<bool>(KEY_USE_DEEPL_ALT_OPTION, data.ParseList, true);

            data.UseGoogleLanguageCode = SettingDataFactory.Create<bool>(KeyEnableUseGoogleLanguageCode, data.ParseList, true);

            data.CustomApiLanguageSource = SettingDataFactory.Create<string>(KeyCustomApiLanguageSource, data.ParseList, "en");
            data.CustomApiLanguageTarget = SettingDataFactory.Create<string>(KeyCustomApiLanguageTarget, data.ParseList, "ko");
            data.CustomApiUrl = SettingDataFactory.Create<string>(KeyCustomApiUrl, data.ParseList, "http://localhost:8080/translator");

            data.GeminiCommand = SettingDataFactory.Create<string>(KeyGeminiCommand, data.ParseList, "");
            data.GeminiModel = SettingDataFactory.Create<string>(KeyGeminiModel, data.ParseList, "gemini-2.0-flash");
            data.GeminiDisableDefaultCommand = SettingDataFactory.Create<bool>(KeyGeminiIncludeDefaultCommand, data.ParseList, false);
            data.GeminiPreset = SettingDataFactory.Create(
                KeyGeminiPreset,
                data.ParseList,
                new GeminiPresetValue(temperature: 100, thinkingBudget: 0, tokenLimit: 1000, 0)
            );
        }

        private static void LoadDicSetting()
        {
            data.DicReProcessCount = SettingDataFactory.Create<int>(KEY_DIC_REPROCESS_COUNT, data.ParseList, 0);
        }

        private static void LoadTranslationFormSetting()
        {
            data.UseAutoMerge = SettingDataFactory.Create<bool>(KeyOverlayAutoMerge, data.ParseList, false);
            data.OverlayKeepSourceDirection = SettingDataFactory.Create<bool>(KeyOverlayKeepSourceDirection, data.ParseList, false);
            data.OverlayAutoColor = SettingDataFactory.Create<bool>(KeyOverlayAutoColor, data.ParseList, true);
            data.OverlayAutoBackgroundColor = SettingDataFactory.Create<bool>(KeyOverlayAutoBackgroundColor, data.ParseList, true);
            data.OverlayAutoFontColor = SettingDataFactory.Create<bool>(KeyOverlayAutoFontColor, data.ParseList, true);
            data.OverlayUseFontOutline = SettingDataFactory.Create<bool>(KeyOverlayUseFontOutline, data.ParseList, false);
            data.OverlayUseBackgroundAlpha = SettingDataFactory.Create<bool>(KeyOverlayUseBackgroundAlpha, data.ParseList, false);
            data.UseAutoSizeFont = SettingDataFactory.Create<bool>(KEY_FONT_AUTO_SIZE, data.ParseList, false);
            data.MinAutoSizeFont = SettingDataFactory.Create<int>(KEY_FONT_AUTO_MIN_SIZE, data.ParseList, 10);
            data.MaxAutoSizeFont = SettingDataFactory.Create<int>(KEY_FONT_AUTO_MAX_SIZE, data.ParseList, 50);
            data.SnapShotRemainTime = SettingDataFactory.Create<int>(KEY_SNAP_SHOP_REMAIN_TIEM, data.ParseList, 5);

            data.UseTopMostOptionWhenTranslate = SettingDataFactory.Create<bool>(KEY_USE_TOP_MOST_WHEN_TRANSLATE, data.ParseList, false);
            data.UseIgnoreEmptyTranslate = SettingDataFactory.Create<bool>(KEY_USE_IGONORE_EMPTY_TRANSLATE, data.ParseList, false);

            data.BasicFont = SettingDataFactory.Create<string>(KEY_BASIC_FONT, data.ParseList, "");

            data.LayerTextAlignmentBottom = SettingDataFactory.Create<bool>(KeyLayerTextAlignmentBottom, data.ParseList, false);
            data.LayerTextAlignmentRight = SettingDataFactory.Create<bool>(KeyLayerTextAlignmentRight, data.ParseList, false);
            data.EnableAdvencedHideTransform = SettingDataFactory.Create<bool>(KeyEnableAdvencedHideTransform, data.ParseList, false);

            //번역 결과 기억하기
            data.EnableTranslateMemory = SettingDataFactory.Create<bool>(KeyEnableMemory, data.ParseList, false);
            data.TranslateMemoryLimit = SettingDataFactory.Create<int>(KeyMemoryLimit, data.ParseList, 5);
            data.TranslateMemoryRemainTime = SettingDataFactory.Create<int>(KeyMemoryRemainTime, data.ParseList, 10);
        }

        private static void LoadAppSetting()
        {
            data.EnableRttTopMost = SettingDataFactory.Create<bool>(KeyRttTopMost, data.ParseList, false);
            data.EnableSystemTrayMode = SettingDataFactory.Create<bool>(KEY_ENABLE_SYSTEM_TRAY_MODE, data.ParseList, false);
            data.EnableYellowBorder = SettingDataFactory.Create<bool>(KEY_ENABLE_YELLOW_BORADER, data.ParseList, false);
            data.SelectOcrAreaBackColor = SettingDataFactory.Create<string>(KeySelectOcrAreaColor, data.ParseList, "");
            data.SelectOcrAreaBackgroundColor = SettingDataFactory.Create<string>(KeySelectOcrAreaBackgroundColor, data.ParseList, "");

            // TODO : 시스템 언어에 따라 다르게 해줘야 한다
            data.EnableRTL = SettingDataFactory.Create<bool>(KeyEnableRTL, data.ParseList, false);
            data.UseLegacyOcrAreaFollowMouse = SettingDataFactory.Create<bool>(KeyUseLegacyOcrAreaFollowMouse, data.ParseList, false);
            data.UseOnlyOcrAreaFollowMouse = SettingDataFactory.Create<bool>(KeyUseOnlyOcrAreaFollowMouse, data.ParseList, true);
        }

        private static void LoadClipboardSetting()
        {
            data.UseClipboardTrans = SettingDataFactory.Create<bool>(KEY_IS_USE_CLIPBOARD_TRANS, data.ParseList, false);
            data.ShowClipboardOriginal = SettingDataFactory.Create<bool>(KEY_IS_SHOW_CLIPBOARD_ORIGINAL, data.ParseList, false);
            data.ShowClipboardProcessing = SettingDataFactory.Create<bool>(KEY_IS_SHOW_CLIPBOARD_PROICESSING, data.ParseList, false);
        }

        private static void LoadClipboardSaveSetting()
        {
            data.ClipboardSaveType = SettingDataFactory.Create<int>(KeyClipboardSaveType, data.ParseList, 0);
        }

        private static void LoadOcrSetting()
        {
            data.UseGoogleOCRPriority = SettingDataFactory.Create<bool>(KEY_USE_GOOGLE_OCR_PRIORTY, data.ParseList, false);
            data.GoogleOcrLimit = SettingDataFactory.Create<int>(KEY_GOOGLE_OCR_LIMIT, data.ParseList, 950);
        }


        public static void Reset()
        {
            foreach (var obj in data.ParseList)
            {
                obj.SetDefault();
            }

            CultureInfo ci = CultureInfo.CurrentUICulture;
            switch(ci.TwoLetterISOLanguageName.ToUpper())
            {
                case "AR-SA":
                case "HE-IL":
                case "UR-PK":
                case "FA-IR":
                    EnableRTL = true;
                    break;
            }
        }

        public static void Save()
        {
            string result = "";


            foreach(var obj in data.ParseList)
            {
                result += obj.ToSave();
            }

            Util.SaveFile(GlobalDefine.ADVENCED_SETTING_FILE, result);
        }
    }

}
