//만든이 : 몽키해드
//블로그 주소 : https://blog.naver.com/killkimno

using MORT.Manager;
using MORT.Service.ProcessTranslateService;
using MORT.VersionCheck;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static MORT.Manager.OcrManager;
using static MORT.TransManager;

namespace MORT
{
    public enum eCurrentStateType
    {
        None, Init, LoadFile, SaveFile, Accept, SetDefault,
    }
    public partial class Form1 : Form, IMainFormContract
    {
        public static bool IsLockHotKey = false;

        public class ImgData
        {
            public int channels;
            public byte[] data;

            public int x;
            public int y;

            public int index;

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
        }


        #region:::::::::::::::::::::::::::::::::::::::::::Form level declarations:::::::::::::::::::::::::::::::::::::::::::

        private eCurrentStateType eCurrentState = eCurrentStateType.None;
        public delegate void PDelegateSetSpellCheck();

        /// <summary>
        /// 현재 번역중 상태인가?
        /// </summary>
        private bool _processTrans = false;

        bool isTranslateFormTopMostFlag = true;     //번역창이 최상위냐 아니냐

        private Point mousePoint;                   //창 이동 관련
        int _ocrProcessSpeed = 2000;                 //ocr 처리 딜레이 시간

        //폰트 관련
        Font textFont;
        Color textColor;
        Color outlineColor1;
        Color outlineColor2;
        Color backgroundColor;

        public bool IsUseClipBoardFlag
        {
            set
            {
                MySettingManager.NowIsSaveInClipboardFlag = value;
            }

            get
            {
                return MySettingManager.NowIsSaveInClipboardFlag;
            }
        }

        int nowColorGroupIndex = 0;                 //색 그룹 수

        List<int> locationXList = new List<int>();
        List<int> locationYList = new List<int>();
        List<int> sizeXList = new List<int>();
        List<int> sizeYList = new List<int>();

        //제외 영역 관련.
        List<int> exceptionLocationXList = new List<int>();
        List<int> exceptionLocationYList = new List<int>();
        List<int> exceptionSizeXList = new List<int>();
        List<int> exceptionSizeYList = new List<int>();

        List<ColorGroup> colorGroup = new List<ColorGroup>();   //색 그룹 리스트

        string naverIDKey = "";
        string naverSecretKey = "";

        private List<string> winLanguageCodeList = new List<string>();
        public List<string> WinLanguageCodeList
        {
            get { return winLanguageCodeList; }
        }


        private bool _initialized = false;                //초기화 완료
        public bool isAvailableWinOCR = true;           //윈도우 10 OCR 사용 가능한지 확인.
        private string winOcrErrorCode = "";
        public bool isShowWinOCRWarning = false;
        public SettingManager MySettingManager = new SettingManager(); //설정 관리자
        GlobalKeyboardHook gHook;
        List<int> nowKeyPressList = new List<int>();

        public static bool isDebugShowFormerResultLog = false;
        public static bool isDebugTransOneLine = false;
        public static bool isDebugShowWordArea = false;
        public static bool IsDebugDisplayLowQuality = false;

        private List<KeyInputLabel> inputKeyUIList = new List<KeyInputLabel>();

        private ProcessTranslateService _processTranslateService;

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
            int width, int height, int positionX, int positionY, [In, Out][MarshalAs(UnmanagedType.LPArray)] byte[] data);



        //MORT_CORE 스펠링 체크
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessGetSpellingCheck(StringBuilder ocrResult, bool isUseJpn);

        //MORT_CORE DB만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ProcessGetDBText(StringBuilder original, StringBuilder result);

        //MORT_CORE 이미지 데이터만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern System.IntPtr processGetImgData(int index, ref int x, ref int y, ref int channels, ref int locationX, ref int locationY);

        //MORT_CORE 이미지 데이터만 가져오기
        [DllImport(@"DLL\\MORT_CORE.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern System.IntPtr processGetImgDataFromByte(int index, int width, int height, int positionX, int positionY, [In, Out][MarshalAs(UnmanagedType.LPArray)] byte[] data, ref int x, ref int y, ref int channels);

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

        //MORT_CORE NHocr 사용 설정
        [DllImport(@"DLL\\MORT_CORE.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetIsUseNHocr(bool isUseNHocr);

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

        public class WinOcrLoader : MarshalByRefObject
        {
            public override object InitializeLifetimeService()
            {
                return null;
            }

            public void LoadAssembly(string path)
            {
                _assembly = Assembly.Load(AssemblyName.GetAssemblyName(path));
            }

            public void InitFunc()
            {
                Module[] modules = _assembly.GetModules();

                for (int i = 0; i < modules.Length; i++)
                {
                    // Console.WriteLine("sdfsdfdsfsf" + modules[i].ToString());
                }

                //loader.Initialize(1, "test2.Class1", "Test");
                //Type type = Type.GetType("namespace.qualified.TypeName, MORT_WIN10OCR.Class1");
                //Type type = modules[0].GetType("MORT_WIN10OCR.Class1");

                Type type = _assembly.GetType("MORT_WIN10OCR.Class1");
                MethodInfo method = type.GetMethod("StartMakeBitmap", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method2 = type.GetMethod("ProcessOCR", BindingFlags.Static | BindingFlags.Public);

                MethodInfo method3 = type.GetMethod("GetIsAvailable", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method4 = type.GetMethod("InitOcr", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method5 = type.GetMethod("GetIsAvailableDLL", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method6 = type.GetMethod("GetText", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method7 = type.GetMethod("GetAvailableLanguageList", BindingFlags.Static | BindingFlags.Public);

                MethodInfo method8 = type.GetMethod("TestMar", BindingFlags.Static | BindingFlags.Public);
                MethodInfo method9 = type.GetMethod("TextToSpeach", BindingFlags.Static | BindingFlags.Public);


                MethodInfo method11 = type.GetMethod("SetBitMap", BindingFlags.Static | BindingFlags.Public);

                makeBitmap = (Action)Delegate.CreateDelegate(typeof(Action), method);
                processOCRFunc = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), method2);
                getTextFunc = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), method6);
                getDLLAvailableFunc = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method5);
                getOCRAvailableFunc = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method3);
                initOCRFunc = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), method4);
                getLanguageListFunc = (Func<List<string>>)Delegate.CreateDelegate(typeof(Func<List<string>>), method7);

                textToSpeachFunc = (Action<string, int>)Delegate.CreateDelegate(typeof(Action<string, int>), method9);
                testFunc = (Func<IntPtr>)Delegate.CreateDelegate(typeof(Func<IntPtr>), method8);

                SetBitmapFunc = (Action<byte[], int, int, int>)Delegate.CreateDelegate(typeof(Action<byte[], int, int, int>), method11);
            }


            public List<string> GetLanguageList()
            {
                return getLanguageListFunc();
            }

            public string GetText()
            {
                return getTextFunc();
            }

            public IntPtr GetMar()
            {
                return testFunc();
            }

            public void InitOCR(string code)
            {
                initOCRFunc(code);
            }

            public bool GetIsAvailableOCR()
            {
                return getOCRAvailableFunc();
            }

            public void TextToSpeach(string text, int type)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    textToSpeachFunc(text, type);
                }

            }

            public void MakeBitMap()
            {
                makeBitmap();

            }

            public void SetImg(byte[] data, int channels, int x, int y)
            {
                SetBitmapFunc(data, channels, x, y);
            }

            public string ProcessOcrFunc()
            {
                string result = "yes";
                result = processOCRFunc();


                return result;
            }

            private Assembly _assembly;
            public Action makeBitmap;

            public Action<byte[], int, int, int> SetBitmapFunc;
            public Func<string> processOCRFunc;       //OCR 처리하기.
            public Func<string> getTextFunc;       //OCR 처리하기.
            public Func<bool> getDLLAvailableFunc;          //DLL 사용 가능한지 확인.
            public Func<bool> getOCRAvailableFunc;          //OCR 사용 가능한지 확인.
            public Action<string> initOCRFunc;          //OCR 사용 가능한지 확인.
            public Func<List<string>> getLanguageListFunc;  //사요 가능한 언어 가져오기.

            public Action<string, int> textToSpeachFunc;           //tts

            public Func<IntPtr> testFunc;   //마샬링 테스트.

        }

        private static WinOcrLoader loader;
        private static AppDomain Domain;

        private const string m_kDomainName = "myProgram";
        private const string m_kTargetFolder = "DLL";
        private const string m_kFilePath = ".\\";
        private const string m_kFileName = "MORT_WIN10OCR.dll";

        public void LoadDll()
        {
            if (Domain != null)
                AppDomain.Unload(Domain);

            string dest = Path.Combine(m_kFilePath, m_kTargetFolder, m_kFileName);

            Domain = AppDomain.CreateDomain(m_kDomainName);
            loader = (WinOcrLoader)Domain.CreateInstanceAndUnwrap(typeof(WinOcrLoader).Assembly.FullName, typeof(WinOcrLoader).FullName);
            loader.LoadAssembly(dest);
            loader.InitFunc();
        }


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
            else if (eCurrentState == eCurrentStateType.SetDefault || eCurrentState == eCurrentStateType.LoadFile)
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
            switch (eCurrentState)
            {
                case eCurrentStateType.Init:
                case eCurrentStateType.LoadFile:

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

                case eCurrentStateType.SetDefault:

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

                case eCurrentStateType.Accept:

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
                    try
                    {
                        System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/70185869419");
                    }
                    catch { }
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
            eCurrentState = eCurrentStateType.LoadFile;
            _processTranslateService.PauseAndRestartTranslate(() =>
            {
                LoadSettingfile(fileName);
                ApplyUIValueToSetting();
            });



            SaveSetting(GlobalDefine.USER_SETTING_FILE);
            eCurrentState = eCurrentStateType.None;
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
        public Form1()
        {
            _versionCheckLogic = new VersionCheckLogic(this);
            try
            {
                eCurrentState = eCurrentStateType.Init;

                //SetProcessDPIAware();  
                InitializeComponent();

                this.Text = $"Monkeyhead's OCR RealTime Translator - {Properties.Settings.Default.MORT_VERSION}(32bit)";

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
                    //윈도우 10 ocr 설정.
                    LoadDll();
                    List<string> codeList = loader.GetLanguageList();


                    WinOCR_Language_comboBox.Items.Clear();
                    for (int i = 0; i < codeList.Count; i++)
                    {
                        string[] key = codeList[i].Split(',');
                        if (key.Length >= 2)
                        {
                            winLanguageCodeList.Add(key[0]);
                            WinOCR_Language_comboBox.Items.Add(key[1]);
                        }
                    }

                    if (winLanguageCodeList.Count > 0)
                    {
                        WinOCR_Language_comboBox.SelectedIndex = 0;
                        loader.InitOCR(winLanguageCodeList[0]);
                    }
                    else
                    {
                        loader.InitOCR("");
                    }

                    if(codeList.Count == 0)
                    {
                        isAvailableWinOCR = false;
                    }


                }
                catch (Exception e)
                {
                    isAvailableWinOCR = false;
                    winOcrErrorCode = e.Message;
                }

                OcrManager.Instace.Init();
                OpenNaverKeyFile();
                OpenGoogleKeyFile();


                TransManager.Instace.InitFormerDic();
                InitHotKey();
                InitTransCode();


                LoadSettingfile(GlobalDefine.USER_SETTING_FILE);
                initOcr();
                //GDI+ 동작 여부 검사.
                CheckGDI();

                _processTranslateService = new ProcessTranslateService(this, MySettingManager, loader, isAvailableWinOCR, StopTrans);

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
                try
                {
                    System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/70185869419");
                }
                catch { }
                this.Close();
            }

            eCurrentState = eCurrentStateType.None;
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
            toolTip_OCR.SetToolTip(showOcrCheckBox, Properties.Settings.Default.TOOLTIP_SHOW_OCR_RESULT);
            toolTip_OCR.SetToolTip(saveOCRCheckBox, Properties.Settings.Default.TOOLTIP_OCRSAVE);
            toolTip_OCR.SetToolTip(isClipBoardcheckBox1, Properties.Settings.Default.TOOLTIP_CLIPBOARD);
            toolTip_OCR.SetToolTip(checkDic, Properties.Settings.Default.TOOLTIP_DIC);
            toolTip_OCR.SetToolTip(cbPerWordDic, Properties.Settings.Default.TOOLTIP_WORDDIC);

            toolTip_OCR.SetToolTip(checkRGB, Properties.Settings.Default.TOOLTIP_RGB);
            toolTip_OCR.SetToolTip(checkHSV, Properties.Settings.Default.TOOLTIP_HSV);

            toolTip_OCR.SetToolTip(cbDBMultiGet, Properties.Settings.Default.TOOLTIP_MULTI_DB);
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
                result += key + System.Environment.NewLine + System.Environment.NewLine; ;

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

            inputKeyUIList.Add(transKeyInputLabel);
            inputKeyUIList.Add(dicKeyInputLabel);
            inputKeyUIList.Add(quickKeyInputLabel);
            inputKeyUIList.Add(snapShotInputLabel);
            inputKeyUIList.Add(lbOneTrans);
            inputKeyUIList.Add(lbHideTranslate);


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
                        obj.SetKeyList(key);
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
            if (transKeyInputLabel.isFocus || quickKeyInputLabel.isFocus || dicKeyInputLabel.isFocus || snapShotInputLabel.isFocus || lbOneTrans.isFocus)
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
                    StopTrans();
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
                    _processTranslateService.PauseAndRestartTranslate(SetCaptureArea, OcrMethodType.Once);
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
                    }
                }
            }
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
            outlineColor1 = Color.FromArgb(192, 192, 192);      //old : 100 / 149 / 237
            outlineColor2 = Color.FromArgb(0, 0, 0);       //old : 65 / 105 / 225
            backgroundColor = Color.FromArgb(145, 0, 0, 0);      // 0,0,0

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

            acd.ColorChanged += delegate
            {
                System.Diagnostics.Debug.WriteLine("Color changed: " + acd.Color);
            };

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
            bool paidVersion = false;
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
            try
            {
                using (StreamWriter newTask = new StreamWriter(GlobalDefine.GOOGLE_ACCOUNT_FILE, false))
                {
                    newTask.WriteLine(googleSheet_textBox.Text);
                    newTask.WriteLine(textBox_GoogleClientID.Text);
                    newTask.WriteLine(textBox_GoogleSecretKey.Text);
                    newTask.Close();
                }
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(GlobalDefine.GOOGLE_ACCOUNT_FILE))
                {
                    fs.Close();
                    fs.Dispose();
                    using (StreamWriter newTask = new StreamWriter(GlobalDefine.GOOGLE_ACCOUNT_FILE, false))
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
                StreamReader r = new StreamReader(GlobalDefine.GOOGLE_ACCOUNT_FILE);
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

        #endregion


        public void ProcessTrans(OcrMethodType ocrMethodType)              //번역 시작 쓰레드
        {
            _processTranslateService.OcrProcessSpeed = _ocrProcessSpeed;
            _processTranslateService.ProcessTrans(ocrMethodType);
        }

        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::외부 조작 함수:::::::::::::::::::::::::::::::::::::::::::

        public void SetUseColorGroup()
        {
            ClearOcrColorSet();
            for (int i = 0; i < MySettingManager.NowOCRGroupcount; i++)
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
                if (!MySettingManager.isUseAttachedCapture && MySettingManager.NowIsActiveWindow)
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

                    try
                    {

                        System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/221904784013");
                    }
                    catch { }
                }
                return;
            }

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
            else if(MySettingManager.NowTransType == SettingManager.TransType.papago_web && !TransManager.s_CheckedPapagoWebWarning)
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

                if (!(MySettingManager.OCRType == SettingManager.OcrType.Window || MySettingManager.OCRType == SettingManager.OcrType.Google))
                {
                    isError = true;
                    errorMsg = LocalizeString("Overlay Error OCR");
                }
                else if (!MySettingManager.isUseAttachedCapture && !MySettingManager.NowIsActiveWindow)
                {
                    isError = true;
                    errorMsg = LocalizeString("Overlay Error Window", true);
                }


                if (isError)
                {
                    errorMsg += System.Environment.NewLine + LocalizeString("Overlay Error Guide Link");


                    if (MessageBox.Show(errorMsg, LocalizeString("Overlay Error Title"), MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/222233614879");
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

        public void StopTrans(bool isOnceTrans = false)
        {
            _processTrans = false;

            FormManager.Instace.MyRemoteController.ToggleStartButton(false);
            _processTranslateService.StopTranslate();

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

        public void SetCaptureArea()
        {
            int BorderWidth = Util.ocrFormBorder;
            int TitlebarHeight = Util.ocrFormTitleBar;

            FormManager.BorderWidth = Util.GetBorderWidth();
            FormManager.BorderHeight = +SystemInformation.FrameBorderSize.Height;
            FormManager.TitlebarHeight = Util.GetTitlebarHeight();
            locationXList = new List<int>();
            locationYList = new List<int>();
            sizeXList = new List<int>();
            sizeYList = new List<int>();

            exceptionLocationXList.Clear();
            exceptionLocationYList.Clear();
            exceptionSizeXList.Clear();
            exceptionSizeYList.Clear();

            List<int> tempXList = new List<int>();
            List<int> tempYList = new List<int>();
            List<int> tempSizeXList = new List<int>();
            List<int> tempSizeYList = new List<int>();

            //2019 01 01
            //스냅샷이 있으면 모든걸 없애버린다.
            bool isSnapShot = false;
            if (FormManager.Instace.snapOcrAreaForm != null)
            {
                isSnapShot = true;
            }

            if (isSnapShot)
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

                MySettingManager.LastSnapShotRect = new Rectangle(quickX, quickY, quickSizeX, quickSizeY); ;
            }


            for (int i = 0; i < FormManager.Instace.OcrAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.OcrAreaFormList[i];

                int locationX = foundedForm.Location.X + BorderWidth;
                int locationY = foundedForm.Location.Y + TitlebarHeight;
                int sizeX = Math.Max(foundedForm.Size.Width - BorderWidth * 2 , 1);
                int sizeY = Math.Max(foundedForm.Size.Height - TitlebarHeight - BorderWidth, 1);
               
                Util.ShowLog("!!!!! " + locationY + " size y : " + sizeY);
                locationXList.Add(locationX);
                locationYList.Add(locationY);
                sizeXList.Add(sizeX);
                sizeYList.Add(sizeY);

                if (!isSnapShot)
                {
                    tempXList.Add(locationX);
                    tempYList.Add(locationY);
                    tempSizeXList.Add(sizeX);
                    tempSizeYList.Add(sizeY);
                }
            }

            //OCR 설정 저장용
            MySettingManager.NowOCRGroupcount = locationYList.Count;
            MySettingManager.NowLocationXList = locationXList;
            MySettingManager.NowLocationYList = locationYList;
            MySettingManager.NowSizeXList = sizeXList;
            MySettingManager.NowSizeYList = sizeYList;


            //제외 영역 설정
            for (int i = 0; i < FormManager.Instace.exceptionAreaFormList.Count; i++)
            {
                OcrAreaForm foundedForm = FormManager.Instace.exceptionAreaFormList[i];

                int locationX = foundedForm.Location.X + BorderWidth;
                int locationY = foundedForm.Location.Y + TitlebarHeight;
                int sizeX = foundedForm.Size.Width - BorderWidth * 2;
                int sizeY = foundedForm.Size.Height - TitlebarHeight - BorderWidth;
                Util.ShowLog("Exception  " + locationY + " size y : " + sizeY);

                exceptionLocationXList.Add(locationX);
                exceptionLocationYList.Add(locationY);
                exceptionSizeXList.Add(sizeX);
                exceptionSizeYList.Add(sizeY);
            }


            if (FormManager.Instace.quickOcrAreaForm != null && !isSnapShot)
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

            // TODO : 이걸 여기서 해야하나??
            _processTranslateService.PauseAndRestartTranslate(() =>
            {
                setCutPoint(tempXList.ToArray(), tempYList.ToArray(), tempSizeXList.ToArray(), tempSizeYList.ToArray(), tempXList.Count);
                SetExceptPoint(exceptionLocationXList.ToArray(), exceptionLocationYList.ToArray(), exceptionSizeXList.ToArray(), exceptionSizeYList.ToArray(), exceptionLocationXList.Count);
                SetUseColorGroup();
            });
        }


        public void MakeCaptureArea()            //영역 검색 버튼 클릭
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

                e.Cancel = true;//종료를 취소하고 
            }
        }

        #region:::::::::::::::::::::::::::::::::::::::::::체크박스 및 라디오 클릭:::::::::::::::::::::::::::::::::::::::::::

        private bool isLockImgCheckBox = false;
        private void SetImgCheckBox(bool isUseHsv, bool isUseRgb, bool isUseThreshold)
        {

            if (!isLockImgCheckBox && eCurrentState == eCurrentStateType.None)
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
            if (groupCombo.SelectedIndex == 0)  //아이템 추가
            {
                colorGroup.Add(new ColorGroup());

                groupCombo.Items.Add(groupCombo.Items.Count - 1);       //카운트는 1부터 시작
                nowColorGroupIndex = groupCombo.Items.Count - 3;        //나우 인덱스는 0부터 시작 (실질적인 숫자는 2부터 시작) -> 번호 2 = 카운트 4 / 나우는 1 이어야 함
                groupCombo.SelectedIndex = groupCombo.Items.Count - 1;  //현재 선택 -> 가장 위

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
                    groupCombo.Items.RemoveAt(nowColorGroupIndex + 2);      //나우 + 2 = 실질적인 콤보박스 번호
                    if (nowColorGroupIndex == 0)
                    {
                        groupCombo.SelectedIndex = 2;
                        removePoint = 2;
                    }
                    else
                    {
                        groupCombo.SelectedIndex = nowColorGroupIndex + 1;      //나우 + 1 = 지우기 전 이전
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
                nowColorGroupIndex = groupCombo.SelectedIndex - 2;      //나우 + 2 = 그룹의 숫자 인덱스
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

            eCurrentState = eCurrentStateType.Accept;
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

            _isDoingClipboard = false;  //적용을 누르면 클립보드 상태를 강제로 해제
            eCurrentState = eCurrentStateType.None;
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
            try
            {
                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/70179867557");
            }
            catch { }

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


        private void panealBorder_Paint(object sender, PaintEventArgs e)        //패널에 경계선 칠하기 함수
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
            eCurrentState = eCurrentStateType.SaveFile;
            _processTranslateService.PauseAndRestartTranslate(ApplyUIValueToSetting);

            SaveFileDialog savePanel = new SaveFileDialog();
            savePanel.RestoreDirectory = false;
            savePanel.InitialDirectory = System.Environment.CurrentDirectory + "\\setting";
            savePanel.Filter = "Config File (*.conf)|*.conf";
            if (savePanel.ShowDialog() == DialogResult.OK)
            {
                SaveSetting(savePanel.FileName);
            }

            eCurrentState = eCurrentStateType.None;
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

                if (file != "")
                {
                    OpenSettingFile(file);
                }
            }
        }

        private void settingDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eCurrentState = eCurrentStateType.SetDefault;
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
            eCurrentState = eCurrentStateType.None;
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
            try
            {

                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/221904784013");
            }
            catch { }
        }

        private void error_Information_Button_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/70185869419");
            }
            catch { }
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
        private VersionCheckLogic _versionCheckLogic;

        //OCR 방식 변경
        private void OCR_Type_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tesseract_panel.Visible = false;
            WinOCR_panel.Visible = false;
            pnNHocr.Visible = false;
            pnGoogleOcr.Visible = false;

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
                            try
                            {
                                System.Diagnostics.Process.Start("https://blog.naver.com/killkimno/220865537274");
                            }
                            catch { }
                        }

                        isShowWinOCRWarning = true;
                    }
                }
            }
            else if (ocrType == SettingManager.OcrType.NHocr)
            {
                pnNHocr.Visible = true;
                naverTransComboBox.SelectedIndex = 1;
                googleTransComboBox.SelectedIndex = 1;
                removeSpaceCheckBox.Checked = true;
                cbPerWordDic.Checked = false;
            }
            else if (ocrType == SettingManager.OcrType.Google)
            {
                pnGoogleOcr.Visible = true;
            }

            //유저가 변경한 상태다.
            if (eCurrentState == eCurrentStateType.None)
            {
                bool isRequireChange = false;
                int languageType = 0;   //0 = 영어, 1 = 일본어 , //2 = 기타

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
                else if (beforeOcrPanelType == SettingManager.OcrType.NHocr)
                {
                    isRequireChange = true;
                    languageType = 1;
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
                                if (Util.GetIsEqualWinCode(winLanguageCodeList[i], code))
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
                }
            }

            beforeOcrPanelType = ocrType;
        }

        //번역 방식 변경.
        private void TransType_Combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DB_Panel.Visible = false;
            Naver_Panel.Visible = false;
            Google_Panel.Visible = false;
            pnGoogleBasic.Visible = false;
            pnDeepl.Visible = false;
            pnEzTrans.Visible = false;

            if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.db)
            {
                DB_Panel.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.naver)
            {
                Naver_Panel.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.google)
            {
                Google_Panel.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.deepl)
            {
                pnDeepl.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.google_url)
            {
                pnGoogleBasic.Visible = true;
            }
            else if (TransType_Combobox.SelectedIndex == (int)SettingManager.TransType.ezTrans)
            {
                pnEzTrans.Visible = true;
            }
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



        /// <summary>
        /// win ocr 언어 변경시 적용
        /// </summary>
        /// <param name="resultCode"></param>
        private void SetTransLangugage(string resultCode)
        {
            Util.ShowLog("OCR Code : " + resultCode);
            TransManager.TransCodeData codeData = TransManager.Instace.GetTransCodeData(resultCode);

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
            string resultCode = "";
            if (index < winLanguageCodeList.Count)
            {
                //Util.ShowLog(languageCodeList[WinOCR_Language_comboBox.SelectedIndex]);
                resultCode = winLanguageCodeList[index];
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
            SetTransLangugage(resultCode);

            if (WinOCR_Language_comboBox.SelectedIndex != index)
            {
                WinOCR_Language_comboBox.SelectedIndex = index;
            }
        }


        private void cbGoogleOcrLanguge_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eCurrentState == eCurrentStateType.None)
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
            FormManager.Instace.SetTemporaryDisableTopMostTransform();

            FormManager.Instace.ShowDonatePage();

            FormManager.Instace.ResetTemporaryDisableTopMostTransform();
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
            notifyIcon1.Visible = false;
            notifyIcon1.Icon = null;
        }

        public void ForceDisableTopMost()
        {
            isTranslateFormTopMostFlag = false;
            setTranslateTopMostToolStripMenuItem.Checked = false;
        }       
    }
}
