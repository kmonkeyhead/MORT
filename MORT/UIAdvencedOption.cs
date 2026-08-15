using MORT.CustomControl;
using MORT.LocalizeManager;
using MORT.Model.CustomApi;
using MORT.Service.CustomApi;
using MORT.SettingData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace MORT
{
    public partial class UIAdvencedOption : Form, ILocalizeForm
    {
        private string _fontData;
        private Dictionary<int, CtSettingHotKey> settingHotKeyDic = new Dictionary<int, CtSettingHotKey>();
        private Dictionary<KeyInputLabel.KeyType, CustomControl.CtHotKey> hotKeyDic = new System.Collections.Generic.Dictionary<KeyInputLabel.KeyType, CustomControl.CtHotKey>();
        private bool _isInit = false;
        private readonly CustomApiPresetService _customApiPresetService;
        private List<CustomApiModel> _customApiPresetList = new List<CustomApiModel>();
        private int _beforeSelectedPresetIndex = -1;
        private const string FilePresetPrefix = "[F] ";

        public UIAdvencedOption()
        {
            InitializeComponent();
            _customApiPresetService = Program.ServiceContainer?.GetService(typeof(CustomApiPresetService)) as CustomApiPresetService;
        }

        private string LocalizeString(string key)
        {
            return LocalizeManager.LocalizeManager.GetLocalizeString(key, "");
        }

        private void AddHotKey(string title, string information, KeyInputLabel.KeyType type, CustomControl.CtHotKey hotKey)
        {
            hotKey.Init(title, information, LocalizeString("Common HotKey Clear"), type);
            hotKeyDic.Add(type, hotKey);
        }

        private void Init()
        {
            settingHotKeyDic.Add(0, ctSettingHotKey1);
            settingHotKeyDic.Add(1, ctSettingHotKey2);
            settingHotKeyDic.Add(2, ctSettingHotKey3);
            settingHotKeyDic.Add(3, ctSettingHotKey4);

            string title = LocalizeManager.LocalizeManager.GetLocalizeString("Adv HotKey Setting Load Input", "");
            string file = LocalizeManager.LocalizeManager.GetLocalizeString("Adv HotKey Setting File Input", "");
            string clear = LocalizeManager.LocalizeManager.GetLocalizeString("Common HotKey Clear", "");
            string fileSelect = LocalizeManager.LocalizeManager.GetLocalizeString("Common File Select", "");

            foreach(var obj in settingHotKeyDic)
            {
                obj.Value.Init(string.Format(title, obj.Key + 1), string.Format(file, obj.Key + 1), clear, fileSelect, obj.Key, KeyInputLabel.KeyType.OpenSetting);
            }

            AddHotKey(LocalizeString("Adv HotKey Force Invisible"), LocalizeString("Adv HotKey Force Invisible Info"), KeyInputLabel.KeyType.LayerTransparency, ctLayerTransparencyHotKey);
            AddHotKey(LocalizeString("Adv HotKey Db"), LocalizeString("Adv HotKey Db Info"), KeyInputLabel.KeyType.DBTranslate, ctDb);
            AddHotKey(LocalizeString("Adv HotKey PaPago"), LocalizeString("Adv HotKey PaPago Info"), KeyInputLabel.KeyType.NaverTranslate, ctNaverTrans);
            AddHotKey(LocalizeString("Adv HotKey BasicTrans"), LocalizeString("Adv HotKey BasicTrans Info"), KeyInputLabel.KeyType.GoogleTranslate, ctGoogleTrans);
            AddHotKey(LocalizeString("Adv HotKey Google Sheet"), LocalizeString("Adv HotKey Google Sheet Info"), KeyInputLabel.KeyType.GoogleSheetTranslate, ctGoogleSheet);
            AddHotKey(LocalizeString("Adv HotKey EzTrans"), LocalizeString("Adv HotKey EzTrans Info"), KeyInputLabel.KeyType.EzTrans, ctEzTrans);
            AddHotKey(LocalizeString("Adv HotKey DeepL"), LocalizeString("Adv HotKey DeepL Info"), KeyInputLabel.KeyType.DeepL, ctDeepL);
            AddHotKey(LocalizeString("Adv HotKey PapagoWeb"), LocalizeString("Adv HotKey PapagoWeb Info"), KeyInputLabel.KeyType.PapagoWeb, ctPapagoWeb);

            LocalizeForm();
            InitData();
        }

        private void InitData()
        {

            _isInit = false;
            //단축키 데이터를 가져온다.
            var list = AdvencedOptionManager.GetHotKeyList();

            foreach(var obj in settingHotKeyDic)
            {
                obj.Value.SetEmpty();
            }

            foreach(var obj in hotKeyDic)
            {
                obj.Value.SetEmpty();
            }

            foreach(var obj in list)
            {
                if(obj.keyType == KeyInputLabel.KeyType.OpenSetting)
                {
                    if(settingHotKeyDic.ContainsKey(obj.index))
                    {
                        settingHotKeyDic[obj.index].SetKeys(obj.keyList, obj.extraData);
                    }
                }
                else if(hotKeyDic.TryGetValue(obj.keyType, out var hotKey))
                {
                    hotKey.SetKeys(obj.keyList);
                }
            }
            //앱 설정
            _cbRttTopMost.Checked = AdvencedOptionManager.EnableRttTopMost;
            cbEnableSystemTray.Checked = AdvencedOptionManager.EnableSystemTrayMode;
            cbEnableBorder.Checked = AdvencedOptionManager.EnableYellowBorder;
            cbEnableRTL.Checked = AdvencedOptionManager.EnableRTL;
            cbUseLegacyOcrAreaFollowMouse.Checked = AdvencedOptionManager.UseLegacyOcrAreaFollowMouse;
            cbUseOnlyOcrAreaFollowMouse.Checked = AdvencedOptionManager.UseOnlyOcrAreaFollowMouse;

            ocrAreaBackgroundColor.BackColor = AdvencedOptionManager.SelectOcrAreaBackgroundColor;
            ocrAreaSelectedColor.BackColor = AdvencedOptionManager.SelectOcrAreaColor;

            //번역창 설정
            cbOverlayAutoSize.Checked = AdvencedOptionManager.IsAutoFontSize;
            cbOverlayAutoMerge.Checked = AdvencedOptionManager.UseAutoMerge;
            cbOverlayKeepSourceDirection.Checked = AdvencedOptionManager.OverlayKeepSourceDirection;
            cbOverlayUseFontOutline.Checked = AdvencedOptionManager.OverlayUseFontOutline;
            cbOverlayAutoColor.Checked = AdvencedOptionManager.OverlayAutoColor;
            cbAutoFontColor.Checked = AdvencedOptionManager.OverlayAutoFontColor;
            cbAutoBackgroundColor.Checked = AdvencedOptionManager.OverlayAutoBackgroundColor;
            cbOverlayUseBackgroundAlpha.Checked = AdvencedOptionManager.OverlayUseBackgroundAlpha;
            _fontData = AdvencedOptionManager.BasicFontData;
            cbLayerAlignmentBottom.Checked = AdvencedOptionManager.LayerTextAlignmentBottom;
            cbLayerAlignmentRight.Checked = AdvencedOptionManager.LayerTextAlignmentRight;
            cbAdvencedHideTransform.Checked = AdvencedOptionManager.EnableAdvencedHideTransform;

            //결과 유지하기
            cbEnableMemory.Checked = AdvencedOptionManager.EnableTranslateMemory;
            SetUpDownValue(udMemoryTime, AdvencedOptionManager.TranslateMemoryLimit);
            SetUpDownValue(udMemoryTime, AdvencedOptionManager.TranslateMemoryRemainTime);

            //TOP MOST 설정
            cbTopMost.Checked = AdvencedOptionManager.UseTopMostOptionWhenTranslate;

            //비어있는 번역 무시
            cbIgonreEmpty.Checked = AdvencedOptionManager.UseIgonoreEmptyTranslate;

            SetUpDownValue(udMinFontSize, AdvencedOptionManager.MinAutoFontSize);
            SetUpDownValue(udMaxSFontize, AdvencedOptionManager.MaxAutoFontSize);
            SetUpDownValue(udSnapShotRemainTime, AdvencedOptionManager.SnapShopRemainTime);

            //번역기 설정
            cbJpnExecutive.Checked = AdvencedOptionManager.IsExecutive;
            cbDeeplAltOption.Checked = AdvencedOptionManager.UseDeeplAltOption;

            //커스텀 api
            _tbCustomURL.Text = AdvencedOptionManager.CustomApiUrl;
            tbCustomApiSource.Text = AdvencedOptionManager.CustomApiLanguageSource;
            tbCustomApiTarget.Text = AdvencedOptionManager.CustomApiLanguageTarget;


            cbCustomApiLanguageCode.Checked = AdvencedOptionManager.UseGoogleLanguageCode;

            //gemini 설정
            tbGeminiCommand.Text = AdvencedOptionManager.GeminiCommand;
            tbGeminiModelName.Text = AdvencedOptionManager.GeminiModel;
            cbDisableDefaultCommand.Checked = AdvencedOptionManager.GeminiDisableDefaultCommand;

            var preset = AdvencedOptionManager.GeminiPreset;
            RenderGeminiPreset(preset);


            //구글 ocr 설정
            cbGoogleOcrPriority.Checked = AdvencedOptionManager.UseGoogleOCRPriority;
            SetUpDownValue(udGoogleOcrLimit, AdvencedOptionManager.GoogleOcrLimit);

            //교정사전 설정
            SetUpDownValue(udReProcessDicCount, AdvencedOptionManager.DicReProcessCount);

            //클립보드 설정
            cbIsUseClipboardTrans.Checked = AdvencedOptionManager.IsUseClipboardTrans;
            cbIsShowClipboardOriginal.Checked = AdvencedOptionManager.IsShowClipboardOriginal;
            cbShowProcessClipboard.Checked = AdvencedOptionManager.IsShowClipboardProcessing;

            //클립보드 저장
            cbClipboardSaveType.SelectedIndex = AdvencedOptionManager.ClipboardSaveType;

            //번역집 설정
            InitTranslationFile();
            InitAppLanguage();

            RenderCustomApiPresetList();
            RenderCustomApiLanguageCode();
            _isInit = true;

        }

        private void RenderGeminiTokenLimit(int value)
        {

            _lbGeminiTokenLimit.Text = $"{value}";
            _geminiTokenLimit.Value = value;
        }

        private void RenderGeminiThinkingBudget(int value)
        {

            string text = LocalizeManager.LocalizeManager.GetLocalizeString($"Adv Gemini Thinking Budget {value}", $"{value}");
            _lbThinkingBudget.Text = text;
            _geminiThinkingBudget.Value = value;
        }

        private void RenderGeminiTemperature(int value)
        {

            _lbGeminiTemperature.Text = $"{(float)value / 100:f2}";
            _geminiTemperature.Value = value;
        }

        private void RenderGeminiPreset(GeminiPresetValue preset)
        {
            RenderGeminiPresetRadioButton(preset.PresetType);

            if(preset.PresetType == GeminiPresetValue.CustomPreset)
            {
                RenderGeminiTemperature(preset.TemperatureValue);
                RenderGeminiThinkingBudget(preset.ThinkingBudgetValue);
                RenderGeminiTokenLimit(preset.TokenLimitValue);
            }
            else
            {
                RenderGeminiPresetValue(preset.PresetType);
            }
        }

        private void RenderGeminiPresetValue(int presetType)
        {
            RenderGeminiTemperature(GeminiPresetValue.TemperaturePresetValue(presetType));
            RenderGeminiThinkingBudget(GeminiPresetValue.ThinkingBudgetPresetValue(presetType));
            RenderGeminiTokenLimit(GeminiPresetValue.TokenLimitPresetValue(presetType));
        }

        private void RenderGeminiPresetRadioButton(int presetType)
        {
            _geminiTemperature.Enabled = presetType == GeminiPresetValue.CustomPreset;
            _geminiThinkingBudget.Enabled = presetType == GeminiPresetValue.CustomPreset;
            _geminiTokenLimit.Enabled = presetType == GeminiPresetValue.CustomPreset;
            if(presetType == GeminiPresetValue.DefaultPreset)
            {
                _rbGeminiPresetDefault.Checked = true;
            }
            else if(presetType == GeminiPresetValue.LowPreset)
            {
                _rbGeminiPresetLow.Checked = true;
            }
            else if(presetType == GeminiPresetValue.CustomPreset)
            {
                _rbGeminiPresetCustom.Checked = true;
            }
            else
            {
                _rbGeminiPresetCustom.Checked = true;
            }
        }



        private void SetUpDownValue(NumericUpDown componet, int value)
        {
            if(value < componet.Minimum)
            {
                value = (int)componet.Minimum;
            }
            else if(value > componet.Maximum)
            {
                value = (int)componet.Maximum;
            }

            componet.Value = value;
        }

        #region :::::::::: 앱 설정 ::::::::::

        public void SetAppSetting()
        {
            AdvencedOptionManager.EnableRttTopMost = _cbRttTopMost.Checked;
            AdvencedOptionManager.EnableSystemTrayMode = cbEnableSystemTray.Checked;
            AdvencedOptionManager.EnableYellowBorder = cbEnableBorder.Checked;

            AdvencedOptionManager.SelectOcrAreaBackgroundColor = ocrAreaBackgroundColor.BackColor;
            AdvencedOptionManager.SelectOcrAreaColor = ocrAreaSelectedColor.BackColor;

            AdvencedOptionManager.EnableRTL = cbEnableRTL.Checked;

            bool beforeUseLegacyOcrAreaFollowMouse = AdvencedOptionManager.UseLegacyOcrAreaFollowMouse;
            AdvencedOptionManager.UseLegacyOcrAreaFollowMouse = cbUseLegacyOcrAreaFollowMouse.Checked;
            AdvencedOptionManager.UseOnlyOcrAreaFollowMouse = cbUseOnlyOcrAreaFollowMouse.Checked;
            if (!beforeUseLegacyOcrAreaFollowMouse && AdvencedOptionManager.UseLegacyOcrAreaFollowMouse)
            {
                FormManager.Instace.MyMainForm.ClearOcrAreaFollowMouseForm();
            }
        }

        #endregion


        #region :::::::::::: 고급 단축키 :::::::::::


        private void SetHotKey()
        {
            //설정
            List<HotKeyData> keyList = new List<HotKeyData>();

            foreach(var obj in settingHotKeyDic)
            {
                string result = obj.Value.Apply();
                HotKeyData data = new HotKeyData(obj.Key, obj.Value.keyType, obj.Value.GetKeys(), result, obj.Value.FilePath);
                keyList.Add(data);
            }

            foreach(var obj in hotKeyDic)
            {
                string keyResult = obj.Value.Apply();
                HotKeyData keyData = new HotKeyData(obj.Value, keyResult);
                keyList.Add(keyData);
            }

            AdvencedOptionManager.SetHotKey(keyList);

        }


        #endregion

        #region ::::::::: 번역 관련 :::::::::
        public void SetTranslatorSetting()
        {
            AdvencedOptionManager.SetExecutive(cbJpnExecutive.Checked);
            AdvencedOptionManager.SetDeeplOption(cbDeeplAltOption.Checked);

            AdvencedOptionManager.SetCustomApiOption(cbCustomApiLanguageCode.Checked, tbCustomApiSource.Text, tbCustomApiTarget.Text, _tbCustomURL.Text);


            AdvencedOptionManager.SetGeminiOption(tbGeminiCommand.Text, tbGeminiModelName.Text, cbDisableDefaultCommand.Checked);
            int presetType = 0;
            if(_rbGeminiPresetDefault.Checked)
            {
                presetType = GeminiPresetValue.DefaultPreset;
            }
            else if(_rbGeminiPresetLow.Checked)
            {
                presetType = GeminiPresetValue.LowPreset;
            }
            else if(_rbGeminiPresetCustom.Checked)
            {
                presetType = GeminiPresetValue.CustomPreset;
            }
            GeminiPresetValue preset = new GeminiPresetValue((int)_geminiTemperature.Value, (int)_geminiThinkingBudget.Value, (int)_geminiTokenLimit.Value, presetType);
            AdvencedOptionManager.SetGeminiPreset(preset);
            ApplyCustomApreset();
        }

        private void ApplyCustomApreset()
        {
            SaveCurrentPreset();

            var fileItems = new List<CustomApiModel>();
            var normalItems = new List<CustomApiModel>();
            foreach(var m in _customApiPresetList)
            {
                if(m == null)
                {
                    continue;
                }
                if(_customApiPresetService.IsFilePreset(m.Name))
                {
                    fileItems.Add(m);
                }
                else
                {
                    normalItems.Add(m);
                }
            }

            // 일반 프리셋이 비어있고 파일 기반도 없으면 기존 폴백 유지
            if(normalItems.Count == 0 && fileItems.Count == 0)
            {
                normalItems.Add(new CustomApiModel("bla", "bla", "bla", "blabla", "bla", new()));
            }

            foreach(var fm in fileItems)
            {
                _customApiPresetService.SaveFilePreset(fm);
            }
            _customApiPresetService.SaveAdditionalList(normalItems);
        }



        #endregion

        private void SetOcrSetting()
        {
            AdvencedOptionManager.UseGoogleOCRPriority = cbGoogleOcrPriority.Checked;
            AdvencedOptionManager.GoogleOcrLimit = (int)udGoogleOcrLimit.Value;
        }


        #region ::::::::::: 번역창 관련 ::::::::::::

        public void SetOverlaySetting()
        {
            AdvencedOptionManager.SetOverLay(
                cbOverlayAutoSize.Checked,
                (int)udMinFontSize.Value, (int)udMaxSFontize.Value,
                (int)udSnapShotRemainTime.Value,
                cbOverlayAutoMerge.Checked,
                cbOverlayKeepSourceDirection.Checked,
                cbOverlayAutoColor.Checked,
                cbAutoFontColor.Checked,
                cbAutoBackgroundColor.Checked,
                cbOverlayUseBackgroundAlpha.Checked);
            AdvencedOptionManager.SetOverlayFontOutline(cbOverlayUseFontOutline.Checked);
        }

        public void SetLayerSetting()
        {
            AdvencedOptionManager.SetLayer(cbLayerAlignmentBottom.Checked, cbLayerAlignmentRight.Checked);
        }

        public void SetTranslationFormSetting()
        {
            AdvencedOptionManager.SetTranslationFormSetting(cbTopMost.Checked, cbIgonreEmpty.Checked, _fontData, cbAdvencedHideTransform.Checked);
            AdvencedOptionManager.SetTranslateMemory(cbEnableMemory.Checked, (int)udMemoryCount.Value, (int)udMemoryTime.Value);
        }

        #endregion

        #region:::::::::: 교정사전 관련 ::::::::::

        public void SetDicSetting()
        {
            AdvencedOptionManager.SetDicReProcessCount((int)udReProcessDicCount.Value);
        }

        #endregion

        #region ::::::::: 클립보드 관련 ::::::::::

        public void SetClipboardSetting()
        {
            AdvencedOptionManager.SetClipboardTrans(cbIsUseClipboardTrans.Checked, cbIsShowClipboardOriginal.Checked, cbShowProcessClipboard.Checked);
        }

        public void SetClipboardSaveSetting()
        {
            AdvencedOptionManager.SetClipboardSave(cbClipboardSaveType.SelectedIndex);
        }

        #endregion


        #region ::::::::::: 번역집 관련 :::::::::::

        private void InitTranslationFile()
        {
            cblTransration.Items.Clear();

            string[] fileEntries = Directory.GetFiles(GlobalDefine.ADVENCED_TRANSRATION_PATH, "*.txt");
            List<string> fileList = AdvencedOptionManager.TranslationFileList;
            foreach(var obj in fileEntries)
            {
                bool isCheck = false;
                string fileName = Path.GetFileNameWithoutExtension(obj);

                if(fileList.Contains(fileName))
                {
                    isCheck = true;
                }

                cblTransration.Items.Add(fileName, isCheck);
            }

            cbUseDbStyle.Checked = AdvencedOptionManager.IsTranslationDbStyle;
            cbCheckStringUpper.Checked = AdvencedOptionManager.IsTranslationStringUpper;

            gbDbOption.Enabled = cbUseDbStyle.Checked;
        }

        public void SetTranslationFile()
        {
            List<string> list = new List<string>();
            foreach(var obj in cblTransration.CheckedItems)
            {
                list.Add(obj.ToString());
            }

            AdvencedOptionManager.TranslationFileList = list;
            AdvencedOptionManager.IsTranslationDbStyle = cbUseDbStyle.Checked;
            AdvencedOptionManager.IsTranslationStringUpper = cbCheckStringUpper.Checked;
        }

        private void GetTranslationInfo(string file)
        {
            string information = Util.ParseStringFromFile(file, "@INFO");
            information = information.Replace("\r\n", "\n");
            information = information.Replace("\n", System.Environment.NewLine);

            tbInformation.Text = information;
        }

        private void cblTransration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cblTransration.SelectedItem == null)
            {
                return;
            }

            string file = cblTransration.SelectedItem.ToString();
            Console.WriteLine(file);
            GetTranslationInfo(GlobalDefine.ADVENCED_TRANSRATION_PATH + file + ".txt");
        }

        private void OnClickTransrtionAllOn(object sender, EventArgs e)
        {
            for(int i = 0; i < cblTransration.Items.Count; i++)
            {
                cblTransration.SetItemCheckState(i, CheckState.Checked);
            }
        }

        private void OnClickTransrtionAllOff(object sender, EventArgs e)
        {
            for(int i = 0; i < cblTransration.Items.Count; i++)
            {
                cblTransration.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void cbUseDbStyle_CheckedChanged(object sender, EventArgs e)
        {
            gbDbOption.Enabled = cbUseDbStyle.Checked;
        }


        #endregion

        #region :::::::::: 앱 언어 관련 ::::::::::

        private void SetAppLanguage()
        {
            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", '[', ']');
            string saveValue = "";
            var before = LocalizeManager.LocalizeManager.ConvertAppLanguage(result);
            var current = LocalizeManager.AppLanguage.Korea;

            if(rbEnglish.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "en");
                current = LocalizeManager.AppLanguage.English;
                saveValue = "en";
            }
            else if(rbKorea.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "ko");
                current = LocalizeManager.AppLanguage.Korea;
                saveValue = "ko";
            }
            else if(rbJpn.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "ja");
                current = LocalizeManager.AppLanguage.Japanese;
                saveValue = "ja";
            }

            else if(rbzh.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "zh");
                current = LocalizeManager.AppLanguage.SimplifiedChinese;
                saveValue = "zh";
            }
            else if(rbId.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "id");
                current = LocalizeManager.AppLanguage.Indonesian;
                saveValue = "id";
            }
            else if(rbRu.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "ru");
                current = LocalizeManager.AppLanguage.Russian;
                saveValue = "ru";
            }
            else if(rbPt.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "pt");
                current = LocalizeManager.AppLanguage.Portuguese;
                saveValue = "pt";
            }
            else if(rbUk.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "uk");
                current = LocalizeManager.AppLanguage.Ukrainian;
                saveValue = "pt";
            }
            else if(rbTr.Checked)
            {
                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", "tr");
                current = LocalizeManager.AppLanguage.Turkish;
                saveValue = "tr";
            }

            if(before != current)
            {
                string message = LocalizeManager.LocalizeManager.GetLocalizeString("LanguageNotice", "언어는 앱을 재시작해야 적용됩니다.", current);
                FormManager.ShowPopupMessage("", message);
            }

            // Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@SET_BASIC_DEFAULT_PAGE ", saveValue);
        }

        private void InitAppLanguage()
        {
            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", '[', ']');
            var language = LocalizeManager.LocalizeManager.ConvertAppLanguage(result);

            switch(language)
            {
                case LocalizeManager.AppLanguage.Korea:
                    rbKorea.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.English:
                    rbEnglish.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Japanese:
                    rbJpn.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.SimplifiedChinese:
                    rbzh.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Indonesian:
                    rbId.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Russian:
                    rbRu.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Portuguese:
                    rbPt.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Ukrainian:
                    rbUk.Checked = true;
                    break;

                case LocalizeManager.AppLanguage.Turkish:
                    rbTr.Checked = true;
                    break;

                default:
                    rbKorea.Checked = true;
                    break;
            }
        }



        #endregion

        private void UIAdvencedOption_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(FormManager.GetIsRemain())
            {
                FormManager.Instace.DestoryAdvencedOption();
            }
        }

        private void OnClickApply(object sender, EventArgs e)
        {
            SetAppSetting();
            SetHotKey();
            SetOverlaySetting();
            SetLayerSetting();
            SetTranslationFormSetting();
            SetDicSetting();
            SetClipboardSetting();
            SetClipboardSaveSetting();
            SetTranslatorSetting();
            SetOcrSetting();
            SetTranslationFile();
            SetAppLanguage();

            AdvencedOptionManager.Save();

            FormManager.Instace.MyMainForm.ApplyAdvencedOption();
            FormManager.ShowPopupMessage("MORT", LocalizeString("Apply Complete"));
        }


        private void OnClickReset(object sender, EventArgs e)
        {
            if(DialogResult.OK == MessageBox.Show("설정을 초기화 하시겠습니까?", "고급 설정 초기화", MessageBoxButtons.OKCancel))
            {
                AdvencedOptionManager.Reset();
                AdvencedOptionManager.Save();

                InitData();

                FormManager.Instace.MyMainForm.ApplyAdvencedOption();
            }
        }

        private void UIAdvencedOption_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void udMinFontSize_ValueChanged(object sender, EventArgs e)
        {
            if(_isInit)
            {
                if(udMinFontSize.Value > udMaxSFontize.Value)
                {
                    udMaxSFontize.Value = udMinFontSize.Value;
                }

            }
        }

        private void udMaxSFontize_ValueChanged(object sender, EventArgs e)
        {
            if(_isInit)
            {
                if(udMinFontSize.Value > udMaxSFontize.Value)
                {
                    udMinFontSize.Value = udMaxSFontize.Value;
                }
            }
        }

        private void udReProcessDicCount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(_fontData))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(SerializableFont));
                    using(TextReader tr = new StringReader(_fontData))
                    {
                        SerializableFont font = (SerializableFont)deserializer.Deserialize(tr);
                        fontDialog.Font = font.ToFont();
                    }
                }
            }
            catch
            {

            }

            try
            {
                DialogResult dr = this.fontDialog.ShowDialog();
                //확인버튼 누르면 변경
                if(dr == DialogResult.OK)
                {
                    this.Invoke(new Action(delegate ()
                    {
                        SerializableFont serializableFont = new SerializableFont(this.fontDialog.Font);
                        XmlSerializer serializer = new XmlSerializer(serializableFont.GetType());
                        using(StringWriter sw = new StringWriter())
                        {
                            serializer.Serialize(sw, serializableFont);
                            _fontData = sw.ToString();
                            Console.WriteLine(_fontData);
                        }
                    }));
                }
            }
            catch(System.ArgumentException ex)
            {
                MessageBox.Show("사용할 수 없는 폰트입니다");
            }

        }

        public void LocalizeForm()
        {
            this.LocalizeLabel("Advenced Config");
            //버튼
            btnApply.LocalizeLabel("Common Apply");
            btReset.LocalizeLabel("Common Default");

            //탭
            AppConfigTab.LocalizeLabel("TabAppConfig");
            HotKeyTab.LocalizeLabel("TabHotKey");
            TransFormTab.LocalizeLabel("TabTransForm");
            TransZipTab.LocalizeLabel("TabTransBook");
            TransTab.LocalizeLabel("TabTrans");
            OcrTab.LocalizeLabel("TabOCR");
            DicTab.LocalizeLabel("TabDic");
            //앱 설정
            gbGeneral.LocalizeLabel("AdvencedGbGeneral");
            cbEnableSystemTray.LocalizeLabel("AdvencedCbSystemTray");
            cbEnableRTL.LocalizeLabel("AdvencedCbEnableRTL");
            _cbRttTopMost.LocalizeLabel("AdvencedCbRttTopMost");
            gbMouseFollowOcrArea.LocalizeLabel("Advenced Mouse Follow Ocr Area");
            cbUseLegacyOcrAreaFollowMouse.LocalizeLabel("Advenced Use Legacy Ocr Area Follow Mouse");
            cbUseOnlyOcrAreaFollowMouse.LocalizeLabel("Advenced Use Only Ocr Area Follow Mouse");

            gbAttachWindow.LocalizeLabel("Advenced Attach Window");
            cbEnableBorder.LocalizeLabel("Advenced Enable Yellow Border");

            gbOcrAreaColor.LocalizeLabel("Advenced OCR Area Background Color");
            lbOcrAreaBackgroundColor.LocalizeLabel("Adv OCR Area Background");
            lbOcrAreaSelectedColor.LocalizeLabel("Adv OCR Area Select");
            btnColorPreview.LocalizeLabel("Adv OCR Area Preview");
            btnSetDefaultColor.LocalizeLabel("Adv OCR Area Set Default");


            //고급 단축키
            gbHotKeySetting.LocalizeLabel("Adv HotKey Setting");
            gbHotKeyTransform.LocalizeLabel("Adv HotKey Transform");
            gbHotKeyTrans.LocalizeLabel("Adv HotKey Trans");


            //번역창
            gbOverlay.LocalizeLabel("Adv Transform Overlay");
            gbDark.LocalizeLabel("Adv Transform Dark");
            gbTransformLayer.LocalizeLabel("Adv Transform Layer");
            gbTransformGeneral.LocalizeLabel("Adv Transform General");

            cbOverlayAutoSize.LocalizeLabel("Adv Overlay Auto Size");
            cbOverlayAutoMerge.LocalizeLabel("Adv Overlay Auto Merge");
            cbOverlayKeepSourceDirection.LocalizeLabel("Adv Overlay Keep Source Direction");
            cbOverlayUseFontOutline.LocalizeLabel("Adv Overlay Use Font Outline");
            cbOverlayUseBackgroundAlpha.LocalizeLabel("Adv Overlay Use Background Alpha");
            cbOverlayAutoColor.LocalizeLabel("Adv Overlay Auto Color Option");
            gbAutoColor.LocalizeLabel("Adv Overlay Auto Color");
            cbAutoFontColor.LocalizeLabel("Adv Overlay Auto Font Color");
            cbAutoBackgroundColor.LocalizeLabel("Adv Overlay Auto Background Color");

            lbOverlayFontMinSize.LocalizeLabel("Adv Overlay Min Font Size");
            lbOverlayFontMaxSize.LocalizeLabel("Adv Overlay Max Font Size");
            lbOverlaySnapShotRemainTime.LocalizeLabel("Adv Overlay Snap Shot Remain Time");
            btnFont.LocalizeLabel("Adv Font Setting");
            cbLayerAlignmentBottom.LocalizeLabel("Adv Layer Alignment Bottom");
            cbLayerAlignmentRight.LocalizeLabel("Adv Layer Alignment Right");
            cbIgonreEmpty.LocalizeLabel("Adv Ignore Empty");
            cbTopMost.LocalizeLabel("Adv Topmost");
            cbAdvencedHideTransform.LocalizeLabel("Adv Enable Advenced Hide Transform");

            udMaxSFontize.Anchor(lbOverlayFontMaxSize, 10, 50);
            udMinFontSize.Anchor(lbOverlayFontMinSize, 10, 50);
            udSnapShotRemainTime.Anchor(lbOverlaySnapShotRemainTime, 10, 50);

            gbMemory.LocalizeLabel("Adv Memory Title");
            cbEnableMemory.LocalizeLabel("Adv Enable Memory");
            lbMemoryCount.LocalizeLabel("Adv Memory Count");
            lbMemoryTime.LocalizeLabel("Adv Memory Time");
            lbMemoryInformation.LocalizeLabel("Adv Memory Information");

            udMemoryCount.Anchor(lbMemoryCount, 10, 50);
            udMemoryTime.Anchor(lbMemoryTime, 10, 50);

            //번역집
            gbTranslationZip.LocalizeLabel("Adv Translation Zip");
            lbUseTranslationZip.LocalizeLabel("Adv Use Translation Zip");
            lbInfoTranslationZip.LocalizeLabel("Adv Info Translation Zip");
            cbUseDbStyle.LocalizeLabel("Adv DbStyle Translation Zip");
            btAllOff.LocalizeLabel("Common Clear All");
            btAllOn.LocalizeLabel("Common Selact All");
            gbDbOption.LocalizeLabel("Adb DbStyleOption Translation Zip");
            cbCheckStringUpper.LocalizeLabel("Adv DbStyleOption String Upper Translation Zip");

            //번역 설정
            gbGoogleTrans.LocalizeLabel("Adv Google translation");
            gbDeepL.LocalizeLabel("Adv DeepL Option");
            gbClipboard.LocalizeLabel("Adv Clipboard");
            cbJpnExecutive.LocalizeLabel("Adv Jpn Executive");
            cbDeeplAltOption.LocalizeLabel("Adv Deepl Use Alt Option");
            cbIsUseClipboardTrans.LocalizeLabel("Adv Use Clipboard");
            cbIsShowClipboardOriginal.LocalizeLabel("Adv Clipboard Display Original");
            cbShowProcessClipboard.LocalizeLabel("Adv Clipboard Display Progress");

            gbCustomApi.LocalizeLabel("Adv Custom Api");
            gbCustomApiCode.LocalizeLabel("Adv Custom Api Language Code");
            cbCustomApiLanguageCode.LocalizeLabel("Adv Custom Api Use Google Language Code");
            gbCustomPreset.LocalizeLabel("Adv Custom Api Preset");
            _btCustomAdd.LocalizeLabel("Common Add");
            _btCustomRemove.LocalizeLabel("Common Remove");
            _lbCustomName.LocalizeLabel("Adv Custom Api Preset Name");

            gbGeminiCommand.LocalizeLabel("Adv Gemini API Command");
            cbDisableDefaultCommand.LocalizeLabel("Adv Gemini Disable Default Command");
            lbGeminiCustomModel.LocalizeLabel("Adv Gemini Custom Model");
            lbCustomInformation.LocalizeLabel("Adv Custom Api Information");
            lbDisableDefaultInformation.LocalizeLabel("Adv Gemini Disable Default Command Information");
            _gbGeminiThinking.LocalizeLabel("Adv Gemini Thinking Config");
            _lbGeminiPreset.LocalizeLabel("Adv Gemini Preset");
            _rbGeminiPresetDefault.LocalizeLabel("Adv Gemini Preset Default");
            _rbGeminiPresetLow.LocalizeLabel(_rbGeminiPresetDefault, "Adv Gemini Preset Low", 10);
            _rbGeminiPresetCustom.LocalizeLabel(_rbGeminiPresetLow, "Adv Gemini Preset Custom", 10);



            //OCR 설정
            gbGoogleOcr.LocalizeLabel("Adv Google Ocr");
            cbGoogleOcrPriority.LocalizeLabel("Adv Priority Google Ocr");
            lbGoogleOcrLimit.LocalizeLabel("Adv Google Ocr Limit");
            lbLimitInfo1.LocalizeLabel("Adv Google Ocr Limit Info1");
            lbLimitInfo2.LocalizeLabel("Adv Google Ocr Limit Info2");
            lbLimitInfo3.LocalizeLabel("Adv Google Ocr Limit Info3");

            udGoogleOcrLimit.Anchor(lbGoogleOcrLimit, 10);

            //클립보드 저장 설정
            gbClipboardSave.LocalizeLabel("Adv Clipboard Save");
            lbClipboardSaveType.LocalizeLabel("Adv Clipboard Save Type");
            cbClipboardSaveType.LocalizeItems();
            cbClipboardSaveType.Anchor(lbClipboardSaveType, 10);

            //교정 사전
            gbDic.LocalizeLabel("Adv Dic");
            lbReProcessDic.LocalizeLabel("Adv RePorcess Dic");
            lbDicInfo.LocalizeLabel("Adv Dic Info");
            udReProcessDicCount.Anchor(lbReProcessDic, 10);
        }

        private void RenderCustomApiPresetList()
        {
            _beforeSelectedPresetIndex = -1;
            var models = _customApiPresetService.GetAllPresets();
            _listCustomPreset.Items.Clear();
            _customApiPresetList.Clear();
            _customApiPresetList.AddRange(models);

            // 리스트에 이름 추가 (파일 기반은 [F] prefix 표시)
            foreach(var obj in _customApiPresetList)
            {
                if(obj == null)
                {
                    continue;
                }
                _listCustomPreset.Items.Add(_customApiPresetService.IsFilePreset(obj.Name) ? FilePresetPrefix + obj.Name : obj.Name);
            }

            // 첫 번째 모델 정보 입력
            if(_customApiPresetList.Count > 0)
            {
                _listCustomPreset.SelectedIndex = 0;
                _beforeSelectedPresetIndex = 0;
                var first = _customApiPresetList[0];
                RenderCustomApiPreset(first);
            }
        }

        private void RenderCustomApiPreset(CustomApiModel model)
        {
            if(model == null)
            {
                return;
            }
            _tbCustomURL.Text = model.Url;
            _tbPresetName.Text = model.Name;
            _tbCustomRequest.Text = model.Request;
            _tbCustomResponse.Text = model.Response;
            _tbCustomHeaders.Text = model.Headers != null ? string.Join(Environment.NewLine, model.Headers) : "";

            bool isFile = _customApiPresetService.IsFilePreset(model.Name);
            _tbPresetName.ReadOnly = isFile;
            _btCustomRemove.Enabled = !isFile;
        }

        private void RenderCustomApiLanguageCode()
        {
            gbCustomApiCode.Enabled = !cbCustomApiLanguageCode.Checked;
        }

        private void cbCustomApiLanguageCode_CheckedChanged(object sender, EventArgs e) => RenderCustomApiLanguageCode();

        private void ocrAreaBackgroundColor_Click(object sender, EventArgs e)
        {
            Opulos.Core.UI.AlphaColorDialog acd = new Opulos.Core.UI.AlphaColorDialog();

            acd.ColorChanged += delegate
            {
                System.Diagnostics.Debug.WriteLine("Color changed: " + acd.Color);
            };

            acd.SetColor(ocrAreaBackgroundColor.BackColor);
            DialogResult dr2 = acd.ShowDialog();


            if(dr2 == DialogResult.OK)
            {
                Util.ShowLog("Result = " + acd.Color.A.ToString());
                var resultColor = acd.Color;
                ocrAreaBackgroundColor.BackColor = resultColor;
            }
            acd.Dispose();
        }

        private void ocrAreaSelectedColor_Click(object sender, EventArgs e)
        {
            this.colorDialog1.Color = AdvencedOptionManager.SelectOcrAreaColor;
            DialogResult dr = this.colorDialog1.ShowDialog();

            if(dr == DialogResult.OK)
            {
                ocrAreaSelectedColor.BackColor = this.colorDialog1.Color;
            }
        }

        private void btnSetDefaultColor_Click(object sender, EventArgs e)
        {
            ocrAreaBackgroundColor.BackColor = Color.White;
            ocrAreaSelectedColor.BackColor = Color.Black;
        }

        private void btnColorPreview_Click(object sender, EventArgs e)
        {
            screenForm.MakePreview(ocrAreaSelectedColor.BackColor, ocrAreaBackgroundColor.BackColor);
        }

        private void cbOverlayAutoColor_CheckedChanged(object sender, EventArgs e)
        {
            gbAutoColor.Enabled = cbOverlayAutoColor.Checked;
        }

        private void _geminiTemperature_ValueChanged(object sender, EventArgs e)
        {
            if(!_isInit)
            {
                return;
            }
            RenderGeminiTemperature(_geminiTemperature.Value);
        }

        private void _geminiTokenLimit_ValueChanged(object sender, EventArgs e)
        {
            if(!_isInit)
            {
                return;
            }
            RenderGeminiTokenLimit(_geminiTokenLimit.Value);
        }

        private void _geminiThinkingBudget_ValueChanged(object sender, EventArgs e)
        {
            if(!_isInit)
            {
                return;
            }
            RenderGeminiThinkingBudget(_geminiThinkingBudget.Value);
        }

        private void rbGeminiPresetDefault_CheckedChanged(object sender, EventArgs e)
        {
            if(!_isInit || !_rbGeminiPresetDefault.Checked)
            {
                return;
            }

            RenderGeminiPresetRadioButton(GeminiPresetValue.DefaultPreset);
            RenderGeminiPresetValue(GeminiPresetValue.DefaultPreset);
        }

        private void rbGeminiPresetLow_CheckedChanged(object sender, EventArgs e)
        {
            if(!_isInit || !_rbGeminiPresetLow.Checked)
            {
                return;
            }

            RenderGeminiPresetRadioButton(GeminiPresetValue.LowPreset);
            RenderGeminiPresetValue(GeminiPresetValue.LowPreset);
        }

        private void rbGeminiPresetCustom_CheckedChanged(object sender, EventArgs e)
        {
            if(!_isInit || !_rbGeminiPresetCustom.Checked)
            {
                return;
            }
            RenderGeminiPresetRadioButton(GeminiPresetValue.CustomPreset);
        }

        private void _listCustomPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!_isInit)
            {
                return;
            }

            int index = _listCustomPreset.SelectedIndex;

            SaveCurrentPreset();
            _beforeSelectedPresetIndex = index;
            RenderCustomApiPreset(_customApiPresetList[index]);
        }

        private void SaveCurrentPreset()
        {
            bool before = _isInit;
            _isInit = false;
            string request = _tbCustomRequest.Text;
            string response = _tbCustomResponse.Text;
            string presetName = _tbPresetName.Text;
            string url = _tbCustomURL.Text;

            string headerText = _tbCustomHeaders.Text;
            List<string> headers = ParseHeaders(headerText);

            // 파일 기반 프리셋은 이름 변경 불가 - 원본 이름 유지
            bool isFile = (0 <= _beforeSelectedPresetIndex)
                          && _beforeSelectedPresetIndex < _customApiPresetList.Count
                          && _customApiPresetService.IsFilePreset(_customApiPresetList[_beforeSelectedPresetIndex].Name);

            string finalName = isFile
                ? _customApiPresetList[_beforeSelectedPresetIndex].Name
                : GetUniquePresetName(presetName, _beforeSelectedPresetIndex);

            CustomApiModel newModel = new(finalName, url, "", request, response, headers);

            if(0 <= _beforeSelectedPresetIndex)
            {
                _customApiPresetList[_beforeSelectedPresetIndex] = newModel;
                _listCustomPreset.Items[_beforeSelectedPresetIndex] = isFile
                    ? FilePresetPrefix + finalName
                    : finalName;

                // UI에 고유 이름 반영 (중복일 경우에만)
                if(presetName != finalName)
                {
                    _tbPresetName.Text = finalName;
                }
            }

            _isInit = before;
        }

        private List<string> ParseHeaders(string headerText)
        {
            List<string> headers = new List<string>();
            if(string.IsNullOrWhiteSpace(headerText))
            {
                return headers;
            }

            // Split by newline or comma
            string[] separators = new[] { "\r\n", "\r", "\n", "," };
            string[] parts = headerText.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            foreach(var part in parts)
            {
                string trimmed = part.Trim();
                if(!string.IsNullOrEmpty(trimmed))
                {
                    headers.Add(trimmed);
                }
            }

            return headers;
        }

        private string GetUniquePresetName(string baseName, int excludeIndex = -1)
        {
            // 현재 인덱스를 제외한 모든 프리셋 이름 수집
            var existingNames = new HashSet<string>();
            for(int i = 0; i < _customApiPresetList.Count; i++)
            {
                if(i != excludeIndex && _customApiPresetList[i] != null)
                {
                    existingNames.Add(_customApiPresetList[i].Name);
                }
            }

            // 기본 이름이 중복되지 않으면 그대로 반환
            if(!existingNames.Contains(baseName))
            {
                return baseName;
            }

            // 중복되면 (1), (2), ... 형식으로 고유한 이름 생성
            int counter = 1;
            string uniqueName;
            do
            {
                uniqueName = $"{baseName} ({counter})";
                counter++;
            } while(existingNames.Contains(uniqueName));

            return uniqueName;
        }

        private void _btCustomAdd_Click(object sender, EventArgs e)
        {
            _isInit = false;

            SaveCurrentPreset();
            string baseName = $"New Preset - {_customApiPresetList.Count}";
            string uniqueName = GetUniquePresetName(baseName);
            var model = new CustomApiModel(uniqueName, "https://", "", "", "", new());
            _customApiPresetList.Add(model);
            _listCustomPreset.Items.Add(model.Name);

            _listCustomPreset.SelectedIndex = _customApiPresetList.Count - 1;
            _beforeSelectedPresetIndex = _listCustomPreset.SelectedIndex;
            RenderCustomApiPreset(model);

            _isInit = true;

        }

        private void _btCustomRemove_Click(object sender, EventArgs e)
        {
            _isInit = false;
            int index = _listCustomPreset.SelectedIndex;
            if(0 <= index && index < _customApiPresetList.Count
               && _customApiPresetService.IsFilePreset(_customApiPresetList[index].Name))
            {
                _isInit = true;
                return;
            }
            if(0 <= index)
            {
                _customApiPresetList.RemoveAt(index);
                _listCustomPreset.Items.RemoveAt(index);

                if(_customApiPresetList.Count <= 0)
                {
                    _listCustomPreset.SelectedIndex = -1;
                    _beforeSelectedPresetIndex = -1;
                    RenderCustomApiPreset(new CustomApiModel("", "", "", "", "", new()));
                    return;
                }

                if(_customApiPresetList.Count <= index)
                {
                    index = _customApiPresetList.Count - 1;
                }


                var model = _customApiPresetList[index];
                _listCustomPreset.SelectedIndex = index;
                _beforeSelectedPresetIndex = index;
                RenderCustomApiPreset(model);
            }

            _isInit = true;
        }
    }
}
