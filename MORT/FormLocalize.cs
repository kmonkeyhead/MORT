using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using MORT.LocalizeManager;
using MORT.TransAPI;

namespace MORT
{
    public partial class Form1 : IGoogleBasicTranslateAPIContract, IDeeplAPIContract
    {
        private void InitLocalize()
        {
            AppLanguage language = AppLanguage.Korea; // 어디선가 가져와야 한다
            CultureInfo ci = CultureInfo.CurrentUICulture;

            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", '[', ']');

            if(string.IsNullOrEmpty(result))
            {
                result = ci.TwoLetterISOLanguageName;
            }
            language = LocalizeManager.LocalizeManager.ConvertAppLanguage(result);
            LocalizeManager.LocalizeManager.Init(Properties.Resources.localize, language);

            //TODO : 딴곳에 둬야한다
            GoogleBasicTranslateAPI.instance.InitContract(this);
            GoogleBasicTranslateAPI.instance.UpdateCondition();
            TransManager.Instace.InitDeepLContract(this);
            LocalizeContext();
            LocalizeBasicForm();
        }

        private string LocalizeString(string key, bool replaceLine = false)
        {
             return LocalizeManager.LocalizeManager.GetLocalizeString(key).Replace("[]", "");
        }

        private void LocalizeContext()
        {
            optionToolStripMenuItem.LocalizeLabel("Context Option");
            showTransToolStripMenuItem.LocalizeLabel("Context Translate");
            rTTToolStripMenuItem.LocalizeLabel("Context Remocon");
            setTranslateTopMostToolStripMenuItem.LocalizeLabel("Context Fix Translate");
            setCheckSpellingToolStripMenuItem.LocalizeLabel("Context Use Dic");
            setCutPointToolStripMenuItem.LocalizeLabel("Context Set OCR Area");
            transToolStripMenuItem.LocalizeLabel("Context Start Translate");
            settingToolStripMenuItem.LocalizeLabel("Context Setting");
            settingSaveToolStripMenuItem2.LocalizeLabel("Context Save");
            settingLoadToolStripMenuItem2.LocalizeLabel("Context Load");
            settingDefaultToolStripMenuItem.LocalizeLabel("Context Set Default");
            checkUpdateToolStripMenuItem.LocalizeLabel("Context Update");
            ExitToolStripMenuItem.LocalizeLabel("Context Exit");
        }

        private void LocalizeBasicForm()
        {
            tpBasic.LocalizeLabel("Tab Basic");
            tpText.LocalizeLabel("Tab Text");
            tpExtra.LocalizeLabel("Tab Extra");
            tpTranslation.LocalizeLabel("Tab Translation");
            tpETC.LocalizeLabel("Tab ETC");
            tpQuickSetting.LocalizeLabel("Tab Quick Setting");
            tpDebuging.LocalizeLabel("Tab Debuging");

            donationButton.LocalizeLabel("Donate");
            acceptButton.LocalizeLabel("Apply Button");

            //기본설정 - 패널
            lbTransTypeTitle.LocalizeLabel("Panel Translate");
            lbAdjustImg.LocalizeLabel("Panel Adjust Img");

            //기본설정 - OCR
            showOcrCheckBox.LocalizeLabel("Show OCR Result");
            saveOCRCheckBox.LocalizeLabel("Save OCR Result");
            isClipBoardcheckBox1.LocalizeLabel("OCR Clipboard");

            OCR_Type_comboBox.LocalizeItems();

            //win ocr
            lbWinOCRLanguage.LocalizeLabel("Common Language");
            btnAddWinOcrLanguage.LocalizeLabel("Win OCR Add Language");

            //구글 OCR
            lbGoogleOCRLanguage.LocalizeLabel("Common Language");
            btnSettingGoogleOCR.LocalizeLabel("Google OCR API Setting");
            lbGoogleOcrStatus.LocalizeLabel("Google OCR Information");

            //NH OCR
            //lbOneOcrInfo.LocalizeLabel("NHOCR Info");

            //테저렉 OCR
            lbWinOCRLanguage.LocalizeLabel("Common Language");
            cbFastTess.LocalizeLabel("Fast Tesseract");
            lbTesseractLanguage.LocalizeLabel("Tesseract Language");
            tesseractLanguageComboBox.LocalizeItems();

            //Easy OCR
            lbEasyOcrLanguage.LocalizeLabel("Common Language");
            cbEasyOcrCode.LocalizeItems();
            btnInstallEasyOcr.LocalizeLabel("Easy OCR Install Button");

            //ONE OCR
            lbOneOcrLanguage.LocalizeLabel("Common Language");

            //번역
            lbTransType.LocalizeLabel("Translation Type");
            _cbTranslateType.LocalizeItems();


            lbBasicInfo.LocalizeLabel("Basic Translate Info");
            lbPapagoWebInfo.LocalizeLabel("Papago Web Translate Info");
            lbPapagoID.LocalizeLabel("Papago ID");
            lbPapagoSecret.LocalizeLabel("Papago Secret");
            Button_NaverTransKeyList.LocalizeLabel("Key Manage");
            lbGoogleSheetAddress.LocalizeLabel("Sheet Address");
            lbSheetID.LocalizeLabel("Sheet ID");
            lbSheetSecret.LocalizeLabel("Sheet Secret");
            button_RemoveAllGoogleToekn.LocalizeLabel("Clear Sheet Toekn");
            lbEzTransInfo.LocalizeLabel("EzTrans Info");
            lbDeepLInfo.LocalizeLabel("DeepL_Information");
            btnCheckDeeplState.LocalizeLabel("DeepL_CheckState");
            lbDeepLStatus.LocalizeLabel("DeepL_RequireApply");

            //deeplapi
            lbDeeplApi.LocalizeLabel("Translate API Key");

            rbDeepLAPIEndpointFree.LocalizeLabel("DeepLAPI Free");
            rbDeepLAPIEndpointPaid.LocalizeLabel("DeepLAPI Paid");

            lbDbFile.LocalizeLabel("Common File Name");
            checkStringUpper.LocalizeLabel("DB Check String Upper");
            cbDBMultiGet.LocalizeLabel("DB Multi Line");

            checkDic.LocalizeLabel("Use Dic");
            lbDicFile.LocalizeLabel("Common File Name");
            cbPerWordDic.LocalizeLabel("Use Per Word Dic");

            if (LocalizeManager.LocalizeManager.Language is not AppLanguage.Korea or AppLanguage.Auto)
            {
                _cbTranslateType.Anchor(lbTransType, 5);
                btnTransHelp.Anchor(_cbTranslateType, 5);

                lbDicFile.Anchor(checkDic, 5);
                dicFileTextBox.Anchor(lbDicFile, 5);
            }

            //Gemini API

            lbGeminiApi.LocalizeLabel("Translate API Key");
            lbGeminiModel.LocalizeLabel("Translate Model");

            //커스텀 api
            lbCustomApiInformation.LocalizeLabel("Custom Api Information");

            //크롬 번역기
            lbChromeBridgeInformation.LocalizeLabel("Chrome Bridge Information");
            btChromeBridgeRun.LocalizeLabel("Chrome Bridge Run");


            //이미지 보정
            checkRGB.LocalizeLabel("Extract by RGB");
            checkHSV.LocalizeLabel("Extract by HSV");
            cbThreshold.LocalizeLabel("Extract by Threshold");
            tbThreshold.Anchor(cbThreshold, 10, 151);

            checkErode.LocalizeLabel("Use Erode");
            btImgResult.LocalizeLabel("Preview Img Result");
            btImgResult.Anchor(checkErode, 10, 232);
            lbImgGroup.LocalizeLabel("Img Group");
            lbImgGroupCount.LocalizeLabel("Img Group Count");
            groupLabel.Anchor(lbImgGroupCount, 10);


            //폰트 설정
            lbFontSetting.LocalizeLabel("Font Setting");
            lbFont.LocalizeLabel("Label Font");
            fontButton.Anchor(lbFont, 10, 53);
            fontButton.LocalizeLabel("Font Setting");
            lbFontSize.LocalizeLabel("Label Font Size");
            fontSizeUpDown.Anchor(lbFontSize, 10, 63);

            lbFontColor.LocalizeLabel("Common Color");
            lbFontBasicColor.LocalizeLabel("Font Basic Color");
            lbFontOutlineColor1.LocalizeLabel("Font Outline Color 1");
            lbFontOutlineColor2.LocalizeLabel("Font Outline Color 2");
            lbFontBackground.LocalizeLabel("Font Background Color");
            defaultColorButton.LocalizeLabel("Set Default Color");

            //폰트 부가 설정
            lbTextAdditionalSettings.LocalizeLabel("Font Additional Seting");
            alignmentCenterCheckBox.LocalizeLabel("Font Center");
            removeSpaceCheckBox.LocalizeLabel("Font Remove Space");
            useBackColorCheckBox.LocalizeLabel("Font Use BackGround Color");
            cbShowOCRIndex.LocalizeLabel("Font Display Ocr Index");

            if(LocalizeManager.LocalizeManager.Language == AppLanguage.English)
            {
                removeSpaceCheckBox.Anchor(alignmentCenterCheckBox, 2);
                useBackColorCheckBox.Anchor(removeSpaceCheckBox, 2);
            }

            //폰트 미리보기
            lbPreview.LocalizeLabel("Common Preview");
            string first = LocalizeManager.LocalizeManager.GetLocalizeString("Label Font Preview1", "");
            string second = LocalizeManager.LocalizeManager.GetLocalizeString("Label Font Preview2", "");
            FormManager.InitCustomLabelString(first, second);

            //이미지 캡쳐
            lbImgCapture.LocalizeLabel("Img Capture");
            activeWinodeCheckBox.LocalizeLabel("Capture From Actvie Window");
            lbImgZoom.LocalizeLabel("Img Zoom");
            SetDefaultZoomSizeButton.LocalizeLabel("Common Default");
            btAttachCapture.LocalizeLabel("Capture From Attached Window");
            checkUpdateCheckBox.LocalizeLabel("Check Last Version");
            topMostcheckBox.LocalizeLabel("TopMost Trans Form");

            //처리속도
            lbSpeed.LocalizeLabel("Process Speed");
            speedRadioButton1.LocalizeLabel("Fastest");
            speedRadioButton2.LocalizeLabel("Fast");
            speedRadioButton3.LocalizeLabel("Normal");
            speedRadioButton4.LocalizeLabel("Slow");
            speedRadioButton5.LocalizeLabel("Slowest");
            lbSpeedInformation.LocalizeLabel("Process Speed Information");

            //설정 파일
            lbSettingFile.LocalizeLabel("Config File");
            openConfigButton.LocalizeLabel("Load Config File");
            saveConfigButton.LocalizeLabel("Save Config File");
            defaultButton.LocalizeLabel("Reset Config");

            //설정 검색
            lbSearchConfig.LocalizeLabel("Config Browser");
            btSettingBrowser.LocalizeLabel("Config Browser");
            btSettingUpload.LocalizeLabel("Upload Config");

            //고급 설정
            lbAdvencedConfig.LocalizeLabel("Advenced Config");
            btAdvencedOption.LocalizeLabel("Advenced Config");

            lbTransformType.LocalizeLabel("Transform Type");
            skinDarkRadioButton.LocalizeLabel("Transform Type Dark");
            skinLayerRadioButton.LocalizeLabel("Transform Type Layer");
            skinOverRadioButton.LocalizeLabel("Transform Type Overlay");

            //네이버 번역
            lbPaPago.LocalizeLabel("Papago Setting");
            lbPaPagoFrom.LocalizeLabel("Common From");
            lbPaPagoTo.LocalizeLabel("Common To");
            lbPapagoLanguageCodeInformation.LocalizeLabel("Papago Language Code Information");

            //구글 번역
            lbGoogle.LocalizeLabel("Google Setting");
            lbGoogleFrom.LocalizeLabel("Common From");
            lbGoogleTo.LocalizeLabel("Common To");

            //딥플 번역
            lbDeepL.LocalizeLabel("DeepL Setting");
            lbDeepLFrom.LocalizeLabel("Common From");
            lbDeepLTo.LocalizeLabel("Common To");

            //TTS
            cbUseTTS.LocalizeLabel("Use TTS");
            cbTTSWaitEnd.LocalizeLabel("Wait End of TTS");


            //단축키
            lbHotkey.LocalizeLabel("HotKey");
            lbHotKeyDoTrans.LocalizeLabel("HotKey Do Trans");
            lbHotKeyDic.LocalizeLabel("HotKey Dic");
            lbHotKeyQuickOCR.LocalizeLabel("HotKey Quick OCR");
            lbHotKeySnapShot.LocalizeLabel("HotKey Snap Shot");
            lbHotKeyOnceTranslate.LocalizeLabel("HotKey Once Translate");
            lbHotKeyHideTransWindow.LocalizeLabel("HotKey Hide Trnas Window");
            lbHotKeyFollowMouse.LocalizeLabel("HotKey OCR Area Follow Mouse");
            lbHotKeyInformation.LocalizeLabel("HotKey Information");
            transKeyInputResetButton.LocalizeLabel("Common Default");
            transKeyInputEmptyButton.LocalizeLabel("Common Clear");

            transKeyInputResetButton.LocalizeLabel("Common Default");
            transKeyInputEmptyButton.LocalizeLabel("Common Clear");

            dicKeyInputResetButton.LocalizeLabel("Common Default");
            dicKeyInputEmptyButton.LocalizeLabel("Common Clear");

            quickKeyInputResetButton.LocalizeLabel("Common Default");
            quickKeyInputEmptyButton.LocalizeLabel("Common Clear");

            snapShotKeyInputResetButton.LocalizeLabel("Common Default");
            snapShotKeyInputEmptyButton.LocalizeLabel("Common Clear");

            btnOneTransDefault.LocalizeLabel("Common Default");
            btnOneTransEmpty.LocalizeLabel("Common Clear");

            btnHideTransDefault.LocalizeLabel("Common Default");
            btnHideTransEmpty.LocalizeLabel("Common Clear");

            btnFollowMouseDefault.LocalizeLabel("Common Default");
            btnFollowMouseEmpty.LocalizeLabel("Common Clear");

            lbETC.LocalizeLabel("Common ETC");
            help_Button.LocalizeLabel("Mort Guide");
            error_Information_Button.LocalizeLabel("Error List");

            lbLink.LocalizeLabel("Link");
            btnGitHub.LocalizeLabel("Link Github");
            openBlogButton.LocalizeLabel("Link Blog");
            btnOpenDiscord.LocalizeLabel("Link Discord");

            //빠른설정
            lbQuickSetting.LocalizeLabel("Label Quick Setting");
            btQucickEnglish.LocalizeLabel("Quick Setting En");
            btQuickJap.LocalizeLabel("Quick Setting Jap");
            lbQuickSettingInformation.LocalizeLabel("Quick Setting Information");
            cbSetBasicDefaultPage.LocalizeLabel("Set Default Tab");

            //디버깅
            lbDebugging.LocalizeLabel("Label Debugging");
        }

        void IGoogleBasicTranslateAPIContract.UpdateCondition(string key)
        {
            lbBasicStatus.LocalizeLabel(key);
        }

        void IDeeplAPIContract.UpdateCondition(string key)
        {
            lbDeepLStatus.LocalizeLabel(key);
        }
    }

    public static class ComponenetLoclize
    {
        public static void LocalizeLabel(this System.Windows.Forms.Control control, string key)
        {
            if(control.InvokeRequired)
            {

                control.BeginInvoke(new Action(() => control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text)));
            }
            else
            {
                control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text);
            }
        }

        public static void LocalizeLabel(this ToolStripMenuItem item, string key)
        {
            item.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, item.Text);
        }

        public static void LocalizeItems(this System.Windows.Forms.ComboBox comboBox)
        {
            List<string> keys = new List<string>();
            System.Object[] results = new System.Object[comboBox.Items.Count];
            int index = 0;
            foreach (var obj in comboBox.Items)
            {
                string key = obj.ToString();
                string result = LocalizeManager.LocalizeManager.GetLocalizeString(key, key);
                results[index++] = result;
            }
            Action action = delegate ()
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(results);
            };

            if (comboBox.InvokeRequired)
            {
                comboBox.BeginInvoke(action);
            }
            else
            {
                action();
            }

        }

        public static void LocalizeLabel(this System.Windows.Forms.Control control)
        {
            if (control.InvokeRequired)
            {

                control.BeginInvoke(new Action(() => control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(control.Name, control.Text)));
            }
            else
            {
                control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(control.Name, control.Text);
            }
        }

        public static void LocalizeLabel(this System.Windows.Forms.Control control, System.Windows.Forms.Control target, string key, int left, int min = 0)
        {
            if (control.InvokeRequired)
            {

                control.BeginInvoke(new Action(() => control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text)));
            }
            else
            {
                control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text);
            }
            control.Anchor(target, left, min);
        }

        public static void Anchor(this System.Windows.Forms.Control control, System.Windows.Forms.Control target, int left, int min = 0)
        {
            int result = target.Location.X + target.Size.Width + left;

            if(result < min)
            {
                result = target.Location.X + min;
            }

            control.Location = new System.Drawing.Point(result, control.Location.Y);
        }
    }
}
