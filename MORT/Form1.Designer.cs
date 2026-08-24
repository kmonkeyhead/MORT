
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
            if (disposing && (components != null))
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ContextOption = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTransToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rTTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.setTranslateTopMostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setCheckSpellingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setCutPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.transToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingSaveToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingLoadToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.설정저장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.설정불러오기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.acceptButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.donationButton = new System.Windows.Forms.Button();
            this.toolTip_OCR = new System.Windows.Forms.ToolTip(this.components);
            this.tbMain = new Dotnetrix_Samples.TabControl();
            this.tpBasic = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.pnAdjustImg = new System.Windows.Forms.Panel();
            this.btImgResult = new System.Windows.Forms.Button();
            this.cbThreshold = new System.Windows.Forms.CheckBox();
            this.tbThreshold = new System.Windows.Forms.TextBox();
            this.groupCombo = new System.Windows.Forms.ComboBox();
            this.groupLabel = new System.Windows.Forms.Label();
            this.lbImgGroupCount = new System.Windows.Forms.Label();
            this.lbImgGroup = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.v2TextBox = new System.Windows.Forms.TextBox();
            this.lbAdjustImg = new System.Windows.Forms.Label();
            this.checkRGB = new System.Windows.Forms.CheckBox();
            this.s2TextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.rTextBox = new System.Windows.Forms.TextBox();
            this.checkErode = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkHSV = new System.Windows.Forms.CheckBox();
            this.gTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.v1TextBox = new System.Windows.Forms.TextBox();
            this.bTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.s1TextBox = new System.Windows.Forms.TextBox();
            this.pnOCR = new System.Windows.Forms.Panel();
            this.btOcrHelp = new System.Windows.Forms.Button();
            this.label48 = new System.Windows.Forms.Label();
            this.OCR_Type_comboBox = new System.Windows.Forms.ComboBox();
            this.isClipBoardcheckBox1 = new System.Windows.Forms.CheckBox();
            this.saveOCRCheckBox = new System.Windows.Forms.CheckBox();
            this.ocrLabel = new System.Windows.Forms.Label();
            this.showOcrCheckBox = new System.Windows.Forms.CheckBox();
            this.Tesseract_panel = new System.Windows.Forms.Panel();
            this.cbFastTess = new System.Windows.Forms.CheckBox();
            this.tesseractLanguageComboBox = new System.Windows.Forms.ComboBox();
            this.lbTesseractLanguage = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tessDataTextBox = new System.Windows.Forms.TextBox();
            this.pnGoogleOcr = new System.Windows.Forms.Panel();
            this.cbGoogleOcrLanguge = new System.Windows.Forms.ComboBox();
            this.lbGoogleOCRLanguage = new System.Windows.Forms.Label();
            this.lbGoogleOcrStatus = new System.Windows.Forms.Label();
            this.btnSettingGoogleOCR = new System.Windows.Forms.Button();
            this.pnNHocr = new System.Windows.Forms.Panel();
            this.lbNHOcrInfo = new System.Windows.Forms.Label();
            this.WinOCR_panel = new System.Windows.Forms.Panel();
            this.WinOCR_Language_comboBox = new System.Windows.Forms.ComboBox();
            this.lbWinOCRLanguage = new System.Windows.Forms.Label();
            this.pnTranslate = new System.Windows.Forms.Panel();
            this.btnTransHelp = new System.Windows.Forms.Button();
            this.cbPerWordDic = new System.Windows.Forms.CheckBox();
            this.lbTransType = new System.Windows.Forms.Label();
            this.TransType_Combobox = new System.Windows.Forms.ComboBox();
            this.checkDic = new System.Windows.Forms.CheckBox();
            this.dicFileTextBox = new System.Windows.Forms.TextBox();
            this.lbDicFile = new System.Windows.Forms.Label();
            this.lbTransTypeTitle = new System.Windows.Forms.Label();
            this.pnDeepl = new System.Windows.Forms.Panel();
            this.btnCheckDeeplState = new System.Windows.Forms.Button();
            this.lbDeepLStatus = new System.Windows.Forms.Label();
            this.lbDeepLInfo = new System.Windows.Forms.Label();
            this.pnGoogleBasic = new System.Windows.Forms.Panel();
            this.lbBasicStatus = new System.Windows.Forms.Label();
            this.lbBasicInfo = new System.Windows.Forms.Label();
            this.DB_Panel = new System.Windows.Forms.Panel();
            this.cbDBMultiGet = new System.Windows.Forms.CheckBox();
            this.checkStringUpper = new System.Windows.Forms.CheckBox();
            this.dbFileTextBox = new System.Windows.Forms.TextBox();
            this.lbDbFile = new System.Windows.Forms.Label();
            this.Naver_Panel = new System.Windows.Forms.Panel();
            this.Button_NaverTransKeyList = new System.Windows.Forms.Button();
            this.lbPapagoSecret = new System.Windows.Forms.Label();
            this.NaverSecretKeyTextBox = new System.Windows.Forms.TextBox();
            this.NaverIDKeyTextBox = new System.Windows.Forms.TextBox();
            this.lbPapagoID = new System.Windows.Forms.Label();
            this.Google_Panel = new System.Windows.Forms.Panel();
            this.button_RemoveAllGoogleToekn = new System.Windows.Forms.Button();
            this.textBox_GoogleSecretKey = new System.Windows.Forms.TextBox();
            this.lbSheetSecret = new System.Windows.Forms.Label();
            this.textBox_GoogleClientID = new System.Windows.Forms.TextBox();
            this.lbSheetID = new System.Windows.Forms.Label();
            this.googleSheet_textBox = new System.Windows.Forms.TextBox();
            this.lbGoogleSheetAddress = new System.Windows.Forms.Label();
            this.pnEzTrans = new System.Windows.Forms.Panel();
            this.lbEzTransInfo = new System.Windows.Forms.Label();
            this.tpText = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel17 = new System.Windows.Forms.Panel();
            this.lbPreview = new System.Windows.Forms.Label();
            this.fontResultLabel = new MORT.CustomLabel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.defaultColorButton = new System.Windows.Forms.Button();
            this.lbFontBackground = new System.Windows.Forms.Label();
            this.lbFontOutlineColor2 = new System.Windows.Forms.Label();
            this.lbFontOutlineColor1 = new System.Windows.Forms.Label();
            this.backgroundColorBox = new System.Windows.Forms.PictureBox();
            this.outlineColor2Box = new System.Windows.Forms.PictureBox();
            this.outlineColor1Box = new System.Windows.Forms.PictureBox();
            this.lbFontBasicColor = new System.Windows.Forms.Label();
            this.textColorBox = new System.Windows.Forms.PictureBox();
            this.lbFontColor = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.cbShowOCRIndex = new System.Windows.Forms.CheckBox();
            this.useBackColorCheckBox = new System.Windows.Forms.CheckBox();
            this.removeSpaceCheckBox = new System.Windows.Forms.CheckBox();
            this.alignmentCenterCheckBox = new System.Windows.Forms.CheckBox();
            this.lbTextAdditionalSettings = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.fontSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.lbFontSize = new System.Windows.Forms.Label();
            this.lbFont = new System.Windows.Forms.Label();
            this.fontButton = new System.Windows.Forms.Button();
            this.lbFontSetting = new System.Windows.Forms.Label();
            this.tpExtra = new System.Windows.Forms.TabPage();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel21 = new System.Windows.Forms.Panel();
            this.btAdvencedOption = new System.Windows.Forms.Button();
            this.lbAdvencedConfig = new System.Windows.Forms.Label();
            this.panel25 = new System.Windows.Forms.Panel();
            this.btSettingUpload = new System.Windows.Forms.Button();
            this.btSettingBrowser = new System.Windows.Forms.Button();
            this.lbSearchConfig = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.speedRadioButton5 = new System.Windows.Forms.RadioButton();
            this.lbSpeed = new System.Windows.Forms.Label();
            this.lbSpeedInformation = new System.Windows.Forms.Label();
            this.speedRadioButton4 = new System.Windows.Forms.RadioButton();
            this.speedRadioButton1 = new System.Windows.Forms.RadioButton();
            this.speedRadioButton3 = new System.Windows.Forms.RadioButton();
            this.speedRadioButton2 = new System.Windows.Forms.RadioButton();
            this.panel13 = new System.Windows.Forms.Panel();
            this.defaultButton = new System.Windows.Forms.Button();
            this.saveConfigButton = new System.Windows.Forms.Button();
            this.openConfigButton = new System.Windows.Forms.Button();
            this.lbSettingFile = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.topMostcheckBox = new System.Windows.Forms.CheckBox();
            this.checkUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.btAttachCapture = new System.Windows.Forms.Button();
            this.SetDefaultZoomSizeButton = new System.Windows.Forms.Button();
            this.imgZoomsizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.lbImgZoom = new System.Windows.Forms.Label();
            this.activeWinodeCheckBox = new System.Windows.Forms.CheckBox();
            this.lbImgCapture = new System.Windows.Forms.Label();
            this.tpTranslation = new System.Windows.Forms.TabPage();
            this.panel19 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbDeepLLanguageTo = new System.Windows.Forms.ComboBox();
            this.lbDeepLTo = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbDeepLLanguage = new System.Windows.Forms.ComboBox();
            this.lbDeepL = new System.Windows.Forms.Label();
            this.lbDeepLFrom = new System.Windows.Forms.Label();
            this.panel27 = new System.Windows.Forms.Panel();
            this.cbTTSWaitEnd = new System.Windows.Forms.CheckBox();
            this.cbUseTTS = new System.Windows.Forms.CheckBox();
            this.label66 = new System.Windows.Forms.Label();
            this.panel22 = new System.Windows.Forms.Panel();
            this.googleResultCodeComboBox = new System.Windows.Forms.ComboBox();
            this.lbGoogleTo = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.googleTransComboBox = new System.Windows.Forms.ComboBox();
            this.lbGoogle = new System.Windows.Forms.Label();
            this.lbGoogleFrom = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.skinOverRadioButton = new System.Windows.Forms.RadioButton();
            this.skinLayerRadioButton = new System.Windows.Forms.RadioButton();
            this.lbTransformType = new System.Windows.Forms.Label();
            this.skinDarkRadioButton = new System.Windows.Forms.RadioButton();
            this.panel15 = new System.Windows.Forms.Panel();
            this.cbNaverResultCode = new System.Windows.Forms.ComboBox();
            this.lbPaPagoTo = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.naverTransComboBox = new System.Windows.Forms.ComboBox();
            this.lbPaPago = new System.Windows.Forms.Label();
            this.lbPaPagoFrom = new System.Windows.Forms.Label();
            this.tpETC = new System.Windows.Forms.TabPage();
            this.panel18 = new System.Windows.Forms.Panel();
            this.panel16 = new System.Windows.Forms.Panel();
            this.btnOpenDiscord = new System.Windows.Forms.Button();
            this.openBlogButton = new System.Windows.Forms.Button();
            this.btnGitHub = new System.Windows.Forms.Button();
            this.lbLink = new System.Windows.Forms.Label();
            this.panel20 = new System.Windows.Forms.Panel();
            this.about_Button = new System.Windows.Forms.Button();
            this.error_Information_Button = new System.Windows.Forms.Button();
            this.help_Button = new System.Windows.Forms.Button();
            this.lbETC = new System.Windows.Forms.Label();
            this.panel23 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.transKeyInputLabel = new MORT.KeyInputLabel();
            this.btnHideTransEmpty = new System.Windows.Forms.Button();
            this.dicKeyInputLabel = new MORT.KeyInputLabel();
            this.btnHideTransDefault = new System.Windows.Forms.Button();
            this.quickKeyInputLabel = new MORT.KeyInputLabel();
            this.lbHideTranslate = new MORT.KeyInputLabel();
            this.transKeyInputResetButton = new System.Windows.Forms.Button();
            this.dicKeyInputResetButton = new System.Windows.Forms.Button();
            this.btnOneTransEmpty = new System.Windows.Forms.Button();
            this.quickKeyInputResetButton = new System.Windows.Forms.Button();
            this.btnOneTransDefault = new System.Windows.Forms.Button();
            this.transKeyInputEmptyButton = new System.Windows.Forms.Button();
            this.lbOneTrans = new MORT.KeyInputLabel();
            this.dicKeyInputEmptyButton = new System.Windows.Forms.Button();
            this.quickKeyInputEmptyButton = new System.Windows.Forms.Button();
            this.snapShotKeyInputEmptyButton = new System.Windows.Forms.Button();
            this.snapShotInputLabel = new MORT.KeyInputLabel();
            this.snapShotKeyInputResetButton = new System.Windows.Forms.Button();
            this.lbHotKeyHideTransWindow = new System.Windows.Forms.Label();
            this.lbHotKeyOnceTranslate = new System.Windows.Forms.Label();
            this.lbHotKeySnapShot = new System.Windows.Forms.Label();
            this.lbHotKeyInformation = new System.Windows.Forms.Label();
            this.lbHotKeyQuickOCR = new System.Windows.Forms.Label();
            this.lbHotKeyDic = new System.Windows.Forms.Label();
            this.lbHotKeyDoTrans = new System.Windows.Forms.Label();
            this.lbHotkey = new System.Windows.Forms.Label();
            this.tpQuickSetting = new System.Windows.Forms.TabPage();
            this.panel28 = new System.Windows.Forms.Panel();
            this.panel31 = new System.Windows.Forms.Panel();
            this.cbSetBasicDefaultPage = new System.Windows.Forms.CheckBox();
            this.btQuickJap = new System.Windows.Forms.Button();
            this.lbQuickSettingInformation = new System.Windows.Forms.Label();
            this.btQucickEnglish = new System.Windows.Forms.Button();
            this.lbQuickSetting = new System.Windows.Forms.Label();
            this.tpDebuging = new System.Windows.Forms.TabPage();
            this.panel24 = new System.Windows.Forms.Panel();
            this.panel26 = new System.Windows.Forms.Panel();
            this.plDebugOn = new System.Windows.Forms.Panel();
            this.cbDisplayLowQuality = new System.Windows.Forms.CheckBox();
            this.cbShowOverlayWordArea = new System.Windows.Forms.CheckBox();
            this.cbSetLineTrans = new System.Windows.Forms.CheckBox();
            this.btClearFormerResult = new System.Windows.Forms.Button();
            this.cbShowFormerLog = new System.Windows.Forms.CheckBox();
            this.cbUnlockSpeed = new System.Windows.Forms.CheckBox();
            this.cbSaveCaptureResult = new System.Windows.Forms.CheckBox();
            this.cbSaveCapture = new System.Windows.Forms.CheckBox();
            this.cbShowReplace = new System.Windows.Forms.CheckBox();
            this.plDebugOff = new System.Windows.Forms.Panel();
            this.label63 = new System.Windows.Forms.Label();
            this.btnDebugOn = new System.Windows.Forms.Button();
            this.label70 = new System.Windows.Forms.Label();
            this.ContextOption.SuspendLayout();
            this.optionMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tbMain.SuspendLayout();
            this.tpBasic.SuspendLayout();
            this.panel8.SuspendLayout();
            this.pnAdjustImg.SuspendLayout();
            this.pnOCR.SuspendLayout();
            this.Tesseract_panel.SuspendLayout();
            this.pnGoogleOcr.SuspendLayout();
            this.pnNHocr.SuspendLayout();
            this.WinOCR_panel.SuspendLayout();
            this.pnTranslate.SuspendLayout();
            this.pnDeepl.SuspendLayout();
            this.pnGoogleBasic.SuspendLayout();
            this.DB_Panel.SuspendLayout();
            this.Naver_Panel.SuspendLayout();
            this.Google_Panel.SuspendLayout();
            this.pnEzTrans.SuspendLayout();
            this.tpText.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel17.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outlineColor2Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outlineColor1Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textColorBox)).BeginInit();
            this.panel9.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeUpDown)).BeginInit();
            this.tpExtra.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel21.SuspendLayout();
            this.panel25.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgZoomsizeUpDown)).BeginInit();
            this.tpTranslation.SuspendLayout();
            this.panel19.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel27.SuspendLayout();
            this.panel22.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel15.SuspendLayout();
            this.tpETC.SuspendLayout();
            this.panel18.SuspendLayout();
            this.panel16.SuspendLayout();
            this.panel20.SuspendLayout();
            this.panel23.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tpQuickSetting.SuspendLayout();
            this.panel28.SuspendLayout();
            this.panel31.SuspendLayout();
            this.tpDebuging.SuspendLayout();
            this.panel24.SuspendLayout();
            this.panel26.SuspendLayout();
            this.plDebugOn.SuspendLayout();
            this.plDebugOff.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.ContextOption;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "MORT";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // ContextOption
            // 
            this.ContextOption.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ContextOption.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionToolStripMenuItem,
            this.showTransToolStripMenuItem,
            this.rTTToolStripMenuItem,
            this.toolStripSeparator7,
            this.setTranslateTopMostToolStripMenuItem,
            this.setCheckSpellingToolStripMenuItem,
            this.setCutPointToolStripMenuItem,
            this.toolStripSeparator5,
            this.transToolStripMenuItem,
            this.toolStripSeparator6,
            this.settingToolStripMenuItem,
            this.toolStripSeparator8,
            this.aboutToolStripMenuItem,
            this.checkUpdateToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.ContextOption.Name = "contextMenuStrip1";
            this.ContextOption.Size = new System.Drawing.Size(199, 380);
            // 
            // optionToolStripMenuItem
            // 
            this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
            this.optionToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.optionToolStripMenuItem.Text = "옵션";
            this.optionToolStripMenuItem.Click += new System.EventHandler(this.ContextOption_Click);
            // 
            // showTransToolStripMenuItem
            // 
            this.showTransToolStripMenuItem.Name = "showTransToolStripMenuItem";
            this.showTransToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.showTransToolStripMenuItem.Text = "번역창";
            this.showTransToolStripMenuItem.Click += new System.EventHandler(this.showTransToolStripMenuItem_Click);
            // 
            // rTTToolStripMenuItem
            // 
            this.rTTToolStripMenuItem.Name = "rTTToolStripMenuItem";
            this.rTTToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.rTTToolStripMenuItem.Text = "리모컨";
            this.rTTToolStripMenuItem.Click += new System.EventHandler(this.rTTToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(195, 6);
            // 
            // setTranslateTopMostToolStripMenuItem
            // 
            this.setTranslateTopMostToolStripMenuItem.Checked = true;
            this.setTranslateTopMostToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setTranslateTopMostToolStripMenuItem.Name = "setTranslateTopMostToolStripMenuItem";
            this.setTranslateTopMostToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.setTranslateTopMostToolStripMenuItem.Text = "번역창 고정";
            this.setTranslateTopMostToolStripMenuItem.Click += new System.EventHandler(this.setTranslateTopMostToolStripMenuItem_Click);
            // 
            // setCheckSpellingToolStripMenuItem
            // 
            this.setCheckSpellingToolStripMenuItem.Checked = true;
            this.setCheckSpellingToolStripMenuItem.CheckOnClick = true;
            this.setCheckSpellingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setCheckSpellingToolStripMenuItem.Name = "setCheckSpellingToolStripMenuItem";
            this.setCheckSpellingToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.setCheckSpellingToolStripMenuItem.Text = "교정사전 사용";
            this.setCheckSpellingToolStripMenuItem.Click += new System.EventHandler(this.setCheckSpellingToolStripMenuItem_Click);
            // 
            // setCutPointToolStripMenuItem
            // 
            this.setCutPointToolStripMenuItem.Name = "setCutPointToolStripMenuItem";
            this.setCutPointToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.setCutPointToolStripMenuItem.Text = "영역 설정";
            this.setCutPointToolStripMenuItem.Click += new System.EventHandler(this.setCutPointToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(195, 6);
            // 
            // transToolStripMenuItem
            // 
            this.transToolStripMenuItem.Name = "transToolStripMenuItem";
            this.transToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.transToolStripMenuItem.Text = "번역 시작";
            this.transToolStripMenuItem.Click += new System.EventHandler(this.ContextTranslate_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(195, 6);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingSaveToolStripMenuItem2,
            this.settingLoadToolStripMenuItem2,
            this.settingDefaultToolStripMenuItem});
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.settingToolStripMenuItem.Text = "설정";
            // 
            // settingSaveToolStripMenuItem2
            // 
            this.settingSaveToolStripMenuItem2.Name = "settingSaveToolStripMenuItem2";
            this.settingSaveToolStripMenuItem2.Size = new System.Drawing.Size(186, 34);
            this.settingSaveToolStripMenuItem2.Text = "저장";
            this.settingSaveToolStripMenuItem2.Click += new System.EventHandler(this.settingSaveToolStripMenuItem2_Click);
            // 
            // settingLoadToolStripMenuItem2
            // 
            this.settingLoadToolStripMenuItem2.Name = "settingLoadToolStripMenuItem2";
            this.settingLoadToolStripMenuItem2.Size = new System.Drawing.Size(186, 34);
            this.settingLoadToolStripMenuItem2.Text = "불러오기";
            this.settingLoadToolStripMenuItem2.Click += new System.EventHandler(this.settingLoadToolStripMenuItem2_Click);
            // 
            // settingDefaultToolStripMenuItem
            // 
            this.settingDefaultToolStripMenuItem.Name = "settingDefaultToolStripMenuItem";
            this.settingDefaultToolStripMenuItem.Size = new System.Drawing.Size(186, 34);
            this.settingDefaultToolStripMenuItem.Text = "초기화";
            this.settingDefaultToolStripMenuItem.Click += new System.EventHandler(this.settingDefaultToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(195, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.aboutToolStripMenuItem.Text = " About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // checkUpdateToolStripMenuItem
            // 
            this.checkUpdateToolStripMenuItem.Name = "checkUpdateToolStripMenuItem";
            this.checkUpdateToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.checkUpdateToolStripMenuItem.Text = "업데이트";
            this.checkUpdateToolStripMenuItem.Click += new System.EventHandler(this.checkUpdateToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.ExitToolStripMenuItem.Text = "종료";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // optionMenuStrip
            // 
            this.optionMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.optionMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.설정저장ToolStripMenuItem,
            this.설정불러오기ToolStripMenuItem});
            this.optionMenuStrip.Name = "optionMenuStrip";
            this.optionMenuStrip.Size = new System.Drawing.Size(199, 68);
            // 
            // 설정저장ToolStripMenuItem
            // 
            this.설정저장ToolStripMenuItem.Name = "설정저장ToolStripMenuItem";
            this.설정저장ToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.설정저장ToolStripMenuItem.Text = "설정 저장";
            // 
            // 설정불러오기ToolStripMenuItem
            // 
            this.설정불러오기ToolStripMenuItem.Name = "설정불러오기ToolStripMenuItem";
            this.설정불러오기ToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.설정불러오기ToolStripMenuItem.Text = "설정 불러오기";
            // 
            // acceptButton
            // 
            this.acceptButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(91)))), ((int)(((byte)(91)))));
            this.acceptButton.FlatAppearance.BorderSize = 0;
            this.acceptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.acceptButton.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.acceptButton.ForeColor = System.Drawing.Color.White;
            this.acceptButton.Location = new System.Drawing.Point(567, 884);
            this.acceptButton.Margin = new System.Windows.Forms.Padding(0);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(292, 82);
            this.acceptButton.TabIndex = 44;
            this.acceptButton.Text = "적 용";
            this.acceptButton.UseVisualStyleBackColor = false;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(91)))), ((int)(((byte)(91)))));
            this.pictureBox1.Location = new System.Drawing.Point(0, 890);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(76, 165);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 45;
            this.pictureBox1.TabStop = false;
            // 
            // donationButton
            // 
            this.donationButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(91)))), ((int)(((byte)(91)))));
            this.donationButton.FlatAppearance.BorderSize = 0;
            this.donationButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.donationButton.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.donationButton.ForeColor = System.Drawing.Color.White;
            this.donationButton.Location = new System.Drawing.Point(150, 884);
            this.donationButton.Margin = new System.Windows.Forms.Padding(0);
            this.donationButton.Name = "donationButton";
            this.donationButton.Size = new System.Drawing.Size(292, 82);
            this.donationButton.TabIndex = 46;
            this.donationButton.Text = "후원하기";
            this.donationButton.UseVisualStyleBackColor = false;
            this.donationButton.Click += new System.EventHandler(this.donationButton_Click);
            // 
            // toolTip_OCR
            // 
            this.toolTip_OCR.AutoPopDelay = 500000000;
            this.toolTip_OCR.InitialDelay = 300;
            this.toolTip_OCR.ReshowDelay = 100;
            // 
            // tbMain
            // 
            this.tbMain.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tbMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(91)))), ((int)(((byte)(91)))));
            this.tbMain.Controls.Add(this.tpBasic);
            this.tbMain.Controls.Add(this.tpText);
            this.tbMain.Controls.Add(this.tpExtra);
            this.tbMain.Controls.Add(this.tpTranslation);
            this.tbMain.Controls.Add(this.tpETC);
            this.tbMain.Controls.Add(this.tpQuickSetting);
            this.tbMain.Controls.Add(this.tpDebuging);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tbMain.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.tbMain.ItemSize = new System.Drawing.Size(44, 76);
            this.tbMain.Location = new System.Drawing.Point(0, 0);
            this.tbMain.Margin = new System.Windows.Forms.Padding(0);
            this.tbMain.Multiline = true;
            this.tbMain.Name = "tbMain";
            this.tbMain.Padding = new System.Drawing.Point(0, 0);
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(902, 890);
            this.tbMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tbMain.TabIndex = 43;
            this.tbMain.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpBasic
            // 
            this.tpBasic.Controls.Add(this.panel8);
            this.tpBasic.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tpBasic.Location = new System.Drawing.Point(80, 4);
            this.tpBasic.Margin = new System.Windows.Forms.Padding(0);
            this.tpBasic.Name = "tpBasic";
            this.tpBasic.Size = new System.Drawing.Size(818, 882);
            this.tpBasic.TabIndex = 0;
            this.tpBasic.Text = "기본설정";
            this.tpBasic.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.panel8.Controls.Add(this.pnAdjustImg);
            this.panel8.Controls.Add(this.pnOCR);
            this.panel8.Controls.Add(this.pnTranslate);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(818, 882);
            this.panel8.TabIndex = 39;
            // 
            // pnAdjustImg
            // 
            this.pnAdjustImg.Controls.Add(this.btImgResult);
            this.pnAdjustImg.Controls.Add(this.cbThreshold);
            this.pnAdjustImg.Controls.Add(this.tbThreshold);
            this.pnAdjustImg.Controls.Add(this.groupCombo);
            this.pnAdjustImg.Controls.Add(this.groupLabel);
            this.pnAdjustImg.Controls.Add(this.lbImgGroupCount);
            this.pnAdjustImg.Controls.Add(this.lbImgGroup);
            this.pnAdjustImg.Controls.Add(this.label1);
            this.pnAdjustImg.Controls.Add(this.v2TextBox);
            this.pnAdjustImg.Controls.Add(this.lbAdjustImg);
            this.pnAdjustImg.Controls.Add(this.checkRGB);
            this.pnAdjustImg.Controls.Add(this.s2TextBox);
            this.pnAdjustImg.Controls.Add(this.label3);
            this.pnAdjustImg.Controls.Add(this.label16);
            this.pnAdjustImg.Controls.Add(this.rTextBox);
            this.pnAdjustImg.Controls.Add(this.checkErode);
            this.pnAdjustImg.Controls.Add(this.label4);
            this.pnAdjustImg.Controls.Add(this.checkHSV);
            this.pnAdjustImg.Controls.Add(this.gTextBox);
            this.pnAdjustImg.Controls.Add(this.label5);
            this.pnAdjustImg.Controls.Add(this.v1TextBox);
            this.pnAdjustImg.Controls.Add(this.bTextBox);
            this.pnAdjustImg.Controls.Add(this.label7);
            this.pnAdjustImg.Controls.Add(this.label8);
            this.pnAdjustImg.Controls.Add(this.s1TextBox);
            this.pnAdjustImg.Location = new System.Drawing.Point(4, 580);
            this.pnAdjustImg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnAdjustImg.Name = "pnAdjustImg";
            this.pnAdjustImg.Size = new System.Drawing.Size(770, 274);
            this.pnAdjustImg.TabIndex = 37;
            this.pnAdjustImg.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // btImgResult
            // 
            this.btImgResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btImgResult.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btImgResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btImgResult.ForeColor = System.Drawing.Color.White;
            this.btImgResult.Location = new System.Drawing.Point(366, 164);
            this.btImgResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btImgResult.Name = "btImgResult";
            this.btImgResult.Size = new System.Drawing.Size(294, 38);
            this.btImgResult.TabIndex = 51;
            this.btImgResult.Text = "보정 결과 확인하기";
            this.btImgResult.UseVisualStyleBackColor = false;
            this.btImgResult.Click += new System.EventHandler(this.OnClickShowImgResult);
            // 
            // cbThreshold
            // 
            this.cbThreshold.AutoSize = true;
            this.cbThreshold.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbThreshold.ForeColor = System.Drawing.Color.White;
            this.cbThreshold.Location = new System.Drawing.Point(18, 123);
            this.cbThreshold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbThreshold.Name = "cbThreshold";
            this.cbThreshold.Size = new System.Drawing.Size(185, 32);
            this.cbThreshold.TabIndex = 49;
            this.cbThreshold.Text = "임계값으로 추출";
            this.cbThreshold.UseVisualStyleBackColor = true;
            this.cbThreshold.CheckedChanged += new System.EventHandler(this.cbThreshold_CheckedChanged);
            // 
            // tbThreshold
            // 
            this.tbThreshold.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.tbThreshold.Location = new System.Drawing.Point(244, 122);
            this.tbThreshold.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbThreshold.Name = "tbThreshold";
            this.tbThreshold.Size = new System.Drawing.Size(68, 33);
            this.tbThreshold.TabIndex = 50;
            this.tbThreshold.Text = "0";
            this.tbThreshold.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.tbThreshold.Leave += new System.EventHandler(this.thresholdTextLeave);
            // 
            // groupCombo
            // 
            this.groupCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupCombo.FormattingEnabled = true;
            this.groupCombo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.groupCombo.Location = new System.Drawing.Point(326, 216);
            this.groupCombo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupCombo.Name = "groupCombo";
            this.groupCombo.Size = new System.Drawing.Size(82, 33);
            this.groupCombo.TabIndex = 47;
            this.groupCombo.SelectedIndexChanged += new System.EventHandler(this.groupCombo_SelectedIndexChanged);
            // 
            // groupLabel
            // 
            this.groupLabel.AutoSize = true;
            this.groupLabel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupLabel.ForeColor = System.Drawing.Color.White;
            this.groupLabel.Location = new System.Drawing.Point(525, 216);
            this.groupLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.groupLabel.Name = "groupLabel";
            this.groupLabel.Size = new System.Drawing.Size(24, 28);
            this.groupLabel.TabIndex = 46;
            this.groupLabel.Text = "0";
            // 
            // lbImgGroupCount
            // 
            this.lbImgGroupCount.AutoSize = true;
            this.lbImgGroupCount.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbImgGroupCount.ForeColor = System.Drawing.Color.White;
            this.lbImgGroupCount.Location = new System.Drawing.Point(418, 216);
            this.lbImgGroupCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbImgGroupCount.Name = "lbImgGroupCount";
            this.lbImgGroupCount.Size = new System.Drawing.Size(98, 28);
            this.lbImgGroupCount.TabIndex = 45;
            this.lbImgGroupCount.Text = "그룹 수 : ";
            // 
            // lbImgGroup
            // 
            this.lbImgGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbImgGroup.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbImgGroup.ForeColor = System.Drawing.Color.White;
            this.lbImgGroup.Location = new System.Drawing.Point(140, 216);
            this.lbImgGroup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbImgGroup.Name = "lbImgGroup";
            this.lbImgGroup.Size = new System.Drawing.Size(178, 34);
            this.lbImgGroup.TabIndex = 43;
            this.lbImgGroup.Text = "범위 그룹";
            this.lbImgGroup.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(552, 80);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 28);
            this.label1.TabIndex = 18;
            this.label1.Text = "~";
            // 
            // v2TextBox
            // 
            this.v2TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.v2TextBox.Location = new System.Drawing.Point(582, 76);
            this.v2TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.v2TextBox.Name = "v2TextBox";
            this.v2TextBox.Size = new System.Drawing.Size(68, 33);
            this.v2TextBox.TabIndex = 16;
            this.v2TextBox.Text = "0";
            this.v2TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.v2TextBox.Leave += new System.EventHandler(this.hsvTextLeave);
            // 
            // lbAdjustImg
            // 
            this.lbAdjustImg.AutoSize = true;
            this.lbAdjustImg.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbAdjustImg.ForeColor = System.Drawing.Color.White;
            this.lbAdjustImg.Location = new System.Drawing.Point(6, 4);
            this.lbAdjustImg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAdjustImg.Name = "lbAdjustImg";
            this.lbAdjustImg.Size = new System.Drawing.Size(137, 31);
            this.lbAdjustImg.TabIndex = 8;
            this.lbAdjustImg.Text = "이미지 보정";
            // 
            // checkRGB
            // 
            this.checkRGB.AutoSize = true;
            this.checkRGB.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.checkRGB.ForeColor = System.Drawing.Color.White;
            this.checkRGB.Location = new System.Drawing.Point(18, 38);
            this.checkRGB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkRGB.Name = "checkRGB";
            this.checkRGB.Size = new System.Drawing.Size(145, 32);
            this.checkRGB.TabIndex = 8;
            this.checkRGB.Text = "RGB로 추출";
            this.checkRGB.UseVisualStyleBackColor = true;
            this.checkRGB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkRGB_MouseDown);
            // 
            // s2TextBox
            // 
            this.s2TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.s2TextBox.Location = new System.Drawing.Point(358, 76);
            this.s2TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.s2TextBox.Name = "s2TextBox";
            this.s2TextBox.Size = new System.Drawing.Size(68, 33);
            this.s2TextBox.TabIndex = 14;
            this.s2TextBox.Text = "0";
            this.s2TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.s2TextBox.Leave += new System.EventHandler(this.hsvTextLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(210, 38);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 28);
            this.label3.TabIndex = 2;
            this.label3.Text = "R";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(326, 80);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(26, 28);
            this.label16.TabIndex = 16;
            this.label16.Text = "~";
            // 
            // rTextBox
            // 
            this.rTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.rTextBox.Location = new System.Drawing.Point(244, 33);
            this.rTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rTextBox.Name = "rTextBox";
            this.rTextBox.Size = new System.Drawing.Size(68, 33);
            this.rTextBox.TabIndex = 9;
            this.rTextBox.Text = "0";
            this.rTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.rTextBox.Leave += new System.EventHandler(this.rgbTextLeave);
            // 
            // checkErode
            // 
            this.checkErode.AutoSize = true;
            this.checkErode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.checkErode.ForeColor = System.Drawing.Color.White;
            this.checkErode.Location = new System.Drawing.Point(18, 170);
            this.checkErode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkErode.Name = "checkErode";
            this.checkErode.Size = new System.Drawing.Size(340, 32);
            this.checkErode.TabIndex = 17;
            this.checkErode.Text = "침식 사용 (굵은 글씨체일때 사용)";
            this.checkErode.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(324, 38);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 28);
            this.label4.TabIndex = 4;
            this.label4.Text = "G";
            // 
            // checkHSV
            // 
            this.checkHSV.AutoSize = true;
            this.checkHSV.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.checkHSV.ForeColor = System.Drawing.Color.White;
            this.checkHSV.Location = new System.Drawing.Point(18, 78);
            this.checkHSV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkHSV.Name = "checkHSV";
            this.checkHSV.Size = new System.Drawing.Size(144, 32);
            this.checkHSV.TabIndex = 12;
            this.checkHSV.Text = "HSV로 추출";
            this.checkHSV.UseVisualStyleBackColor = true;
            this.checkHSV.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkHSV_MouseDown);
            // 
            // gTextBox
            // 
            this.gTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.gTextBox.Location = new System.Drawing.Point(358, 33);
            this.gTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gTextBox.Name = "gTextBox";
            this.gTextBox.Size = new System.Drawing.Size(68, 33);
            this.gTextBox.TabIndex = 10;
            this.gTextBox.Text = "0";
            this.gTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.gTextBox.Leave += new System.EventHandler(this.rgbTextLeave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(438, 38);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 28);
            this.label5.TabIndex = 6;
            this.label5.Text = "B";
            // 
            // v1TextBox
            // 
            this.v1TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.v1TextBox.Location = new System.Drawing.Point(472, 76);
            this.v1TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.v1TextBox.Name = "v1TextBox";
            this.v1TextBox.Size = new System.Drawing.Size(68, 33);
            this.v1TextBox.TabIndex = 15;
            this.v1TextBox.Text = "0";
            this.v1TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.v1TextBox.Leave += new System.EventHandler(this.hsvTextLeave);
            // 
            // bTextBox
            // 
            this.bTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.bTextBox.Location = new System.Drawing.Point(472, 33);
            this.bTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bTextBox.Name = "bTextBox";
            this.bTextBox.Size = new System.Drawing.Size(68, 33);
            this.bTextBox.TabIndex = 11;
            this.bTextBox.Text = "0";
            this.bTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.bTextBox.Leave += new System.EventHandler(this.rgbTextLeave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(438, 78);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 28);
            this.label7.TabIndex = 11;
            this.label7.Text = "V";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(210, 78);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 28);
            this.label8.TabIndex = 9;
            this.label8.Text = "S";
            // 
            // s1TextBox
            // 
            this.s1TextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.s1TextBox.Location = new System.Drawing.Point(244, 76);
            this.s1TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.s1TextBox.Name = "s1TextBox";
            this.s1TextBox.Size = new System.Drawing.Size(68, 33);
            this.s1TextBox.TabIndex = 13;
            this.s1TextBox.Text = "0";
            this.s1TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
            this.s1TextBox.Leave += new System.EventHandler(this.hsvTextLeave);
            // 
            // pnOCR
            // 
            this.pnOCR.Controls.Add(this.btOcrHelp);
            this.pnOCR.Controls.Add(this.label48);
            this.pnOCR.Controls.Add(this.OCR_Type_comboBox);
            this.pnOCR.Controls.Add(this.isClipBoardcheckBox1);
            this.pnOCR.Controls.Add(this.saveOCRCheckBox);
            this.pnOCR.Controls.Add(this.ocrLabel);
            this.pnOCR.Controls.Add(this.showOcrCheckBox);
            this.pnOCR.Controls.Add(this.Tesseract_panel);
            this.pnOCR.Controls.Add(this.pnGoogleOcr);
            this.pnOCR.Controls.Add(this.pnNHocr);
            this.pnOCR.Controls.Add(this.WinOCR_panel);
            this.pnOCR.Location = new System.Drawing.Point(4, 4);
            this.pnOCR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnOCR.Name = "pnOCR";
            this.pnOCR.Size = new System.Drawing.Size(770, 232);
            this.pnOCR.TabIndex = 37;
            this.pnOCR.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // btOcrHelp
            // 
            this.btOcrHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(91)))), ((int)(((byte)(91)))));
            this.btOcrHelp.FlatAppearance.BorderSize = 0;
            this.btOcrHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btOcrHelp.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btOcrHelp.ForeColor = System.Drawing.Color.White;
            this.btOcrHelp.Location = new System.Drawing.Point(426, 46);
            this.btOcrHelp.Margin = new System.Windows.Forms.Padding(0);
            this.btOcrHelp.Name = "btOcrHelp";
            this.btOcrHelp.Size = new System.Drawing.Size(42, 38);
            this.btOcrHelp.TabIndex = 59;
            this.btOcrHelp.Text = "?";
            this.btOcrHelp.UseVisualStyleBackColor = false;
            this.btOcrHelp.Click += new System.EventHandler(this.OnClick_btOcrHelp);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label48.ForeColor = System.Drawing.Color.White;
            this.label48.Location = new System.Drawing.Point(24, 51);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(61, 28);
            this.label48.TabIndex = 50;
            this.label48.Text = "OCR ";
            // 
            // OCR_Type_comboBox
            // 
            this.OCR_Type_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OCR_Type_comboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.OCR_Type_comboBox.FormattingEnabled = true;
            this.OCR_Type_comboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.OCR_Type_comboBox.Items.AddRange(new object[] {
            "OCR Tesseract",
            "OCR Win OCR",
            "OCR NHocr",
            "OCR Google"});
            this.OCR_Type_comboBox.Location = new System.Drawing.Point(158, 46);
            this.OCR_Type_comboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OCR_Type_comboBox.Name = "OCR_Type_comboBox";
            this.OCR_Type_comboBox.Size = new System.Drawing.Size(246, 36);
            this.OCR_Type_comboBox.TabIndex = 51;
            this.OCR_Type_comboBox.SelectedIndexChanged += new System.EventHandler(this.OCR_Type_comboBox_SelectedIndexChanged);
            // 
            // isClipBoardcheckBox1
            // 
            this.isClipBoardcheckBox1.AutoSize = true;
            this.isClipBoardcheckBox1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.isClipBoardcheckBox1.ForeColor = System.Drawing.Color.White;
            this.isClipBoardcheckBox1.Location = new System.Drawing.Point(550, 184);
            this.isClipBoardcheckBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.isClipBoardcheckBox1.Name = "isClipBoardcheckBox1";
            this.isClipBoardcheckBox1.Size = new System.Drawing.Size(185, 32);
            this.isClipBoardcheckBox1.TabIndex = 26;
            this.isClipBoardcheckBox1.Text = "클립보드에 저장";
            this.isClipBoardcheckBox1.UseVisualStyleBackColor = true;
            // 
            // saveOCRCheckBox
            // 
            this.saveOCRCheckBox.AutoSize = true;
            this.saveOCRCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.saveOCRCheckBox.ForeColor = System.Drawing.Color.White;
            this.saveOCRCheckBox.Location = new System.Drawing.Point(290, 184);
            this.saveOCRCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.saveOCRCheckBox.Name = "saveOCRCheckBox";
            this.saveOCRCheckBox.Size = new System.Drawing.Size(174, 32);
            this.saveOCRCheckBox.TabIndex = 24;
            this.saveOCRCheckBox.Text = "OCR 결과 저장";
            this.saveOCRCheckBox.UseVisualStyleBackColor = true;
            // 
            // ocrLabel
            // 
            this.ocrLabel.AutoSize = true;
            this.ocrLabel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ocrLabel.ForeColor = System.Drawing.Color.White;
            this.ocrLabel.Location = new System.Drawing.Point(6, 4);
            this.ocrLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ocrLabel.Name = "ocrLabel";
            this.ocrLabel.Size = new System.Drawing.Size(62, 31);
            this.ocrLabel.TabIndex = 8;
            this.ocrLabel.Text = "OCR";
            // 
            // showOcrCheckBox
            // 
            this.showOcrCheckBox.AutoSize = true;
            this.showOcrCheckBox.Checked = true;
            this.showOcrCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOcrCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.showOcrCheckBox.ForeColor = System.Drawing.Color.White;
            this.showOcrCheckBox.Location = new System.Drawing.Point(40, 184);
            this.showOcrCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showOcrCheckBox.Name = "showOcrCheckBox";
            this.showOcrCheckBox.Size = new System.Drawing.Size(174, 32);
            this.showOcrCheckBox.TabIndex = 2;
            this.showOcrCheckBox.Text = "OCR 결과 출력";
            this.showOcrCheckBox.UseVisualStyleBackColor = true;
            // 
            // Tesseract_panel
            // 
            this.Tesseract_panel.Controls.Add(this.cbFastTess);
            this.Tesseract_panel.Controls.Add(this.tesseractLanguageComboBox);
            this.Tesseract_panel.Controls.Add(this.lbTesseractLanguage);
            this.Tesseract_panel.Controls.Add(this.label18);
            this.Tesseract_panel.Controls.Add(this.tessDataTextBox);
            this.Tesseract_panel.Location = new System.Drawing.Point(12, 81);
            this.Tesseract_panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Tesseract_panel.Name = "Tesseract_panel";
            this.Tesseract_panel.Size = new System.Drawing.Size(706, 94);
            this.Tesseract_panel.TabIndex = 53;
            // 
            // cbFastTess
            // 
            this.cbFastTess.AutoSize = true;
            this.cbFastTess.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbFastTess.ForeColor = System.Drawing.Color.White;
            this.cbFastTess.Location = new System.Drawing.Point(28, 63);
            this.cbFastTess.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbFastTess.Name = "cbFastTess";
            this.cbFastTess.Size = new System.Drawing.Size(508, 32);
            this.cbFastTess.TabIndex = 55;
            this.cbFastTess.Text = "고속 모드 (빠르나 정확도가 떨어짐, Tesseract 전용)";
            this.cbFastTess.UseVisualStyleBackColor = true;
            // 
            // tesseractLanguageComboBox
            // 
            this.tesseractLanguageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tesseractLanguageComboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.tesseractLanguageComboBox.FormattingEnabled = true;
            this.tesseractLanguageComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tesseractLanguageComboBox.Items.AddRange(new object[] {
            "en",
            "ja",
            "Language ETC"});
            this.tesseractLanguageComboBox.Location = new System.Drawing.Point(520, 9);
            this.tesseractLanguageComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tesseractLanguageComboBox.Name = "tesseractLanguageComboBox";
            this.tesseractLanguageComboBox.Size = new System.Drawing.Size(110, 36);
            this.tesseractLanguageComboBox.TabIndex = 52;
            this.tesseractLanguageComboBox.SelectionChangeCommitted += new System.EventHandler(this.tesseractLanguageComboBox_SelectionChangeCommitted);
            // 
            // lbTesseractLanguage
            // 
            this.lbTesseractLanguage.AutoSize = true;
            this.lbTesseractLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbTesseractLanguage.ForeColor = System.Drawing.Color.White;
            this.lbTesseractLanguage.Location = new System.Drawing.Point(410, 15);
            this.lbTesseractLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTesseractLanguage.Name = "lbTesseractLanguage";
            this.lbTesseractLanguage.Size = new System.Drawing.Size(99, 28);
            this.lbTesseractLanguage.TabIndex = 51;
            this.lbTesseractLanguage.Text = "추출 언어";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(15, 14);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(96, 28);
            this.label18.TabIndex = 50;
            this.label18.Text = "Tessdata";
            // 
            // tessDataTextBox
            // 
            this.tessDataTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tessDataTextBox.Location = new System.Drawing.Point(146, 10);
            this.tessDataTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tessDataTextBox.Name = "tessDataTextBox";
            this.tessDataTextBox.Size = new System.Drawing.Size(210, 30);
            this.tessDataTextBox.TabIndex = 49;
            this.tessDataTextBox.Text = "eng";
            // 
            // pnGoogleOcr
            // 
            this.pnGoogleOcr.Controls.Add(this.cbGoogleOcrLanguge);
            this.pnGoogleOcr.Controls.Add(this.lbGoogleOCRLanguage);
            this.pnGoogleOcr.Controls.Add(this.lbGoogleOcrStatus);
            this.pnGoogleOcr.Controls.Add(this.btnSettingGoogleOCR);
            this.pnGoogleOcr.Location = new System.Drawing.Point(12, 81);
            this.pnGoogleOcr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnGoogleOcr.Name = "pnGoogleOcr";
            this.pnGoogleOcr.Size = new System.Drawing.Size(706, 94);
            this.pnGoogleOcr.TabIndex = 60;
            // 
            // cbGoogleOcrLanguge
            // 
            this.cbGoogleOcrLanguge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGoogleOcrLanguge.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbGoogleOcrLanguge.FormattingEnabled = true;
            this.cbGoogleOcrLanguge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbGoogleOcrLanguge.Items.AddRange(new object[] {
            "자동"});
            this.cbGoogleOcrLanguge.Location = new System.Drawing.Point(146, 14);
            this.cbGoogleOcrLanguge.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbGoogleOcrLanguge.Name = "cbGoogleOcrLanguge";
            this.cbGoogleOcrLanguge.Size = new System.Drawing.Size(246, 36);
            this.cbGoogleOcrLanguge.TabIndex = 55;
            this.cbGoogleOcrLanguge.SelectedIndexChanged += new System.EventHandler(this.cbGoogleOcrLanguge_SelectedIndexChanged);
            // 
            // lbGoogleOCRLanguage
            // 
            this.lbGoogleOCRLanguage.AutoSize = true;
            this.lbGoogleOCRLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbGoogleOCRLanguage.ForeColor = System.Drawing.Color.White;
            this.lbGoogleOCRLanguage.Location = new System.Drawing.Point(12, 18);
            this.lbGoogleOCRLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbGoogleOCRLanguage.Name = "lbGoogleOCRLanguage";
            this.lbGoogleOCRLanguage.Size = new System.Drawing.Size(59, 28);
            this.lbGoogleOCRLanguage.TabIndex = 54;
            this.lbGoogleOCRLanguage.Text = "언어 ";
            // 
            // lbGoogleOcrStatus
            // 
            this.lbGoogleOcrStatus.AutoSize = true;
            this.lbGoogleOcrStatus.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbGoogleOcrStatus.ForeColor = System.Drawing.Color.White;
            this.lbGoogleOcrStatus.Location = new System.Drawing.Point(141, 58);
            this.lbGoogleOcrStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbGoogleOcrStatus.Name = "lbGoogleOcrStatus";
            this.lbGoogleOcrStatus.Size = new System.Drawing.Size(463, 28);
            this.lbGoogleOcrStatus.TabIndex = 53;
            this.lbGoogleOcrStatus.Text = "스냅샷 / 한 번만 번역하기에서만 사용 가능합니다";
            // 
            // btnSettingGoogleOCR
            // 
            this.btnSettingGoogleOCR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnSettingGoogleOCR.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnSettingGoogleOCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettingGoogleOCR.ForeColor = System.Drawing.Color.White;
            this.btnSettingGoogleOCR.Location = new System.Drawing.Point(412, 12);
            this.btnSettingGoogleOCR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSettingGoogleOCR.Name = "btnSettingGoogleOCR";
            this.btnSettingGoogleOCR.Size = new System.Drawing.Size(290, 38);
            this.btnSettingGoogleOCR.TabIndex = 52;
            this.btnSettingGoogleOCR.Text = "API 설정";
            this.btnSettingGoogleOCR.UseVisualStyleBackColor = false;
            this.btnSettingGoogleOCR.Click += new System.EventHandler(this.OnClick_btGoogleOcrSetting);
            // 
            // pnNHocr
            // 
            this.pnNHocr.Controls.Add(this.lbNHOcrInfo);
            this.pnNHocr.Location = new System.Drawing.Point(12, 81);
            this.pnNHocr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnNHocr.Name = "pnNHocr";
            this.pnNHocr.Size = new System.Drawing.Size(706, 94);
            this.pnNHocr.TabIndex = 56;
            // 
            // lbNHOcrInfo
            // 
            this.lbNHOcrInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbNHOcrInfo.AutoEllipsis = true;
            this.lbNHOcrInfo.AutoSize = true;
            this.lbNHOcrInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbNHOcrInfo.ForeColor = System.Drawing.Color.White;
            this.lbNHOcrInfo.Location = new System.Drawing.Point(81, 21);
            this.lbNHOcrInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbNHOcrInfo.Name = "lbNHOcrInfo";
            this.lbNHOcrInfo.Size = new System.Drawing.Size(557, 56);
            this.lbNHOcrInfo.TabIndex = 18;
            this.lbNHOcrInfo.Text = "특정 상황에 한해서만 Tesseract OCR 보다 인식이 잘됩니다.\r\n가능하면 Tessract이나 WinOCR을 사용해 주세요";
            this.lbNHOcrInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WinOCR_panel
            // 
            this.WinOCR_panel.Controls.Add(this.WinOCR_Language_comboBox);
            this.WinOCR_panel.Controls.Add(this.lbWinOCRLanguage);
            this.WinOCR_panel.Location = new System.Drawing.Point(12, 81);
            this.WinOCR_panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.WinOCR_panel.Name = "WinOCR_panel";
            this.WinOCR_panel.Size = new System.Drawing.Size(706, 54);
            this.WinOCR_panel.TabIndex = 54;
            // 
            // WinOCR_Language_comboBox
            // 
            this.WinOCR_Language_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WinOCR_Language_comboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.WinOCR_Language_comboBox.FormattingEnabled = true;
            this.WinOCR_Language_comboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.WinOCR_Language_comboBox.Items.AddRange(new object[] {
            "초기화 실패"});
            this.WinOCR_Language_comboBox.Location = new System.Drawing.Point(146, 9);
            this.WinOCR_Language_comboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.WinOCR_Language_comboBox.Name = "WinOCR_Language_comboBox";
            this.WinOCR_Language_comboBox.Size = new System.Drawing.Size(246, 36);
            this.WinOCR_Language_comboBox.TabIndex = 52;
            this.WinOCR_Language_comboBox.SelectionChangeCommitted += new System.EventHandler(this.WinOCR_Language_comboBox_SelectionChangeCommitted);
            // 
            // lbWinOCRLanguage
            // 
            this.lbWinOCRLanguage.AutoSize = true;
            this.lbWinOCRLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbWinOCRLanguage.ForeColor = System.Drawing.Color.White;
            this.lbWinOCRLanguage.Location = new System.Drawing.Point(12, 14);
            this.lbWinOCRLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbWinOCRLanguage.Name = "lbWinOCRLanguage";
            this.lbWinOCRLanguage.Size = new System.Drawing.Size(59, 28);
            this.lbWinOCRLanguage.TabIndex = 50;
            this.lbWinOCRLanguage.Text = "언어 ";
            // 
            // pnTranslate
            // 
            this.pnTranslate.Controls.Add(this.btnTransHelp);
            this.pnTranslate.Controls.Add(this.cbPerWordDic);
            this.pnTranslate.Controls.Add(this.lbTransType);
            this.pnTranslate.Controls.Add(this.TransType_Combobox);
            this.pnTranslate.Controls.Add(this.checkDic);
            this.pnTranslate.Controls.Add(this.dicFileTextBox);
            this.pnTranslate.Controls.Add(this.lbDicFile);
            this.pnTranslate.Controls.Add(this.lbTransTypeTitle);
            this.pnTranslate.Controls.Add(this.pnDeepl);
            this.pnTranslate.Controls.Add(this.pnGoogleBasic);
            this.pnTranslate.Controls.Add(this.DB_Panel);
            this.pnTranslate.Controls.Add(this.Naver_Panel);
            this.pnTranslate.Controls.Add(this.Google_Panel);
            this.pnTranslate.Controls.Add(this.pnEzTrans);
            this.pnTranslate.Location = new System.Drawing.Point(4, 246);
            this.pnTranslate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnTranslate.Name = "pnTranslate";
            this.pnTranslate.Size = new System.Drawing.Size(770, 326);
            this.pnTranslate.TabIndex = 37;
            this.pnTranslate.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // btnTransHelp
            // 
            this.btnTransHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(91)))), ((int)(((byte)(91)))));
            this.btnTransHelp.FlatAppearance.BorderSize = 0;
            this.btnTransHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransHelp.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTransHelp.ForeColor = System.Drawing.Color.White;
            this.btnTransHelp.Location = new System.Drawing.Point(426, 45);
            this.btnTransHelp.Margin = new System.Windows.Forms.Padding(0);
            this.btnTransHelp.Name = "btnTransHelp";
            this.btnTransHelp.Size = new System.Drawing.Size(42, 38);
            this.btnTransHelp.TabIndex = 58;
            this.btnTransHelp.Text = "?";
            this.btnTransHelp.UseVisualStyleBackColor = false;
            this.btnTransHelp.Click += new System.EventHandler(this.OnClick_btnTransHelp);
            // 
            // cbPerWordDic
            // 
            this.cbPerWordDic.AutoSize = true;
            this.cbPerWordDic.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbPerWordDic.ForeColor = System.Drawing.Color.White;
            this.cbPerWordDic.Location = new System.Drawing.Point(16, 282);
            this.cbPerWordDic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbPerWordDic.Name = "cbPerWordDic";
            this.cbPerWordDic.Size = new System.Drawing.Size(454, 32);
            this.cbPerWordDic.TabIndex = 57;
            this.cbPerWordDic.Text = "단어 단위로 교정 (완벽히 일치한 단어만 교정)";
            this.cbPerWordDic.UseVisualStyleBackColor = true;
            // 
            // lbTransType
            // 
            this.lbTransType.AutoSize = true;
            this.lbTransType.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbTransType.ForeColor = System.Drawing.Color.White;
            this.lbTransType.Location = new System.Drawing.Point(14, 50);
            this.lbTransType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTransType.Name = "lbTransType";
            this.lbTransType.Size = new System.Drawing.Size(92, 28);
            this.lbTransType.TabIndex = 20;
            this.lbTransType.Text = "번역방법";
            // 
            // TransType_Combobox
            // 
            this.TransType_Combobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TransType_Combobox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.TransType_Combobox.FormattingEnabled = true;
            this.TransType_Combobox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TransType_Combobox.Items.AddRange(new object[] {
            "TRANSLATE GOOGLE",
            "TRANSLATE DB",
            "TRANSLATE PAPAGO WEB",
            "TRANSLATE NAVER",
            "TRANSLATE GOOGLE SHEET",
            "TRANSLATE DEEPL",
            "TRANSLATE EZTRANS"});
            this.TransType_Combobox.Location = new System.Drawing.Point(158, 45);
            this.TransType_Combobox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TransType_Combobox.Name = "TransType_Combobox";
            this.TransType_Combobox.Size = new System.Drawing.Size(246, 36);
            this.TransType_Combobox.TabIndex = 49;
            this.TransType_Combobox.SelectedIndexChanged += new System.EventHandler(this.TransType_Combobox_SelectedIndexChanged);
            // 
            // checkDic
            // 
            this.checkDic.AutoSize = true;
            this.checkDic.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.checkDic.ForeColor = System.Drawing.Color.White;
            this.checkDic.Location = new System.Drawing.Point(16, 242);
            this.checkDic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkDic.Name = "checkDic";
            this.checkDic.Size = new System.Drawing.Size(165, 32);
            this.checkDic.TabIndex = 24;
            this.checkDic.Text = "교정사전 사용";
            this.checkDic.UseVisualStyleBackColor = true;
            this.checkDic.CheckedChanged += new System.EventHandler(this.checkDic_CheckedChanged);
            // 
            // dicFileTextBox
            // 
            this.dicFileTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.dicFileTextBox.Location = new System.Drawing.Point(338, 238);
            this.dicFileTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dicFileTextBox.Name = "dicFileTextBox";
            this.dicFileTextBox.Size = new System.Drawing.Size(376, 33);
            this.dicFileTextBox.TabIndex = 23;
            // 
            // lbDicFile
            // 
            this.lbDicFile.AutoSize = true;
            this.lbDicFile.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbDicFile.ForeColor = System.Drawing.Color.White;
            this.lbDicFile.Location = new System.Drawing.Point(208, 243);
            this.lbDicFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDicFile.Name = "lbDicFile";
            this.lbDicFile.Size = new System.Drawing.Size(92, 28);
            this.lbDicFile.TabIndex = 22;
            this.lbDicFile.Text = "파일이름";
            // 
            // lbTransTypeTitle
            // 
            this.lbTransTypeTitle.AutoSize = true;
            this.lbTransTypeTitle.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTransTypeTitle.ForeColor = System.Drawing.Color.White;
            this.lbTransTypeTitle.Location = new System.Drawing.Point(6, 4);
            this.lbTransTypeTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTransTypeTitle.Name = "lbTransTypeTitle";
            this.lbTransTypeTitle.Size = new System.Drawing.Size(114, 31);
            this.lbTransTypeTitle.TabIndex = 8;
            this.lbTransTypeTitle.Text = "번역 설정";
            // 
            // pnDeepl
            // 
            this.pnDeepl.Controls.Add(this.btnCheckDeeplState);
            this.pnDeepl.Controls.Add(this.lbDeepLStatus);
            this.pnDeepl.Controls.Add(this.lbDeepLInfo);
            this.pnDeepl.Location = new System.Drawing.Point(10, 92);
            this.pnDeepl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnDeepl.Name = "pnDeepl";
            this.pnDeepl.Size = new System.Drawing.Size(724, 141);
            this.pnDeepl.TabIndex = 54;
            // 
            // btnCheckDeeplState
            // 
            this.btnCheckDeeplState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCheckDeeplState.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnCheckDeeplState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckDeeplState.ForeColor = System.Drawing.Color.White;
            this.btnCheckDeeplState.Location = new System.Drawing.Point(483, 80);
            this.btnCheckDeeplState.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCheckDeeplState.Name = "btnCheckDeeplState";
            this.btnCheckDeeplState.Size = new System.Drawing.Size(225, 38);
            this.btnCheckDeeplState.TabIndex = 52;
            this.btnCheckDeeplState.Text = "상태 확인하기";
            this.btnCheckDeeplState.UseVisualStyleBackColor = false;
            this.btnCheckDeeplState.Click += new System.EventHandler(this.OnClickCheckDeeplState);
            // 
            // lbDeepLStatus
            // 
            this.lbDeepLStatus.AutoSize = true;
            this.lbDeepLStatus.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbDeepLStatus.ForeColor = System.Drawing.Color.White;
            this.lbDeepLStatus.Location = new System.Drawing.Point(4, 93);
            this.lbDeepLStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDeepLStatus.Name = "lbDeepLStatus";
            this.lbDeepLStatus.Size = new System.Drawing.Size(131, 28);
            this.lbDeepLStatus.TabIndex = 18;
            this.lbDeepLStatus.Text = "상태 : 고품질";
            this.lbDeepLStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDeepLInfo
            // 
            this.lbDeepLInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbDeepLInfo.ForeColor = System.Drawing.Color.White;
            this.lbDeepLInfo.Location = new System.Drawing.Point(4, 12);
            this.lbDeepLInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDeepLInfo.Name = "lbDeepLInfo";
            this.lbDeepLInfo.Size = new System.Drawing.Size(704, 51);
            this.lbDeepLInfo.TabIndex = 17;
            this.lbDeepLInfo.Text = "사용을 위해 마이크로소프트 엣지가 필요합니다";
            this.lbDeepLInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnGoogleBasic
            // 
            this.pnGoogleBasic.Controls.Add(this.lbBasicStatus);
            this.pnGoogleBasic.Controls.Add(this.lbBasicInfo);
            this.pnGoogleBasic.Location = new System.Drawing.Point(10, 92);
            this.pnGoogleBasic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnGoogleBasic.Name = "pnGoogleBasic";
            this.pnGoogleBasic.Size = new System.Drawing.Size(724, 141);
            this.pnGoogleBasic.TabIndex = 53;
            // 
            // lbBasicStatus
            // 
            this.lbBasicStatus.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbBasicStatus.ForeColor = System.Drawing.Color.White;
            this.lbBasicStatus.Location = new System.Drawing.Point(4, 90);
            this.lbBasicStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBasicStatus.Name = "lbBasicStatus";
            this.lbBasicStatus.Size = new System.Drawing.Size(502, 27);
            this.lbBasicStatus.TabIndex = 18;
            this.lbBasicStatus.Text = "상태 : 고품질";
            this.lbBasicStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbBasicInfo
            // 
            this.lbBasicInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbBasicInfo.ForeColor = System.Drawing.Color.White;
            this.lbBasicInfo.Location = new System.Drawing.Point(4, 32);
            this.lbBasicInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBasicInfo.Name = "lbBasicInfo";
            this.lbBasicInfo.Size = new System.Drawing.Size(704, 51);
            this.lbBasicInfo.TabIndex = 17;
            this.lbBasicInfo.Text = "구글 기본 번역기의 고품질 번역은 시간당 100회까지만 적용되며\r\n초과시 낮은 품질로 번역됩니다.";
            this.lbBasicInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DB_Panel
            // 
            this.DB_Panel.Controls.Add(this.cbDBMultiGet);
            this.DB_Panel.Controls.Add(this.checkStringUpper);
            this.DB_Panel.Controls.Add(this.dbFileTextBox);
            this.DB_Panel.Controls.Add(this.lbDbFile);
            this.DB_Panel.Location = new System.Drawing.Point(10, 92);
            this.DB_Panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DB_Panel.Name = "DB_Panel";
            this.DB_Panel.Size = new System.Drawing.Size(678, 141);
            this.DB_Panel.TabIndex = 50;
            // 
            // cbDBMultiGet
            // 
            this.cbDBMultiGet.AutoSize = true;
            this.cbDBMultiGet.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbDBMultiGet.ForeColor = System.Drawing.Color.White;
            this.cbDBMultiGet.Location = new System.Drawing.Point(8, 98);
            this.cbDBMultiGet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDBMultiGet.Name = "cbDBMultiGet";
            this.cbDBMultiGet.Size = new System.Drawing.Size(604, 32);
            this.cbDBMultiGet.TabIndex = 26;
            this.cbDBMultiGet.Text = "DB 부분 일치 검색 - 문장과 부분 일치한 번역문 모두 가져오기";
            this.cbDBMultiGet.UseVisualStyleBackColor = true;
            // 
            // checkStringUpper
            // 
            this.checkStringUpper.AutoSize = true;
            this.checkStringUpper.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.checkStringUpper.ForeColor = System.Drawing.Color.White;
            this.checkStringUpper.Location = new System.Drawing.Point(8, 60);
            this.checkStringUpper.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkStringUpper.Name = "checkStringUpper";
            this.checkStringUpper.Size = new System.Drawing.Size(328, 32);
            this.checkStringUpper.TabIndex = 25;
            this.checkStringUpper.Text = "DB 검색 시 대소문자 구분 안 함";
            this.checkStringUpper.UseVisualStyleBackColor = true;
            // 
            // dbFileTextBox
            // 
            this.dbFileTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.dbFileTextBox.Location = new System.Drawing.Point(147, 4);
            this.dbFileTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dbFileTextBox.Name = "dbFileTextBox";
            this.dbFileTextBox.Size = new System.Drawing.Size(376, 33);
            this.dbFileTextBox.TabIndex = 19;
            this.dbFileTextBox.Text = "empty.txt";
            // 
            // lbDbFile
            // 
            this.lbDbFile.AutoSize = true;
            this.lbDbFile.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbDbFile.ForeColor = System.Drawing.Color.White;
            this.lbDbFile.Location = new System.Drawing.Point(4, 12);
            this.lbDbFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDbFile.Name = "lbDbFile";
            this.lbDbFile.Size = new System.Drawing.Size(106, 28);
            this.lbDbFile.TabIndex = 16;
            this.lbDbFile.Text = "파일이름  ";
            // 
            // Naver_Panel
            // 
            this.Naver_Panel.Controls.Add(this.Button_NaverTransKeyList);
            this.Naver_Panel.Controls.Add(this.lbPapagoSecret);
            this.Naver_Panel.Controls.Add(this.NaverSecretKeyTextBox);
            this.Naver_Panel.Controls.Add(this.NaverIDKeyTextBox);
            this.Naver_Panel.Controls.Add(this.lbPapagoID);
            this.Naver_Panel.Location = new System.Drawing.Point(10, 92);
            this.Naver_Panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Naver_Panel.Name = "Naver_Panel";
            this.Naver_Panel.Size = new System.Drawing.Size(724, 141);
            this.Naver_Panel.TabIndex = 52;
            // 
            // Button_NaverTransKeyList
            // 
            this.Button_NaverTransKeyList.AutoSize = true;
            this.Button_NaverTransKeyList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(91)))), ((int)(((byte)(91)))));
            this.Button_NaverTransKeyList.FlatAppearance.BorderSize = 0;
            this.Button_NaverTransKeyList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_NaverTransKeyList.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.Button_NaverTransKeyList.ForeColor = System.Drawing.Color.White;
            this.Button_NaverTransKeyList.Location = new System.Drawing.Point(540, 9);
            this.Button_NaverTransKeyList.Margin = new System.Windows.Forms.Padding(0);
            this.Button_NaverTransKeyList.Name = "Button_NaverTransKeyList";
            this.Button_NaverTransKeyList.Size = new System.Drawing.Size(186, 126);
            this.Button_NaverTransKeyList.TabIndex = 52;
            this.Button_NaverTransKeyList.Text = "키 관리";
            this.Button_NaverTransKeyList.UseVisualStyleBackColor = false;
            this.Button_NaverTransKeyList.Click += new System.EventHandler(this.Button_NaverTransKeyList_Click);
            // 
            // lbPapagoSecret
            // 
            this.lbPapagoSecret.AutoSize = true;
            this.lbPapagoSecret.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbPapagoSecret.ForeColor = System.Drawing.Color.White;
            this.lbPapagoSecret.Location = new System.Drawing.Point(4, 57);
            this.lbPapagoSecret.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPapagoSecret.Name = "lbPapagoSecret";
            this.lbPapagoSecret.Size = new System.Drawing.Size(98, 28);
            this.lbPapagoSecret.TabIndex = 23;
            this.lbPapagoSecret.Text = "Secret 키";
            // 
            // NaverSecretKeyTextBox
            // 
            this.NaverSecretKeyTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.NaverSecretKeyTextBox.Location = new System.Drawing.Point(147, 51);
            this.NaverSecretKeyTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NaverSecretKeyTextBox.Name = "NaverSecretKeyTextBox";
            this.NaverSecretKeyTextBox.Size = new System.Drawing.Size(376, 33);
            this.NaverSecretKeyTextBox.TabIndex = 22;
            // 
            // NaverIDKeyTextBox
            // 
            this.NaverIDKeyTextBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.NaverIDKeyTextBox.Location = new System.Drawing.Point(147, 4);
            this.NaverIDKeyTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NaverIDKeyTextBox.Name = "NaverIDKeyTextBox";
            this.NaverIDKeyTextBox.Size = new System.Drawing.Size(376, 33);
            this.NaverIDKeyTextBox.TabIndex = 21;
            // 
            // lbPapagoID
            // 
            this.lbPapagoID.AutoSize = true;
            this.lbPapagoID.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbPapagoID.ForeColor = System.Drawing.Color.White;
            this.lbPapagoID.Location = new System.Drawing.Point(4, 12);
            this.lbPapagoID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPapagoID.Name = "lbPapagoID";
            this.lbPapagoID.Size = new System.Drawing.Size(67, 28);
            this.lbPapagoID.TabIndex = 17;
            this.lbPapagoID.Text = "ID 키 ";
            // 
            // Google_Panel
            // 
            this.Google_Panel.Controls.Add(this.button_RemoveAllGoogleToekn);
            this.Google_Panel.Controls.Add(this.textBox_GoogleSecretKey);
            this.Google_Panel.Controls.Add(this.lbSheetSecret);
            this.Google_Panel.Controls.Add(this.textBox_GoogleClientID);
            this.Google_Panel.Controls.Add(this.lbSheetID);
            this.Google_Panel.Controls.Add(this.googleSheet_textBox);
            this.Google_Panel.Controls.Add(this.lbGoogleSheetAddress);
            this.Google_Panel.Location = new System.Drawing.Point(10, 92);
            this.Google_Panel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Google_Panel.Name = "Google_Panel";
            this.Google_Panel.Size = new System.Drawing.Size(724, 138);
            this.Google_Panel.TabIndex = 53;
            // 
            // button_RemoveAllGoogleToekn
            // 
            this.button_RemoveAllGoogleToekn.AutoSize = true;
            this.button_RemoveAllGoogleToekn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(91)))), ((int)(((byte)(91)))));
            this.button_RemoveAllGoogleToekn.FlatAppearance.BorderSize = 0;
            this.button_RemoveAllGoogleToekn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_RemoveAllGoogleToekn.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold);
            this.button_RemoveAllGoogleToekn.ForeColor = System.Drawing.Color.White;
            this.button_RemoveAllGoogleToekn.Location = new System.Drawing.Point(540, 9);
            this.button_RemoveAllGoogleToekn.Margin = new System.Windows.Forms.Padding(0);
            this.button_RemoveAllGoogleToekn.Name = "button_RemoveAllGoogleToekn";
            this.button_RemoveAllGoogleToekn.Size = new System.Drawing.Size(230, 135);
            this.button_RemoveAllGoogleToekn.TabIndex = 45;
            this.button_RemoveAllGoogleToekn.Text = "모든 인증\r\n초기화";
            this.button_RemoveAllGoogleToekn.UseVisualStyleBackColor = false;
            this.button_RemoveAllGoogleToekn.Click += new System.EventHandler(this.button_RemoveAllGoogleToekn_Click);
            // 
            // textBox_GoogleSecretKey
            // 
            this.textBox_GoogleSecretKey.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBox_GoogleSecretKey.Location = new System.Drawing.Point(147, 98);
            this.textBox_GoogleSecretKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_GoogleSecretKey.Name = "textBox_GoogleSecretKey";
            this.textBox_GoogleSecretKey.Size = new System.Drawing.Size(376, 33);
            this.textBox_GoogleSecretKey.TabIndex = 27;
            // 
            // lbSheetSecret
            // 
            this.lbSheetSecret.AutoSize = true;
            this.lbSheetSecret.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbSheetSecret.ForeColor = System.Drawing.Color.White;
            this.lbSheetSecret.Location = new System.Drawing.Point(4, 100);
            this.lbSheetSecret.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSheetSecret.Name = "lbSheetSecret";
            this.lbSheetSecret.Size = new System.Drawing.Size(98, 28);
            this.lbSheetSecret.TabIndex = 26;
            this.lbSheetSecret.Text = "Secret 키";
            // 
            // textBox_GoogleClientID
            // 
            this.textBox_GoogleClientID.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBox_GoogleClientID.Location = new System.Drawing.Point(147, 51);
            this.textBox_GoogleClientID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_GoogleClientID.Name = "textBox_GoogleClientID";
            this.textBox_GoogleClientID.Size = new System.Drawing.Size(376, 33);
            this.textBox_GoogleClientID.TabIndex = 25;
            // 
            // lbSheetID
            // 
            this.lbSheetID.AutoSize = true;
            this.lbSheetID.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbSheetID.ForeColor = System.Drawing.Color.White;
            this.lbSheetID.Location = new System.Drawing.Point(4, 57);
            this.lbSheetID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSheetID.Name = "lbSheetID";
            this.lbSheetID.Size = new System.Drawing.Size(94, 28);
            this.lbSheetID.TabIndex = 24;
            this.lbSheetID.Text = "Client ID";
            // 
            // googleSheet_textBox
            // 
            this.googleSheet_textBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.googleSheet_textBox.Location = new System.Drawing.Point(147, 4);
            this.googleSheet_textBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.googleSheet_textBox.Name = "googleSheet_textBox";
            this.googleSheet_textBox.Size = new System.Drawing.Size(376, 33);
            this.googleSheet_textBox.TabIndex = 21;
            // 
            // lbGoogleSheetAddress
            // 
            this.lbGoogleSheetAddress.AutoSize = true;
            this.lbGoogleSheetAddress.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbGoogleSheetAddress.ForeColor = System.Drawing.Color.White;
            this.lbGoogleSheetAddress.Location = new System.Drawing.Point(4, 12);
            this.lbGoogleSheetAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbGoogleSheetAddress.Name = "lbGoogleSheetAddress";
            this.lbGoogleSheetAddress.Size = new System.Drawing.Size(99, 28);
            this.lbGoogleSheetAddress.TabIndex = 17;
            this.lbGoogleSheetAddress.Text = "시트 주소";
            // 
            // pnEzTrans
            // 
            this.pnEzTrans.Controls.Add(this.lbEzTransInfo);
            this.pnEzTrans.Location = new System.Drawing.Point(10, 92);
            this.pnEzTrans.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnEzTrans.Name = "pnEzTrans";
            this.pnEzTrans.Size = new System.Drawing.Size(724, 141);
            this.pnEzTrans.TabIndex = 54;
            // 
            // lbEzTransInfo
            // 
            this.lbEzTransInfo.AutoSize = true;
            this.lbEzTransInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbEzTransInfo.ForeColor = System.Drawing.Color.White;
            this.lbEzTransInfo.Location = new System.Drawing.Point(118, 32);
            this.lbEzTransInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbEzTransInfo.Name = "lbEzTransInfo";
            this.lbEzTransInfo.Size = new System.Drawing.Size(479, 84);
            this.lbEzTransInfo.TabIndex = 17;
            this.lbEzTransInfo.Text = "일본어 전용\r\nezTrans XP가 설치 되어 있어야 합니다.\r\n자세한 사용법은 번역 설정 도움말을 확인해 주세요.";
            this.lbEzTransInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tpText
            // 
            this.tpText.Controls.Add(this.panel5);
            this.tpText.Location = new System.Drawing.Point(80, 4);
            this.tpText.Margin = new System.Windows.Forms.Padding(0);
            this.tpText.Name = "tpText";
            this.tpText.Size = new System.Drawing.Size(818, 882);
            this.tpText.TabIndex = 1;
            this.tpText.Text = "텍스트";
            this.tpText.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.panel5.Controls.Add(this.panel17);
            this.panel5.Controls.Add(this.panel10);
            this.panel5.Controls.Add(this.panel9);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(818, 882);
            this.panel5.TabIndex = 0;
            // 
            // panel17
            // 
            this.panel17.Controls.Add(this.lbPreview);
            this.panel17.Controls.Add(this.fontResultLabel);
            this.panel17.Location = new System.Drawing.Point(4, 422);
            this.panel17.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(770, 430);
            this.panel17.TabIndex = 40;
            this.panel17.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // lbPreview
            // 
            this.lbPreview.AutoSize = true;
            this.lbPreview.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbPreview.ForeColor = System.Drawing.Color.White;
            this.lbPreview.Location = new System.Drawing.Point(6, 4);
            this.lbPreview.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPreview.Name = "lbPreview";
            this.lbPreview.Size = new System.Drawing.Size(106, 31);
            this.lbPreview.TabIndex = 8;
            this.lbPreview.Text = "미리보기";
            // 
            // fontResultLabel
            // 
            this.fontResultLabel.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.fontResultLabel.IsAlignmentCenter = false;
            this.fontResultLabel.IsFillBackColor = false;
            this.fontResultLabel.Location = new System.Drawing.Point(18, 51);
            this.fontResultLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fontResultLabel.Name = "fontResultLabel";
            this.fontResultLabel.OutlineForeColor = System.Drawing.Color.Blue;
            this.fontResultLabel.OutlineForecolor2 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.fontResultLabel.Size = new System.Drawing.Size(736, 374);
            this.fontResultLabel.TabIndex = 39;
            this.fontResultLabel.Text = "-설정 결과를 미리 봅니다.\r\n-어두운 번역창에는 적용되지 않습니다.\r\n\r\n-1 2 3 4 5 6\r\n-Tank division!";
            this.fontResultLabel.TextColor = System.Drawing.Color.White;
            this.fontResultLabel.TextFont = new System.Drawing.Font("맑은 고딕", 15F);
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.defaultColorButton);
            this.panel10.Controls.Add(this.lbFontBackground);
            this.panel10.Controls.Add(this.lbFontOutlineColor2);
            this.panel10.Controls.Add(this.lbFontOutlineColor1);
            this.panel10.Controls.Add(this.backgroundColorBox);
            this.panel10.Controls.Add(this.outlineColor2Box);
            this.panel10.Controls.Add(this.outlineColor1Box);
            this.panel10.Controls.Add(this.lbFontBasicColor);
            this.panel10.Controls.Add(this.textColorBox);
            this.panel10.Controls.Add(this.lbFontColor);
            this.panel10.Location = new System.Drawing.Point(4, 117);
            this.panel10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(770, 160);
            this.panel10.TabIndex = 38;
            this.panel10.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // defaultColorButton
            // 
            this.defaultColorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.defaultColorButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.defaultColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.defaultColorButton.ForeColor = System.Drawing.Color.White;
            this.defaultColorButton.Location = new System.Drawing.Point(14, 118);
            this.defaultColorButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.defaultColorButton.Name = "defaultColorButton";
            this.defaultColorButton.Size = new System.Drawing.Size(744, 38);
            this.defaultColorButton.TabIndex = 25;
            this.defaultColorButton.Text = "기본 색으로";
            this.defaultColorButton.UseVisualStyleBackColor = false;
            this.defaultColorButton.Click += new System.EventHandler(this.defaultColorButton_Click);
            // 
            // lbFontBackground
            // 
            this.lbFontBackground.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbFontBackground.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbFontBackground.ForeColor = System.Drawing.Color.White;
            this.lbFontBackground.Location = new System.Drawing.Point(516, 72);
            this.lbFontBackground.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFontBackground.Name = "lbFontBackground";
            this.lbFontBackground.Size = new System.Drawing.Size(135, 30);
            this.lbFontBackground.TabIndex = 30;
            this.lbFontBackground.Text = "배경색";
            this.lbFontBackground.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbFontOutlineColor2
            // 
            this.lbFontOutlineColor2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbFontOutlineColor2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbFontOutlineColor2.ForeColor = System.Drawing.Color.White;
            this.lbFontOutlineColor2.Location = new System.Drawing.Point(381, 72);
            this.lbFontOutlineColor2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFontOutlineColor2.Name = "lbFontOutlineColor2";
            this.lbFontOutlineColor2.Size = new System.Drawing.Size(135, 30);
            this.lbFontOutlineColor2.TabIndex = 29;
            this.lbFontOutlineColor2.Text = "외곽선2";
            this.lbFontOutlineColor2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbFontOutlineColor1
            // 
            this.lbFontOutlineColor1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbFontOutlineColor1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbFontOutlineColor1.ForeColor = System.Drawing.Color.White;
            this.lbFontOutlineColor1.Location = new System.Drawing.Point(246, 72);
            this.lbFontOutlineColor1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFontOutlineColor1.Name = "lbFontOutlineColor1";
            this.lbFontOutlineColor1.Size = new System.Drawing.Size(135, 30);
            this.lbFontOutlineColor1.TabIndex = 28;
            this.lbFontOutlineColor1.Text = "외곽선1";
            this.lbFontOutlineColor1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // backgroundColorBox
            // 
            this.backgroundColorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.backgroundColorBox.Location = new System.Drawing.Point(561, 36);
            this.backgroundColorBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.backgroundColorBox.Name = "backgroundColorBox";
            this.backgroundColorBox.Size = new System.Drawing.Size(36, 36);
            this.backgroundColorBox.TabIndex = 27;
            this.backgroundColorBox.TabStop = false;
            this.backgroundColorBox.Click += new System.EventHandler(this.backgroundColorBox_Click);
            // 
            // outlineColor2Box
            // 
            this.outlineColor2Box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.outlineColor2Box.Location = new System.Drawing.Point(430, 36);
            this.outlineColor2Box.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.outlineColor2Box.Name = "outlineColor2Box";
            this.outlineColor2Box.Size = new System.Drawing.Size(36, 36);
            this.outlineColor2Box.TabIndex = 26;
            this.outlineColor2Box.TabStop = false;
            this.outlineColor2Box.Click += new System.EventHandler(this.outlineColor2Box_Click);
            // 
            // outlineColor1Box
            // 
            this.outlineColor1Box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.outlineColor1Box.Location = new System.Drawing.Point(296, 36);
            this.outlineColor1Box.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.outlineColor1Box.Name = "outlineColor1Box";
            this.outlineColor1Box.Size = new System.Drawing.Size(36, 36);
            this.outlineColor1Box.TabIndex = 25;
            this.outlineColor1Box.TabStop = false;
            this.outlineColor1Box.Click += new System.EventHandler(this.outlineColor1Box_Click);
            // 
            // lbFontBasicColor
            // 
            this.lbFontBasicColor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbFontBasicColor.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbFontBasicColor.ForeColor = System.Drawing.Color.White;
            this.lbFontBasicColor.Location = new System.Drawing.Point(111, 72);
            this.lbFontBasicColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFontBasicColor.Name = "lbFontBasicColor";
            this.lbFontBasicColor.Size = new System.Drawing.Size(135, 30);
            this.lbFontBasicColor.TabIndex = 24;
            this.lbFontBasicColor.Text = "색상";
            this.lbFontBasicColor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textColorBox
            // 
            this.textColorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.textColorBox.Location = new System.Drawing.Point(160, 36);
            this.textColorBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textColorBox.Name = "textColorBox";
            this.textColorBox.Size = new System.Drawing.Size(36, 36);
            this.textColorBox.TabIndex = 24;
            this.textColorBox.TabStop = false;
            this.textColorBox.Click += new System.EventHandler(this.textColorBox_Click);
            // 
            // lbFontColor
            // 
            this.lbFontColor.AutoSize = true;
            this.lbFontColor.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbFontColor.ForeColor = System.Drawing.Color.White;
            this.lbFontColor.Location = new System.Drawing.Point(6, 4);
            this.lbFontColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFontColor.Name = "lbFontColor";
            this.lbFontColor.Size = new System.Drawing.Size(37, 31);
            this.lbFontColor.TabIndex = 8;
            this.lbFontColor.Text = "색";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.cbShowOCRIndex);
            this.panel9.Controls.Add(this.useBackColorCheckBox);
            this.panel9.Controls.Add(this.removeSpaceCheckBox);
            this.panel9.Controls.Add(this.alignmentCenterCheckBox);
            this.panel9.Controls.Add(this.lbTextAdditionalSettings);
            this.panel9.Location = new System.Drawing.Point(4, 286);
            this.panel9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(770, 126);
            this.panel9.TabIndex = 38;
            this.panel9.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // cbShowOCRIndex
            // 
            this.cbShowOCRIndex.AutoSize = true;
            this.cbShowOCRIndex.Checked = true;
            this.cbShowOCRIndex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowOCRIndex.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbShowOCRIndex.ForeColor = System.Drawing.Color.White;
            this.cbShowOCRIndex.Location = new System.Drawing.Point(26, 80);
            this.cbShowOCRIndex.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbShowOCRIndex.Name = "cbShowOCRIndex";
            this.cbShowOCRIndex.Size = new System.Drawing.Size(201, 29);
            this.cbShowOCRIndex.TabIndex = 12;
            this.cbShowOCRIndex.Text = "OCR 영역 번호 표시";
            this.cbShowOCRIndex.UseVisualStyleBackColor = true;
            this.cbShowOCRIndex.CheckedChanged += new System.EventHandler(this.cbShowOCRIndex_CheckedChanged);
            // 
            // useBackColorCheckBox
            // 
            this.useBackColorCheckBox.AutoSize = true;
            this.useBackColorCheckBox.Checked = true;
            this.useBackColorCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useBackColorCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.useBackColorCheckBox.ForeColor = System.Drawing.Color.White;
            this.useBackColorCheckBox.Location = new System.Drawing.Point(608, 39);
            this.useBackColorCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.useBackColorCheckBox.Name = "useBackColorCheckBox";
            this.useBackColorCheckBox.Size = new System.Drawing.Size(134, 29);
            this.useBackColorCheckBox.TabIndex = 11;
            this.useBackColorCheckBox.Text = "배경색 사용";
            this.useBackColorCheckBox.UseVisualStyleBackColor = true;
            this.useBackColorCheckBox.CheckedChanged += new System.EventHandler(this.useBackColorCheckBox_CheckedChanged);
            // 
            // removeSpaceCheckBox
            // 
            this.removeSpaceCheckBox.AutoSize = true;
            this.removeSpaceCheckBox.Checked = true;
            this.removeSpaceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removeSpaceCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.removeSpaceCheckBox.ForeColor = System.Drawing.Color.White;
            this.removeSpaceCheckBox.Location = new System.Drawing.Point(279, 39);
            this.removeSpaceCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.removeSpaceCheckBox.Name = "removeSpaceCheckBox";
            this.removeSpaceCheckBox.Size = new System.Drawing.Size(201, 29);
            this.removeSpaceCheckBox.TabIndex = 10;
            this.removeSpaceCheckBox.Text = "OCR 결과 공백 제거";
            this.removeSpaceCheckBox.UseVisualStyleBackColor = true;
            this.removeSpaceCheckBox.CheckedChanged += new System.EventHandler(this.removeSpaceCheckBox_CheckedChanged);
            // 
            // alignmentCenterCheckBox
            // 
            this.alignmentCenterCheckBox.AutoSize = true;
            this.alignmentCenterCheckBox.Checked = true;
            this.alignmentCenterCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.alignmentCenterCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.alignmentCenterCheckBox.ForeColor = System.Drawing.Color.White;
            this.alignmentCenterCheckBox.Location = new System.Drawing.Point(26, 39);
            this.alignmentCenterCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.alignmentCenterCheckBox.Name = "alignmentCenterCheckBox";
            this.alignmentCenterCheckBox.Size = new System.Drawing.Size(134, 29);
            this.alignmentCenterCheckBox.TabIndex = 9;
            this.alignmentCenterCheckBox.Text = "가운데 정렬";
            this.alignmentCenterCheckBox.UseVisualStyleBackColor = true;
            this.alignmentCenterCheckBox.CheckedChanged += new System.EventHandler(this.alignmentCenterCheckBox_CheckedChanged);
            // 
            // lbTextAdditionalSettings
            // 
            this.lbTextAdditionalSettings.AutoSize = true;
            this.lbTextAdditionalSettings.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTextAdditionalSettings.ForeColor = System.Drawing.Color.White;
            this.lbTextAdditionalSettings.Location = new System.Drawing.Point(6, 4);
            this.lbTextAdditionalSettings.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTextAdditionalSettings.Name = "lbTextAdditionalSettings";
            this.lbTextAdditionalSettings.Size = new System.Drawing.Size(106, 31);
            this.lbTextAdditionalSettings.TabIndex = 8;
            this.lbTextAdditionalSettings.Text = "부가설정";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.fontSizeUpDown);
            this.panel7.Controls.Add(this.lbFontSize);
            this.panel7.Controls.Add(this.lbFont);
            this.panel7.Controls.Add(this.fontButton);
            this.panel7.Controls.Add(this.lbFontSetting);
            this.panel7.Location = new System.Drawing.Point(4, 4);
            this.panel7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(770, 104);
            this.panel7.TabIndex = 37;
            this.panel7.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // fontSizeUpDown
            // 
            this.fontSizeUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.fontSizeUpDown.ForeColor = System.Drawing.Color.White;
            this.fontSizeUpDown.Location = new System.Drawing.Point(477, 54);
            this.fontSizeUpDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fontSizeUpDown.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.fontSizeUpDown.Name = "fontSizeUpDown";
            this.fontSizeUpDown.Size = new System.Drawing.Size(70, 31);
            this.fontSizeUpDown.TabIndex = 24;
            this.fontSizeUpDown.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.fontSizeUpDown.ValueChanged += new System.EventHandler(this.fontSizeUpDown_ValueChanged);
            // 
            // lbFontSize
            // 
            this.lbFontSize.AutoSize = true;
            this.lbFontSize.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbFontSize.ForeColor = System.Drawing.Color.White;
            this.lbFontSize.Location = new System.Drawing.Point(382, 54);
            this.lbFontSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFontSize.Name = "lbFontSize";
            this.lbFontSize.Size = new System.Drawing.Size(64, 28);
            this.lbFontSize.TabIndex = 22;
            this.lbFontSize.Text = "크기 :";
            // 
            // lbFont
            // 
            this.lbFont.AutoSize = true;
            this.lbFont.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbFont.ForeColor = System.Drawing.Color.White;
            this.lbFont.Location = new System.Drawing.Point(20, 54);
            this.lbFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFont.Name = "lbFont";
            this.lbFont.Size = new System.Drawing.Size(71, 28);
            this.lbFont.TabIndex = 20;
            this.lbFont.Text = "글꼴 : ";
            // 
            // fontButton
            // 
            this.fontButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.fontButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.fontButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fontButton.ForeColor = System.Drawing.Color.White;
            this.fontButton.Location = new System.Drawing.Point(99, 50);
            this.fontButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fontButton.Name = "fontButton";
            this.fontButton.Size = new System.Drawing.Size(228, 38);
            this.fontButton.TabIndex = 9;
            this.fontButton.Text = "폰트설정";
            this.fontButton.UseVisualStyleBackColor = false;
            this.fontButton.Click += new System.EventHandler(this.fontButton_Click);
            // 
            // lbFontSetting
            // 
            this.lbFontSetting.AutoSize = true;
            this.lbFontSetting.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbFontSetting.ForeColor = System.Drawing.Color.White;
            this.lbFontSetting.Location = new System.Drawing.Point(6, 4);
            this.lbFontSetting.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFontSetting.Name = "lbFontSetting";
            this.lbFontSetting.Size = new System.Drawing.Size(106, 31);
            this.lbFontSetting.TabIndex = 8;
            this.lbFontSetting.Text = "폰트설정";
            // 
            // tpExtra
            // 
            this.tpExtra.Controls.Add(this.panel11);
            this.tpExtra.Location = new System.Drawing.Point(80, 4);
            this.tpExtra.Margin = new System.Windows.Forms.Padding(0);
            this.tpExtra.Name = "tpExtra";
            this.tpExtra.Size = new System.Drawing.Size(818, 882);
            this.tpExtra.TabIndex = 2;
            this.tpExtra.Text = "부가설정";
            this.tpExtra.UseVisualStyleBackColor = true;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.panel11.Controls.Add(this.panel21);
            this.panel11.Controls.Add(this.panel25);
            this.panel11.Controls.Add(this.panel3);
            this.panel11.Controls.Add(this.panel13);
            this.panel11.Controls.Add(this.panel12);
            this.panel11.Controls.Add(this.panel14);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Margin = new System.Windows.Forms.Padding(0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(818, 882);
            this.panel11.TabIndex = 1;
            // 
            // panel21
            // 
            this.panel21.Controls.Add(this.btAdvencedOption);
            this.panel21.Controls.Add(this.lbAdvencedConfig);
            this.panel21.Location = new System.Drawing.Point(6, 750);
            this.panel21.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(770, 104);
            this.panel21.TabIndex = 41;
            this.panel21.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // btAdvencedOption
            // 
            this.btAdvencedOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btAdvencedOption.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btAdvencedOption.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAdvencedOption.ForeColor = System.Drawing.Color.White;
            this.btAdvencedOption.Location = new System.Drawing.Point(12, 52);
            this.btAdvencedOption.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btAdvencedOption.Name = "btAdvencedOption";
            this.btAdvencedOption.Size = new System.Drawing.Size(744, 38);
            this.btAdvencedOption.TabIndex = 25;
            this.btAdvencedOption.Text = "고급 설정";
            this.btAdvencedOption.UseVisualStyleBackColor = false;
            this.btAdvencedOption.Click += new System.EventHandler(this.OnClick_btAdvencedOption);
            // 
            // lbAdvencedConfig
            // 
            this.lbAdvencedConfig.AutoSize = true;
            this.lbAdvencedConfig.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbAdvencedConfig.ForeColor = System.Drawing.Color.White;
            this.lbAdvencedConfig.Location = new System.Drawing.Point(6, 4);
            this.lbAdvencedConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAdvencedConfig.Name = "lbAdvencedConfig";
            this.lbAdvencedConfig.Size = new System.Drawing.Size(114, 31);
            this.lbAdvencedConfig.TabIndex = 8;
            this.lbAdvencedConfig.Text = "고급 설정";
            // 
            // panel25
            // 
            this.panel25.Controls.Add(this.btSettingUpload);
            this.panel25.Controls.Add(this.btSettingBrowser);
            this.panel25.Controls.Add(this.lbSearchConfig);
            this.panel25.Location = new System.Drawing.Point(6, 588);
            this.panel25.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel25.Name = "panel25";
            this.panel25.Size = new System.Drawing.Size(770, 153);
            this.panel25.TabIndex = 40;
            this.panel25.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // btSettingUpload
            // 
            this.btSettingUpload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btSettingUpload.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btSettingUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSettingUpload.ForeColor = System.Drawing.Color.White;
            this.btSettingUpload.Location = new System.Drawing.Point(12, 98);
            this.btSettingUpload.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btSettingUpload.Name = "btSettingUpload";
            this.btSettingUpload.Size = new System.Drawing.Size(744, 38);
            this.btSettingUpload.TabIndex = 26;
            this.btSettingUpload.Text = "설정 업로드";
            this.btSettingUpload.UseVisualStyleBackColor = false;
            this.btSettingUpload.Click += new System.EventHandler(this.OnClick_btSettingUpload);
            // 
            // btSettingBrowser
            // 
            this.btSettingBrowser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btSettingBrowser.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btSettingBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSettingBrowser.ForeColor = System.Drawing.Color.White;
            this.btSettingBrowser.Location = new System.Drawing.Point(12, 52);
            this.btSettingBrowser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btSettingBrowser.Name = "btSettingBrowser";
            this.btSettingBrowser.Size = new System.Drawing.Size(744, 38);
            this.btSettingBrowser.TabIndex = 25;
            this.btSettingBrowser.Text = "설정 검색";
            this.btSettingBrowser.UseVisualStyleBackColor = false;
            this.btSettingBrowser.Click += new System.EventHandler(this.Onclick_btSettingBrowser);
            // 
            // lbSearchConfig
            // 
            this.lbSearchConfig.AutoSize = true;
            this.lbSearchConfig.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSearchConfig.ForeColor = System.Drawing.Color.White;
            this.lbSearchConfig.Location = new System.Drawing.Point(6, 4);
            this.lbSearchConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSearchConfig.Name = "lbSearchConfig";
            this.lbSearchConfig.Size = new System.Drawing.Size(114, 31);
            this.lbSearchConfig.TabIndex = 8;
            this.lbSearchConfig.Text = "설정 검색";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.speedRadioButton5);
            this.panel3.Controls.Add(this.lbSpeed);
            this.panel3.Controls.Add(this.lbSpeedInformation);
            this.panel3.Controls.Add(this.speedRadioButton4);
            this.panel3.Controls.Add(this.speedRadioButton1);
            this.panel3.Controls.Add(this.speedRadioButton3);
            this.panel3.Controls.Add(this.speedRadioButton2);
            this.panel3.Location = new System.Drawing.Point(4, 256);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(770, 118);
            this.panel3.TabIndex = 42;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // speedRadioButton5
            // 
            this.speedRadioButton5.AutoSize = true;
            this.speedRadioButton5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.speedRadioButton5.ForeColor = System.Drawing.Color.White;
            this.speedRadioButton5.Location = new System.Drawing.Point(609, 40);
            this.speedRadioButton5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.speedRadioButton5.Name = "speedRadioButton5";
            this.speedRadioButton5.Size = new System.Drawing.Size(124, 32);
            this.speedRadioButton5.TabIndex = 9;
            this.speedRadioButton5.Text = "매우 느림";
            this.speedRadioButton5.UseVisualStyleBackColor = true;
            // 
            // lbSpeed
            // 
            this.lbSpeed.AutoSize = true;
            this.lbSpeed.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSpeed.ForeColor = System.Drawing.Color.White;
            this.lbSpeed.Location = new System.Drawing.Point(6, 4);
            this.lbSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSpeed.Name = "lbSpeed";
            this.lbSpeed.Size = new System.Drawing.Size(106, 31);
            this.lbSpeed.TabIndex = 8;
            this.lbSpeed.Text = "처리속도";
            // 
            // lbSpeedInformation
            // 
            this.lbSpeedInformation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbSpeedInformation.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbSpeedInformation.ForeColor = System.Drawing.Color.White;
            this.lbSpeedInformation.Location = new System.Drawing.Point(34, 75);
            this.lbSpeedInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSpeedInformation.Name = "lbSpeedInformation";
            this.lbSpeedInformation.Size = new System.Drawing.Size(699, 39);
            this.lbSpeedInformation.TabIndex = 4;
            this.lbSpeedInformation.Text = "주의 : 빠름 이상으로 설정할 경우 게임이 느려질 수 있습니다.\r\n";
            this.lbSpeedInformation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // speedRadioButton4
            // 
            this.speedRadioButton4.AutoSize = true;
            this.speedRadioButton4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.speedRadioButton4.ForeColor = System.Drawing.Color.White;
            this.speedRadioButton4.Location = new System.Drawing.Point(477, 40);
            this.speedRadioButton4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.speedRadioButton4.Name = "speedRadioButton4";
            this.speedRadioButton4.Size = new System.Drawing.Size(77, 32);
            this.speedRadioButton4.TabIndex = 3;
            this.speedRadioButton4.Text = "느림";
            this.speedRadioButton4.UseVisualStyleBackColor = true;
            // 
            // speedRadioButton1
            // 
            this.speedRadioButton1.AutoSize = true;
            this.speedRadioButton1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.speedRadioButton1.ForeColor = System.Drawing.Color.White;
            this.speedRadioButton1.Location = new System.Drawing.Point(34, 40);
            this.speedRadioButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.speedRadioButton1.Name = "speedRadioButton1";
            this.speedRadioButton1.Size = new System.Drawing.Size(124, 32);
            this.speedRadioButton1.TabIndex = 0;
            this.speedRadioButton1.Text = "매우 빠름";
            this.speedRadioButton1.UseVisualStyleBackColor = true;
            // 
            // speedRadioButton3
            // 
            this.speedRadioButton3.AutoSize = true;
            this.speedRadioButton3.Checked = true;
            this.speedRadioButton3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.speedRadioButton3.ForeColor = System.Drawing.Color.White;
            this.speedRadioButton3.Location = new System.Drawing.Point(345, 40);
            this.speedRadioButton3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.speedRadioButton3.Name = "speedRadioButton3";
            this.speedRadioButton3.Size = new System.Drawing.Size(77, 32);
            this.speedRadioButton3.TabIndex = 2;
            this.speedRadioButton3.TabStop = true;
            this.speedRadioButton3.Text = "보통";
            this.speedRadioButton3.UseVisualStyleBackColor = true;
            // 
            // speedRadioButton2
            // 
            this.speedRadioButton2.AutoSize = true;
            this.speedRadioButton2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.speedRadioButton2.ForeColor = System.Drawing.Color.White;
            this.speedRadioButton2.Location = new System.Drawing.Point(213, 40);
            this.speedRadioButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.speedRadioButton2.Name = "speedRadioButton2";
            this.speedRadioButton2.Size = new System.Drawing.Size(77, 32);
            this.speedRadioButton2.TabIndex = 1;
            this.speedRadioButton2.Text = "빠름";
            this.speedRadioButton2.UseVisualStyleBackColor = true;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.defaultButton);
            this.panel13.Controls.Add(this.saveConfigButton);
            this.panel13.Controls.Add(this.openConfigButton);
            this.panel13.Controls.Add(this.lbSettingFile);
            this.panel13.Location = new System.Drawing.Point(4, 382);
            this.panel13.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(770, 196);
            this.panel13.TabIndex = 39;
            this.panel13.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // defaultButton
            // 
            this.defaultButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.defaultButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.defaultButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.defaultButton.ForeColor = System.Drawing.Color.White;
            this.defaultButton.Location = new System.Drawing.Point(12, 142);
            this.defaultButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(744, 38);
            this.defaultButton.TabIndex = 27;
            this.defaultButton.Text = "초기화";
            this.defaultButton.UseVisualStyleBackColor = false;
            this.defaultButton.Click += new System.EventHandler(this.defaultButton_Click);
            // 
            // saveConfigButton
            // 
            this.saveConfigButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.saveConfigButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.saveConfigButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveConfigButton.ForeColor = System.Drawing.Color.White;
            this.saveConfigButton.Location = new System.Drawing.Point(12, 98);
            this.saveConfigButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.saveConfigButton.Name = "saveConfigButton";
            this.saveConfigButton.Size = new System.Drawing.Size(744, 38);
            this.saveConfigButton.TabIndex = 26;
            this.saveConfigButton.Text = "설정 저장하기";
            this.saveConfigButton.UseVisualStyleBackColor = false;
            this.saveConfigButton.Click += new System.EventHandler(this.settingSaveToolStripMenuItem2_Click);
            // 
            // openConfigButton
            // 
            this.openConfigButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.openConfigButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.openConfigButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openConfigButton.ForeColor = System.Drawing.Color.White;
            this.openConfigButton.Location = new System.Drawing.Point(12, 52);
            this.openConfigButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.openConfigButton.Name = "openConfigButton";
            this.openConfigButton.Size = new System.Drawing.Size(744, 38);
            this.openConfigButton.TabIndex = 25;
            this.openConfigButton.Text = "설정 불러오기";
            this.openConfigButton.UseVisualStyleBackColor = false;
            this.openConfigButton.Click += new System.EventHandler(this.settingLoadToolStripMenuItem2_Click);
            // 
            // lbSettingFile
            // 
            this.lbSettingFile.AutoSize = true;
            this.lbSettingFile.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSettingFile.ForeColor = System.Drawing.Color.White;
            this.lbSettingFile.Location = new System.Drawing.Point(6, 4);
            this.lbSettingFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSettingFile.Name = "lbSettingFile";
            this.lbSettingFile.Size = new System.Drawing.Size(114, 31);
            this.lbSettingFile.TabIndex = 8;
            this.lbSettingFile.Text = "설정 파일";
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.topMostcheckBox);
            this.panel12.Controls.Add(this.checkUpdateCheckBox);
            this.panel12.Controls.Add(this.label15);
            this.panel12.Location = new System.Drawing.Point(4, 156);
            this.panel12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(770, 93);
            this.panel12.TabIndex = 38;
            this.panel12.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // topMostcheckBox
            // 
            this.topMostcheckBox.AutoSize = true;
            this.topMostcheckBox.Checked = true;
            this.topMostcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.topMostcheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.topMostcheckBox.ForeColor = System.Drawing.Color.White;
            this.topMostcheckBox.Location = new System.Drawing.Point(298, 54);
            this.topMostcheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.topMostcheckBox.Name = "topMostcheckBox";
            this.topMostcheckBox.Size = new System.Drawing.Size(185, 32);
            this.topMostcheckBox.TabIndex = 12;
            this.topMostcheckBox.Text = "번역창 최상위로";
            this.topMostcheckBox.UseVisualStyleBackColor = true;
            // 
            // checkUpdateCheckBox
            // 
            this.checkUpdateCheckBox.AutoSize = true;
            this.checkUpdateCheckBox.Checked = true;
            this.checkUpdateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUpdateCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.checkUpdateCheckBox.ForeColor = System.Drawing.Color.White;
            this.checkUpdateCheckBox.Location = new System.Drawing.Point(26, 54);
            this.checkUpdateCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkUpdateCheckBox.Name = "checkUpdateCheckBox";
            this.checkUpdateCheckBox.Size = new System.Drawing.Size(172, 32);
            this.checkUpdateCheckBox.TabIndex = 11;
            this.checkUpdateCheckBox.Text = "최신 버전 확인";
            this.checkUpdateCheckBox.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(6, 4);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(54, 31);
            this.label15.TabIndex = 8;
            this.label15.Text = "ETC";
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.btAttachCapture);
            this.panel14.Controls.Add(this.SetDefaultZoomSizeButton);
            this.panel14.Controls.Add(this.imgZoomsizeUpDown);
            this.panel14.Controls.Add(this.lbImgZoom);
            this.panel14.Controls.Add(this.activeWinodeCheckBox);
            this.panel14.Controls.Add(this.lbImgCapture);
            this.panel14.Location = new System.Drawing.Point(4, 4);
            this.panel14.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(770, 144);
            this.panel14.TabIndex = 37;
            this.panel14.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // btAttachCapture
            // 
            this.btAttachCapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btAttachCapture.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btAttachCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAttachCapture.ForeColor = System.Drawing.Color.White;
            this.btAttachCapture.Location = new System.Drawing.Point(14, 98);
            this.btAttachCapture.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btAttachCapture.Name = "btAttachCapture";
            this.btAttachCapture.Size = new System.Drawing.Size(742, 38);
            this.btAttachCapture.TabIndex = 27;
            this.btAttachCapture.Text = "화면을 가져올 윈도우 지정하기";
            this.btAttachCapture.UseVisualStyleBackColor = false;
            this.btAttachCapture.Click += new System.EventHandler(this.OnClick_btAttachCapture);
            // 
            // SetDefaultZoomSizeButton
            // 
            this.SetDefaultZoomSizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.SetDefaultZoomSizeButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.SetDefaultZoomSizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SetDefaultZoomSizeButton.ForeColor = System.Drawing.Color.White;
            this.SetDefaultZoomSizeButton.Location = new System.Drawing.Point(668, 54);
            this.SetDefaultZoomSizeButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SetDefaultZoomSizeButton.Name = "SetDefaultZoomSizeButton";
            this.SetDefaultZoomSizeButton.Size = new System.Drawing.Size(84, 34);
            this.SetDefaultZoomSizeButton.TabIndex = 28;
            this.SetDefaultZoomSizeButton.Text = "기본값";
            this.SetDefaultZoomSizeButton.UseVisualStyleBackColor = false;
            this.SetDefaultZoomSizeButton.Click += new System.EventHandler(this.SetDefaultZoomSizeButton_Click);
            // 
            // imgZoomsizeUpDown
            // 
            this.imgZoomsizeUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.imgZoomsizeUpDown.DecimalPlaces = 1;
            this.imgZoomsizeUpDown.ForeColor = System.Drawing.Color.White;
            this.imgZoomsizeUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.imgZoomsizeUpDown.Location = new System.Drawing.Point(588, 54);
            this.imgZoomsizeUpDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imgZoomsizeUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.imgZoomsizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.imgZoomsizeUpDown.Name = "imgZoomsizeUpDown";
            this.imgZoomsizeUpDown.Size = new System.Drawing.Size(70, 31);
            this.imgZoomsizeUpDown.TabIndex = 53;
            this.imgZoomsizeUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lbImgZoom
            // 
            this.lbImgZoom.AutoSize = true;
            this.lbImgZoom.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbImgZoom.ForeColor = System.Drawing.Color.White;
            this.lbImgZoom.Location = new System.Drawing.Point(404, 54);
            this.lbImgZoom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbImgZoom.Name = "lbImgZoom";
            this.lbImgZoom.Size = new System.Drawing.Size(178, 28);
            this.lbImgZoom.TabIndex = 51;
            this.lbImgZoom.Text = "추출 이미지 확대 :";
            // 
            // activeWinodeCheckBox
            // 
            this.activeWinodeCheckBox.AutoSize = true;
            this.activeWinodeCheckBox.Checked = true;
            this.activeWinodeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.activeWinodeCheckBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.activeWinodeCheckBox.ForeColor = System.Drawing.Color.White;
            this.activeWinodeCheckBox.Location = new System.Drawing.Point(26, 54);
            this.activeWinodeCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.activeWinodeCheckBox.Name = "activeWinodeCheckBox";
            this.activeWinodeCheckBox.Size = new System.Drawing.Size(339, 32);
            this.activeWinodeCheckBox.TabIndex = 10;
            this.activeWinodeCheckBox.Text = "활성화된 윈도우에서 이미지 캡쳐\r\n";
            this.activeWinodeCheckBox.UseVisualStyleBackColor = true;
            // 
            // lbImgCapture
            // 
            this.lbImgCapture.AutoSize = true;
            this.lbImgCapture.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbImgCapture.ForeColor = System.Drawing.Color.White;
            this.lbImgCapture.Location = new System.Drawing.Point(6, 4);
            this.lbImgCapture.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbImgCapture.Name = "lbImgCapture";
            this.lbImgCapture.Size = new System.Drawing.Size(137, 31);
            this.lbImgCapture.TabIndex = 8;
            this.lbImgCapture.Text = "이미지 캡쳐";
            // 
            // tpTranslation
            // 
            this.tpTranslation.Controls.Add(this.panel19);
            this.tpTranslation.Location = new System.Drawing.Point(80, 4);
            this.tpTranslation.Margin = new System.Windows.Forms.Padding(0);
            this.tpTranslation.Name = "tpTranslation";
            this.tpTranslation.Size = new System.Drawing.Size(818, 882);
            this.tpTranslation.TabIndex = 4;
            this.tpTranslation.Text = "번역설정";
            this.tpTranslation.UseVisualStyleBackColor = true;
            // 
            // panel19
            // 
            this.panel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.panel19.Controls.Add(this.panel4);
            this.panel19.Controls.Add(this.panel27);
            this.panel19.Controls.Add(this.panel22);
            this.panel19.Controls.Add(this.panel1);
            this.panel19.Controls.Add(this.panel15);
            this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel19.Location = new System.Drawing.Point(0, 0);
            this.panel19.Margin = new System.Windows.Forms.Padding(0);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(818, 882);
            this.panel19.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cbDeepLLanguageTo);
            this.panel4.Controls.Add(this.lbDeepLTo);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.cbDeepLLanguage);
            this.panel4.Controls.Add(this.lbDeepL);
            this.panel4.Controls.Add(this.lbDeepLFrom);
            this.panel4.Location = new System.Drawing.Point(0, 333);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(770, 114);
            this.panel4.TabIndex = 57;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // cbDeepLLanguageTo
            // 
            this.cbDeepLLanguageTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeepLLanguageTo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbDeepLLanguageTo.FormattingEnabled = true;
            this.cbDeepLLanguageTo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbDeepLLanguageTo.Items.AddRange(new object[] {
            "한국어",
            "영어",
            "일본어",
            "중국어 - 간체",
            "중국어 - 번체",
            "러시아어",
            "독일어",
            "브라질어",
            "포르투갈어",
            "스페인어",
            "프랑스어",
            "베트남어",
            "태국어"});
            this.cbDeepLLanguageTo.Location = new System.Drawing.Point(456, 52);
            this.cbDeepLLanguageTo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDeepLLanguageTo.Name = "cbDeepLLanguageTo";
            this.cbDeepLLanguageTo.Size = new System.Drawing.Size(148, 36);
            this.cbDeepLLanguageTo.TabIndex = 53;
            // 
            // lbDeepLTo
            // 
            this.lbDeepLTo.AutoSize = true;
            this.lbDeepLTo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbDeepLTo.ForeColor = System.Drawing.Color.White;
            this.lbDeepLTo.Location = new System.Drawing.Point(626, 57);
            this.lbDeepLTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDeepLTo.Name = "lbDeepLTo";
            this.lbDeepLTo.Size = new System.Drawing.Size(79, 28);
            this.lbDeepLTo.TabIndex = 52;
            this.lbDeepLTo.Text = "로 번역";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(374, 57);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 28);
            this.label6.TabIndex = 51;
            this.label6.Text = "->";
            // 
            // cbDeepLLanguage
            // 
            this.cbDeepLLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeepLLanguage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbDeepLLanguage.FormattingEnabled = true;
            this.cbDeepLLanguage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbDeepLLanguage.Items.AddRange(new object[] {
            "영어",
            "일본어",
            "중국어 간체",
            "중국어 번체",
            "한국어",
            "러시아어",
            "독일어",
            "브라질어",
            "포르투갈어",
            "스페인어",
            "프랑스어",
            "베트남어",
            "태국어"});
            this.cbDeepLLanguage.Location = new System.Drawing.Point(75, 52);
            this.cbDeepLLanguage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDeepLLanguage.Name = "cbDeepLLanguage";
            this.cbDeepLLanguage.Size = new System.Drawing.Size(148, 36);
            this.cbDeepLLanguage.TabIndex = 50;
            // 
            // lbDeepL
            // 
            this.lbDeepL.AutoSize = true;
            this.lbDeepL.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbDeepL.ForeColor = System.Drawing.Color.White;
            this.lbDeepL.Location = new System.Drawing.Point(6, 4);
            this.lbDeepL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDeepL.Name = "lbDeepL";
            this.lbDeepL.Size = new System.Drawing.Size(191, 31);
            this.lbDeepL.TabIndex = 8;
            this.lbDeepL.Text = "DeepL 번역 설정";
            // 
            // lbDeepLFrom
            // 
            this.lbDeepLFrom.AutoSize = true;
            this.lbDeepLFrom.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbDeepLFrom.ForeColor = System.Drawing.Color.White;
            this.lbDeepLFrom.Location = new System.Drawing.Point(255, 57);
            this.lbDeepLFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDeepLFrom.Name = "lbDeepLFrom";
            this.lbDeepLFrom.Size = new System.Drawing.Size(52, 28);
            this.lbDeepLFrom.TabIndex = 49;
            this.lbDeepLFrom.Text = "에서";
            // 
            // panel27
            // 
            this.panel27.Controls.Add(this.cbTTSWaitEnd);
            this.panel27.Controls.Add(this.cbUseTTS);
            this.panel27.Controls.Add(this.label66);
            this.panel27.Location = new System.Drawing.Point(0, 453);
            this.panel27.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel27.Name = "panel27";
            this.panel27.Size = new System.Drawing.Size(770, 126);
            this.panel27.TabIndex = 54;
            this.panel27.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // cbTTSWaitEnd
            // 
            this.cbTTSWaitEnd.AutoSize = true;
            this.cbTTSWaitEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.cbTTSWaitEnd.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbTTSWaitEnd.ForeColor = System.Drawing.Color.White;
            this.cbTTSWaitEnd.Location = new System.Drawing.Point(202, 39);
            this.cbTTSWaitEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbTTSWaitEnd.Name = "cbTTSWaitEnd";
            this.cbTTSWaitEnd.Size = new System.Drawing.Size(266, 32);
            this.cbTTSWaitEnd.TabIndex = 12;
            this.cbTTSWaitEnd.Text = "음성이 끝날 때 까지 대기";
            this.cbTTSWaitEnd.UseVisualStyleBackColor = false;
            // 
            // cbUseTTS
            // 
            this.cbUseTTS.AutoSize = true;
            this.cbUseTTS.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbUseTTS.ForeColor = System.Drawing.Color.White;
            this.cbUseTTS.Location = new System.Drawing.Point(30, 39);
            this.cbUseTTS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbUseTTS.Name = "cbUseTTS";
            this.cbUseTTS.Size = new System.Drawing.Size(120, 32);
            this.cbUseTTS.TabIndex = 9;
            this.cbUseTTS.Text = "TTS 사용";
            this.cbUseTTS.UseVisualStyleBackColor = true;
            this.cbUseTTS.CheckedChanged += new System.EventHandler(this.cbUseTTS_CheckedChanged);
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label66.ForeColor = System.Drawing.Color.White;
            this.label66.Location = new System.Drawing.Point(6, 4);
            this.label66.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(53, 31);
            this.label66.TabIndex = 8;
            this.label66.Text = "TTS";
            // 
            // panel22
            // 
            this.panel22.Controls.Add(this.googleResultCodeComboBox);
            this.panel22.Controls.Add(this.lbGoogleTo);
            this.panel22.Controls.Add(this.label56);
            this.panel22.Controls.Add(this.googleTransComboBox);
            this.panel22.Controls.Add(this.lbGoogle);
            this.panel22.Controls.Add(this.lbGoogleFrom);
            this.panel22.Location = new System.Drawing.Point(0, 210);
            this.panel22.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel22.Name = "panel22";
            this.panel22.Size = new System.Drawing.Size(770, 114);
            this.panel22.TabIndex = 56;
            this.panel22.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // googleResultCodeComboBox
            // 
            this.googleResultCodeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.googleResultCodeComboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.googleResultCodeComboBox.FormattingEnabled = true;
            this.googleResultCodeComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.googleResultCodeComboBox.Items.AddRange(new object[] {
            "한국어",
            "영어",
            "일본어",
            "중국어 - 간체",
            "중국어 - 번체",
            "러시아어",
            "독일어",
            "브라질어",
            "포르투갈어",
            "스페인어",
            "프랑스어",
            "베트남어",
            "태국어"});
            this.googleResultCodeComboBox.Location = new System.Drawing.Point(456, 52);
            this.googleResultCodeComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.googleResultCodeComboBox.Name = "googleResultCodeComboBox";
            this.googleResultCodeComboBox.Size = new System.Drawing.Size(148, 36);
            this.googleResultCodeComboBox.TabIndex = 53;
            // 
            // lbGoogleTo
            // 
            this.lbGoogleTo.AutoSize = true;
            this.lbGoogleTo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbGoogleTo.ForeColor = System.Drawing.Color.White;
            this.lbGoogleTo.Location = new System.Drawing.Point(626, 57);
            this.lbGoogleTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbGoogleTo.Name = "lbGoogleTo";
            this.lbGoogleTo.Size = new System.Drawing.Size(79, 28);
            this.lbGoogleTo.TabIndex = 52;
            this.lbGoogleTo.Text = "로 번역";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label56.ForeColor = System.Drawing.Color.White;
            this.label56.Location = new System.Drawing.Point(374, 57);
            this.label56.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(34, 28);
            this.label56.TabIndex = 51;
            this.label56.Text = "->";
            // 
            // googleTransComboBox
            // 
            this.googleTransComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.googleTransComboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.googleTransComboBox.FormattingEnabled = true;
            this.googleTransComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.googleTransComboBox.Items.AddRange(new object[] {
            "영어",
            "일본어",
            "중국어 간체",
            "중국어 번체",
            "한국어",
            "러시아어",
            "독일어",
            "브라질어",
            "포르투갈어",
            "스페인어",
            "프랑스어",
            "베트남어",
            "태국어"});
            this.googleTransComboBox.Location = new System.Drawing.Point(75, 52);
            this.googleTransComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.googleTransComboBox.Name = "googleTransComboBox";
            this.googleTransComboBox.Size = new System.Drawing.Size(148, 36);
            this.googleTransComboBox.TabIndex = 50;
            // 
            // lbGoogle
            // 
            this.lbGoogle.AutoSize = true;
            this.lbGoogle.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbGoogle.ForeColor = System.Drawing.Color.White;
            this.lbGoogle.Location = new System.Drawing.Point(6, 4);
            this.lbGoogle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbGoogle.Name = "lbGoogle";
            this.lbGoogle.Size = new System.Drawing.Size(514, 31);
            this.lbGoogle.TabIndex = 8;
            this.lbGoogle.Text = "구글 번역 설정 (기본 번역기 , 구글 시트 번역기)";
            // 
            // lbGoogleFrom
            // 
            this.lbGoogleFrom.AutoSize = true;
            this.lbGoogleFrom.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbGoogleFrom.ForeColor = System.Drawing.Color.White;
            this.lbGoogleFrom.Location = new System.Drawing.Point(255, 57);
            this.lbGoogleFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbGoogleFrom.Name = "lbGoogleFrom";
            this.lbGoogleFrom.Size = new System.Drawing.Size(52, 28);
            this.lbGoogleFrom.TabIndex = 49;
            this.lbGoogleFrom.Text = "에서";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.skinOverRadioButton);
            this.panel1.Controls.Add(this.skinLayerRadioButton);
            this.panel1.Controls.Add(this.lbTransformType);
            this.panel1.Controls.Add(this.skinDarkRadioButton);
            this.panel1.Location = new System.Drawing.Point(0, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 74);
            this.panel1.TabIndex = 55;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // skinOverRadioButton
            // 
            this.skinOverRadioButton.AutoSize = true;
            this.skinOverRadioButton.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.skinOverRadioButton.ForeColor = System.Drawing.Color.White;
            this.skinOverRadioButton.Location = new System.Drawing.Point(225, 34);
            this.skinOverRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.skinOverRadioButton.Name = "skinOverRadioButton";
            this.skinOverRadioButton.Size = new System.Drawing.Size(117, 32);
            this.skinOverRadioButton.TabIndex = 10;
            this.skinOverRadioButton.Text = "오버레이";
            this.skinOverRadioButton.UseVisualStyleBackColor = true;
            // 
            // skinLayerRadioButton
            // 
            this.skinLayerRadioButton.AutoSize = true;
            this.skinLayerRadioButton.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.skinLayerRadioButton.ForeColor = System.Drawing.Color.White;
            this.skinLayerRadioButton.Location = new System.Drawing.Point(116, 34);
            this.skinLayerRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.skinLayerRadioButton.Name = "skinLayerRadioButton";
            this.skinLayerRadioButton.Size = new System.Drawing.Size(97, 32);
            this.skinLayerRadioButton.TabIndex = 9;
            this.skinLayerRadioButton.Text = "레이어";
            this.skinLayerRadioButton.UseVisualStyleBackColor = true;
            // 
            // lbTransformType
            // 
            this.lbTransformType.AutoSize = true;
            this.lbTransformType.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTransformType.ForeColor = System.Drawing.Color.White;
            this.lbTransformType.Location = new System.Drawing.Point(6, 4);
            this.lbTransformType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTransformType.Name = "lbTransformType";
            this.lbTransformType.Size = new System.Drawing.Size(137, 31);
            this.lbTransformType.TabIndex = 8;
            this.lbTransformType.Text = "번역창 방식";
            // 
            // skinDarkRadioButton
            // 
            this.skinDarkRadioButton.AutoSize = true;
            this.skinDarkRadioButton.Checked = true;
            this.skinDarkRadioButton.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.skinDarkRadioButton.ForeColor = System.Drawing.Color.White;
            this.skinDarkRadioButton.Location = new System.Drawing.Point(16, 34);
            this.skinDarkRadioButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.skinDarkRadioButton.Name = "skinDarkRadioButton";
            this.skinDarkRadioButton.Size = new System.Drawing.Size(97, 32);
            this.skinDarkRadioButton.TabIndex = 6;
            this.skinDarkRadioButton.TabStop = true;
            this.skinDarkRadioButton.Text = "어두운";
            this.skinDarkRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.cbNaverResultCode);
            this.panel15.Controls.Add(this.lbPaPagoTo);
            this.panel15.Controls.Add(this.label43);
            this.panel15.Controls.Add(this.naverTransComboBox);
            this.panel15.Controls.Add(this.lbPaPago);
            this.panel15.Controls.Add(this.lbPaPagoFrom);
            this.panel15.Location = new System.Drawing.Point(0, 87);
            this.panel15.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(770, 114);
            this.panel15.TabIndex = 54;
            this.panel15.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // cbNaverResultCode
            // 
            this.cbNaverResultCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNaverResultCode.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbNaverResultCode.FormattingEnabled = true;
            this.cbNaverResultCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbNaverResultCode.Items.AddRange(new object[] {
            "한국어",
            "영어"});
            this.cbNaverResultCode.Location = new System.Drawing.Point(456, 45);
            this.cbNaverResultCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbNaverResultCode.Name = "cbNaverResultCode";
            this.cbNaverResultCode.Size = new System.Drawing.Size(148, 36);
            this.cbNaverResultCode.TabIndex = 55;
            // 
            // lbPaPagoTo
            // 
            this.lbPaPagoTo.AutoSize = true;
            this.lbPaPagoTo.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbPaPagoTo.ForeColor = System.Drawing.Color.White;
            this.lbPaPagoTo.Location = new System.Drawing.Point(626, 50);
            this.lbPaPagoTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPaPagoTo.Name = "lbPaPagoTo";
            this.lbPaPagoTo.Size = new System.Drawing.Size(79, 28);
            this.lbPaPagoTo.TabIndex = 54;
            this.lbPaPagoTo.Text = "로 번역";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.label43.ForeColor = System.Drawing.Color.White;
            this.label43.Location = new System.Drawing.Point(374, 57);
            this.label43.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(34, 28);
            this.label43.TabIndex = 51;
            this.label43.Text = "->";
            // 
            // naverTransComboBox
            // 
            this.naverTransComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.naverTransComboBox.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.naverTransComboBox.FormattingEnabled = true;
            this.naverTransComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.naverTransComboBox.Items.AddRange(new object[] {
            "영어",
            "일본어",
            "중국어 간체",
            "중국어 번체",
            "스페인어",
            "프랑스어",
            "베트남어",
            "태국어",
            "인도네시아어",
            "한국어"});
            this.naverTransComboBox.Location = new System.Drawing.Point(75, 52);
            this.naverTransComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.naverTransComboBox.Name = "naverTransComboBox";
            this.naverTransComboBox.Size = new System.Drawing.Size(148, 36);
            this.naverTransComboBox.TabIndex = 50;
            // 
            // lbPaPago
            // 
            this.lbPaPago.AutoSize = true;
            this.lbPaPago.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbPaPago.ForeColor = System.Drawing.Color.White;
            this.lbPaPago.Location = new System.Drawing.Point(6, 4);
            this.lbPaPago.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPaPago.Name = "lbPaPago";
            this.lbPaPago.Size = new System.Drawing.Size(276, 31);
            this.lbPaPago.TabIndex = 8;
            this.lbPaPago.Text = "네이버(파파고) 번역 설정";
            // 
            // lbPaPagoFrom
            // 
            this.lbPaPagoFrom.AutoSize = true;
            this.lbPaPagoFrom.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbPaPagoFrom.ForeColor = System.Drawing.Color.White;
            this.lbPaPagoFrom.Location = new System.Drawing.Point(255, 57);
            this.lbPaPagoFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPaPagoFrom.Name = "lbPaPagoFrom";
            this.lbPaPagoFrom.Size = new System.Drawing.Size(52, 28);
            this.lbPaPagoFrom.TabIndex = 49;
            this.lbPaPagoFrom.Text = "에서";
            // 
            // tpETC
            // 
            this.tpETC.Controls.Add(this.panel18);
            this.tpETC.Location = new System.Drawing.Point(80, 4);
            this.tpETC.Margin = new System.Windows.Forms.Padding(0);
            this.tpETC.Name = "tpETC";
            this.tpETC.Size = new System.Drawing.Size(818, 882);
            this.tpETC.TabIndex = 3;
            this.tpETC.Text = "그 외";
            this.tpETC.UseVisualStyleBackColor = true;
            // 
            // panel18
            // 
            this.panel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.panel18.Controls.Add(this.panel16);
            this.panel18.Controls.Add(this.panel20);
            this.panel18.Controls.Add(this.panel23);
            this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel18.Location = new System.Drawing.Point(0, 0);
            this.panel18.Margin = new System.Windows.Forms.Padding(0);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(818, 882);
            this.panel18.TabIndex = 2;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.btnOpenDiscord);
            this.panel16.Controls.Add(this.openBlogButton);
            this.panel16.Controls.Add(this.btnGitHub);
            this.panel16.Controls.Add(this.lbLink);
            this.panel16.Location = new System.Drawing.Point(6, 567);
            this.panel16.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(770, 192);
            this.panel16.TabIndex = 42;
            this.panel16.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // btnOpenDiscord
            // 
            this.btnOpenDiscord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnOpenDiscord.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnOpenDiscord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenDiscord.ForeColor = System.Drawing.Color.White;
            this.btnOpenDiscord.Location = new System.Drawing.Point(10, 142);
            this.btnOpenDiscord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenDiscord.Name = "btnOpenDiscord";
            this.btnOpenDiscord.Size = new System.Drawing.Size(744, 38);
            this.btnOpenDiscord.TabIndex = 26;
            this.btnOpenDiscord.Text = "디스코드 채널로 이동";
            this.btnOpenDiscord.UseVisualStyleBackColor = false;
            this.btnOpenDiscord.Click += new System.EventHandler(this.OnClickOpenDiscord);
            // 
            // openBlogButton
            // 
            this.openBlogButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.openBlogButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.openBlogButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openBlogButton.ForeColor = System.Drawing.Color.White;
            this.openBlogButton.Location = new System.Drawing.Point(12, 98);
            this.openBlogButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.openBlogButton.Name = "openBlogButton";
            this.openBlogButton.Size = new System.Drawing.Size(744, 38);
            this.openBlogButton.TabIndex = 25;
            this.openBlogButton.Text = "블로그 방문";
            this.openBlogButton.UseVisualStyleBackColor = false;
            this.openBlogButton.Click += new System.EventHandler(this.OnClickopenBlog);
            // 
            // btnGitHub
            // 
            this.btnGitHub.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnGitHub.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnGitHub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGitHub.ForeColor = System.Drawing.Color.White;
            this.btnGitHub.Location = new System.Drawing.Point(12, 52);
            this.btnGitHub.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGitHub.Name = "btnGitHub";
            this.btnGitHub.Size = new System.Drawing.Size(744, 38);
            this.btnGitHub.TabIndex = 25;
            this.btnGitHub.Text = "Github로 이동";
            this.btnGitHub.UseVisualStyleBackColor = false;
            this.btnGitHub.Click += new System.EventHandler(this.OnClick_GitHub);
            // 
            // lbLink
            // 
            this.lbLink.AutoSize = true;
            this.lbLink.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbLink.ForeColor = System.Drawing.Color.White;
            this.lbLink.Location = new System.Drawing.Point(6, 4);
            this.lbLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLink.Name = "lbLink";
            this.lbLink.Size = new System.Drawing.Size(60, 31);
            this.lbLink.TabIndex = 8;
            this.lbLink.Text = "링크";
            // 
            // panel20
            // 
            this.panel20.Controls.Add(this.about_Button);
            this.panel20.Controls.Add(this.error_Information_Button);
            this.panel20.Controls.Add(this.help_Button);
            this.panel20.Controls.Add(this.lbETC);
            this.panel20.Location = new System.Drawing.Point(4, 363);
            this.panel20.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(770, 195);
            this.panel20.TabIndex = 41;
            this.panel20.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // about_Button
            // 
            this.about_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.about_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.about_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.about_Button.ForeColor = System.Drawing.Color.White;
            this.about_Button.Location = new System.Drawing.Point(12, 142);
            this.about_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.about_Button.Name = "about_Button";
            this.about_Button.Size = new System.Drawing.Size(744, 38);
            this.about_Button.TabIndex = 26;
            this.about_Button.Text = "About";
            this.about_Button.UseVisualStyleBackColor = false;
            this.about_Button.Click += new System.EventHandler(this.about_Button_Click);
            // 
            // error_Information_Button
            // 
            this.error_Information_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.error_Information_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.error_Information_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.error_Information_Button.ForeColor = System.Drawing.Color.White;
            this.error_Information_Button.Location = new System.Drawing.Point(12, 98);
            this.error_Information_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.error_Information_Button.Name = "error_Information_Button";
            this.error_Information_Button.Size = new System.Drawing.Size(744, 38);
            this.error_Information_Button.TabIndex = 25;
            this.error_Information_Button.Text = "에러 메시지 목록";
            this.error_Information_Button.UseVisualStyleBackColor = false;
            this.error_Information_Button.Click += new System.EventHandler(this.error_Information_Button_Click);
            // 
            // help_Button
            // 
            this.help_Button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.help_Button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.help_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.help_Button.ForeColor = System.Drawing.Color.White;
            this.help_Button.Location = new System.Drawing.Point(12, 52);
            this.help_Button.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.help_Button.Name = "help_Button";
            this.help_Button.Size = new System.Drawing.Size(744, 38);
            this.help_Button.TabIndex = 25;
            this.help_Button.Text = "MORT 사용법";
            this.help_Button.UseVisualStyleBackColor = false;
            this.help_Button.Click += new System.EventHandler(this.help_Button_Click);
            // 
            // lbETC
            // 
            this.lbETC.AutoSize = true;
            this.lbETC.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbETC.ForeColor = System.Drawing.Color.White;
            this.lbETC.Location = new System.Drawing.Point(6, 4);
            this.lbETC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbETC.Name = "lbETC";
            this.lbETC.Size = new System.Drawing.Size(68, 31);
            this.lbETC.TabIndex = 8;
            this.lbETC.Text = "그 외";
            // 
            // panel23
            // 
            this.panel23.Controls.Add(this.panel2);
            this.panel23.Controls.Add(this.lbHotKeyHideTransWindow);
            this.panel23.Controls.Add(this.lbHotKeyOnceTranslate);
            this.panel23.Controls.Add(this.lbHotKeySnapShot);
            this.panel23.Controls.Add(this.lbHotKeyInformation);
            this.panel23.Controls.Add(this.lbHotKeyQuickOCR);
            this.panel23.Controls.Add(this.lbHotKeyDic);
            this.panel23.Controls.Add(this.lbHotKeyDoTrans);
            this.panel23.Controls.Add(this.lbHotkey);
            this.panel23.Location = new System.Drawing.Point(4, 4);
            this.panel23.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel23.Name = "panel23";
            this.panel23.Size = new System.Drawing.Size(770, 358);
            this.panel23.TabIndex = 37;
            this.panel23.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.transKeyInputLabel);
            this.panel2.Controls.Add(this.btnHideTransEmpty);
            this.panel2.Controls.Add(this.dicKeyInputLabel);
            this.panel2.Controls.Add(this.btnHideTransDefault);
            this.panel2.Controls.Add(this.quickKeyInputLabel);
            this.panel2.Controls.Add(this.lbHideTranslate);
            this.panel2.Controls.Add(this.transKeyInputResetButton);
            this.panel2.Controls.Add(this.dicKeyInputResetButton);
            this.panel2.Controls.Add(this.btnOneTransEmpty);
            this.panel2.Controls.Add(this.quickKeyInputResetButton);
            this.panel2.Controls.Add(this.btnOneTransDefault);
            this.panel2.Controls.Add(this.transKeyInputEmptyButton);
            this.panel2.Controls.Add(this.lbOneTrans);
            this.panel2.Controls.Add(this.dicKeyInputEmptyButton);
            this.panel2.Controls.Add(this.quickKeyInputEmptyButton);
            this.panel2.Controls.Add(this.snapShotKeyInputEmptyButton);
            this.panel2.Controls.Add(this.snapShotInputLabel);
            this.panel2.Controls.Add(this.snapShotKeyInputResetButton);
            this.panel2.Location = new System.Drawing.Point(194, 36);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(542, 276);
            this.panel2.TabIndex = 62;
            // 
            // transKeyInputLabel
            // 
            this.transKeyInputLabel.Location = new System.Drawing.Point(24, 21);
            this.transKeyInputLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.transKeyInputLabel.Name = "transKeyInputLabel";
            this.transKeyInputLabel.Size = new System.Drawing.Size(297, 32);
            this.transKeyInputLabel.TabIndex = 26;
            // 
            // btnHideTransEmpty
            // 
            this.btnHideTransEmpty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnHideTransEmpty.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnHideTransEmpty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHideTransEmpty.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnHideTransEmpty.ForeColor = System.Drawing.Color.White;
            this.btnHideTransEmpty.Location = new System.Drawing.Point(444, 230);
            this.btnHideTransEmpty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHideTransEmpty.Name = "btnHideTransEmpty";
            this.btnHideTransEmpty.Size = new System.Drawing.Size(84, 34);
            this.btnHideTransEmpty.TabIndex = 61;
            this.btnHideTransEmpty.Text = "비우기";
            this.btnHideTransEmpty.UseVisualStyleBackColor = false;
            this.btnHideTransEmpty.Click += new System.EventHandler(this.btnHideTransEmpty_Click);
            // 
            // dicKeyInputLabel
            // 
            this.dicKeyInputLabel.Location = new System.Drawing.Point(24, 62);
            this.dicKeyInputLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dicKeyInputLabel.Name = "dicKeyInputLabel";
            this.dicKeyInputLabel.Size = new System.Drawing.Size(297, 32);
            this.dicKeyInputLabel.TabIndex = 28;
            // 
            // btnHideTransDefault
            // 
            this.btnHideTransDefault.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnHideTransDefault.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnHideTransDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHideTransDefault.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnHideTransDefault.ForeColor = System.Drawing.Color.White;
            this.btnHideTransDefault.Location = new System.Drawing.Point(351, 230);
            this.btnHideTransDefault.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHideTransDefault.Name = "btnHideTransDefault";
            this.btnHideTransDefault.Size = new System.Drawing.Size(84, 34);
            this.btnHideTransDefault.TabIndex = 60;
            this.btnHideTransDefault.Text = "기본값";
            this.btnHideTransDefault.UseVisualStyleBackColor = false;
            this.btnHideTransDefault.Click += new System.EventHandler(this.btnHideTransDefault_Click);
            // 
            // quickKeyInputLabel
            // 
            this.quickKeyInputLabel.Location = new System.Drawing.Point(24, 102);
            this.quickKeyInputLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.quickKeyInputLabel.Name = "quickKeyInputLabel";
            this.quickKeyInputLabel.Size = new System.Drawing.Size(297, 32);
            this.quickKeyInputLabel.TabIndex = 30;
            // 
            // lbHideTranslate
            // 
            this.lbHideTranslate.Location = new System.Drawing.Point(24, 224);
            this.lbHideTranslate.Margin = new System.Windows.Forms.Padding(4, 9, 4, 9);
            this.lbHideTranslate.Name = "lbHideTranslate";
            this.lbHideTranslate.Size = new System.Drawing.Size(297, 39);
            this.lbHideTranslate.TabIndex = 59;
            // 
            // transKeyInputResetButton
            // 
            this.transKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.transKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.transKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.transKeyInputResetButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.transKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            this.transKeyInputResetButton.Location = new System.Drawing.Point(351, 16);
            this.transKeyInputResetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.transKeyInputResetButton.Name = "transKeyInputResetButton";
            this.transKeyInputResetButton.Size = new System.Drawing.Size(84, 34);
            this.transKeyInputResetButton.TabIndex = 44;
            this.transKeyInputResetButton.Text = "기본값";
            this.transKeyInputResetButton.UseVisualStyleBackColor = false;
            this.transKeyInputResetButton.Click += new System.EventHandler(this.transKeyInputResetButton_Click);
            // 
            // dicKeyInputResetButton
            // 
            this.dicKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.dicKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.dicKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dicKeyInputResetButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dicKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            this.dicKeyInputResetButton.Location = new System.Drawing.Point(351, 57);
            this.dicKeyInputResetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dicKeyInputResetButton.Name = "dicKeyInputResetButton";
            this.dicKeyInputResetButton.Size = new System.Drawing.Size(84, 34);
            this.dicKeyInputResetButton.TabIndex = 45;
            this.dicKeyInputResetButton.Text = "기본값";
            this.dicKeyInputResetButton.UseVisualStyleBackColor = false;
            this.dicKeyInputResetButton.Click += new System.EventHandler(this.dicKeyInputResetButton_Click);
            // 
            // btnOneTransEmpty
            // 
            this.btnOneTransEmpty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnOneTransEmpty.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnOneTransEmpty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOneTransEmpty.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOneTransEmpty.ForeColor = System.Drawing.Color.White;
            this.btnOneTransEmpty.Location = new System.Drawing.Point(444, 186);
            this.btnOneTransEmpty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOneTransEmpty.Name = "btnOneTransEmpty";
            this.btnOneTransEmpty.Size = new System.Drawing.Size(84, 34);
            this.btnOneTransEmpty.TabIndex = 57;
            this.btnOneTransEmpty.Text = "비우기";
            this.btnOneTransEmpty.UseVisualStyleBackColor = false;
            this.btnOneTransEmpty.Click += new System.EventHandler(this.btnOneTransEmpty_Click);
            // 
            // quickKeyInputResetButton
            // 
            this.quickKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.quickKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.quickKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quickKeyInputResetButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.quickKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            this.quickKeyInputResetButton.Location = new System.Drawing.Point(351, 98);
            this.quickKeyInputResetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.quickKeyInputResetButton.Name = "quickKeyInputResetButton";
            this.quickKeyInputResetButton.Size = new System.Drawing.Size(84, 34);
            this.quickKeyInputResetButton.TabIndex = 46;
            this.quickKeyInputResetButton.Text = "기본값";
            this.quickKeyInputResetButton.UseVisualStyleBackColor = false;
            this.quickKeyInputResetButton.Click += new System.EventHandler(this.quickKeyInputResetButton_Click);
            // 
            // btnOneTransDefault
            // 
            this.btnOneTransDefault.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnOneTransDefault.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnOneTransDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOneTransDefault.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOneTransDefault.ForeColor = System.Drawing.Color.White;
            this.btnOneTransDefault.Location = new System.Drawing.Point(351, 186);
            this.btnOneTransDefault.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOneTransDefault.Name = "btnOneTransDefault";
            this.btnOneTransDefault.Size = new System.Drawing.Size(84, 34);
            this.btnOneTransDefault.TabIndex = 56;
            this.btnOneTransDefault.Text = "기본값";
            this.btnOneTransDefault.UseVisualStyleBackColor = false;
            this.btnOneTransDefault.Click += new System.EventHandler(this.btnOneTransDefault_Click);
            // 
            // transKeyInputEmptyButton
            // 
            this.transKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.transKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.transKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.transKeyInputEmptyButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.transKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            this.transKeyInputEmptyButton.Location = new System.Drawing.Point(444, 16);
            this.transKeyInputEmptyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.transKeyInputEmptyButton.Name = "transKeyInputEmptyButton";
            this.transKeyInputEmptyButton.Size = new System.Drawing.Size(84, 34);
            this.transKeyInputEmptyButton.TabIndex = 47;
            this.transKeyInputEmptyButton.Text = "비우기";
            this.transKeyInputEmptyButton.UseVisualStyleBackColor = false;
            this.transKeyInputEmptyButton.Click += new System.EventHandler(this.transKeyInputEmptyButton_Click);
            // 
            // lbOneTrans
            // 
            this.lbOneTrans.Location = new System.Drawing.Point(24, 183);
            this.lbOneTrans.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.lbOneTrans.Name = "lbOneTrans";
            this.lbOneTrans.Size = new System.Drawing.Size(297, 32);
            this.lbOneTrans.TabIndex = 55;
            // 
            // dicKeyInputEmptyButton
            // 
            this.dicKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.dicKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.dicKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dicKeyInputEmptyButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dicKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            this.dicKeyInputEmptyButton.Location = new System.Drawing.Point(444, 57);
            this.dicKeyInputEmptyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dicKeyInputEmptyButton.Name = "dicKeyInputEmptyButton";
            this.dicKeyInputEmptyButton.Size = new System.Drawing.Size(84, 34);
            this.dicKeyInputEmptyButton.TabIndex = 48;
            this.dicKeyInputEmptyButton.Text = "비우기";
            this.dicKeyInputEmptyButton.UseVisualStyleBackColor = false;
            this.dicKeyInputEmptyButton.Click += new System.EventHandler(this.dicKeyInputEmptyButton_Click);
            // 
            // quickKeyInputEmptyButton
            // 
            this.quickKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.quickKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.quickKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.quickKeyInputEmptyButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.quickKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            this.quickKeyInputEmptyButton.Location = new System.Drawing.Point(444, 98);
            this.quickKeyInputEmptyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.quickKeyInputEmptyButton.Name = "quickKeyInputEmptyButton";
            this.quickKeyInputEmptyButton.Size = new System.Drawing.Size(84, 34);
            this.quickKeyInputEmptyButton.TabIndex = 49;
            this.quickKeyInputEmptyButton.Text = "비우기";
            this.quickKeyInputEmptyButton.UseVisualStyleBackColor = false;
            this.quickKeyInputEmptyButton.Click += new System.EventHandler(this.quickKeyInputEmptyButton_Click);
            // 
            // snapShotKeyInputEmptyButton
            // 
            this.snapShotKeyInputEmptyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.snapShotKeyInputEmptyButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.snapShotKeyInputEmptyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.snapShotKeyInputEmptyButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.snapShotKeyInputEmptyButton.ForeColor = System.Drawing.Color.White;
            this.snapShotKeyInputEmptyButton.Location = new System.Drawing.Point(444, 142);
            this.snapShotKeyInputEmptyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.snapShotKeyInputEmptyButton.Name = "snapShotKeyInputEmptyButton";
            this.snapShotKeyInputEmptyButton.Size = new System.Drawing.Size(84, 34);
            this.snapShotKeyInputEmptyButton.TabIndex = 53;
            this.snapShotKeyInputEmptyButton.Text = "비우기";
            this.snapShotKeyInputEmptyButton.UseVisualStyleBackColor = false;
            this.snapShotKeyInputEmptyButton.Click += new System.EventHandler(this.snapShotKeyInputEmptyButton_Click);
            // 
            // snapShotInputLabel
            // 
            this.snapShotInputLabel.Location = new System.Drawing.Point(24, 142);
            this.snapShotInputLabel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.snapShotInputLabel.Name = "snapShotInputLabel";
            this.snapShotInputLabel.Size = new System.Drawing.Size(297, 32);
            this.snapShotInputLabel.TabIndex = 51;
            // 
            // snapShotKeyInputResetButton
            // 
            this.snapShotKeyInputResetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.snapShotKeyInputResetButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.snapShotKeyInputResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.snapShotKeyInputResetButton.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.snapShotKeyInputResetButton.ForeColor = System.Drawing.Color.White;
            this.snapShotKeyInputResetButton.Location = new System.Drawing.Point(351, 142);
            this.snapShotKeyInputResetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.snapShotKeyInputResetButton.Name = "snapShotKeyInputResetButton";
            this.snapShotKeyInputResetButton.Size = new System.Drawing.Size(84, 34);
            this.snapShotKeyInputResetButton.TabIndex = 52;
            this.snapShotKeyInputResetButton.Text = "기본값";
            this.snapShotKeyInputResetButton.UseVisualStyleBackColor = false;
            this.snapShotKeyInputResetButton.Click += new System.EventHandler(this.snapShotKeyInputResetButton_Click);
            // 
            // lbHotKeyHideTransWindow
            // 
            this.lbHotKeyHideTransWindow.AutoSize = true;
            this.lbHotKeyHideTransWindow.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbHotKeyHideTransWindow.ForeColor = System.Drawing.Color.White;
            this.lbHotKeyHideTransWindow.Location = new System.Drawing.Point(21, 264);
            this.lbHotKeyHideTransWindow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHotKeyHideTransWindow.Name = "lbHotKeyHideTransWindow";
            this.lbHotKeyHideTransWindow.Size = new System.Drawing.Size(157, 28);
            this.lbHotKeyHideTransWindow.TabIndex = 58;
            this.lbHotKeyHideTransWindow.Text = "번역창 숨기기 : ";
            // 
            // lbHotKeyOnceTranslate
            // 
            this.lbHotKeyOnceTranslate.AutoSize = true;
            this.lbHotKeyOnceTranslate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbHotKeyOnceTranslate.ForeColor = System.Drawing.Color.White;
            this.lbHotKeyOnceTranslate.Location = new System.Drawing.Point(21, 218);
            this.lbHotKeyOnceTranslate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHotKeyOnceTranslate.Name = "lbHotKeyOnceTranslate";
            this.lbHotKeyOnceTranslate.Size = new System.Drawing.Size(137, 28);
            this.lbHotKeyOnceTranslate.TabIndex = 54;
            this.lbHotKeyOnceTranslate.Text = "한 번만 번역 :";
            // 
            // lbHotKeySnapShot
            // 
            this.lbHotKeySnapShot.AutoSize = true;
            this.lbHotKeySnapShot.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbHotKeySnapShot.ForeColor = System.Drawing.Color.White;
            this.lbHotKeySnapShot.Location = new System.Drawing.Point(21, 177);
            this.lbHotKeySnapShot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHotKeySnapShot.Name = "lbHotKeySnapShot";
            this.lbHotKeySnapShot.Size = new System.Drawing.Size(83, 28);
            this.lbHotKeySnapShot.TabIndex = 50;
            this.lbHotKeySnapShot.Text = "스냅샷 :";
            // 
            // lbHotKeyInformation
            // 
            this.lbHotKeyInformation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbHotKeyInformation.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbHotKeyInformation.ForeColor = System.Drawing.Color.White;
            this.lbHotKeyInformation.Location = new System.Drawing.Point(12, 315);
            this.lbHotKeyInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHotKeyInformation.Name = "lbHotKeyInformation";
            this.lbHotKeyInformation.Size = new System.Drawing.Size(744, 39);
            this.lbHotKeyInformation.TabIndex = 43;
            this.lbHotKeyInformation.Text = "ESC, 백스페이바로 비울 수 있습니다.";
            this.lbHotKeyInformation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbHotKeyQuickOCR
            // 
            this.lbHotKeyQuickOCR.AutoSize = true;
            this.lbHotKeyQuickOCR.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbHotKeyQuickOCR.ForeColor = System.Drawing.Color.White;
            this.lbHotKeyQuickOCR.Location = new System.Drawing.Point(21, 136);
            this.lbHotKeyQuickOCR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHotKeyQuickOCR.Name = "lbHotKeyQuickOCR";
            this.lbHotKeyQuickOCR.Size = new System.Drawing.Size(157, 28);
            this.lbHotKeyQuickOCR.TabIndex = 29;
            this.lbHotKeyQuickOCR.Text = "빠른 OCR 영역 :";
            // 
            // lbHotKeyDic
            // 
            this.lbHotKeyDic.AutoSize = true;
            this.lbHotKeyDic.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbHotKeyDic.ForeColor = System.Drawing.Color.White;
            this.lbHotKeyDic.Location = new System.Drawing.Point(21, 96);
            this.lbHotKeyDic.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHotKeyDic.Name = "lbHotKeyDic";
            this.lbHotKeyDic.Size = new System.Drawing.Size(164, 28);
            this.lbHotKeyDic.TabIndex = 27;
            this.lbHotKeyDic.Text = "교정 사전 열기 : ";
            // 
            // lbHotKeyDoTrans
            // 
            this.lbHotKeyDoTrans.AutoSize = true;
            this.lbHotKeyDoTrans.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbHotKeyDoTrans.ForeColor = System.Drawing.Color.White;
            this.lbHotKeyDoTrans.Location = new System.Drawing.Point(21, 56);
            this.lbHotKeyDoTrans.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHotKeyDoTrans.Name = "lbHotKeyDoTrans";
            this.lbHotKeyDoTrans.Size = new System.Drawing.Size(165, 28);
            this.lbHotKeyDoTrans.TabIndex = 25;
            this.lbHotKeyDoTrans.Text = "번역 시작/중지 : ";
            // 
            // lbHotkey
            // 
            this.lbHotkey.AutoSize = true;
            this.lbHotkey.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbHotkey.ForeColor = System.Drawing.Color.White;
            this.lbHotkey.Location = new System.Drawing.Point(6, 4);
            this.lbHotkey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHotkey.Name = "lbHotkey";
            this.lbHotkey.Size = new System.Drawing.Size(83, 31);
            this.lbHotkey.TabIndex = 8;
            this.lbHotkey.Text = "단축키";
            // 
            // tpQuickSetting
            // 
            this.tpQuickSetting.Controls.Add(this.panel28);
            this.tpQuickSetting.Location = new System.Drawing.Point(80, 4);
            this.tpQuickSetting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tpQuickSetting.Name = "tpQuickSetting";
            this.tpQuickSetting.Size = new System.Drawing.Size(818, 882);
            this.tpQuickSetting.TabIndex = 6;
            this.tpQuickSetting.Text = "빠른설정";
            this.tpQuickSetting.UseVisualStyleBackColor = true;
            // 
            // panel28
            // 
            this.panel28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.panel28.Controls.Add(this.panel31);
            this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel28.Location = new System.Drawing.Point(0, 0);
            this.panel28.Margin = new System.Windows.Forms.Padding(0);
            this.panel28.Name = "panel28";
            this.panel28.Size = new System.Drawing.Size(818, 882);
            this.panel28.TabIndex = 3;
            // 
            // panel31
            // 
            this.panel31.Controls.Add(this.cbSetBasicDefaultPage);
            this.panel31.Controls.Add(this.btQuickJap);
            this.panel31.Controls.Add(this.lbQuickSettingInformation);
            this.panel31.Controls.Add(this.btQucickEnglish);
            this.panel31.Controls.Add(this.lbQuickSetting);
            this.panel31.Location = new System.Drawing.Point(4, 4);
            this.panel31.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel31.Name = "panel31";
            this.panel31.Size = new System.Drawing.Size(770, 832);
            this.panel31.TabIndex = 55;
            this.panel31.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // cbSetBasicDefaultPage
            // 
            this.cbSetBasicDefaultPage.AutoSize = true;
            this.cbSetBasicDefaultPage.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbSetBasicDefaultPage.ForeColor = System.Drawing.Color.White;
            this.cbSetBasicDefaultPage.Location = new System.Drawing.Point(30, 452);
            this.cbSetBasicDefaultPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbSetBasicDefaultPage.Name = "cbSetBasicDefaultPage";
            this.cbSetBasicDefaultPage.Size = new System.Drawing.Size(346, 32);
            this.cbSetBasicDefaultPage.TabIndex = 18;
            this.cbSetBasicDefaultPage.Text = "기본설정 탭을 시작 화면으로 설정";
            this.cbSetBasicDefaultPage.UseVisualStyleBackColor = true;
            // 
            // btQuickJap
            // 
            this.btQuickJap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btQuickJap.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btQuickJap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btQuickJap.ForeColor = System.Drawing.Color.White;
            this.btQuickJap.Location = new System.Drawing.Point(30, 248);
            this.btQuickJap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btQuickJap.Name = "btQuickJap";
            this.btQuickJap.Size = new System.Drawing.Size(710, 92);
            this.btQuickJap.TabIndex = 12;
            this.btQuickJap.Text = "일본어 게임";
            this.btQuickJap.UseVisualStyleBackColor = false;
            this.btQuickJap.Click += new System.EventHandler(this.OnClickQuickJap);
            // 
            // lbQuickSettingInformation
            // 
            this.lbQuickSettingInformation.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbQuickSettingInformation.ForeColor = System.Drawing.Color.White;
            this.lbQuickSettingInformation.Location = new System.Drawing.Point(30, 388);
            this.lbQuickSettingInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbQuickSettingInformation.Name = "lbQuickSettingInformation";
            this.lbQuickSettingInformation.Size = new System.Drawing.Size(705, 34);
            this.lbQuickSettingInformation.TabIndex = 11;
            this.lbQuickSettingInformation.Text = "처음 사용자를 위한 설정값을 불러옵니다";
            this.lbQuickSettingInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btQucickEnglish
            // 
            this.btQucickEnglish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btQucickEnglish.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btQucickEnglish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btQucickEnglish.ForeColor = System.Drawing.Color.White;
            this.btQucickEnglish.Location = new System.Drawing.Point(30, 112);
            this.btQucickEnglish.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btQucickEnglish.Name = "btQucickEnglish";
            this.btQucickEnglish.Size = new System.Drawing.Size(710, 92);
            this.btQucickEnglish.TabIndex = 10;
            this.btQucickEnglish.Text = "영문 게임";
            this.btQucickEnglish.UseVisualStyleBackColor = false;
            this.btQucickEnglish.Click += new System.EventHandler(this.OnClickQucickEnglish);
            // 
            // lbQuickSetting
            // 
            this.lbQuickSetting.AutoSize = true;
            this.lbQuickSetting.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbQuickSetting.ForeColor = System.Drawing.Color.White;
            this.lbQuickSetting.Location = new System.Drawing.Point(6, 4);
            this.lbQuickSetting.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbQuickSetting.Name = "lbQuickSetting";
            this.lbQuickSetting.Size = new System.Drawing.Size(293, 31);
            this.lbQuickSetting.TabIndex = 8;
            this.lbQuickSetting.Text = "어느 게임을 번역하시나요?";
            // 
            // tpDebuging
            // 
            this.tpDebuging.Controls.Add(this.panel24);
            this.tpDebuging.Location = new System.Drawing.Point(80, 4);
            this.tpDebuging.Margin = new System.Windows.Forms.Padding(0);
            this.tpDebuging.Name = "tpDebuging";
            this.tpDebuging.Size = new System.Drawing.Size(818, 882);
            this.tpDebuging.TabIndex = 5;
            this.tpDebuging.Text = "디버깅";
            this.tpDebuging.UseVisualStyleBackColor = true;
            // 
            // panel24
            // 
            this.panel24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.panel24.Controls.Add(this.panel26);
            this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel24.Location = new System.Drawing.Point(0, 0);
            this.panel24.Margin = new System.Windows.Forms.Padding(0);
            this.panel24.Name = "panel24";
            this.panel24.Size = new System.Drawing.Size(818, 882);
            this.panel24.TabIndex = 3;
            // 
            // panel26
            // 
            this.panel26.Controls.Add(this.plDebugOn);
            this.panel26.Controls.Add(this.plDebugOff);
            this.panel26.Controls.Add(this.label70);
            this.panel26.Location = new System.Drawing.Point(4, 4);
            this.panel26.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel26.Name = "panel26";
            this.panel26.Size = new System.Drawing.Size(770, 832);
            this.panel26.TabIndex = 37;
            this.panel26.Paint += new System.Windows.Forms.PaintEventHandler(this.panealBorder_Paint);
            // 
            // plDebugOn
            // 
            this.plDebugOn.Controls.Add(this.cbDisplayLowQuality);
            this.plDebugOn.Controls.Add(this.cbShowOverlayWordArea);
            this.plDebugOn.Controls.Add(this.cbSetLineTrans);
            this.plDebugOn.Controls.Add(this.btClearFormerResult);
            this.plDebugOn.Controls.Add(this.cbShowFormerLog);
            this.plDebugOn.Controls.Add(this.cbUnlockSpeed);
            this.plDebugOn.Controls.Add(this.cbSaveCaptureResult);
            this.plDebugOn.Controls.Add(this.cbSaveCapture);
            this.plDebugOn.Controls.Add(this.cbShowReplace);
            this.plDebugOn.Location = new System.Drawing.Point(4, 39);
            this.plDebugOn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.plDebugOn.Name = "plDebugOn";
            this.plDebugOn.Size = new System.Drawing.Size(760, 768);
            this.plDebugOn.TabIndex = 56;
            // 
            // cbDisplayLowQuality
            // 
            this.cbDisplayLowQuality.AutoSize = true;
            this.cbDisplayLowQuality.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbDisplayLowQuality.ForeColor = System.Drawing.Color.White;
            this.cbDisplayLowQuality.Location = new System.Drawing.Point(21, 360);
            this.cbDisplayLowQuality.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDisplayLowQuality.Name = "cbDisplayLowQuality";
            this.cbDisplayLowQuality.Size = new System.Drawing.Size(313, 32);
            this.cbDisplayLowQuality.TabIndex = 29;
            this.cbDisplayLowQuality.Text = "번역 결과에 [저품질] 표시하기";
            this.cbDisplayLowQuality.UseVisualStyleBackColor = true;
            this.cbDisplayLowQuality.CheckedChanged += new System.EventHandler(this.cbDisplayLowQuality_CheckedChanged);
            // 
            // cbShowOverlayWordArea
            // 
            this.cbShowOverlayWordArea.AutoSize = true;
            this.cbShowOverlayWordArea.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbShowOverlayWordArea.ForeColor = System.Drawing.Color.White;
            this.cbShowOverlayWordArea.Location = new System.Drawing.Point(21, 320);
            this.cbShowOverlayWordArea.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbShowOverlayWordArea.Name = "cbShowOverlayWordArea";
            this.cbShowOverlayWordArea.Size = new System.Drawing.Size(361, 32);
            this.cbShowOverlayWordArea.TabIndex = 28;
            this.cbShowOverlayWordArea.Text = "오버레이 번역창 - 문자 영역 보이기";
            this.cbShowOverlayWordArea.UseVisualStyleBackColor = true;
            this.cbShowOverlayWordArea.CheckedChanged += new System.EventHandler(this.cbShowOverlayWordArea_CheckedChanged);
            // 
            // cbSetLineTrans
            // 
            this.cbSetLineTrans.AutoSize = true;
            this.cbSetLineTrans.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbSetLineTrans.ForeColor = System.Drawing.Color.White;
            this.cbSetLineTrans.Location = new System.Drawing.Point(21, 279);
            this.cbSetLineTrans.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbSetLineTrans.Name = "cbSetLineTrans";
            this.cbSetLineTrans.Size = new System.Drawing.Size(521, 32);
            this.cbSetLineTrans.TabIndex = 27;
            this.cbSetLineTrans.Text = "줄 단위로 번역하기 (문장단위로 번역하기 사용 안 함)";
            this.cbSetLineTrans.UseVisualStyleBackColor = true;
            this.cbSetLineTrans.CheckedChanged += new System.EventHandler(this.cbSetLineTrans_CheckedChanged);
            // 
            // btClearFormerResult
            // 
            this.btClearFormerResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btClearFormerResult.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btClearFormerResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClearFormerResult.ForeColor = System.Drawing.Color.White;
            this.btClearFormerResult.Location = new System.Drawing.Point(21, 232);
            this.btClearFormerResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btClearFormerResult.Name = "btClearFormerResult";
            this.btClearFormerResult.Size = new System.Drawing.Size(730, 38);
            this.btClearFormerResult.TabIndex = 26;
            this.btClearFormerResult.Text = "번역 기억하기 모두 삭제";
            this.btClearFormerResult.UseVisualStyleBackColor = false;
            this.btClearFormerResult.Click += new System.EventHandler(this.btClearFormerResult_Click);
            // 
            // cbShowFormerLog
            // 
            this.cbShowFormerLog.AutoSize = true;
            this.cbShowFormerLog.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbShowFormerLog.ForeColor = System.Drawing.Color.White;
            this.cbShowFormerLog.Location = new System.Drawing.Point(21, 192);
            this.cbShowFormerLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbShowFormerLog.Name = "cbShowFormerLog";
            this.cbShowFormerLog.Size = new System.Drawing.Size(259, 32);
            this.cbShowFormerLog.TabIndex = 15;
            this.cbShowFormerLog.Text = "번역 기억하기 결과 출력";
            this.cbShowFormerLog.UseVisualStyleBackColor = true;
            this.cbShowFormerLog.CheckedChanged += new System.EventHandler(this.cbShowFormerLog_CheckedChanged);
            // 
            // cbUnlockSpeed
            // 
            this.cbUnlockSpeed.AutoSize = true;
            this.cbUnlockSpeed.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbUnlockSpeed.ForeColor = System.Drawing.Color.White;
            this.cbUnlockSpeed.Location = new System.Drawing.Point(21, 152);
            this.cbUnlockSpeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbUnlockSpeed.Name = "cbUnlockSpeed";
            this.cbUnlockSpeed.Size = new System.Drawing.Size(219, 32);
            this.cbUnlockSpeed.TabIndex = 14;
            this.cbUnlockSpeed.Text = "번역 속도 제한 해제";
            this.cbUnlockSpeed.UseVisualStyleBackColor = true;
            this.cbUnlockSpeed.CheckedChanged += new System.EventHandler(this.cbUnlockSpeed_CheckedChanged);
            // 
            // cbSaveCaptureResult
            // 
            this.cbSaveCaptureResult.AutoSize = true;
            this.cbSaveCaptureResult.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbSaveCaptureResult.ForeColor = System.Drawing.Color.White;
            this.cbSaveCaptureResult.Location = new System.Drawing.Point(21, 111);
            this.cbSaveCaptureResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbSaveCaptureResult.Name = "cbSaveCaptureResult";
            this.cbSaveCaptureResult.Size = new System.Drawing.Size(489, 32);
            this.cbSaveCaptureResult.TabIndex = 13;
            this.cbSaveCaptureResult.Text = "이미지 캡쳐 보정 결과 저장 - captue_Result.bmp";
            this.cbSaveCaptureResult.UseVisualStyleBackColor = true;
            // 
            // cbSaveCapture
            // 
            this.cbSaveCapture.AutoSize = true;
            this.cbSaveCapture.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbSaveCapture.ForeColor = System.Drawing.Color.White;
            this.cbSaveCapture.Location = new System.Drawing.Point(21, 70);
            this.cbSaveCapture.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbSaveCapture.Name = "cbSaveCapture";
            this.cbSaveCapture.Size = new System.Drawing.Size(459, 32);
            this.cbSaveCapture.TabIndex = 12;
            this.cbSaveCapture.Text = "이미지 캡쳐 원본 저장 - captue_Original.bmp";
            this.cbSaveCapture.UseVisualStyleBackColor = true;
            // 
            // cbShowReplace
            // 
            this.cbShowReplace.AutoSize = true;
            this.cbShowReplace.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.cbShowReplace.ForeColor = System.Drawing.Color.White;
            this.cbShowReplace.Location = new System.Drawing.Point(21, 30);
            this.cbShowReplace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbShowReplace.Name = "cbShowReplace";
            this.cbShowReplace.Size = new System.Drawing.Size(212, 32);
            this.cbShowReplace.TabIndex = 11;
            this.cbShowReplace.Text = "교정사전 결과 표시";
            this.cbShowReplace.UseVisualStyleBackColor = true;
            // 
            // plDebugOff
            // 
            this.plDebugOff.Controls.Add(this.label63);
            this.plDebugOff.Controls.Add(this.btnDebugOn);
            this.plDebugOff.Location = new System.Drawing.Point(9, 282);
            this.plDebugOff.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.plDebugOff.Name = "plDebugOff";
            this.plDebugOff.Size = new System.Drawing.Size(752, 321);
            this.plDebugOff.TabIndex = 57;
            this.plDebugOff.Visible = false;
            // 
            // label63
            // 
            this.label63.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label63.ForeColor = System.Drawing.Color.White;
            this.label63.Location = new System.Drawing.Point(78, 30);
            this.label63.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(609, 112);
            this.label63.TabIndex = 43;
            this.label63.Text = "MORT 디버깅 기능을 활성화 합니다.\r\n개발, 진단용 기능이기 때문에 평상시에는 사용할 필요가 없습니다.\r\n\r\n※ 디버깅을 활성화 했을 시 성능" +
    "에 영향을 줄 수 있습니다.\r\n";
            this.label63.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnDebugOn
            // 
            this.btnDebugOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnDebugOn.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDebugOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDebugOn.ForeColor = System.Drawing.Color.White;
            this.btnDebugOn.Location = new System.Drawing.Point(318, 237);
            this.btnDebugOn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDebugOn.Name = "btnDebugOn";
            this.btnDebugOn.Size = new System.Drawing.Size(84, 34);
            this.btnDebugOn.TabIndex = 46;
            this.btnDebugOn.Text = "활성화";
            this.btnDebugOn.UseVisualStyleBackColor = false;
            this.btnDebugOn.Click += new System.EventHandler(this.OnClick_DebugOn);
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label70.ForeColor = System.Drawing.Color.White;
            this.label70.Location = new System.Drawing.Point(6, 4);
            this.label70.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(83, 31);
            this.label70.TabIndex = 8;
            this.label70.Text = "디버깅";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(902, 987);
            this.Controls.Add(this.donationButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.tbMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Monkeyhead\'s OCR RealTime Translator ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.ContextOption.ResumeLayout(false);
            this.optionMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tbMain.ResumeLayout(false);
            this.tpBasic.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.pnAdjustImg.ResumeLayout(false);
            this.pnAdjustImg.PerformLayout();
            this.pnOCR.ResumeLayout(false);
            this.pnOCR.PerformLayout();
            this.Tesseract_panel.ResumeLayout(false);
            this.Tesseract_panel.PerformLayout();
            this.pnGoogleOcr.ResumeLayout(false);
            this.pnGoogleOcr.PerformLayout();
            this.pnNHocr.ResumeLayout(false);
            this.pnNHocr.PerformLayout();
            this.WinOCR_panel.ResumeLayout(false);
            this.WinOCR_panel.PerformLayout();
            this.pnTranslate.ResumeLayout(false);
            this.pnTranslate.PerformLayout();
            this.pnDeepl.ResumeLayout(false);
            this.pnDeepl.PerformLayout();
            this.pnGoogleBasic.ResumeLayout(false);
            this.DB_Panel.ResumeLayout(false);
            this.DB_Panel.PerformLayout();
            this.Naver_Panel.ResumeLayout(false);
            this.Naver_Panel.PerformLayout();
            this.Google_Panel.ResumeLayout(false);
            this.Google_Panel.PerformLayout();
            this.pnEzTrans.ResumeLayout(false);
            this.pnEzTrans.PerformLayout();
            this.tpText.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outlineColor2Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outlineColor1Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textColorBox)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeUpDown)).EndInit();
            this.tpExtra.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel21.ResumeLayout(false);
            this.panel21.PerformLayout();
            this.panel25.ResumeLayout(false);
            this.panel25.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgZoomsizeUpDown)).EndInit();
            this.tpTranslation.ResumeLayout(false);
            this.panel19.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel27.ResumeLayout(false);
            this.panel27.PerformLayout();
            this.panel22.ResumeLayout(false);
            this.panel22.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.tpETC.ResumeLayout(false);
            this.panel18.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.panel20.ResumeLayout(false);
            this.panel20.PerformLayout();
            this.panel23.ResumeLayout(false);
            this.panel23.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tpQuickSetting.ResumeLayout(false);
            this.panel28.ResumeLayout(false);
            this.panel31.ResumeLayout(false);
            this.panel31.PerformLayout();
            this.tpDebuging.ResumeLayout(false);
            this.panel24.ResumeLayout(false);
            this.panel26.ResumeLayout(false);
            this.panel26.PerformLayout();
            this.plDebugOn.ResumeLayout(false);
            this.plDebugOn.PerformLayout();
            this.plDebugOff.ResumeLayout(false);
            this.plDebugOff.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.ComboBox TransType_Combobox;
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
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.CheckBox cbDisplayLowQuality;
        private System.Windows.Forms.CheckBox cbShowOCRIndex;
        private System.Windows.Forms.Button btnHideTransEmpty;
        private System.Windows.Forms.Button btnHideTransDefault;
        private KeyInputLabel lbHideTranslate;
        private System.Windows.Forms.Label lbHotKeyHideTransWindow;
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
        private System.Windows.Forms.Label lbNHOcrInfo;
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
    }




}

