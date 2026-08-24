
namespace MORT
{
    partial class Form1
    {


        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            ContextOption = new System.Windows.Forms.ContextMenuStrip(components);
            optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showTransToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            rTTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            setTranslateTopMostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setCheckSpellingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setCutPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            transToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            settingSaveToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            settingLoadToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            settingDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            checkUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            optionMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            설정저장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            설정불러오기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fontDialog = new System.Windows.Forms.FontDialog();
            colorDialog1 = new System.Windows.Forms.ColorDialog();
            acceptButton = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            donationButton = new System.Windows.Forms.Button();
            toolTip_OCR = new System.Windows.Forms.ToolTip(components);
            tbMain = new Dotnetrix_Samples.TabControl();
            tpBasic = new System.Windows.Forms.TabPage();
            panel8 = new System.Windows.Forms.Panel();
            pnAdjustImg = new System.Windows.Forms.Panel();
            btImgResult = new System.Windows.Forms.Button();
            cbThreshold = new System.Windows.Forms.CheckBox();
            tbThreshold = new System.Windows.Forms.TextBox();
            groupCombo = new System.Windows.Forms.ComboBox();
            groupLabel = new System.Windows.Forms.Label();
            lbImgGroupCount = new System.Windows.Forms.Label();
            lbImgGroup = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            v2TextBox = new System.Windows.Forms.TextBox();
            lbAdjustImg = new System.Windows.Forms.Label();
            checkRGB = new System.Windows.Forms.CheckBox();
            s2TextBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            label16 = new System.Windows.Forms.Label();
            rTextBox = new System.Windows.Forms.TextBox();
            checkErode = new System.Windows.Forms.CheckBox();
            label4 = new System.Windows.Forms.Label();
            checkHSV = new System.Windows.Forms.CheckBox();
            gTextBox = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            v1TextBox = new System.Windows.Forms.TextBox();
            bTextBox = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            s1TextBox = new System.Windows.Forms.TextBox();
            pnOCR = new System.Windows.Forms.Panel();
            btOcrHelp = new System.Windows.Forms.Button();
            label48 = new System.Windows.Forms.Label();
            OCR_Type_comboBox = new System.Windows.Forms.ComboBox();
            isClipBoardcheckBox1 = new System.Windows.Forms.CheckBox();
            saveOCRCheckBox = new System.Windows.Forms.CheckBox();
            ocrLabel = new System.Windows.Forms.Label();
            showOcrCheckBox = new System.Windows.Forms.CheckBox();
            pnNHocr = new System.Windows.Forms.Panel();
            cbOneOcrLanguage = new System.Windows.Forms.ComboBox();
            lbOneOcrLanguage = new System.Windows.Forms.Label();
            WinOCR_panel = new System.Windows.Forms.Panel();
            btnAddWinOcrLanguage = new System.Windows.Forms.Button();
            WinOCR_Language_comboBox = new System.Windows.Forms.ComboBox();
            lbWinOCRLanguage = new System.Windows.Forms.Label();
            pnEasyOcr = new System.Windows.Forms.Panel();
            btnInstallEasyOcr = new System.Windows.Forms.Button();
            cbEasyOcrCode = new System.Windows.Forms.ComboBox();
            lbEasyOcrLanguage = new System.Windows.Forms.Label();
            Tesseract_panel = new System.Windows.Forms.Panel();
            cbFastTess = new System.Windows.Forms.CheckBox();
            tesseractLanguageComboBox = new System.Windows.Forms.ComboBox();
            lbTesseractLanguage = new System.Windows.Forms.Label();
            label18 = new System.Windows.Forms.Label();
            tessDataTextBox = new System.Windows.Forms.TextBox();
            pnGoogleOcr = new System.Windows.Forms.Panel();
            cbGoogleOcrLanguge = new System.Windows.Forms.ComboBox();
            lbGoogleOCRLanguage = new System.Windows.Forms.Label();
            lbGoogleOcrStatus = new System.Windows.Forms.Label();
            btnSettingGoogleOCR = new System.Windows.Forms.Button();
            pnTranslate = new System.Windows.Forms.Panel();
            pnGemini = new System.Windows.Forms.Panel();
            cbGeminiModel = new System.Windows.Forms.ComboBox();
            lbGeminiModel = new System.Windows.Forms.Label();
            tbGeminiApi = new System.Windows.Forms.TextBox();
            lbGeminiApi = new System.Windows.Forms.Label();
            btnTransHelp = new System.Windows.Forms.Button();
            cbPerWordDic = new System.Windows.Forms.CheckBox();
            lbTransType = new System.Windows.Forms.Label();
            _cbTranslateType = new System.Windows.Forms.ComboBox();
            checkDic = new System.Windows.Forms.CheckBox();
            dicFileTextBox = new System.Windows.Forms.TextBox();
            lbDicFile = new System.Windows.Forms.Label();
            lbTransTypeTitle = new System.Windows.Forms.Label();
            Naver_Panel = new System.Windows.Forms.Panel();
            Button_NaverTransKeyList = new System.Windows.Forms.Button();
            lbPapagoSecret = new System.Windows.Forms.Label();
            NaverSecretKeyTextBox = new System.Windows.Forms.TextBox();
            NaverIDKeyTextBox = new System.Windows.Forms.TextBox();
            lbPapagoID = new System.Windows.Forms.Label();
            Google_Panel = new System.Windows.Forms.Panel();
            button_RemoveAllGoogleToekn = new System.Windows.Forms.Button();
            textBox_GoogleSecretKey = new System.Windows.Forms.TextBox();
            lbSheetSecret = new System.Windows.Forms.Label();
            textBox_GoogleClientID = new System.Windows.Forms.TextBox();
            lbSheetID = new System.Windows.Forms.Label();
            googleSheet_textBox = new System.Windows.Forms.TextBox();
            lbGoogleSheetAddress = new System.Windows.Forms.Label();
            pnEzTrans = new System.Windows.Forms.Panel();
            lbEzTransInfo = new System.Windows.Forms.Label();
            pnPapagoWeb = new System.Windows.Forms.Panel();
            lbPapagoWebInfo = new System.Windows.Forms.Label();
            pnGoogleBasic = new System.Windows.Forms.Panel();
            lbBasicInfo = new System.Windows.Forms.Label();
            lbBasicStatus = new System.Windows.Forms.Label();
            pnCustomApi = new System.Windows.Forms.Panel();
            lbCustomApiInformation = new System.Windows.Forms.Label();
            pnDeepLAPI = new System.Windows.Forms.Panel();
            tbDeeplApi = new System.Windows.Forms.TextBox();
            lbDeeplApi = new System.Windows.Forms.Label();
            rbDeepLAPIEndpointFree = new System.Windows.Forms.RadioButton();
            rbDeepLAPIEndpointPaid = new System.Windows.Forms.RadioButton();
            pnDeepl = new System.Windows.Forms.Panel();
            btnCheckDeeplState = new System.Windows.Forms.Button();
            lbDeepLStatus = new System.Windows.Forms.Label();
            lbDeepLInfo = new System.Windows.Forms.Label();
            DB_Panel = new System.Windows.Forms.Panel();
            cbDBMultiGet = new System.Windows.Forms.CheckBox();
            checkStringUpper = new System.Windows.Forms.CheckBox();
            dbFileTextBox = new System.Windows.Forms.TextBox();
            lbDbFile = new System.Windows.Forms.Label();
            tpText = new System.Windows.Forms.TabPage();
            panel5 = new System.Windows.Forms.Panel();
            panel17 = new System.Windows.Forms.Panel();
            lbPreview = new System.Windows.Forms.Label();
            fontResultLabel = new CustomLabel();
            panel10 = new System.Windows.Forms.Panel();
            defaultColorButton = new System.Windows.Forms.Button();
            lbFontBackground = new System.Windows.Forms.Label();
            lbFontOutlineColor2 = new System.Windows.Forms.Label();
            lbFontOutlineColor1 = new System.Windows.Forms.Label();
            backgroundColorBox = new System.Windows.Forms.PictureBox();
            outlineColor2Box = new System.Windows.Forms.PictureBox();
            outlineColor1Box = new System.Windows.Forms.PictureBox();
            lbFontBasicColor = new System.Windows.Forms.Label();
            textColorBox = new System.Windows.Forms.PictureBox();
            lbFontColor = new System.Windows.Forms.Label();
            panel9 = new System.Windows.Forms.Panel();
            cbShowOCRIndex = new System.Windows.Forms.CheckBox();
            useBackColorCheckBox = new System.Windows.Forms.CheckBox();
            removeSpaceCheckBox = new System.Windows.Forms.CheckBox();
            alignmentCenterCheckBox = new System.Windows.Forms.CheckBox();
            lbTextAdditionalSettings = new System.Windows.Forms.Label();
            panel7 = new System.Windows.Forms.Panel();
            fontSizeUpDown = new System.Windows.Forms.NumericUpDown();
            lbFontSize = new System.Windows.Forms.Label();
            lbFont = new System.Windows.Forms.Label();
            fontButton = new System.Windows.Forms.Button();
            lbFontSetting = new System.Windows.Forms.Label();
            tpExtra = new System.Windows.Forms.TabPage();
            panel11 = new System.Windows.Forms.Panel();
            panel21 = new System.Windows.Forms.Panel();
            btAdvencedOption = new System.Windows.Forms.Button();
            lbAdvencedConfig = new System.Windows.Forms.Label();
            panel25 = new System.Windows.Forms.Panel();
            btSettingUpload = new System.Windows.Forms.Button();
            btSettingBrowser = new System.Windows.Forms.Button();
            lbSearchConfig = new System.Windows.Forms.Label();
            panel3 = new System.Windows.Forms.Panel();
            speedRadioButton5 = new System.Windows.Forms.RadioButton();
            lbSpeed = new System.Windows.Forms.Label();
            lbSpeedInformation = new System.Windows.Forms.Label();
            speedRadioButton4 = new System.Windows.Forms.RadioButton();
            speedRadioButton1 = new System.Windows.Forms.RadioButton();
            speedRadioButton3 = new System.Windows.Forms.RadioButton();
            speedRadioButton2 = new System.Windows.Forms.RadioButton();
            panel13 = new System.Windows.Forms.Panel();
            defaultButton = new System.Windows.Forms.Button();
            saveConfigButton = new System.Windows.Forms.Button();
            openConfigButton = new System.Windows.Forms.Button();
            lbSettingFile = new System.Windows.Forms.Label();
            panel12 = new System.Windows.Forms.Panel();
            topMostcheckBox = new System.Windows.Forms.CheckBox();
            checkUpdateCheckBox = new System.Windows.Forms.CheckBox();
            label15 = new System.Windows.Forms.Label();
            panel14 = new System.Windows.Forms.Panel();
            btAttachCapture = new System.Windows.Forms.Button();
            SetDefaultZoomSizeButton = new System.Windows.Forms.Button();
            imgZoomsizeUpDown = new System.Windows.Forms.NumericUpDown();
            lbImgZoom = new System.Windows.Forms.Label();
            activeWinodeCheckBox = new System.Windows.Forms.CheckBox();
            lbImgCapture = new System.Windows.Forms.Label();
            tpTranslation = new System.Windows.Forms.TabPage();
            panel19 = new System.Windows.Forms.Panel();
            panel4 = new System.Windows.Forms.Panel();
            cbDeepLLanguageTo = new System.Windows.Forms.ComboBox();
            lbDeepLTo = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            cbDeepLLanguage = new System.Windows.Forms.ComboBox();
            lbDeepL = new System.Windows.Forms.Label();
            lbDeepLFrom = new System.Windows.Forms.Label();
            panel27 = new System.Windows.Forms.Panel();
            cbTTSWaitEnd = new System.Windows.Forms.CheckBox();
            cbUseTTS = new System.Windows.Forms.CheckBox();
            label66 = new System.Windows.Forms.Label();
            panel22 = new System.Windows.Forms.Panel();
            googleResultCodeComboBox = new System.Windows.Forms.ComboBox();
            lbGoogleTo = new System.Windows.Forms.Label();
            label56 = new System.Windows.Forms.Label();
            googleTransComboBox = new System.Windows.Forms.ComboBox();
            lbGoogle = new System.Windows.Forms.Label();
            lbGoogleFrom = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            skinOverRadioButton = new System.Windows.Forms.RadioButton();
            skinLayerRadioButton = new System.Windows.Forms.RadioButton();
            lbTransformType = new System.Windows.Forms.Label();
            skinDarkRadioButton = new System.Windows.Forms.RadioButton();
            panel15 = new System.Windows.Forms.Panel();
            lbPapagoLanguageCodeInformation = new System.Windows.Forms.Label();
            cbNaverResultCode = new System.Windows.Forms.ComboBox();
            lbPaPagoTo = new System.Windows.Forms.Label();
            label43 = new System.Windows.Forms.Label();
            naverTransComboBox = new System.Windows.Forms.ComboBox();
            lbPaPago = new System.Windows.Forms.Label();
            lbPaPagoFrom = new System.Windows.Forms.Label();
            tpETC = new System.Windows.Forms.TabPage();
            panel18 = new System.Windows.Forms.Panel();
            panel16 = new System.Windows.Forms.Panel();
            btnOpenDiscord = new System.Windows.Forms.Button();
            openBlogButton = new System.Windows.Forms.Button();
            btnGitHub = new System.Windows.Forms.Button();
            lbLink = new System.Windows.Forms.Label();
            panel20 = new System.Windows.Forms.Panel();
            about_Button = new System.Windows.Forms.Button();
            error_Information_Button = new System.Windows.Forms.Button();
            help_Button = new System.Windows.Forms.Button();
            lbETC = new System.Windows.Forms.Label();
            panel23 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            transKeyInputLabel = new KeyInputLabel();
            btnHideTransEmpty = new System.Windows.Forms.Button();
            dicKeyInputLabel = new KeyInputLabel();
            btnHideTransDefault = new System.Windows.Forms.Button();
            quickKeyInputLabel = new KeyInputLabel();
            lbHideTranslate = new KeyInputLabel();
            lbFollowMouse = new KeyInputLabel();
            btnFollowMouseEmpty = new System.Windows.Forms.Button();
            btnFollowMouseDefault = new System.Windows.Forms.Button();
            transKeyInputResetButton = new System.Windows.Forms.Button();
            dicKeyInputResetButton = new System.Windows.Forms.Button();
            btnOneTransEmpty = new System.Windows.Forms.Button();
            quickKeyInputResetButton = new System.Windows.Forms.Button();
            btnOneTransDefault = new System.Windows.Forms.Button();
            transKeyInputEmptyButton = new System.Windows.Forms.Button();
            lbOneTrans = new KeyInputLabel();
            dicKeyInputEmptyButton = new System.Windows.Forms.Button();
            quickKeyInputEmptyButton = new System.Windows.Forms.Button();
            snapShotKeyInputEmptyButton = new System.Windows.Forms.Button();
            snapShotInputLabel = new KeyInputLabel();
            snapShotKeyInputResetButton = new System.Windows.Forms.Button();
            lbHotKeyFollowMouse = new System.Windows.Forms.Label();
            lbHotKeyHideTransWindow = new System.Windows.Forms.Label();
            lbHotKeyOnceTranslate = new System.Windows.Forms.Label();
            lbHotKeySnapShot = new System.Windows.Forms.Label();
            lbHotKeyInformation = new System.Windows.Forms.Label();
            lbHotKeyQuickOCR = new System.Windows.Forms.Label();
            lbHotKeyDic = new System.Windows.Forms.Label();
            lbHotKeyDoTrans = new System.Windows.Forms.Label();
            lbHotkey = new System.Windows.Forms.Label();
            tpQuickSetting = new System.Windows.Forms.TabPage();
            panel28 = new System.Windows.Forms.Panel();
            panel31 = new System.Windows.Forms.Panel();
            cbSetBasicDefaultPage = new System.Windows.Forms.CheckBox();
            btQuickJap = new System.Windows.Forms.Button();
            lbQuickSettingInformation = new System.Windows.Forms.Label();
            btQucickEnglish = new System.Windows.Forms.Button();
            lbQuickSetting = new System.Windows.Forms.Label();
            tpDebuging = new System.Windows.Forms.TabPage();
            panel24 = new System.Windows.Forms.Panel();
            panel26 = new System.Windows.Forms.Panel();
            plDebugOn = new System.Windows.Forms.Panel();
            cbDisplayLowQuality = new System.Windows.Forms.CheckBox();
            cbSaveAnalysisResult = new System.Windows.Forms.CheckBox();
            cbShowOverlayWordArea = new System.Windows.Forms.CheckBox();
            cbSetLineTrans = new System.Windows.Forms.CheckBox();
            btClearFormerResult = new System.Windows.Forms.Button();
            cbShowFormerLog = new System.Windows.Forms.CheckBox();
            cbUnlockSpeed = new System.Windows.Forms.CheckBox();
            cbSaveCaptureResult = new System.Windows.Forms.CheckBox();
            cbSaveCapture = new System.Windows.Forms.CheckBox();
            cbShowReplace = new System.Windows.Forms.CheckBox();
            plDebugOff = new System.Windows.Forms.Panel();
            label63 = new System.Windows.Forms.Label();
            btnDebugOn = new System.Windows.Forms.Button();
            lbDebugging = new System.Windows.Forms.Label();
            ContextOption.SuspendLayout();
            optionMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tbMain.SuspendLayout();
            tpBasic.SuspendLayout();
            panel8.SuspendLayout();
            pnAdjustImg.SuspendLayout();
            pnOCR.SuspendLayout();
            pnNHocr.SuspendLayout();
            WinOCR_panel.SuspendLayout();
            pnEasyOcr.SuspendLayout();
            Tesseract_panel.SuspendLayout();
            pnGoogleOcr.SuspendLayout();
            pnTranslate.SuspendLayout();
            pnGemini.SuspendLayout();
            Naver_Panel.SuspendLayout();
            Google_Panel.SuspendLayout();
            pnEzTrans.SuspendLayout();
            pnPapagoWeb.SuspendLayout();
            pnGoogleBasic.SuspendLayout();
            pnCustomApi.SuspendLayout();
            pnDeepLAPI.SuspendLayout();
            pnDeepl.SuspendLayout();
            DB_Panel.SuspendLayout();
            tpText.SuspendLayout();
            panel5.SuspendLayout();
            panel17.SuspendLayout();
            panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)backgroundColorBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)outlineColor2Box).BeginInit();
            ((System.ComponentModel.ISupportInitialize)outlineColor1Box).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textColorBox).BeginInit();
            panel9.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fontSizeUpDown).BeginInit();
            tpExtra.SuspendLayout();
            panel11.SuspendLayout();
            panel21.SuspendLayout();
            panel25.SuspendLayout();
            panel3.SuspendLayout();
            panel13.SuspendLayout();
            panel12.SuspendLayout();
            panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imgZoomsizeUpDown).BeginInit();
            tpTranslation.SuspendLayout();
            panel19.SuspendLayout();
            panel4.SuspendLayout();
            panel27.SuspendLayout();
            panel22.SuspendLayout();
            panel1.SuspendLayout();
            panel15.SuspendLayout();
            tpETC.SuspendLayout();
            panel18.SuspendLayout();
            panel16.SuspendLayout();
            panel20.SuspendLayout();
            panel23.SuspendLayout();
            panel2.SuspendLayout();
            tpQuickSetting.SuspendLayout();
            panel28.SuspendLayout();
            panel31.SuspendLayout();
            tpDebuging.SuspendLayout();
            panel24.SuspendLayout();
            panel26.SuspendLayout();
            plDebugOn.SuspendLayout();
            plDebugOff.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = ContextOption;
            notifyIcon1.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "MORT";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
            notifyIcon1.MouseClick += notifyIcon1_MouseClick;
            // 
            // ContextOption
            // 
            ContextOption.ImageScalingSize = new System.Drawing.Size(24, 24);
            ContextOption.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { optionToolStripMenuItem, showTransToolStripMenuItem, rTTToolStripMenuItem, toolStripSeparator7, setTranslateTopMostToolStripMenuItem, setCheckSpellingToolStripMenuItem, setCutPointToolStripMenuItem, toolStripSeparator5, transToolStripMenuItem, toolStripSeparator6, settingToolStripMenuItem, toolStripSeparator8, aboutToolStripMenuItem, checkUpdateToolStripMenuItem, ExitToolStripMenuItem });
            ContextOption.Name = "contextMenuStrip1";
            ContextOption.Size = new System.Drawing.Size(151, 270);
            // 
            // optionToolStripMenuItem
            // 
            optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            optionToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            optionToolStripMenuItem.Text = "옵션";
            optionToolStripMenuItem.Click += ContextOption_Click;
            // 
            // showTransToolStripMenuItem
            // 
            showTransToolStripMenuItem.Name = "showTransToolStripMenuItem";
            showTransToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            showTransToolStripMenuItem.Text = "번역창";
            showTransToolStripMenuItem.Click += showTransToolStripMenuItem_Click;
            // 
            // rTTToolStripMenuItem
            // 
            rTTToolStripMenuItem.Name = "rTTToolStripMenuItem";
            rTTToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            rTTToolStripMenuItem.Text = "리모컨";
            rTTToolStripMenuItem.Click += rTTToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(147, 6);
            // 
            // setTranslateTopMostToolStripMenuItem
            // 
            setTranslateTopMostToolStripMenuItem.Checked = true;
            setTranslateTopMostToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            setTranslateTopMostToolStripMenuItem.Name = "setTranslateTopMostToolStripMenuItem";
            setTranslateTopMostToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            setTranslateTopMostToolStripMenuItem.Text = "번역창 고정";
            setTranslateTopMostToolStripMenuItem.Click += setTranslateTopMostToolStripMenuItem_Click;
            // 
            // setCheckSpellingToolStripMenuItem
            // 
            setCheckSpellingToolStripMenuItem.Checked = true;
            setCheckSpellingToolStripMenuItem.CheckOnClick = true;
            setCheckSpellingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            setCheckSpellingToolStripMenuItem.Name = "setCheckSpellingToolStripMenuItem";
            setCheckSpellingToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            setCheckSpellingToolStripMenuItem.Text = "교정사전 사용";
            setCheckSpellingToolStripMenuItem.Click += setCheckSpellingToolStripMenuItem_Click;
            // 
            // setCutPointToolStripMenuItem
            // 
            setCutPointToolStripMenuItem.Name = "setCutPointToolStripMenuItem";
            setCutPointToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            setCutPointToolStripMenuItem.Text = "영역 설정";
            setCutPointToolStripMenuItem.Click += setCutPointToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(147, 6);
            // 
            // transToolStripMenuItem
            // 
            transToolStripMenuItem.Name = "transToolStripMenuItem";
            transToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            transToolStripMenuItem.Text = "번역 시작";
            transToolStripMenuItem.Click += ContextTranslate_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(147, 6);
            // 
            // settingToolStripMenuItem
            // 
            settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { settingSaveToolStripMenuItem2, settingLoadToolStripMenuItem2, settingDefaultToolStripMenuItem });
            settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            settingToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            settingToolStripMenuItem.Text = "설정";
            // 
            // settingSaveToolStripMenuItem2
            // 
            settingSaveToolStripMenuItem2.Name = "settingSaveToolStripMenuItem2";
            settingSaveToolStripMenuItem2.Size = new System.Drawing.Size(122, 22);
            settingSaveToolStripMenuItem2.Text = "저장";
            settingSaveToolStripMenuItem2.Click += settingSaveToolStripMenuItem2_Click;
            // 
            // settingLoadToolStripMenuItem2
            // 
            settingLoadToolStripMenuItem2.Name = "settingLoadToolStripMenuItem2";
            settingLoadToolStripMenuItem2.Size = new System.Drawing.Size(122, 22);
            settingLoadToolStripMenuItem2.Text = "불러오기";
            settingLoadToolStripMenuItem2.Click += settingLoadToolStripMenuItem2_Click;
            // 
            // settingDefaultToolStripMenuItem
            // 
            settingDefaultToolStripMenuItem.Name = "settingDefaultToolStripMenuItem";
            settingDefaultToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            settingDefaultToolStripMenuItem.Text = "초기화";
            settingDefaultToolStripMenuItem.Click += settingDefaultToolStripMenuItem_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(147, 6);
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // checkUpdateToolStripMenuItem
            // 
            checkUpdateToolStripMenuItem.Name = "checkUpdateToolStripMenuItem";
            checkUpdateToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            checkUpdateToolStripMenuItem.Text = "업데이트";
            checkUpdateToolStripMenuItem.Click += checkUpdateToolStripMenuItem_Click;
            // 
            // ExitToolStripMenuItem
            // 
            ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            ExitToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            ExitToolStripMenuItem.Text = "종료";
            ExitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // optionMenuStrip
            // 
            optionMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            optionMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 설정저장ToolStripMenuItem, 설정불러오기ToolStripMenuItem });
            optionMenuStrip.Name = "optionMenuStrip";
            optionMenuStrip.Size = new System.Drawing.Size(151, 48);
            // 
            // 설정저장ToolStripMenuItem
            // 
            설정저장ToolStripMenuItem.Name = "설정저장ToolStripMenuItem";
            설정저장ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            설정저장ToolStripMenuItem.Text = "설정 저장";
            // 
            // 설정불러오기ToolStripMenuItem
            // 
            설정불러오기ToolStripMenuItem.Name = "설정불러오기ToolStripMenuItem";
            설정불러오기ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            설정불러오기ToolStripMenuItem.Text = "설정 불러오기";
            // 
            // acceptButton
            // 
            acceptButton.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            acceptButton.FlatAppearance.BorderSize = 0;
            acceptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            acceptButton.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            acceptButton.ForeColor = System.Drawing.Color.White;
            acceptButton.Location = new System.Drawing.Point(398, 589);
            acceptButton.Margin = new System.Windows.Forms.Padding(0);
            acceptButton.Name = "acceptButton";
            acceptButton.Size = new System.Drawing.Size(195, 55);
            acceptButton.TabIndex = 44;
            acceptButton.Text = "적 용";
            acceptButton.UseVisualStyleBackColor = false;
            acceptButton.Click += acceptButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            pictureBox1.Location = new System.Drawing.Point(0, 593);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(76, 165);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 45;
            pictureBox1.TabStop = false;
            // 
            // donationButton
            // 
            donationButton.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            donationButton.FlatAppearance.BorderSize = 0;
            donationButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            donationButton.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            donationButton.ForeColor = System.Drawing.Color.White;
            donationButton.Location = new System.Drawing.Point(100, 589);
            donationButton.Margin = new System.Windows.Forms.Padding(0);
            donationButton.Name = "donationButton";
            donationButton.Size = new System.Drawing.Size(195, 55);
            donationButton.TabIndex = 46;
            donationButton.Text = "후원하기";
            donationButton.UseVisualStyleBackColor = false;
            donationButton.Click += donationButton_Click;
            // 
            // toolTip_OCR
            // 
            toolTip_OCR.AutoPopDelay = 500000000;
            toolTip_OCR.InitialDelay = 300;
            toolTip_OCR.ReshowDelay = 100;
            // 
            // tbMain
            // 
            tbMain.Alignment = System.Windows.Forms.TabAlignment.Left;
            tbMain.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            tbMain.Controls.Add(tpBasic);
            tbMain.Controls.Add(tpText);
            tbMain.Controls.Add(tpExtra);
            tbMain.Controls.Add(tpTranslation);
            tbMain.Controls.Add(tpETC);
            tbMain.Controls.Add(tpQuickSetting);
            tbMain.Controls.Add(tpDebuging);
            tbMain.Dock = System.Windows.Forms.DockStyle.Top;
            tbMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            tbMain.Font = new System.Drawing.Font("맑은 고딕", 9F);
            tbMain.ItemSize = new System.Drawing.Size(44, 76);
            tbMain.Location = new System.Drawing.Point(0, 0);
            tbMain.Margin = new System.Windows.Forms.Padding(0);
            tbMain.Multiline = true;
            tbMain.Name = "tbMain";
            tbMain.Padding = new System.Drawing.Point(0, 0);
            tbMain.SelectedIndex = 0;
            tbMain.Size = new System.Drawing.Size(624, 593);
            tbMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            tbMain.TabIndex = 43;
            tbMain.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tpBasic
            // 
            tpBasic.Controls.Add(panel8);
            tpBasic.ForeColor = System.Drawing.SystemColors.ControlText;
            tpBasic.Location = new System.Drawing.Point(80, 4);
            tpBasic.Margin = new System.Windows.Forms.Padding(0);
            tpBasic.Name = "tpBasic";
            tpBasic.Size = new System.Drawing.Size(540, 585);
            tpBasic.TabIndex = 0;
            tpBasic.Text = "기본설정";
            tpBasic.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            panel8.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel8.Controls.Add(pnAdjustImg);
            panel8.Controls.Add(pnOCR);
            panel8.Controls.Add(pnTranslate);
            panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            panel8.Location = new System.Drawing.Point(0, 0);
            panel8.Margin = new System.Windows.Forms.Padding(0);
            panel8.Name = "panel8";
            panel8.Size = new System.Drawing.Size(540, 585);
            panel8.TabIndex = 39;
            // 
            // pnAdjustImg
            // 
            pnAdjustImg.Controls.Add(btImgResult);
            pnAdjustImg.Controls.Add(cbThreshold);
            pnAdjustImg.Controls.Add(tbThreshold);
            pnAdjustImg.Controls.Add(groupCombo);
            pnAdjustImg.Controls.Add(groupLabel);
            pnAdjustImg.Controls.Add(lbImgGroupCount);
            pnAdjustImg.Controls.Add(lbImgGroup);
            pnAdjustImg.Controls.Add(label1);
            pnAdjustImg.Controls.Add(v2TextBox);
            pnAdjustImg.Controls.Add(lbAdjustImg);
            pnAdjustImg.Controls.Add(checkRGB);
            pnAdjustImg.Controls.Add(s2TextBox);
            pnAdjustImg.Controls.Add(label3);
            pnAdjustImg.Controls.Add(label16);
            pnAdjustImg.Controls.Add(rTextBox);
            pnAdjustImg.Controls.Add(checkErode);
            pnAdjustImg.Controls.Add(label4);
            pnAdjustImg.Controls.Add(checkHSV);
            pnAdjustImg.Controls.Add(gTextBox);
            pnAdjustImg.Controls.Add(label5);
            pnAdjustImg.Controls.Add(v1TextBox);
            pnAdjustImg.Controls.Add(bTextBox);
            pnAdjustImg.Controls.Add(label7);
            pnAdjustImg.Controls.Add(label8);
            pnAdjustImg.Controls.Add(s1TextBox);
            pnAdjustImg.Location = new System.Drawing.Point(3, 387);
            pnAdjustImg.Name = "pnAdjustImg";
            pnAdjustImg.Size = new System.Drawing.Size(533, 183);
            pnAdjustImg.TabIndex = 37;
            pnAdjustImg.Paint += panealBorder_Paint;
            // 
            // btImgResult
            // 
            btImgResult.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btImgResult.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btImgResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btImgResult.ForeColor = System.Drawing.Color.White;
            btImgResult.Location = new System.Drawing.Point(244, 109);
            btImgResult.Name = "btImgResult";
            btImgResult.Size = new System.Drawing.Size(196, 25);
            btImgResult.TabIndex = 51;
            btImgResult.Text = "보정 결과 확인하기";
            btImgResult.UseVisualStyleBackColor = false;
            btImgResult.Click += OnClickShowImgResult;
            // 
            // cbThreshold
            // 
            cbThreshold.AutoSize = true;
            cbThreshold.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbThreshold.ForeColor = System.Drawing.Color.White;
            cbThreshold.Location = new System.Drawing.Point(12, 82);
            cbThreshold.Name = "cbThreshold";
            cbThreshold.Size = new System.Drawing.Size(123, 21);
            cbThreshold.TabIndex = 49;
            cbThreshold.Text = "임계값으로 추출";
            cbThreshold.UseVisualStyleBackColor = true;
            cbThreshold.CheckedChanged += cbThreshold_CheckedChanged;
            // 
            // tbThreshold
            // 
            tbThreshold.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            tbThreshold.Location = new System.Drawing.Point(163, 81);
            tbThreshold.Name = "tbThreshold";
            tbThreshold.Size = new System.Drawing.Size(47, 25);
            tbThreshold.TabIndex = 50;
            tbThreshold.Text = "0";
            tbThreshold.KeyPress += textBox_KeyPress;
            tbThreshold.Leave += thresholdTextLeave;
            // 
            // groupCombo
            // 
            groupCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            groupCombo.FormattingEnabled = true;
            groupCombo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            groupCombo.Location = new System.Drawing.Point(217, 144);
            groupCombo.Name = "groupCombo";
            groupCombo.Size = new System.Drawing.Size(56, 23);
            groupCombo.TabIndex = 47;
            groupCombo.SelectedIndexChanged += groupCombo_SelectedIndexChanged;
            // 
            // groupLabel
            // 
            groupLabel.AutoSize = true;
            groupLabel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            groupLabel.ForeColor = System.Drawing.Color.White;
            groupLabel.Location = new System.Drawing.Point(350, 144);
            groupLabel.Name = "groupLabel";
            groupLabel.Size = new System.Drawing.Size(16, 17);
            groupLabel.TabIndex = 46;
            groupLabel.Text = "0";
            // 
            // lbImgGroupCount
            // 
            lbImgGroupCount.AutoSize = true;
            lbImgGroupCount.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbImgGroupCount.ForeColor = System.Drawing.Color.White;
            lbImgGroupCount.Location = new System.Drawing.Point(279, 144);
            lbImgGroupCount.Name = "lbImgGroupCount";
            lbImgGroupCount.Size = new System.Drawing.Size(65, 17);
            lbImgGroupCount.TabIndex = 45;
            lbImgGroupCount.Text = "그룹 수 : ";
            // 
            // lbImgGroup
            // 
            lbImgGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lbImgGroup.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbImgGroup.ForeColor = System.Drawing.Color.White;
            lbImgGroup.Location = new System.Drawing.Point(91, 146);
            lbImgGroup.Name = "lbImgGroup";
            lbImgGroup.Size = new System.Drawing.Size(119, 23);
            lbImgGroup.TabIndex = 43;
            lbImgGroup.Text = "범위 그룹";
            lbImgGroup.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(368, 53);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(17, 17);
            label1.TabIndex = 18;
            label1.Text = "~";
            // 
            // v2TextBox
            // 
            v2TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            v2TextBox.Location = new System.Drawing.Point(388, 51);
            v2TextBox.Name = "v2TextBox";
            v2TextBox.Size = new System.Drawing.Size(47, 25);
            v2TextBox.TabIndex = 16;
            v2TextBox.Text = "0";
            v2TextBox.KeyPress += textBox_KeyPress;
            v2TextBox.Leave += hsvTextLeave;
            // 
            // lbAdjustImg
            // 
            lbAdjustImg.AutoSize = true;
            lbAdjustImg.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbAdjustImg.ForeColor = System.Drawing.Color.White;
            lbAdjustImg.Location = new System.Drawing.Point(4, 3);
            lbAdjustImg.Name = "lbAdjustImg";
            lbAdjustImg.Size = new System.Drawing.Size(89, 20);
            lbAdjustImg.TabIndex = 8;
            lbAdjustImg.Text = "이미지 보정";
            // 
            // checkRGB
            // 
            checkRGB.AutoSize = true;
            checkRGB.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            checkRGB.ForeColor = System.Drawing.Color.White;
            checkRGB.Location = new System.Drawing.Point(12, 25);
            checkRGB.Name = "checkRGB";
            checkRGB.Size = new System.Drawing.Size(96, 21);
            checkRGB.TabIndex = 8;
            checkRGB.Text = "RGB로 추출";
            checkRGB.UseVisualStyleBackColor = true;
            checkRGB.MouseDown += checkRGB_MouseDown;
            // 
            // s2TextBox
            // 
            s2TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            s2TextBox.Location = new System.Drawing.Point(239, 51);
            s2TextBox.Name = "s2TextBox";
            s2TextBox.Size = new System.Drawing.Size(47, 25);
            s2TextBox.TabIndex = 14;
            s2TextBox.Text = "0";
            s2TextBox.KeyPress += textBox_KeyPress;
            s2TextBox.Leave += hsvTextLeave;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label3.ForeColor = System.Drawing.Color.White;
            label3.Location = new System.Drawing.Point(140, 25);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(16, 17);
            label3.TabIndex = 2;
            label3.Text = "R";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label16.ForeColor = System.Drawing.Color.White;
            label16.Location = new System.Drawing.Point(217, 53);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(17, 17);
            label16.TabIndex = 16;
            label16.Text = "~";
            // 
            // rTextBox
            // 
            rTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            rTextBox.Location = new System.Drawing.Point(163, 22);
            rTextBox.Name = "rTextBox";
            rTextBox.Size = new System.Drawing.Size(47, 25);
            rTextBox.TabIndex = 9;
            rTextBox.Text = "0";
            rTextBox.KeyPress += textBox_KeyPress;
            rTextBox.Leave += rgbTextLeave;
            // 
            // checkErode
            // 
            checkErode.AutoSize = true;
            checkErode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            checkErode.ForeColor = System.Drawing.Color.White;
            checkErode.Location = new System.Drawing.Point(12, 113);
            checkErode.Name = "checkErode";
            checkErode.Size = new System.Drawing.Size(226, 21);
            checkErode.TabIndex = 17;
            checkErode.Text = "침식 사용 (굵은 글씨체일때 사용)";
            checkErode.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label4.ForeColor = System.Drawing.Color.White;
            label4.Location = new System.Drawing.Point(216, 25);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(17, 17);
            label4.TabIndex = 4;
            label4.Text = "G";
            // 
            // checkHSV
            // 
            checkHSV.AutoSize = true;
            checkHSV.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            checkHSV.ForeColor = System.Drawing.Color.White;
            checkHSV.Location = new System.Drawing.Point(12, 52);
            checkHSV.Name = "checkHSV";
            checkHSV.Size = new System.Drawing.Size(97, 21);
            checkHSV.TabIndex = 12;
            checkHSV.Text = "HSV로 추출";
            checkHSV.UseVisualStyleBackColor = true;
            checkHSV.MouseDown += checkHSV_MouseDown;
            // 
            // gTextBox
            // 
            gTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            gTextBox.Location = new System.Drawing.Point(239, 22);
            gTextBox.Name = "gTextBox";
            gTextBox.Size = new System.Drawing.Size(47, 25);
            gTextBox.TabIndex = 10;
            gTextBox.Text = "0";
            gTextBox.KeyPress += textBox_KeyPress;
            gTextBox.Leave += rgbTextLeave;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label5.ForeColor = System.Drawing.Color.White;
            label5.Location = new System.Drawing.Point(292, 25);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(16, 17);
            label5.TabIndex = 6;
            label5.Text = "B";
            // 
            // v1TextBox
            // 
            v1TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            v1TextBox.Location = new System.Drawing.Point(315, 51);
            v1TextBox.Name = "v1TextBox";
            v1TextBox.Size = new System.Drawing.Size(47, 25);
            v1TextBox.TabIndex = 15;
            v1TextBox.Text = "0";
            v1TextBox.KeyPress += textBox_KeyPress;
            v1TextBox.Leave += hsvTextLeave;
            // 
            // bTextBox
            // 
            bTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            bTextBox.Location = new System.Drawing.Point(315, 22);
            bTextBox.Name = "bTextBox";
            bTextBox.Size = new System.Drawing.Size(47, 25);
            bTextBox.TabIndex = 11;
            bTextBox.Text = "0";
            bTextBox.KeyPress += textBox_KeyPress;
            bTextBox.Leave += rgbTextLeave;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label7.ForeColor = System.Drawing.Color.White;
            label7.Location = new System.Drawing.Point(292, 52);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(17, 17);
            label7.TabIndex = 11;
            label7.Text = "V";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label8.ForeColor = System.Drawing.Color.White;
            label8.Location = new System.Drawing.Point(140, 52);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(15, 17);
            label8.TabIndex = 9;
            label8.Text = "S";
            // 
            // s1TextBox
            // 
            s1TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            s1TextBox.Location = new System.Drawing.Point(163, 51);
            s1TextBox.Name = "s1TextBox";
            s1TextBox.Size = new System.Drawing.Size(47, 25);
            s1TextBox.TabIndex = 13;
            s1TextBox.Text = "0";
            s1TextBox.KeyPress += textBox_KeyPress;
            s1TextBox.Leave += hsvTextLeave;
            // 
            // pnOCR
            // 
            pnOCR.Controls.Add(btOcrHelp);
            pnOCR.Controls.Add(label48);
            pnOCR.Controls.Add(OCR_Type_comboBox);
            pnOCR.Controls.Add(isClipBoardcheckBox1);
            pnOCR.Controls.Add(saveOCRCheckBox);
            pnOCR.Controls.Add(ocrLabel);
            pnOCR.Controls.Add(showOcrCheckBox);
            pnOCR.Controls.Add(pnNHocr);
            pnOCR.Controls.Add(WinOCR_panel);
            pnOCR.Controls.Add(pnEasyOcr);
            pnOCR.Controls.Add(Tesseract_panel);
            pnOCR.Controls.Add(pnGoogleOcr);
            pnOCR.Location = new System.Drawing.Point(3, 3);
            pnOCR.Name = "pnOCR";
            pnOCR.Size = new System.Drawing.Size(533, 155);
            pnOCR.TabIndex = 37;
            pnOCR.Paint += panealBorder_Paint;
            // 
            // btOcrHelp
            // 
            btOcrHelp.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            btOcrHelp.FlatAppearance.BorderSize = 0;
            btOcrHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btOcrHelp.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            btOcrHelp.ForeColor = System.Drawing.Color.White;
            btOcrHelp.Location = new System.Drawing.Point(320, 31);
            btOcrHelp.Margin = new System.Windows.Forms.Padding(0);
            btOcrHelp.Name = "btOcrHelp";
            btOcrHelp.Size = new System.Drawing.Size(28, 25);
            btOcrHelp.TabIndex = 59;
            btOcrHelp.Text = "?";
            btOcrHelp.UseVisualStyleBackColor = false;
            btOcrHelp.Click += OnClick_btOcrHelp;
            // 
            // label48
            // 
            label48.AutoSize = true;
            label48.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label48.ForeColor = System.Drawing.Color.White;
            label48.Location = new System.Drawing.Point(16, 34);
            label48.Name = "label48";
            label48.Size = new System.Drawing.Size(39, 17);
            label48.TabIndex = 50;
            label48.Text = "OCR ";
            // 
            // OCR_Type_comboBox
            // 
            OCR_Type_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            OCR_Type_comboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            OCR_Type_comboBox.FormattingEnabled = true;
            OCR_Type_comboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            OCR_Type_comboBox.Items.AddRange(new object[] { "OCR Tesseract", "OCR Win OCR", "OCR Snipping Tool OCR", "OCR Google", "OCR Easy OCR" });
            OCR_Type_comboBox.Location = new System.Drawing.Point(105, 31);
            OCR_Type_comboBox.Name = "OCR_Type_comboBox";
            OCR_Type_comboBox.Size = new System.Drawing.Size(212, 25);
            OCR_Type_comboBox.TabIndex = 51;
            OCR_Type_comboBox.SelectedIndexChanged += OCR_Type_comboBox_SelectedIndexChanged;
            // 
            // isClipBoardcheckBox1
            // 
            isClipBoardcheckBox1.AutoSize = true;
            isClipBoardcheckBox1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            isClipBoardcheckBox1.ForeColor = System.Drawing.Color.White;
            isClipBoardcheckBox1.Location = new System.Drawing.Point(367, 123);
            isClipBoardcheckBox1.Name = "isClipBoardcheckBox1";
            isClipBoardcheckBox1.Size = new System.Drawing.Size(123, 21);
            isClipBoardcheckBox1.TabIndex = 26;
            isClipBoardcheckBox1.Text = "클립보드에 저장";
            isClipBoardcheckBox1.UseVisualStyleBackColor = true;
            // 
            // saveOCRCheckBox
            // 
            saveOCRCheckBox.AutoSize = true;
            saveOCRCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            saveOCRCheckBox.ForeColor = System.Drawing.Color.White;
            saveOCRCheckBox.Location = new System.Drawing.Point(193, 123);
            saveOCRCheckBox.Name = "saveOCRCheckBox";
            saveOCRCheckBox.Size = new System.Drawing.Size(115, 21);
            saveOCRCheckBox.TabIndex = 24;
            saveOCRCheckBox.Text = "OCR 결과 저장";
            saveOCRCheckBox.UseVisualStyleBackColor = true;
            // 
            // ocrLabel
            // 
            ocrLabel.AutoSize = true;
            ocrLabel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            ocrLabel.ForeColor = System.Drawing.Color.White;
            ocrLabel.Location = new System.Drawing.Point(4, 3);
            ocrLabel.Name = "ocrLabel";
            ocrLabel.Size = new System.Drawing.Size(41, 20);
            ocrLabel.TabIndex = 8;
            ocrLabel.Text = "OCR";
            // 
            // showOcrCheckBox
            // 
            showOcrCheckBox.AutoSize = true;
            showOcrCheckBox.Checked = true;
            showOcrCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            showOcrCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            showOcrCheckBox.ForeColor = System.Drawing.Color.White;
            showOcrCheckBox.Location = new System.Drawing.Point(27, 123);
            showOcrCheckBox.Name = "showOcrCheckBox";
            showOcrCheckBox.Size = new System.Drawing.Size(115, 21);
            showOcrCheckBox.TabIndex = 2;
            showOcrCheckBox.Text = "OCR 결과 출력";
            showOcrCheckBox.UseVisualStyleBackColor = true;
            // 
            // pnNHocr
            // 
            pnNHocr.Controls.Add(cbOneOcrLanguage);
            pnNHocr.Controls.Add(lbOneOcrLanguage);
            pnNHocr.Location = new System.Drawing.Point(8, 54);
            pnNHocr.Name = "pnNHocr";
            pnNHocr.Size = new System.Drawing.Size(471, 63);
            pnNHocr.TabIndex = 56;
            // 
            // cbOneOcrLanguage
            // 
            cbOneOcrLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbOneOcrLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbOneOcrLanguage.FormattingEnabled = true;
            cbOneOcrLanguage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbOneOcrLanguage.Items.AddRange(new object[] { "en", "ja", "kr", "etc" });
            cbOneOcrLanguage.Location = new System.Drawing.Point(97, 7);
            cbOneOcrLanguage.Name = "cbOneOcrLanguage";
            cbOneOcrLanguage.Size = new System.Drawing.Size(212, 25);
            cbOneOcrLanguage.TabIndex = 54;
            cbOneOcrLanguage.SelectedIndexChanged += cbOneOcrLanguage_SelectedIndexChanged;
            // 
            // lbOneOcrLanguage
            // 
            lbOneOcrLanguage.AutoSize = true;
            lbOneOcrLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbOneOcrLanguage.ForeColor = System.Drawing.Color.White;
            lbOneOcrLanguage.Location = new System.Drawing.Point(8, 9);
            lbOneOcrLanguage.Name = "lbOneOcrLanguage";
            lbOneOcrLanguage.Size = new System.Drawing.Size(39, 17);
            lbOneOcrLanguage.TabIndex = 53;
            lbOneOcrLanguage.Text = "언어 ";
            // 
            // WinOCR_panel
            // 
            WinOCR_panel.Controls.Add(btnAddWinOcrLanguage);
            WinOCR_panel.Controls.Add(WinOCR_Language_comboBox);
            WinOCR_panel.Controls.Add(lbWinOCRLanguage);
            WinOCR_panel.Location = new System.Drawing.Point(8, 54);
            WinOCR_panel.Name = "WinOCR_panel";
            WinOCR_panel.Size = new System.Drawing.Size(471, 63);
            WinOCR_panel.TabIndex = 54;
            // 
            // btnAddWinOcrLanguage
            // 
            btnAddWinOcrLanguage.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnAddWinOcrLanguage.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnAddWinOcrLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAddWinOcrLanguage.ForeColor = System.Drawing.Color.White;
            btnAddWinOcrLanguage.Location = new System.Drawing.Point(312, 8);
            btnAddWinOcrLanguage.Name = "btnAddWinOcrLanguage";
            btnAddWinOcrLanguage.Size = new System.Drawing.Size(156, 25);
            btnAddWinOcrLanguage.TabIndex = 61;
            btnAddWinOcrLanguage.Text = "언어 추가";
            btnAddWinOcrLanguage.UseVisualStyleBackColor = false;
            btnAddWinOcrLanguage.Click += btnAddWinOcrLanguage_Click;
            // 
            // WinOCR_Language_comboBox
            // 
            WinOCR_Language_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            WinOCR_Language_comboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            WinOCR_Language_comboBox.FormattingEnabled = true;
            WinOCR_Language_comboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            WinOCR_Language_comboBox.Items.AddRange(new object[] { "초기화 실패" });
            WinOCR_Language_comboBox.Location = new System.Drawing.Point(97, 6);
            WinOCR_Language_comboBox.Name = "WinOCR_Language_comboBox";
            WinOCR_Language_comboBox.Size = new System.Drawing.Size(212, 25);
            WinOCR_Language_comboBox.TabIndex = 52;
            WinOCR_Language_comboBox.SelectionChangeCommitted += WinOCR_Language_comboBox_SelectionChangeCommitted;
            // 
            // lbWinOCRLanguage
            // 
            lbWinOCRLanguage.AutoSize = true;
            lbWinOCRLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbWinOCRLanguage.ForeColor = System.Drawing.Color.White;
            lbWinOCRLanguage.Location = new System.Drawing.Point(8, 9);
            lbWinOCRLanguage.Name = "lbWinOCRLanguage";
            lbWinOCRLanguage.Size = new System.Drawing.Size(39, 17);
            lbWinOCRLanguage.TabIndex = 50;
            lbWinOCRLanguage.Text = "언어 ";
            // 
            // pnEasyOcr
            // 
            pnEasyOcr.Controls.Add(btnInstallEasyOcr);
            pnEasyOcr.Controls.Add(cbEasyOcrCode);
            pnEasyOcr.Controls.Add(lbEasyOcrLanguage);
            pnEasyOcr.Location = new System.Drawing.Point(8, 54);
            pnEasyOcr.Name = "pnEasyOcr";
            pnEasyOcr.Size = new System.Drawing.Size(471, 63);
            pnEasyOcr.TabIndex = 61;
            // 
            // btnInstallEasyOcr
            // 
            btnInstallEasyOcr.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnInstallEasyOcr.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnInstallEasyOcr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnInstallEasyOcr.ForeColor = System.Drawing.Color.White;
            btnInstallEasyOcr.Location = new System.Drawing.Point(284, 8);
            btnInstallEasyOcr.Name = "btnInstallEasyOcr";
            btnInstallEasyOcr.Size = new System.Drawing.Size(184, 25);
            btnInstallEasyOcr.TabIndex = 60;
            btnInstallEasyOcr.Text = "Easy OCR 설치";
            btnInstallEasyOcr.UseVisualStyleBackColor = false;
            btnInstallEasyOcr.Click += btnInstallEasyOcr_Click;
            // 
            // cbEasyOcrCode
            // 
            cbEasyOcrCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbEasyOcrCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbEasyOcrCode.FormattingEnabled = true;
            cbEasyOcrCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbEasyOcrCode.Items.AddRange(new object[] { "자동" });
            cbEasyOcrCode.Location = new System.Drawing.Point(97, 9);
            cbEasyOcrCode.Name = "cbEasyOcrCode";
            cbEasyOcrCode.Size = new System.Drawing.Size(181, 25);
            cbEasyOcrCode.TabIndex = 55;
            cbEasyOcrCode.SelectionChangeCommitted += cbEasyOcrOcde_SelectionChangeCommitted;
            // 
            // lbEasyOcrLanguage
            // 
            lbEasyOcrLanguage.AutoSize = true;
            lbEasyOcrLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbEasyOcrLanguage.ForeColor = System.Drawing.Color.White;
            lbEasyOcrLanguage.Location = new System.Drawing.Point(8, 12);
            lbEasyOcrLanguage.Name = "lbEasyOcrLanguage";
            lbEasyOcrLanguage.Size = new System.Drawing.Size(39, 17);
            lbEasyOcrLanguage.TabIndex = 54;
            lbEasyOcrLanguage.Text = "언어 ";
            // 
            // Tesseract_panel
            // 
            Tesseract_panel.Controls.Add(cbFastTess);
            Tesseract_panel.Controls.Add(tesseractLanguageComboBox);
            Tesseract_panel.Controls.Add(lbTesseractLanguage);
            Tesseract_panel.Controls.Add(label18);
            Tesseract_panel.Controls.Add(tessDataTextBox);
            Tesseract_panel.Location = new System.Drawing.Point(8, 54);
            Tesseract_panel.Name = "Tesseract_panel";
            Tesseract_panel.Size = new System.Drawing.Size(471, 63);
            Tesseract_panel.TabIndex = 53;
            // 
            // cbFastTess
            // 
            cbFastTess.AutoSize = true;
            cbFastTess.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbFastTess.ForeColor = System.Drawing.Color.White;
            cbFastTess.Location = new System.Drawing.Point(19, 42);
            cbFastTess.Name = "cbFastTess";
            cbFastTess.Size = new System.Drawing.Size(335, 21);
            cbFastTess.TabIndex = 55;
            cbFastTess.Text = "고속 모드 (빠르나 정확도가 떨어짐, Tesseract 전용)";
            cbFastTess.UseVisualStyleBackColor = true;
            // 
            // tesseractLanguageComboBox
            // 
            tesseractLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            tesseractLanguageComboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            tesseractLanguageComboBox.FormattingEnabled = true;
            tesseractLanguageComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            tesseractLanguageComboBox.Items.AddRange(new object[] { "en", "ja", "Language ETC" });
            tesseractLanguageComboBox.Location = new System.Drawing.Point(347, 6);
            tesseractLanguageComboBox.Name = "tesseractLanguageComboBox";
            tesseractLanguageComboBox.Size = new System.Drawing.Size(75, 25);
            tesseractLanguageComboBox.TabIndex = 52;
            tesseractLanguageComboBox.SelectionChangeCommitted += tesseractLanguageComboBox_SelectionChangeCommitted;
            // 
            // lbTesseractLanguage
            // 
            lbTesseractLanguage.AutoSize = true;
            lbTesseractLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbTesseractLanguage.ForeColor = System.Drawing.Color.White;
            lbTesseractLanguage.Location = new System.Drawing.Point(273, 10);
            lbTesseractLanguage.Name = "lbTesseractLanguage";
            lbTesseractLanguage.Size = new System.Drawing.Size(65, 17);
            lbTesseractLanguage.TabIndex = 51;
            lbTesseractLanguage.Text = "추출 언어";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label18.ForeColor = System.Drawing.Color.White;
            label18.Location = new System.Drawing.Point(10, 9);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(62, 17);
            label18.TabIndex = 50;
            label18.Text = "Tessdata";
            // 
            // tessDataTextBox
            // 
            tessDataTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            tessDataTextBox.Location = new System.Drawing.Point(97, 7);
            tessDataTextBox.Name = "tessDataTextBox";
            tessDataTextBox.Size = new System.Drawing.Size(141, 22);
            tessDataTextBox.TabIndex = 49;
            tessDataTextBox.Text = "eng";
            // 
            // pnGoogleOcr
            // 
            pnGoogleOcr.Controls.Add(cbGoogleOcrLanguge);
            pnGoogleOcr.Controls.Add(lbGoogleOCRLanguage);
            pnGoogleOcr.Controls.Add(lbGoogleOcrStatus);
            pnGoogleOcr.Controls.Add(btnSettingGoogleOCR);
            pnGoogleOcr.Location = new System.Drawing.Point(8, 54);
            pnGoogleOcr.Name = "pnGoogleOcr";
            pnGoogleOcr.Size = new System.Drawing.Size(471, 63);
            pnGoogleOcr.TabIndex = 60;
            // 
            // cbGoogleOcrLanguge
            // 
            cbGoogleOcrLanguge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbGoogleOcrLanguge.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbGoogleOcrLanguge.FormattingEnabled = true;
            cbGoogleOcrLanguge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbGoogleOcrLanguge.Items.AddRange(new object[] { "자동" });
            cbGoogleOcrLanguge.Location = new System.Drawing.Point(97, 9);
            cbGoogleOcrLanguge.Name = "cbGoogleOcrLanguge";
            cbGoogleOcrLanguge.Size = new System.Drawing.Size(212, 25);
            cbGoogleOcrLanguge.TabIndex = 55;
            cbGoogleOcrLanguge.SelectedIndexChanged += cbGoogleOcrLanguge_SelectedIndexChanged;
            // 
            // lbGoogleOCRLanguage
            // 
            lbGoogleOCRLanguage.AutoSize = true;
            lbGoogleOCRLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbGoogleOCRLanguage.ForeColor = System.Drawing.Color.White;
            lbGoogleOCRLanguage.Location = new System.Drawing.Point(8, 12);
            lbGoogleOCRLanguage.Name = "lbGoogleOCRLanguage";
            lbGoogleOCRLanguage.Size = new System.Drawing.Size(39, 17);
            lbGoogleOCRLanguage.TabIndex = 54;
            lbGoogleOCRLanguage.Text = "언어 ";
            // 
            // lbGoogleOcrStatus
            // 
            lbGoogleOcrStatus.AutoSize = true;
            lbGoogleOcrStatus.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbGoogleOcrStatus.ForeColor = System.Drawing.Color.White;
            lbGoogleOcrStatus.Location = new System.Drawing.Point(94, 39);
            lbGoogleOcrStatus.Name = "lbGoogleOcrStatus";
            lbGoogleOcrStatus.Size = new System.Drawing.Size(304, 17);
            lbGoogleOcrStatus.TabIndex = 53;
            lbGoogleOcrStatus.Text = "스냅샷 / 한 번만 번역하기에서만 사용 가능합니다";
            // 
            // btnSettingGoogleOCR
            // 
            btnSettingGoogleOCR.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnSettingGoogleOCR.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnSettingGoogleOCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSettingGoogleOCR.ForeColor = System.Drawing.Color.White;
            btnSettingGoogleOCR.Location = new System.Drawing.Point(312, 8);
            btnSettingGoogleOCR.Name = "btnSettingGoogleOCR";
            btnSettingGoogleOCR.Size = new System.Drawing.Size(156, 25);
            btnSettingGoogleOCR.TabIndex = 52;
            btnSettingGoogleOCR.Text = "API 설정";
            btnSettingGoogleOCR.UseVisualStyleBackColor = false;
            btnSettingGoogleOCR.Click += OnClick_btGoogleOcrSetting;
            // 
            // pnTranslate
            // 
            pnTranslate.Controls.Add(pnGemini);
            pnTranslate.Controls.Add(btnTransHelp);
            pnTranslate.Controls.Add(cbPerWordDic);
            pnTranslate.Controls.Add(lbTransType);
            pnTranslate.Controls.Add(_cbTranslateType);
            pnTranslate.Controls.Add(checkDic);
            pnTranslate.Controls.Add(dicFileTextBox);
            pnTranslate.Controls.Add(lbDicFile);
            pnTranslate.Controls.Add(lbTransTypeTitle);
            pnTranslate.Controls.Add(Naver_Panel);
            pnTranslate.Controls.Add(Google_Panel);
            pnTranslate.Controls.Add(pnEzTrans);
            pnTranslate.Controls.Add(pnPapagoWeb);
            pnTranslate.Controls.Add(pnGoogleBasic);
            pnTranslate.Controls.Add(pnCustomApi);
            pnTranslate.Controls.Add(pnDeepLAPI);
            pnTranslate.Controls.Add(pnDeepl);
            pnTranslate.Controls.Add(DB_Panel);
            pnTranslate.Location = new System.Drawing.Point(3, 164);
            pnTranslate.Name = "pnTranslate";
            pnTranslate.Size = new System.Drawing.Size(533, 217);
            pnTranslate.TabIndex = 37;
            pnTranslate.Paint += panealBorder_Paint;
            // 
            // pnGemini
            // 
            pnGemini.Controls.Add(cbGeminiModel);
            pnGemini.Controls.Add(lbGeminiModel);
            pnGemini.Controls.Add(tbGeminiApi);
            pnGemini.Controls.Add(lbGeminiApi);
            pnGemini.Location = new System.Drawing.Point(7, 61);
            pnGemini.Name = "pnGemini";
            pnGemini.Size = new System.Drawing.Size(483, 94);
            pnGemini.TabIndex = 60;
            // 
            // cbGeminiModel
            // 
            cbGeminiModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbGeminiModel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbGeminiModel.FormattingEnabled = true;
            cbGeminiModel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbGeminiModel.Location = new System.Drawing.Point(98, 36);
            cbGeminiModel.Name = "cbGeminiModel";
            cbGeminiModel.Size = new System.Drawing.Size(354, 25);
            cbGeminiModel.TabIndex = 53;
            // 
            // lbGeminiModel
            // 
            lbGeminiModel.AutoSize = true;
            lbGeminiModel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbGeminiModel.ForeColor = System.Drawing.Color.White;
            lbGeminiModel.Location = new System.Drawing.Point(3, 38);
            lbGeminiModel.Name = "lbGeminiModel";
            lbGeminiModel.Size = new System.Drawing.Size(48, 17);
            lbGeminiModel.TabIndex = 23;
            lbGeminiModel.Text = "Model";
            // 
            // tbGeminiApi
            // 
            tbGeminiApi.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            tbGeminiApi.Location = new System.Drawing.Point(98, 3);
            tbGeminiApi.Name = "tbGeminiApi";
            tbGeminiApi.Size = new System.Drawing.Size(354, 25);
            tbGeminiApi.TabIndex = 21;
            // 
            // lbGeminiApi
            // 
            lbGeminiApi.AutoSize = true;
            lbGeminiApi.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbGeminiApi.ForeColor = System.Drawing.Color.White;
            lbGeminiApi.Location = new System.Drawing.Point(3, 8);
            lbGeminiApi.Name = "lbGeminiApi";
            lbGeminiApi.Size = new System.Drawing.Size(52, 17);
            lbGeminiApi.TabIndex = 17;
            lbGeminiApi.Text = "API 키 ";
            // 
            // btnTransHelp
            // 
            btnTransHelp.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            btnTransHelp.FlatAppearance.BorderSize = 0;
            btnTransHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnTransHelp.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            btnTransHelp.ForeColor = System.Drawing.Color.White;
            btnTransHelp.Location = new System.Drawing.Point(320, 28);
            btnTransHelp.Margin = new System.Windows.Forms.Padding(0);
            btnTransHelp.Name = "btnTransHelp";
            btnTransHelp.Size = new System.Drawing.Size(28, 25);
            btnTransHelp.TabIndex = 58;
            btnTransHelp.Text = "?";
            btnTransHelp.UseVisualStyleBackColor = false;
            btnTransHelp.Click += OnClick_btnTransHelp;
            // 
            // cbPerWordDic
            // 
            cbPerWordDic.AutoSize = true;
            cbPerWordDic.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbPerWordDic.ForeColor = System.Drawing.Color.White;
            cbPerWordDic.Location = new System.Drawing.Point(11, 188);
            cbPerWordDic.Name = "cbPerWordDic";
            cbPerWordDic.Size = new System.Drawing.Size(301, 21);
            cbPerWordDic.TabIndex = 57;
            cbPerWordDic.Text = "단어 단위로 교정 (완벽히 일치한 단어만 교정)";
            cbPerWordDic.UseVisualStyleBackColor = true;
            // 
            // lbTransType
            // 
            lbTransType.AutoSize = true;
            lbTransType.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbTransType.ForeColor = System.Drawing.Color.White;
            lbTransType.Location = new System.Drawing.Point(9, 33);
            lbTransType.Name = "lbTransType";
            lbTransType.Size = new System.Drawing.Size(60, 17);
            lbTransType.TabIndex = 20;
            lbTransType.Text = "번역방법";
            // 
            // _cbTranslateType
            // 
            _cbTranslateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _cbTranslateType.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            _cbTranslateType.FormattingEnabled = true;
            _cbTranslateType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            _cbTranslateType.Items.AddRange(new object[] { "TRANSLATE GOOGLE", "TRANSLATE DB", "TRANSLATE PAPAGO WEB", "TRANSLATE NAVER", "TRANSLATE GOOGLE SHEET", "TRANSLATE DEEPL", "TRANSLATE DEEPLAPI", "TRANSLATE GEMINI API", "TRANSLATE EZTRANS", "TRANSLATE CUSTOM API" });
            _cbTranslateType.Location = new System.Drawing.Point(105, 30);
            _cbTranslateType.Name = "_cbTranslateType";
            _cbTranslateType.Size = new System.Drawing.Size(212, 25);
            _cbTranslateType.TabIndex = 49;
            _cbTranslateType.SelectedIndexChanged += CbTranslateTypeSelectedIndexChanged;
            // 
            // checkDic
            // 
            checkDic.AutoSize = true;
            checkDic.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            checkDic.ForeColor = System.Drawing.Color.White;
            checkDic.Location = new System.Drawing.Point(11, 161);
            checkDic.Name = "checkDic";
            checkDic.Size = new System.Drawing.Size(110, 21);
            checkDic.TabIndex = 24;
            checkDic.Text = "교정사전 사용";
            checkDic.UseVisualStyleBackColor = true;
            checkDic.CheckedChanged += checkDic_CheckedChanged;
            // 
            // dicFileTextBox
            // 
            dicFileTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            dicFileTextBox.Location = new System.Drawing.Point(225, 159);
            dicFileTextBox.Name = "dicFileTextBox";
            dicFileTextBox.Size = new System.Drawing.Size(252, 25);
            dicFileTextBox.TabIndex = 23;
            // 
            // lbDicFile
            // 
            lbDicFile.AutoSize = true;
            lbDicFile.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbDicFile.ForeColor = System.Drawing.Color.White;
            lbDicFile.Location = new System.Drawing.Point(139, 162);
            lbDicFile.Name = "lbDicFile";
            lbDicFile.Size = new System.Drawing.Size(60, 17);
            lbDicFile.TabIndex = 22;
            lbDicFile.Text = "파일이름";
            // 
            // lbTransTypeTitle
            // 
            lbTransTypeTitle.AutoSize = true;
            lbTransTypeTitle.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbTransTypeTitle.ForeColor = System.Drawing.Color.White;
            lbTransTypeTitle.Location = new System.Drawing.Point(4, 3);
            lbTransTypeTitle.Name = "lbTransTypeTitle";
            lbTransTypeTitle.Size = new System.Drawing.Size(74, 20);
            lbTransTypeTitle.TabIndex = 8;
            lbTransTypeTitle.Text = "번역 설정";
            // 
            // Naver_Panel
            // 
            Naver_Panel.Controls.Add(Button_NaverTransKeyList);
            Naver_Panel.Controls.Add(lbPapagoSecret);
            Naver_Panel.Controls.Add(NaverSecretKeyTextBox);
            Naver_Panel.Controls.Add(NaverIDKeyTextBox);
            Naver_Panel.Controls.Add(lbPapagoID);
            Naver_Panel.Location = new System.Drawing.Point(7, 61);
            Naver_Panel.Name = "Naver_Panel";
            Naver_Panel.Size = new System.Drawing.Size(483, 94);
            Naver_Panel.TabIndex = 52;
            // 
            // Button_NaverTransKeyList
            // 
            Button_NaverTransKeyList.AutoSize = true;
            Button_NaverTransKeyList.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            Button_NaverTransKeyList.FlatAppearance.BorderSize = 0;
            Button_NaverTransKeyList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Button_NaverTransKeyList.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            Button_NaverTransKeyList.ForeColor = System.Drawing.Color.White;
            Button_NaverTransKeyList.Location = new System.Drawing.Point(360, 6);
            Button_NaverTransKeyList.Margin = new System.Windows.Forms.Padding(0);
            Button_NaverTransKeyList.Name = "Button_NaverTransKeyList";
            Button_NaverTransKeyList.Size = new System.Drawing.Size(110, 84);
            Button_NaverTransKeyList.TabIndex = 52;
            Button_NaverTransKeyList.Text = "키 관리";
            Button_NaverTransKeyList.UseVisualStyleBackColor = false;
            Button_NaverTransKeyList.Click += Button_NaverTransKeyList_Click;
            // 
            // lbPapagoSecret
            // 
            lbPapagoSecret.AutoSize = true;
            lbPapagoSecret.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbPapagoSecret.ForeColor = System.Drawing.Color.White;
            lbPapagoSecret.Location = new System.Drawing.Point(3, 38);
            lbPapagoSecret.Name = "lbPapagoSecret";
            lbPapagoSecret.Size = new System.Drawing.Size(63, 17);
            lbPapagoSecret.TabIndex = 23;
            lbPapagoSecret.Text = "Secret 키";
            // 
            // NaverSecretKeyTextBox
            // 
            NaverSecretKeyTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            NaverSecretKeyTextBox.Location = new System.Drawing.Point(98, 34);
            NaverSecretKeyTextBox.Name = "NaverSecretKeyTextBox";
            NaverSecretKeyTextBox.Size = new System.Drawing.Size(252, 25);
            NaverSecretKeyTextBox.TabIndex = 22;
            // 
            // NaverIDKeyTextBox
            // 
            NaverIDKeyTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            NaverIDKeyTextBox.Location = new System.Drawing.Point(98, 3);
            NaverIDKeyTextBox.Name = "NaverIDKeyTextBox";
            NaverIDKeyTextBox.Size = new System.Drawing.Size(252, 25);
            NaverIDKeyTextBox.TabIndex = 21;
            // 
            // lbPapagoID
            // 
            lbPapagoID.AutoSize = true;
            lbPapagoID.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbPapagoID.ForeColor = System.Drawing.Color.White;
            lbPapagoID.Location = new System.Drawing.Point(3, 8);
            lbPapagoID.Name = "lbPapagoID";
            lbPapagoID.Size = new System.Drawing.Size(45, 17);
            lbPapagoID.TabIndex = 17;
            lbPapagoID.Text = "ID 키 ";
            // 
            // Google_Panel
            // 
            Google_Panel.Controls.Add(button_RemoveAllGoogleToekn);
            Google_Panel.Controls.Add(textBox_GoogleSecretKey);
            Google_Panel.Controls.Add(lbSheetSecret);
            Google_Panel.Controls.Add(textBox_GoogleClientID);
            Google_Panel.Controls.Add(lbSheetID);
            Google_Panel.Controls.Add(googleSheet_textBox);
            Google_Panel.Controls.Add(lbGoogleSheetAddress);
            Google_Panel.Location = new System.Drawing.Point(7, 61);
            Google_Panel.Name = "Google_Panel";
            Google_Panel.Size = new System.Drawing.Size(483, 92);
            Google_Panel.TabIndex = 53;
            // 
            // button_RemoveAllGoogleToekn
            // 
            button_RemoveAllGoogleToekn.AutoSize = true;
            button_RemoveAllGoogleToekn.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
            button_RemoveAllGoogleToekn.FlatAppearance.BorderSize = 0;
            button_RemoveAllGoogleToekn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button_RemoveAllGoogleToekn.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            button_RemoveAllGoogleToekn.ForeColor = System.Drawing.Color.White;
            button_RemoveAllGoogleToekn.Location = new System.Drawing.Point(360, 6);
            button_RemoveAllGoogleToekn.Margin = new System.Windows.Forms.Padding(0);
            button_RemoveAllGoogleToekn.Name = "button_RemoveAllGoogleToekn";
            button_RemoveAllGoogleToekn.Size = new System.Drawing.Size(128, 84);
            button_RemoveAllGoogleToekn.TabIndex = 45;
            button_RemoveAllGoogleToekn.Text = "모든 인증\r\n초기화";
            button_RemoveAllGoogleToekn.UseVisualStyleBackColor = false;
            button_RemoveAllGoogleToekn.Click += button_RemoveAllGoogleToekn_Click;
            // 
            // textBox_GoogleSecretKey
            // 
            textBox_GoogleSecretKey.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            textBox_GoogleSecretKey.Location = new System.Drawing.Point(98, 65);
            textBox_GoogleSecretKey.Name = "textBox_GoogleSecretKey";
            textBox_GoogleSecretKey.Size = new System.Drawing.Size(252, 25);
            textBox_GoogleSecretKey.TabIndex = 27;
            // 
            // lbSheetSecret
            // 
            lbSheetSecret.AutoSize = true;
            lbSheetSecret.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbSheetSecret.ForeColor = System.Drawing.Color.White;
            lbSheetSecret.Location = new System.Drawing.Point(3, 67);
            lbSheetSecret.Name = "lbSheetSecret";
            lbSheetSecret.Size = new System.Drawing.Size(63, 17);
            lbSheetSecret.TabIndex = 26;
            lbSheetSecret.Text = "Secret 키";
            // 
            // textBox_GoogleClientID
            // 
            textBox_GoogleClientID.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            textBox_GoogleClientID.Location = new System.Drawing.Point(98, 34);
            textBox_GoogleClientID.Name = "textBox_GoogleClientID";
            textBox_GoogleClientID.Size = new System.Drawing.Size(252, 25);
            textBox_GoogleClientID.TabIndex = 25;
            // 
            // lbSheetID
            // 
            lbSheetID.AutoSize = true;
            lbSheetID.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbSheetID.ForeColor = System.Drawing.Color.White;
            lbSheetID.Location = new System.Drawing.Point(3, 38);
            lbSheetID.Name = "lbSheetID";
            lbSheetID.Size = new System.Drawing.Size(63, 17);
            lbSheetID.TabIndex = 24;
            lbSheetID.Text = "Client ID";
            // 
            // googleSheet_textBox
            // 
            googleSheet_textBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            googleSheet_textBox.Location = new System.Drawing.Point(98, 3);
            googleSheet_textBox.Name = "googleSheet_textBox";
            googleSheet_textBox.Size = new System.Drawing.Size(252, 25);
            googleSheet_textBox.TabIndex = 21;
            // 
            // lbGoogleSheetAddress
            // 
            lbGoogleSheetAddress.AutoSize = true;
            lbGoogleSheetAddress.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbGoogleSheetAddress.ForeColor = System.Drawing.Color.White;
            lbGoogleSheetAddress.Location = new System.Drawing.Point(3, 8);
            lbGoogleSheetAddress.Name = "lbGoogleSheetAddress";
            lbGoogleSheetAddress.Size = new System.Drawing.Size(65, 17);
            lbGoogleSheetAddress.TabIndex = 17;
            lbGoogleSheetAddress.Text = "시트 주소";
            // 
            // pnEzTrans
            // 
            pnEzTrans.Controls.Add(lbEzTransInfo);
            pnEzTrans.Location = new System.Drawing.Point(7, 61);
            pnEzTrans.Name = "pnEzTrans";
            pnEzTrans.Size = new System.Drawing.Size(483, 94);
            pnEzTrans.TabIndex = 54;
            // 
            // lbEzTransInfo
            // 
            lbEzTransInfo.AutoSize = true;
            lbEzTransInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbEzTransInfo.ForeColor = System.Drawing.Color.White;
            lbEzTransInfo.Location = new System.Drawing.Point(79, 21);
            lbEzTransInfo.Name = "lbEzTransInfo";
            lbEzTransInfo.Size = new System.Drawing.Size(314, 51);
            lbEzTransInfo.TabIndex = 17;
            lbEzTransInfo.Text = "일본어 전용\r\nezTrans XP가 설치 되어 있어야 합니다.\r\n자세한 사용법은 번역 설정 도움말을 확인해 주세요.";
            lbEzTransInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnPapagoWeb
            // 
            pnPapagoWeb.Controls.Add(lbPapagoWebInfo);
            pnPapagoWeb.Location = new System.Drawing.Point(7, 61);
            pnPapagoWeb.Name = "pnPapagoWeb";
            pnPapagoWeb.Size = new System.Drawing.Size(503, 94);
            pnPapagoWeb.TabIndex = 59;
            // 
            // lbPapagoWebInfo
            // 
            lbPapagoWebInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbPapagoWebInfo.ForeColor = System.Drawing.Color.White;
            lbPapagoWebInfo.Location = new System.Drawing.Point(3, 21);
            lbPapagoWebInfo.Name = "lbPapagoWebInfo";
            lbPapagoWebInfo.Size = new System.Drawing.Size(489, 34);
            lbPapagoWebInfo.TabIndex = 17;
            lbPapagoWebInfo.Text = "파파고 웹 번역기 입니다.\r\n스냅샷 위주로 사용해 주세요.";
            lbPapagoWebInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnGoogleBasic
            // 
            pnGoogleBasic.Controls.Add(lbBasicInfo);
            pnGoogleBasic.Controls.Add(lbBasicStatus);
            pnGoogleBasic.Location = new System.Drawing.Point(7, 61);
            pnGoogleBasic.Name = "pnGoogleBasic";
            pnGoogleBasic.Size = new System.Drawing.Size(503, 94);
            pnGoogleBasic.TabIndex = 53;
            // 
            // lbBasicInfo
            // 
            lbBasicInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbBasicInfo.ForeColor = System.Drawing.Color.White;
            lbBasicInfo.Location = new System.Drawing.Point(3, 21);
            lbBasicInfo.Name = "lbBasicInfo";
            lbBasicInfo.Size = new System.Drawing.Size(489, 34);
            lbBasicInfo.TabIndex = 17;
            lbBasicInfo.Text = "구글 기본 번역기의 고품질 번역은 시간당 100회까지만 적용되며\r\n초과시 낮은 품질로 번역됩니다.";
            lbBasicInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbBasicStatus
            // 
            lbBasicStatus.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbBasicStatus.ForeColor = System.Drawing.Color.White;
            lbBasicStatus.Location = new System.Drawing.Point(3, 60);
            lbBasicStatus.Name = "lbBasicStatus";
            lbBasicStatus.Size = new System.Drawing.Size(335, 18);
            lbBasicStatus.TabIndex = 18;
            lbBasicStatus.Text = "상태 : 고품질";
            lbBasicStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnCustomApi
            // 
            pnCustomApi.Controls.Add(lbCustomApiInformation);
            pnCustomApi.Location = new System.Drawing.Point(7, 61);
            pnCustomApi.Name = "pnCustomApi";
            pnCustomApi.Size = new System.Drawing.Size(483, 94);
            pnCustomApi.TabIndex = 55;
            // 
            // lbCustomApiInformation
            // 
            lbCustomApiInformation.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbCustomApiInformation.ForeColor = System.Drawing.Color.White;
            lbCustomApiInformation.Location = new System.Drawing.Point(3, 8);
            lbCustomApiInformation.Name = "lbCustomApiInformation";
            lbCustomApiInformation.Size = new System.Drawing.Size(469, 34);
            lbCustomApiInformation.TabIndex = 17;
            lbCustomApiInformation.Text = "커스텀 API는 고급 설정에서 설정하시면 됩니다";
            lbCustomApiInformation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnDeepLAPI
            // 
            pnDeepLAPI.Controls.Add(tbDeeplApi);
            pnDeepLAPI.Controls.Add(lbDeeplApi);
            pnDeepLAPI.Controls.Add(rbDeepLAPIEndpointFree);
            pnDeepLAPI.Controls.Add(rbDeepLAPIEndpointPaid);
            pnDeepLAPI.Location = new System.Drawing.Point(7, 61);
            pnDeepLAPI.Name = "pnDeepLAPI";
            pnDeepLAPI.Size = new System.Drawing.Size(483, 94);
            pnDeepLAPI.TabIndex = 55;
            // 
            // tbDeeplApi
            // 
            tbDeeplApi.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            tbDeeplApi.Location = new System.Drawing.Point(98, 3);
            tbDeeplApi.Name = "tbDeeplApi";
            tbDeeplApi.Size = new System.Drawing.Size(252, 25);
            tbDeeplApi.TabIndex = 23;
            // 
            // lbDeeplApi
            // 
            lbDeeplApi.AutoSize = true;
            lbDeeplApi.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbDeeplApi.ForeColor = System.Drawing.Color.White;
            lbDeeplApi.Location = new System.Drawing.Point(3, 8);
            lbDeeplApi.Name = "lbDeeplApi";
            lbDeeplApi.Size = new System.Drawing.Size(60, 17);
            lbDeeplApi.TabIndex = 22;
            lbDeeplApi.Text = "API 키 : ";
            // 
            // rbDeepLAPIEndpointFree
            // 
            rbDeepLAPIEndpointFree.AutoSize = true;
            rbDeepLAPIEndpointFree.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            rbDeepLAPIEndpointFree.ForeColor = System.Drawing.Color.White;
            rbDeepLAPIEndpointFree.Location = new System.Drawing.Point(3, 40);
            rbDeepLAPIEndpointFree.Name = "rbDeepLAPIEndpointFree";
            rbDeepLAPIEndpointFree.Size = new System.Drawing.Size(113, 21);
            rbDeepLAPIEndpointFree.TabIndex = 3;
            rbDeepLAPIEndpointFree.Text = "Free Endpoint";
            rbDeepLAPIEndpointFree.UseVisualStyleBackColor = true;
            rbDeepLAPIEndpointFree.CheckedChanged += RbDeepLAPIEndpoint_CheckedChanged;
            // 
            // rbDeepLAPIEndpointPaid
            // 
            rbDeepLAPIEndpointPaid.AutoSize = true;
            rbDeepLAPIEndpointPaid.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            rbDeepLAPIEndpointPaid.ForeColor = System.Drawing.Color.White;
            rbDeepLAPIEndpointPaid.Location = new System.Drawing.Point(3, 68);
            rbDeepLAPIEndpointPaid.Name = "rbDeepLAPIEndpointPaid";
            rbDeepLAPIEndpointPaid.Size = new System.Drawing.Size(114, 21);
            rbDeepLAPIEndpointPaid.TabIndex = 3;
            rbDeepLAPIEndpointPaid.Text = "Paid Endpoint";
            rbDeepLAPIEndpointPaid.UseVisualStyleBackColor = true;
            rbDeepLAPIEndpointPaid.CheckedChanged += RbDeepLAPIEndpoint_CheckedChanged;
            // 
            // pnDeepl
            // 
            pnDeepl.Controls.Add(btnCheckDeeplState);
            pnDeepl.Controls.Add(lbDeepLStatus);
            pnDeepl.Controls.Add(lbDeepLInfo);
            pnDeepl.Location = new System.Drawing.Point(7, 61);
            pnDeepl.Name = "pnDeepl";
            pnDeepl.Size = new System.Drawing.Size(483, 94);
            pnDeepl.TabIndex = 54;
            // 
            // btnCheckDeeplState
            // 
            btnCheckDeeplState.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnCheckDeeplState.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnCheckDeeplState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCheckDeeplState.ForeColor = System.Drawing.Color.White;
            btnCheckDeeplState.Location = new System.Drawing.Point(322, 53);
            btnCheckDeeplState.Name = "btnCheckDeeplState";
            btnCheckDeeplState.Size = new System.Drawing.Size(150, 25);
            btnCheckDeeplState.TabIndex = 52;
            btnCheckDeeplState.Text = "상태 확인하기";
            btnCheckDeeplState.UseVisualStyleBackColor = false;
            btnCheckDeeplState.Click += OnClickCheckDeeplState;
            // 
            // lbDeepLStatus
            // 
            lbDeepLStatus.AutoSize = true;
            lbDeepLStatus.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbDeepLStatus.ForeColor = System.Drawing.Color.White;
            lbDeepLStatus.Location = new System.Drawing.Point(3, 62);
            lbDeepLStatus.Name = "lbDeepLStatus";
            lbDeepLStatus.Size = new System.Drawing.Size(86, 17);
            lbDeepLStatus.TabIndex = 18;
            lbDeepLStatus.Text = "상태 : 고품질";
            lbDeepLStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDeepLInfo
            // 
            lbDeepLInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbDeepLInfo.ForeColor = System.Drawing.Color.White;
            lbDeepLInfo.Location = new System.Drawing.Point(3, 8);
            lbDeepLInfo.Name = "lbDeepLInfo";
            lbDeepLInfo.Size = new System.Drawing.Size(469, 34);
            lbDeepLInfo.TabIndex = 17;
            lbDeepLInfo.Text = "사용을 위해 마이크로소프트 엣지가 필요합니다";
            lbDeepLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DB_Panel
            // 
            DB_Panel.Controls.Add(cbDBMultiGet);
            DB_Panel.Controls.Add(checkStringUpper);
            DB_Panel.Controls.Add(dbFileTextBox);
            DB_Panel.Controls.Add(lbDbFile);
            DB_Panel.Location = new System.Drawing.Point(7, 61);
            DB_Panel.Name = "DB_Panel";
            DB_Panel.Size = new System.Drawing.Size(452, 94);
            DB_Panel.TabIndex = 50;
            // 
            // cbDBMultiGet
            // 
            cbDBMultiGet.AutoSize = true;
            cbDBMultiGet.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbDBMultiGet.ForeColor = System.Drawing.Color.White;
            cbDBMultiGet.Location = new System.Drawing.Point(5, 65);
            cbDBMultiGet.Name = "cbDBMultiGet";
            cbDBMultiGet.Size = new System.Drawing.Size(399, 21);
            cbDBMultiGet.TabIndex = 26;
            cbDBMultiGet.Text = "DB 부분 일치 검색 - 문장과 부분 일치한 번역문 모두 가져오기";
            cbDBMultiGet.UseVisualStyleBackColor = true;
            // 
            // checkStringUpper
            // 
            checkStringUpper.AutoSize = true;
            checkStringUpper.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            checkStringUpper.ForeColor = System.Drawing.Color.White;
            checkStringUpper.Location = new System.Drawing.Point(5, 40);
            checkStringUpper.Name = "checkStringUpper";
            checkStringUpper.Size = new System.Drawing.Size(218, 21);
            checkStringUpper.TabIndex = 25;
            checkStringUpper.Text = "DB 검색 시 대소문자 구분 안 함";
            checkStringUpper.UseVisualStyleBackColor = true;
            // 
            // dbFileTextBox
            // 
            dbFileTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            dbFileTextBox.Location = new System.Drawing.Point(98, 3);
            dbFileTextBox.Name = "dbFileTextBox";
            dbFileTextBox.Size = new System.Drawing.Size(252, 25);
            dbFileTextBox.TabIndex = 19;
            dbFileTextBox.Text = "empty.txt";
            // 
            // lbDbFile
            // 
            lbDbFile.AutoSize = true;
            lbDbFile.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbDbFile.ForeColor = System.Drawing.Color.White;
            lbDbFile.Location = new System.Drawing.Point(3, 8);
            lbDbFile.Name = "lbDbFile";
            lbDbFile.Size = new System.Drawing.Size(70, 17);
            lbDbFile.TabIndex = 16;
            lbDbFile.Text = "파일이름  ";
            // 
            // tpText
            // 
            tpText.Controls.Add(panel5);
            tpText.Location = new System.Drawing.Point(80, 4);
            tpText.Margin = new System.Windows.Forms.Padding(0);
            tpText.Name = "tpText";
            tpText.Size = new System.Drawing.Size(540, 585);
            tpText.TabIndex = 1;
            tpText.Text = "텍스트";
            tpText.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            panel5.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel5.Controls.Add(panel17);
            panel5.Controls.Add(panel10);
            panel5.Controls.Add(panel9);
            panel5.Controls.Add(panel7);
            panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            panel5.Location = new System.Drawing.Point(0, 0);
            panel5.Margin = new System.Windows.Forms.Padding(0);
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size(540, 585);
            panel5.TabIndex = 0;
            // 
            // panel17
            // 
            panel17.Controls.Add(lbPreview);
            panel17.Controls.Add(fontResultLabel);
            panel17.Location = new System.Drawing.Point(3, 281);
            panel17.Name = "panel17";
            panel17.Size = new System.Drawing.Size(533, 287);
            panel17.TabIndex = 40;
            panel17.Paint += panealBorder_Paint;
            // 
            // lbPreview
            // 
            lbPreview.AutoSize = true;
            lbPreview.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbPreview.ForeColor = System.Drawing.Color.White;
            lbPreview.Location = new System.Drawing.Point(4, 3);
            lbPreview.Name = "lbPreview";
            lbPreview.Size = new System.Drawing.Size(69, 20);
            lbPreview.TabIndex = 8;
            lbPreview.Text = "미리보기";
            // 
            // fontResultLabel
            // 
            fontResultLabel.Font = new System.Drawing.Font("맑은 고딕", 15.75F);
            fontResultLabel.Location = new System.Drawing.Point(12, 34);
            fontResultLabel.Name = "fontResultLabel";
            fontResultLabel.Size = new System.Drawing.Size(511, 249);
            fontResultLabel.TabIndex = 39;
            fontResultLabel.Text = "-설정 결과를 미리 봅니다.\r\n-어두운 번역창에는 적용되지 않습니다.\r\n\r\n-1 2 3 4 5 6\r\n-Tank division!";
            // 
            // panel10
            // 
            panel10.Controls.Add(defaultColorButton);
            panel10.Controls.Add(lbFontBackground);
            panel10.Controls.Add(lbFontOutlineColor2);
            panel10.Controls.Add(lbFontOutlineColor1);
            panel10.Controls.Add(backgroundColorBox);
            panel10.Controls.Add(outlineColor2Box);
            panel10.Controls.Add(outlineColor1Box);
            panel10.Controls.Add(lbFontBasicColor);
            panel10.Controls.Add(textColorBox);
            panel10.Controls.Add(lbFontColor);
            panel10.Location = new System.Drawing.Point(3, 78);
            panel10.Name = "panel10";
            panel10.Size = new System.Drawing.Size(533, 107);
            panel10.TabIndex = 38;
            panel10.Paint += panealBorder_Paint;
            // 
            // defaultColorButton
            // 
            defaultColorButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            defaultColorButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            defaultColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            defaultColorButton.ForeColor = System.Drawing.Color.White;
            defaultColorButton.Location = new System.Drawing.Point(9, 79);
            defaultColorButton.Name = "defaultColorButton";
            defaultColorButton.Size = new System.Drawing.Size(516, 25);
            defaultColorButton.TabIndex = 25;
            defaultColorButton.Text = "기본 색으로";
            defaultColorButton.UseVisualStyleBackColor = false;
            defaultColorButton.Click += defaultColorButton_Click;
            // 
            // lbFontBackground
            // 
            lbFontBackground.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbFontBackground.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbFontBackground.ForeColor = System.Drawing.Color.White;
            lbFontBackground.Location = new System.Drawing.Point(354, 48);
            lbFontBackground.Name = "lbFontBackground";
            lbFontBackground.Size = new System.Drawing.Size(90, 20);
            lbFontBackground.TabIndex = 30;
            lbFontBackground.Text = "배경색";
            lbFontBackground.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbFontOutlineColor2
            // 
            lbFontOutlineColor2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbFontOutlineColor2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbFontOutlineColor2.ForeColor = System.Drawing.Color.White;
            lbFontOutlineColor2.Location = new System.Drawing.Point(264, 48);
            lbFontOutlineColor2.Name = "lbFontOutlineColor2";
            lbFontOutlineColor2.Size = new System.Drawing.Size(90, 20);
            lbFontOutlineColor2.TabIndex = 29;
            lbFontOutlineColor2.Text = "외곽선2";
            lbFontOutlineColor2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbFontOutlineColor1
            // 
            lbFontOutlineColor1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbFontOutlineColor1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbFontOutlineColor1.ForeColor = System.Drawing.Color.White;
            lbFontOutlineColor1.Location = new System.Drawing.Point(174, 48);
            lbFontOutlineColor1.Name = "lbFontOutlineColor1";
            lbFontOutlineColor1.Size = new System.Drawing.Size(90, 20);
            lbFontOutlineColor1.TabIndex = 28;
            lbFontOutlineColor1.Text = "외곽선1";
            lbFontOutlineColor1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // backgroundColorBox
            // 
            backgroundColorBox.BackColor = System.Drawing.Color.FromArgb(255, 255, 128);
            backgroundColorBox.Location = new System.Drawing.Point(374, 24);
            backgroundColorBox.Name = "backgroundColorBox";
            backgroundColorBox.Size = new System.Drawing.Size(24, 24);
            backgroundColorBox.TabIndex = 27;
            backgroundColorBox.TabStop = false;
            backgroundColorBox.Click += backgroundColorBox_Click;
            // 
            // outlineColor2Box
            // 
            outlineColor2Box.BackColor = System.Drawing.Color.FromArgb(255, 255, 128);
            outlineColor2Box.Location = new System.Drawing.Point(287, 24);
            outlineColor2Box.Name = "outlineColor2Box";
            outlineColor2Box.Size = new System.Drawing.Size(24, 24);
            outlineColor2Box.TabIndex = 26;
            outlineColor2Box.TabStop = false;
            outlineColor2Box.Click += outlineColor2Box_Click;
            // 
            // outlineColor1Box
            // 
            outlineColor1Box.BackColor = System.Drawing.Color.FromArgb(255, 255, 128);
            outlineColor1Box.Location = new System.Drawing.Point(197, 24);
            outlineColor1Box.Name = "outlineColor1Box";
            outlineColor1Box.Size = new System.Drawing.Size(24, 24);
            outlineColor1Box.TabIndex = 25;
            outlineColor1Box.TabStop = false;
            outlineColor1Box.Click += outlineColor1Box_Click;
            // 
            // lbFontBasicColor
            // 
            lbFontBasicColor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbFontBasicColor.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbFontBasicColor.ForeColor = System.Drawing.Color.White;
            lbFontBasicColor.Location = new System.Drawing.Point(84, 48);
            lbFontBasicColor.Name = "lbFontBasicColor";
            lbFontBasicColor.Size = new System.Drawing.Size(90, 20);
            lbFontBasicColor.TabIndex = 24;
            lbFontBasicColor.Text = "색상";
            lbFontBasicColor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textColorBox
            // 
            textColorBox.BackColor = System.Drawing.Color.FromArgb(255, 255, 128);
            textColorBox.Location = new System.Drawing.Point(107, 24);
            textColorBox.Name = "textColorBox";
            textColorBox.Size = new System.Drawing.Size(24, 24);
            textColorBox.TabIndex = 24;
            textColorBox.TabStop = false;
            textColorBox.Click += textColorBox_Click;
            // 
            // lbFontColor
            // 
            lbFontColor.AutoSize = true;
            lbFontColor.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbFontColor.ForeColor = System.Drawing.Color.White;
            lbFontColor.Location = new System.Drawing.Point(4, 3);
            lbFontColor.Name = "lbFontColor";
            lbFontColor.Size = new System.Drawing.Size(24, 20);
            lbFontColor.TabIndex = 8;
            lbFontColor.Text = "색";
            // 
            // panel9
            // 
            panel9.Controls.Add(cbShowOCRIndex);
            panel9.Controls.Add(useBackColorCheckBox);
            panel9.Controls.Add(removeSpaceCheckBox);
            panel9.Controls.Add(alignmentCenterCheckBox);
            panel9.Controls.Add(lbTextAdditionalSettings);
            panel9.Location = new System.Drawing.Point(3, 191);
            panel9.Name = "panel9";
            panel9.Size = new System.Drawing.Size(533, 84);
            panel9.TabIndex = 38;
            panel9.Paint += panealBorder_Paint;
            // 
            // cbShowOCRIndex
            // 
            cbShowOCRIndex.AutoSize = true;
            cbShowOCRIndex.Checked = true;
            cbShowOCRIndex.CheckState = System.Windows.Forms.CheckState.Checked;
            cbShowOCRIndex.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            cbShowOCRIndex.ForeColor = System.Drawing.Color.White;
            cbShowOCRIndex.Location = new System.Drawing.Point(17, 53);
            cbShowOCRIndex.Name = "cbShowOCRIndex";
            cbShowOCRIndex.Size = new System.Drawing.Size(135, 19);
            cbShowOCRIndex.TabIndex = 12;
            cbShowOCRIndex.Text = "OCR 영역 번호 표시";
            cbShowOCRIndex.UseVisualStyleBackColor = true;
            cbShowOCRIndex.CheckedChanged += cbShowOCRIndex_CheckedChanged;
            // 
            // useBackColorCheckBox
            // 
            useBackColorCheckBox.AutoSize = true;
            useBackColorCheckBox.Checked = true;
            useBackColorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            useBackColorCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            useBackColorCheckBox.ForeColor = System.Drawing.Color.White;
            useBackColorCheckBox.Location = new System.Drawing.Point(405, 26);
            useBackColorCheckBox.Name = "useBackColorCheckBox";
            useBackColorCheckBox.Size = new System.Drawing.Size(90, 19);
            useBackColorCheckBox.TabIndex = 11;
            useBackColorCheckBox.Text = "배경색 사용";
            useBackColorCheckBox.UseVisualStyleBackColor = true;
            useBackColorCheckBox.CheckedChanged += useBackColorCheckBox_CheckedChanged;
            // 
            // removeSpaceCheckBox
            // 
            removeSpaceCheckBox.AutoSize = true;
            removeSpaceCheckBox.Checked = true;
            removeSpaceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            removeSpaceCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            removeSpaceCheckBox.ForeColor = System.Drawing.Color.White;
            removeSpaceCheckBox.Location = new System.Drawing.Point(186, 26);
            removeSpaceCheckBox.Name = "removeSpaceCheckBox";
            removeSpaceCheckBox.Size = new System.Drawing.Size(135, 19);
            removeSpaceCheckBox.TabIndex = 10;
            removeSpaceCheckBox.Text = "OCR 결과 공백 제거";
            removeSpaceCheckBox.UseVisualStyleBackColor = true;
            removeSpaceCheckBox.CheckedChanged += removeSpaceCheckBox_CheckedChanged;
            // 
            // alignmentCenterCheckBox
            // 
            alignmentCenterCheckBox.AutoSize = true;
            alignmentCenterCheckBox.Checked = true;
            alignmentCenterCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            alignmentCenterCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            alignmentCenterCheckBox.ForeColor = System.Drawing.Color.White;
            alignmentCenterCheckBox.Location = new System.Drawing.Point(17, 26);
            alignmentCenterCheckBox.Name = "alignmentCenterCheckBox";
            alignmentCenterCheckBox.Size = new System.Drawing.Size(90, 19);
            alignmentCenterCheckBox.TabIndex = 9;
            alignmentCenterCheckBox.Text = "가운데 정렬";
            alignmentCenterCheckBox.UseVisualStyleBackColor = true;
            alignmentCenterCheckBox.CheckedChanged += alignmentCenterCheckBox_CheckedChanged;
            // 
            // lbTextAdditionalSettings
            // 
            lbTextAdditionalSettings.AutoSize = true;
            lbTextAdditionalSettings.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbTextAdditionalSettings.ForeColor = System.Drawing.Color.White;
            lbTextAdditionalSettings.Location = new System.Drawing.Point(4, 3);
            lbTextAdditionalSettings.Name = "lbTextAdditionalSettings";
            lbTextAdditionalSettings.Size = new System.Drawing.Size(69, 20);
            lbTextAdditionalSettings.TabIndex = 8;
            lbTextAdditionalSettings.Text = "부가설정";
            // 
            // panel7
            // 
            panel7.Controls.Add(fontSizeUpDown);
            panel7.Controls.Add(lbFontSize);
            panel7.Controls.Add(lbFont);
            panel7.Controls.Add(fontButton);
            panel7.Controls.Add(lbFontSetting);
            panel7.Location = new System.Drawing.Point(3, 3);
            panel7.Name = "panel7";
            panel7.Size = new System.Drawing.Size(533, 69);
            panel7.TabIndex = 37;
            panel7.Paint += panealBorder_Paint;
            // 
            // fontSizeUpDown
            // 
            fontSizeUpDown.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            fontSizeUpDown.ForeColor = System.Drawing.Color.White;
            fontSizeUpDown.Location = new System.Drawing.Point(318, 36);
            fontSizeUpDown.Minimum = new decimal(new int[] { 8, 0, 0, 0 });
            fontSizeUpDown.Name = "fontSizeUpDown";
            fontSizeUpDown.Size = new System.Drawing.Size(47, 23);
            fontSizeUpDown.TabIndex = 24;
            fontSizeUpDown.Value = new decimal(new int[] { 8, 0, 0, 0 });
            fontSizeUpDown.ValueChanged += fontSizeUpDown_ValueChanged;
            // 
            // lbFontSize
            // 
            lbFontSize.AutoSize = true;
            lbFontSize.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbFontSize.ForeColor = System.Drawing.Color.White;
            lbFontSize.Location = new System.Drawing.Point(255, 36);
            lbFontSize.Name = "lbFontSize";
            lbFontSize.Size = new System.Drawing.Size(42, 17);
            lbFontSize.TabIndex = 22;
            lbFontSize.Text = "크기 :";
            // 
            // lbFont
            // 
            lbFont.AutoSize = true;
            lbFont.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbFont.ForeColor = System.Drawing.Color.White;
            lbFont.Location = new System.Drawing.Point(13, 36);
            lbFont.Name = "lbFont";
            lbFont.Size = new System.Drawing.Size(47, 17);
            lbFont.TabIndex = 20;
            lbFont.Text = "글꼴 : ";
            // 
            // fontButton
            // 
            fontButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            fontButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            fontButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            fontButton.ForeColor = System.Drawing.Color.White;
            fontButton.Location = new System.Drawing.Point(66, 33);
            fontButton.Name = "fontButton";
            fontButton.Size = new System.Drawing.Size(152, 25);
            fontButton.TabIndex = 9;
            fontButton.Text = "폰트설정";
            fontButton.UseVisualStyleBackColor = false;
            fontButton.Click += fontButton_Click;
            // 
            // lbFontSetting
            // 
            lbFontSetting.AutoSize = true;
            lbFontSetting.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbFontSetting.ForeColor = System.Drawing.Color.White;
            lbFontSetting.Location = new System.Drawing.Point(4, 3);
            lbFontSetting.Name = "lbFontSetting";
            lbFontSetting.Size = new System.Drawing.Size(69, 20);
            lbFontSetting.TabIndex = 8;
            lbFontSetting.Text = "폰트설정";
            // 
            // tpExtra
            // 
            tpExtra.Controls.Add(panel11);
            tpExtra.Location = new System.Drawing.Point(80, 4);
            tpExtra.Margin = new System.Windows.Forms.Padding(0);
            tpExtra.Name = "tpExtra";
            tpExtra.Size = new System.Drawing.Size(540, 585);
            tpExtra.TabIndex = 2;
            tpExtra.Text = "부가설정";
            tpExtra.UseVisualStyleBackColor = true;
            // 
            // panel11
            // 
            panel11.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel11.Controls.Add(panel21);
            panel11.Controls.Add(panel25);
            panel11.Controls.Add(panel3);
            panel11.Controls.Add(panel13);
            panel11.Controls.Add(panel12);
            panel11.Controls.Add(panel14);
            panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            panel11.Location = new System.Drawing.Point(0, 0);
            panel11.Margin = new System.Windows.Forms.Padding(0);
            panel11.Name = "panel11";
            panel11.Size = new System.Drawing.Size(540, 585);
            panel11.TabIndex = 1;
            // 
            // panel21
            // 
            panel21.Controls.Add(btAdvencedOption);
            panel21.Controls.Add(lbAdvencedConfig);
            panel21.Location = new System.Drawing.Point(4, 500);
            panel21.Name = "panel21";
            panel21.Size = new System.Drawing.Size(533, 69);
            panel21.TabIndex = 41;
            panel21.Paint += panealBorder_Paint;
            // 
            // btAdvencedOption
            // 
            btAdvencedOption.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btAdvencedOption.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btAdvencedOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btAdvencedOption.ForeColor = System.Drawing.Color.White;
            btAdvencedOption.Location = new System.Drawing.Point(8, 35);
            btAdvencedOption.Name = "btAdvencedOption";
            btAdvencedOption.Size = new System.Drawing.Size(516, 25);
            btAdvencedOption.TabIndex = 25;
            btAdvencedOption.Text = "고급 설정";
            btAdvencedOption.UseVisualStyleBackColor = false;
            btAdvencedOption.Click += OnClick_btAdvencedOption;
            // 
            // lbAdvencedConfig
            // 
            lbAdvencedConfig.AutoSize = true;
            lbAdvencedConfig.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbAdvencedConfig.ForeColor = System.Drawing.Color.White;
            lbAdvencedConfig.Location = new System.Drawing.Point(4, 3);
            lbAdvencedConfig.Name = "lbAdvencedConfig";
            lbAdvencedConfig.Size = new System.Drawing.Size(74, 20);
            lbAdvencedConfig.TabIndex = 8;
            lbAdvencedConfig.Text = "고급 설정";
            // 
            // panel25
            // 
            panel25.Controls.Add(btSettingUpload);
            panel25.Controls.Add(btSettingBrowser);
            panel25.Controls.Add(lbSearchConfig);
            panel25.Location = new System.Drawing.Point(4, 392);
            panel25.Name = "panel25";
            panel25.Size = new System.Drawing.Size(533, 102);
            panel25.TabIndex = 40;
            panel25.Paint += panealBorder_Paint;
            // 
            // btSettingUpload
            // 
            btSettingUpload.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btSettingUpload.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btSettingUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btSettingUpload.ForeColor = System.Drawing.Color.White;
            btSettingUpload.Location = new System.Drawing.Point(8, 65);
            btSettingUpload.Name = "btSettingUpload";
            btSettingUpload.Size = new System.Drawing.Size(516, 25);
            btSettingUpload.TabIndex = 26;
            btSettingUpload.Text = "설정 업로드";
            btSettingUpload.UseVisualStyleBackColor = false;
            btSettingUpload.Click += OnClick_btSettingUpload;
            // 
            // btSettingBrowser
            // 
            btSettingBrowser.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btSettingBrowser.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btSettingBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btSettingBrowser.ForeColor = System.Drawing.Color.White;
            btSettingBrowser.Location = new System.Drawing.Point(8, 35);
            btSettingBrowser.Name = "btSettingBrowser";
            btSettingBrowser.Size = new System.Drawing.Size(516, 25);
            btSettingBrowser.TabIndex = 25;
            btSettingBrowser.Text = "설정 검색";
            btSettingBrowser.UseVisualStyleBackColor = false;
            btSettingBrowser.Click += Onclick_btSettingBrowser;
            // 
            // lbSearchConfig
            // 
            lbSearchConfig.AutoSize = true;
            lbSearchConfig.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbSearchConfig.ForeColor = System.Drawing.Color.White;
            lbSearchConfig.Location = new System.Drawing.Point(4, 3);
            lbSearchConfig.Name = "lbSearchConfig";
            lbSearchConfig.Size = new System.Drawing.Size(74, 20);
            lbSearchConfig.TabIndex = 8;
            lbSearchConfig.Text = "설정 검색";
            // 
            // panel3
            // 
            panel3.Controls.Add(speedRadioButton5);
            panel3.Controls.Add(lbSpeed);
            panel3.Controls.Add(lbSpeedInformation);
            panel3.Controls.Add(speedRadioButton4);
            panel3.Controls.Add(speedRadioButton1);
            panel3.Controls.Add(speedRadioButton3);
            panel3.Controls.Add(speedRadioButton2);
            panel3.Location = new System.Drawing.Point(3, 171);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(533, 79);
            panel3.TabIndex = 42;
            panel3.Paint += panealBorder_Paint;
            // 
            // speedRadioButton5
            // 
            speedRadioButton5.AutoSize = true;
            speedRadioButton5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            speedRadioButton5.ForeColor = System.Drawing.Color.White;
            speedRadioButton5.Location = new System.Drawing.Point(406, 27);
            speedRadioButton5.Name = "speedRadioButton5";
            speedRadioButton5.Size = new System.Drawing.Size(83, 21);
            speedRadioButton5.TabIndex = 9;
            speedRadioButton5.Text = "매우 느림";
            speedRadioButton5.UseVisualStyleBackColor = true;
            // 
            // lbSpeed
            // 
            lbSpeed.AutoSize = true;
            lbSpeed.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbSpeed.ForeColor = System.Drawing.Color.White;
            lbSpeed.Location = new System.Drawing.Point(4, 3);
            lbSpeed.Name = "lbSpeed";
            lbSpeed.Size = new System.Drawing.Size(69, 20);
            lbSpeed.TabIndex = 8;
            lbSpeed.Text = "처리속도";
            // 
            // lbSpeedInformation
            // 
            lbSpeedInformation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            lbSpeedInformation.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbSpeedInformation.ForeColor = System.Drawing.Color.White;
            lbSpeedInformation.Location = new System.Drawing.Point(33, 50);
            lbSpeedInformation.Name = "lbSpeedInformation";
            lbSpeedInformation.Size = new System.Drawing.Size(466, 26);
            lbSpeedInformation.TabIndex = 4;
            lbSpeedInformation.Text = "주의 : 빠름 이상으로 설정할 경우 게임이 느려질 수 있습니다.\r\n";
            lbSpeedInformation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // speedRadioButton4
            // 
            speedRadioButton4.AutoSize = true;
            speedRadioButton4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            speedRadioButton4.ForeColor = System.Drawing.Color.White;
            speedRadioButton4.Location = new System.Drawing.Point(318, 27);
            speedRadioButton4.Name = "speedRadioButton4";
            speedRadioButton4.Size = new System.Drawing.Size(52, 21);
            speedRadioButton4.TabIndex = 3;
            speedRadioButton4.Text = "느림";
            speedRadioButton4.UseVisualStyleBackColor = true;
            // 
            // speedRadioButton1
            // 
            speedRadioButton1.AutoSize = true;
            speedRadioButton1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            speedRadioButton1.ForeColor = System.Drawing.Color.White;
            speedRadioButton1.Location = new System.Drawing.Point(23, 27);
            speedRadioButton1.Name = "speedRadioButton1";
            speedRadioButton1.Size = new System.Drawing.Size(83, 21);
            speedRadioButton1.TabIndex = 0;
            speedRadioButton1.Text = "매우 빠름";
            speedRadioButton1.UseVisualStyleBackColor = true;
            // 
            // speedRadioButton3
            // 
            speedRadioButton3.AutoSize = true;
            speedRadioButton3.Checked = true;
            speedRadioButton3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            speedRadioButton3.ForeColor = System.Drawing.Color.White;
            speedRadioButton3.Location = new System.Drawing.Point(230, 27);
            speedRadioButton3.Name = "speedRadioButton3";
            speedRadioButton3.Size = new System.Drawing.Size(52, 21);
            speedRadioButton3.TabIndex = 2;
            speedRadioButton3.TabStop = true;
            speedRadioButton3.Text = "보통";
            speedRadioButton3.UseVisualStyleBackColor = true;
            // 
            // speedRadioButton2
            // 
            speedRadioButton2.AutoSize = true;
            speedRadioButton2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            speedRadioButton2.ForeColor = System.Drawing.Color.White;
            speedRadioButton2.Location = new System.Drawing.Point(142, 27);
            speedRadioButton2.Name = "speedRadioButton2";
            speedRadioButton2.Size = new System.Drawing.Size(52, 21);
            speedRadioButton2.TabIndex = 1;
            speedRadioButton2.Text = "빠름";
            speedRadioButton2.UseVisualStyleBackColor = true;
            // 
            // panel13
            // 
            panel13.Controls.Add(defaultButton);
            panel13.Controls.Add(saveConfigButton);
            panel13.Controls.Add(openConfigButton);
            panel13.Controls.Add(lbSettingFile);
            panel13.Location = new System.Drawing.Point(3, 255);
            panel13.Name = "panel13";
            panel13.Size = new System.Drawing.Size(533, 131);
            panel13.TabIndex = 39;
            panel13.Paint += panealBorder_Paint;
            // 
            // defaultButton
            // 
            defaultButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            defaultButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            defaultButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            defaultButton.ForeColor = System.Drawing.Color.White;
            defaultButton.Location = new System.Drawing.Point(8, 95);
            defaultButton.Name = "defaultButton";
            defaultButton.Size = new System.Drawing.Size(516, 25);
            defaultButton.TabIndex = 27;
            defaultButton.Text = "초기화";
            defaultButton.UseVisualStyleBackColor = false;
            defaultButton.Click += defaultButton_Click;
            // 
            // saveConfigButton
            // 
            saveConfigButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            saveConfigButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            saveConfigButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            saveConfigButton.ForeColor = System.Drawing.Color.White;
            saveConfigButton.Location = new System.Drawing.Point(8, 65);
            saveConfigButton.Name = "saveConfigButton";
            saveConfigButton.Size = new System.Drawing.Size(516, 25);
            saveConfigButton.TabIndex = 26;
            saveConfigButton.Text = "설정 저장하기";
            saveConfigButton.UseVisualStyleBackColor = false;
            saveConfigButton.Click += settingSaveToolStripMenuItem2_Click;
            // 
            // openConfigButton
            // 
            openConfigButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            openConfigButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            openConfigButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            openConfigButton.ForeColor = System.Drawing.Color.White;
            openConfigButton.Location = new System.Drawing.Point(8, 35);
            openConfigButton.Name = "openConfigButton";
            openConfigButton.Size = new System.Drawing.Size(516, 25);
            openConfigButton.TabIndex = 25;
            openConfigButton.Text = "설정 불러오기";
            openConfigButton.UseVisualStyleBackColor = false;
            openConfigButton.Click += settingLoadToolStripMenuItem2_Click;
            // 
            // lbSettingFile
            // 
            lbSettingFile.AutoSize = true;
            lbSettingFile.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbSettingFile.ForeColor = System.Drawing.Color.White;
            lbSettingFile.Location = new System.Drawing.Point(4, 3);
            lbSettingFile.Name = "lbSettingFile";
            lbSettingFile.Size = new System.Drawing.Size(74, 20);
            lbSettingFile.TabIndex = 8;
            lbSettingFile.Text = "설정 파일";
            // 
            // panel12
            // 
            panel12.Controls.Add(topMostcheckBox);
            panel12.Controls.Add(checkUpdateCheckBox);
            panel12.Controls.Add(label15);
            panel12.Location = new System.Drawing.Point(3, 104);
            panel12.Name = "panel12";
            panel12.Size = new System.Drawing.Size(533, 62);
            panel12.TabIndex = 38;
            panel12.Paint += panealBorder_Paint;
            // 
            // topMostcheckBox
            // 
            topMostcheckBox.AutoSize = true;
            topMostcheckBox.Checked = true;
            topMostcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            topMostcheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            topMostcheckBox.ForeColor = System.Drawing.Color.White;
            topMostcheckBox.Location = new System.Drawing.Point(199, 36);
            topMostcheckBox.Name = "topMostcheckBox";
            topMostcheckBox.Size = new System.Drawing.Size(123, 21);
            topMostcheckBox.TabIndex = 12;
            topMostcheckBox.Text = "번역창 최상위로";
            topMostcheckBox.UseVisualStyleBackColor = true;
            // 
            // checkUpdateCheckBox
            // 
            checkUpdateCheckBox.AutoSize = true;
            checkUpdateCheckBox.Checked = true;
            checkUpdateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            checkUpdateCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            checkUpdateCheckBox.ForeColor = System.Drawing.Color.White;
            checkUpdateCheckBox.Location = new System.Drawing.Point(17, 36);
            checkUpdateCheckBox.Name = "checkUpdateCheckBox";
            checkUpdateCheckBox.Size = new System.Drawing.Size(115, 21);
            checkUpdateCheckBox.TabIndex = 11;
            checkUpdateCheckBox.Text = "최신 버전 확인";
            checkUpdateCheckBox.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            label15.ForeColor = System.Drawing.Color.White;
            label15.Location = new System.Drawing.Point(4, 3);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(36, 20);
            label15.TabIndex = 8;
            label15.Text = "ETC";
            // 
            // panel14
            // 
            panel14.Controls.Add(btAttachCapture);
            panel14.Controls.Add(SetDefaultZoomSizeButton);
            panel14.Controls.Add(imgZoomsizeUpDown);
            panel14.Controls.Add(lbImgZoom);
            panel14.Controls.Add(activeWinodeCheckBox);
            panel14.Controls.Add(lbImgCapture);
            panel14.Location = new System.Drawing.Point(3, 3);
            panel14.Name = "panel14";
            panel14.Size = new System.Drawing.Size(533, 96);
            panel14.TabIndex = 37;
            panel14.Paint += panealBorder_Paint;
            // 
            // btAttachCapture
            // 
            btAttachCapture.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btAttachCapture.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btAttachCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btAttachCapture.ForeColor = System.Drawing.Color.White;
            btAttachCapture.Location = new System.Drawing.Point(9, 65);
            btAttachCapture.Name = "btAttachCapture";
            btAttachCapture.Size = new System.Drawing.Size(515, 25);
            btAttachCapture.TabIndex = 27;
            btAttachCapture.Text = "화면을 가져올 윈도우 지정하기";
            btAttachCapture.UseVisualStyleBackColor = false;
            btAttachCapture.Click += OnClick_btAttachCapture;
            // 
            // SetDefaultZoomSizeButton
            // 
            SetDefaultZoomSizeButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            SetDefaultZoomSizeButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            SetDefaultZoomSizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            SetDefaultZoomSizeButton.ForeColor = System.Drawing.Color.White;
            SetDefaultZoomSizeButton.Location = new System.Drawing.Point(445, 36);
            SetDefaultZoomSizeButton.Name = "SetDefaultZoomSizeButton";
            SetDefaultZoomSizeButton.Size = new System.Drawing.Size(56, 23);
            SetDefaultZoomSizeButton.TabIndex = 28;
            SetDefaultZoomSizeButton.Text = "기본값";
            SetDefaultZoomSizeButton.UseVisualStyleBackColor = false;
            SetDefaultZoomSizeButton.Click += SetDefaultZoomSizeButton_Click;
            // 
            // imgZoomsizeUpDown
            // 
            imgZoomsizeUpDown.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            imgZoomsizeUpDown.DecimalPlaces = 1;
            imgZoomsizeUpDown.ForeColor = System.Drawing.Color.White;
            imgZoomsizeUpDown.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            imgZoomsizeUpDown.Location = new System.Drawing.Point(392, 36);
            imgZoomsizeUpDown.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            imgZoomsizeUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            imgZoomsizeUpDown.Name = "imgZoomsizeUpDown";
            imgZoomsizeUpDown.Size = new System.Drawing.Size(47, 23);
            imgZoomsizeUpDown.TabIndex = 53;
            imgZoomsizeUpDown.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // lbImgZoom
            // 
            lbImgZoom.AutoSize = true;
            lbImgZoom.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbImgZoom.ForeColor = System.Drawing.Color.White;
            lbImgZoom.Location = new System.Drawing.Point(269, 36);
            lbImgZoom.Name = "lbImgZoom";
            lbImgZoom.Size = new System.Drawing.Size(117, 17);
            lbImgZoom.TabIndex = 51;
            lbImgZoom.Text = "추출 이미지 확대 :";
            // 
            // activeWinodeCheckBox
            // 
            activeWinodeCheckBox.AutoSize = true;
            activeWinodeCheckBox.Checked = true;
            activeWinodeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            activeWinodeCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            activeWinodeCheckBox.ForeColor = System.Drawing.Color.White;
            activeWinodeCheckBox.Location = new System.Drawing.Point(17, 36);
            activeWinodeCheckBox.Name = "activeWinodeCheckBox";
            activeWinodeCheckBox.Size = new System.Drawing.Size(224, 21);
            activeWinodeCheckBox.TabIndex = 10;
            activeWinodeCheckBox.Text = "활성화된 윈도우에서 이미지 캡쳐\r\n";
            activeWinodeCheckBox.UseVisualStyleBackColor = true;
            // 
            // lbImgCapture
            // 
            lbImgCapture.AutoSize = true;
            lbImgCapture.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbImgCapture.ForeColor = System.Drawing.Color.White;
            lbImgCapture.Location = new System.Drawing.Point(4, 3);
            lbImgCapture.Name = "lbImgCapture";
            lbImgCapture.Size = new System.Drawing.Size(89, 20);
            lbImgCapture.TabIndex = 8;
            lbImgCapture.Text = "이미지 캡쳐";
            // 
            // tpTranslation
            // 
            tpTranslation.Controls.Add(panel19);
            tpTranslation.Location = new System.Drawing.Point(80, 4);
            tpTranslation.Margin = new System.Windows.Forms.Padding(0);
            tpTranslation.Name = "tpTranslation";
            tpTranslation.Size = new System.Drawing.Size(540, 585);
            tpTranslation.TabIndex = 4;
            tpTranslation.Text = "번역설정";
            tpTranslation.UseVisualStyleBackColor = true;
            // 
            // panel19
            // 
            panel19.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel19.Controls.Add(panel4);
            panel19.Controls.Add(panel27);
            panel19.Controls.Add(panel22);
            panel19.Controls.Add(panel1);
            panel19.Controls.Add(panel15);
            panel19.Dock = System.Windows.Forms.DockStyle.Fill;
            panel19.Location = new System.Drawing.Point(0, 0);
            panel19.Margin = new System.Windows.Forms.Padding(0);
            panel19.Name = "panel19";
            panel19.Size = new System.Drawing.Size(540, 585);
            panel19.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(cbDeepLLanguageTo);
            panel4.Controls.Add(lbDeepLTo);
            panel4.Controls.Add(label6);
            panel4.Controls.Add(cbDeepLLanguage);
            panel4.Controls.Add(lbDeepL);
            panel4.Controls.Add(lbDeepLFrom);
            panel4.Location = new System.Drawing.Point(3, 265);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(531, 76);
            panel4.TabIndex = 57;
            panel4.Paint += panealBorder_Paint;
            // 
            // cbDeepLLanguageTo
            // 
            cbDeepLLanguageTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbDeepLLanguageTo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbDeepLLanguageTo.FormattingEnabled = true;
            cbDeepLLanguageTo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbDeepLLanguageTo.Items.AddRange(new object[] { "한국어", "영어", "일본어", "중국어 - 간체", "중국어 - 번체", "러시아어", "독일어", "브라질어", "포르투갈어", "스페인어", "프랑스어", "베트남어", "태국어" });
            cbDeepLLanguageTo.Location = new System.Drawing.Point(304, 35);
            cbDeepLLanguageTo.Name = "cbDeepLLanguageTo";
            cbDeepLLanguageTo.Size = new System.Drawing.Size(100, 25);
            cbDeepLLanguageTo.TabIndex = 53;
            // 
            // lbDeepLTo
            // 
            lbDeepLTo.AutoSize = true;
            lbDeepLTo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbDeepLTo.ForeColor = System.Drawing.Color.White;
            lbDeepLTo.Location = new System.Drawing.Point(417, 38);
            lbDeepLTo.Name = "lbDeepLTo";
            lbDeepLTo.Size = new System.Drawing.Size(52, 17);
            lbDeepLTo.TabIndex = 52;
            lbDeepLTo.Text = "로 번역";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label6.ForeColor = System.Drawing.Color.White;
            label6.Location = new System.Drawing.Point(249, 38);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(22, 17);
            label6.TabIndex = 51;
            label6.Text = "->";
            // 
            // cbDeepLLanguage
            // 
            cbDeepLLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbDeepLLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbDeepLLanguage.FormattingEnabled = true;
            cbDeepLLanguage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbDeepLLanguage.Items.AddRange(new object[] { "영어", "일본어", "중국어 간체", "중국어 번체", "한국어", "러시아어", "독일어", "브라질어", "포르투갈어", "스페인어", "프랑스어", "베트남어", "태국어" });
            cbDeepLLanguage.Location = new System.Drawing.Point(50, 35);
            cbDeepLLanguage.Name = "cbDeepLLanguage";
            cbDeepLLanguage.Size = new System.Drawing.Size(100, 25);
            cbDeepLLanguage.TabIndex = 50;
            // 
            // lbDeepL
            // 
            lbDeepL.AutoSize = true;
            lbDeepL.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbDeepL.ForeColor = System.Drawing.Color.White;
            lbDeepL.Location = new System.Drawing.Point(4, 3);
            lbDeepL.Name = "lbDeepL";
            lbDeepL.Size = new System.Drawing.Size(123, 20);
            lbDeepL.TabIndex = 8;
            lbDeepL.Text = "DeepL 번역 설정";
            // 
            // lbDeepLFrom
            // 
            lbDeepLFrom.AutoSize = true;
            lbDeepLFrom.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbDeepLFrom.ForeColor = System.Drawing.Color.White;
            lbDeepLFrom.Location = new System.Drawing.Point(170, 38);
            lbDeepLFrom.Name = "lbDeepLFrom";
            lbDeepLFrom.Size = new System.Drawing.Size(34, 17);
            lbDeepLFrom.TabIndex = 49;
            lbDeepLFrom.Text = "에서";
            // 
            // panel27
            // 
            panel27.Controls.Add(cbTTSWaitEnd);
            panel27.Controls.Add(cbUseTTS);
            panel27.Controls.Add(label66);
            panel27.Location = new System.Drawing.Point(3, 345);
            panel27.Name = "panel27";
            panel27.Size = new System.Drawing.Size(531, 84);
            panel27.TabIndex = 54;
            panel27.Paint += panealBorder_Paint;
            // 
            // cbTTSWaitEnd
            // 
            cbTTSWaitEnd.AutoSize = true;
            cbTTSWaitEnd.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            cbTTSWaitEnd.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbTTSWaitEnd.ForeColor = System.Drawing.Color.White;
            cbTTSWaitEnd.Location = new System.Drawing.Point(135, 26);
            cbTTSWaitEnd.Name = "cbTTSWaitEnd";
            cbTTSWaitEnd.Size = new System.Drawing.Size(177, 21);
            cbTTSWaitEnd.TabIndex = 12;
            cbTTSWaitEnd.Text = "음성이 끝날 때 까지 대기";
            cbTTSWaitEnd.UseVisualStyleBackColor = false;
            // 
            // cbUseTTS
            // 
            cbUseTTS.AutoSize = true;
            cbUseTTS.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbUseTTS.ForeColor = System.Drawing.Color.White;
            cbUseTTS.Location = new System.Drawing.Point(20, 26);
            cbUseTTS.Name = "cbUseTTS";
            cbUseTTS.Size = new System.Drawing.Size(81, 21);
            cbUseTTS.TabIndex = 9;
            cbUseTTS.Text = "TTS 사용";
            cbUseTTS.UseVisualStyleBackColor = true;
            cbUseTTS.CheckedChanged += cbUseTTS_CheckedChanged;
            // 
            // label66
            // 
            label66.AutoSize = true;
            label66.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            label66.ForeColor = System.Drawing.Color.White;
            label66.Location = new System.Drawing.Point(4, 3);
            label66.Name = "label66";
            label66.Size = new System.Drawing.Size(35, 20);
            label66.TabIndex = 8;
            label66.Text = "TTS";
            // 
            // panel22
            // 
            panel22.Controls.Add(googleResultCodeComboBox);
            panel22.Controls.Add(lbGoogleTo);
            panel22.Controls.Add(label56);
            panel22.Controls.Add(googleTransComboBox);
            panel22.Controls.Add(lbGoogle);
            panel22.Controls.Add(lbGoogleFrom);
            panel22.Location = new System.Drawing.Point(3, 183);
            panel22.Name = "panel22";
            panel22.Size = new System.Drawing.Size(531, 76);
            panel22.TabIndex = 56;
            panel22.Paint += panealBorder_Paint;
            // 
            // googleResultCodeComboBox
            // 
            googleResultCodeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            googleResultCodeComboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            googleResultCodeComboBox.FormattingEnabled = true;
            googleResultCodeComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            googleResultCodeComboBox.Items.AddRange(new object[] { "한국어", "영어", "일본어", "중국어 - 간체", "중국어 - 번체", "러시아어", "독일어", "브라질어", "포르투갈어", "스페인어", "프랑스어", "베트남어", "태국어" });
            googleResultCodeComboBox.Location = new System.Drawing.Point(304, 35);
            googleResultCodeComboBox.Name = "googleResultCodeComboBox";
            googleResultCodeComboBox.Size = new System.Drawing.Size(100, 25);
            googleResultCodeComboBox.TabIndex = 53;
            // 
            // lbGoogleTo
            // 
            lbGoogleTo.AutoSize = true;
            lbGoogleTo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbGoogleTo.ForeColor = System.Drawing.Color.White;
            lbGoogleTo.Location = new System.Drawing.Point(417, 38);
            lbGoogleTo.Name = "lbGoogleTo";
            lbGoogleTo.Size = new System.Drawing.Size(52, 17);
            lbGoogleTo.TabIndex = 52;
            lbGoogleTo.Text = "로 번역";
            // 
            // label56
            // 
            label56.AutoSize = true;
            label56.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label56.ForeColor = System.Drawing.Color.White;
            label56.Location = new System.Drawing.Point(249, 38);
            label56.Name = "label56";
            label56.Size = new System.Drawing.Size(22, 17);
            label56.TabIndex = 51;
            label56.Text = "->";
            // 
            // googleTransComboBox
            // 
            googleTransComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            googleTransComboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            googleTransComboBox.FormattingEnabled = true;
            googleTransComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            googleTransComboBox.Items.AddRange(new object[] { "영어", "일본어", "중국어 간체", "중국어 번체", "한국어", "러시아어", "독일어", "브라질어", "포르투갈어", "스페인어", "프랑스어", "베트남어", "태국어" });
            googleTransComboBox.Location = new System.Drawing.Point(50, 35);
            googleTransComboBox.Name = "googleTransComboBox";
            googleTransComboBox.Size = new System.Drawing.Size(100, 25);
            googleTransComboBox.TabIndex = 50;
            // 
            // lbGoogle
            // 
            lbGoogle.AutoSize = true;
            lbGoogle.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbGoogle.ForeColor = System.Drawing.Color.White;
            lbGoogle.Location = new System.Drawing.Point(4, 3);
            lbGoogle.Name = "lbGoogle";
            lbGoogle.Size = new System.Drawing.Size(392, 20);
            lbGoogle.TabIndex = 8;
            lbGoogle.Text = "구글 번역 설정 (기본 번역기 , 구글 시트 번역기, Gemini)";
            // 
            // lbGoogleFrom
            // 
            lbGoogleFrom.AutoSize = true;
            lbGoogleFrom.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbGoogleFrom.ForeColor = System.Drawing.Color.White;
            lbGoogleFrom.Location = new System.Drawing.Point(170, 38);
            lbGoogleFrom.Name = "lbGoogleFrom";
            lbGoogleFrom.Size = new System.Drawing.Size(34, 17);
            lbGoogleFrom.TabIndex = 49;
            lbGoogleFrom.Text = "에서";
            // 
            // panel1
            // 
            panel1.Controls.Add(skinOverRadioButton);
            panel1.Controls.Add(skinLayerRadioButton);
            panel1.Controls.Add(lbTransformType);
            panel1.Controls.Add(skinDarkRadioButton);
            panel1.Location = new System.Drawing.Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(531, 49);
            panel1.TabIndex = 55;
            panel1.Paint += panealBorder_Paint;
            // 
            // skinOverRadioButton
            // 
            skinOverRadioButton.AutoSize = true;
            skinOverRadioButton.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            skinOverRadioButton.ForeColor = System.Drawing.Color.White;
            skinOverRadioButton.Location = new System.Drawing.Point(150, 23);
            skinOverRadioButton.Name = "skinOverRadioButton";
            skinOverRadioButton.Size = new System.Drawing.Size(78, 21);
            skinOverRadioButton.TabIndex = 10;
            skinOverRadioButton.Text = "오버레이";
            skinOverRadioButton.UseVisualStyleBackColor = true;
            // 
            // skinLayerRadioButton
            // 
            skinLayerRadioButton.AutoSize = true;
            skinLayerRadioButton.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            skinLayerRadioButton.ForeColor = System.Drawing.Color.White;
            skinLayerRadioButton.Location = new System.Drawing.Point(77, 23);
            skinLayerRadioButton.Name = "skinLayerRadioButton";
            skinLayerRadioButton.Size = new System.Drawing.Size(65, 21);
            skinLayerRadioButton.TabIndex = 9;
            skinLayerRadioButton.Text = "레이어";
            skinLayerRadioButton.UseVisualStyleBackColor = true;
            // 
            // lbTransformType
            // 
            lbTransformType.AutoSize = true;
            lbTransformType.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbTransformType.ForeColor = System.Drawing.Color.White;
            lbTransformType.Location = new System.Drawing.Point(4, 3);
            lbTransformType.Name = "lbTransformType";
            lbTransformType.Size = new System.Drawing.Size(89, 20);
            lbTransformType.TabIndex = 8;
            lbTransformType.Text = "번역창 방식";
            // 
            // skinDarkRadioButton
            // 
            skinDarkRadioButton.AutoSize = true;
            skinDarkRadioButton.Checked = true;
            skinDarkRadioButton.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            skinDarkRadioButton.ForeColor = System.Drawing.Color.White;
            skinDarkRadioButton.Location = new System.Drawing.Point(11, 23);
            skinDarkRadioButton.Name = "skinDarkRadioButton";
            skinDarkRadioButton.Size = new System.Drawing.Size(65, 21);
            skinDarkRadioButton.TabIndex = 6;
            skinDarkRadioButton.TabStop = true;
            skinDarkRadioButton.Text = "어두운";
            skinDarkRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel15
            // 
            panel15.Controls.Add(lbPapagoLanguageCodeInformation);
            panel15.Controls.Add(cbNaverResultCode);
            panel15.Controls.Add(lbPaPagoTo);
            panel15.Controls.Add(label43);
            panel15.Controls.Add(naverTransComboBox);
            panel15.Controls.Add(lbPaPago);
            panel15.Controls.Add(lbPaPagoFrom);
            panel15.Location = new System.Drawing.Point(3, 58);
            panel15.Name = "panel15";
            panel15.Size = new System.Drawing.Size(531, 119);
            panel15.TabIndex = 54;
            panel15.Paint += panealBorder_Paint;
            // 
            // lbPapagoLanguageCodeInformation
            // 
            lbPapagoLanguageCodeInformation.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbPapagoLanguageCodeInformation.ForeColor = System.Drawing.Color.White;
            lbPapagoLanguageCodeInformation.Location = new System.Drawing.Point(21, 74);
            lbPapagoLanguageCodeInformation.Name = "lbPapagoLanguageCodeInformation";
            lbPapagoLanguageCodeInformation.Size = new System.Drawing.Size(489, 34);
            lbPapagoLanguageCodeInformation.TabIndex = 56;
            lbPapagoLanguageCodeInformation.Text = "방식에 따라 지원되지 않는 언어가 있습니다\r\n실제 지원하는 언어는 API 문서를 참고하시기 바랍니다";
            lbPapagoLanguageCodeInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbNaverResultCode
            // 
            cbNaverResultCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbNaverResultCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbNaverResultCode.FormattingEnabled = true;
            cbNaverResultCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            cbNaverResultCode.Items.AddRange(new object[] { "한국어", "영어" });
            cbNaverResultCode.Location = new System.Drawing.Point(304, 30);
            cbNaverResultCode.Name = "cbNaverResultCode";
            cbNaverResultCode.Size = new System.Drawing.Size(100, 25);
            cbNaverResultCode.TabIndex = 55;
            // 
            // lbPaPagoTo
            // 
            lbPaPagoTo.AutoSize = true;
            lbPaPagoTo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbPaPagoTo.ForeColor = System.Drawing.Color.White;
            lbPaPagoTo.Location = new System.Drawing.Point(417, 33);
            lbPaPagoTo.Name = "lbPaPagoTo";
            lbPaPagoTo.Size = new System.Drawing.Size(52, 17);
            lbPaPagoTo.TabIndex = 54;
            lbPaPagoTo.Text = "로 번역";
            // 
            // label43
            // 
            label43.AutoSize = true;
            label43.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            label43.ForeColor = System.Drawing.Color.White;
            label43.Location = new System.Drawing.Point(249, 38);
            label43.Name = "label43";
            label43.Size = new System.Drawing.Size(22, 17);
            label43.TabIndex = 51;
            label43.Text = "->";
            // 
            // naverTransComboBox
            // 
            naverTransComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            naverTransComboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            naverTransComboBox.FormattingEnabled = true;
            naverTransComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            naverTransComboBox.Items.AddRange(new object[] { "영어", "일본어", "중국어 간체", "중국어 번체", "스페인어", "프랑스어", "베트남어", "태국어", "인도네시아어", "한국어" });
            naverTransComboBox.Location = new System.Drawing.Point(50, 35);
            naverTransComboBox.Name = "naverTransComboBox";
            naverTransComboBox.Size = new System.Drawing.Size(100, 25);
            naverTransComboBox.TabIndex = 50;
            // 
            // lbPaPago
            // 
            lbPaPago.AutoSize = true;
            lbPaPago.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbPaPago.ForeColor = System.Drawing.Color.White;
            lbPaPago.Location = new System.Drawing.Point(4, 3);
            lbPaPago.Name = "lbPaPago";
            lbPaPago.Size = new System.Drawing.Size(179, 20);
            lbPaPago.TabIndex = 8;
            lbPaPago.Text = "네이버(파파고) 번역 설정";
            // 
            // lbPaPagoFrom
            // 
            lbPaPagoFrom.AutoSize = true;
            lbPaPagoFrom.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbPaPagoFrom.ForeColor = System.Drawing.Color.White;
            lbPaPagoFrom.Location = new System.Drawing.Point(170, 38);
            lbPaPagoFrom.Name = "lbPaPagoFrom";
            lbPaPagoFrom.Size = new System.Drawing.Size(34, 17);
            lbPaPagoFrom.TabIndex = 49;
            lbPaPagoFrom.Text = "에서";
            // 
            // tpETC
            // 
            tpETC.Controls.Add(panel18);
            tpETC.Location = new System.Drawing.Point(80, 4);
            tpETC.Margin = new System.Windows.Forms.Padding(0);
            tpETC.Name = "tpETC";
            tpETC.Size = new System.Drawing.Size(540, 585);
            tpETC.TabIndex = 3;
            tpETC.Text = "그 외";
            tpETC.UseVisualStyleBackColor = true;
            // 
            // panel18
            // 
            panel18.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel18.Controls.Add(panel16);
            panel18.Controls.Add(panel20);
            panel18.Controls.Add(panel23);
            panel18.Dock = System.Windows.Forms.DockStyle.Fill;
            panel18.Location = new System.Drawing.Point(0, 0);
            panel18.Margin = new System.Windows.Forms.Padding(0);
            panel18.Name = "panel18";
            panel18.Size = new System.Drawing.Size(540, 585);
            panel18.TabIndex = 2;
            // 
            // panel16
            // 
            panel16.Controls.Add(btnOpenDiscord);
            panel16.Controls.Add(openBlogButton);
            panel16.Controls.Add(btnGitHub);
            panel16.Controls.Add(lbLink);
            panel16.Location = new System.Drawing.Point(3, 405);
            panel16.Name = "panel16";
            panel16.Size = new System.Drawing.Size(531, 128);
            panel16.TabIndex = 42;
            panel16.Paint += panealBorder_Paint;
            // 
            // btnOpenDiscord
            // 
            btnOpenDiscord.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnOpenDiscord.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnOpenDiscord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOpenDiscord.ForeColor = System.Drawing.Color.White;
            btnOpenDiscord.Location = new System.Drawing.Point(7, 95);
            btnOpenDiscord.Name = "btnOpenDiscord";
            btnOpenDiscord.Size = new System.Drawing.Size(516, 25);
            btnOpenDiscord.TabIndex = 26;
            btnOpenDiscord.Text = "디스코드 채널로 이동";
            btnOpenDiscord.UseVisualStyleBackColor = false;
            btnOpenDiscord.Click += OnClickOpenDiscord;
            // 
            // openBlogButton
            // 
            openBlogButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            openBlogButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            openBlogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            openBlogButton.ForeColor = System.Drawing.Color.White;
            openBlogButton.Location = new System.Drawing.Point(8, 65);
            openBlogButton.Name = "openBlogButton";
            openBlogButton.Size = new System.Drawing.Size(516, 25);
            openBlogButton.TabIndex = 25;
            openBlogButton.Text = "블로그 방문";
            openBlogButton.UseVisualStyleBackColor = false;
            openBlogButton.Click += OnClickopenBlog;
            // 
            // btnGitHub
            // 
            btnGitHub.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnGitHub.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnGitHub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnGitHub.ForeColor = System.Drawing.Color.White;
            btnGitHub.Location = new System.Drawing.Point(8, 35);
            btnGitHub.Name = "btnGitHub";
            btnGitHub.Size = new System.Drawing.Size(516, 25);
            btnGitHub.TabIndex = 25;
            btnGitHub.Text = "Github로 이동";
            btnGitHub.UseVisualStyleBackColor = false;
            btnGitHub.Click += OnClick_GitHub;
            // 
            // lbLink
            // 
            lbLink.AutoSize = true;
            lbLink.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbLink.ForeColor = System.Drawing.Color.White;
            lbLink.Location = new System.Drawing.Point(4, 3);
            lbLink.Name = "lbLink";
            lbLink.Size = new System.Drawing.Size(39, 20);
            lbLink.TabIndex = 8;
            lbLink.Text = "링크";
            // 
            // panel20
            // 
            panel20.Controls.Add(about_Button);
            panel20.Controls.Add(error_Information_Button);
            panel20.Controls.Add(help_Button);
            panel20.Controls.Add(lbETC);
            panel20.Location = new System.Drawing.Point(3, 269);
            panel20.Name = "panel20";
            panel20.Size = new System.Drawing.Size(531, 130);
            panel20.TabIndex = 41;
            panel20.Paint += panealBorder_Paint;
            // 
            // about_Button
            // 
            about_Button.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            about_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            about_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            about_Button.ForeColor = System.Drawing.Color.White;
            about_Button.Location = new System.Drawing.Point(8, 95);
            about_Button.Name = "about_Button";
            about_Button.Size = new System.Drawing.Size(516, 25);
            about_Button.TabIndex = 26;
            about_Button.Text = "About";
            about_Button.UseVisualStyleBackColor = false;
            about_Button.Click += about_Button_Click;
            // 
            // error_Information_Button
            // 
            error_Information_Button.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            error_Information_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            error_Information_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            error_Information_Button.ForeColor = System.Drawing.Color.White;
            error_Information_Button.Location = new System.Drawing.Point(8, 65);
            error_Information_Button.Name = "error_Information_Button";
            error_Information_Button.Size = new System.Drawing.Size(516, 25);
            error_Information_Button.TabIndex = 25;
            error_Information_Button.Text = "에러 메시지 목록";
            error_Information_Button.UseVisualStyleBackColor = false;
            error_Information_Button.Click += error_Information_Button_Click;
            // 
            // help_Button
            // 
            help_Button.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            help_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            help_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            help_Button.ForeColor = System.Drawing.Color.White;
            help_Button.Location = new System.Drawing.Point(8, 35);
            help_Button.Name = "help_Button";
            help_Button.Size = new System.Drawing.Size(516, 25);
            help_Button.TabIndex = 25;
            help_Button.Text = "MORT 사용법";
            help_Button.UseVisualStyleBackColor = false;
            help_Button.Click += help_Button_Click;
            // 
            // lbETC
            // 
            lbETC.AutoSize = true;
            lbETC.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbETC.ForeColor = System.Drawing.Color.White;
            lbETC.Location = new System.Drawing.Point(4, 3);
            lbETC.Name = "lbETC";
            lbETC.Size = new System.Drawing.Size(44, 20);
            lbETC.TabIndex = 8;
            lbETC.Text = "그 외";
            // 
            // panel23
            // 
            panel23.Controls.Add(panel2);
            panel23.Controls.Add(lbHotKeyFollowMouse);
            panel23.Controls.Add(lbHotKeyHideTransWindow);
            panel23.Controls.Add(lbHotKeyOnceTranslate);
            panel23.Controls.Add(lbHotKeySnapShot);
            panel23.Controls.Add(lbHotKeyInformation);
            panel23.Controls.Add(lbHotKeyQuickOCR);
            panel23.Controls.Add(lbHotKeyDic);
            panel23.Controls.Add(lbHotKeyDoTrans);
            panel23.Controls.Add(lbHotkey);
            panel23.Location = new System.Drawing.Point(3, 3);
            panel23.Name = "panel23";
            panel23.Size = new System.Drawing.Size(531, 266);
            panel23.TabIndex = 37;
            panel23.Paint += panealBorder_Paint;
            // 
            // panel2
            // 
            panel2.Controls.Add(transKeyInputLabel);
            panel2.Controls.Add(btnHideTransEmpty);
            panel2.Controls.Add(dicKeyInputLabel);
            panel2.Controls.Add(btnHideTransDefault);
            panel2.Controls.Add(quickKeyInputLabel);
            panel2.Controls.Add(lbHideTranslate);
            panel2.Controls.Add(lbFollowMouse);
            panel2.Controls.Add(btnFollowMouseEmpty);
            panel2.Controls.Add(btnFollowMouseDefault);
            panel2.Controls.Add(transKeyInputResetButton);
            panel2.Controls.Add(dicKeyInputResetButton);
            panel2.Controls.Add(btnOneTransEmpty);
            panel2.Controls.Add(quickKeyInputResetButton);
            panel2.Controls.Add(btnOneTransDefault);
            panel2.Controls.Add(transKeyInputEmptyButton);
            panel2.Controls.Add(lbOneTrans);
            panel2.Controls.Add(dicKeyInputEmptyButton);
            panel2.Controls.Add(quickKeyInputEmptyButton);
            panel2.Controls.Add(snapShotKeyInputEmptyButton);
            panel2.Controls.Add(snapShotInputLabel);
            panel2.Controls.Add(snapShotKeyInputResetButton);
            panel2.Location = new System.Drawing.Point(163, 23);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(361, 211);
            panel2.TabIndex = 62;
            // 
            // transKeyInputLabel
            // 
            transKeyInputLabel.Location = new System.Drawing.Point(16, 14);
            transKeyInputLabel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            transKeyInputLabel.Name = "transKeyInputLabel";
            transKeyInputLabel.Size = new System.Drawing.Size(198, 21);
            transKeyInputLabel.TabIndex = 26;
            // 
            // btnHideTransEmpty
            // 
            btnHideTransEmpty.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnHideTransEmpty.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnHideTransEmpty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnHideTransEmpty.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            btnHideTransEmpty.ForeColor = System.Drawing.Color.White;
            btnHideTransEmpty.Location = new System.Drawing.Point(296, 153);
            btnHideTransEmpty.Name = "btnHideTransEmpty";
            btnHideTransEmpty.Size = new System.Drawing.Size(56, 23);
            btnHideTransEmpty.TabIndex = 61;
            btnHideTransEmpty.Text = "비우기";
            btnHideTransEmpty.UseVisualStyleBackColor = false;
            btnHideTransEmpty.Click += btnHideTransEmpty_Click;
            // 
            // dicKeyInputLabel
            // 
            dicKeyInputLabel.Location = new System.Drawing.Point(16, 41);
            dicKeyInputLabel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            dicKeyInputLabel.Name = "dicKeyInputLabel";
            dicKeyInputLabel.Size = new System.Drawing.Size(198, 21);
            dicKeyInputLabel.TabIndex = 28;
            // 
            // btnHideTransDefault
            // 
            btnHideTransDefault.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnHideTransDefault.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnHideTransDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnHideTransDefault.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            btnHideTransDefault.ForeColor = System.Drawing.Color.White;
            btnHideTransDefault.Location = new System.Drawing.Point(234, 153);
            btnHideTransDefault.Name = "btnHideTransDefault";
            btnHideTransDefault.Size = new System.Drawing.Size(56, 23);
            btnHideTransDefault.TabIndex = 60;
            btnHideTransDefault.Text = "기본값";
            btnHideTransDefault.UseVisualStyleBackColor = false;
            btnHideTransDefault.Click += btnHideTransDefault_Click;
            // 
            // quickKeyInputLabel
            // 
            quickKeyInputLabel.Location = new System.Drawing.Point(16, 68);
            quickKeyInputLabel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            quickKeyInputLabel.Name = "quickKeyInputLabel";
            quickKeyInputLabel.Size = new System.Drawing.Size(198, 21);
            quickKeyInputLabel.TabIndex = 30;
            // 
            // lbHideTranslate
            // 
            lbHideTranslate.Location = new System.Drawing.Point(16, 149);
            lbHideTranslate.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            lbHideTranslate.Name = "lbHideTranslate";
            lbHideTranslate.Size = new System.Drawing.Size(198, 26);
            lbHideTranslate.TabIndex = 59;
            // 
            // lbFollowMouse
            // 
            lbFollowMouse.Location = new System.Drawing.Point(16, 176);
            lbFollowMouse.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            lbFollowMouse.Name = "lbFollowMouse";
            lbFollowMouse.Size = new System.Drawing.Size(198, 26);
            lbFollowMouse.TabIndex = 63;
            // 
            // btnFollowMouseEmpty
            // 
            btnFollowMouseEmpty.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnFollowMouseEmpty.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnFollowMouseEmpty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnFollowMouseEmpty.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            btnFollowMouseEmpty.ForeColor = System.Drawing.Color.White;
            btnFollowMouseEmpty.Location = new System.Drawing.Point(296, 180);
            btnFollowMouseEmpty.Name = "btnFollowMouseEmpty";
            btnFollowMouseEmpty.Size = new System.Drawing.Size(56, 23);
            btnFollowMouseEmpty.TabIndex = 65;
            btnFollowMouseEmpty.Text = "비우기";
            btnFollowMouseEmpty.UseVisualStyleBackColor = false;
            btnFollowMouseEmpty.Click += btnFollowMouseEmpty_Click;
            // 
            // btnFollowMouseDefault
            // 
            btnFollowMouseDefault.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnFollowMouseDefault.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnFollowMouseDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnFollowMouseDefault.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            btnFollowMouseDefault.ForeColor = System.Drawing.Color.White;
            btnFollowMouseDefault.Location = new System.Drawing.Point(234, 180);
            btnFollowMouseDefault.Name = "btnFollowMouseDefault";
            btnFollowMouseDefault.Size = new System.Drawing.Size(56, 23);
            btnFollowMouseDefault.TabIndex = 64;
            btnFollowMouseDefault.Text = "기본값";
            btnFollowMouseDefault.UseVisualStyleBackColor = false;
            btnFollowMouseDefault.Click += btnFollowMouseDefault_Click;
            // 
            // transKeyInputResetButton
            // 
            transKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            transKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            transKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            transKeyInputResetButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            transKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            transKeyInputResetButton.Location = new System.Drawing.Point(234, 11);
            transKeyInputResetButton.Name = "transKeyInputResetButton";
            transKeyInputResetButton.Size = new System.Drawing.Size(56, 23);
            transKeyInputResetButton.TabIndex = 44;
            transKeyInputResetButton.Text = "기본값";
            transKeyInputResetButton.UseVisualStyleBackColor = false;
            transKeyInputResetButton.Click += transKeyInputResetButton_Click;
            // 
            // dicKeyInputResetButton
            // 
            dicKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            dicKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            dicKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            dicKeyInputResetButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            dicKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            dicKeyInputResetButton.Location = new System.Drawing.Point(234, 38);
            dicKeyInputResetButton.Name = "dicKeyInputResetButton";
            dicKeyInputResetButton.Size = new System.Drawing.Size(56, 23);
            dicKeyInputResetButton.TabIndex = 45;
            dicKeyInputResetButton.Text = "기본값";
            dicKeyInputResetButton.UseVisualStyleBackColor = false;
            dicKeyInputResetButton.Click += dicKeyInputResetButton_Click;
            // 
            // btnOneTransEmpty
            // 
            btnOneTransEmpty.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnOneTransEmpty.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnOneTransEmpty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOneTransEmpty.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            btnOneTransEmpty.ForeColor = System.Drawing.Color.White;
            btnOneTransEmpty.Location = new System.Drawing.Point(296, 124);
            btnOneTransEmpty.Name = "btnOneTransEmpty";
            btnOneTransEmpty.Size = new System.Drawing.Size(56, 23);
            btnOneTransEmpty.TabIndex = 57;
            btnOneTransEmpty.Text = "비우기";
            btnOneTransEmpty.UseVisualStyleBackColor = false;
            btnOneTransEmpty.Click += btnOneTransEmpty_Click;
            // 
            // quickKeyInputResetButton
            // 
            quickKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            quickKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            quickKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            quickKeyInputResetButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            quickKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            quickKeyInputResetButton.Location = new System.Drawing.Point(234, 65);
            quickKeyInputResetButton.Name = "quickKeyInputResetButton";
            quickKeyInputResetButton.Size = new System.Drawing.Size(56, 23);
            quickKeyInputResetButton.TabIndex = 46;
            quickKeyInputResetButton.Text = "기본값";
            quickKeyInputResetButton.UseVisualStyleBackColor = false;
            quickKeyInputResetButton.Click += quickKeyInputResetButton_Click;
            // 
            // btnOneTransDefault
            // 
            btnOneTransDefault.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnOneTransDefault.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnOneTransDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnOneTransDefault.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            btnOneTransDefault.ForeColor = System.Drawing.Color.White;
            btnOneTransDefault.Location = new System.Drawing.Point(234, 124);
            btnOneTransDefault.Name = "btnOneTransDefault";
            btnOneTransDefault.Size = new System.Drawing.Size(56, 23);
            btnOneTransDefault.TabIndex = 56;
            btnOneTransDefault.Text = "기본값";
            btnOneTransDefault.UseVisualStyleBackColor = false;
            btnOneTransDefault.Click += btnOneTransDefault_Click;
            // 
            // transKeyInputEmptyButton
            // 
            transKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            transKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            transKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            transKeyInputEmptyButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            transKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            transKeyInputEmptyButton.Location = new System.Drawing.Point(296, 11);
            transKeyInputEmptyButton.Name = "transKeyInputEmptyButton";
            transKeyInputEmptyButton.Size = new System.Drawing.Size(56, 23);
            transKeyInputEmptyButton.TabIndex = 47;
            transKeyInputEmptyButton.Text = "비우기";
            transKeyInputEmptyButton.UseVisualStyleBackColor = false;
            transKeyInputEmptyButton.Click += transKeyInputEmptyButton_Click;
            // 
            // lbOneTrans
            // 
            lbOneTrans.Location = new System.Drawing.Point(16, 122);
            lbOneTrans.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            lbOneTrans.Name = "lbOneTrans";
            lbOneTrans.Size = new System.Drawing.Size(198, 21);
            lbOneTrans.TabIndex = 55;
            // 
            // dicKeyInputEmptyButton
            // 
            dicKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            dicKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            dicKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            dicKeyInputEmptyButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            dicKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            dicKeyInputEmptyButton.Location = new System.Drawing.Point(296, 38);
            dicKeyInputEmptyButton.Name = "dicKeyInputEmptyButton";
            dicKeyInputEmptyButton.Size = new System.Drawing.Size(56, 23);
            dicKeyInputEmptyButton.TabIndex = 48;
            dicKeyInputEmptyButton.Text = "비우기";
            dicKeyInputEmptyButton.UseVisualStyleBackColor = false;
            dicKeyInputEmptyButton.Click += dicKeyInputEmptyButton_Click;
            // 
            // quickKeyInputEmptyButton
            // 
            quickKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            quickKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            quickKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            quickKeyInputEmptyButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            quickKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            quickKeyInputEmptyButton.Location = new System.Drawing.Point(296, 65);
            quickKeyInputEmptyButton.Name = "quickKeyInputEmptyButton";
            quickKeyInputEmptyButton.Size = new System.Drawing.Size(56, 23);
            quickKeyInputEmptyButton.TabIndex = 49;
            quickKeyInputEmptyButton.Text = "비우기";
            quickKeyInputEmptyButton.UseVisualStyleBackColor = false;
            quickKeyInputEmptyButton.Click += quickKeyInputEmptyButton_Click;
            // 
            // snapShotKeyInputEmptyButton
            // 
            snapShotKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            snapShotKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            snapShotKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            snapShotKeyInputEmptyButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            snapShotKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            snapShotKeyInputEmptyButton.Location = new System.Drawing.Point(296, 95);
            snapShotKeyInputEmptyButton.Name = "snapShotKeyInputEmptyButton";
            snapShotKeyInputEmptyButton.Size = new System.Drawing.Size(56, 23);
            snapShotKeyInputEmptyButton.TabIndex = 53;
            snapShotKeyInputEmptyButton.Text = "비우기";
            snapShotKeyInputEmptyButton.UseVisualStyleBackColor = false;
            snapShotKeyInputEmptyButton.Click += snapShotKeyInputEmptyButton_Click;
            // 
            // snapShotInputLabel
            // 
            snapShotInputLabel.Location = new System.Drawing.Point(16, 95);
            snapShotInputLabel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            snapShotInputLabel.Name = "snapShotInputLabel";
            snapShotInputLabel.Size = new System.Drawing.Size(198, 21);
            snapShotInputLabel.TabIndex = 51;
            // 
            // snapShotKeyInputResetButton
            // 
            snapShotKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            snapShotKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            snapShotKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            snapShotKeyInputResetButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            snapShotKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            snapShotKeyInputResetButton.Location = new System.Drawing.Point(234, 95);
            snapShotKeyInputResetButton.Name = "snapShotKeyInputResetButton";
            snapShotKeyInputResetButton.Size = new System.Drawing.Size(56, 23);
            snapShotKeyInputResetButton.TabIndex = 52;
            snapShotKeyInputResetButton.Text = "기본값";
            snapShotKeyInputResetButton.UseVisualStyleBackColor = false;
            snapShotKeyInputResetButton.Click += snapShotKeyInputResetButton_Click;
            // 
            // lbHotKeyFollowMouse
            // 
            lbHotKeyFollowMouse.AutoSize = true;
            lbHotKeyFollowMouse.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            lbHotKeyFollowMouse.ForeColor = System.Drawing.Color.White;
            lbHotKeyFollowMouse.Location = new System.Drawing.Point(14, 203);
            lbHotKeyFollowMouse.Name = "lbHotKeyFollowMouse";
            lbHotKeyFollowMouse.Size = new System.Drawing.Size(130, 17);
            lbHotKeyFollowMouse.TabIndex = 62;
            lbHotKeyFollowMouse.Text = "OCR 영역 따라가기 :";
            // 
            // lbHotKeyHideTransWindow
            // 
            lbHotKeyHideTransWindow.AutoSize = true;
            lbHotKeyHideTransWindow.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            lbHotKeyHideTransWindow.ForeColor = System.Drawing.Color.White;
            lbHotKeyHideTransWindow.Location = new System.Drawing.Point(14, 176);
            lbHotKeyHideTransWindow.Name = "lbHotKeyHideTransWindow";
            lbHotKeyHideTransWindow.Size = new System.Drawing.Size(104, 17);
            lbHotKeyHideTransWindow.TabIndex = 58;
            lbHotKeyHideTransWindow.Text = "번역창 숨기기 : ";
            // 
            // lbHotKeyOnceTranslate
            // 
            lbHotKeyOnceTranslate.AutoSize = true;
            lbHotKeyOnceTranslate.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            lbHotKeyOnceTranslate.ForeColor = System.Drawing.Color.White;
            lbHotKeyOnceTranslate.Location = new System.Drawing.Point(14, 145);
            lbHotKeyOnceTranslate.Name = "lbHotKeyOnceTranslate";
            lbHotKeyOnceTranslate.Size = new System.Drawing.Size(91, 17);
            lbHotKeyOnceTranslate.TabIndex = 54;
            lbHotKeyOnceTranslate.Text = "한 번만 번역 :";
            // 
            // lbHotKeySnapShot
            // 
            lbHotKeySnapShot.AutoSize = true;
            lbHotKeySnapShot.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            lbHotKeySnapShot.ForeColor = System.Drawing.Color.White;
            lbHotKeySnapShot.Location = new System.Drawing.Point(14, 118);
            lbHotKeySnapShot.Name = "lbHotKeySnapShot";
            lbHotKeySnapShot.Size = new System.Drawing.Size(55, 17);
            lbHotKeySnapShot.TabIndex = 50;
            lbHotKeySnapShot.Text = "스냅샷 :";
            // 
            // lbHotKeyInformation
            // 
            lbHotKeyInformation.Anchor = System.Windows.Forms.AnchorStyles.None;
            lbHotKeyInformation.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            lbHotKeyInformation.ForeColor = System.Drawing.Color.White;
            lbHotKeyInformation.Location = new System.Drawing.Point(17, 237);
            lbHotKeyInformation.Name = "lbHotKeyInformation";
            lbHotKeyInformation.Size = new System.Drawing.Size(496, 26);
            lbHotKeyInformation.TabIndex = 43;
            lbHotKeyInformation.Text = "ESC, 백스페이바로 비울 수 있습니다.";
            lbHotKeyInformation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbHotKeyQuickOCR
            // 
            lbHotKeyQuickOCR.AutoSize = true;
            lbHotKeyQuickOCR.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            lbHotKeyQuickOCR.ForeColor = System.Drawing.Color.White;
            lbHotKeyQuickOCR.Location = new System.Drawing.Point(14, 91);
            lbHotKeyQuickOCR.Name = "lbHotKeyQuickOCR";
            lbHotKeyQuickOCR.Size = new System.Drawing.Size(104, 17);
            lbHotKeyQuickOCR.TabIndex = 29;
            lbHotKeyQuickOCR.Text = "빠른 OCR 영역 :";
            // 
            // lbHotKeyDic
            // 
            lbHotKeyDic.AutoSize = true;
            lbHotKeyDic.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            lbHotKeyDic.ForeColor = System.Drawing.Color.White;
            lbHotKeyDic.Location = new System.Drawing.Point(14, 64);
            lbHotKeyDic.Name = "lbHotKeyDic";
            lbHotKeyDic.Size = new System.Drawing.Size(109, 17);
            lbHotKeyDic.TabIndex = 27;
            lbHotKeyDic.Text = "교정 사전 열기 : ";
            // 
            // lbHotKeyDoTrans
            // 
            lbHotKeyDoTrans.AutoSize = true;
            lbHotKeyDoTrans.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            lbHotKeyDoTrans.ForeColor = System.Drawing.Color.White;
            lbHotKeyDoTrans.Location = new System.Drawing.Point(14, 37);
            lbHotKeyDoTrans.Name = "lbHotKeyDoTrans";
            lbHotKeyDoTrans.Size = new System.Drawing.Size(109, 17);
            lbHotKeyDoTrans.TabIndex = 25;
            lbHotKeyDoTrans.Text = "번역 시작/중지 : ";
            // 
            // lbHotkey
            // 
            lbHotkey.AutoSize = true;
            lbHotkey.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbHotkey.ForeColor = System.Drawing.Color.White;
            lbHotkey.Location = new System.Drawing.Point(4, 3);
            lbHotkey.Name = "lbHotkey";
            lbHotkey.Size = new System.Drawing.Size(54, 20);
            lbHotkey.TabIndex = 8;
            lbHotkey.Text = "단축키";
            // 
            // tpQuickSetting
            // 
            tpQuickSetting.Controls.Add(panel28);
            tpQuickSetting.Location = new System.Drawing.Point(80, 4);
            tpQuickSetting.Name = "tpQuickSetting";
            tpQuickSetting.Size = new System.Drawing.Size(540, 585);
            tpQuickSetting.TabIndex = 6;
            tpQuickSetting.Text = "빠른설정";
            tpQuickSetting.UseVisualStyleBackColor = true;
            // 
            // panel28
            // 
            panel28.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel28.Controls.Add(panel31);
            panel28.Dock = System.Windows.Forms.DockStyle.Fill;
            panel28.Location = new System.Drawing.Point(0, 0);
            panel28.Margin = new System.Windows.Forms.Padding(0);
            panel28.Name = "panel28";
            panel28.Size = new System.Drawing.Size(540, 585);
            panel28.TabIndex = 3;
            // 
            // panel31
            // 
            panel31.Controls.Add(cbSetBasicDefaultPage);
            panel31.Controls.Add(btQuickJap);
            panel31.Controls.Add(lbQuickSettingInformation);
            panel31.Controls.Add(btQucickEnglish);
            panel31.Controls.Add(lbQuickSetting);
            panel31.Location = new System.Drawing.Point(3, 3);
            panel31.Name = "panel31";
            panel31.Size = new System.Drawing.Size(533, 555);
            panel31.TabIndex = 55;
            panel31.Paint += panealBorder_Paint;
            // 
            // cbSetBasicDefaultPage
            // 
            cbSetBasicDefaultPage.AutoSize = true;
            cbSetBasicDefaultPage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbSetBasicDefaultPage.ForeColor = System.Drawing.Color.White;
            cbSetBasicDefaultPage.Location = new System.Drawing.Point(20, 301);
            cbSetBasicDefaultPage.Name = "cbSetBasicDefaultPage";
            cbSetBasicDefaultPage.Size = new System.Drawing.Size(229, 21);
            cbSetBasicDefaultPage.TabIndex = 18;
            cbSetBasicDefaultPage.Text = "기본설정 탭을 시작 화면으로 설정";
            cbSetBasicDefaultPage.UseVisualStyleBackColor = true;
            // 
            // btQuickJap
            // 
            btQuickJap.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btQuickJap.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btQuickJap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btQuickJap.ForeColor = System.Drawing.Color.White;
            btQuickJap.Location = new System.Drawing.Point(20, 165);
            btQuickJap.Name = "btQuickJap";
            btQuickJap.Size = new System.Drawing.Size(493, 61);
            btQuickJap.TabIndex = 12;
            btQuickJap.Text = "일본어 게임";
            btQuickJap.UseVisualStyleBackColor = false;
            btQuickJap.Click += OnClickQuickJap;
            // 
            // lbQuickSettingInformation
            // 
            lbQuickSettingInformation.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            lbQuickSettingInformation.ForeColor = System.Drawing.Color.White;
            lbQuickSettingInformation.Location = new System.Drawing.Point(20, 259);
            lbQuickSettingInformation.Name = "lbQuickSettingInformation";
            lbQuickSettingInformation.Size = new System.Drawing.Size(490, 23);
            lbQuickSettingInformation.TabIndex = 11;
            lbQuickSettingInformation.Text = "처음 사용자를 위한 설정값을 불러옵니다";
            lbQuickSettingInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btQucickEnglish
            // 
            btQucickEnglish.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btQucickEnglish.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btQucickEnglish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btQucickEnglish.ForeColor = System.Drawing.Color.White;
            btQucickEnglish.Location = new System.Drawing.Point(20, 75);
            btQucickEnglish.Name = "btQucickEnglish";
            btQucickEnglish.Size = new System.Drawing.Size(493, 61);
            btQucickEnglish.TabIndex = 10;
            btQucickEnglish.Text = "영문 게임";
            btQucickEnglish.UseVisualStyleBackColor = false;
            btQucickEnglish.Click += OnClickQucickEnglish;
            // 
            // lbQuickSetting
            // 
            lbQuickSetting.AutoSize = true;
            lbQuickSetting.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbQuickSetting.ForeColor = System.Drawing.Color.White;
            lbQuickSetting.Location = new System.Drawing.Point(4, 3);
            lbQuickSetting.Name = "lbQuickSetting";
            lbQuickSetting.Size = new System.Drawing.Size(191, 20);
            lbQuickSetting.TabIndex = 8;
            lbQuickSetting.Text = "어느 게임을 번역하시나요?";
            // 
            // tpDebuging
            // 
            tpDebuging.Controls.Add(panel24);
            tpDebuging.Location = new System.Drawing.Point(80, 4);
            tpDebuging.Margin = new System.Windows.Forms.Padding(0);
            tpDebuging.Name = "tpDebuging";
            tpDebuging.Size = new System.Drawing.Size(540, 585);
            tpDebuging.TabIndex = 5;
            tpDebuging.Text = "디버깅";
            tpDebuging.UseVisualStyleBackColor = true;
            // 
            // panel24
            // 
            panel24.BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            panel24.Controls.Add(panel26);
            panel24.Dock = System.Windows.Forms.DockStyle.Fill;
            panel24.Location = new System.Drawing.Point(0, 0);
            panel24.Margin = new System.Windows.Forms.Padding(0);
            panel24.Name = "panel24";
            panel24.Size = new System.Drawing.Size(540, 585);
            panel24.TabIndex = 3;
            // 
            // panel26
            // 
            panel26.Controls.Add(plDebugOn);
            panel26.Controls.Add(plDebugOff);
            panel26.Controls.Add(lbDebugging);
            panel26.Location = new System.Drawing.Point(3, 3);
            panel26.Name = "panel26";
            panel26.Size = new System.Drawing.Size(533, 555);
            panel26.TabIndex = 37;
            panel26.Paint += panealBorder_Paint;
            // 
            // plDebugOn
            // 
            plDebugOn.Controls.Add(cbDisplayLowQuality);
            plDebugOn.Controls.Add(cbSaveAnalysisResult);
            plDebugOn.Controls.Add(cbShowOverlayWordArea);
            plDebugOn.Controls.Add(cbSetLineTrans);
            plDebugOn.Controls.Add(btClearFormerResult);
            plDebugOn.Controls.Add(cbShowFormerLog);
            plDebugOn.Controls.Add(cbUnlockSpeed);
            plDebugOn.Controls.Add(cbSaveCaptureResult);
            plDebugOn.Controls.Add(cbSaveCapture);
            plDebugOn.Controls.Add(cbShowReplace);
            plDebugOn.Location = new System.Drawing.Point(3, 26);
            plDebugOn.Name = "plDebugOn";
            plDebugOn.Size = new System.Drawing.Size(507, 512);
            plDebugOn.TabIndex = 56;
            //
            // cbDisplayLowQuality
            //
            cbDisplayLowQuality.AutoSize = true;
            cbDisplayLowQuality.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbDisplayLowQuality.ForeColor = System.Drawing.Color.White;
            cbDisplayLowQuality.Location = new System.Drawing.Point(14, 267);
            cbDisplayLowQuality.Name = "cbDisplayLowQuality";
            cbDisplayLowQuality.Size = new System.Drawing.Size(222, 21);
            cbDisplayLowQuality.TabIndex = 30;
            cbDisplayLowQuality.Text = "번역 결과에 [저품질] 표시하기";
            cbDisplayLowQuality.UseVisualStyleBackColor = true;
            cbDisplayLowQuality.CheckedChanged += cbDisplayLowQuality_CheckedChanged;
            //
            // cbSaveAnalysisResult
            //
            cbSaveAnalysisResult.AutoSize = true;
            cbSaveAnalysisResult.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbSaveAnalysisResult.ForeColor = System.Drawing.Color.White;
            cbSaveAnalysisResult.Location = new System.Drawing.Point(14, 240);
            cbSaveAnalysisResult.Name = "cbSaveAnalysisResult";
            cbSaveAnalysisResult.Size = new System.Drawing.Size(383, 21);
            cbSaveAnalysisResult.TabIndex = 29;
            cbSaveAnalysisResult.Text = "이미지 인식 결과 저장 - UserData/Debug/OcrAnalysis/*.json";
            cbSaveAnalysisResult.UseVisualStyleBackColor = true;
            cbSaveAnalysisResult.CheckedChanged += cbSaveAnalysisResult_CheckedChanged;
            //
            // cbShowOverlayWordArea
            //
            cbShowOverlayWordArea.AutoSize = true;
            cbShowOverlayWordArea.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbShowOverlayWordArea.ForeColor = System.Drawing.Color.White;
            cbShowOverlayWordArea.Location = new System.Drawing.Point(14, 213);
            cbShowOverlayWordArea.Name = "cbShowOverlayWordArea";
            cbShowOverlayWordArea.Size = new System.Drawing.Size(239, 21);
            cbShowOverlayWordArea.TabIndex = 28;
            cbShowOverlayWordArea.Text = "오버레이 번역창 - 문자 영역 보이기";
            cbShowOverlayWordArea.UseVisualStyleBackColor = true;
            cbShowOverlayWordArea.CheckedChanged += cbShowOverlayWordArea_CheckedChanged;
            // 
            // cbSetLineTrans
            // 
            cbSetLineTrans.AutoSize = true;
            cbSetLineTrans.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbSetLineTrans.ForeColor = System.Drawing.Color.White;
            cbSetLineTrans.Location = new System.Drawing.Point(14, 186);
            cbSetLineTrans.Name = "cbSetLineTrans";
            cbSetLineTrans.Size = new System.Drawing.Size(345, 21);
            cbSetLineTrans.TabIndex = 27;
            cbSetLineTrans.Text = "줄 단위로 번역하기 (문장단위로 번역하기 사용 안 함)";
            cbSetLineTrans.UseVisualStyleBackColor = true;
            cbSetLineTrans.CheckedChanged += cbSetLineTrans_CheckedChanged;
            // 
            // btClearFormerResult
            // 
            btClearFormerResult.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btClearFormerResult.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btClearFormerResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btClearFormerResult.ForeColor = System.Drawing.Color.White;
            btClearFormerResult.Location = new System.Drawing.Point(14, 155);
            btClearFormerResult.Name = "btClearFormerResult";
            btClearFormerResult.Size = new System.Drawing.Size(487, 25);
            btClearFormerResult.TabIndex = 26;
            btClearFormerResult.Text = "번역 기억하기 모두 삭제";
            btClearFormerResult.UseVisualStyleBackColor = false;
            btClearFormerResult.Click += btClearFormerResult_Click;
            // 
            // cbShowFormerLog
            // 
            cbShowFormerLog.AutoSize = true;
            cbShowFormerLog.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbShowFormerLog.ForeColor = System.Drawing.Color.White;
            cbShowFormerLog.Location = new System.Drawing.Point(14, 128);
            cbShowFormerLog.Name = "cbShowFormerLog";
            cbShowFormerLog.Size = new System.Drawing.Size(172, 21);
            cbShowFormerLog.TabIndex = 15;
            cbShowFormerLog.Text = "번역 기억하기 결과 출력";
            cbShowFormerLog.UseVisualStyleBackColor = true;
            cbShowFormerLog.CheckedChanged += cbShowFormerLog_CheckedChanged;
            // 
            // cbUnlockSpeed
            // 
            cbUnlockSpeed.AutoSize = true;
            cbUnlockSpeed.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbUnlockSpeed.ForeColor = System.Drawing.Color.White;
            cbUnlockSpeed.Location = new System.Drawing.Point(14, 101);
            cbUnlockSpeed.Name = "cbUnlockSpeed";
            cbUnlockSpeed.Size = new System.Drawing.Size(146, 21);
            cbUnlockSpeed.TabIndex = 14;
            cbUnlockSpeed.Text = "번역 속도 제한 해제";
            cbUnlockSpeed.UseVisualStyleBackColor = true;
            cbUnlockSpeed.CheckedChanged += cbUnlockSpeed_CheckedChanged;
            // 
            // cbSaveCaptureResult
            // 
            cbSaveCaptureResult.AutoSize = true;
            cbSaveCaptureResult.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbSaveCaptureResult.ForeColor = System.Drawing.Color.White;
            cbSaveCaptureResult.Location = new System.Drawing.Point(14, 74);
            cbSaveCaptureResult.Name = "cbSaveCaptureResult";
            cbSaveCaptureResult.Size = new System.Drawing.Size(321, 21);
            cbSaveCaptureResult.TabIndex = 13;
            cbSaveCaptureResult.Text = "이미지 캡쳐 보정 결과 저장 - captue_Result.bmp";
            cbSaveCaptureResult.UseVisualStyleBackColor = true;
            // 
            // cbSaveCapture
            // 
            cbSaveCapture.AutoSize = true;
            cbSaveCapture.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbSaveCapture.ForeColor = System.Drawing.Color.White;
            cbSaveCapture.Location = new System.Drawing.Point(14, 47);
            cbSaveCapture.Name = "cbSaveCapture";
            cbSaveCapture.Size = new System.Drawing.Size(302, 21);
            cbSaveCapture.TabIndex = 12;
            cbSaveCapture.Text = "이미지 캡쳐 원본 저장 - captue_Original.bmp";
            cbSaveCapture.UseVisualStyleBackColor = true;
            // 
            // cbShowReplace
            // 
            cbShowReplace.AutoSize = true;
            cbShowReplace.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            cbShowReplace.ForeColor = System.Drawing.Color.White;
            cbShowReplace.Location = new System.Drawing.Point(14, 20);
            cbShowReplace.Name = "cbShowReplace";
            cbShowReplace.Size = new System.Drawing.Size(141, 21);
            cbShowReplace.TabIndex = 11;
            cbShowReplace.Text = "교정사전 결과 표시";
            cbShowReplace.UseVisualStyleBackColor = true;
            // 
            // plDebugOff
            // 
            plDebugOff.Controls.Add(label63);
            plDebugOff.Controls.Add(btnDebugOn);
            plDebugOff.Location = new System.Drawing.Point(6, 188);
            plDebugOff.Name = "plDebugOff";
            plDebugOff.Size = new System.Drawing.Size(501, 214);
            plDebugOff.TabIndex = 57;
            plDebugOff.Visible = false;
            // 
            // label63
            // 
            label63.Anchor = System.Windows.Forms.AnchorStyles.None;
            label63.AutoSize = true;
            label63.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            label63.ForeColor = System.Drawing.Color.White;
            label63.Location = new System.Drawing.Point(52, 20);
            label63.Name = "label63";
            label63.Size = new System.Drawing.Size(400, 68);
            label63.TabIndex = 43;
            label63.Text = "MORT 디버깅 기능을 활성화 합니다.\r\n개발, 진단용 기능이기 때문에 평상시에는 사용할 필요가 없습니다.\r\n\r\n※ 디버깅을 활성화 했을 시 성능에 영향을 줄 수 있습니다.\r\n";
            label63.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnDebugOn
            // 
            btnDebugOn.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnDebugOn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            btnDebugOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDebugOn.ForeColor = System.Drawing.Color.White;
            btnDebugOn.Location = new System.Drawing.Point(212, 158);
            btnDebugOn.Name = "btnDebugOn";
            btnDebugOn.Size = new System.Drawing.Size(56, 23);
            btnDebugOn.TabIndex = 46;
            btnDebugOn.Text = "활성화";
            btnDebugOn.UseVisualStyleBackColor = false;
            btnDebugOn.Click += OnClick_DebugOn;
            // 
            // lbDebugging
            // 
            lbDebugging.AutoSize = true;
            lbDebugging.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            lbDebugging.ForeColor = System.Drawing.Color.White;
            lbDebugging.Location = new System.Drawing.Point(4, 3);
            lbDebugging.Name = "lbDebugging";
            lbDebugging.Size = new System.Drawing.Size(54, 20);
            lbDebugging.TabIndex = 8;
            lbDebugging.Text = "디버깅";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = System.Drawing.Color.FromArgb(35, 36, 38);
            ClientSize = new System.Drawing.Size(624, 658);
            Controls.Add(donationButton);
            Controls.Add(pictureBox1);
            Controls.Add(acceptButton);
            Controls.Add(tbMain);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Monkeyhead's OCR RealTime Translator ";
            FormClosing += Form1_FormClosing;
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            Shown += Form1_Shown;
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            ContextOption.ResumeLayout(false);
            optionMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tbMain.ResumeLayout(false);
            tpBasic.ResumeLayout(false);
            panel8.ResumeLayout(false);
            pnAdjustImg.ResumeLayout(false);
            pnAdjustImg.PerformLayout();
            pnOCR.ResumeLayout(false);
            pnOCR.PerformLayout();
            pnNHocr.ResumeLayout(false);
            pnNHocr.PerformLayout();
            WinOCR_panel.ResumeLayout(false);
            WinOCR_panel.PerformLayout();
            pnEasyOcr.ResumeLayout(false);
            pnEasyOcr.PerformLayout();
            Tesseract_panel.ResumeLayout(false);
            Tesseract_panel.PerformLayout();
            pnGoogleOcr.ResumeLayout(false);
            pnGoogleOcr.PerformLayout();
            pnTranslate.ResumeLayout(false);
            pnTranslate.PerformLayout();
            pnGemini.ResumeLayout(false);
            pnGemini.PerformLayout();
            Naver_Panel.ResumeLayout(false);
            Naver_Panel.PerformLayout();
            Google_Panel.ResumeLayout(false);
            Google_Panel.PerformLayout();
            pnEzTrans.ResumeLayout(false);
            pnEzTrans.PerformLayout();
            pnPapagoWeb.ResumeLayout(false);
            pnGoogleBasic.ResumeLayout(false);
            pnCustomApi.ResumeLayout(false);
            pnDeepLAPI.ResumeLayout(false);
            pnDeepLAPI.PerformLayout();
            pnDeepl.ResumeLayout(false);
            pnDeepl.PerformLayout();
            DB_Panel.ResumeLayout(false);
            DB_Panel.PerformLayout();
            tpText.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel17.ResumeLayout(false);
            panel17.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)backgroundColorBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)outlineColor2Box).EndInit();
            ((System.ComponentModel.ISupportInitialize)outlineColor1Box).EndInit();
            ((System.ComponentModel.ISupportInitialize)textColorBox).EndInit();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)fontSizeUpDown).EndInit();
            tpExtra.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel21.ResumeLayout(false);
            panel21.PerformLayout();
            panel25.ResumeLayout(false);
            panel25.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panel12.ResumeLayout(false);
            panel12.PerformLayout();
            panel14.ResumeLayout(false);
            panel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imgZoomsizeUpDown).EndInit();
            tpTranslation.ResumeLayout(false);
            panel19.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel27.ResumeLayout(false);
            panel27.PerformLayout();
            panel22.ResumeLayout(false);
            panel22.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            tpETC.ResumeLayout(false);
            panel18.ResumeLayout(false);
            panel16.ResumeLayout(false);
            panel16.PerformLayout();
            panel20.ResumeLayout(false);
            panel20.PerformLayout();
            panel23.ResumeLayout(false);
            panel23.PerformLayout();
            panel2.ResumeLayout(false);
            tpQuickSetting.ResumeLayout(false);
            panel28.ResumeLayout(false);
            panel31.ResumeLayout(false);
            panel31.PerformLayout();
            tpDebuging.ResumeLayout(false);
            panel24.ResumeLayout(false);
            panel26.ResumeLayout(false);
            panel26.PerformLayout();
            plDebugOn.ResumeLayout(false);
            plDebugOn.PerformLayout();
            plDebugOff.ResumeLayout(false);
            plDebugOff.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip ContextOption;
        private System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.TextBox bTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox gTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox rTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox v1TextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox s1TextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkHSV;
        private System.Windows.Forms.CheckBox checkRGB;
        private System.Windows.Forms.CheckBox checkErode;
        private System.Windows.Forms.TextBox v2TextBox;
        private System.Windows.Forms.TextBox s2TextBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox showOcrCheckBox;
        private System.Windows.Forms.ToolStripMenuItem setCutPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem showTransToolStripMenuItem;
        private System.Windows.Forms.CheckBox saveOCRCheckBox;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.Panel pnOCR;
        private System.Windows.Forms.Label ocrLabel;
        private System.Windows.Forms.Panel pnTranslate;
        private System.Windows.Forms.Label lbTransTypeTitle;
        private System.Windows.Forms.Panel pnAdjustImg;
        private System.Windows.Forms.Label lbAdjustImg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem rTTToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem setTranslateTopMostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkUpdateToolStripMenuItem;
        private System.Windows.Forms.CheckBox isClipBoardcheckBox1;
        private System.Windows.Forms.ToolStripMenuItem setCheckSpellingToolStripMenuItem;
        private System.Windows.Forms.Label lbImgGroup;
        private System.Windows.Forms.ComboBox groupCombo;
        private System.Windows.Forms.Label groupLabel;
        private System.Windows.Forms.Label lbImgGroupCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ContextMenuStrip optionMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 설정저장ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 설정불러오기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingSaveToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem settingLoadToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem settingDefaultToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkDic;
        private System.Windows.Forms.TextBox dicFileTextBox;
        private System.Windows.Forms.Label lbDicFile;
        private Dotnetrix_Samples.TabControl tbMain;
        private System.Windows.Forms.TabPage tpBasic;
        private System.Windows.Forms.TabPage tpText;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lbFontSetting;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TabPage tpExtra;
        private System.Windows.Forms.Button fontButton;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label lbTextAdditionalSettings;
        private System.Windows.Forms.Label lbFontSize;
        private System.Windows.Forms.Label lbFont;
        private System.Windows.Forms.CheckBox alignmentCenterCheckBox;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label lbFontColor;
        private System.Windows.Forms.CheckBox removeSpaceCheckBox;
        private System.Windows.Forms.Label lbFontOutlineColor2;
        private System.Windows.Forms.Label lbFontOutlineColor1;
        private System.Windows.Forms.PictureBox backgroundColorBox;
        private System.Windows.Forms.PictureBox outlineColor2Box;
        private System.Windows.Forms.PictureBox outlineColor1Box;
        private System.Windows.Forms.Label lbFontBasicColor;
        private System.Windows.Forms.PictureBox textColorBox;
        private System.Windows.Forms.CheckBox useBackColorCheckBox;
        private System.Windows.Forms.Label lbFontBackground;
        private System.Windows.Forms.NumericUpDown fontSizeUpDown;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button defaultColorButton;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label lbImgCapture;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.CheckBox checkUpdateCheckBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox activeWinodeCheckBox;
        private System.Windows.Forms.CheckBox topMostcheckBox;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Button openConfigButton;
        private System.Windows.Forms.Label lbSettingFile;
        private CustomLabel fontResultLabel;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Label lbPreview;
        private System.Windows.Forms.Button defaultButton;
        private System.Windows.Forms.Button saveConfigButton;
        private System.Windows.Forms.Label lbImgZoom;
        private System.Windows.Forms.Button SetDefaultZoomSizeButton;
        private System.Windows.Forms.NumericUpDown imgZoomsizeUpDown;
        private System.Windows.Forms.TabPage tpETC;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.Button error_Information_Button;
        private System.Windows.Forms.Button help_Button;
        private System.Windows.Forms.Label lbETC;
        private System.Windows.Forms.Panel panel23;
        private System.Windows.Forms.Label lbHotKeyDoTrans;
        private System.Windows.Forms.Label lbHotkey;
        private System.Windows.Forms.Button about_Button;
        private System.Windows.Forms.TabPage tpTranslation;
        private System.Windows.Forms.Panel panel19;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton speedRadioButton5;
        private System.Windows.Forms.Label lbSpeed;
        private System.Windows.Forms.RadioButton speedRadioButton4;
        private System.Windows.Forms.RadioButton speedRadioButton1;
        private System.Windows.Forms.RadioButton speedRadioButton3;
        private System.Windows.Forms.RadioButton speedRadioButton2;
        private System.Windows.Forms.Label lbDbFile;
        private System.Windows.Forms.TextBox dbFileTextBox;
        private System.Windows.Forms.Panel DB_Panel;
        private System.Windows.Forms.ComboBox _cbTranslateType;
        private System.Windows.Forms.Label lbTransType;
        private System.Windows.Forms.Panel Naver_Panel;
        private System.Windows.Forms.Label lbPapagoSecret;
        private System.Windows.Forms.TextBox NaverSecretKeyTextBox;
        private System.Windows.Forms.TextBox NaverIDKeyTextBox;
        private System.Windows.Forms.Label lbPapagoID;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.ComboBox naverTransComboBox;
        private System.Windows.Forms.Label lbPaPagoFrom;
        private System.Windows.Forms.Label lbPaPago;
        private KeyInputLabel quickKeyInputLabel;
        private System.Windows.Forms.Label lbHotKeyQuickOCR;
        private KeyInputLabel dicKeyInputLabel;
        private System.Windows.Forms.Label lbHotKeyDic;
        private KeyInputLabel transKeyInputLabel;
        private System.Windows.Forms.Label lbHotKeyInformation;
        private System.Windows.Forms.Button quickKeyInputResetButton;
        private System.Windows.Forms.Button dicKeyInputResetButton;
        private System.Windows.Forms.Button transKeyInputResetButton;
        private System.Windows.Forms.CheckBox checkStringUpper;
        private System.Windows.Forms.Button quickKeyInputEmptyButton;
        private System.Windows.Forms.Button dicKeyInputEmptyButton;
        private System.Windows.Forms.Button transKeyInputEmptyButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton skinLayerRadioButton;
        private System.Windows.Forms.Label lbTransformType;
        private System.Windows.Forms.RadioButton skinDarkRadioButton;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.ComboBox OCR_Type_comboBox;
        private System.Windows.Forms.Panel Tesseract_panel;
        private System.Windows.Forms.Panel WinOCR_panel;
        private System.Windows.Forms.ComboBox WinOCR_Language_comboBox;
        private System.Windows.Forms.Label lbWinOCRLanguage;
        private System.Windows.Forms.ComboBox tesseractLanguageComboBox;
        private System.Windows.Forms.Label lbTesseractLanguage;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tessDataTextBox;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel Google_Panel;
        private System.Windows.Forms.TextBox googleSheet_textBox;
        private System.Windows.Forms.Label lbGoogleSheetAddress;
        private System.Windows.Forms.Panel panel22;
        private System.Windows.Forms.ComboBox googleResultCodeComboBox;
        private System.Windows.Forms.Label lbGoogleTo;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.ComboBox googleTransComboBox;
        private System.Windows.Forms.Label lbGoogle;
        private System.Windows.Forms.Label lbGoogleFrom;
        private System.Windows.Forms.Button button_RemoveAllGoogleToekn;
        private System.Windows.Forms.TextBox textBox_GoogleSecretKey;
        private System.Windows.Forms.Label lbSheetSecret;
        private System.Windows.Forms.TextBox textBox_GoogleClientID;
        private System.Windows.Forms.Label lbSheetID;
        private System.Windows.Forms.Button snapShotKeyInputEmptyButton;
        private System.Windows.Forms.Button snapShotKeyInputResetButton;
        private KeyInputLabel snapShotInputLabel;
        private System.Windows.Forms.Label lbHotKeySnapShot;
        private System.Windows.Forms.Button donationButton;
        private System.Windows.Forms.Button btnOneTransEmpty;
        private System.Windows.Forms.Button btnOneTransDefault;
        private KeyInputLabel lbOneTrans;
        private System.Windows.Forms.Label lbHotKeyOnceTranslate;
        private System.Windows.Forms.Button Button_NaverTransKeyList;
        private System.Windows.Forms.CheckBox cbPerWordDic;
        private System.Windows.Forms.ToolTip toolTip_OCR;
        private System.Windows.Forms.Panel pnGoogleBasic;
        private System.Windows.Forms.Label lbBasicInfo;
        private System.Windows.Forms.Button btnTransHelp;
        private System.Windows.Forms.TabPage tpDebuging;
        private System.Windows.Forms.Panel panel24;
        private System.Windows.Forms.Panel panel26;
        private System.Windows.Forms.Label lbDebugging;
        private System.Windows.Forms.CheckBox cbDisplayLowQuality;
        private System.Windows.Forms.CheckBox cbSaveAnalysisResult;
        private System.Windows.Forms.CheckBox cbShowOCRIndex;
        private System.Windows.Forms.Button btnHideTransEmpty;
        private System.Windows.Forms.Button btnHideTransDefault;
        private KeyInputLabel lbHideTranslate;
        private System.Windows.Forms.Label lbHotKeyHideTransWindow;
        private System.Windows.Forms.Button btnFollowMouseEmpty;
        private System.Windows.Forms.Button btnFollowMouseDefault;
        private KeyInputLabel lbFollowMouse;
        private System.Windows.Forms.Label lbHotKeyFollowMouse;
        private System.Windows.Forms.CheckBox cbFastTess;
        private System.Windows.Forms.Panel plDebugOn;
        private System.Windows.Forms.CheckBox cbShowReplace;
        private System.Windows.Forms.Panel plDebugOff;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Button btnDebugOn;
        private System.Windows.Forms.CheckBox cbSaveCapture;
        private System.Windows.Forms.CheckBox cbSaveCaptureResult;
        private System.Windows.Forms.CheckBox cbDBMultiGet;
        private System.Windows.Forms.ComboBox cbNaverResultCode;
        private System.Windows.Forms.Label lbPaPagoTo;
        private System.Windows.Forms.Label lbSpeedInformation;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Button openBlogButton;
        private System.Windows.Forms.Button btnGitHub;
        private System.Windows.Forms.Label lbLink;
        private System.Windows.Forms.Panel panel25;
        private System.Windows.Forms.Button btSettingUpload;
        private System.Windows.Forms.Button btSettingBrowser;
        private System.Windows.Forms.Label lbSearchConfig;
        private System.Windows.Forms.Panel panel27;
        private System.Windows.Forms.CheckBox cbTTSWaitEnd;
        private System.Windows.Forms.CheckBox cbUseTTS;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.CheckBox cbUnlockSpeed;
        private System.Windows.Forms.CheckBox cbShowFormerLog;
        private System.Windows.Forms.Button btClearFormerResult;
        private System.Windows.Forms.Button btAttachCapture;
        private System.Windows.Forms.RadioButton skinOverRadioButton;
        private System.Windows.Forms.CheckBox cbShowOverlayWordArea;
        private System.Windows.Forms.CheckBox cbSetLineTrans;
        private System.Windows.Forms.Button btnOpenDiscord;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.Button btAdvencedOption;
        private System.Windows.Forms.Label lbAdvencedConfig;
        private System.Windows.Forms.Panel pnEzTrans;
        private System.Windows.Forms.Label lbEzTransInfo;
        private System.Windows.Forms.TabPage tpQuickSetting;
        private System.Windows.Forms.Panel panel28;
        private System.Windows.Forms.Panel panel31;
        private System.Windows.Forms.CheckBox cbSetBasicDefaultPage;
        private System.Windows.Forms.Button btQuickJap;
        private System.Windows.Forms.Label lbQuickSettingInformation;
        private System.Windows.Forms.Button btQucickEnglish;
        private System.Windows.Forms.Label lbQuickSetting;
        private System.Windows.Forms.Button btOcrHelp;
        private System.Windows.Forms.CheckBox cbThreshold;
        private System.Windows.Forms.TextBox tbThreshold;
        private System.Windows.Forms.Button btImgResult;
        private System.Windows.Forms.Panel pnNHocr;
        private System.Windows.Forms.Panel pnGoogleOcr;
        private System.Windows.Forms.Button btnSettingGoogleOCR;
        private System.Windows.Forms.Label lbGoogleOcrStatus;
        private System.Windows.Forms.ComboBox cbGoogleOcrLanguge;
        private System.Windows.Forms.Label lbGoogleOCRLanguage;
        private System.Windows.Forms.Label lbBasicStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnDeepl;
        private System.Windows.Forms.Label lbDeepLStatus;
        private System.Windows.Forms.Label lbDeepLInfo;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cbDeepLLanguageTo;
        private System.Windows.Forms.Label lbDeepLTo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbDeepLLanguage;
        private System.Windows.Forms.Label lbDeepL;
        private System.Windows.Forms.Label lbDeepLFrom;
        private System.Windows.Forms.Button btnCheckDeeplState;
        private System.Windows.Forms.Panel pnCustomApi;
        private System.Windows.Forms.Label lbCustomApiInformation;
        private System.Windows.Forms.Panel pnDeepLAPI;
        private System.Windows.Forms.RadioButton rbDeepLAPIEndpointFree;
        private System.Windows.Forms.Panel pnEasyOcr;
        private System.Windows.Forms.ComboBox cbEasyOcrCode;
        private System.Windows.Forms.Label lbEasyOcrLanguage;
        private System.Windows.Forms.Button btnInstallEasyOcr;
        private System.Windows.Forms.Panel pnPapagoWeb;
        private System.Windows.Forms.Label lbPapagoWebInfo;
        private System.Windows.Forms.Label lbPapagoLanguageCodeInformation;
        private System.Windows.Forms.Button btnAddWinOcrLanguage;
        private System.Windows.Forms.RadioButton rbDeepLAPIEndpointPaid;
        private System.Windows.Forms.TextBox tbDeeplApi;
        private System.Windows.Forms.Label lbDeeplApi;
        private System.Windows.Forms.Panel pnGemini;
        private System.Windows.Forms.ComboBox cbGeminiModel;
        private System.Windows.Forms.Label lbGeminiModel;
        private System.Windows.Forms.TextBox tbGeminiApi;
        private System.Windows.Forms.Label lbGeminiApi;
        private System.Windows.Forms.ComboBox cbOneOcrLanguage;
        private System.Windows.Forms.Label lbOneOcrLanguage;
    }




}

