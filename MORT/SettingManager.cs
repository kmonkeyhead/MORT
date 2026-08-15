using R3;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MORT
{
    //TODO : 공용으로 서야한다
    public enum OcrLanguageType
    {
        None,
        English,
        Japen,
        Chinese_Simplified,
        Chinese_Traditional,
        Korean,
        Other,
    }

    public class QuickSettingData
    {
        public enum FontColorType
        {
            None,
            Black,
            White,
        }

        public class HSVData
        {
            public int startS;
            public int endS;

            public int startV;
            public int endV;

            public HSVData(int startS, int endS, int startV, int endV)
            {
                this.startS = startS;
                this.endS = endS;
                this.startV = startV;
                this.endV = endV;
            }
        }


        public FontColorType fontColorType = FontColorType.None;
        public SettingManager.TransType transType = SettingManager.TransType.google_url;
        public SettingManager.OcrType ocrType = SettingManager.OcrType.OneOcr;
        public OcrLanguageType languageType = OcrLanguageType.None;

        public string LanguageCode
        {
            get
            {
                string code = "";

                if (languageType == OcrLanguageType.English)
                {
                    code = "en";
                }
                else if (languageType == OcrLanguageType.Japen)
                {
                    code = "ja";
                }

                return code;
            }
        }

        public List<HSVData> HsvList
        {
            get
            {
                List<HSVData> list = new List<HSVData>();

                if (fontColorType == FontColorType.Black)
                {
                    HSVData data1 = new HSVData(0, 8, 0, 32);
                    HSVData data2 = new HSVData(95, 100, 0, 32);

                    list.Add(data1);
                    list.Add(data2);
                }
                else if (fontColorType == FontColorType.White)
                {
                    HSVData data1 = new HSVData(0, 10, 75, 100);

                    list.Add(data1);
                }

                return list;
            }
        }
    }

    public class SettingManager
    {
        public enum Skin
        {
            dark,
            layer,
            over
        }; //앞 소문자 바꾸며 안 됨! -> 기존 버전과 호환성

        public enum TransType
        {
            google_url,
            db,
            papago_web,
            naver,
            google,
            deepl,
            deeplApi,
            gemini,
            ezTrans,
            customApi
        }; //앞 소문자 바꾸며 안 됨! -> 기존 버전과 호환성

        public enum OcrType
        {
            Tesseract = 0,
            Window = 1,
            OneOcr = 2,
            Google = 3,
            EasyOcr = 4,
            Max = 5
        };

        public enum SortType
        {
            Normal,
            Center
        };

        public enum DeepLAPIEndpointType
        {
            Free = 0,
            Paid = 1
        };

        public OcrLanguageType OcrLanguageType = OcrLanguageType.English;

        TransType nowTransType;
        public string TranslateTypeSubKey { get; set; } = string.Empty;
        OcrType ocrType;
        Skin nowSkin;
        public bool nowIsUsePartialDB = false;
        string nowTessData = "eng";
        public Boolean nowIsFastTess = false;
        Boolean nowIsShowOcrReulstFlag = true;
        Boolean nowIsSaveOcrReulstFlag = false;

        //언어 관련.
        Boolean nowIsUseEngFlag = true;
        Boolean nowIsUseJpnFlag = false;
        Boolean nowIsUseOtherLangFlag = false;
        Boolean isUseStringUpper = false; //대소문자 구분 사용 안 함.


        string naverTransCode = "en";
        string naverResultCode = "ko";
        string naverApiType = MORT.NaverTranslateAPI.API_NMT;

        string googleTransCode = "en";
        string googleResultCode = "ko";

        public string DeepLTransCode { get; set; } = "en";
        public string DeepLResultCode { get; set; } = "en";

        public DeepLAPIEndpointType nowDeepLAPIEndpointType = DeepLAPIEndpointType.Free;


        //윈도우 10 관련
        string windowLanguageCode = "";
        public string EasyOcrCode { get; set; } = "en";

        Boolean nowIsSaveInClipboardFlag = false;
        int nowOCRSpeed = 3;
        string nowDBFile = "empty.txt";
        string nowDicFile = "myDic.txt";
        Boolean nowIsUseDicFileFlag = true;
        Boolean nowIsUseErodeFlag = false;

        public bool isUseMatchWordDic = true;
        int nowColorGroupCount = 1;
        Boolean nowIsUseRGBFlag = false;
        Boolean nowIsUseHSVFlag = false;

        public bool isUseThreshold = false;
        public int thresholdValue = 127;

        List<List<int>> useColorGroup = new List<List<int>>();
        List<int> quickOcrUseColorGroup = new List<int>();
        List<ColorGroup> nowColorGroup = new List<ColorGroup>();

        //OCR 영역 수
        int nowOCRGroupcount = 0;
        List<int> nowLocationXList = new List<int>();
        List<int> nowLocationYList = new List<int>();
        List<int> nowSizeXList = new List<int>();
        List<int> nowSizeYList = new List<int>();

        //레이어 번역창 위치
        public int transFormLocationX;
        public int transFormLocationY;
        public int transFormSizeX;
        public int transFormSizeY;

        //제외 영역 수.
        public int nowExceptionGroupCount = 0;
        public List<int> nowExceptionLocationXList = new List<int>();
        public List<int> nowExceptionLocationYList = new List<int>();
        public List<int> nowExceptionSizeXList = new List<int>();
        public List<int> nowExceptionSizeYList = new List<int>();

        SortType nowSortType = SortType.Normal;
        Boolean nowIsRemoveSpaceFlag = false;
        bool isShowOCRIndex = false;
        Boolean nowIsActiveWindow = false;
        Boolean nowIsUseBackColor = false;
        float imgZoomSize;

        Font textFont;
        Color textColor;
        Color outLineColor1;
        Color outLineColor2;
        Color backgroundColor;

        public static ReactiveProperty<bool> AttachedCapture = new ReactiveProperty<bool>(false);

        public bool IsUseAttachedCapture
        {
            get { return AttachedCapture.Value; }
            set { AttachedCapture.Value = value; }
        } //지정 캡쳐를 쓰고 있나? -> 저장은 안 함

        private bool isUseTTS = false;

        public bool IsUseTTS
        {
            get { return isUseTTS; }
            set { isUseTTS = value; }
        }

        private bool isWaitTTSEnd = false;

        public bool IsWaitTTSEnd
        {
            get { return isWaitTTSEnd; }
            set { isWaitTTSEnd = value; }
        }

        public bool isDebugMode = false;
        public static bool IsErrorEmptyGoogleToken = false;

        public bool IsShowOCRIndex
        {
            get { return isShowOCRIndex; }
            set { isShowOCRIndex = value; }
        }


        public Boolean NowIsUseBackColor
        {
            get { return nowIsUseBackColor; }
            set { nowIsUseBackColor = value; }
        }

        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public Color OutLineColor1
        {
            get { return outLineColor1; }
            set { outLineColor1 = value; }
        }

        public Color OutLineColor2
        {
            get { return outLineColor2; }
            set { outLineColor2 = value; }
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        public Font TextFont
        {
            get { return textFont; }

            set { textFont = value; }
        }

        public bool NowIsUseDicFileFlag
        {
            get { return nowIsUseDicFileFlag; }
            set { nowIsUseDicFileFlag = value; }
        }

        public string NowDicFile
        {
            get { return nowDicFile; }
            set { nowDicFile = value; }
        }

        public Skin NowSkin
        {
            get { return nowSkin; }
            set { nowSkin = value; }
        }

        public TransType NowTransType
        {
            get { return nowTransType; }
            set { nowTransType = value; }
        }

        public OcrType OCRType
        {
            get { return ocrType; }
            set { ocrType = value; }
        }

        public string NowTessData
        {
            get { return nowTessData; }
            set { nowTessData = value; }
        }

        public bool NowIsShowOcrResultFlag
        {
            get { return nowIsShowOcrReulstFlag; }
            set { nowIsShowOcrReulstFlag = value; }
        }

        public bool NowIsSaveOcrReulstFlag
        {
            get { return nowIsSaveOcrReulstFlag; }
            set { nowIsSaveOcrReulstFlag = value; }
        }

        public bool NowIsUseEngFlag
        {
            get { return nowIsUseEngFlag; }
            set { nowIsUseEngFlag = value; }
        }

        public bool NowIsUseJpnFlag
        {
            get { return nowIsUseJpnFlag; }
            set { nowIsUseJpnFlag = value; }
        }

        public bool NowIsUseOtherLangFlag
        {
            get { return nowIsUseOtherLangFlag; }
            set { nowIsUseOtherLangFlag = value; }
        }

        public bool IsUseStringUpper
        {
            get { return isUseStringUpper; }
            set { isUseStringUpper = value; }
        }

        public string NaverTransCode
        {
            get { return naverTransCode; }
            set { naverTransCode = value; }
        }

        public string NaverResultCode
        {
            get { return naverResultCode; }
            set { naverResultCode = value; }
        }

        public string NaverApiType
        {
            get { return naverApiType; }
            set { naverApiType = value; }
        }

        public string GoogleTransCode
        {
            get { return googleTransCode; }
            set { googleTransCode = value; }
        }

        public string GoogleResultCode
        {
            get { return googleResultCode; }
            set { googleResultCode = value; }
        }


        public string WindowLanguageCode
        {
            get { return windowLanguageCode; }
            set { windowLanguageCode = value; }
        }

        public bool NowIsSaveInClipboardFlag
        {
            get { return nowIsSaveInClipboardFlag; }
            set { nowIsSaveInClipboardFlag = value; }
        }

        public int NowOCRSpeed
        {
            get { return nowOCRSpeed; }
            set { nowOCRSpeed = value; }
        }

        public string NowDBFile
        {
            get { return nowDBFile; }
            set { nowDBFile = value; }
        }

        public bool NowIsUseErodeFlag
        {
            get { return nowIsUseErodeFlag; }
            set { nowIsUseErodeFlag = value; }
        }

        public List<List<int>> UseColorGroup
        {
            get { return useColorGroup; }
            set { useColorGroup = value; }
        }

        public List<int> QuickOcrUsecolorGroup
        {
            get { return quickOcrUseColorGroup; }
            set { quickOcrUseColorGroup = value; }
        }

        public int NowColorGroupCount
        {
            get { return nowColorGroupCount; }
            set { nowColorGroupCount = value; }
        }

        public List<ColorGroup> NowColorGroup
        {
            get
            {
                List<ColorGroup> list = new List<ColorGroup>(nowColorGroup);
                return list;
            }
            set
            {
                nowColorGroup.Clear();
                if (value != null)
                {
                    for (int i = 0; i < value.Count; i++)
                    {
                        nowColorGroup.Add(value[i]);
                    }
                }
            }
        }

        public int NowOCRGroupcount
        {
            get { return nowOCRGroupcount; }
            set { nowOCRGroupcount = value; }
        }

        public bool NowIsUseRGBFlag
        {
            get { return nowIsUseRGBFlag; }
            set { nowIsUseRGBFlag = value; }
        }

        public bool NowIsUseHSVFlag
        {
            get { return nowIsUseHSVFlag; }
            set { nowIsUseHSVFlag = value; }
        }


        public List<int> NowLocationXList
        {
            get { return nowLocationXList; }
            set { nowLocationXList = value; }
        }

        public List<int> NowLocationYList
        {
            get { return nowLocationYList; }
            set { nowLocationYList = value; }
        }

        public List<int> NowSizeXList
        {
            get { return nowSizeXList; }
            set { nowSizeXList = value; }
        }

        public List<int> NowSizeYList
        {
            get { return nowSizeYList; }
            set { nowSizeYList = value; }
        }

        public Rectangle LastSnapShotRect { get; set; }


        public SortType NowSortType
        {
            get { return nowSortType; }
            set { nowSortType = value; }
        }

        public Boolean NowIsRemoveSpace
        {
            get { return nowIsRemoveSpaceFlag; }
            set { nowIsRemoveSpaceFlag = value; }
        }

        public Boolean IsForceTransparency { get; set; }


        public Boolean NowIsActiveWindow
        {
            get { return nowIsActiveWindow; }
            set { nowIsActiveWindow = value; }
        }

        public float ImgZoomSize
        {
            get { return imgZoomSize; }
            set { imgZoomSize = value; }
        }

        public SettingManager()
        {
        }

        public Rectangle GetCaptureFullArea()
        {
            Rectangle resultRect = new Rectangle(1, 1, 1, 1);

            if (nowLocationXList.Count > 0)
            {
                int x = nowLocationXList[0];
                int y = nowLocationYList[0];
                int width = nowSizeXList[0];
                int height = nowSizeYList[0];

                resultRect = new Rectangle(x, y, width, height);

                for (int i = 1; i < nowLocationXList.Count; i++)
                {
                    x = nowLocationXList[i];
                    y = nowLocationYList[i];
                    width = nowSizeXList[i];
                    height = nowSizeYList[i];

                    var rect = new Rectangle(x, y, width, height);

                    resultRect = Rectangle.Union(resultRect, rect);
                }
            }


            return resultRect;
        }


        public int GetLocationX(int index)
        {
            int x = 0;

            int BorderWidth = FormManager.BorderWidth;
            int TitlebarHeight = FormManager.TitlebarHeight;

            if (nowLocationXList.Count > index)
            {
                x = nowLocationXList[index];
            }

            //x = Form1.testx;
            return x;
        }

        public int GetLocationY(int index)
        {
            int y = 0;

            int BorderWidth = FormManager.BorderWidth;
            int TitlebarHeight = FormManager.TitlebarHeight;

            if (nowLocationYList.Count > index)
            {
                y = nowLocationYList[index];
            }

            // y = Form1.testy;
            return y;
        }


        private void SaveLine(StreamWriter newTask, string key, string value)
        {
            newTask.WriteLine(key + " = @" + value, StringComparison.InvariantCulture);
        }

        public void saveSetting(string fileName)
        {
            try
            {
                using (StreamWriter newTask = new StreamWriter(fileName, false))
                {
                    //스킨
                    string skinString = "#SKIN = @" + nowSkin.ToString();
                    ;
                    newTask.WriteLine(skinString, StringComparison.InvariantCulture);

                    //tess 데이타
                    string tessDataString = "#TESS_DATA = @";
                    tessDataString = tessDataString + nowTessData;
                    newTask.WriteLine(tessDataString, StringComparison.InvariantCulture);

                    //빠른 테저렉 사용
                    string fastTess = "#USE_FAST_TESS = @";
                    fastTess = fastTess + nowIsFastTess.ToString();
                    newTask.WriteLine(fastTess, StringComparison.InvariantCulture);


                    //ocr 결과 보여주기
                    string showOCRResultString = "#SHOW_OCR_RESULT = @";
                    showOCRResultString = showOCRResultString + nowIsShowOcrReulstFlag.ToString();
                    newTask.WriteLine(showOCRResultString, StringComparison.InvariantCulture);

                    //ocr 결과 저장
                    string saveOCRResultString = "#SAVE_OCR_RESULT = @";
                    saveOCRResultString = saveOCRResultString + nowIsSaveOcrReulstFlag.ToString();
                    newTask.WriteLine(saveOCRResultString, StringComparison.InvariantCulture);

                    //문자 대소문자 구분 안함.
                    string useToUpper = "#STRING_TOUPPER = @" + isUseStringUpper.ToString();
                    newTask.WriteLine(useToUpper, StringComparison.InvariantCulture);

                    //영어 사용
                    string useEngString = "#USE_ENG = @" + nowIsUseEngFlag.ToString();
                    newTask.WriteLine(useEngString, StringComparison.InvariantCulture);

                    //일본어 사용
                    string useJpnString = "#USE_JPN = @" + nowIsUseJpnFlag.ToString();
                    newTask.WriteLine(useJpnString, StringComparison.InvariantCulture);

                    //다른 언어 사용
                    string useOtherLangString = "#USE_OTHER_LANG = @" + this.nowIsUseOtherLangFlag.ToString();
                    newTask.WriteLine(useOtherLangString, StringComparison.InvariantCulture);

                    string naverTransCodeString = "#NAVER_TRANS_CODE = @" + naverTransCode;
                    newTask.WriteLine(naverTransCodeString, StringComparison.InvariantCulture);

                    string naverResultCodeString = "#NAVER_RESULT_CODE = @" + naverResultCode;
                    newTask.WriteLine(naverResultCodeString, StringComparison.InvariantCulture);

                    string naverApiTypeString = "#NAVER_API_TYPE = @" + NaverApiType;
                    newTask.WriteLine(naverApiTypeString, StringComparison.InvariantCulture);

                    string googleTransCodeString = "#GOOGLE_TRANS_CODE = @" + googleTransCode;
                    newTask.WriteLine(googleTransCodeString, StringComparison.InvariantCulture);

                    string googleResultCodeString = "#GOOGLE_RESULT_CODE = @" + googleResultCode;
                    newTask.WriteLine(googleResultCodeString, StringComparison.InvariantCulture);

                    string deepLTransCodeString = "#DEEPL_TRANS_CODE = @" + DeepLTransCode;
                    newTask.WriteLine(deepLTransCodeString, StringComparison.InvariantCulture);

                    string deepLResultCodeString = "#DEEPL_RESULT_CODE = @" + DeepLResultCode;
                    newTask.WriteLine(deepLResultCodeString, StringComparison.InvariantCulture);

                    string windowLanguageCodeString = "#WINDOW_OCR_LANGUAGE = @" + windowLanguageCode;
                    newTask.WriteLine(windowLanguageCodeString, StringComparison.InvariantCulture);

                    string easyOcrCode = "#EASY_OCR = @" + EasyOcrCode;
                    newTask.WriteLine(easyOcrCode, StringComparison.InvariantCulture);

                    string mainOcrCode = "#OCR_CODE = @" + OcrLanguageType.ToString();
                    newTask.WriteLine(mainOcrCode, StringComparison.InvariantCulture);

                    string PartialDB = "#USE_PARTIAL_DB = @";
                    PartialDB = PartialDB + nowIsUsePartialDB.ToString();
                    newTask.WriteLine(PartialDB, StringComparison.InvariantCulture);


                    //클립보드에 저장
                    string saveInClipboardString = "#SAVE_IN_CLIPBOARD = @" + nowIsSaveInClipboardFlag.ToString();
                    newTask.WriteLine(saveInClipboardString, StringComparison.InvariantCulture);

                    //ocr 속도
                    string ocrSpeedString = "#OCR_SPEED = @" + nowOCRSpeed.ToString();
                    newTask.WriteLine(ocrSpeedString, StringComparison.InvariantCulture);

                    //OCR 방식
                    string ocrTypeString = "#OCR_TPYE = @" + ocrType.ToString();
                    newTask.WriteLine(ocrTypeString, StringComparison.InvariantCulture);

                    //번역 방식
                    string transTypeString = "#TRANS_TPYE = @" + nowTransType.ToString();
                    newTask.WriteLine(transTypeString, StringComparison.InvariantCulture);

                    //번역 방식 - 서브 키 -> 커스텀 api 전용
                    {
                        string key = $"#TRANS_TYPE_SUB = @{TranslateTypeSubKey}";
                        newTask.WriteLine(key, StringComparison.InvariantCulture);
                    }

                    // EndpointType
                    string endpointTypeString = "#ENDPOINT_TPYE = @" + nowDeepLAPIEndpointType.ToString();
                    newTask.WriteLine(endpointTypeString, StringComparison.InvariantCulture);

                    //db 파일
                    string dbFileString = "#DB_FILE = @" + nowDBFile;
                    newTask.WriteLine(dbFileString, StringComparison.InvariantCulture);

                    //dic 파일
                    string dicFileString = "#DIC_FILE =@" + nowDicFile;
                    newTask.WriteLine(dicFileString, StringComparison.InvariantCulture);

                    //dic 사용
                    string useDicFlagString = "#USE_DIC = @";
                    useDicFlagString = useDicFlagString + nowIsUseDicFileFlag.ToString();
                    newTask.WriteLine(useDicFlagString, StringComparison.InvariantCulture);

                    string useDicMatchString = "#MATCHING_WORD_DIC = @";
                    useDicMatchString = useDicMatchString + isUseMatchWordDic.ToString();
                    newTask.WriteLine(useDicMatchString, StringComparison.InvariantCulture);

                    //침식 사용
                    string useErodeString = "#USE_ERODE = @" + nowIsUseErodeFlag.ToString();
                    newTask.WriteLine(useErodeString, StringComparison.InvariantCulture);

                    //컬러 그룹
                    string colorGroupString = "#COLOR_GROUP = @" + nowColorGroupCount.ToString();
                    newTask.WriteLine(colorGroupString, StringComparison.InvariantCulture);

                    //RGB
                    string rgbString = "#RGB = @" + nowIsUseRGBFlag.ToString();
                    newTask.WriteLine(rgbString, StringComparison.InvariantCulture);

                    //rgb 값들
                    for (int i = 0; i < nowColorGroupCount; i++)
                    {
                        newTask.WriteLine(nowColorGroup[i].getValueR().ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowColorGroup[i].getValueG().ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowColorGroup[i].getValueB().ToString(), StringComparison.InvariantCulture);
                    }

                    //HSV
                    string hsvString = "#HSV = @" + nowIsUseHSVFlag.ToString();
                    newTask.WriteLine(hsvString);

                    for (int i = 0; i < nowColorGroupCount; i++)
                    {
                        newTask.WriteLine(nowColorGroup[i].getValueS1().ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowColorGroup[i].getValueS2().ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowColorGroup[i].getValueV1().ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowColorGroup[i].getValueV2().ToString(), StringComparison.InvariantCulture);
                    }

                    string thresHoldString = "#THRESHOLD = @" + isUseThreshold.ToString();
                    newTask.WriteLine(thresHoldString, StringComparison.InvariantCulture);

                    thresHoldString = "#THRESHOLD_VALUE = @" + thresholdValue.ToString();
                    newTask.WriteLine(thresHoldString, StringComparison.InvariantCulture);

                    //OCR 그룹
                    string ocrGroupString = "#OCR_GROUP = @" + nowOCRGroupcount.ToString();
                    newTask.WriteLine(ocrGroupString, StringComparison.InvariantCulture);

                    for (int i = 0; i < nowOCRGroupcount; i++)
                    {
                        newTask.WriteLine(nowLocationXList[i].ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowLocationYList[i].ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowSizeXList[i].ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowSizeYList[i].ToString(), StringComparison.InvariantCulture);
                    }

                    //OCR 제외 그룹
                    ocrGroupString = "#OCR_EXCEPTION_GROUP = @" + nowExceptionGroupCount.ToString();
                    newTask.WriteLine(ocrGroupString);

                    for (int i = 0; i < nowExceptionGroupCount; i++)
                    {
                        newTask.WriteLine(nowExceptionLocationXList[i].ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowExceptionLocationYList[i].ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowExceptionSizeXList[i].ToString(), StringComparison.InvariantCulture);
                        newTask.WriteLine(nowExceptionSizeYList[i].ToString(), StringComparison.InvariantCulture);
                    }


                    //OCR 사용하는 색그룹
                    string useColorGroupString = "#USE_OCR_COLOR_GROUP = @" + useColorGroup.Count.ToString();
                    newTask.WriteLine(useColorGroupString, StringComparison.InvariantCulture);

                    for (int i = 0; i < useColorGroup.Count; i++)
                    {
                        string data = "";
                        for (int j = 0; j < useColorGroup[i].Count; j++)
                        {
                            data += useColorGroup[i][j].ToString(); // +" ";
                            if (j + 1 != useColorGroup[i].Count)
                            {
                                data += " ";
                            }
                        }

                        newTask.WriteLine(data, StringComparison.InvariantCulture);
                    }

                    //텍스트 정렬
                    string textSort = "#TEXT_SORT = @" + nowSortType.ToString();
                    newTask.WriteLine(textSort, StringComparison.InvariantCulture);

                    //공백 제거
                    string removeSpace = "#USE_REMOVE_SPACE = @" + nowIsRemoveSpaceFlag.ToString();
                    newTask.WriteLine(removeSpace, StringComparison.InvariantCulture);

                    //OCR 인덱스
                    string showOCRIndex = "#SHOW_OCR_INDEX = @" + isShowOCRIndex.ToString();
                    newTask.WriteLine(showOCRIndex, StringComparison.InvariantCulture);

                    //폰트 이름
                    string fontName = "#FONT_NAME = @" + textFont.FontFamily.Name;
                    newTask.WriteLine(fontName, StringComparison.InvariantCulture);

                    string fontSize = "#FONT_SIZE = @" + textFont.Size.ToString();
                    newTask.WriteLine(fontSize, StringComparison.InvariantCulture);

                    //텍스트 컬러
                    string textColor = "#TEXT_COLOR = @";
                    newTask.WriteLine(textColor, StringComparison.InvariantCulture);
                    newTask.WriteLine(((int)(255)).ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.textColor.R.ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.textColor.G.ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.textColor.B.ToString(), StringComparison.InvariantCulture);

                    //외곽선1 컬러
                    string outline1 = "#OUTLINE1_COLOR = @";
                    newTask.WriteLine(outline1, StringComparison.InvariantCulture);
                    newTask.WriteLine(((int)(255)).ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.outLineColor1.R.ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.outLineColor1.G.ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.outLineColor1.B.ToString(), StringComparison.InvariantCulture);

                    //외곽선2 컬러
                    string outline2 = "#OUTLINE2_COLOR = @";
                    newTask.WriteLine(outline2, StringComparison.InvariantCulture);
                    newTask.WriteLine(((int)(255)).ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.outLineColor2.R.ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.outLineColor2.G.ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.outLineColor2.B.ToString(), StringComparison.InvariantCulture);

                    //배경 컬러
                    string backColor = "#BACK_COLOR = @" + this.nowIsUseBackColor.ToString();
                    newTask.WriteLine(backColor, StringComparison.InvariantCulture);
                    //newTask.WriteLine(((int)(255)).ToString());
                    newTask.WriteLine(this.backgroundColor.A.ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.backgroundColor.R.ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.backgroundColor.G.ToString(), StringComparison.InvariantCulture);
                    newTask.WriteLine(this.backgroundColor.B.ToString(), StringComparison.InvariantCulture);

                    //활성화 윈도우 이미지 가져오기
                    string attachWindow = "#USE_ACTIVE_WINDOW = @" + nowIsActiveWindow.ToString();
                    newTask.WriteLine(attachWindow, StringComparison.InvariantCulture);

                    //이미지 리사이즈 크기
                    string imgZoomSizeString = "#IMG_ZOOM_SIZE = @" + string.Format("{0:F1}", imgZoomSize, StringComparison.InvariantCulture);
                    newTask.WriteLine(imgZoomSizeString, StringComparison.InvariantCulture);


                    //TTS 사용 여부
                    string text = "#USE_TTS = @";
                    text = text + IsUseTTS.ToString();
                    newTask.WriteLine(text, StringComparison.InvariantCulture);

                    //TTS 대기 사용 여부
                    text = "#WAIT_TTS_END = @";
                    text = text + IsWaitTTSEnd.ToString();
                    newTask.WriteLine(text, StringComparison.InvariantCulture);

                    SaveLine(newTask, "#TRANS_LOCATION_X", transFormLocationX.ToString());
                    SaveLine(newTask, "#TRANS_LOCATION_Y", transFormLocationY.ToString());
                    SaveLine(newTask, "#TRANS_SIZE_X", transFormSizeX.ToString());
                    SaveLine(newTask, "#TRANS_SIZE_Y", transFormSizeY.ToString());

                    newTask.Close();
                }
            }
            catch (FileNotFoundException)
            {
                //FileNotFoundException
                using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                {
                    fs.Close();
                    fs.Dispose();
                }

                saveSetting(fileName);
            }
        }

        public string GetDefaultResultCode(bool isDeepl = false)
        {
            //TODO : 번역 코드에서 가져와야 한다 ISDEEPL이 아닌 번역 타입을 던져서 가져와야 한다
            switch (LocalizeManager.LocalizeManager.Language)
            {
                case LocalizeManager.AppLanguage.Auto:
                case LocalizeManager.AppLanguage.Korea:
                    return "ko";

                case LocalizeManager.AppLanguage.English:
                    return "en";

                case LocalizeManager.AppLanguage.Japanese:
                    return "ja";

                case LocalizeManager.AppLanguage.Indonesian:
                    return "id";

                case LocalizeManager.AppLanguage.SimplifiedChinese:

                    return isDeepl ? "ZH-HANS" : "zh-CN";

                case LocalizeManager.AppLanguage.Russian:
                    return "ru";

                default:
                    return "ko";
            }
        }


        public void SetDefault()
        {
            nowSkin = Skin.over;
            //nowTransType = TransType.db;
            nowTransType = TransType.google_url;
            TranslateTypeSubKey = string.Empty;
            ocrType = OcrType.OneOcr;
            nowTessData = "eng";
            nowIsFastTess = false;
            nowIsShowOcrReulstFlag = true;
            nowIsSaveOcrReulstFlag = false;
            isUseStringUpper = false;
            nowIsUseEngFlag = true;
            nowIsUseJpnFlag = false;
            nowIsUseOtherLangFlag = false;
            nowIsUsePartialDB = false;
            naverTransCode = "en";
            naverResultCode = GetDefaultResultCode();
            naverApiType = MORT.NaverTranslateAPI.API_NMT;

            googleTransCode = "en";
            googleResultCode = GetDefaultResultCode();

            DeepLTransCode = "en";
            DeepLResultCode = GetDefaultResultCode(true);

            EasyOcrCode = "en";
            OcrLanguageType = OcrLanguageType.English;

            windowLanguageCode = "";
            nowIsSaveInClipboardFlag = false;
            nowOCRSpeed = 2;
            nowDBFile = "empty.txt";
            nowDicFile = "myDic.txt";
            nowIsUseDicFileFlag = true;
            isUseMatchWordDic = true;
            nowIsUseErodeFlag = false;
            nowColorGroupCount = 1;
            nowColorGroup.Clear();
            nowColorGroup = new List<ColorGroup>();
            nowColorGroup.Add(new ColorGroup());
            nowOCRGroupcount = 0;
            quickOcrUseColorGroup.Clear();
            nowIsUseHSVFlag = false;
            nowIsUseRGBFlag = false;
            nowLocationXList.Clear();
            nowLocationXList = new List<int>();
            nowLocationYList.Clear();
            nowLocationYList = new List<int>();
            nowSizeXList.Clear();
            nowSizeXList = new List<int>();
            nowSizeYList.Clear();
            nowSizeYList = new List<int>();

            isUseThreshold = false;
            thresholdValue = 127;


            //제외 영역.
            nowExceptionGroupCount = 0;
            nowExceptionLocationXList.Clear();
            nowExceptionLocationXList = new List<int>();
            nowExceptionLocationYList.Clear();
            nowExceptionLocationYList = new List<int>();
            nowExceptionSizeXList.Clear();
            nowExceptionSizeXList = new List<int>();
            nowExceptionSizeYList.Clear();
            nowExceptionSizeYList = new List<int>();

            nowSortType = SortType.Normal;
            nowIsRemoveSpaceFlag = false;
            nowIsActiveWindow = false;
            nowIsUseBackColor = true; //old : false
            isShowOCRIndex = false;

            textFont = new Font("맑은 고딕", 15);
            textColor = new Color();
            outLineColor2 = new Color();
            outLineColor1 = new Color();
            backgroundColor = new Color();

            textColor = Color.FromArgb(255, 255, 255);
            outLineColor1 = Color.FromArgb(192, 192, 192); //old : 100 / 149 / 237
            outLineColor2 = Color.FromArgb(0, 0, 0); //old : 65 / 105 / 225
            backgroundColor = Color.FromArgb(170, 0, 0, 0); // 0,0,0
            imgZoomSize = 2;

            IsUseTTS = false;
            IsWaitTTSEnd = false;

            transFormLocationX = -1;
            transFormLocationY = -1;
            transFormSizeX = -1;
            transFormSizeY = -1;
        }

        //
        public void LoadSettingfile(string fileName)
        {
            bool isFoundMatchDic = false;
            SetDefault();
            try
            {
                StreamReader r = new StreamReader(fileName);
                string line;

                Util.ShowLog(r.ReadToEnd());
                r.Close();
                r = new StreamReader(fileName);


                while ((line = r.ReadLine()) != null)
                {
                    if (line.StartsWith("#SKIN", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("dark") == 0)
                            {
                                nowSkin = Skin.dark;
                            }
                            else if (resultString.CompareTo("layer") == 0)
                            {
                                nowSkin = Skin.layer;
                            }
                            else if (resultString.CompareTo("over") == 0)
                            {
                                nowSkin = Skin.over;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#TESS_DATA", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowTessData = resultString;
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }

                    else if (line.StartsWith("#USE_FAST_TESS", StringComparison.InvariantCulture))
                    {
                        ParseBoolData(line, ref nowIsFastTess);
                    }
                    else if (line.StartsWith("#SHOW_OCR_RESULT", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsShowOcrReulstFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsShowOcrReulstFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#SAVE_OCR_RESULT", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsSaveOcrReulstFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsSaveOcrReulstFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#STRING_TOUPPER", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                isUseStringUpper = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                isUseStringUpper = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#USE_JPN", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseJpnFlag = true;
                                nowIsUseOtherLangFlag = false;
                                nowIsUseEngFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseJpnFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }

                    else if (line.StartsWith("#USE_ENG", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseEngFlag = true;
                                nowIsUseJpnFlag = false;
                                nowIsUseOtherLangFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseEngFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }

                    else if (line.StartsWith("#USE_OTHER_LANG", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseOtherLangFlag = true;
                                nowIsUseEngFlag = false;
                                nowIsUseJpnFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseOtherLangFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }


                    else if (line.StartsWith("#NAVER_TRANS_CODE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            naverTransCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#NAVER_RESULT_CODE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            naverResultCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#NAVER_API_TYPE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            NaverApiType = resultString;
                        }
                    }
                    else if (line.StartsWith("#GOOGLE_TRANS_CODE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            googleTransCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#GOOGLE_RESULT_CODE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            googleResultCode = resultString;
                        }
                    }

                    else if (line.StartsWith("#DEEPL_TRANS_CODE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            DeepLTransCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#DEEPL_RESULT_CODE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            DeepLResultCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#EASY_OCR", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            EasyOcrCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#OCR_CODE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (Enum.TryParse<OcrLanguageType>(resultString, out var code))
                            {
                                OcrLanguageType = code;
                            }
                            else
                            {
                                OcrLanguageType = OcrLanguageType.English;
                            }
                        }
                    }
                    else if (line.StartsWith("#WINDOW_OCR_LANGUAGE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            windowLanguageCode = resultString;
                        }
                    }
                    else if (line.StartsWith("#SAVE_IN_CLIPBOARD", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsSaveInClipboardFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsSaveInClipboardFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#OCR_SPEED", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        ;
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowOCRSpeed = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#OCR_TPYE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("Tesseract") == 0)
                            {
                                ocrType = OcrType.Tesseract;
                            }
                            else if (resultString.CompareTo("Window") == 0)
                            {
                                ocrType = OcrType.Window;
                            }
                            else if (resultString.CompareTo("OneOcr") == 0)
                            {
                                ocrType = OcrType.OneOcr;
                            }
                            else if (resultString.CompareTo("Google") == 0)
                            {
                                ocrType = OcrType.Google;
                            }
                            else if (resultString.CompareTo("EasyOcr") == 0)
                            {
                                ocrType = OcrType.EasyOcr;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#TRANS_TPYE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("db") == 0)
                            {
                                nowTransType = TransType.db;
                            }
                            else if (resultString.CompareTo("naver") == 0)
                            {
                                nowTransType = TransType.naver;
                            }
                            else if (resultString.CompareTo("papago_web") == 0)
                            {
                                nowTransType = TransType.papago_web;
                            }
                            else if (resultString.CompareTo("google") == 0)
                            {
                                nowTransType = TransType.google;
                            }
                            else if (resultString.CompareTo("google_url") == 0)
                            {
                                nowTransType = TransType.google_url;
                            }
                            else if (resultString.CompareTo("deepl") == 0)
                            {
                                nowTransType = TransType.deepl;
                            }
                            else if (resultString.CompareTo("ezTrans") == 0)
                            {
                                nowTransType = TransType.ezTrans;
                            }
                            else if (resultString.CompareTo("customApi") == 0)
                            {
                                nowTransType = TransType.customApi;
                            }
                            else if (resultString.CompareTo("deeplApi") == 0)
                            {
                                nowTransType = TransType.deeplApi;
                            }
                            else if (resultString.CompareTo("gemini") == 0)
                            {
                                nowTransType = TransType.gemini;
                            }
                        }
                    }
                    else if (line.StartsWith("#TRANS_TYPE_SUB", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            TranslateTypeSubKey = resultString;
                        }
                    }
                    else if (line.StartsWith("#ENDPOINT_TPYE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("Free") == 0)
                            {
                                nowDeepLAPIEndpointType = DeepLAPIEndpointType.Free;
                            }
                            else if (resultString.CompareTo("Paid") == 0)
                            {
                                nowDeepLAPIEndpointType = DeepLAPIEndpointType.Paid;
                            }
                        }
                    }
                    else if (line.StartsWith("#DB_FILE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowDBFile = resultString;
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#DIC_FILE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowDicFile = resultString;
                        }
                    }
                    else if (line.StartsWith("#USE_DIC", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseDicFileFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseDicFileFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#MATCHING_WORD_DIC", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            isFoundMatchDic = true;
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                isUseMatchWordDic = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                isUseMatchWordDic = false;
                            }
                        }
                    }
                    else if (line.StartsWith("#USE_ERODE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseErodeFlag = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseErodeFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#COLOR_GROUP", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowColorGroupCount = Convert.ToInt32(resultString);
                            nowColorGroup.Clear();
                            for (int i = 0; i < nowColorGroupCount; i++)
                            {
                                nowColorGroup.Add(new ColorGroup());
                            }

                            quickOcrUseColorGroup.Clear();

                            for (int i = 0; i < nowColorGroupCount; i++)
                            {
                                quickOcrUseColorGroup.Add(1);
                            }
                        }
                    }
                    else if (line.StartsWith("#USE_OCR_COLOR_GROUP", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            int count = Convert.ToInt32(resultString);
                            useColorGroup.Clear();


                            for (int i = 0; i < count; i++)
                            {
                                useColorGroup.Add(new List<int>());

                                string[] data = (r.ReadLine()).Split(' ');
                                for (int j = 0; j < data.Length; j++)
                                {
                                    useColorGroup[i].Add(Convert.ToInt32(data[j]));
                                }
                            }
                        }
                    }

                    else if (line.StartsWith("#RGB", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseRGBFlag = true;
                                nowIsUseHSVFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseRGBFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }

                        for (int i = 0; i < nowColorGroupCount; i++)
                        {
                            int colorR = Convert.ToInt32(r.ReadLine());
                            int colorG = Convert.ToInt32(r.ReadLine());
                            int colorB = Convert.ToInt32(r.ReadLine());
                            nowColorGroup[i].setRGBValuse(colorR, colorG, colorB);
                        }
                    }
                    else if (line.StartsWith("#HSV", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseHSVFlag = true;
                                nowIsUseRGBFlag = false;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseHSVFlag = false;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }

                        for (int i = 0; i < nowColorGroupCount; i++)
                        {
                            int colorS1 = Convert.ToInt32(r.ReadLine());
                            int colorS2 = Convert.ToInt32(r.ReadLine());
                            int colorV1 = Convert.ToInt32(r.ReadLine());
                            int colorV2 = Convert.ToInt32(r.ReadLine());
                            nowColorGroup[i].setHSVValuse(colorS1, colorS2, colorV1, colorV2);
                            nowColorGroup[i].checkHSVRange();
                        }
                    }
                    else if (line.StartsWith("#THRESHOLD = @", StringComparison.InvariantCulture))
                    {
                        ParseBoolData(line, ref isUseThreshold);
                    }
                    else if (line.StartsWith("#THRESHOLD_VALUE = @", StringComparison.InvariantCulture))
                    {
                        ParseIntData(line, ref thresholdValue);
                    }
                    else if (line.StartsWith("#OCR_GROUP", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowOCRGroupcount = Convert.ToInt32(resultString);

                            for (int i = 0; i < nowOCRGroupcount; i++)
                            {
                                nowLocationXList.Add(Convert.ToInt32(r.ReadLine()));
                                nowLocationYList.Add(Convert.ToInt32(r.ReadLine()));
                                nowSizeXList.Add(Convert.ToInt32(r.ReadLine()));
                                nowSizeYList.Add(Convert.ToInt32(r.ReadLine()));
                            }
                        }
                    }
                    else if (line.StartsWith("#OCR_EXCEPTION_GROUP", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            nowExceptionGroupCount = Convert.ToInt32(resultString);

                            for (int i = 0; i < nowExceptionGroupCount; i++)
                            {
                                nowExceptionLocationXList.Add(Convert.ToInt32(r.ReadLine()));
                                nowExceptionLocationYList.Add(Convert.ToInt32(r.ReadLine()));
                                nowExceptionSizeXList.Add(Convert.ToInt32(r.ReadLine()));
                                nowExceptionSizeYList.Add(Convert.ToInt32(r.ReadLine()));
                            }
                        }
                    }
                    //폰트 색
                    else if (line.StartsWith("#TEXT_COLOR", StringComparison.InvariantCulture))
                    {
                        int colorA = Convert.ToInt32(r.ReadLine());
                        int colorR = Convert.ToInt32(r.ReadLine());
                        int colorG = Convert.ToInt32(r.ReadLine());
                        int colorB = Convert.ToInt32(r.ReadLine());

                        textColor = Color.FromArgb(colorR, colorG, colorB);
                    }

                    else if (line.StartsWith("#OUTLINE1_COLOR", StringComparison.InvariantCulture))
                    {
                        int colorA = Convert.ToInt32(r.ReadLine());
                        int colorR = Convert.ToInt32(r.ReadLine());
                        int colorG = Convert.ToInt32(r.ReadLine());
                        int colorB = Convert.ToInt32(r.ReadLine());

                        outLineColor1 = Color.FromArgb(colorR, colorG, colorB);
                    }

                    else if (line.StartsWith("#OUTLINE2_COLOR", StringComparison.InvariantCulture))
                    {
                        int colorA = Convert.ToInt32(r.ReadLine());
                        int colorR = Convert.ToInt32(r.ReadLine());
                        int colorG = Convert.ToInt32(r.ReadLine());
                        int colorB = Convert.ToInt32(r.ReadLine());

                        outLineColor2 = Color.FromArgb(colorR, colorG, colorB);
                    }

                    else if (line.StartsWith("#BACK_COLOR", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsUseBackColor = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsUseBackColor = false;
                            }
                        }

                        int colorA = Convert.ToInt32(r.ReadLine());
                        int colorR = Convert.ToInt32(r.ReadLine());
                        int colorG = Convert.ToInt32(r.ReadLine());
                        int colorB = Convert.ToInt32(r.ReadLine());

                        backgroundColor = Color.FromArgb(colorA, colorR, colorG, colorB);
                    }

                    else if (line.StartsWith("#FONT_NAME", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            textFont = new Font(resultString, textFont.Size);
                        }
                    }

                    else if (line.StartsWith("#FONT_SIZE", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            textFont = new Font(textFont.FontFamily, Convert.ToSingle(resultString));
                        }
                    }


                    else if (line.StartsWith("#TEXT_SORT", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("Normal") == 0)
                            {
                                nowSortType = SortType.Normal;
                            }
                            else if (resultString.CompareTo("Center") == 0)
                            {
                                nowSortType = SortType.Center;
                            }
                            //int reulst = Convert.ToInt32(resultString);
                        }
                    }
                    else if (line.StartsWith("#USE_PARTIAL_DB", StringComparison.InvariantCulture))
                    {
                        ParseBoolData(line, ref nowIsUsePartialDB);
                    }
                    else if (line.StartsWith("#USE_REMOVE_SPACE", StringComparison.InvariantCulture))
                    {
                        ParseBoolData(line, ref nowIsRemoveSpaceFlag);
                    }
                    else if (line.StartsWith("#SHOW_OCR_INDEX", StringComparison.InvariantCulture))
                    {
                        ParseBoolData(line, ref isShowOCRIndex);
                    }

                    else if (line.StartsWith("#USE_ACTIVE_WINDOW", StringComparison.InvariantCulture))
                    {
                        int index = line.IndexOf("@", StringComparison.InvariantCulture);
                        if (index != -1)
                        {
                            string resultString = line.Substring(index + 1);
                            if (resultString.CompareTo("True") == 0)
                            {
                                nowIsActiveWindow = true;
                            }
                            else if (resultString.CompareTo("False") == 0)
                            {
                                nowIsActiveWindow = false;
                            }
                        }
                    }
                    else if (line.StartsWith("#IMG_ZOOM_SIZE", StringComparison.InvariantCulture))
                    {
                        try
                        {
                            int index = line.IndexOf("@", StringComparison.InvariantCulture);
                            if (index != -1)
                            {
                                string imgZoomSizeString = line.Substring(index + 1);
                                imgZoomSize = (float)(Convert.ToDouble(imgZoomSizeString));

                                if (imgZoomSize > 10)
                                {
                                    imgZoomSize = 2;
                                }
                            }
                        }
                        catch
                        {
                            imgZoomSize = 2;
                        }
                    }
                    else if (line.StartsWith("#USE_TTS", StringComparison.InvariantCulture))
                    {
                        ParseBoolData(line, ref isUseTTS);
                    }
                    else if (line.StartsWith("#WAIT_TTS_END", StringComparison.InvariantCulture))
                    {
                        ParseBoolData(line, ref isWaitTTSEnd);
                    }
                    else if (line.StartsWith("#TRANS_LOCATION_X", StringComparison.InvariantCulture))
                    {
                        ParseIntData(line, ref transFormLocationX);
                    }
                    else if (line.StartsWith("#TRANS_LOCATION_Y", StringComparison.InvariantCulture))
                    {
                        ParseIntData(line, ref transFormLocationY);
                    }
                    else if (line.StartsWith("#TRANS_SIZE_X", StringComparison.InvariantCulture))
                    {
                        ParseIntData(line, ref transFormSizeX);
                    }
                    else if (line.StartsWith("#TRANS_SIZE_Y", StringComparison.InvariantCulture))
                    {
                        ParseIntData(line, ref transFormSizeY);
                    }
                }

                r.Close();
                r.Dispose();
            }
            catch (FileNotFoundException)
            {
                Util.ShowLog("File not Found");
                SetDefault();
            }
            catch (Exception e)
            {
                Util.ShowLog($"Exception {e}");
                SetDefault();
            }

            if (!isFoundMatchDic && NowIsUseJpnFlag)
            {
                isUseMatchWordDic = false;
            }

            Util.ShowLog("isUseMatchWordDic : " + isUseMatchWordDic);
        }

        private void ParseBoolData(string line, ref bool boolValue)
        {
            int index = line.IndexOf("@", StringComparison.InvariantCulture);
            if (index != -1)
            {
                string resultString = line.Substring(index + 1);
                if (resultString.CompareTo("True") == 0)
                {
                    boolValue = true;
                }
                else if (resultString.CompareTo("False") == 0)
                {
                    boolValue = false;
                }
            }
        }

        private void ParseIntData(string line, ref int value)
        {
            int index = line.IndexOf("@", StringComparison.InvariantCulture);
            if (index != -1)
            {
                string resultString = line.Substring(index + 1);
                value = Convert.ToInt32(resultString);
            }
        }

        public OcrLanguageType GetOcrLanguage()
        {
            OcrLanguageType result = OcrLanguageType.None;

            if (ocrType == OcrType.OneOcr)
            {
                result = OcrLanguageType;
            }
            else if (ocrType == OcrType.Tesseract)
            {
                if (nowIsUseEngFlag)
                {
                    result = OcrLanguageType.English;
                }
                else if (nowIsUseJpnFlag)
                {
                    result = OcrLanguageType.Japen;
                }
                else
                {
                    result = OcrLanguageType.Other;
                }
            }
            else if (ocrType == OcrType.Window)
            {
                if (Util.GetIsEqualMainOcrCode("en", windowLanguageCode))
                {
                    result = OcrLanguageType.English;
                }
                else if (Util.GetIsEqualMainOcrCode("ja", windowLanguageCode))
                {
                    result = OcrLanguageType.Japen;
                }
                else
                {
                    result = OcrLanguageType.Other;
                }
            }
            else if (ocrType == OcrType.EasyOcr)
            {
                if (Util.GetIsEqualMainOcrCode("en", EasyOcrCode))
                {
                    result = OcrLanguageType.English;
                }
                else if (Util.GetIsEqualMainOcrCode("ja", EasyOcrCode))
                {
                    result = OcrLanguageType.Japen;
                }
                else
                {
                    result = OcrLanguageType.Other;
                }
            }

            return result;
        }


        public static OcrType GetOcrType(int type)
        {
            OcrType result = (OcrType)type;

            return result;
        }
    }
}