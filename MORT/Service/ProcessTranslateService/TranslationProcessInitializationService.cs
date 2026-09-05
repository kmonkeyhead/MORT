using MORT.Manager;
using MORT.Model;
using System;
using System.Drawing;
using System.Windows.Forms;
using static MORT.Manager.OcrManager;
using static MORT.SettingManager;

namespace MORT.Service.ProcessTranslateService
{
    internal sealed class TranslationProcessInitializationService
    {
        private readonly SettingManager _settingManager;

        public TranslationProcessInitializationService(SettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public TranslationProcessInitializationResult Initialize(OcrMethodType ocrMethodType)
        {
            ApplyOcrAreaWarning(ocrMethodType);

            bool isOnce = ocrMethodType != OcrMethodType.Normal;
            bool requireOriginalScreen = _settingManager.NowSkin == Skin.over
                && AdvencedOptionManager.OverlayAutoColor;
            bool useGoogleOcr = false;

            if(_settingManager.OCRType == OcrType.Google)
            {
                if(!isOnce)
                {
                    FormManager.Instace.ForceUpdateText(LocalizeString("Google Ocr Realtime Error"));
                    return new TranslationProcessInitializationResult(false, isOnce, false, requireOriginalScreen);
                }

                useGoogleOcr = true;
            }
            else if(isOnce && OcrManager.Instace.CheckGoogleOcrPriorty)
            {
                useGoogleOcr = true;
            }

            PrepareChromeBridge();
            PrepareTranslationWindow();
            return new TranslationProcessInitializationResult(true, isOnce, useGoogleOcr, requireOriginalScreen);
        }

        /// <summary>
        /// 크롬 창은 번역을 실제로 시작할 때만 띄운다. 설정 적용만 했는데 브라우저가 뜨면 성가시다.
        /// 이미 창이 붙어 있으면 다시 띄우지 않는다. 창이 둘이면 서로 밀어내기 때문이다.
        /// </summary>
        private void PrepareChromeBridge()
        {
            if(_settingManager.NowTransType != TransType.chromeBridge)
            {
                return;
            }

            var service = Program.ServiceContainer?.GetService(typeof(ChromeBridge.ChromeBridgeService))
                as ChromeBridge.ChromeBridgeService;

            if(service == null)
            {
                return;
            }

            string error;

            if(!service.Prepare(AdvencedOptionManager.ChromeBridgePort, AdvencedOptionManager.ChromeBridgeAutoRun, out error))
            {
                Util.ShowLog($"[ChromeBridge] 로컬 서버를 열지 못했습니다 : {error}");
            }
        }

        private void PrepareTranslationWindow()
        {
            ITransform transForm = FormManager.Instace.GetITransform();
            if(transForm == null)
            {
                return;
            }

            void PrepareAndStart()
            {
                transForm.Prepare();
                transForm.StartTrans();
            }

            if(transForm is Form form && form.InvokeRequired)
            {
                form.Invoke((Action)PrepareAndStart);
            }
            else
            {
                PrepareAndStart();
            }
        }

        private void ApplyOcrAreaWarning(OcrMethodType ocrMethodType)
        {
            if(!RequiresOcrAreaWarning(ocrMethodType))
            {
                FormManager.Instace.ClearWarningMessage();
                return;
            }

            string message = LocalizeString("Ocar Area Location Warning") + Environment.NewLine;
            FormManager.Instace.ApplyWarningMessage(message);
        }

        private bool RequiresOcrAreaWarning(OcrMethodType ocrMethodType)
        {
            if(ocrMethodType != OcrMethodType.Normal
                || _settingManager.IsUseAttachedCapture
                || _settingManager.NowIsActiveWindow)
            {
                return false;
            }

            if(_settingManager.NowSkin != Skin.layer && _settingManager.NowSkin != Skin.dark)
            {
                return false;
            }

            if(FormManager.Instace.GetITransform() is not Form transform)
            {
                return false;
            }

            Rectangle formRectangle = transform.Bounds;
            for(int index = 0; index < _settingManager.NowOCRGroupcount; index++)
            {
                Rectangle ocrRectangle = new Rectangle(
                    _settingManager.NowLocationXList[index],
                    _settingManager.NowLocationYList[index],
                    _settingManager.NowSizeXList[index],
                    _settingManager.NowSizeYList[index]);
                if(Rectangle.Intersect(formRectangle, ocrRectangle) != Rectangle.Empty)
                {
                    return true;
                }
            }

            return false;
        }

        private static string LocalizeString(string key)
        {
            return LocalizeManager.LocalizeManager.GetLocalizeString(key).Replace("[]", "");
        }
    }
}
