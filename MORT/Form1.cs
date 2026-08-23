//만든이 : 몽키해드
//블로그 주소 : https://blog.naver.com/killkimno

using MORT.Logger;
using MORT.Manager;
using MORT.Model;
using MORT.OcrApi.WindowOcr;
using MORT.Service.Gemini;
using MORT.Service.MouseFollowOcrArea;
using MORT.Service.ProcessTranslateService;
using MORT.Service.PythonService;
using MORT.VersionCheck;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MORT.Service.TranslateTyp;
using static MORT.Manager.OcrManager;
using static MORT.TransManager;

namespace MORT
{
    public enum CurrentStateType
    {
        None,
        Init,
        LoadFile,
        SaveFile,
        Accept,
        SetDefault
    }

    public partial class Form1 : Form, IMainFormContract
    {
        private readonly TranslateTypListService _translateTypListService;
        public static bool IsLockHotKey = false;

        public class ImgData
        {
            public int channels;
            public int originalChannels;
            public int originalX;
            public int originalY;
            public byte[] data;
            public byte[] originalData = null;

            public int x;
            public int y;

            public int index;

            public bool UseAutoColor => originalData != null
                && originalChannels >= 3
                && originalX > 0
                && originalY > 0
                && originalData.LongLength >= (long)originalX * originalY * originalChannels;

            public void ClearList(List<byte> lista)
            {
                if (lista == null)
                {
                    return;
                }

                //todo
                //https://jacking75.github.io/NET_Span_5_Reasons_to_Use/ span을 이용 하던가
                //byte 를 intptr 로 보낸 후 마샬링 하던가 해야 함
                //IntPtr nativeMem = Marshal.AllocHGlobal(1000000);
                lista.Clear();
                int identificador = GC.GetGeneration(lista);
                GC.Collect(identificador, GCCollectionMode.Forced);

                lista.TrimExcess();
                lista = null;
            }

            public void Clear()
            {
                Array.Clear(data, 0, data.Length);

                int identificador = GC.GetGeneration(data);
                GC.Collect(identificador, GCCollectionMode.Forced);
                data = null;
            }

            public void ClearOriginalData()
            {
                if (originalData != null)
                {
                    Array.Clear(originalData, 0, originalData.Length);
                    int identificador = GC.GetGeneration(originalData);
                    GC.Collect(identificador, GCCollectionMode.Forced);
                    originalData = null;
                }
            }
        }


        #region:::::::::::::::::::::::::::::::::::::::::::Form level declarations:::::::::::::::::::::::::::::::::::::::::::

        private CurrentStateType _currentState = CurrentStateType.None;
        public CurrentStateType CurrentStateType => _currentState;

        public delegate void PDelegateSetSpellCheck();

        /// <summary>
        /// 현재 번역중 상태인가?
        /// </summary>
        private bool _processTrans = false;

        bool isTranslateFormTopMostFlag = true; //번역창이 최상위냐 아니냐

        private Point mousePoint; //창 이동 관련
        int _ocrProcessSpeed = 2000; //ocr 처리 딜레이 시간

        //폰트 관련
        Font textFont;
        Color textColor;
        Color outlineColor1;
        Color outlineColor2;
        Color backgroundColor;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsUseClipBoardFlag
        {
            set { MySettingManager.NowIsSaveInClipboardFlag = value; }

            get { return MySettingManager.NowIsSaveInClipboardFlag; }
        }

        int nowColorGroupIndex = 0; //색 그룹 수

        List<int> _locationXList = new List<int>();
        List<int> _locationYList = new List<int>();
        List<int> _sizeXList = new List<int>();
        List<int> _sizeYList = new List<int>();

        //제외 영역 관련.
        List<int> _exceptionLocationXList = new List<int>();
        List<int> _exceptionLocationYList = new List<int>();
        List<int> _exceptionSizeXList = new List<int>();
        List<int> _exceptionSizeYList = new List<int>();

        List<ColorGroup> colorGroup = new List<ColorGroup>(); //색 그룹 리스트

        string naverIDKey = "";
        string naverSecretKey = "";

        private List<string> winLanguageCodeList = new List<string>();

        public List<string> WinLanguageCodeList
        {
            get { return winLanguageCodeList; }
        }

        private List<OcrLanguageType> _ocrLanguages = Enum.GetValues<OcrLanguageType>().Where(v => v != OcrLanguageType.None).ToList();
        private OcrLanguageType _currentOcrLanguage;

        public bool Initialized => _initialized;
        public bool IsOcrAreaFollowMouse => _mouseFollowOcrAreaService?.IsEnabled ?? false;
        private bool _initialized = false; //초기화 완료
        public bool isAvailableWinOCR = true; //윈도우 10 OCR 사용 가능한지 확인.
        private string winOcrErrorCode = "";
        public bool isShowWinOCRWarning = false;
        public SettingManager MySettingManager = new SettingManager(); //설정 관리자
        GlobalKeyboardHook gHook;
        List<int> nowKeyPressList = new List<int>();

        public static bool IsDebugShowFormerResultLog = false;
        public static bool IsDebugTransOneLine = false;
        public static bool IsDebugShowWordArea = false;
        public static bool IsDebugSaveAnalysisResult = false;

        private List<KeyInputLabel> inputKeyUIList = new List<KeyInputLabel>();

        private ProcessTranslateService _processTranslateService;
        internal ProcessTranslateService ProcessTranslateService => _processTranslateService;
        private readonly MouseFollowOcrAreaService _mouseFollowOcrAreaService;

        #region ::::::::::::::::::::::::::DLL:::::::::::::::::::::::::::::::::::::::::::::::::

        //MORT_CORE 침식함수
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setErode();

        //MORT_CORE 내부 동작 함수
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void processOcr(StringBuilder test, StringBuilder test1);


        //MORT_CORE 내부 동작 함수 - 데이터도 같이 보냄
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void processOcrWithData(StringBuilder ocrBuilder, StringBuilder resultBuilder,
            int width, int height, int positionX, int positionY, [In, Out] [MarshalAs(UnmanagedType.LPArray)] byte[] data);


        //MORT_CORE 스펠링 체크
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessGetSpellingCheck(StringBuilder ocrResult, bool isUseJpn);

        //MORT_CORE DB만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessGetDBText(StringBuilder original, StringBuilder result);

        //MORT_CORE 이미지 데이터만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern System.IntPtr processGetImgData(int index, ref int x, ref int y, ref int channels, ref int locationX, ref int locationY, bool getOriginal);

        //MORT_CORE 이미지 데이터만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern System.IntPtr ProcessGetImgDataFromByte(int index, int width, int height, int positionX, int positionY,
            [In, Out] [MarshalAs(UnmanagedType.LPArray)]
            byte[] data, ref int x, ref int y, ref int channels, bool getOriginal);

        //MORT_CORE 이미지 영역 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setCutPoint(int[] newX, int[] newY, int[] newX2, int[] newY2, int size);

        //MORT_CORE 제외 영역 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetExceptPoint(int[] newX, int[] newY, int[] newX2, int[] newY2, int size);
        //

        //MORT_CORE 초기화
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initOcr();

        //MORT_CORE 폰트 교육자료 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setTessdata(string tessData, bool isUseJpnFlag);

        //MORT_CORE RGB, HSV 값 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setFiducialValue(int[] newValueR, int[] newValueG, int[] newValueB, int[] newValueS1, int[] newValueS2, int[] newValueV1, int[] newValueV2, int size);


        //디버그 모드 활성화.
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsDebugMode(bool isDebug, bool isShowReplace, bool isSaveCapture, bool isSaveCaptureResult);

        //MORT_CORE 빙 / DB 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setUseDB(bool newIsUseDBFlag, bool IsUsePartialDB, string newDbFileText);

        //MORT_CORE 교정 사전 사용
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setUseCheckSpelling(bool newIsUseCheckSpellingFlag, bool isMatchingWord, string newDicFileText);

        //MORT_CORE 이미지 보정 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setAdvencedImgOption(bool newIsUseRGBFlag, bool newIsUseHSVFlag, bool newIsUseErodeFlag, float imgZoomSize, bool isUseThreshold, int thresholdValue);

        //MORT_CORE isUseJPN 강제 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsUseJpn(bool _isUseJpn);

        //MORT_CORE isUseJPN 강제 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetReCheckSpellingCount(int _reCheckCount);

        //MORT_CORE 대소문자 구분 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsStringUpper(bool isUpper);

        //MORT_CORE 공백제거
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetRemoveSpace(bool isRemove);

        //MORT_CORE OCR 영역 인덱스 표시
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetShowOCRIndex(bool isShow);

        //MORT_CORE 활성화 윈도우 캡쳐 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsActiveWindow(bool isActiveWindow);

        //MORT_CORE 사용할 색그룹 추가
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddOcrColorSet(int[] colorList, int size);

        //MORT_CORE 사용할 색그룹 초기화
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ClearOcrColorSet();


        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }

            return null;
        }

        //TODO : 위치 변경하거나 해야함
        private WindowOcr loader = new WindowOcr();
        private readonly TranslateResultMemoryService _translateResultMemoryService = new TranslateResultMemoryService();
        private PythonModouleService _pythonService = new PythonModouleService();

        #endregion

        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::Skin Change Function:::::::::::::::::::::::::::::::::::::::::::

        public void ChangeSkin()
        {
            bool isChange = false;
            if (MySettingManager.NowSkin == SettingManager.Skin.dark && !skinDarkRadioButton.Checked)
            {
                isChange = true;
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer && !skinLayerRadioButton.Checked)
            {
                isChange = true;
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.over && !skinOverRadioButton.Checked)
            {
                isChange = true;
            }
            else if (_currentState == CurrentStateType.SetDefault || _currentState == CurrentStateType.LoadFile)
            {
                isChange = true;
            }

            if (isChange)
            {
                FormManager.Instace.DestoryTransForm();

                if (skinDarkRadioButton.Checked)
                {
                    MySettingManager.NowSkin = SettingManager.Skin.dark;
                }
                else if (skinLayerRadioButton.Checked)
                {
                    MySettingManager.NowSkin = SettingManager.Skin.layer;
                }
                else if (skinOverRadioButton.Checked)
                {
                    MySettingManager.NowSkin = SettingManager.Skin.over;
                }

                MakeTransForm();
            }


            //번역창 위치 설정.    처음 킬 때 , 설정 파일을 부를 때만 번역창 위치를 설정한다.
            SetTransFormLocation();
        }

        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::폼 생성 관련 함수:::::::::::::::::::::::::::::::::::::::::::

        private void MakeSearchOptionForm()
        {
            FormManager.Instace.MakeSearchOptionForm();
        }

        //리모컨 생성 함수
        private void MakeRTT()
        {
            FormManager.Instace.MakeRTT();
        }

        //교정사전 편집창 생성
        private void MakeDicEditorForm()
        {
            FormManager.Instace.MakeDicEditorForm(_processTranslateService.NowOcrString, MySettingManager.NowIsUseJpnFlag, MySettingManager.NowDicFile);
        }

        //번역창 생성 함수
        private void MakeLogo()
        {
            Logo logo = new Logo();
            logo.Name = "Logo";
            logo.StartPosition = FormStartPosition.CenterScreen;

            logo.Show();

            _versionCheckLogic.CheckVersion();

            if (!Program.IS_FORCE_QUITE)
            {
                _versionCheckLogic.CheckDefaultSetting();

                DateTime Tthen = DateTime.Now;
                do
                {
                    Application.DoEvents();
                } while (Tthen.AddSeconds(0.7f) > DateTime.Now);

                logo.disableLogo(2.0f);
            }
            else
            {
                logo.closeApplication();
                logo.Close();
            }
        }

        private void MakeTransForm()
        {
            if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                FormManager.Instace.MakeBasicTransForm(isTranslateFormTopMostFlag);
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                FormManager.Instace.MakeLayerTransForm(isTranslateFormTopMostFlag, _processTrans);
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                FormManager.Instace.MakeOverTransForm(isTranslateFormTopMostFlag, _processTrans);
            }
        }

        /// <summary>
        /// 번역창 위치 / 크기 설정
        /// </summary>
        private void SetTransFormLocation()
        {
            // 처음 킬 때 , 설정 파일을 부를 때만 번역창 위치를 설정한다.
            switch (_currentState)
            {
                case CurrentStateType.Init:
                case CurrentStateType.LoadFile:

                    if (MySettingManager.NowSkin == SettingManager.Skin.layer)
                    {
                        if (FormManager.Instace.MyLayerTransForm != null)
                        {
                            TransFormLayer transForm = FormManager.Instace.MyLayerTransForm;

                            int x = MySettingManager.transFormLocationX;
                            int y = MySettingManager.transFormLocationY;

                            int sizeX = MySettingManager.transFormSizeX;
                            int sizeY = MySettingManager.transFormSizeY;

                            if (sizeX < TransFormLayer.MIN_SIZE_X)
                            {
                                sizeX = TransFormLayer.MIN_SIZE_X;
                            }

                            if (sizeY < TransFormLayer.MIN_SIZE_Y)
                            {
                                sizeY = TransFormLayer.MIN_SIZE_Y;
                            }


                            //모두 -1이면 기본값이다.
                            if (x != -1 && y != -1 && sizeX != -1 && sizeY != -1)
                            {
                                //실제 모니터와 크기와 위치 보정하기
                                //Screen.PrimaryScreen.Bounds.Height
                                Rectangle rect = new Rectangle(x, y, sizeX, sizeY);
                                Screen screen = Screen.FromRectangle(rect);

                                bool isContain = false;
                                if (screen == null)
                                {
                                    Util.ShowLog("1Screen = null");
                                    isContain = false;
                                }
                                else
                                {
                                    Util.ShowLog("1Screen = not null " + screen.ToString());


                                    Rectangle Inter = Rectangle.Intersect(screen.Bounds, rect);
                                    if (Inter != null && Inter.Width > 0 && Inter.Height > 0)
                                    {
                                        isContain = true;
                                        //x축 검사.
                                        if (rect.X < screen.Bounds.X)
                                        {
                                            rect.X = screen.Bounds.X;
                                        }
                                        else if (screen.Bounds.X + screen.Bounds.Width < rect.X + rect.Width)
                                        {
                                            rect.X = (screen.Bounds.X + screen.Bounds.Width) - rect.Width;
                                        }

                                        //축 검사.
                                        if (rect.Y < screen.Bounds.Y)
                                        {
                                            rect.Y = screen.Bounds.Y;
                                        }
                                        else if (screen.Bounds.Y + screen.Bounds.Height < rect.Y + rect.Height)
                                        {
                                            rect.Y = (screen.Bounds.Y + screen.Bounds.Height) - rect.Height;
                                        }
                                    }
                                    else
                                    {
                                        Util.ShowLog("! not contain");
                                        isContain = false;
                                    }
                                }

                                if (!isContain)
                                {
                                    rect = new Rectangle(20, Screen.PrimaryScreen.Bounds.Height - 300, 973, 192);
                                }

                                transForm.Location = new Point(rect.X, rect.Y);
                                transForm.Size = new Size(rect.Width, rect.Height);
                            }
                        }
                    }

                    break;

                case CurrentStateType.SetDefault:

                    if (MySettingManager.NowSkin == SettingManager.Skin.layer)
                    {
                        if (FormManager.Instace.MyLayerTransForm != null)
                        {
                            TransFormLayer transForm = FormManager.Instace.MyLayerTransForm;
                            transForm.Location = new Point(20, Screen.PrimaryScreen.Bounds.Height - 300);
                            transForm.Size = new Size(973, 192);
                        }
                    }

                    break;

                case CurrentStateType.Accept:

                    if (MySettingManager.NowSkin == SettingManager.Skin.layer)
                    {
                        if (FormManager.Instace.MyLayerTransForm != null)
                        {
                            TransFormLayer transForm = FormManager.Instace.MyLayerTransForm;

                            Screen screen = Screen.FromControl(transForm);

                            if (screen == null)
                            {
                                Util.ShowLog("Screen = null");
                            }
                            else
                            {
                                Util.ShowLog("Screen = not null " + screen.ToString());
                            }
                        }
                    }

                    break;
            }
        }

        #endregion


        #region:::::::::::::::::::::::::::::::::::::::::::초기화:::::::::::::::::::::::::::::::::::::::::::

        private void CheckGDI()
        {
            TransFormLayer.isActiveGDI = true;
            CustomLabel.isActiveGDI = true;

            try
            {
                using (GraphicsPath gp = new GraphicsPath())
                using (StringFormat sf = new StringFormat())
                {
                    Font textFont = FormManager.Instace.MyMainForm.MySettingManager.TextFont;
                    gp.AddString("테스트, どうした 1234!", textFont.FontFamily, (int)textFont.Style, 10, ClientRectangle, sf);
                }
            }
            catch (Exception ex)
            {
                TransFormLayer.isActiveGDI = false;
                CustomLabel.isActiveGDI = false;
                if (DialogResult.OK == MessageBox.Show(LocalizeManager.LocalizeManager.GetLocalizeString("GDI Error Message"),
                        LocalizeManager.LocalizeManager.GetLocalizeString("GDI Error Title"), MessageBoxButtons.OKCancel))
                {
                    Util.OpenURL("https://blog.naver.com/killkimno/70185869419");
                }
            }
        }

        //파일로 부터 세팅 불러옴
        private void LoadSettingfile(string fileName)
        {
            MySettingManager.LoadSettingfile(fileName);
            SetValueToUIValue();
        }

        /// <summary>
        /// 파일 불러오기 -> 파일값 ui에 적용 -> ui 값을 실제 설정에 적용
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenSettingFile(string fileName)
        {
            _currentState = CurrentStateType.LoadFile;
            _processTranslateService.PauseAndRestartTranslate(() =>
            {
                LoadSettingfile(fileName);
                ApplyUIValueToSetting();
            });


            SaveSetting(GlobalDefine.USER_SETTING_FILE);
            _currentState = CurrentStateType.None;
        }

        //색 그룹 초기화
        private void InitColorGroup()
        {
            colorGroup.Clear();
            colorGroup.Add(new ColorGroup());
            groupCombo.Items.Clear();
            groupCombo.Items.Add(LocalizeString("Common Add"));
            groupCombo.Items.Add(LocalizeString("Common Remove"));
            groupCombo.Items.Add("1");
            groupCombo.SelectedIndex = 2;
        }

        //단축키를 위한 키 후킹 기능 초기화
        private void InitKeyHooker()
        {
            gHook = new GlobalKeyboardHook(); // Create a new GlobalKeyboardHook
            // Declare a KeyDown Event
            gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);
            gHook.KeyUp += new KeyEventHandler(gHook_KeyUp);
            // Add the keys you want to hook to the HookedKeys list
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                gHook.HookedKeys.Add(key);
            }

            gHook.hook();
        }

        private void InitializeTranslateType()
        {
            _cbTranslateType.Items.Clear();
            var titleList = _translateTypListService.GetTitles();
            _cbTranslateType.Items.AddRange(titleList.Cast<object>().ToArray());
        }

        private void UpdateTranslateType()
        {
            _translateTypListService.Initialize();
            // 1. _cbTranslateType의 목록을 _translateTypListService.GetTitles()에서 다시 가져온다
            var titles = _translateTypListService.GetTitles();
            var currentTransType = MySettingManager.NowTransType;

            _cbTranslateType.Items.Clear();
            foreach(var title in titles)
            {
                _cbTranslateType.Items.Add(title);
            }

            // 2. 만약 현재 사용하고 있는 번역 타입이 없으면 db 번역 타입으로 변경한다
            int newIndex = _translateTypListService.GetIndexByTransType(currentTransType, MySettingManager.TranslateTypeSubKey);
            if(newIndex == -1)
            {
                // 현재 번역 타입이 목록에 없으면 DB 번역으로 변경
                newIndex = _translateTypListService.GetIndexByTransType(SettingManager.TransType.db);
                if(newIndex != -1)
                {
                    MySettingManager.NowTransType = SettingManager.TransType.db;
                }
            }

            if(newIndex != -1 && newIndex < _cbTranslateType.Items.Count)
            {
                _cbTranslateType.SelectedIndex = newIndex;
            }
        }

        private void InitTransCode()
        {
            naverTransComboBox.SelectedIndex = 0;

            googleTransComboBox.SelectedIndex = 0;
            googleResultCodeComboBox.SelectedIndex = 0;
            cbDeepLLanguage.SelectedIndex = 0;
            cbDeepLLanguageTo.SelectedIndex = 0;

            cbGoogleOcrLanguge.SelectedIndex = 0;


            TransManager.Instace.InitTransCode(naverTransComboBox, cbNaverResultCode, googleTransComboBox, googleResultCodeComboBox,
                cbDeepLLanguage, cbDeepLLanguageTo, cbGoogleOcrLanguge);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();

        //폼 생성
        public Form1(GeminiConfigMaker geminiConfigMaker, TranslateTypListService translateTypListService, MouseFollowOcrAreaService mouseFollowOcrAreaService)
        {
            _translateTypListService = translateTypListService;
            _mouseFollowOcrAreaService = mouseFollowOcrAreaService;
            _versionCheckLogic = new VersionCheckLogic(this);
            //var logger = new LoggerForm();
            //logger.Show();
            try
            {
                _currentState = CurrentStateType.Init;

                //SetProcessDPIAware();
                InitializeComponent();
                _mouseFollowOcrAreaService.Initialize(this);

                this.Text = $"Monkeyhead's OCR RealTime Translator - {Properties.Settings.Default.MORT_VERSION}";

                plDebugOff.Visible = true;
                plDebugOn.Visible = false;

                FormManager.Instace.MyMainForm = this;
                notifyIcon1.Visible = false;

                NaverTranslateAPI.instance = new NaverTranslateAPI();
                GoogleBasicTranslateAPI.instance = new GoogleBasicTranslateAPI();

                isAvailableWinOCR = true;
                try
                {
                    InitLocalize();
                    _translateTypListService.Initialize();
                    InitializeTranslateType();
                    var codeList = loader.GetAvailableLanguageList();

                    WinOCR_Language_comboBox.Items.Clear();
                    for (int i = 0; i < codeList.Count; i++)
                    {
                        winLanguageCodeList.Add(codeList[i].Code);
                        WinOCR_Language_comboBox.Items.Add(codeList[i].DisplayName);
                    }

                    int installedCount = winLanguageCodeList.Count;

                    if (installedCount > 0)
                    {
                        WinOCR_Language_comboBox.SelectedIndex = 0;
                        loader.InitOcr(winLanguageCodeList[0]);
                    }
                    else
                    {
                        loader.InitOcr("");
                    }

                    if (codeList.Count == 0)
                    {
                        isAvailableWinOCR = false;
                    }
                }
                catch (Exception e)
                {
                    isAvailableWinOCR = false;
                    winOcrErrorCode = e.Message;
                }

                OcrManager.Instace.Init(_pythonService);

                cbEasyOcrCode.Items.Clear();
                for (int i = 0; i < OcrManager.Instace.EasyOcrCodeList.Count; i++)
                {
                    //TODO : 로컬라이징 필요
                    cbEasyOcrCode.Items.Add(OcrManager.Instace.EasyOcrCodeList[i]);
                }

                cbEasyOcrCode.LocalizeItems();
                if (OcrManager.Instace.EasyOcrCodeList.Count > 0)
                {
                    cbEasyOcrCode.SelectedIndex = 0;
                }

                cbOneOcrLanguage.Items.Clear();
                for (int i = 0; i < _ocrLanguages.Count; i++)
                {
                    //TODO : 로컬라이징 필요

                    cbOneOcrLanguage.Items.Add(Util.GetOcrLanguageCode(_ocrLanguages[i]));
                }

                cbOneOcrLanguage.LocalizeItems();

                cbGeminiModel.Items.Clear();
                for (int i = 0; i < geminiConfigMaker.GeminiConfigModels.Count; i++)
                {
                    cbGeminiModel.Items.Add(geminiConfigMaker.GeminiConfigModels[i].ModelName);
                }

                OpenNaverKeyFile();
                OpenGoogleKeyFile();
                OpenDeeplKeyFile();
                OpenGeminiKeyFile();


                TransManager.Instace.InitFormerDic();
                InitHotKey();
                InitTransCode();


                LoadSettingfile(GlobalDefine.USER_SETTING_FILE);
                initOcr();
                //GDI+ 동작 여부 검사.
                CheckGDI();

                _processTranslateService = new ProcessTranslateService(this, _translateResultMemoryService, MySettingManager, loader, isAvailableWinOCR, isOnceTrans => StopTrans(isOnceTrans));

                MakeLogo();


                if (!Program.IS_FORCE_QUITE)
                {
                    AdvencedOptionManager.Init();
                    MakeTransForm();
                    ApplyUIValueToSetting();

                    MakeRTT();
                    InitKeyHooker();

                    notifyIcon1.Visible = true;
                    _initialized = true;

                    ApplyAdvencedOption();
                }
                else
                {
                    this.Opacity = 0;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                Util.OpenURL("https://blog.naver.com/killkimno/70185869419");
                this.Close();
            }

            _currentState = CurrentStateType.None;
        }

        //폼이 불러온 후 처리함.
        private void Form1_Load(object sender, EventArgs e)
        {
            if (Program.IS_FORCE_QUITE)
            {
                this.Opacity = 0;
                this.Hide();
                return;
            }

            using (Graphics graphics = this.CreateGraphics())
            {
                Util.SetDPI(graphics.DpiX, graphics.DpiY);

                int width = tbMain.ItemSize.Width;
                int height = tbMain.ItemSize.Height;

                if (Util.dpiMulti > 1)
                {
                    width = width + (int)(Util.dpiMulti / 2 * width);
                    height = (int)(tbMain.ItemSize.Height * Util.dpiMulti);

                    tbMain.ItemSize = new Size(width, height);
                    pictureBox1.Size = new Size(height, pictureBox1.Height);
                }
            }

            //비활성화 -> 빠른 설정 탭으로
            if (!cbSetBasicDefaultPage.Checked)
            {
                tbMain.SelectedIndex = 5;
            }


            //툴팁 초기화.
            toolTip_OCR.SetToolTip(showOcrCheckBox, LocalizeManager.LocalizeManager.GetLocalizeString("TOOLTIP_SHOW_OCR_RESULT"));
            toolTip_OCR.SetToolTip(saveOCRCheckBox, LocalizeManager.LocalizeManager.GetLocalizeString("TOOLTIP_OCRSAVE"));
            toolTip_OCR.SetToolTip(isClipBoardcheckBox1, LocalizeManager.LocalizeManager.GetLocalizeString("TOOLTIP_CLIPBOARD"));
            toolTip_OCR.SetToolTip(checkDic, LocalizeManager.LocalizeManager.GetLocalizeString("TOOLTIP_DIC"));
            toolTip_OCR.SetToolTip(cbPerWordDic, LocalizeManager.LocalizeManager.GetLocalizeString("TOOLTIP_WORDDIC"));

            toolTip_OCR.SetToolTip(checkRGB, LocalizeManager.LocalizeManager.GetLocalizeString("TOOLTIP_RGB"));
            toolTip_OCR.SetToolTip(checkHSV, LocalizeManager.LocalizeManager.GetLocalizeString("TOOLTIP_HSV"));

            toolTip_OCR.SetToolTip(cbDBMultiGet, LocalizeManager.LocalizeManager.GetLocalizeString("TOOLTIP_MULTI_DB"));
            //OCR 영역 다시 초기화 함.

            FormManager.Instace.RefreshOCRAreaForm();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            GetIsHasError();
        }

        public bool GetIsHasError()
        {
            bool isError = false;

            if (SettingManager.IsErrorEmptyGoogleToken)
            {
                Logo.SetTopmost(false);
                if (MessageBox.Show(LocalizeManager.LocalizeManager.GetLocalizeString("Google Token Error"), LocalizeManager.LocalizeManager.GetLocalizeString("Google Token Title"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TransManager.Instace.InitGTransToken();
                }

                isError = true;
            }

            return isError;
        }

        #endregion


        #region::::::::::::::::::::::::::::::::::::::::::키 후킹::::::::::::::::::::::::::::::::::::::::::::::::::

        private void SaveHotKeyFile()
        {
            string result = "";
            foreach (var obj in inputKeyUIList)
            {
                string key = obj.GetKeyListToString();
                result += obj.keyType.ToString() + System.Environment.NewLine;
                result += key + System.Environment.NewLine + System.Environment.NewLine;
                ;
            }

            Util.SaveFile(GlobalDefine.HOTKEY_FILE, result);

            return;
        }

        private void InitHotKey()
        {
            transKeyInputLabel.keyType = KeyInputLabel.KeyType.Translate;
            dicKeyInputLabel.keyType = KeyInputLabel.KeyType.OpenDic;
            quickKeyInputLabel.keyType = KeyInputLabel.KeyType.QuickOCR;
            snapShotInputLabel.keyType = KeyInputLabel.KeyType.SnapShot;
            lbOneTrans.keyType = KeyInputLabel.KeyType.TranslateOnce;
            lbHideTranslate.keyType = KeyInputLabel.KeyType.Hide;
            lbFollowMouse.keyType = KeyInputLabel.KeyType.OcrAreaFollowMouse;

            inputKeyUIList.Add(transKeyInputLabel);
            inputKeyUIList.Add(dicKeyInputLabel);
            inputKeyUIList.Add(quickKeyInputLabel);
            inputKeyUIList.Add(snapShotInputLabel);
            inputKeyUIList.Add(lbOneTrans);
            inputKeyUIList.Add(lbHideTranslate);
            inputKeyUIList.Add(lbFollowMouse);


            foreach (var obj in inputKeyUIList)
            {
                obj.SetDefaultKey();
            }

            if (File.Exists(GlobalDefine.HOTKEY_FILE))
            {
                OpenHotKeyFile(inputKeyUIList);
            }
            else if (File.Exists(GlobalDefine.HOTKEY_FILE_OLD) || File.Exists(GlobalDefine.HOTKEY_FILE_OLD_V2))
            {
                OpenOldHotKeyFile();
            }

            if (File.Exists(GlobalDefine.HOTKEY_FILE_OLD))
            {
                File.Delete(GlobalDefine.HOTKEY_FILE_OLD);
            }

            if (File.Exists(GlobalDefine.HOTKEY_FILE_OLD_V2))
            {
                File.Delete(GlobalDefine.HOTKEY_FILE_OLD_V2);
            }
        }


        private void OpenHotKeyFile(List<KeyInputLabel> keyList)
        {
            try
            {
                var reader = Util.OpenFile(GlobalDefine.HOTKEY_FILE);

                if (reader != null)
                {
                    string data = reader.ReadToEnd();
                    reader.Close();

                    foreach (var obj in keyList)
                    {
                        string key = Util.GetNextLine(data, obj.keyType.ToString());
                        if (data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Any(line => line == obj.keyType.ToString()))
                        {
                            obj.SetKeyList(key);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 구버전 단축키를 불러온다
        /// </summary>
        private void OpenOldHotKeyFile()
        {
            try
            {
                bool isOldFile = false;
                string filePath = GlobalDefine.HOTKEY_FILE_OLD_V2;
                if (File.Exists(GlobalDefine.HOTKEY_FILE_OLD))
                {
                    filePath = GlobalDefine.HOTKEY_FILE_OLD;
                    isOldFile = true;
                }

                StreamReader r = new StreamReader(filePath);

                string line = r.ReadLine();

                if (line == null || line == "")
                {
                    line = "";
                    InitTansKey();
                }
                else
                {
                    transKeyInputLabel.SetKeyList(line);
                }

                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitDicKey();
                }
                else
                {
                    dicKeyInputLabel.SetKeyList(line);
                }

                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitQuickKey();
                }
                else
                {
                    quickKeyInputLabel.SetKeyList(line);
                }

                //스냅샷.
                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitSnapShotKey();
                }
                else
                {
                    snapShotInputLabel.SetKeyList(line);
                }

                //한 번만 번역하기.
                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitOneTranslateKey();
                }
                else
                {
                    lbOneTrans.SetKeyList(line);
                }

                //번역창 숨기기번역하기.
                line = r.ReadLine();
                if (line == null)
                {
                    line = "";
                    InitHideTransKey();
                }
                else
                {
                    lbHideTranslate.SetKeyList(line);
                }

                r.Close();
                r.Dispose();
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.HOTKEY_FILE_OLD_V2))
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        List<Keys> inputKeyList = new List<Keys>();

        public void gHook_KeyUp(object sender, KeyEventArgs e)
        {
            inputKeyList.Clear();
        }

        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsLockHotKey)
            {
                return;
            }

            Keys code = e.KeyCode;

            //테스트용
            if (e.KeyCode == Keys.V)
            {
                //loader.TextToSpeach("test");
                //Util.ShowLog("v");
            }

            //----

            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey)
            {
                code = Keys.ShiftKey;
            }
            else if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
            {
                code = Keys.ControlKey;
            }
            else if (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)
            {
                code = Keys.Menu;
            }

            if (transKeyInputLabel.isFocus || quickKeyInputLabel.isFocus || dicKeyInputLabel.isFocus || snapShotInputLabel.isFocus || lbOneTrans.isFocus || lbHideTranslate.isFocus || lbFollowMouse.isFocus)
            {
                return;
            }

            //이미 입력된 코드라면 무시한다
            if (inputKeyList.Any(r => r == code))
            {
                return;
            }

            inputKeyList.Add(code);

            //번역 시작.
            if (transKeyInputLabel.GetIsCorrect(inputKeyList))
            {
                if (_processTranslateService.IdleState)
                {
                    BeforeStartRealTimeTrans();
                }
                else if (_processTranslateService.ProcessingState)
                {
                    //저수준 키보드 훅에서 부르는 자리다. 오래 붙잡히면 훅이 제거된다
                    StopTrans(false, true);
                }
            }
            //한 번만 번역하기
            else if (lbOneTrans.GetIsCorrect(inputKeyList))
            {
                if (_processTranslateService.IdleState)
                {
                    SetCaptureArea();
                    StartTrnas(OcrMethodType.Once);
                }
                else if (_processTranslateService.ProcessingState)
                {
                    _processTranslateService.PauseAndRestartTranslate(SetCaptureArea, OcrMethodType.Once,
                        ProcessTranslateService.KeyHookJoinTimeoutMs);
                }
            }

            else if (quickKeyInputLabel.GetIsCorrect(inputKeyList))
            {
                //빠른 ocr 영역.
                FormManager.Instace.MakeQuickCaptureAreaForm();
            }
            else if (dicKeyInputLabel.GetIsCorrect(inputKeyList))
            {
                //교정사전 열기
                MakeDicEditorForm();
            }
            else if (snapShotInputLabel.GetIsCorrect(inputKeyList))
            {
                //스냅샷 열기
                MakeAndStartSnapShop();
            }
            else if (lbHideTranslate.GetIsCorrect(inputKeyList))
            {
                //스냅샷 열기
                FormManager.Instace.HideTransFrom();

                if (AdvencedOptionManager.EnableAdvencedHideTransform)
                {
                    if (_processTranslateService.IdleState)
                    {
                        BeforeStartRealTimeTrans();
                    }
                    else if (_processTranslateService.ProcessingState)
                    {
                        StopTrans();
                    }
                }
            }
            else if (lbFollowMouse.GetIsCorrect(inputKeyList))
            {
                ToggleOcrAreaFollowMouse();
            }
            else
            {
                HotKeyData data = AdvencedOptionManager.GetHotKeyResult(inputKeyList);
                //고급 단축키
                if (data != null)
                {
                    switch (data.keyType)
                    {
                        case KeyInputLabel.KeyType.OpenSetting:

                            Util.ShowLog("Open setting : " + data.extraData);
                            if (data.extraData != "")
                            {
                                string path = GlobalDefine.SETTING_PATH + data.extraData;

                                if (File.Exists(path))
                                {
                                    OpenSettingFile(path);
                                }
                                else
                                {
                                    string error = Properties.Settings.Default.FAIL_HOTKEY_OPEN_SETTING_FILE;
                                    error = string.Format(error, data.extraData);
                                    FormManager.Instace.AddText(error);
                                    Util.ShowLog("None");
                                }
                            }

                            break;

                        case KeyInputLabel.KeyType.LayerTransparency:
                            FormManager.Instace.SetForceTransparency(_processTrans);
                            break;

                        case KeyInputLabel.KeyType.DBTranslate:
                            ApplyTransTypeFromHotKey(SettingManager.TransType.db, LocalizeString("Switching DB"));
                            break;
                        case KeyInputLabel.KeyType.GoogleSheetTranslate:
                            ApplyTransTypeFromHotKey(SettingManager.TransType.google, LocalizeString("Switching Google Sheet"));
                            break;
                        case KeyInputLabel.KeyType.GoogleTranslate:
                            ApplyTransTypeFromHotKey(SettingManager.TransType.google_url, LocalizeString("Switching Basic"));
                            break;
                        case KeyInputLabel.KeyType.NaverTranslate:
                            ApplyTransTypeFromHotKey(SettingManager.TransType.naver, LocalizeString("Switching Papago"));
                            break;
                        case KeyInputLabel.KeyType.EzTrans:
                            ApplyTransTypeFromHotKey(SettingManager.TransType.ezTrans, LocalizeString("Switching ezTrans"));
                            break;
                        case KeyInputLabel.KeyType.DeepL:
                            ApplyTransTypeFromHotKey(SettingManager.TransType.deepl, LocalizeString("Switching DeepL"));
                            break;
                        case KeyInputLabel.KeyType.PapagoWeb:
                            ApplyTransTypeFromHotKey(SettingManager.TransType.papago_web, LocalizeString("Switching Papago Web"));
                            break;
                    }
                }
            }
        }

        private void ToggleOcrAreaFollowMouse()
        {
            _mouseFollowOcrAreaService.Toggle();
        }

        private bool IsUseOnlyOcrAreaFollowMouse()
        {
            return _mouseFollowOcrAreaService.IsUseOnlyOcrAreaFollowMouse;
        }

        public void ClearOcrAreaFollowMouseForm()
        {
            _mouseFollowOcrAreaService.ClearForm();
        }

        #endregion


        #region:::::::::::::::::::::::::::::::::::::::::::내부 동작 함수:::::::::::::::::::::::::::::::::::::::::::

        #region:::::::::::::::::::::::::::::::::::::::::::텍스트 설정 :::::::::::::::::::::::::::::::::::::::::::

        private void useBackColorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowResultFont();
        }

        private void removeSpaceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowResultFont();
        }

        private void alignmentCenterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShowResultFont();
        }

        private void cbShowOCRIndex_CheckedChanged(object sender, EventArgs e)
        {
            ShowResultFont();
        }

        private void ShowResultFont()
        {
            string first = FormManager.CUSTOM_LABEL_TEXT;
            string second = FormManager.CUSTOM_LABEL_TEXT2;


            if (this.removeSpaceCheckBox.Checked)
            {
                fontResultLabel.Text = first.Replace(" ", "");
                fontResultLabel.Text += second.Replace(" ", "");
            }
            else
            {
                fontResultLabel.Text = first;
                fontResultLabel.Text += second;
            }

            if (this.cbShowOCRIndex.Checked)
            {
                fontResultLabel.Text = string.Format(fontResultLabel.Text, "1. ", "2. ");
            }
            else
            {
                fontResultLabel.Text = string.Format(fontResultLabel.Text, "- ", "- ");
            }


            fontResultLabel.TextFont = this.textFont;
            fontResultLabel.TextColor = this.textColorBox.BackColor;
            fontResultLabel.OutlineForeColor = this.outlineColor1Box.BackColor;
            fontResultLabel.OutlineForecolor2 = this.outlineColor2Box.BackColor;
            fontResultLabel.BackColor = this.backgroundColorBox.BackColor;
            fontResultLabel.IsFillBackColor = useBackColorCheckBox.Checked;
            fontResultLabel.IsAlignmentCenter = alignmentCenterCheckBox.Checked;
            fontResultLabel.AlignmentRight = AdvencedOptionManager.LayerTextAlignmentRight;
            fontResultLabel.Refresh();
        }

        private void fontButton_Click(object sender, EventArgs e)
        {
            this.fontDialog.Font = textFont;
            try
            {
                DialogResult dr = this.fontDialog.ShowDialog();
                //확인버튼 누르면 변경
                if (dr == DialogResult.OK)
                {
                    textFont = this.fontDialog.Font;
                    int fontSize = (int)this.fontDialog.Font.Size;

                    if (fontSize > fontSizeUpDown.Maximum)
                        fontSize = (int)fontSizeUpDown.Maximum;
                    else if (fontSize < fontSizeUpDown.Minimum)
                        fontSize = (int)fontSizeUpDown.Minimum;

                    fontButton.Text = this.fontDialog.Font.FontFamily.Name;
                    fontSizeUpDown.Value = fontSize;

                    ShowResultFont();
                }
            }
            catch (System.ArgumentException ex)
            {
                MessageBox.Show("사용할 수 없는 폰트입니다");
            }
        }

        private void fontSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            textFont = new Font(textFont.FontFamily, (int)fontSizeUpDown.Value);
            ShowResultFont();
        }

        private void defaultColorButton_Click(object sender, EventArgs e)
        {
            textColor = new Color();
            outlineColor1 = new Color();
            outlineColor2 = new Color();
            backgroundColor = new Color();

            textColor = Color.FromArgb(255, 255, 255);
            outlineColor1 = Color.FromArgb(192, 192, 192); //old : 100 / 149 / 237
            outlineColor2 = Color.FromArgb(0, 0, 0); //old : 65 / 105 / 225
            backgroundColor = Color.FromArgb(170, 0, 0, 0); // 0,0,0

            SetColorBoxColor(textColorBox, textColor);
            SetColorBoxColor(outlineColor1Box, outlineColor1);
            SetColorBoxColor(outlineColor2Box, outlineColor2);
            SetColorBoxColor(backgroundColorBox, backgroundColor);

            ShowResultFont();
        }

        private void SetColorBoxColor(PictureBox obj, Color color)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;

            if (r == 0)
                r = 1;
            if (g == 0)
                g = 1;
            if (b == 0)
                b = 1;

            Color picturBoxColor = new Color();
            picturBoxColor = Color.FromArgb(r, g, b);

            obj.BackColor = picturBoxColor;

            ShowResultFont();
        }

        private void textColorBox_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = textColor;
            DialogResult dr = this.colorDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                textColor = this.colorDialog1.Color;
                SetColorBoxColor(textColorBox, this.colorDialog1.Color);
            }
        }

        private void outlineColor1Box_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = outlineColor1;
            DialogResult dr = this.colorDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                outlineColor1 = this.colorDialog1.Color;
                SetColorBoxColor(outlineColor1Box, this.colorDialog1.Color);
            }
        }

        private void outlineColor2Box_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = outlineColor2;
            DialogResult dr = this.colorDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                outlineColor2 = this.colorDialog1.Color;
                SetColorBoxColor(outlineColor2Box, this.colorDialog1.Color);
            }
        }

        private void backgroundColorBox_Click(object sender, EventArgs e)
        {
            Opulos.Core.UI.AlphaColorDialog acd = new Opulos.Core.UI.AlphaColorDialog();

            acd.ColorChanged += delegate { System.Diagnostics.Debug.WriteLine("Color changed: " + acd.Color); };

            acd.SetColor(backgroundColor);
            DialogResult dr2 = acd.ShowDialog();


            if (dr2 == DialogResult.OK)
            {
                Util.ShowLog("Result = " + acd.Color.A.ToString());
                backgroundColor = acd.Color;
                SetColorBoxColor(backgroundColorBox, acd.Color);
            }

            acd.Dispose();
        }

        #endregion

        #endregion

        //프로그램 닫기
        private void OnCloseApplication()
        {
            FormManager.Instace.SetTemporaryDisableTopMostTransform();
            if (MessageBox.Show(new Form { TopMost = true }, LocalizeString("Close App Message", true), LocalizeString("Close App Title"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseApplication();
            }

            FormManager.Instace.ResetTemporaryDisableTopMostTransform();
            return;
        }

        private void SetColorValueText(ColorGroup nowColorGroup)
        {
            rTextBox.Text = nowColorGroup.getValueR().ToString();
            gTextBox.Text = nowColorGroup.getValueG().ToString();
            bTextBox.Text = nowColorGroup.getValueB().ToString();

            v1TextBox.Text = nowColorGroup.getValueV1().ToString();
            v2TextBox.Text = nowColorGroup.getValueV2().ToString();
            s1TextBox.Text = nowColorGroup.getValueS1().ToString();
            s2TextBox.Text = nowColorGroup.getValueS2().ToString();
        }

        #region :::::::::: 번역 계정키 관련 ::::::::::

        private void SaveNaverKeyFile()
        {
            bool paidVersion = true;
            var data = TransManager.Instace.GetNaverKey();

            paidVersion = data.isPaid;

            TransManager.Instace.SaveNaverKeyFile(NaverIDKeyTextBox.Text, NaverSecretKeyTextBox.Text, paidVersion);
        }

        private void OpenNaverKeyFile()
        {
            try
            {
                TransManager.Instace.OpenNaverKeyFile();
                TransManager.Instace.SortNaverKeyList();

                TransManager.NaverKeyData data = TransManager.Instace.GetNaverKey();
                naverIDKey = data.id;
                NaverIDKeyTextBox.Text = data.id;
                naverSecretKey = data.secret;
                NaverSecretKeyTextBox.Text = data.secret;
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

        private void SaveGoogleKeyFile()
        {
            string path = Util.ToCurrentFilePath(GlobalDefine.GOOGLE_ACCOUNT_FILE);
            try
            {

                using (StreamWriter newTask = new StreamWriter(path, false))
                {
                    newTask.WriteLine(googleSheet_textBox.Text);
                    newTask.WriteLine(textBox_GoogleClientID.Text);
                    newTask.WriteLine(textBox_GoogleSecretKey.Text);
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
                        newTask.WriteLine(googleSheet_textBox.Text);
                        newTask.WriteLine(textBox_GoogleClientID.Text);
                        newTask.WriteLine(textBox_GoogleSecretKey.Text);
                        newTask.Close();
                    }
                }
            }
        }

        private void OpenGoogleKeyFile()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = System.IO.Path.Combine(baseDir, GlobalDefine.GOOGLE_ACCOUNT_FILE);
                StreamReader r = new StreamReader(fullPath);
                string line = r.ReadLine();
                TransManager.Instace.googleKey = line;
                googleSheet_textBox.Text = line;

                line = r.ReadLine();
                textBox_GoogleClientID.Text = line;
                line = r.ReadLine();
                textBox_GoogleSecretKey.Text = line;
                r.Close();
                r.Dispose();
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.GOOGLE_ACCOUNT_FILE))
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        private void OpenDeeplKeyFile()
        {
            using var reader = Util.OpenFile(GlobalDefine.DeeplApiFile);
            string line = reader.ReadLine();
            tbDeeplApi.Text = line;

            TransManager.Instace.InitDeeplApiKey(line);

            reader.Close();
        }

        private void SaveGeminiKeyFile()
        {
            string key = tbGeminiApi.Text;
            string modelName = cbGeminiModel.SelectedItem?.ToString() ?? "";

            Util.SaveFile(GlobalDefine.GeminiApiFile, key + System.Environment.NewLine + modelName);
        }

        private void OpenGeminiKeyFile()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string fullPath = System.IO.Path.Combine(baseDir, GlobalDefine.GeminiApiFile);
            if (!File.Exists(fullPath))
            {
                using (var fs = System.IO.File.Create(fullPath))
                {
                    fs.Close();
                }

                cbGeminiModel.SelectedIndex = 0;
                return;
            }

            try
            {
                using var reader = Util.OpenFile(GlobalDefine.GeminiApiFile);
                string api = reader.ReadLine();
                tbGeminiApi.Text = api;

                string model = reader.ReadLine();
                int index = cbGeminiModel.FindStringExact(model);

                if (index >= 0)
                {
                    cbGeminiModel.SelectedIndex = index;
                }
                else
                {
                    cbGeminiModel.SelectedIndex = 0; //기본값으로 설정
                }

                TransManager.Instace.InitializeGeminiModel(cbGeminiModel.SelectedItem.ToString(), api);
                reader.Close();
            }
            catch (Exception e)
            {
                cbGeminiModel.SelectedIndex = 0;
            }
        }

        #endregion


        public void ProcessTrans(OcrMethodType ocrMethodType) //번역 시작 쓰레드
        {
            _processTranslateService.OcrProcessSpeed = _ocrProcessSpeed;
            _processTranslateService.ProcessTrans(ocrMethodType);
        }


        #region:::::::::::::::::::::::::::::::::::::::::::외부 조작 함수:::::::::::::::::::::::::::::::::::::::::::

        public void SetUseColorGroup()
        {
            ClearOcrColorSet();
            if (IsUseOnlyOcrAreaFollowMouse())
            {
                AddOcrColorSet(MySettingManager.QuickOcrUsecolorGroup.ToArray(), MySettingManager.QuickOcrUsecolorGroup.Count);
                return;
            }

            for (int i = 0; i < MySettingManager.UseColorGroup.Count; i++)
            {
                AddOcrColorSet(MySettingManager.UseColorGroup[i].ToArray(), MySettingManager.UseColorGroup[i].Count);
            }

            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                AddOcrColorSet(MySettingManager.QuickOcrUsecolorGroup.ToArray(), MySettingManager.QuickOcrUsecolorGroup.Count);
            }

            if (FormManager.Instace.snapOcrAreaForm != null)
            {
                AddOcrColorSet(MySettingManager.QuickOcrUsecolorGroup.ToArray(), MySettingManager.QuickOcrUsecolorGroup.Count);
            }

            if (IsOcrAreaFollowMouse && FormManager.Instace.ocrAreaFollowMouseForm != null && FormManager.Instace.snapOcrAreaForm == null && !AdvencedOptionManager.UseLegacyOcrAreaFollowMouse)
            {
                AddOcrColorSet(MySettingManager.QuickOcrUsecolorGroup.ToArray(), MySettingManager.QuickOcrUsecolorGroup.Count);
            }
        }

        public void SetIsRemoveSpace(bool isRemoveSpace)
        {
            removeSpaceCheckBox.Checked = isRemoveSpace;
            MySettingManager.NowIsRemoveSpace = isRemoveSpace;
        }

        public void SetTextSort(SettingManager.SortType sortType)
        {
            if (sortType == SettingManager.SortType.Normal)
                alignmentCenterCheckBox.Checked = false;
            else
                alignmentCenterCheckBox.Checked = true;

            MySettingManager.NowSortType = sortType;
        }

        //스냅샷 위치 -> 바로 번역.
        public void MakeAndStartSnapShop()
        {
            Action callback = delegate
            {
                if (!MySettingManager.IsUseAttachedCapture && MySettingManager.NowIsActiveWindow)
                {
                    int waitCount = 0;

                    while (waitCount < 15)
                    {
                        Thread.Sleep(100);
                        string name = GetActiveWindowTitle();
                        waitCount++;
                        Util.ShowLog(name + "!!!!!!!!");
                        if (!(name == "OcrAreaForm" || name == "RTT"))
                        {
                            break;
                        }
                    }

                    if (waitCount >= 15)
                    {
                        FormManager.Instace.ForceUpdateText(LocalizeString("SnapShot Time Out"));
                        return;
                    }
                }


                this.BeginInvoke(new myDelegate(updateText), new object[] { LocalizeString("Translate Start") });

                SetCaptureArea();

                if (!_processTranslateService.PauseAndRestartTranslate(MakeTransForm, OcrMethodType.Snap))
                {
                    StartTrnas(OcrMethodType.Snap);
                }
            };

            FormManager.Instace.MakeSnapShotAreaForm(() => BeginInvoke(callback));
        }

        //델리게이트 이용
        public void ApplySpellCheck()
        {
            _processTranslateService.PauseAndRestartTranslate(() => setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.isUseMatchWordDic, MySettingManager.NowDicFile));
        }

        private void CloseApplication()
        {
            StopTrans();

            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Logo")
                {
                    Logo foundedForm = (Logo)frm;
                    foundedForm.closeApplication();
                    break;
                }
            }

            if (TransManager.Instace != null)
            {
                TransManager.Instace.Dispose();
            }

            this.Dispose();

            Application.Exit();
        }

        public void BeforeStartRealTimeTrans()
        {
            //스냅샷을 했을경우 ocr영역이 바뀌기 때문에 다시 설정해 줘야함.

            if (MySettingManager.LastSnapShotRect != Rectangle.Empty)
            {
                SetCaptureArea();
            }

            if (FormManager.Instace.GetOcrAreaCount() == 0)
            {
                if (MessageBox.Show(LocalizeString("Translate Start Error"), LocalizeString("Translate Start Error Title"), MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Util.OpenURL("https://blog.naver.com/killkimno/221904784013");
                }

                return;
            }

            //TODO : 함수로 따로 빼자

            if (MySettingManager.NowTransType == SettingManager.TransType.google_url && !TransManager.s_CheckedGoogleBasicWarning)
            {
                Action checkCallback = delegate
                {
                    FormManager.ShowTwoButtonPopupMessage(LocalizeString("Basic Translate Warning Title"), LocalizeString("Basic Translate Warning"),
                        () =>
                        {
                            TransManager.s_CheckedGoogleBasicWarning = true;
                            StartTrnas(OcrMethodType.Normal);
                        });
                };

                this.BeginInvoke(checkCallback);
            }
            else if (MySettingManager.NowTransType == SettingManager.TransType.papago_web && !TransManager.s_CheckedPapagoWebWarning)
            {
                Action checkCallback = delegate
                {
                    FormManager.ShowTwoButtonPopupMessage(LocalizeString("Papago Web Translate Warning Title"), LocalizeString("Papago Web Translate Warning"),
                        () =>
                        {
                            TransManager.s_CheckedPapagoWebWarning = true;
                            StartTrnas(OcrMethodType.Normal);
                        });
                };

                this.BeginInvoke(checkCallback);
            }
            else if (MySettingManager.NowTransType == SettingManager.TransType.deepl && !TransManager.s_CheckedDeeplWarning)
            {
                Action checkCallback = delegate
                {
                    FormManager.ShowTwoButtonPopupMessage(LocalizeString("DeepL Translate Warning Title"), LocalizeString("DeepL Translate Warning"),
                        () =>
                        {
                            TransManager.s_CheckedDeeplWarning = true;
                            StartTrnas(OcrMethodType.Normal);
                        });
                };

                this.BeginInvoke(checkCallback);
            }
            else
            {
                StartTrnas(OcrMethodType.Normal);
            }
        }

        public void StartTrnas(OcrMethodType ocrMethodType)
        {
            if (MySettingManager.OCRType == SettingManager.OcrType.Window && !isAvailableWinOCR)
            {
                MessageBox.Show(LocalizeString("Win OCR Error") + winOcrErrorCode);
                return;
            }

            if (MySettingManager.OCRType == SettingManager.OcrType.EasyOcr)
            {
                var required = OcrManager.Instace.CheckEasyOcrinstallationIsRequired();

                if (required)
                {
                    FormManager.ShowTwoButtonPopupMessage(LocalizeString("Easy OCR Require Install Title"), LocalizeString("Easy OCR Require Install Message"),
                        () => { FormManager.Instace.ShowEasyOcrInstaller(OcrManager.Instace, _pythonService); });
                    return;
                }
            }

            if (MySettingManager.OCRType == SettingManager.OcrType.Google && ocrMethodType == OcrMethodType.Normal)
            {
                MessageBox.Show(LocalizeString("Google Ocr Realtime Error"));
                return;
            }


            if (ocrMethodType != OcrMethodType.Snap)
            {
                //스냅샷 기록을 없앤다.
                MySettingManager.LastSnapShotRect = new Rectangle();
            }


            //오버레이 번역창 가능여부 체크.
            if (MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                bool isError = false;
                string errorMsg = "";
                //TODO : EASY OCR 도 지원해야 한다
                if (!(MySettingManager.OCRType == SettingManager.OcrType.Window || MySettingManager.OCRType == SettingManager.OcrType.Google ||
                      MySettingManager.OCRType == SettingManager.OcrType.EasyOcr || MySettingManager.OCRType == SettingManager.OcrType.OneOcr))
                {
                    isError = true;
                    errorMsg = LocalizeString("Overlay Error OCR");
                }

                if (isError)
                {
                    errorMsg += System.Environment.NewLine + LocalizeString("Overlay Error Guide Link");


                    if (MessageBox.Show(errorMsg, LocalizeString("Overlay Error Title"), MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Util.OpenURL("https://blog.naver.com/killkimno/222233614879");
                    }

                    return;
                }
            }

            if (FormManager.Instace.MySearchOptionForm != null)
            {
                FormManager.Instace.MySearchOptionForm.acceptCaptureArea();
            }

            _processTrans = true;
            FormManager.Instace.MyRemoteController.ToggleStartButton(true);

            ProcessTrans(ocrMethodType);

            if (ocrMethodType != OcrMethodType.Normal && !FormManager.Instace.MyMainForm.MySettingManager.IsForceTransparency)
            {
                if (!(MySettingManager.NowSkin == SettingManager.Skin.over && AdvencedOptionManager.SnapShopRemainTime > 0))
                {
                    _processTrans = false;
                }
            }

            MakeTransForm();
        }

        /// <param name="fromKeyHook">
        /// 저수준 키보드 훅에서 불렸는지. 훅 프로시저가 300ms 넘게 붙잡히면
        /// 윈도우가 훅을 제거해 이후 모든 단축키가 죽으므로 대기 시간을 짧게 잡는다.
        /// </param>
        public void StopTrans(bool isOnceTrans = false, bool fromKeyHook = false)
        {
            _processTrans = false;

            FormManager.Instace.MyRemoteController.ToggleStartButton(false);
            _processTranslateService.StopTranslate(fromKeyHook
                ? ProcessTranslateService.KeyHookJoinTimeoutMs
                : ProcessTranslateService.DefaultJoinTimeoutMs);

            var transform = FormManager.Instace.GetITransform();

            if (transform != null)
            {
                transform.StopTrans();
            }

            if (MySettingManager.NowSkin != SettingManager.Skin.dark)
            {
                //한번만 번역 & 강제 투명화 -> 번역이 끝나도 투명상태 유지.
                if (isOnceTrans)
                {
                    if (!FormManager.Instace.MyMainForm.MySettingManager.IsForceTransparency)
                    {
                        if (MySettingManager.NowSkin == SettingManager.Skin.over && AdvencedOptionManager.SnapShopRemainTime > 0)
                        {
                            FormManager.Instace.VisibleOverlayTrans(AdvencedOptionManager.SnapShopRemainTime);
                        }
                        else
                        {
                            FormManager.Instace.SetVisibleTrans();
                        }
                    }
                }
                else
                {
                    FormManager.Instace.SetVisibleTrans();
                }
            }

            if (FormManager.Instace.snapOcrAreaForm != null)
            {
                FormManager.Instace.snapOcrAreaForm.Close();
            }
        }

        //ocr 영역 적용

        private BackupOcrAreaModel _backupOcrAreaModel;

        public void RevertTempCaptureArea()
        {
            if (_backupOcrAreaModel == null)
            {
                return;
            }

            _exceptionLocationXList = _backupOcrAreaModel.ExceptionX;
            _exceptionLocationYList = _backupOcrAreaModel.ExceptionY;
            _exceptionSizeXList = _backupOcrAreaModel.ExceptionSizeX;
            _exceptionSizeYList = _backupOcrAreaModel.ExceptionSizeY;

            _locationXList = _backupOcrAreaModel.XList;
            _locationYList = _backupOcrAreaModel.YList;
            _sizeXList = _backupOcrAreaModel.SizeX;
            _sizeYList = _backupOcrAreaModel.SizeY;

            MySettingManager.NowOCRGroupcount = _locationYList.Count;
            MySettingManager.NowLocationXList = _locationXList;
            MySettingManager.NowLocationYList = _locationYList;
            MySettingManager.NowSizeXList = _sizeXList;
            MySettingManager.NowSizeYList = _sizeYList;

            _backupOcrAreaModel = null;
        }


        private void BackupCaptureArea()
        {
            if (_backupOcrAreaModel == null)
            {
                _backupOcrAreaModel = new BackupOcrAreaModel(MySettingManager.NowLocationXList, MySettingManager.NowLocationYList, MySettingManager.NowSizeXList, MySettingManager.NowSizeYList,
                    MySettingManager.nowExceptionLocationXList, MySettingManager.nowExceptionLocationYList, MySettingManager.nowExceptionSizeXList, MySettingManager.nowExceptionSizeYList);
            }
        }

        public void SetCaptureArea()
        {
            _backupOcrAreaModel = null;
            ApplyCaptureArea(true);
        }

        public void SetTempCaptureArea()
        {
            BackupCaptureArea();
            ApplyCaptureArea(false);
        }

        //TODO : Async 문으로 바꾸자
        private void ApplyCaptureArea(bool restartTranslateProcess)
        {
            int BorderWidth = Util.ocrFormBorder;
            int TitlebarHeight = Util.ocrFormTitleBar;

            FormManager.BorderWidth = Util.GetBorderWidth();
            FormManager.BorderHeight = +SystemInformation.FrameBorderSize.Height;
            FormManager.TitlebarHeight = Util.GetTitlebarHeight();
            _locationXList = new List<int>();
            _locationYList = new List<int>();
            _sizeXList = new List<int>();
            _sizeYList = new List<int>();

            _exceptionLocationXList.Clear();
            _exceptionLocationYList.Clear();
            _exceptionSizeXList.Clear();
            _exceptionSizeYList.Clear();

            List<int> tempXList = new List<int>();
            List<int> tempYList = new List<int>();
            List<int> tempSizeXList = new List<int>();
            List<int> tempSizeYList = new List<int>();
            bool useOnlyOcrAreaFollowMouse = IsUseOnlyOcrAreaFollowMouse();

            //2019 01 01
            //스냅샷이 있으면 모든걸 없애버린다.
            bool isSnapShot = false;
            if (FormManager.Instace.snapOcrAreaForm != null)
            {
                isSnapShot = true;
            }

            if (isSnapShot && !useOnlyOcrAreaFollowMouse)
            {
                //퀵 사이즈 전용.
                int quickX = 0;
                int quickY = 0;
                int quickSizeX = 0;
                int quickSizeY = 0;

                quickX = FormManager.Instace.snapOcrAreaForm.Location.X + BorderWidth;
                quickY = FormManager.Instace.snapOcrAreaForm.Location.Y + TitlebarHeight;
                quickSizeX = FormManager.Instace.snapOcrAreaForm.Size.Width - BorderWidth * 2;
                quickSizeY = FormManager.Instace.snapOcrAreaForm.Size.Height - TitlebarHeight - BorderWidth;

                tempXList.Add(quickX);
                tempYList.Add(quickY);
                tempSizeXList.Add(quickSizeX);
                tempSizeYList.Add(quickSizeY);

                MySettingManager.LastSnapShotRect = new Rectangle(quickX, quickY, quickSizeX, quickSizeY);
                ;
            }


            for (int i = 0; i < FormManager.Instace.OcrAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.OcrAreaFormList[i];

                int locationX = foundedForm.Location.X + BorderWidth;
                int locationY = foundedForm.Location.Y + TitlebarHeight;
                int sizeX = Math.Max(foundedForm.Size.Width - BorderWidth * 2, 1);
                int sizeY = Math.Max(foundedForm.Size.Height - TitlebarHeight - BorderWidth, 1);

                Util.ShowLog("!!!!! " + locationY + " size y : " + sizeY);
                _locationXList.Add(locationX);
                _locationYList.Add(locationY);
                _sizeXList.Add(sizeX);
                _sizeYList.Add(sizeY);

                if (!isSnapShot && !useOnlyOcrAreaFollowMouse)
                {
                    tempXList.Add(locationX);
                    tempYList.Add(locationY);
                    tempSizeXList.Add(sizeX);
                    tempSizeYList.Add(sizeY);
                }
            }

            //OCR 설정 저장용
            MySettingManager.NowOCRGroupcount = _locationYList.Count;
            MySettingManager.NowLocationXList = _locationXList;
            MySettingManager.NowLocationYList = _locationYList;
            MySettingManager.NowSizeXList = _sizeXList;
            MySettingManager.NowSizeYList = _sizeYList;


            //제외 영역 설정
            for (int i = 0; i < FormManager.Instace.exceptionAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.exceptionAreaFormList[i];

                int locationX = foundedForm.Location.X + BorderWidth;
                int locationY = foundedForm.Location.Y + TitlebarHeight;
                int sizeX = foundedForm.Size.Width - BorderWidth * 2;
                int sizeY = foundedForm.Size.Height - TitlebarHeight - BorderWidth;
                Util.ShowLog("Exception  " + locationY + " size y : " + sizeY);

                _exceptionLocationXList.Add(locationX);
                _exceptionLocationYList.Add(locationY);
                _exceptionSizeXList.Add(sizeX);
                _exceptionSizeYList.Add(sizeY);
            }


            if (FormManager.Instace.quickOcrAreaForm != null && !isSnapShot && !useOnlyOcrAreaFollowMouse)
            {
                //퀵 사이즈 전용.
                int quickX = 0;
                int quickY = 0;
                int quickSizeX = 0;
                int quickSizeY = 0;

                OcrAreaForm quickForm = FormManager.Instace.quickOcrAreaForm;

                quickX = quickForm.Location.X + BorderWidth;
                quickY = quickForm.Location.Y + TitlebarHeight;
                quickSizeX = quickForm.Size.Width - BorderWidth * 2;
                quickSizeY = quickForm.Size.Height - TitlebarHeight - BorderWidth;

                tempXList.Add(quickX);
                tempYList.Add(quickY);
                tempSizeXList.Add(quickSizeX);
                tempSizeYList.Add(quickSizeY);
            }

            if (IsOcrAreaFollowMouse && FormManager.Instace.ocrAreaFollowMouseForm != null && !FormManager.Instace.ocrAreaFollowMouseForm.IsDisposed && (!isSnapShot || useOnlyOcrAreaFollowMouse) && !AdvencedOptionManager.UseLegacyOcrAreaFollowMouse)
            {
                Rectangle mouseFollowRect = GetOcrAreaFormProcessRect(FormManager.Instace.ocrAreaFollowMouseForm);
                if (mouseFollowRect != Rectangle.Empty)
                {
                    tempXList.Add(mouseFollowRect.X);
                    tempYList.Add(mouseFollowRect.Y);
                    tempSizeXList.Add(mouseFollowRect.Width);
                    tempSizeYList.Add(mouseFollowRect.Height);
                }
            }

            for (int i = 0; i < tempSizeXList.Count; i++)
            {
                //TODO : stride로 처리해야 한다
                // x 는 4의 배수로 만들어야 한다
                int addPixel = 4 - tempSizeXList[i] % 4;

                if (addPixel != 4)
                {
                    tempSizeXList[i] += addPixel;
                }
            }

            if (restartTranslateProcess)
            {
                //스냅샷 처리를 위해 필요하다
                //TODO : 아래와 통합해서 처리해야 한다
                _processTranslateService.PauseAndRestartTranslate(() =>
                {
                    setCutPoint(tempXList.ToArray(), tempYList.ToArray(), tempSizeXList.ToArray(), tempSizeYList.ToArray(), tempXList.Count);
                    SetExceptPoint(_exceptionLocationXList.ToArray(), _exceptionLocationYList.ToArray(), _exceptionSizeXList.ToArray(), _exceptionSizeYList.ToArray(), _exceptionLocationXList.Count);
                    SetUseColorGroup();
                });
            }
            else
            {
                setCutPoint(tempXList.ToArray(), tempYList.ToArray(), tempSizeXList.ToArray(), tempSizeYList.ToArray(), tempXList.Count);
                SetExceptPoint(_exceptionLocationXList.ToArray(), _exceptionLocationYList.ToArray(), _exceptionSizeXList.ToArray(), _exceptionSizeYList.ToArray(), _exceptionLocationXList.Count);
                SetUseColorGroup();
            }
        }

        private Rectangle GetOcrAreaFormProcessRect(OcrAreaForm ocrAreaForm)
        {
            if (ocrAreaForm == null || ocrAreaForm.IsDisposed)
            {
                return Rectangle.Empty;
            }

            int borderWidth = Util.ocrFormBorder;
            int titlebarHeight = Util.ocrFormTitleBar;
            return new Rectangle(
                ocrAreaForm.Location.X + borderWidth,
                ocrAreaForm.Location.Y + titlebarHeight,
                Math.Max(ocrAreaForm.Size.Width - borderWidth * 2, 1),
                Math.Max(ocrAreaForm.Size.Height - titlebarHeight - borderWidth, 1));
        }

        public Rectangle GetOcrAreaProcessRect(int index)
        {
            if (IsUseOnlyOcrAreaFollowMouse())
            {
                return index == 0 ? GetOcrAreaFormProcessRect(FormManager.Instace.ocrAreaFollowMouseForm) : Rectangle.Empty;
            }

            if (MySettingManager.LastSnapShotRect != Rectangle.Empty)
            {
                return MySettingManager.LastSnapShotRect;
            }

            if (index < MySettingManager.NowLocationXList.Count
                && index < MySettingManager.NowLocationYList.Count
                && index < MySettingManager.NowSizeXList.Count
                && index < MySettingManager.NowSizeYList.Count)
            {
                return new Rectangle(
                    MySettingManager.NowLocationXList[index],
                    MySettingManager.NowLocationYList[index],
                    MySettingManager.NowSizeXList[index],
                    MySettingManager.NowSizeYList[index]);
            }

            int quickIndex = MySettingManager.NowLocationXList.Count;
            if (index == quickIndex && FormManager.Instace.quickOcrAreaForm != null)
            {
                return GetOcrAreaFormProcessRect(FormManager.Instace.quickOcrAreaForm);
            }

            int mouseFollowIndex = quickIndex;
            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                mouseFollowIndex++;
            }

            if (index == mouseFollowIndex && IsOcrAreaFollowMouse && FormManager.Instace.ocrAreaFollowMouseForm != null && !AdvencedOptionManager.UseLegacyOcrAreaFollowMouse)
            {
                return GetOcrAreaFormProcessRect(FormManager.Instace.ocrAreaFollowMouseForm);
            }

            return Rectangle.Empty;
        }

        public Rectangle GetOcrAreaProcessFullRect()
        {
            Rectangle resultRect = Rectangle.Empty;

            if (IsUseOnlyOcrAreaFollowMouse())
            {
                Rectangle mouseFollowRect = GetOcrAreaFormProcessRect(FormManager.Instace.ocrAreaFollowMouseForm);
                return mouseFollowRect == Rectangle.Empty ? new Rectangle(1, 1, 1, 1) : mouseFollowRect;
            }

            if (MySettingManager.LastSnapShotRect != Rectangle.Empty)
            {
                return MySettingManager.LastSnapShotRect;
            }

            for (int i = 0; i < MySettingManager.NowLocationXList.Count; i++)
            {
                Rectangle rect = GetOcrAreaProcessRect(i);
                if (rect == Rectangle.Empty)
                {
                    continue;
                }

                resultRect = resultRect == Rectangle.Empty ? rect : Rectangle.Union(resultRect, rect);
            }

            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                Rectangle quickRect = GetOcrAreaProcessRect(MySettingManager.NowLocationXList.Count);
                if (quickRect != Rectangle.Empty)
                {
                    resultRect = resultRect == Rectangle.Empty ? quickRect : Rectangle.Union(resultRect, quickRect);
                }
            }

            if (IsOcrAreaFollowMouse && FormManager.Instace.ocrAreaFollowMouseForm != null && !AdvencedOptionManager.UseLegacyOcrAreaFollowMouse)
            {
                int mouseFollowIndex = MySettingManager.NowLocationXList.Count;
                if (FormManager.Instace.quickOcrAreaForm != null)
                {
                    mouseFollowIndex++;
                }

                Rectangle mouseFollowRect = GetOcrAreaProcessRect(mouseFollowIndex);
                if (mouseFollowRect != Rectangle.Empty)
                {
                    resultRect = resultRect == Rectangle.Empty ? mouseFollowRect : Rectangle.Union(resultRect, mouseFollowRect);
                }
            }

            return resultRect == Rectangle.Empty ? new Rectangle(1, 1, 1, 1) : resultRect;
        }


        public void MakeCaptureArea() //영역 검색 버튼 클릭
        {
            int searchAreaQuantity = 0;

            for (int i = 0; i < FormManager.Instace.OcrAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.OcrAreaFormList[i];
                searchAreaQuantity++;
                foundedForm.SetVisible(true);
            }

            for (int i = 0; i < FormManager.Instace.exceptionAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.exceptionAreaFormList[i];
                foundedForm.SetVisible(true);
            }

            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                FormManager.Instace.quickOcrAreaForm.SetVisible(true);
            }

            if (FormManager.Instace.ocrAreaFollowMouseForm != null)
            {
                FormManager.Instace.ocrAreaFollowMouseForm.SetVisible(true);
            }

            MakeSearchOptionForm();
            if (searchAreaQuantity < 1)
            {
                FormManager.Instace.MakeCpatureAreaForm();
            }
        }

        #endregion


        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (!Program.IS_FORCE_QUITE)
            {
                if (AdvencedOptionManager.EnableSystemTrayMode)
                {
                    this.Visible = false;
                }
                else
                {
                    OnCloseApplication();
                }

                e.Cancel = true; //종료를 취소하고
            }
        }

        #region:::::::::::::::::::::::::::::::::::::::::::체크박스 및 라디오 클릭:::::::::::::::::::::::::::::::::::::::::::

        private bool isLockImgCheckBox = false;

        private void SetImgCheckBox(bool isUseHsv, bool isUseRgb, bool isUseThreshold)
        {
            if (!isLockImgCheckBox && _currentState == CurrentStateType.None)
            {
                isLockImgCheckBox = true;

                checkHSV.Checked = false;
                checkRGB.Checked = false;
                cbThreshold.Checked = false;

                MySettingManager.NowIsUseHSVFlag = false;
                MySettingManager.NowIsUseRGBFlag = false;
                MySettingManager.isUseThreshold = false;

                if (isUseHsv)
                {
                    checkHSV.Checked = true;
                    MySettingManager.NowIsUseHSVFlag = true;
                }
                else if (isUseRgb)
                {
                    checkRGB.Checked = true;
                    MySettingManager.NowIsUseRGBFlag = true;
                }
                else if (isUseThreshold)
                {
                    cbThreshold.Checked = true;
                    MySettingManager.isUseThreshold = true;
                }

                isLockImgCheckBox = false;
            }
        }

        private void cbThreshold_CheckedChanged(object sender, EventArgs e)
        {
            SetImgCheckBox(false, false, cbThreshold.Checked);
        }

        private void checkRGB_MouseDown(object sender, MouseEventArgs e)
        {
            SetImgCheckBox(false, checkRGB.Checked, false);
        }

        private void checkHSV_MouseDown(object sender, MouseEventArgs e)
        {
            SetImgCheckBox(checkHSV.Checked, false, false);
        }

        private void tesseractLanguageComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // 0 = 영어
            // 1 = 일본어
            // 2 = 기타
            ChangeTesseractLanguage(tesseractLanguageComboBox.SelectedIndex);
        }

        private void ChangeTesseractLanguage(int index)
        {
            if (index == (int)GlobalDefine.TesseractLanguageType.English)
            {
                tessDataTextBox.Text = "eng";
                naverTransComboBox.SelectedIndex = 0;
                googleTransComboBox.SelectedIndex = 0;
                cbDeepLLanguage.SelectedIndex = 0;
                removeSpaceCheckBox.Checked = false;
                cbPerWordDic.Checked = true;
            }
            else if (index == (int)GlobalDefine.TesseractLanguageType.Japen)
            {
                tessDataTextBox.Text = "jpn";
                naverTransComboBox.SelectedIndex = 1;
                googleTransComboBox.SelectedIndex = 1;
                cbDeepLLanguage.SelectedIndex = 1;
                removeSpaceCheckBox.Checked = true;
                cbPerWordDic.Checked = false;
            }

            if (index != tesseractLanguageComboBox.SelectedIndex)
            {
                tesseractLanguageComboBox.SelectedIndex = index;
            }
        }

        private void groupCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (groupCombo.SelectedIndex == 0) //아이템 추가
            {
                colorGroup.Add(new ColorGroup());

                groupCombo.Items.Add(groupCombo.Items.Count - 1); //카운트는 1부터 시작
                nowColorGroupIndex = groupCombo.Items.Count - 3; //나우 인덱스는 0부터 시작 (실질적인 숫자는 2부터 시작) -> 번호 2 = 카운트 4 / 나우는 1 이어야 함
                groupCombo.SelectedIndex = groupCombo.Items.Count - 1; //현재 선택 -> 가장 위

                for (int i = 0; i < MySettingManager.UseColorGroup.Count; i++)
                {
                    MySettingManager.UseColorGroup[i].Add(1);
                }

                MySettingManager.QuickOcrUsecolorGroup.Add(1);
            }
            else if (groupCombo.SelectedIndex == 1) //아이템 삭제
            {
                if (groupCombo.Items.Count > 3)
                {
                    int removePoint = 0;
                    colorGroup.RemoveAt(nowColorGroupIndex);
                    groupCombo.Items.RemoveAt(nowColorGroupIndex + 2); //나우 + 2 = 실질적인 콤보박스 번호
                    if (nowColorGroupIndex == 0)
                    {
                        groupCombo.SelectedIndex = 2;
                        removePoint = 2;
                    }
                    else
                    {
                        groupCombo.SelectedIndex = nowColorGroupIndex + 1; //나우 + 1 = 지우기 전 이전
                        removePoint = 3;
                    }


                    for (int i = nowColorGroupIndex + removePoint; i < groupCombo.Items.Count; i++)
                    {
                        int newText = Convert.ToInt32(groupCombo.Items[i].ToString());
                        newText--;
                        groupCombo.Items[i] = newText.ToString();
                    }

                    for (int i = 0; i < MySettingManager.UseColorGroup.Count; i++)
                    {
                        MySettingManager.UseColorGroup[i].RemoveAt(nowColorGroupIndex);
                    }

                    MySettingManager.QuickOcrUsecolorGroup.RemoveAt(nowColorGroupIndex);
                }
                else
                {
                    groupCombo.SelectedIndex = nowColorGroupIndex + 2;
                }
            }
            else
            {
                nowColorGroupIndex = groupCombo.SelectedIndex - 2; //나우 + 2 = 그룹의 숫자 인덱스
                colorGroup[nowColorGroupIndex].checkHSVRange();
                SetColorValueText(colorGroup[nowColorGroupIndex]);
            }

            groupLabel.Text = (groupCombo.Items.Count - 2).ToString();
        }

        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::키값 입력:::::::::::::::::::::::::::::::::::::::::::

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != Convert.ToChar(Keys.Back))
            {
                e.Handled = true;
            }
        }


        private void rgbTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if (thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if (value > 255)
                {
                    value = 255;
                }

                thisTextBox.Text = value.ToString();
            }

            colorGroup[nowColorGroupIndex].setRGBValuse(Convert.ToInt32(rTextBox.Text), Convert.ToInt32(gTextBox.Text), Convert.ToInt32(bTextBox.Text));
        }

        private void hsvTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if (thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if (value > 100)
                {
                    value = 100;
                }

                thisTextBox.Text = value.ToString();
            }

            colorGroup[nowColorGroupIndex].setHSVValuse(Convert.ToInt32(s1TextBox.Text), Convert.ToInt32(s2TextBox.Text), Convert.ToInt32(v1TextBox.Text), Convert.ToInt32(v2TextBox.Text));
        }

        private void thresholdTextLeave(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;

            if (thisTextBox.Text == "")
            {
                thisTextBox.Text = "0";
            }
            else
            {
                int value = Convert.ToInt32(thisTextBox.Text);
                if (value > 255)
                {
                    value = 255;
                }

                thisTextBox.Text = value.ToString();
            }
        }

        #endregion


        private void acceptButton_Click(object sender, EventArgs e)
        {
            inputKeyList.Clear();
            _backupOcrAreaModel = null;
            _currentState = CurrentStateType.Accept;
            acceptButton.Focus();
            _processTranslateService.PauseAndRestartTranslate(ApplyUIValueToSetting);

            TransForm foundedForm = null;
            TransFormLayer foundedLayerForm = null;

            FormManager.Instace.SetTemporaryDisableTopMostTransform();
            //설정 저장
            SaveSetting(GlobalDefine.USER_SETTING_FILE);

            bool isError = GetIsHasError();

            if (!isError)
            {
                FormManager.ShowPopupMessage("MORT", LocalizeString("Apply Complete"));
            }


            FormManager.Instace.ResetTemporaryDisableTopMostTransform();

            _isDoingClipboard = false; //적용을 누르면 클립보드 상태를 강제로 해제
            _currentState = CurrentStateType.None;
        }


        #region:::::::::::::::::::::::::::::::::::::::::::트레이 아이콘 함수:::::::::::::::::::::::::::::::::::::::::::

        //트레이 아이콘을 더블클릭 했을시 호출
        void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true; // 폼의 표시
            MakeTransForm();
            MakeRTT();
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal; // 최소화를 멈춘다
            this.Activate(); // 폼을 활성화 시킨다
        }

        void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormManager.Instace.SetTemporaryDisableTopMostTransform();

            if (MessageBox.Show(LocalizeString("Tray Icon Close"), LocalizeString("Tray Icon Close"), MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CloseApplication();
            }

            FormManager.Instace.ResetTemporaryDisableTopMostTransform();
        }

        private void ContextTranslate_Click(object sender, EventArgs e)
        {
            if (_processTranslateService.IdleState)
            {
                BeforeStartRealTimeTrans();
            }
            else if (_processTranslateService.ProcessingState)
            {
                StopTrans();
            }
        }

        private void ContextOption_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Form1")
                {
                    frm.Activate();
                    this.Show();
                    return;
                }
            }

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(20, 20);
            this.Show();
        }


        private void setTranslateTopMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isTranslateFormTopMostFlag = !isTranslateFormTopMostFlag;
            setTranslateTopMostToolStripMenuItem.Checked = !setTranslateTopMostToolStripMenuItem.Checked;
            topMostcheckBox.Checked = isTranslateFormTopMostFlag;

            FormManager.Instace.SetTopMostTransform(isTranslateFormTopMostFlag);
        }


        private void setCutPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeCaptureArea();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && _initialized == true)
            {
                ContextOption.Show();
                if (_processTranslateService.IdleState)
                {
                    this.BeginInvoke(new myDelegate(updateText), new object[] { LocalizeString("Translate Start") });
                }
                else if (_processTranslateService.ProcessingState)
                {
                    this.BeginInvoke(new myDelegate(updateText), new object[] { LocalizeString("Translate Stop") });
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "About")
                {
                    frm.Activate();
                    frm.Show();
                    return;
                }
            }

            About aboutForm = new About();
            aboutForm.Show();
        }

        private void showTransToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeTransForm();
        }


        private void setCheckSpellingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MySettingManager.NowDicFile = dicFileTextBox.Text;
            MySettingManager.NowIsUseDicFileFlag = setCheckSpellingToolStripMenuItem.Checked;
            checkDic.Checked = MySettingManager.NowIsUseDicFileFlag;
            ApplySpellCheck();
        }

        private void rTTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeRTT();
        }


        private void checkUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Util.OpenURL("https://blog.naver.com/killkimno/70179867557");
        }

        #endregion

        private delegate void myDelegate(string transText);

        private void updateText(string transText)
        {
            transToolStripMenuItem.Text = transText;
        }


        #region:::::::::::::::::::::::::::::::::::::::::::폼 이동 관련 함수:::::::::::::::::::::::::::::::::::::::::::

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        private void fromUpImg_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void fromUpImg_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }

        #endregion


        private void panealBorder_Paint(object sender, PaintEventArgs e) //패널에 경계선 칠하기 함수
        {
            Panel myPanel = (Panel)sender;

            Pen myPen = new Pen(Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70))))), 1);
            e.Graphics.DrawRectangle(myPen,
                myPanel.ClientRectangle.Left,
                myPanel.ClientRectangle.Top,
                myPanel.ClientRectangle.Width - 1,
                myPanel.ClientRectangle.Height - 1);
            base.OnPaint(e);
        }

        /// <summary>
        /// 툴팁 / 버튼 둘 다 / 설정 저장하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingSaveToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _currentState = CurrentStateType.SaveFile;
            _processTranslateService.PauseAndRestartTranslate(ApplyUIValueToSetting);

            SaveFileDialog savePanel = new SaveFileDialog();
            savePanel.RestoreDirectory = false;
            savePanel.InitialDirectory = System.Environment.CurrentDirectory + "\\setting";
            savePanel.Filter = "Config File (*.conf)|*.conf";
            if (savePanel.ShowDialog() == DialogResult.OK)
            {
                SaveSetting(savePanel.FileName);
            }

            _currentState = CurrentStateType.None;
        }

        private void settingLoadToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPanel = new OpenFileDialog();
            openPanel.RestoreDirectory = false;
            openPanel.InitialDirectory = System.Environment.CurrentDirectory + "\\setting";
            openPanel.Filter = "Config File (*.conf)|*.conf";


            string file = "";
            if (openPanel.ShowDialog() == DialogResult.OK)
            {
                file = openPanel.FileName;

                Util.ShowLog("Open Setting file - " + file);
                if (file != "")
                {
                    OpenSettingFile(file);
                }
            }
        }

        private void settingDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _currentState = CurrentStateType.SetDefault;
            isTranslateFormTopMostFlag = true;


            if (FormManager.Instace.quickOcrAreaForm != null)
            {
                FormManager.Instace.quickOcrAreaForm.Close();
                FormManager.Instace.quickOcrAreaForm = null;
            }

            _processTranslateService.PauseAndRestartTranslate(() =>
            {
                MySettingManager.SetDefault();
                SetValueToUIValue();
                ApplyUIValueToSetting();
            });

            if (MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                if (FormManager.Instace.MyLayerTransForm != null)
                {
                    FormManager.Instace.MyLayerTransForm.SetTopMost(isTranslateFormTopMostFlag, AdvencedOptionManager.UseTopMostOptionWhenTranslate);
                }
            }
            else if (MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                if (FormManager.Instace.MyBasicTransForm != null)
                {
                    FormManager.Instace.MyBasicTransForm.SetTopMost(isTranslateFormTopMostFlag, AdvencedOptionManager.UseTopMostOptionWhenTranslate);
                }
            }

            SaveSetting(GlobalDefine.USER_SETTING_FILE);
            _currentState = CurrentStateType.None;
        }


        private void checkDic_CheckedChanged(object sender, EventArgs e)
        {
            setCheckSpellingToolStripMenuItem.Checked = checkDic.Checked;
        }


        private void defaultButton_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageBox.Show(LocalizeString("Trey Icon Set Default Setting"), LocalizeString("Trey Icon Set Default Setting Title"), MessageBoxButtons.OKCancel))
            {
                settingDefaultToolStripMenuItem_Click(sender, e);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbMain.SelectedIndex == 1)
            {
                // MessageBox.Show(".");
            }
        }

        private void SetDefaultZoomSizeButton_Click(object sender, EventArgs e)
        {
            MySettingManager.ImgZoomSize = 2;
            imgZoomsizeUpDown.Value = (decimal)2;
        }

        private void help_Button_Click(object sender, EventArgs e)
        {
            Util.OpenURL("https://blog.naver.com/killkimno/221904784013");
        }

        private void error_Information_Button_Click(object sender, EventArgs e)
        {
            Util.OpenURL("https://blog.naver.com/killkimno/70185869419");
        }

        private void about_Button_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "About")
                {
                    frm.Activate();
                    frm.Show();
                    return;
                }
            }

            About aboutForm = new About();
            aboutForm.Show();
        }

        //마지막으로 선택한 OCR 타입
        private SettingManager.OcrType beforeOcrPanelType = SettingManager.OcrType.Tesseract;
        private readonly VersionCheckLogic _versionCheckLogic;

        //OCR 방식 변경
        private void OCR_Type_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tesseract_panel.Visible = false;
            WinOCR_panel.Visible = false;
            pnNHocr.Visible = false;
            pnGoogleOcr.Visible = false;
            pnEasyOcr.Visible = false;

            cbGoogleOcrLanguge.SelectedIndex = 0;

            //string selectItem = OCR_Type_comboBox.SelectedItem.ToString();
            SettingManager.OcrType ocrType = SettingManager.GetOcrType(OCR_Type_comboBox.SelectedIndex);
            if (ocrType == SettingManager.OcrType.Tesseract)
            {
                Tesseract_panel.Visible = true;
            }
            else if (ocrType == SettingManager.OcrType.Window)
            {
                WinOCR_panel.Visible = true;

                if (_initialized && isAvailableWinOCR && !isShowWinOCRWarning && winLanguageCodeList.Count == 1)
                {
                    if (winLanguageCodeList[0] == "ko")
                    {
                        if (DialogResult.OK == MessageBox.Show(LocalizeString("Win OCR Require Download", true), ".", MessageBoxButtons.OKCancel))
                        {
                            Util.OpenURL("https://blog.naver.com/killkimno/220865537274");
                        }

                        isShowWinOCRWarning = true;
                    }
                }
            }
            else if (ocrType == SettingManager.OcrType.EasyOcr)
            {
                pnEasyOcr.Visible = true;
            }
            else if (ocrType == SettingManager.OcrType.OneOcr)
            {
                pnNHocr.Visible = true;
            }
            else if (ocrType == SettingManager.OcrType.Google)
            {
                pnGoogleOcr.Visible = true;
            }

            //유저가 변경한 상태다.
            if (_currentState == CurrentStateType.None)
            {
                bool isRequireChange = false;
                int languageType = 0; //0 = 영어, 1 = 일본어 , //2 = 기타

                if (beforeOcrPanelType == SettingManager.OcrType.Tesseract)
                {
                    switch (tesseractLanguageComboBox.SelectedIndex)
                    {
                        case (int)GlobalDefine.TesseractLanguageType.English:
                            isRequireChange = true;
                            languageType = 0;
                            break;

                        case (int)GlobalDefine.TesseractLanguageType.Japen:
                            isRequireChange = true;
                            languageType = 1;
                            break;
                    }
                }
                else if (beforeOcrPanelType == SettingManager.OcrType.OneOcr)
                {
                    //TODO : 언어 통합해야 함
                    isRequireChange = true;
                }
                else if (beforeOcrPanelType == SettingManager.OcrType.Window)
                {
                    string selectCode = winLanguageCodeList[WinOCR_Language_comboBox.SelectedIndex];
                    if (selectCode == "en" || selectCode == "en-US")
                    {
                        isRequireChange = true;
                        languageType = 0;
                    }
                    else if (selectCode == "ja")
                    {
                        isRequireChange = true;
                        languageType = 1;
                    }
                }
                else if (beforeOcrPanelType == SettingManager.OcrType.EasyOcr)
                {
                    int index = cbEasyOcrCode.SelectedIndex;
                    string selectCode = OcrManager.Instace.EasyOcrCodeList[index];
                    if (selectCode == "en" || selectCode == "en-US")
                    {
                        isRequireChange = true;
                        languageType = 0;
                    }
                    else if (selectCode == "ja")
                    {
                        isRequireChange = true;
                        languageType = 1;
                    }
                }

                if (isRequireChange)
                {
                    if (ocrType == SettingManager.OcrType.Tesseract)
                    {
                        if (languageType == 0)
                        {
                            ChangeTesseractLanguage((int)GlobalDefine.TesseractLanguageType.English);
                        }
                        else if (languageType == 1)
                        {
                            ChangeTesseractLanguage((int)GlobalDefine.TesseractLanguageType.Japen);
                        }
                    }
                    else if (ocrType == SettingManager.OcrType.Window)
                    {
                        if (isAvailableWinOCR)
                        {
                            string code = "";

                            if (languageType == 0)
                            {
                                code = "en";
                            }
                            else if (languageType == 1)
                            {
                                code = "ja";
                            }

                            //OCR을 찾았나 못 찾았나.
                            //영어는 en 또는 en-us일 수 있다
                            bool isFound = false;
                            for (int i = 0; i < winLanguageCodeList.Count; i++)
                            {
                                if (Util.GetIsEqualMainOcrCode(winLanguageCodeList[i], code))
                                {
                                    if (WinOCR_Language_comboBox.Items.Count > i)
                                    {
                                        isFound = true;
                                        ChangeWinOcrLanguage(i);

                                        break;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (ocrType == SettingManager.OcrType.EasyOcr)
                    {
                        int index = cbEasyOcrCode.SelectedIndex;
                        string code = OcrManager.Instace.EasyOcrCodeList[index];

                        if (languageType == 0)
                        {
                            code = "en";
                        }
                        else if (languageType == 1)
                        {
                            code = "ja";
                        }

                        //OCR을 찾았나 못 찾았나.
                        int codeIndex = OcrManager.Instace.EasyOcrCodeList.FindIndex(r => r == code);

                        if (codeIndex > -1)
                        {
                            ChangeEasyOcrLanguage(codeIndex);
                        }
                    }
                }
            }

            beforeOcrPanelType = ocrType;
        }

        //번역 방식 변경.
        private void CbTranslateTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            DB_Panel.Visible = false;
            Naver_Panel.Visible = false;
            Google_Panel.Visible = false;
            pnGoogleBasic.Visible = false;
            pnDeepl.Visible = false;
            pnEzTrans.Visible = false;
            pnCustomApi.Visible = false;
            pnPapagoWeb.Visible = false;
            pnDeepLAPI.Visible = false;
            pnGemini.Visible = false;

            var translateType = _translateTypListService.GetTransType(_cbTranslateType.SelectedIndex);

            switch (translateType)
            {
                case SettingManager.TransType.db:
                    DB_Panel.Visible = true;
                    break;

                case SettingManager.TransType.naver:
                    Naver_Panel.Visible = true;
                    break;

                case SettingManager.TransType.papago_web:
                    pnPapagoWeb.Visible = true;
                    break;

                case SettingManager.TransType.google:
                    Google_Panel.Visible = true;
                    break;

                case SettingManager.TransType.deepl:
                    pnDeepl.Visible = true;
                    break;

                case SettingManager.TransType.google_url:
                    pnGoogleBasic.Visible = true;
                    break;

                case SettingManager.TransType.ezTrans:
                    pnEzTrans.Visible = true;
                    break;

                case SettingManager.TransType.customApi:
                    pnCustomApi.Visible = true;
                    break;

                case SettingManager.TransType.deeplApi:
                    pnDeepLAPI.Visible = true;
                    break;

                case SettingManager.TransType.gemini:
                    pnGemini.Visible = true;
                    break;
            }
        }

        private void RbDeepLAPIEndpoint_CheckedChanged(object sender, System.EventArgs e)
        {
        }

        //단축키 - 번역 초기값.
        private void SetEmptyTansKey()
        {
            transKeyInputLabel.SetEmpty();
        }

        //단축키 - 교정 사전 초기값.
        private void SetEmptyDicKey()
        {
            this.dicKeyInputLabel.SetEmpty();
        }

        //단축키 - 빠른 영역 초기값.
        private void SetEmptyQuickKey()
        {
            this.quickKeyInputLabel.SetEmpty();
        }

        //단축키 - 스냅샷 초기값.
        private void SetEmptySnapShotKey()
        {
            this.snapShotInputLabel.SetEmpty();
        }

        //단축키 - 한 번만 번역하기
        private void SetEmptyOneTranslate()
        {
            this.lbOneTrans.SetEmpty();
        }

        private void SetEmptyHideTranslate()
        {
            this.lbHideTranslate.SetEmpty();
        }

        private void SetEmptyFollowMouse()
        {
            this.lbFollowMouse.SetEmpty();
        }


        //단축키 - 번역 초기값.
        private void InitTansKey()
        {
            transKeyInputLabel.SetDefaultKey();
        }

        //단축키 - 교정 사전 초기값.
        private void InitDicKey()
        {
            this.dicKeyInputLabel.SetDefaultKey();
        }

        //단축키 - 빠른 영역 초기값.
        private void InitQuickKey()
        {
            this.quickKeyInputLabel.SetDefaultKey();
        }

        //단축키 - 스냅샷 초기값.
        private void InitSnapShotKey()
        {
            this.snapShotInputLabel.SetDefaultKey();
        }

        //한 번만 번역하기 초기값
        private void InitOneTranslateKey()
        {
            this.lbOneTrans.SetDefaultKey();
        }


        //단축키 - 창 숨김 초기값.
        private void InitHideTransKey()
        {
            this.lbHideTranslate.SetDefaultKey();
        }

        //단축키 - OCR 영역 마우스 따라다니기 초기값.
        private void InitFollowMouseKey()
        {
            this.lbFollowMouse.SetDefaultKey();
        }


        #region ::::::::: OCR, 번역 언어 변경 관련 ::::::::::

        /// <summary>
        /// OCR 언어 변경시 사용
        /// </summary>
        /// <param name="ocrCode"></param>
        private void SetTransLangugage(string ocrCode)
        {
            Util.ShowLog("OCR Code : " + ocrCode);
            TransManager.TransCodeData codeData = TransManager.Instace.GetTransCodeData(ocrCode);

            if (codeData != null)
            {
                if (codeData.naverCode != "")
                {
                    foreach (var obj in naverTransComboBox.Items)
                    {
                        TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                        if (codeData.naverCode == data.naverCode)
                        {
                            naverTransComboBox.SelectedItem = obj;
                            break;
                        }
                    }
                }

                if (codeData.googleCode != "")
                {
                    foreach (var obj in googleTransComboBox.Items)
                    {
                        TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                        if (codeData.googleCode == data.googleCode)
                        {
                            googleTransComboBox.SelectedItem = obj;
                            break;
                        }
                    }
                }

                if (codeData.DeepLCode != "")
                {
                    foreach (var obj in cbDeepLLanguage.Items)
                    {
                        TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                        if (codeData.DeepLCode == data.DeepLCode)
                        {
                            cbDeepLLanguage.SelectedItem = obj;
                            break;
                        }
                    }
                }
            }

            return;
        }

        private void ChangeWinOcrLanguage(int index)
        {
            string ocrCode = "";
            if (index < winLanguageCodeList.Count)
            {
                //Util.ShowLog(languageCodeList[WinOCR_Language_comboBox.SelectedIndex]);
                ocrCode = winLanguageCodeList[index];
                CheckRemoveSpaceLanguage(ocrCode);
            }

            SetTransLangugage(ocrCode);

            if (WinOCR_Language_comboBox.SelectedIndex != index)
            {
                WinOCR_Language_comboBox.SelectedIndex = index;
            }
        }

        private void ChangeEasyOcrLanguage(int index)
        {
            string resultCode = "";
            if (index < OcrManager.Instace.EasyOcrCodeList.Count)
            {
                //Util.ShowLog(languageCodeList[WinOCR_Language_comboBox.SelectedIndex]);
                resultCode = OcrManager.Instace.EasyOcrCodeList[index];
                CheckRemoveSpaceLanguage(resultCode);
            }

            SetTransLangugage(resultCode);

            if (cbEasyOcrCode.SelectedIndex != index)
            {
                cbEasyOcrCode.SelectedIndex = index;
            }
        }

        private void CheckRemoveSpaceLanguage(string resultCode)
        {
            if (resultCode == "ko")
            {
                removeSpaceCheckBox.Checked = false;
            }
            else if (resultCode == "en" || resultCode == "en-US")
            {
                removeSpaceCheckBox.Checked = false;
                cbPerWordDic.Checked = true;
            }
            else if (resultCode == "ja" || resultCode == "zh-Hans-CN" || resultCode == "zh-Hant-TW")
            {
                //20190106 일본어를 하면 자동으로 ocr 공백제거 선택
                removeSpaceCheckBox.Checked = true;
                cbPerWordDic.Checked = false;
            }
        }

        private void ChangeMainOcrLangauge(OcrLanguageType ocrLanguageType)
        {
            //TODO : 다른 언어도 다 바꿔야 한다
            var code = Util.GetOcrLanguageCode(ocrLanguageType);
            CheckRemoveSpaceLanguage(code);

            if (code != "other")
            {
                SetTransLangugage(code);
            }

            _currentOcrLanguage = ocrLanguageType;
        }

        #endregion


        private void cbGoogleOcrLanguge_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_currentState == CurrentStateType.None)
            {
                var item = cbGoogleOcrLanguge.SelectedItem;

                if (item is MORT.ComboboxItem)
                {
                    MORT.ComboboxItem cbItem = (MORT.ComboboxItem)item;
                    TransCodeData transCodeData = (TransCodeData)cbItem.Value;

                    Console.WriteLine(transCodeData.Title + "/ " + transCodeData.languageCode);
                    string resultCode = transCodeData.languageCode;

                    if (resultCode == "ko")
                    {
                        removeSpaceCheckBox.Checked = false;
                    }
                    else if (resultCode == "en" || resultCode == "en-US")
                    {
                        removeSpaceCheckBox.Checked = false;
                        cbPerWordDic.Checked = true;
                    }
                    else if (resultCode == "ja" || resultCode == "zh-Hans-CN" || resultCode == "zh-Hant-TW")
                    {
                        //20190106 일본어를 하면 자동으로 ocr 공백제거 선택
                        removeSpaceCheckBox.Checked = true;
                        cbPerWordDic.Checked = false;
                    }

                    SetTransLangugage(resultCode);
                }
            }
        }

        private void WinOCR_Language_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ChangeWinOcrLanguage(WinOCR_Language_comboBox.SelectedIndex);
        }

        private void cbEasyOcrOcde_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ChangeEasyOcrLanguage(cbEasyOcrCode.SelectedIndex);
        }

        private void tabControl1_DrawItem(Object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = tbMain.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tbMain.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Red);
                g.FillRectangle(Brushes.Gray, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Arial", (float)10.0, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }


        /// <summary>
        /// 구글 인증토큰 모두 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_RemoveAllGoogleToekn_Click(object sender, EventArgs e)
        {
            FormManager.Instace.SetTemporaryDisableTopMostTransform();
            if (DialogResult.OK == MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized }, LocalizeString("Delete All Google Token"),
                    LocalizeString("Delete All Google Token Title"), MessageBoxButtons.OKCancel))
            {
                TransManager.Instace.DeleteAllGsTransToken();
            }

            FormManager.Instace.ResetTemporaryDisableTopMostTransform();
        }

        private void donationButton_Click(object sender, EventArgs e)
        {
            ShowDonationPopup();
        }

        private void ShowDonationPopup()
        {
            if (LocalizeManager.LocalizeManager.Language == LocalizeManager.AppLanguage.Korea)
            {
                FormManager.Instace.SetTemporaryDisableTopMostTransform();

                FormManager.Instace.ShowDonatePage();

                FormManager.Instace.ResetTemporaryDisableTopMostTransform();
            }
            else
            {
                Util.OpenURL("https://ko-fi.com/killkimno");
            }
        }

        private void Button_NaverTransKeyList_Click(object sender, EventArgs e)
        {
            Action callback = delegate
            {
                if (TransManager.Instace.naverKeyList.Count > 0)
                {
                    var data = TransManager.Instace.naverKeyList[0];
                    naverIDKey = data.id;
                    naverSecretKey = data.secret;


                    NaverIDKeyTextBox.Text = naverIDKey;
                    NaverSecretKeyTextBox.Text = naverSecretKey;

                    NaverTranslateAPI.instance.ChangeValue(naverIDKey, naverSecretKey, data.isPaid);
                }
            };

            FormManager.Instace.ShowNaverKeyListUI(callback);
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _mouseFollowOcrAreaService.Dispose();
            notifyIcon1.Visible = false;
            notifyIcon1.Icon = null;
        }

        public void ForceDisableTopMost()
        {
            isTranslateFormTopMostFlag = false;
            setTranslateTopMostToolStripMenuItem.Checked = false;
        }

        private void btnInstallEasyOcr_Click(object sender, EventArgs e)
        {
            FormManager.Instace.ShowEasyOcrInstaller(OcrManager.Instace, _pythonService);
        }

        private void btnAddWinOcrLanguage_Click(object sender, EventArgs e)
        {
            FormManager.ShowTwoButtonPopupMessage(LocalizeString("Win OCR Add Language"), LocalizeString("Add Win Ocr Message"),
                () => { Util.OpenURL(@"ms-settings:regionlanguage"); });
        }

        private void cbOneOcrLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_currentState != CurrentStateType.None)
            {
                return;
            }

            var item = cbOneOcrLanguage.SelectedItem;

            ChangeMainOcrLangauge(_ocrLanguages[cbOneOcrLanguage.SelectedIndex]);
        }
    }
}
