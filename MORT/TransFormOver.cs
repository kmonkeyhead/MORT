using R3;
using System;
using MORT.Model.Debug;
using MORT.Service.Debug;
using MORT.Service.Overlay;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    public partial class TransFormOver : Form, ITransform
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TaskIndex { get; private set; }
        public int makeIndex = 0;
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint affinity);
        const uint WDA_EXCLUDEFROMCAPTURE = 0x00000011;
        const uint WDA_NONE = 0x00;

        // 키보드 후킹을 위한 변수
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        #region Win32 Keyboard Hook Setup

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using(var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using(var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(13, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(_hookID); // 프로그램 종료 시 후킹 해제
            base.OnFormClosing(e);
        }
        #endregion



        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        #region Native Methods and Structures

        const Int32 WS_EX_LAYERED = 0x80000;
        const Int32 HTCAPTION = 0x02;
        const Int32 WM_NCHITTEST = 0x84;
        const Int32 ULW_ALPHA = 0x02;
        const byte AC_SRC_OVER = 0x00;
        const byte AC_SRC_ALPHA = 0x01;

        [StructLayout(LayoutKind.Sequential)]
        struct FramePoint
        {
            public Int32 x;
            public Int32 y;

            public FramePoint(Int32 x, Int32 y)
            { this.x = x; this.y = y; }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct FrameSize
        {
            public Int32 cx;
            public Int32 cy;

            public FrameSize(Int32 cx, Int32 cy)
            { this.cx = cx; this.cy = cy; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst,
            ref FramePoint pptDst, ref FrameSize psize, IntPtr hdcSrc, ref FramePoint pprSrc,
            Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject(IntPtr hObject);

        #endregion

        public static bool isActiveGDI = true;


        string resultText = "MORT 1.161V\n레이어 번역창";
        byte alpha = 0;
        private Point mousePoint;
        StringFormat stringFormat = new StringFormat();
        bool isTopMostFlag = true;
        bool isDestroyFormFlag = false;
        bool _isStart = false;
        private bool _attchedCapture;

        private int adjustX = 0;
        private int adjustY = 0;
        private bool _enableRTL;


        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TranslateStatusType TranslateStatusType { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseTopMostOptionWhenTranslate { get; private set; }

        int sizeX;
        int sizeY;

        private List<OCRDataManager.ResultData> _dataList = null;
        private int clientPositionX = 0;
        private int clientPositionY = 0;
        private Rectangle _lastDisplayRect = Rectangle.Empty;
        private readonly Dictionary<int, OverlayDataCache> _overlayDataCache = new Dictionary<int, OverlayDataCache>();
        Bitmap bitmap = null;

        private readonly IDisposable _disposable;

        private class OverlayDataCache
        {
            public readonly Rectangle AreaRect;
            public readonly int ClientPositionX;
            public readonly int ClientPositionY;
            public readonly string OcrText;
            public readonly string TransText;
            public readonly OCRDataManager.ResultData Data;

            public OverlayDataCache(Rectangle areaRect, int clientPositionX, int clientPositionY, string ocrText, string transText, OCRDataManager.ResultData data)
            {
                AreaRect = areaRect;
                ClientPositionX = clientPositionX;
                ClientPositionY = clientPositionY;
                OcrText = ocrText;
                TransText = transText;
                Data = data;
            }
        }


        //번역창에 번역문 출력
        private delegate void myDelegate(string transText, string ocrText, bool isShowOCRResultFlag);
        private void updateProgress(string transText, string ocrText, bool isShowOCRResultFlag)
        {

            if(transText.CompareTo("not thing") == 0)
            {
                transText = "";
            }

            Util.ShowLog(transText + " / " + ocrText);
            resultText = transText;
            if(isShowOCRResultFlag == true)
            {
                resultText += "\r\n" + "OCR : " + ocrText;
            }
        }

        public void UpdateText(List<OCRDataManager.ResultData> dataList, bool isShowOCRResultFlag, int positionX, int positionY)
        {
            this.clientPositionX = positionX;
            this.clientPositionY = positionY;

            if(dataList != null)
            {
                _dataList = GetStableOverlayDataList(dataList);
            }
            else
            {
                _dataList = null;
                _overlayDataCache.Clear();
            }

            Util.CheckTimeSpan(true);
            try
            {
                string transText = "";
                string ocrText = "";

                if(_dataList != null)
                {
                    for(int i = 0; i < _dataList.Count; i++)
                    {
                        ocrText += _dataList[i].GetOCR();
                        transText += _dataList[i].GetTrans();
                    }
                }
                this.BeginInvoke(new myDelegate(updateProgress), new object[] { transText, ocrText, isShowOCRResultFlag });
            }
            catch(InvalidOperationException)
            {
                // Error logging, post processing etc.
                return;
            }
            this.BeginInvoke(new Action(UpdatePaint));

            Util.CheckTimeSpan(false);

            //  UpdatePaint();
        }

        private List<OCRDataManager.ResultData> GetStableOverlayDataList(List<OCRDataManager.ResultData> dataList)
        {
            List<OCRDataManager.ResultData> stableList = new List<OCRDataManager.ResultData>();
            HashSet<int> currentIndexSet = new HashSet<int>();

            foreach(var data in dataList)
            {
                currentIndexSet.Add(data.Index);

                Rectangle areaRect = GetAreaRect(data);
                string ocrText = data.GetOCR();
                string transText = data.TransString;

                if(_overlayDataCache.TryGetValue(data.Index, out var cache)
                    && cache.AreaRect == areaRect
                    && cache.ClientPositionX == clientPositionX
                    && cache.ClientPositionY == clientPositionY
                    && cache.OcrText == ocrText
                    && cache.TransText == transText)
                {
                    cache.Data.ReplaceAutoColor(data);
                    stableList.Add(cache.Data);
                    continue;
                }

                _overlayDataCache[data.Index] = new OverlayDataCache(areaRect, clientPositionX, clientPositionY, ocrText, transText, data);
                stableList.Add(data);
            }

            foreach(var key in _overlayDataCache.Keys.ToList())
            {
                if(!currentIndexSet.Contains(key))
                {
                    _overlayDataCache.Remove(key);
                }
            }

            return stableList;
        }

        private Rectangle GetAreaRect(OCRDataManager.ResultData data)
        {
            if(data.SnapShot)
            {
                return FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect;
            }

            return FormManager.Instace.MyMainForm.GetOcrAreaProcessRect(data.Index);
        }


        #region ::::::::::: 레이어 창 생성 ::::::::::

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // 이 창(오버레이)을 모든 스크린 캡처 도구에서 보이지 않게 설정합니다.
            // 이제 캡처 시 창을 Hide/Show 하지 않아도 됩니다.
            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);

            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }


        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode >= 0 && (wParam == (IntPtr)0x0100 || wParam == (IntPtr)0x0104)) // WM_KEYDOWN 또는 WM_SYSKEYDOWN
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                // 1. PrintScreen 키 (단일 키)
                bool isPrtSc = (key == Keys.PrintScreen);

                // 2. Win + Shift + S 감지 (조합 키)
                // S 키가 눌렸을 때, Win 키와 Shift 키가 현재 눌려있는지 상태 확인
                bool isWinShiftS = (key == Keys.S &&
                                   (GetAsyncKeyState((int)Keys.LWin) < 0 || GetAsyncKeyState((int)Keys.RWin) < 0) &&
                                   (GetAsyncKeyState((int)Keys.ShiftKey) < 0));

                if(!_attchedCapture && !_forceLock && (isPrtSc || isWinShiftS))
                {
                    _forceLock = true;
                    // 캡처에 포함되도록 즉시 변경
                    SetWindowDisplayAffinity(this.Handle, WDA_NONE);

                    // 1초 후 자동 복구
                    var timer = new System.Windows.Forms.Timer { Interval = 5000 };
                    timer.Tick += (s, e) =>
                    {
                        _forceLock = false;
                        if(!_attchedCapture)
                        {
                            SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
                        }

                        timer.Stop();
                        timer.Dispose();
                    };
                    timer.Start();
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private void Init()
        {

            if(FormManager.Instace.MyMainForm.MySettingManager.NowSortType == SettingManager.SortType.Normal)
            {
                stringFormat.Alignment = StringAlignment.Near;
            }
            else
            {
                stringFormat.Alignment = StringAlignment.Center;
            }

            ApplyRTL(AdvencedOptionManager.EnableRTL);
        }

        public TransFormOver()
        {
            InitializeComponent();
            Init();

            _disposable = SettingManager.AttachedCapture.Subscribe(ApplyAttachedCapture);
        }

        protected override void OnClosed(EventArgs e)
        {
            _disposable.Dispose();
            base.OnClosed(e);
        }

        private void ApplyAttachedCapture(bool attached)
        {
            _attchedCapture = attached;
            if(_attchedCapture)
            {
                SetWindowDisplayAffinity(this.Handle, WDA_NONE);
            }
            else
            {
                SetWindowDisplayAffinity(this.Handle, WDA_EXCLUDEFROMCAPTURE);
            }
        }

        public void SetAdjustPosition(int x, int y)
        {
            this.adjustX = x;
            this.adjustY = y;
        }

        public void CheckSizeAndLocation()
        {
            //스크린 캡쳐 아래아로 해야 함.
            Rectangle rect = Rectangle.Empty;
            if(FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect != Rectangle.Empty)
            {
                rect = FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect;
            }
            else
            {
                rect = FormManager.Instace.MyMainForm.GetOcrAreaProcessFullRect();
            }


            rect.Width = (int)(rect.Width * 1.3);
            rect.Height = (int)(rect.Height * 1.3);

            if(_isStart)
            {
                if(_lastDisplayRect != Rectangle.Empty && _lastDisplayRect.Contains(rect))
                {
                    rect = _lastDisplayRect;
                }
                else if(_lastDisplayRect != Rectangle.Empty)
                {
                    rect = Rectangle.Union(_lastDisplayRect, rect);
                }
            }

            if(_lastDisplayRect != rect)
            {
                _lastDisplayRect = rect;
            }

            if(this.Size != rect.Size)
            {
                this.Size = rect.Size;
            }

            if(this.Location != rect.Location)
            {
                this.Location = rect.Location;
            }
        }

        public void HideTaksBar()
        {

            ShowWindow((int)this.Handle, 0);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                // Add the layered extended style (WS_EX_LAYERED) to this window.
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_LAYERED;
                return createParams;
            }
        }


        #endregion

        public void UpdateTransform()
        {
            Init();
            UpdatePaint();
        }

        private OcrDebugSnapshotService _debugSnapshotService;
        private OcrDebugSnapshotService DebugSnapshotService
            => _debugSnapshotService ??= Program.ServiceContainer?.GetService(typeof(OcrDebugSnapshotService)) as OcrDebugSnapshotService;

        private readonly record struct OverlayDrawColors(
            Color Font,
            Color Background,
            bool UsesAutomaticColor,
            bool Corrected,
            Color Outline1,
            Color Outline2);

        private sealed class OverlayRenderBlock
        {
            public OCRDataManager.ResultData TargetData;
            public OCRDataManager.TransData TransData;
            public int ColorIndex;
            public Rectangle CaptureRect;
            public Rectangle SourceRect;
            public Rectangle ViewRect;
            public Rectangle ContentRect;
            public bool VerticalMode;
        }

        private void AddText(GraphicsPath gp, Graphics g, Font textFont, Rectangle rectangleOriginal, StringFormat sf)
        {
            if(!isActiveGDI || _dataList == null)
            {
                return;
            }

            Color outlineColor1 = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor1;
            Color outlineColor2 = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor2;
            const int outlineWidth1 = 2;
            const int outlineWidth2 = 5;
            FontStyle overlayFontStyle = AdvencedOptionManager.OverlayUseFontOutline
                ? textFont.Style
                : textFont.Style & ~FontStyle.Bold;
            using Font overlayBaseFont = new Font(textFont.FontFamily, textFont.SizeInPoints, overlayFontStyle, GraphicsUnit.Point);

            List<OverlayRenderBlock> blocks = BuildRenderBlocks();
            ResolveBlockCollisions(blocks);

            //디버깅 : 실제로 그린 값 그대로를 모아 스냅샷에 넘긴다.
            var debugService = DebugSnapshotService;
            List<OcrDebugOverlayBlock> debugBlocks =
                Form1.IsDebugSaveAnalysisResult && debugService != null && debugService.IsWaitingOverlay
                    ? new List<OcrDebugOverlayBlock>()
                    : null;

            foreach(var block in blocks)
            {
                using StringFormat blockFormat = (StringFormat)sf.Clone();
                ConfigureStringFormat(blockFormat, block.VerticalMode);

                Rectangle contentRect = GetContentRect(block.ViewRect);
                if(contentRect.Width <= 0 || contentRect.Height <= 0)
                {
                    block.TransData.ViewRect = block.ViewRect;
                    block.TransData.ContentRect = Rectangle.Empty;
                    Util.ShowLog($"Overlay block clipped: {block.TransData.trans}");
                    debugBlocks?.Add(MakeDebugOverlayBlock(
                        g, block, null, Rectangle.Empty, 0f, 0f, 0f, false, blockFormat, outlineColor1, outlineColor2, true));
                    continue;
                }

                float minimumSize = AdvencedOptionManager.IsAutoFontSize
                    ? Math.Max(1, AdvencedOptionManager.MinAutoFontSize)
                    : overlayBaseFont.SizeInPoints;
                float preferredSize = AdvencedOptionManager.IsAutoFontSize
                    ? Math.Clamp(
                        GetPreferredFontPointSize(g, block, blocks),
                        minimumSize,
                        Math.Max(minimumSize, AdvencedOptionManager.MaxAutoFontSize))
                    : overlayBaseFont.SizeInPoints;

                if(AdvencedOptionManager.IsAutoFontSize)
                {
                    TryExpandInReadingDirection(g, block, blocks, overlayBaseFont, blockFormat, preferredSize);
                    TryExpandForFont(g, block, blocks, overlayBaseFont, blockFormat, minimumSize);
                    contentRect = GetContentRect(block.ViewRect);
                }

                float fontSize = AdvencedOptionManager.IsAutoFontSize
                    ? FindBestFontSize(g, block, overlayBaseFont, contentRect, blockFormat, preferredSize)
                    : overlayBaseFont.SizeInPoints;

                using Font renderFont = new Font(overlayBaseFont.FontFamily, fontSize, overlayBaseFont.Style, GraphicsUnit.Point);
                block.ContentRect = contentRect;
                block.TransData.ViewRect = block.ViewRect;
                block.TransData.ContentRect = contentRect;
                if(Form1.IsDebugShowWordArea)
                {
                    Util.ShowLog(
                        $"Overlay layout: source={block.SourceRect}, view={block.ViewRect}, content={contentRect}, " +
                        $"font={fontSize:0.00}, sourceFont={GetSourceFontPointSize(g, block):0.00}, text={block.TransData.trans}");
                }

                bool drawBackground = false;
                if(_isStart && Form1.IsDebugShowWordArea)
                {
                    using var debugBrush = new SolidBrush(Color.FromArgb(90, 0, 0, 0));
                    g.FillRectangle(debugBrush, block.SourceRect);
                }
                else if(_isStart && FormManager.Instace.MyMainForm.MySettingManager.NowIsUseBackColor)
                {
                    drawBackground = true;
                    Color background = GetOverlayBackgroundColor(FormManager.Instace.MyMainForm.MySettingManager.BackgroundColor);
                    if(block.TargetData.UseAutoColor
                        && AdvencedOptionManager.OverlayAutoBackgroundColor
                        && block.TargetData.TryGetAutoColor(block.ColorIndex, out var autoColor))
                    {
                        Color sampled = autoColor.BackGround;
                        background = Color.FromArgb(background.A, sampled);
                    }

                    using var backgroundBrush = new SolidBrush(background);
                    g.FillRectangle(backgroundBrush, block.ViewRect);
                }

                DrawWrappedText(g, block, renderFont, blockFormat, outlineColor1, outlineWidth1, outlineColor2, outlineWidth2);

                bool clipped = !DoesTextFit(g, block.TransData.trans, renderFont, contentRect, blockFormat, block.VerticalMode);
                if(clipped)
                {
                    Util.ShowLog($"Overlay block clipped at minimum font: {block.TransData.trans}");
                }

                debugBlocks?.Add(MakeDebugOverlayBlock(
                    g, block, renderFont, contentRect, fontSize, preferredSize, minimumSize, drawBackground,
                    blockFormat, outlineColor1, outlineColor2, clipped));
            }

            //스냅샷 저장은 DoUpdatePaint 가 화면 반영까지 끝낸 뒤에 한다.
            //여기서 바로 넘기면 GetHbitmap / UpdateLayeredWindow 시간을 담을 수 없다.
            _pendingDebugBlocks = debugBlocks;
        }

        //AddText 가 모은 디버깅 블록. DoUpdatePaint 가 마지막에 넘긴다.
        private List<OcrDebugOverlayBlock> _pendingDebugBlocks;

        /// <summary>
        /// 디버깅 스냅샷용 : 이 블록을 실제로 어떻게 그렸는지 최종값을 모은다.
        /// </summary>
        private OcrDebugOverlayBlock MakeDebugOverlayBlock(
            Graphics g,
            OverlayRenderBlock block,
            Font renderFont,
            Rectangle contentRect,
            float fontSize,
            float preferredSize,
            float minimumSize,
            bool drawBackground,
            StringFormat blockFormat,
            Color outlineColor1,
            Color outlineColor2,
            bool clipped)
        {
            OverlayDrawColors colors = ResolveDrawColors(
                block.TargetData, block.ColorIndex, block.TransData.trans, outlineColor1, outlineColor2);

            var wrappedLines = new List<string>();
            float lineAdvance = 0f;
            if(renderFont != null)
            {
                wrappedLines = GetWrappedLinesByAddString(
                    g, block.TransData.trans, renderFont, contentRect.Width, contentRect.Height, blockFormat, block.VerticalMode);
                lineAdvance = renderFont.GetHeight(g) * 1.2f;
            }

            return new OcrDebugOverlayBlock
            {
                AreaIndex = block.TargetData.Index,
                ColorIndex = block.ColorIndex,
                Text = block.TransData.trans,
                IsTitle = block.TransData.TitleData,
                VerticalMode = block.VerticalMode,
                CaptureRect = OcrDebugRect.From(block.CaptureRect),
                SourceRect = OcrDebugRect.From(block.SourceRect),
                ViewRect = OcrDebugRect.From(block.ViewRect),
                ContentRect = OcrDebugRect.From(contentRect),
                FontFamily = renderFont?.FontFamily.Name ?? "",
                FontStyle = renderFont?.Style.ToString() ?? "",
                FontSize = fontSize,
                PreferredFontSize = preferredSize,
                MinimumFontSize = minimumSize,
                SourceFontSize = GetSourceFontPointSize(g, block),
                FontColor = OcrDebugSnapshotService.ToColorText(colors.Font),
                BackgroundColor = OcrDebugSnapshotService.ToColorText(colors.Background),
                DrawBackground = drawBackground,
                UseAutoColor = colors.UsesAutomaticColor,
                ContrastCorrected = colors.Corrected,
                UseOutline = AdvencedOptionManager.OverlayUseFontOutline,
                OutlineColor1 = OcrDebugSnapshotService.ToColorText(colors.Outline1),
                OutlineColor2 = OcrDebugSnapshotService.ToColorText(colors.Outline2),
                WrappedLines = wrappedLines,
                LineAdvance = lineAdvance,
                Clipped = clipped,
            };
        }

        private List<OverlayRenderBlock> BuildRenderBlocks()
        {
            var blocks = new List<OverlayRenderBlock>();
            double zoom = Math.Max(0.01, FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);

            foreach(var targetData in _dataList)
            {
                Rectangle areaRect;
                try
                {
                    areaRect = targetData.SnapShot
                        ? FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect
                        : FormManager.Instace.MyMainForm.GetOcrAreaProcessRect(targetData.Index);
                }
                catch(Exception ex)
                {
                    Util.ShowLog(ex.Message);
                    continue;
                }

                if(areaRect == Rectangle.Empty)
                {
                    continue;
                }

                int originX = areaRect.X - FormManager.BorderWidth / 2;
                int originY = areaRect.Y - FormManager.BorderHeight / 2;
                if(_attchedCapture)
                {
                    originX = Math.Max(originX, clientPositionX);
                    originY = Math.Max(originY, clientPositionY);
                }
                Rectangle captureRect = new Rectangle(
                    originX - Location.X,
                    originY - Location.Y,
                    areaRect.Width,
                    areaRect.Height);
                captureRect = Rectangle.Intersect(ClientRectangle, captureRect);

                for(int colorIndex = 0; colorIndex < targetData.TransDataList.Count; colorIndex++)
                {
                    var transData = targetData.TransDataList[colorIndex];
                    Rectangle source = transData.SourceRect == Rectangle.Empty ? transData.lineRect : transData.SourceRect;
                    Rectangle screenRect = Rectangle.FromLTRB(
                        originX + (int)Math.Floor(source.Left / zoom) - Location.X,
                        originY + (int)Math.Floor(source.Top / zoom) - Location.Y,
                        originX + (int)Math.Ceiling(source.Right / zoom) - Location.X,
                        originY + (int)Math.Ceiling(source.Bottom / zoom) - Location.Y);
                    screenRect = Rectangle.Intersect(captureRect, screenRect);
                    if(screenRect.Width <= 0 || screenRect.Height <= 0)
                    {
                        continue;
                    }

                    blocks.Add(new OverlayRenderBlock
                    {
                        TargetData = targetData,
                        TransData = transData,
                        ColorIndex = colorIndex,
                        CaptureRect = captureRect,
                        SourceRect = screenRect,
                        ViewRect = screenRect,
                        ContentRect = Rectangle.Inflate(screenRect, -4, -4),
                        VerticalMode = AdvencedOptionManager.OverlayKeepSourceDirection
                            && transData.angleType == OCRDataManager.WordAngleType.Vertical,
                    });
                }
            }

            return blocks;
        }

        private static void ResolveBlockCollisions(List<OverlayRenderBlock> blocks)
        {
            int maximumIterations = Math.Max(1, blocks.Count * blocks.Count * 4);
            for(int iteration = 0; iteration < maximumIterations; iteration++)
            {
                int firstIndex = -1;
                int secondIndex = -1;
                long largestArea = 0;

                for(int i = 0; i < blocks.Count; i++)
                {
                    for(int j = i + 1; j < blocks.Count; j++)
                    {
                        Rectangle overlap = Rectangle.Intersect(blocks[i].ViewRect, blocks[j].ViewRect);
                        long area = (long)overlap.Width * overlap.Height;
                        if(area > largestArea)
                        {
                            largestArea = area;
                            firstIndex = i;
                            secondIndex = j;
                        }
                    }
                }

                if(firstIndex < 0)
                {
                    return;
                }

                OverlayRenderBlock first = blocks[firstIndex];
                OverlayRenderBlock second = blocks[secondIndex];
                bool preserveFirst = first.TransData.TitleData && !second.TransData.TitleData;
                bool preserveSecond = second.TransData.TitleData && !first.TransData.TitleData;

                var vertical = SplitVertically(first.ViewRect, second.ViewRect, preserveFirst, preserveSecond);
                var horizontal = SplitHorizontally(first.ViewRect, second.ViewRect, preserveFirst, preserveSecond);
                if(vertical.Loss <= horizontal.Loss)
                {
                    first.ViewRect = vertical.First;
                    second.ViewRect = vertical.Second;
                }
                else
                {
                    first.ViewRect = horizontal.First;
                    second.ViewRect = horizontal.Second;
                }
            }
        }

        private static (Rectangle First, Rectangle Second, long Loss) SplitVertically(Rectangle first, Rectangle second, bool preserveFirst, bool preserveSecond)
        {
            Rectangle originalFirst = first;
            Rectangle originalSecond = second;
            Rectangle overlap = Rectangle.Intersect(first, second);
            bool firstIsLeft = first.Left + first.Width / 2.0 <= second.Left + second.Width / 2.0;
            int boundary;
            if(firstIsLeft)
            {
                boundary = preserveFirst ? first.Right
                    : preserveSecond ? second.Left
                    : GetWeightedBoundary(overlap.Left, overlap.Right, first, second);
                boundary = Math.Clamp(boundary, overlap.Left, overlap.Right);
                first = Rectangle.FromLTRB(first.Left, first.Top, boundary, first.Bottom);
                second = Rectangle.FromLTRB(boundary, second.Top, second.Right, second.Bottom);
            }
            else
            {
                boundary = preserveFirst ? first.Left
                    : preserveSecond ? second.Right
                    : GetWeightedBoundary(overlap.Left, overlap.Right, second, first);
                boundary = Math.Clamp(boundary, overlap.Left, overlap.Right);
                second = Rectangle.FromLTRB(second.Left, second.Top, boundary, second.Bottom);
                first = Rectangle.FromLTRB(boundary, first.Top, first.Right, first.Bottom);
            }

            return (first, second, GetAreaLoss(originalFirst, originalSecond, first, second));
        }

        private static (Rectangle First, Rectangle Second, long Loss) SplitHorizontally(Rectangle first, Rectangle second, bool preserveFirst, bool preserveSecond)
        {
            Rectangle originalFirst = first;
            Rectangle originalSecond = second;
            Rectangle overlap = Rectangle.Intersect(first, second);
            bool firstIsTop = first.Top + first.Height / 2.0 <= second.Top + second.Height / 2.0;
            int boundary;
            if(firstIsTop)
            {
                boundary = preserveFirst ? first.Bottom
                    : preserveSecond ? second.Top
                    : GetWeightedBoundary(overlap.Top, overlap.Bottom, first, second);
                boundary = Math.Clamp(boundary, overlap.Top, overlap.Bottom);
                first = Rectangle.FromLTRB(first.Left, first.Top, first.Right, boundary);
                second = Rectangle.FromLTRB(second.Left, boundary, second.Right, second.Bottom);
            }
            else
            {
                boundary = preserveFirst ? first.Top
                    : preserveSecond ? second.Bottom
                    : GetWeightedBoundary(overlap.Top, overlap.Bottom, second, first);
                boundary = Math.Clamp(boundary, overlap.Top, overlap.Bottom);
                second = Rectangle.FromLTRB(second.Left, second.Top, second.Right, boundary);
                first = Rectangle.FromLTRB(first.Left, boundary, first.Right, first.Bottom);
            }

            return (first, second, GetAreaLoss(originalFirst, originalSecond, first, second));
        }

        private static int GetWeightedBoundary(int start, int end, Rectangle leading, Rectangle trailing)
        {
            long leadingArea = Math.Max(1L, (long)leading.Width * leading.Height);
            long trailingArea = Math.Max(1L, (long)trailing.Width * trailing.Height);
            double leadingShare = leadingArea / (double)(leadingArea + trailingArea);
            return start + (int)Math.Round((end - start) * leadingShare);
        }

        private static long GetAreaLoss(Rectangle originalFirst, Rectangle originalSecond, Rectangle first, Rectangle second)
        {
            long originalArea = (long)originalFirst.Width * originalFirst.Height
                + (long)originalSecond.Width * originalSecond.Height;
            long remainingArea = (long)first.Width * first.Height
                + (long)second.Width * second.Height;
            return Math.Max(0, originalArea - remainingArea);
        }

        private static Rectangle GetContentRect(Rectangle viewRect)
        {
            int padding = AdvencedOptionManager.OverlayUseFontOutline ? 4 : 0;
            return Rectangle.Inflate(viewRect, -padding, -padding);
        }

        private void TryExpandInReadingDirection(Graphics g, OverlayRenderBlock block, List<OverlayRenderBlock> blocks, Font baseFont, StringFormat format, float targetSize)
        {
            if(block.VerticalMode)
            {
                return;
            }

            Rectangle currentContent = GetContentRect(block.ViewRect);
            using Font targetFont = new Font(baseFont.FontFamily, targetSize, baseFont.Style, GraphicsUnit.Point);
            if(!WrapsPastSourceLineCount(g, block, targetFont, currentContent, format))
            {
                return;
            }

            int maximumRight = block.CaptureRect.Right;
            foreach(var other in blocks)
            {
                if(ReferenceEquals(other, block)
                    || GetAxisOverlap(block.ViewRect.Top, block.ViewRect.Bottom, other.ViewRect.Top, other.ViewRect.Bottom) <= 0)
                {
                    continue;
                }
                if(other.ViewRect.Left >= block.ViewRect.Right)
                {
                    maximumRight = Math.Min(maximumRight, other.ViewRect.Left);
                }
            }

            if(maximumRight <= block.ViewRect.Right)
            {
                return;
            }

            Rectangle maximum = Rectangle.FromLTRB(block.ViewRect.Left, block.ViewRect.Top, maximumRight, block.ViewRect.Bottom);
            if(WrapsPastSourceLineCount(g, block, targetFont, GetContentRect(maximum), format))
            {
                block.ViewRect = maximum;
                return;
            }

            int low = block.ViewRect.Right;
            int high = maximumRight;
            while(low < high)
            {
                int middle = (low + high) / 2;
                Rectangle candidate = Rectangle.FromLTRB(block.ViewRect.Left, block.ViewRect.Top, middle, block.ViewRect.Bottom);
                if(!WrapsPastSourceLineCount(g, block, targetFont, GetContentRect(candidate), format))
                {
                    high = middle;
                }
                else
                {
                    low = middle + 1;
                }
            }
            block.ViewRect = Rectangle.FromLTRB(block.ViewRect.Left, block.ViewRect.Top, low, block.ViewRect.Bottom);
        }

        private bool WrapsPastSourceLineCount(Graphics g, OverlayRenderBlock block, Font font, Rectangle contentRect, StringFormat format)
        {
            int sourceLineCount = Math.Max(1, block.TransData.lineDataList.Count);
            int translatedLineCount = GetWrappedLinesByAddString(
                g, block.TransData.trans, font, contentRect.Width, contentRect.Height, format, false).Count;
            return translatedLineCount > sourceLineCount;
        }

        private void TryExpandForFont(Graphics g, OverlayRenderBlock block, List<OverlayRenderBlock> blocks, Font baseFont, StringFormat format, float targetSize)
        {
            Rectangle currentContent = GetContentRect(block.ViewRect);
            using Font targetFont = new Font(baseFont.FontFamily, targetSize, baseFont.Style, GraphicsUnit.Point);
            if(DoesTextFit(g, block.TransData.trans, targetFont, currentContent, format, block.VerticalMode))
            {
                return;
            }

            if(block.VerticalMode)
            {
                int minimumLeft = block.CaptureRect.Left;
                foreach(var other in blocks)
                {
                    if(ReferenceEquals(other, block) || GetAxisOverlap(block.ViewRect.Top, block.ViewRect.Bottom, other.ViewRect.Top, other.ViewRect.Bottom) <= 0)
                    {
                        continue;
                    }
                    if(other.ViewRect.Right <= block.ViewRect.Left)
                    {
                        minimumLeft = Math.Max(minimumLeft, other.ViewRect.Right);
                    }
                }

                Rectangle maximum = Rectangle.FromLTRB(minimumLeft, block.ViewRect.Top, block.ViewRect.Right, block.ViewRect.Bottom);
                if(DoesTextFit(g, block.TransData.trans, targetFont, GetContentRect(maximum), format, true))
                {
                    int low = minimumLeft;
                    int high = block.ViewRect.Left;
                    while(low < high)
                    {
                        int middle = (low + high + 1) / 2;
                        Rectangle candidate = Rectangle.FromLTRB(middle, block.ViewRect.Top, block.ViewRect.Right, block.ViewRect.Bottom);
                        if(DoesTextFit(g, block.TransData.trans, targetFont, GetContentRect(candidate), format, true))
                        {
                            low = middle;
                        }
                        else
                        {
                            high = middle - 1;
                        }
                    }
                    block.ViewRect = Rectangle.FromLTRB(low, block.ViewRect.Top, block.ViewRect.Right, block.ViewRect.Bottom);
                }
            }
            else
            {
                int maximumBottom = block.CaptureRect.Bottom;
                foreach(var other in blocks)
                {
                    if(ReferenceEquals(other, block) || GetAxisOverlap(block.ViewRect.Left, block.ViewRect.Right, other.ViewRect.Left, other.ViewRect.Right) <= 0)
                    {
                        continue;
                    }
                    if(other.ViewRect.Top >= block.ViewRect.Bottom)
                    {
                        maximumBottom = Math.Min(maximumBottom, other.ViewRect.Top);
                    }
                }

                Rectangle maximum = Rectangle.FromLTRB(block.ViewRect.Left, block.ViewRect.Top, block.ViewRect.Right, maximumBottom);
                if(DoesTextFit(g, block.TransData.trans, targetFont, GetContentRect(maximum), format, false))
                {
                    int low = block.ViewRect.Bottom;
                    int high = maximumBottom;
                    while(low < high)
                    {
                        int middle = (low + high) / 2;
                        Rectangle candidate = Rectangle.FromLTRB(block.ViewRect.Left, block.ViewRect.Top, block.ViewRect.Right, middle);
                        if(DoesTextFit(g, block.TransData.trans, targetFont, GetContentRect(candidate), format, false))
                        {
                            high = middle;
                        }
                        else
                        {
                            low = middle + 1;
                        }
                    }
                    block.ViewRect = Rectangle.FromLTRB(block.ViewRect.Left, block.ViewRect.Top, block.ViewRect.Right, low);
                }
            }
        }

        //폰트 크기 탐색을 여기서 멈춘다. 이보다 좁은 차이는 화면에서 구분되지 않는다.
        //탐색 한 번이 문자열 전체를 다시 재는 비용이라 반복 횟수가 페인트 시간을 좌우한다.
        private const float FontSearchEpsilon = 0.25f;

        private float FindBestFontSize(Graphics g, OverlayRenderBlock block, Font baseFont, Rectangle contentRect, StringFormat format, float preferredSize)
        {
            float minimum = Math.Max(1, AdvencedOptionManager.MinAutoFontSize);
            float maximum = Math.Max(minimum, Math.Min(AdvencedOptionManager.MaxAutoFontSize, preferredSize));

            //원하는 크기가 그대로 들어가면 더 볼 것이 없다.
            //이분 탐색은 위 끝을 직접 시험하지 않아 이 흔한 경우에도 끝까지 돈다.
            using(Font preferred = new Font(baseFont.FontFamily, maximum, baseFont.Style, GraphicsUnit.Point))
            {
                if(DoesTextFit(g, block.TransData.trans, preferred, contentRect, format, block.VerticalMode))
                {
                    return maximum;
                }
            }

            float low = minimum;
            float high = maximum;
            for(int iteration = 0; iteration < 9; iteration++)
            {
                if(high - low <= FontSearchEpsilon)
                {
                    break;
                }

                float middle = (low + high) / 2f;
                using Font candidate = new Font(baseFont.FontFamily, middle, baseFont.Style, GraphicsUnit.Point);
                if(DoesTextFit(g, block.TransData.trans, candidate, contentRect, format, block.VerticalMode))
                {
                    low = middle;
                }
                else
                {
                    high = middle;
                }
            }
            return low;
        }

        private static float GetPreferredFontPointSize(Graphics g, OverlayRenderBlock block, List<OverlayRenderBlock> blocks)
        {
            const float sourceFontScale = 1.15f;
            const float leadingTitleRatio = 1.3f;
            var peers = blocks
                .Where(peer => ReferenceEquals(peer.TargetData, block.TargetData)
                    && peer.VerticalMode == block.VerticalMode)
                .ToList();
            float blockSourceSize = GetMedianSourceLineSize(block.TransData.lineDataList);
            float bodySourceSize = GetMedianSourceLineSize(peers
                .Where(peer => !peer.TransData.TitleData)
                .SelectMany(peer => peer.TransData.lineDataList));
            OverlayRenderBlock leadingBlock = peers
                .OrderBy(peer => peer.SourceRect.Top)
                .ThenBy(peer => peer.SourceRect.Left)
                .FirstOrDefault();
            bool preserveOwnSize = block.TransData.TitleData
                || (ReferenceEquals(block, leadingBlock)
                    && bodySourceSize > 0
                    && blockSourceSize >= bodySourceSize * leadingTitleRatio);
            float preferredSourceSize = preserveOwnSize ? blockSourceSize : bodySourceSize;

            if(preferredSourceSize <= 0)
            {
                return Math.Max(1, AdvencedOptionManager.MinAutoFontSize);
            }

            double zoom = Math.Max(0.01, FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);
            return (float)(preferredSourceSize / zoom * 72.0 / g.DpiY * sourceFontScale);
        }

        private static float GetMedianSourceLineSize(IEnumerable<OCRDataManager.LineData> lineDataList)
        {
            var sizes = lineDataList
                .Select(GetSourceLineSize)
                .Where(size => size > 0)
                .OrderBy(size => size)
                .ToList();
            if(sizes.Count == 0)
            {
                return 0;
            }

            int middle = sizes.Count / 2;
            return sizes.Count % 2 == 1
                ? sizes[middle]
                : (sizes[middle - 1] + sizes[middle]) / 2f;
        }

        private static int GetSourceLineSize(OCRDataManager.LineData lineData)
        {
            int size = lineData.angleType == OCRDataManager.WordAngleType.Vertical
                ? lineData.lineRect.Width
                : lineData.lineRect.Height;
            return size > 0 ? size : OCRDataManager.GetFontSize(lineData);
        }

        private static float GetSourceFontPointSize(Graphics g, OverlayRenderBlock block)
        {
            if(block.TransData.lineDataList.Count == 0)
            {
                return 0;
            }

            double zoom = Math.Max(0.01, FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);
            var sourceSizes = block.TransData.lineDataList
                .Select(GetSourceLineSize)
                .OrderBy(size => size)
                .ToList();
            int middle = sourceSizes.Count / 2;
            float sourceMedian = sourceSizes.Count % 2 == 1
                ? sourceSizes[middle]
                : (sourceSizes[middle - 1] + sourceSizes[middle]) / 2f;
            return (float)(sourceMedian / zoom * 72.0 / g.DpiY);
        }

        private bool DoesTextFit(Graphics g, string text, Font font, Rectangle rect, StringFormat format, bool vertical)
        {
            if(string.IsNullOrEmpty(text) || rect.Width <= 0 || rect.Height <= 0)
            {
                return string.IsNullOrEmpty(text);
            }

            List<string> lines = GetWrappedLinesByAddString(g, text, font, rect.Width, rect.Height, format, vertical);
            if(lines.Count == 0)
            {
                return true;
            }

            //DrawWrappedText 가 실제로 놓는 자리에 글자를 얹어보고 rect 를 벗어나는지 본다.
            //줄 수와 줄 간격만 더해서 판단하면 마지막 줄이 자기 칸 안에서 차지하는 여백을 빠뜨려,
            //들어간다고 판단해놓고 실제로는 옆 블록 영역까지 글자가 넘어간다.
            float lineAdvance = font.GetHeight(g) * 1.2f;
            float outlinePadding = AdvencedOptionManager.OverlayUseFontOutline ? 2.5f : 0f;
            float emSize = g.DpiY * font.SizeInPoints / 72f;

            for(int index = 0; index < lines.Count; index++)
            {
                //여기서는 잘라내지 않은 칸을 그대로 쓴다. 잘라내면 넘어간 사실이 가려진다.
                Rectangle lineRect = GetLineRect(rect, index, lineAdvance, vertical);
                using var path = new GraphicsPath();
                path.AddString(lines[index], font.FontFamily, (int)font.Style, emSize, lineRect, format);
                RectangleF bounds = path.GetBounds();
                if(bounds.Width <= 0 && bounds.Height <= 0)
                {
                    //공백뿐인 줄은 그려지는 것이 없다
                    continue;
                }

                bounds.Inflate(outlinePadding, outlinePadding);
                if(bounds.Left < rect.Left || bounds.Right > rect.Right
                    || bounds.Top < rect.Top || bounds.Bottom > rect.Bottom)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 줄 하나가 놓이는 칸. 판정과 그리기가 같은 자리를 써야 한다.
        /// </summary>
        private static Rectangle GetLineRect(Rectangle contentRect, int index, float lineAdvance, bool vertical)
        {
            return vertical
                ? new Rectangle(
                    contentRect.Right - (int)Math.Ceiling((index + 1) * lineAdvance),
                    contentRect.Top,
                    (int)Math.Ceiling(lineAdvance),
                    contentRect.Height)
                : new Rectangle(
                    contentRect.Left,
                    contentRect.Top + (int)Math.Floor(index * lineAdvance),
                    contentRect.Width,
                    (int)Math.Ceiling(lineAdvance));
        }

        private void DrawWrappedText(Graphics g, OverlayRenderBlock block, Font font, StringFormat format, Color outlineColor1, int outlineWidth1, Color outlineColor2, int outlineWidth2)
        {
            List<string> lines = GetWrappedLinesByAddString(g, block.TransData.trans, font, block.ContentRect.Width, block.ContentRect.Height, format, block.VerticalMode);
            float advance = font.GetHeight(g) * 1.2f;
            for(int index = 0; index < lines.Count; index++)
            {
                Rectangle lineRect = GetLineRect(block.ContentRect, index, advance, block.VerticalMode);
                lineRect = Rectangle.Intersect(lineRect, block.ContentRect);
                DrawStringWithOutline2(g, lines[index], font, lineRect, format, block.TargetData, block.ColorIndex, outlineColor1, outlineWidth1, outlineColor2, outlineWidth2);
            }
        }

        private void ConfigureStringFormat(StringFormat format, bool vertical)
        {
            if(vertical)
            {
                format.FormatFlags |= StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
                return;
            }

            format.FormatFlags &= ~StringFormatFlags.DirectionVertical;
            if(_enableRTL)
            {
                format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }
            else
            {
                format.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
            }
        }

        private static int GetAxisOverlap(int firstStart, int firstEnd, int secondStart, int secondEnd)
        {
            return Math.Max(0, Math.Min(firstEnd, secondEnd) - Math.Max(firstStart, secondStart));
        }

        private void AddTextLegacy(GraphicsPath gp, Graphics g, Font textFont, Rectangle rectangleOriginal, StringFormat sf)
        {
            if(!isActiveGDI)
                return;

            Rectangle rectangle = rectangleOriginal;
            SolidBrush backColorBrush = new SolidBrush(GetOverlayBackgroundColor(FormManager.Instace.MyMainForm.MySettingManager.BackgroundColor));
            SolidBrush defualtColorBrush = new SolidBrush(Color.FromArgb(90, 0, 0, 0));
            Color outlineColor1 = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor1;
            Color outlineColor2 = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor2;
            int outlineWidth1 = 2;
            int outlineWidth2 = 5;

            if(_dataList != null)
            {
                for(int i = 0; i < _dataList.Count; i++)
                {
                    int x = 0, y = 0;

                    try
                    {
                        if(_dataList[i].SnapShot)
                        {
                            x = FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect.X;
                            y = FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect.Y;
                        }
                        else
                        {
                            Rectangle ocrAreaRect = FormManager.Instace.MyMainForm.GetOcrAreaProcessRect(_dataList[i].Index);
                            if(ocrAreaRect == Rectangle.Empty)
                            {
                                continue;
                            }

                            x = ocrAreaRect.X;
                            y = ocrAreaRect.Y;
                        }
                    }
                    catch(Exception ex)
                    {
                        Util.ShowLog(ex.Message);
                        continue;
                    }

                    y = y - FormManager.BorderHeight / 2;
                    x = x - FormManager.BorderWidth / 2;

                    Util.ShowLog($"{x} / {y}");

                    if(_attchedCapture)
                    {
                        if(x < clientPositionX)
                            x = clientPositionX;
                        if(y < clientPositionY)
                            y = clientPositionY;
                    }


                    var targetData = _dataList[i];

                    for(int j = 0; j < targetData.TransDataList.Count; j++)
                    {
                        var transData = targetData.TransDataList[j];
                        transData.ViewRect = transData.lineRect;
                        if(targetData.UseAutoColor
                            && AdvencedOptionManager.OverlayAutoBackgroundColor
                            && targetData.TryGetAutoColor(j, out var autoColor))
                        {
                            byte alpha = GetOverlayBackgroundColor(FormManager.Instace.MyMainForm.MySettingManager.BackgroundColor).A;
                            Color backColor = Color.FromArgb(alpha, autoColor.BackGround);
                            backColorBrush = new SolidBrush(backColor);
                        }

                        rectangle.X = x + (int)(transData.lineRect.X / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.X;
                        rectangle.Y = y + (int)(transData.lineRect.Y / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.Y;
                        rectangle.Height = (int)(transData.lineRect.Height / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);
                        rectangle.Width = (int)(transData.lineRect.Width / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);

                        try
                        {
                            Rectangle textRect = Screen.PrimaryScreen.Bounds;

                            if(transData.angleType == OCRDataManager.WordAngleType.Vertical)
                            {
                                sf.FormatFlags |= StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
                                textRect.Height = rectangle.Height;
                            }
                            else
                            {
                                sf.FormatFlags &= ~(StringFormatFlags.DirectionVertical);

                                if(_enableRTL)
                                {
                                    sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                                }
                                else
                                {
                                    sf.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
                                }

                                textRect.Width = rectangle.Width;
                            }

                            if(AdvencedOptionManager.IsAutoFontSize)
                            {
                                float fontSize = OCRDataManager.GetFontSize(transData.lineDataList[0]) / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize / 2;
                                Logger.Logger.AddLog($"Font size : {fontSize}");
                                fontSize *= 1.25f;
                                fontSize = AdvencedOptionManager.GetResultAutoFontSize(fontSize);

                                textFont = new Font(textFont.FontFamily, fontSize);
                            }

                            float emSizeValue = g.DpiY / 72f;
                            float emSize = emSizeValue * textFont.SizeInPoints;

                            CharacterRange[] characterRanges = { new CharacterRange(0, transData.trans.Length) };
                            sf.SetMeasurableCharacterRanges(characterRanges);
                            Region[] stringRegions = g.MeasureCharacterRanges(transData.trans, textFont, textRect, sf);
                            if(stringRegions.Length > 0)
                            {
                                RectangleF measureRect1 = stringRegions[0].GetBounds(g);

                                if(transData.angleType == OCRDataManager.WordAngleType.Vertical)
                                {
                                    if(rectangle.Width < measureRect1.Width)
                                    {
                                        rectangle.Width = (int)measureRect1.Width;
                                    }
                                }
                                else
                                {
                                    if(rectangle.Height < measureRect1.Height)
                                    {
                                        rectangle.Width = rectangle.Width + (int)(rectangle.Width * 0.15f);
                                        stringRegions = g.MeasureCharacterRanges(transData.trans, textFont, rectangle, sf);
                                        if(stringRegions.Length > 0)
                                        {
                                            measureRect1 = stringRegions[0].GetBounds(g);
                                            rectangle.Height = (int)measureRect1.Height;
                                        }
                                    }
                                }
                            }

                            if(transData.lineDataList.Count == 1)
                            {
                                var size = g.MeasureString(transData.trans, textFont, int.MaxValue, sf);
                                if(rectangle.Width < (int)size.Width && transData.angleType == OCRDataManager.WordAngleType.Horizontal)
                                {
                                    rectangle.Width = (int)(size.Width + textFont.Size);
                                }

                                if(rectangle.Height < (int)size.Height && transData.angleType == OCRDataManager.WordAngleType.Vertical)
                                {
                                    rectangle.Height = (int)(size.Height + textFont.Size);
                                }
                            }

                            bool verticalMode = transData.angleType == OCRDataManager.WordAngleType.Vertical;

                            if(_isStart)
                            {
                                if(Form1.IsDebugShowWordArea)
                                {
                                    for(int z = 0; z < transData.lineDataList.Count; z++)
                                    {
                                        Rectangle ocrRect = transData.lineDataList[z].lineRect;
                                        ocrRect.X = x + (int)(ocrRect.X / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.X;
                                        ocrRect.Y = y + (int)(ocrRect.Y / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) - this.Location.Y;
                                        ocrRect.Height = (int)(ocrRect.Height / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) + 5;
                                        ocrRect.Width = (int)(ocrRect.Width / FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize) + 5;

                                        g.FillRectangle(defualtColorBrush, ocrRect.X, ocrRect.Y, ocrRect.Width, ocrRect.Height);
                                    }
                                }
                                else if(FormManager.Instace.MyMainForm.MySettingManager.NowIsUseBackColor)
                                {
                                    rectangle.Height += 10;
                                    rectangle.Width += 10;
                                    RectangleF measureRect1 = rectangle;
                                    g.FillRectangle(backColorBrush, measureRect1.X + 0, measureRect1.Y + 0, measureRect1.Width, measureRect1.Height);
                                }
                            }

                            if(transData.lineDataList.Count <= 1 && transData.TitleData || verticalMode)
                            {
                                // 한 줄이거나 타이틀, 버티컬 모드
                                DrawStringWithOutline2(g, transData.trans, textFont, rectangle, sf, targetData, j, outlineColor1, outlineWidth1, outlineColor2, outlineWidth2);
                            }
                            else
                            {
                                float lineSpacing = 1.2f;
                                List<string> wrappedLines = GetWrappedLinesByAddString(g, transData.trans, textFont, rectangle.Width, rectangle.Height, sf, verticalMode);
                                float fontHeight = textFont.GetHeight(g);

                                for(int lineIdx = 0; lineIdx < wrappedLines.Count; lineIdx++)
                                {
                                    Rectangle lineRect;
                                    if(verticalMode)
                                    {
                                        lineRect = new Rectangle(
                                            rectangle.X + (int)(lineIdx * fontHeight * lineSpacing),
                                            rectangle.Y,
                                            (int)fontHeight,
                                            rectangle.Height
                                        );
                                    }
                                    else
                                    {
                                        lineRect = new Rectangle(
                                            rectangle.X,
                                            rectangle.Y + (int)(lineIdx * fontHeight * lineSpacing),
                                            rectangle.Width,
                                            (int)fontHeight
                                        );
                                    }
                                    DrawStringWithOutline2(g, wrappedLines[lineIdx], textFont, lineRect, sf, targetData, j, outlineColor1, outlineWidth1, outlineColor2, outlineWidth2);
                                }
                            }


                        }
                        catch(Exception ex)
                        {
                            Util.ShowLog(ex.Message);
                        }
                    }
                }
            }
        }

        // 폰트색 기준으로 outline1(조금 밝게), outline2(조금 어둡게) 자동 계산
        private void GetAutoOutlineColors(Color fontColor, out Color outline1, out Color outline2)
        {
            Util.RGB2HSV(fontColor, out double h, out double s, out double v);
            //RgbToHsv(fontColor, out double h, out double s, out double v);

            s /= 255;
            v /= 255;
            // V(명도) 0~1, S(채도) 0~1
            if(v >= 0.5)
            {
                // 밝은 폰트: outline1은 S 10% 감소, V 30% 증가(최대 1), outline2는 V 10%
                double s1 = Math.Max(0, s - 0.05);
                double v1 = Math.Min(1, v - 0.1);
                double v2 = 0.1;
                outline1 = Util.HsvToRgb(h, s1, v1);
                outline2 = Util.HsvToRgb(h, 0, 0);
            }
            else
            {
                // 어두운 폰트: 동일하게 처리(혹은 필요시 다르게 조정)
                double s1 = Math.Max(0, s + 0.05);
                double v1 = Math.Max(0, v + 0.1);
                double v2 = 0.1;
                outline1 = Util.HsvToRgb(h, s1, v1);
                outline2 = Util.HsvToRgb(h, 0, 1);
            }
        }

        // 2중 아웃라인 + 본문 텍스트를 DrawString으로 그리는 함수
        /// <summary>
        /// 저장된 배경색은 유지하고 오버레이에 실제로 적용할 알파만 결정한다.
        /// </summary>
        private static Color GetOverlayBackgroundColor(Color backgroundColor)
        {
            return AdvencedOptionManager.OverlayUseBackgroundAlpha
                ? backgroundColor
                : Color.FromArgb(255, backgroundColor);
        }

        /// <summary>
        /// 실제로 칠하는 색을 정한다. 디버깅 스냅샷도 같은 값을 기록해야 하므로 분리해두었다.
        /// </summary>
        private OverlayDrawColors ResolveDrawColors(OCRDataManager.ResultData targetData, int colorIdx, string text, Color outlineColor1, Color outlineColor2)
        {
            SettingManager setting = FormManager.Instace.MyMainForm.MySettingManager;
            Color fontColor = setting.TextColor;
            Color backgroundColor = GetOverlayBackgroundColor(setting.BackgroundColor);
            bool usesAutomaticColor = targetData.UseAutoColor
                && (AdvencedOptionManager.OverlayAutoFontColor || AdvencedOptionManager.OverlayAutoBackgroundColor);
            (Color Font, Color BackGround) autoColor = default;
            bool hasAutoColor = targetData.UseAutoColor
                && targetData.TryGetAutoColor(colorIdx, out autoColor);

            if(hasAutoColor && AdvencedOptionManager.OverlayAutoFontColor)
            {
                fontColor = autoColor.Font;
            }

            if(hasAutoColor && AdvencedOptionManager.OverlayAutoBackgroundColor)
            {
                backgroundColor = Color.FromArgb(backgroundColor.A, autoColor.BackGround);
            }

            bool corrected = false;
            if(usesAutomaticColor && setting.NowIsUseBackColor && backgroundColor.A > 0)
            {
                Color rawFontColor = fontColor;
                fontColor = OverlayColorAnalyzer.EnsureReadableFontColor(fontColor, backgroundColor, out corrected);
                if(corrected && Form1.IsDebugShowWordArea)
                {
                    Util.ShowLog(
                        $"Overlay color contrast corrected: rawFont={rawFontColor.R},{rawFontColor.G},{rawFontColor.B}, " +
                        $"correctedFont={fontColor.R},{fontColor.G},{fontColor.B}, " +
                        $"background={backgroundColor.R},{backgroundColor.G},{backgroundColor.B}, text={text}");
                }
            }

            if(AdvencedOptionManager.OverlayUseFontOutline
                && (AdvencedOptionManager.OverlayAutoFontColor || corrected))
            {
                GetAutoOutlineColors(fontColor, out outlineColor1, out outlineColor2);
            }

            return new OverlayDrawColors(fontColor, backgroundColor, usesAutomaticColor, corrected, outlineColor1, outlineColor2);
        }

        private void DrawStringWithOutline2(Graphics g, string text, Font font, Rectangle rect, StringFormat sf, OCRDataManager.ResultData targetData, int colorIdx, Color outlineColor1, int outlineWidth1, Color outlineColor2, int outlineWidth2)
        {
            OverlayDrawColors colors = ResolveDrawColors(targetData, colorIdx, text, outlineColor1, outlineColor2);
            Color fontColor = colors.Font;
            outlineColor1 = colors.Outline1;
            outlineColor2 = colors.Outline2;

            // GraphicsPath로 텍스트 경로 생성
            using(GraphicsPath path = new GraphicsPath())
            {
                float emSize = g.DpiY * font.SizeInPoints / 72f;
                path.AddString(
                    text,
                    font.FontFamily,
                    (int)font.Style,
                    emSize,
                    rect,
                    sf
                );

                if(AdvencedOptionManager.OverlayUseFontOutline)
                {
                    // 1. 바깥쪽 아웃라인(더 두껍게)
                    using(Pen outline2 = new Pen(outlineColor2, outlineWidth2) { LineJoin = LineJoin.Round })
                    {
                        g.DrawPath(outline2, path);
                    }
                    // 2. 안쪽 아웃라인(얇게)
                    using(Pen outline1 = new Pen(outlineColor1, outlineWidth1) { LineJoin = LineJoin.Round })
                    {
                        g.DrawPath(outline1, path);
                    }
                }
                // 본문 텍스트
                using(Brush fontBrush = new SolidBrush(fontColor))
                {
                    g.FillPath(fontBrush, path);
                }
            }
        }

        //측정 결과를 재사용하기 위한 키.
        //폰트 크기가 같으면 결과가 완전히 같은 호출이 폰트 이분탐색 때문에 대량으로 중복된다.
        //세로 여부와 StringFormat 플래그가 빠지면 다른 방향의 결과를 잘못 돌려주므로 반드시 포함한다.
        private readonly record struct TextMeasureKey(
            string Text,
            string FontFamily,
            FontStyle FontStyle,
            float EmSize,
            bool Vertical,
            StringFormatFlags FormatFlags,
            StringAlignment Alignment);

        private readonly record struct TextWrapKey(
            string Text,
            string FontFamily,
            FontStyle FontStyle,
            float EmSize,
            int MaxWidth,
            int MaxHeight,
            bool Vertical,
            StringFormatFlags FormatFlags,
            StringAlignment Alignment);

        //페인트 한 번 동안만 살아 있는 캐시. AddText 진입에서 만들고 끝나면 버린다.
        private Dictionary<TextMeasureKey, float> _fitSizeCache;
        private Dictionary<TextWrapKey, List<string>> _wrapCache;

        //디버깅 계측용. 디버깅 저장이 꺼져 있으면 그냥 증가만 하고 쓰이지 않는다.
        private int _measureCacheHit;
        private int _measureCacheMiss;

        /// <summary>
        /// 줄바꿈 판단에 쓰는 길이. 세로는 높이, 가로는 너비를 본다.
        /// </summary>
        private float GetFitSize(Graphics g, string text, Font font, float emSize, StringFormat sf, bool isVertical)
        {
            TextMeasureKey key = default;
            bool useCache = _fitSizeCache != null;
            if(useCache)
            {
                key = new TextMeasureKey(text, font.FontFamily.Name, font.Style, emSize, isVertical, sf.FormatFlags, sf.Alignment);
                if(_fitSizeCache.TryGetValue(key, out float cached))
                {
                    _measureCacheHit++;
                    return cached;
                }

                _measureCacheMiss++;
            }

            float result;
            using(GraphicsPath path = new GraphicsPath())
            {
                path.AddString(text, font.FontFamily, (int)font.Style, emSize, new Point(0, 0), sf);
                RectangleF bounds = path.GetBounds();
                if(isVertical)
                {
                    // 세로 모드는 높이로 판단
                    result = bounds.Height;
                }
                else
                {
                    // 가로 모드: MeasureString과 GraphicsPath 중 더 큰 값 사용
                    SizeF ms = g.MeasureString(text, font);
                    result = Math.Max(bounds.Width, ms.Width);
                }
            }

            if(useCache)
            {
                _fitSizeCache[key] = result;
            }

            return result;
        }

        private List<string> GetWrappedLinesByAddString(Graphics g, string text, Font font, int maxWidth, int maxHeight, StringFormat sf, bool isVertical)
        {
            List<string> lines = new List<string>();
            if(string.IsNullOrEmpty(text))
                return lines;

            float emSize = g.DpiY * font.SizeInPoints / 72f;

            TextWrapKey wrapKey = default;
            bool useWrapCache = _wrapCache != null;
            if(useWrapCache)
            {
                wrapKey = new TextWrapKey(text, font.FontFamily.Name, font.Style, emSize,
                    maxWidth, maxHeight, isVertical, sf.FormatFlags, sf.Alignment);
                if(_wrapCache.TryGetValue(wrapKey, out List<string> cachedLines))
                {
                    _measureCacheHit++;
                    return cachedLines;
                }

                _measureCacheMiss++;
            }

            float fudge = font.Size * 1.2f; // 픽셀 여유

            string[] originalLines = text.Replace("\r\n", "\n").Split('\n');
            foreach(var originalLine in originalLines)
            {
                string remaining = originalLine;
                while(!string.IsNullOrEmpty(remaining))
                {
                    // 글자를 더할수록 길이는 줄어들 수 없으므로 들어가는 길이는 항상 앞쪽 구간이다.
                    // 1부터 하나씩 재면 한 줄마다 글자 수만큼 측정하게 되고, 이 함수가
                    // 폰트 이분탐색 안에서 수십 번 반복되어 페인트 한 번이 통째로 느려진다.
                    // 결과는 같고 측정 횟수만 줄이도록 이분 탐색으로 찾는다.
                    int low = 0;
                    int high = remaining.Length;
                    while(low < high)
                    {
                        int middle = (low + high + 1) / 2;
                        if(GetFitSize(g, remaining.Substring(0, middle), font, emSize, sf, isVertical)
                            > (isVertical ? maxHeight - fudge : maxWidth - fudge))
                        {
                            high = middle - 1;
                        }
                        else
                        {
                            low = middle;
                        }
                    }

                    int lastFit = low;
                    if(lastFit == 0) lastFit = 1;
                    lines.Add(remaining.Substring(0, lastFit));
                    if(lastFit >= remaining.Length)
                        break;
                    remaining = remaining.Substring(lastFit).TrimStart();
                }
            }

            if(useWrapCache)
            {
                _wrapCache[wrapKey] = lines;
            }

            return lines;
        }

        private bool isLockPaint = false;
        private bool _forceLock;

        //페인트 구간별 시간(ms). 디버깅 저장이 켜진 동안에만 채워진다.
        private double _checkSizeMs;
        private double _layoutAndDrawMs;
        private double _presentMs;

        public void UpdatePaint()
        {
            if(_forceLock)
            {
                return;
            }

            if(IsDisposed || isDestroyFormFlag)
            {
                return;
            }

            //핸들이 없으면 InvokeRequired 가 false 라 번역 스레드가 그대로 그리게 되고,
            //그리는 중 this.Handle 을 읽으면 핸들이 그 스레드에 생긴다.
            //그러면 오버레이 메시지 펌프가 메시지를 돌리지 않는 스레드에 묶여
            //이후 이 폼에 대한 Invoke 가 영영 풀리지 않는다.
            if(!IsHandleCreated && FormManager.Instace.MyMainForm is Form mainForm && mainForm.InvokeRequired)
            {
                return;
            }

            try
            {
                if(this.InvokeRequired)
                {
                    Action action = () => DoUpdatePaint();
                    this.BeginInvoke(action);
                }
                else
                {
                    DoUpdatePaint();
                }
            }
            catch(Exception ex)
            {
                //정리 중인 폼에 BeginInvoke 하면 예외가 난다.
                //번역 스레드까지 올라가면 스레드가 그대로 끝나버려 Join 이 풀리지 않으므로 여기서 막는다.
                Util.ShowLog($"TransFormOver.UpdatePaint failed - {ex.Message}");
            }
        }

        private void DoUpdatePaint()
        {
            if(isLockPaint)
            {
                Util.ShowLog("Lock Paint!!!!");
                return;
            }

            isLockPaint = true;

            //측정 결과는 이 페인트 안에서만 재사용한다.
            //폰트 이분탐색이 같은 문자열을 같은 크기로 반복해서 재기 때문에 중복이 대부분이다.
            _fitSizeCache = new Dictionary<TextMeasureKey, float>();
            _wrapCache = new Dictionary<TextWrapKey, List<string>>();
            _measureCacheHit = 0;
            _measureCacheMiss = 0;

            //계측은 디버깅 저장이 켜진 동안에만 한다
            bool measureTiming = Form1.IsDebugSaveAnalysisResult;
            var paintWatch = measureTiming ? System.Diagnostics.Stopwatch.StartNew() : null;
            _checkSizeMs = 0;
            _layoutAndDrawMs = 0;
            _presentMs = 0;

            // Get device contexts
            IntPtr screenDc = IntPtr.Zero;
            IntPtr memDc = IntPtr.Zero;
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr hOldBitmap = IntPtr.Zero;
            try
            {
                var sectionWatch = measureTiming ? System.Diagnostics.Stopwatch.StartNew() : null;
                CheckSizeAndLocation();
                if(measureTiming)
                {
                    _checkSizeMs = sectionWatch.Elapsed.TotalMilliseconds;
                }

                Util.ShowLog("Update paint + " + makeIndex);

                screenDc = GetDC(IntPtr.Zero);
                memDc = CreateCompatibleDC(screenDc);


                if(bitmap == null || bitmap.Width != this.Width || bitmap.Height != Height)
                {
                    //Bitmap 은 네이티브 GDI+ 자원을 들고 있어서 놓아주지 않으면 파이널라이저까지 남는다.
                    //OCR 영역을 끌면 창 크기가 계속 바뀌어 큰 비트맵이 매번 새로 생기므로,
                    //여기서 안 버리면 GDI+ 자원이 말라 그리기가 실패하고 창이 검게 남는다.
                    bitmap?.Dispose();
                    bitmap = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                }

                using(Graphics gF = Graphics.FromImage(bitmap))
                using(SolidBrush brush = new SolidBrush(Color.FromArgb(0, 240, 248, 255)))
                {
                    gF.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
                }

                Font textFont = FormManager.Instace.MyMainForm.MySettingManager.TextFont;

                // Set parameters for layered window update.
                FrameSize newSize = new FrameSize(bitmap.Width, bitmap.Height);
                FramePoint sourceLocation = new FramePoint(0, 0);
                FramePoint newLocation = new FramePoint(this.Left, this.Top);
                BLENDFUNCTION blend = new BLENDFUNCTION();
                blend.BlendOp = AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = 255;
                blend.AlphaFormat = AC_SRC_ALPHA;

                //페인트마다 만들어지므로 놓아주지 않으면 GDI+ 자원이 계속 쌓인다
                using Graphics g = Graphics.FromImage(bitmap);
                Color OutlineForeColor = FormManager.Instace.MyMainForm.MySettingManager.OutLineColor1;
                float OutlineWidth = 2;
                using(GraphicsPath gp = new GraphicsPath())
                using(Pen outline = new Pen(OutlineForeColor, OutlineWidth) { LineJoin = LineJoin.Round })
                using(StringFormat sf = new StringFormat())
                {
                    using Brush foreBrush = new SolidBrush(FormManager.Instace.MyMainForm.MySettingManager.TextColor);
                    sf.Alignment = stringFormat.Alignment;
                    sf.FormatFlags = stringFormat.FormatFlags;
                    Color backgroundColor = Color.FromArgb(alpha, Color.Red);
                    g.Clear(backgroundColor);

                    Rectangle rectangle = ClientRectangle;
                    rectangle.X = this.Location.X;
                    rectangle.Y = this.Location.Y;

                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;


                    var layoutWatch = measureTiming ? System.Diagnostics.Stopwatch.StartNew() : null;
                    AddText(gp, g, textFont, rectangle, sf);
                    if(measureTiming)
                    {
                        _layoutAndDrawMs = layoutWatch.Elapsed.TotalMilliseconds;
                    }

                    if(!_isStart)
                    {
                        using(Pen layerOutline = new Pen(Color.FromArgb(40, 134, 249), 3) { LineJoin = LineJoin.Round })
                            g.DrawRectangle(layerOutline, ClientRectangle);

                    }
                    /*
                    if(isActiveGDI)
                    {
                        using(Pen outline2 = new Pen(FormManager.Instace.MyMainForm.MySettingManager.OutLineColor2, 5) { LineJoin = LineJoin.Round })
                            g.DrawPath(outline2, gp);
                        g.DrawPath(outline, gp);
                        g.FillPath(foreBrush, gp);
                    }
                    else
                    {
                        g.DrawString(resultText, textFont, foreBrush, rectangle);
                    }
                    */

                }

                if(!_isStart)
                {
                    g.Clear(Color.FromArgb(0));
                }

                var presentWatch = measureTiming ? System.Diagnostics.Stopwatch.StartNew() : null;

                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));  //Set the fact that background is transparent
                hOldBitmap = SelectObject(memDc, hBitmap);


                // Update the window.

                if(this == null || this.IsDisposed || this.isDestroyFormFlag)
                {
                    return;
                }

                UpdateLayeredWindow(
                    this.Handle,     // Handle to the layered window
                    screenDc,        // Handle to the screen DC
                    ref newLocation, // New screen position of the layered window
                    ref newSize,     // New size of the layered window
                    memDc,           // Handle to the layered window surface DC
                    ref sourceLocation, // Location of the layer in the DC
                    0,               // Color key of the layered window
                    ref blend,       // Transparency of the layered window
                    ULW_ALPHA        // Use blend as the blend function
                    );
                //SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);

                if(measureTiming)
                {
                    _presentMs = presentWatch.Elapsed.TotalMilliseconds;
                }
            }
            finally
            {
                // Release device context.
                ReleaseDC(IntPtr.Zero, screenDc);
                if(hBitmap != IntPtr.Zero)
                {
                    SelectObject(memDc, hOldBitmap);
                    DeleteObject(hBitmap);
                }
                DeleteDC(memDc);

                //여기서 GC.Collect() 를 돌리지 않는다.
                //페인트마다 전체 블로킹 GC 가 UI 스레드에서 돌아 번역 중 끊김의 원인이 된다.
                //이 함수가 만드는 Bitmap / Graphics / 브러시는 위에서 직접 놓아주고
                //GDI 핸들은 DeleteObject / DeleteDC 로 정리하므로 강제 수집이 필요 없다.

                _fitSizeCache = null;
                _wrapCache = null;

                //화면 반영까지 끝난 뒤에 디버깅 스냅샷을 넘긴다.
                //중간 return 으로 빠져나온 경우에도 모아둔 블록은 버리고 대기 상태를 풀어야 한다.
                if(_pendingDebugBlocks != null)
                {
                    var timing = new OcrDebugPaintTiming
                    {
                        TotalMs = paintWatch?.Elapsed.TotalMilliseconds ?? 0,
                        CheckSizeMs = _checkSizeMs,
                        LayoutAndDrawMs = _layoutAndDrawMs,
                        PresentMs = _presentMs,
                        MeasureCacheHit = _measureCacheHit,
                        MeasureCacheMiss = _measureCacheMiss,
                    };

                    DebugSnapshotService?.CompleteOverlay(
                        Bounds,
                        FormManager.Instace.MyMainForm.MySettingManager.NowIsUseBackColor,
                        _pendingDebugBlocks,
                        timing);

                    _pendingDebugBlocks = null;
                }

                //중간 return 이나 예외로 빠져나가도 잠금은 반드시 푼다.
                //풀리지 않으면 이후 모든 갱신이 맨 위에서 막혀 오버레이가 멈춘 것처럼 보인다.
                isLockPaint = false;
            }
        }


        enum dragMode { none, left, right, up, down, leftUp, rightUp, leftDown, rightDown };
        dragMode nowDragMode = dragMode.none;

        public void ApplyUseTopMostOptionWhenTranslate(bool useTopMostOptionWhenTranslate)
        {
            UseTopMostOptionWhenTranslate = useTopMostOptionWhenTranslate;
            //CheckTopMostOption();
        }

        public void ApplyRTL(bool enableRTL)
        {
            _enableRTL = enableRTL;
            if(enableRTL)
            {
                stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }
            else
            {
                stringFormat.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
            }
        }

        public void SetTopMost(bool topMost, bool useTopMostOptionWhenTranslate)
        {
            //무조건 탑 모스트다
            isTopMostFlag = true;
            this.TopMost = isTopMostFlag;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        public void Prepare()
        {
            if(InvokeRequired)
            {
                Invoke((Action)Prepare);
                return;
            }

            _dataList = new List<OCRDataManager.ResultData>();
            _overlayDataCache.Clear();
            resultText = "";
            _lastDisplayRect = Rectangle.Empty;
            isLockPaint = false;
            _forceLock = false;
            _isStart = true;
            alpha = 0;
            UpdatePaint();
            OverlayRenderSynchronizationService.Flush();
        }

        public void StartTrans()
        {
            TaskIndex++;
            if(TaskIndex > 100000)
            {
                TaskIndex = 0;
            }

            TranslateStatusType = TranslateStatusType.Translate;
        }

        public void StopTrans()
        {
            TranslateStatusType = TranslateStatusType.Stop;
        }

        public void ApplyTopMost()
        {
            this.TopMost = true;
        }

        public void destroyForm()
        {
            isDestroyFormFlag = true;
            FormManager.Instace.MyOverTransForm = null;
            this.Close();
        }


        #region:::::::::::::::::::::::::::::::::::::::::::레이어 창 이동 관련:::::::::::::::::::::::::::::::::::::::::::


        private void TransForm_MouseDown(object sender, MouseEventArgs e)
        {
            if((e.X <= 30 && e.X >= 1) && (e.Y <= 30 && e.Y >= 1))
            {
                nowDragMode = dragMode.leftUp;
            }
            else if((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 1) && (e.Y <= 30 && e.Y >= 1))
            {
                nowDragMode = dragMode.rightUp;
            }
            else if((e.X <= 30 && e.X >= 1) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 1))
            {
                nowDragMode = dragMode.leftDown;
            }
            else if((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 1) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 1))
            {
                nowDragMode = dragMode.rightDown;
            }
            else if((e.X <= 30 && e.X >= 1))
            {
                nowDragMode = dragMode.left;

            }
            else if(this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 1)
            {
                nowDragMode = dragMode.right;
            }
            else if((e.Y <= 30 && e.Y >= 1))
            {
                nowDragMode = dragMode.up;
            }
            else if(this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 1)
            {
                nowDragMode = dragMode.down;
            }
            else
            {
                nowDragMode = dragMode.none;
            }

            mousePoint = new Point(e.X, e.Y);
        }

        private void TransForm_MouseMove(object sender, MouseEventArgs e)
        {

            if((e.Button & MouseButtons.Right) == MouseButtons.Right || (e.Button & MouseButtons.Left) != MouseButtons.Left)
            {
                nowDragMode = dragMode.none;
            }
            if(nowDragMode == dragMode.none)
            {
                if((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
                }
            }
            else
            {
                if(nowDragMode == dragMode.leftUp)
                {
                    int backupTop = this.Top;
                    int backupLeft = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
                    this.Size = new Size(this.Size.Width + backupLeft - this.Left, this.Size.Height + backupTop - this.Top);
                }
                else if(nowDragMode == dragMode.leftDown)
                {
                    int backupLeft = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top);
                    this.Size = new Size(this.Size.Width + backupLeft - this.Left, this.Size.Height - (this.Size.Height - e.Y));
                }
                else if(nowDragMode == dragMode.rightUp)
                {
                    int backupTop = this.Top;

                    Location = new Point(this.Left,
                    this.Top - (mousePoint.Y - e.Y));
                    this.Size = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height + backupTop - this.Top);
                }
                else if(nowDragMode == dragMode.rightDown)
                {
                    this.Size = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height - (this.Size.Height - e.Y));
                }
                else if(nowDragMode == dragMode.up)
                {
                    int backup = this.Top;

                    Location = new Point(this.Left,
                    this.Top - (mousePoint.Y - e.Y));
                    this.Size = new Size(this.Size.Width, this.Size.Height + backup - this.Top);
                }
                else if(nowDragMode == dragMode.down)
                {
                    this.Size = new Size(this.Size.Width, this.Size.Height - (this.Size.Height - e.Y));
                }
                else if(nowDragMode == dragMode.left)
                {
                    int backup = this.Left;

                    Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top);
                    this.Size = new Size(this.Size.Width + backup - this.Left, this.Size.Height);
                }
                else if(nowDragMode == dragMode.right)
                {

                    this.Size = new Size(this.Size.Width - (this.Size.Width - e.X), this.Size.Height);
                }
            }

            if((e.X <= 30 && e.X >= 0) && (e.Y <= 30 && e.Y >= 0))
            {

                Cursor = Cursors.SizeNWSE;
            }
            else if((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 0) && (e.Y <= 30 && e.Y >= 0))
            {
                Cursor = Cursors.SizeNESW;
            }
            else if((e.X <= 30 && e.X >= 0) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 0))
            {
                Cursor = Cursors.SizeNESW;
            }
            else if((this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 0) && (this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 0))
            {
                Cursor = Cursors.SizeNWSE;
            }
            else if((e.X <= 30 && e.X >= 0))
            {
                Cursor = Cursors.SizeWE;

            }
            else if(this.Size.Width - e.X <= 30 && this.Size.Width - e.X >= 0)
            {
                Cursor = Cursors.SizeWE;
            }
            else if((e.Y <= 30 && e.Y >= 0))
            {
                Cursor = Cursors.SizeNS;
            }
            else if(this.Size.Height - e.Y <= 30 && this.Size.Height - e.Y >= 0)
            {
                Cursor = Cursors.SizeNS;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }
        private void TransForm_MouseUp(object sender, MouseEventArgs e)
        {
            nowDragMode = dragMode.none;
        }
        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::레이어 색및 클릭 관련:::::::::::::::::::::::::::::::::::::::::::
        public void setInvisibleBackground()
        {
            isLockPaint = false;
            _isStart = true;
            _forceLock = false;
            alpha = 0;     //0이어야 함
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void setVisibleBackground()
        {
            isLockPaint = false;
            _isStart = false;
            _forceLock = false;
            alpha = 190;
            _lastDisplayRect = Rectangle.Empty;
            this.BeginInvoke(new Action(UpdatePaint));
        }

        public void setOverHitLayer()
        {
            int extendedStyle;
            extendedStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
        public void disableOverHitLayer()
        {
            int extendedStyle;
            extendedStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT);
        }

        public async void VisibleOverlayTransAsync(int waitTime)
        {
            int taskIndex = TaskIndex;
            await Task.Delay(waitTime * 1000);

            if(!this.IsDisposed && taskIndex == TaskIndex)
            {
                setVisibleBackground();
                disableOverHitLayer();
            }

        }

        #endregion

        #region ::::::::: 인터페이스 관련 :::::::::::
        public void ForceTransparency()
        {

        }


        public void DoUpdate(bool isTranslating)
        {

        }

        public SettingManager.Skin GetSkinType()
        {
            return SettingManager.Skin.over;
        }

        public void ForceUpdateText(string text)
        {

        }

        public void ApplyWarningMessage(string message, DateTime dtRemainTime)
        {
            //따로 처리 안 한다
        }


        public void ClearWarningMessage()
        {
            //따로 처리 안 한다
        }

        #endregion


    }
}
