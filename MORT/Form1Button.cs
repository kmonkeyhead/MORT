using MORT.LocalizeManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace MORT
{
    public partial class Form1
    {
        private void OnClick_btnTransHelp(object sender, EventArgs e)
        {
            Util.OpenURL("https://blog.naver.com/killkimno/221760617100");
        }

        private void OnClick_btOcrHelp(object sender, EventArgs e)
        {
            Util.OpenURL("https://blog.naver.com/killkimno/221908677254");
        }

        private void OnClick_GitHub(object sender, EventArgs e)
        {
            Util.OpenURL("https://github.com/kmonkeyhead/MORT");

        }
        private void OnClickopenBlog(object sender, EventArgs e)
        {
            Util.OpenURL("https://blog.naver.com/killkimno/70179867557");
        }

        private void OnClickOpenDiscord(object sender, EventArgs e)
        {
            switch (LocalizeManager.LocalizeManager.Language)
            {
                case AppLanguage.Auto:
                case AppLanguage.Korea:
                    Util.OpenURL("https://discord.gg/ha5yNy9");
                    break;

                default:
                    Util.OpenURL("https://discord.gg/buqVV32wBV");

                    break;
            }
        }

        private void OnClickShowImgResult(object sender, EventArgs e)
        {
            bool isHsv = checkHSV.Checked;
            bool isRgb = checkRGB.Checked;
            bool isThreshold = cbThreshold.Checked;

            int index = groupCombo.Items.Count - 3;
            int threshold = 127;

            Int32.TryParse(tbThreshold.Text, out threshold);

            List<ColorGroup> list = FormManager.Instace.MyMainForm.MySettingManager.NowColorGroup;
            ColorGroup color = null;
            if (list.Count > 0 && list.Count > index)
            {

                color = list[index];
            }
            else
            {
                color = new ColorGroup();
            }

            FormManager.Instace.ShowColorPickResult(isRgb, isHsv, isThreshold, color, threshold);
        }

        private void OnClick_btGoogleOcrSetting(object sender, EventArgs e)
        {
            FormManager.Instace.ShowGoogleOcrSetting();
        }

        #region ::::::::: 빠른 설정 ::::::::::

        private void OnClickQucickEnglish(object sender, EventArgs e)
        {
            FormManager.Instace.ShowQuickSetting(OcrLanguageType.English);
        }

        private void OnClickQuickJap(object sender, EventArgs e)
        {
            FormManager.Instace.ShowQuickSetting(OcrLanguageType.Japen);
        }

        #endregion


        #region ::::::::: 디버그 처리 ::::::::::

        private void OnClick_DebugOn(object sender, EventArgs e)
        {
            MySettingManager.isDebugMode = true;
            plDebugOff.Visible = false;
            plDebugOn.Visible = true;

        }

        /// <summary>
        /// OCR 속도 언락
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbUnlockSpeed_CheckedChanged(object sender, EventArgs e)
        {
            if(MySettingManager.isDebugMode)
            {
                _processTranslateService.DebugUnlockOCRSpeed = cbUnlockSpeed.Checked;
            }
        }

        private void cbShowFormerLog_CheckedChanged(object sender, EventArgs e)
        {
            if (MySettingManager.isDebugMode)
            {
                IsDebugShowFormerResultLog = cbShowFormerLog.Checked;
            }
        }

        private void btClearFormerResult_Click(object sender, EventArgs e)
        {
            if(!TransManager.isSaving)
            {
                TransManager.Instace.ClearFormerDic();
            }
        }

        private void cbSetLineTrans_CheckedChanged(object sender, EventArgs e)
        {
            if (MySettingManager.isDebugMode)
            {
                IsDebugTransOneLine = cbSetLineTrans.Checked;
            }
        }

        private void cbShowOverlayWordArea_CheckedChanged(object sender, EventArgs e)
        {
            if (MySettingManager.isDebugMode)
            {
                IsDebugShowWordArea = cbShowOverlayWordArea.Checked;
            }
        }

        private void cbSaveAnalysisResult_CheckedChanged(object sender, EventArgs e)
        {
            if (MySettingManager.isDebugMode)
            {
                IsDebugSaveAnalysisResult = cbSaveAnalysisResult.Checked;
            }
        }

        private void cbDisplayLowQuality_CheckedChanged(object sender, EventArgs e)
        {
            if (MySettingManager.isDebugMode)
            {
                IsDebugDisplayLowQuality = cbDisplayLowQuality.Checked;
            }
        }

        #endregion


        private void OnClick_btAdvencedOption(object sender, EventArgs e)
        {
            FormManager.Instace.ShowAdvencedOption();
        }

        private void OnClick_btAttachCapture(object sender, EventArgs e)
        {
            if (!isAvailableWinOCR)
            {
                MessageBox.Show("윈도우 10이 아니거나 다른 문제가 발생했습니다.\n에러명 :" + winOcrErrorCode);
                return;
            }

            FormManager.Instace.ShowScreenCapture();
        }
        private void Onclick_btSettingBrowser(object sender, EventArgs e)
        {
            FormManager.Instace.ShowSettingBrowserUI();
        }

        private void OnClick_btSettingUpload(object sender, EventArgs e)
        {
            string data = null;
            try
            {
                var reader = Util.OpenFile(GlobalDefine.USER_SETTING_FILE);

                data = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {

            }
         

            if(!string.IsNullOrEmpty(data))
            {
                try
                {
                    Clipboard.SetText(data);
                    string message = "현재 설정을 클립보드에 저장했습니다." + System.Environment.NewLine + "5번 항목에 클립보드 값을 ctrl+v로 붙여 넣으면 됩니다"
                                    + System.Environment.NewLine + System.Environment.NewLine + "확인을 누르면 업로드 페이지로 이동합니다";

                    //https://docs.google.com/forms/d/e/1FAIpQLSeHTcOQ_W_NXbt3lwf-osmLT_F0E1JwdTJjP7xYMGm_f41AEQ/viewform?usp=sf_link

                    if (DialogResult.OK == MessageBox.Show(message, "MORT", MessageBoxButtons.OK))
                    {
                        Util.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSeHTcOQ_W_NXbt3lwf-osmLT_F0E1JwdTJjP7xYMGm_f41AEQ/viewform?usp=sf_link");
                    }
                  
                }
                catch
                {

                }
            }
        }

        private void OnClickCheckDeeplState(object sender, EventArgs e)
        {
            TransManager.Instace.ShowDeeplWebView();
        }


        #region :::::::::: 체크 박스 ::::::::::

        private void cbUseTTS_CheckedChanged(object sender, EventArgs e)
        {
            if(cbUseTTS.Checked)
            {
            }
            else
            {
            }
        }


        #endregion


        #region :::::::::: 단축키 ::::::::::     


        private void transKeyInputResetButton_Click(object sender, EventArgs e)
        {
            InitTansKey();
        }

        private void dicKeyInputResetButton_Click(object sender, EventArgs e)
        {
            InitDicKey();
        }

        private void quickKeyInputResetButton_Click(object sender, EventArgs e)
        {
            InitQuickKey();
        }

        private void snapShotKeyInputResetButton_Click(object sender, EventArgs e)
        {
            InitSnapShotKey();
        }

        private void btnOneTransDefault_Click(object sender, EventArgs e)
        {
            InitOneTranslateKey();
        }


        private void btnHideTransDefault_Click(object sender, EventArgs e)
        {
            InitHideTransKey();
        }

        private void btnFollowMouseDefault_Click(object sender, EventArgs e)
        {
            InitFollowMouseKey();
        }

        private void transKeyInputEmptyButton_Click(object sender, EventArgs e)
        {
            SetEmptyTansKey();
        }

        private void dicKeyInputEmptyButton_Click(object sender, EventArgs e)
        {
            SetEmptyDicKey();
        }

        private void quickKeyInputEmptyButton_Click(object sender, EventArgs e)
        {
            SetEmptyQuickKey();
        }

        private void snapShotKeyInputEmptyButton_Click(object sender, EventArgs e)
        {
            SetEmptySnapShotKey();
        }

        private void btnOneTransEmpty_Click(object sender, EventArgs e)
        {
            SetEmptyOneTranslate();
        }

        private void btnHideTransEmpty_Click(object sender, EventArgs e)
        {
            SetEmptyHideTranslate();
        }

        private void btnFollowMouseEmpty_Click(object sender, EventArgs e)
        {
            SetEmptyFollowMouse();
        }


        #endregion
    }
}
