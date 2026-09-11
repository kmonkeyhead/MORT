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

        #region 크롬 번역기

        private Service.ChromeBridge.ChromeBridgeService? GetChromeBridgeService()
        {
            return Program.ServiceContainer?.GetService(typeof(Service.ChromeBridge.ChromeBridgeService))
                as Service.ChromeBridge.ChromeBridgeService;
        }

        private bool _chromeBridgeEventHooked;

        /// <summary>
        /// 로컬 서버를 닫고 브릿지 창도 정리한다. 서버가 사라진 창은 아무것도 못 하고 재접속만 되풀이한다.
        /// 종료 경로가 둘(CloseApplication / Form1_FormClosed)이라 양쪽에서 부르고, 두 번 불려도 문제없다.
        /// </summary>
        private void StopChromeBridge()
        {
            GetChromeBridgeService()?.Stop();
        }

        /// <summary>
        /// 브릿지 서버와 크롬 페이지는 따로 죽고 산다. 둘을 갈라서 보여 줘야 사용자가 무엇을 해야 하는지 안다.
        /// </summary>
        private void RefreshChromeBridgeStatus()
        {
            var service = GetChromeBridgeService();

            if(service != null && !_chromeBridgeEventHooked)
            {
                //연결 변화는 서버 스레드에서 올라오므로 UI 스레드로 넘겨서 라벨을 갱신한다.
                _chromeBridgeEventHooked = true;
                service.ConnectionChanged += OnChromeBridgeConnectionChanged;
                service.ModelMissing += OnChromeBridgeModelMissing;
            }

            if(service == null)
            {
                lbChromeBridgeStatus.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Chrome Bridge Status Down");
                return;
            }

            if(!service.IsServerRunning)
            {
                lbChromeBridgeStatus.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Chrome Bridge Status Down");
            }
            else if(!service.IsPageConnected)
            {
                lbChromeBridgeStatus.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Chrome Bridge Status Wait Page");
            }
            else if(service.NeedsModel)
            {
                //모델 내려받기는 크롬 창에서 직접 눌러야 시작된다. MORT가 대신 눌러 줄 수 없으니 그쪽을 보게 한다.
                lbChromeBridgeStatus.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Chrome Bridge Status Need Model");
            }
            else
            {
                lbChromeBridgeStatus.Text = LocalizeManager.LocalizeManager.GetLocalizeString("Chrome Bridge Status Ready");
            }
        }

        /// <summary>
        /// 쓸 모델이 없으면 번역을 멈추고 알린다.
        ///
        /// 그냥 두면 auto 모드가 조용히 LLM으로 떨어져 품질이 나쁜 결과가 계속 쌓인다.
        /// 내려받기는 크롬 창에서 직접 눌러야 시작되므로 여기서는 그쪽을 보라고만 할 수 있다.
        /// </summary>
        private void OnChromeBridgeModelMissing(string detail)
        {
            if(IsDisposed || !IsHandleCreated)
            {
                return;
            }

            try
            {
                BeginInvoke(new Action(() =>
                {
                    if(MySettingManager.NowTransType != SettingManager.TransType.chromeBridge)
                    {
                        return;
                    }

                    StopTrans();
                    RefreshChromeBridgeStatus();
                    Util.ShowLog($"[ChromeBridge] 모델이 없어 번역을 멈췄습니다 : {detail}");

                    //MORT 쪽 팝업은 두지 않는다. 물어보는 창은 브릿지 하나뿐이어야 한다.
                    //MORT에서 한 번, 브릿지에서 또 한 번 확인을 누르게 되고, 정작 받기 버튼은
                    //브릿지에만 있어서 MORT 팝업은 지나가는 관문 노릇밖에 못 했다.
                    //대신 그 창을 앞으로 꺼낸다. 최소화 예약이 걸려 있으면 같이 취소된다.
                    GetChromeBridgeService()?.FocusPage();
                }));
            }
            catch(Exception)
            {
                //창이 닫히는 중이면 무시한다.
            }
        }

        private void OnChromeBridgeConnectionChanged()
        {
            if(IsDisposed || !IsHandleCreated)
            {
                return;
            }

            try
            {
                BeginInvoke(new Action(() =>
                {
                    if(pnChromeBridge.Visible)
                    {
                        RefreshChromeBridgeStatus();
                    }
                }));
            }
            catch(Exception)
            {
                //창이 닫히는 중이면 무시한다.
            }
        }

        /// <summary>
        /// 크롬 번역기의 설정은 크롬 창 안의 브릿지 페이지에 있다. 엔진 선택과 모델 준비가 거기에서만 되기 때문이다.
        /// 모델 내려받기는 사용자가 버튼을 누른 직후에만 시작할 수 있어서 MORT 쪽 창으로는 대신해 줄 수 없다.
        /// </summary>
        private void btChromeBridgeRun_Click(object sender, EventArgs e)
        {
            var service = GetChromeBridgeService();

            if(service == null)
            {
                return;
            }

            service.OpenPage();
            RefreshChromeBridgeStatus();
        }

        /// <summary>

        #endregion

        #endregion
    }
}
