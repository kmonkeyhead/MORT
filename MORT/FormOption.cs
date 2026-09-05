using MORT.Manager;
using MORT.SettingData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using static MORT.AdvencedOptionManager;
using static MORT.SettingManager;

//UI 옵션 등을 저장처리
namespace MORT
{

    public partial class Form1
    {
        //Setting 메니져에 저장된 값을 기본 셋팅에 적용함.
        private void SetValueToUIValue()
        {
            if(MySettingManager.NowSkin == SettingManager.Skin.dark)
            {
                skinDarkRadioButton.Checked = true;
            }
            else if(MySettingManager.NowSkin == SettingManager.Skin.layer)
            {
                skinLayerRadioButton.Checked = true;
            }
            else if(MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                skinOverRadioButton.Checked = true;
            }
            showOcrCheckBox.Checked = MySettingManager.NowIsShowOcrResultFlag;
            saveOCRCheckBox.Checked = MySettingManager.NowIsSaveOcrReulstFlag;
            isClipBoardcheckBox1.Checked = MySettingManager.NowIsSaveInClipboardFlag;

            if((int)MySettingManager.OCRType >= OCR_Type_comboBox.Items.Count)
            {
                OCR_Type_comboBox.SelectedIndex = 0;
            }
            else
            {
                OCR_Type_comboBox.SelectedIndex = (int)MySettingManager.OCRType;
            }

            switch(MySettingManager.nowDeepLAPIEndpointType)
            {
                case DeepLAPIEndpointType.Free:
                    rbDeepLAPIEndpointFree.Checked = true;
                    break;
                case DeepLAPIEndpointType.Paid:
                    rbDeepLAPIEndpointPaid.Checked = true;
                    break;
            }


            checkStringUpper.Checked = MySettingManager.IsUseStringUpper;
            checkRGB.Checked = MySettingManager.NowIsUseRGBFlag;
            checkHSV.Checked = MySettingManager.NowIsUseHSVFlag;
            checkErode.Checked = MySettingManager.NowIsUseErodeFlag;
            cbThreshold.Checked = MySettingManager.isUseThreshold;


            switch(MySettingManager.NowOCRSpeed)
            {
                case 1:
                    speedRadioButton1.Checked = true;
                    break;

                case 2:
                    speedRadioButton2.Checked = true;
                    break;

                case 3:
                    speedRadioButton3.Checked = true;
                    break;

                case 4:
                    speedRadioButton4.Checked = true;
                    break;

                case 5:
                    speedRadioButton5.Checked = true;
                    break;

                default:
                    speedRadioButton3.Checked = true;
                    break;

            }

            dbFileTextBox.Text = MySettingManager.NowDBFile;
            tessDataTextBox.Text = MySettingManager.NowTessData;
            cbFastTess.Checked = MySettingManager.nowIsFastTess;
            cbDBMultiGet.Checked = MySettingManager.nowIsUsePartialDB;
            dicFileTextBox.Text = MySettingManager.NowDicFile;

            checkDic.Checked = MySettingManager.NowIsUseDicFileFlag;
            cbPerWordDic.Checked = MySettingManager.isUseMatchWordDic;
            setCheckSpellingToolStripMenuItem.Checked = MySettingManager.NowIsUseDicFileFlag;

            //TTS 설정
            cbUseTTS.Checked = MySettingManager.IsUseTTS;
            cbTTSWaitEnd.Checked = MySettingManager.IsWaitTTSEnd;

            //언어 설정.
            if(MySettingManager.NowIsUseEngFlag)
            {
                tesseractLanguageComboBox.SelectedIndex = 0;
            }
            else if(MySettingManager.NowIsUseJpnFlag)
            {
                tesseractLanguageComboBox.SelectedIndex = 1;
            }
            else if(MySettingManager.NowIsUseOtherLangFlag)
            {
                tesseractLanguageComboBox.SelectedIndex = 2;
            }

            SetTransLangugage(MySettingManager.WindowLanguageCode);
            SetTranslatorUIValue();

            //윈도우 10 관련.
            if(isAvailableWinOCR)
            {
                //OCR을 찾았나 못 찾았나.
                //영어는 en 또는 en-us일 수 있다
                bool isFound = false;
                for(int i = 0; i < winLanguageCodeList.Count; i++)
                {
                    if(Util.GetIsEqualMainOcrCode(winLanguageCodeList[i], MySettingManager.WindowLanguageCode))
                    {
                        if(WinOCR_Language_comboBox.Items.Count > i)
                        {
                            isFound = true;
                            WinOCR_Language_comboBox.SelectedIndex = i;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if(!isFound && WinOCR_Language_comboBox.Items.Count > 0)
                {
                    WinOCR_Language_comboBox.SelectedIndex = 0;
                }
            }

            //Easy OCR 관련

            int easyOcrCodeIndex = OcrManager.Instace.EasyOcrCodeList.IndexOf(MySettingManager.EasyOcrCode);
            if(easyOcrCodeIndex != -1 && cbEasyOcrCode.Items.Count > easyOcrCodeIndex)
            {
                cbEasyOcrCode.SelectedIndex = easyOcrCodeIndex;
            }

            // 메인 OCR 언어
            if(MySettingManager.OcrLanguageType == OcrLanguageType.None)
            {
                _currentOcrLanguage = OcrLanguageType.Other;

            }
            else
            {
                _currentOcrLanguage = MySettingManager.OcrLanguageType;
            }
            int index = _ocrLanguages.IndexOf(_currentOcrLanguage);
            cbOneOcrLanguage.SelectedIndex = index;

            //색 관련 처리
            InitColorGroup();

            colorGroup = MySettingManager.NowColorGroup;
            if(colorGroup.Count != 0)
            {
                rTextBox.Text = colorGroup[0].getValueR().ToString();
                gTextBox.Text = colorGroup[0].getValueG().ToString();
                bTextBox.Text = colorGroup[0].getValueB().ToString();

                v1TextBox.Text = colorGroup[0].getValueV1().ToString();
                v2TextBox.Text = colorGroup[0].getValueV2().ToString();
                s1TextBox.Text = colorGroup[0].getValueS1().ToString();
                s2TextBox.Text = colorGroup[0].getValueS2().ToString();

                for(int i = 2; i <= MySettingManager.NowColorGroupCount; i++)
                {
                    groupCombo.Items.Add(i);
                }
            }

            tbThreshold.Text = MySettingManager.thresholdValue.ToString();

            _locationXList = MySettingManager.NowLocationXList;
            _locationYList = MySettingManager.NowLocationYList;
            _sizeXList = MySettingManager.NowSizeXList;
            _sizeYList = MySettingManager.NowSizeYList;



            _exceptionLocationXList = MySettingManager.nowExceptionLocationXList;
            _exceptionLocationYList = MySettingManager.nowExceptionLocationYList;
            _exceptionSizeXList = MySettingManager.nowExceptionSizeXList;
            _exceptionSizeYList = MySettingManager.nowExceptionSizeYList;

            textFont = MySettingManager.TextFont;
            textColor = MySettingManager.TextColor;
            outlineColor1 = MySettingManager.OutLineColor1;
            outlineColor2 = MySettingManager.OutLineColor2;
            backgroundColor = MySettingManager.BackgroundColor;

            if(MySettingManager.NowSortType == SettingManager.SortType.Center)
                alignmentCenterCheckBox.Checked = true;
            else
                alignmentCenterCheckBox.Checked = false;

            useBackColorCheckBox.Checked = MySettingManager.NowIsUseBackColor;
            removeSpaceCheckBox.Checked = MySettingManager.NowIsRemoveSpace;
            cbShowOCRIndex.Checked = MySettingManager.IsShowOCRIndex;

            //엑티브 윈도우
            activeWinodeCheckBox.Checked = MySettingManager.NowIsActiveWindow;

            //업데이트 확인
            checkUpdateCheckBox.Checked = _versionCheckLogic.GetCheckUpdate();

            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@SET_BASIC_DEFAULT_PAGE ");
            bool isDefaultPage = false;
            if(Boolean.TryParse(result, out isDefaultPage))
            {
                cbSetBasicDefaultPage.Checked = isDefaultPage;
            }
            else
            {
                cbSetBasicDefaultPage.Checked = false;
            }

            result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@USE_TOP_MOST ");

            if(!bool.TryParse(result, out isTranslateFormTopMostFlag))
            {
                isTranslateFormTopMostFlag = true;
            }


            topMostcheckBox.Checked = isTranslateFormTopMostFlag;
            setTranslateTopMostToolStripMenuItem.Checked = isTranslateFormTopMostFlag;

            MySettingManager.TextColor = Color.FromArgb(15, 15, 15);
            fontButton.Text = textFont.FontFamily.Name;
            fontSizeUpDown.Value = (int)textFont.Size;
            SetColorBoxColor(textColorBox, textColor);
            SetColorBoxColor(outlineColor1Box, outlineColor1);
            SetColorBoxColor(outlineColor2Box, outlineColor2);
            SetColorBoxColor(backgroundColorBox, backgroundColor);

            imgZoomsizeUpDown.Value = (decimal)MySettingManager.ImgZoomSize;


            FormManager.Instace.ResetCaputreAreaForm();
        }

        /// <summary>
        /// 번역 설정을 ui 에 적용
        /// </summary>
        private void SetTranslatorUIValue()
        {
            var translateType = MySettingManager.NowTransType;
            string subTranslateTypeKey = MySettingManager.TranslateTypeSubKey;


            var translateTypeModel = _translateTypListService.GetModel(MySettingManager.NowTransType, MySettingManager.TranslateTypeSubKey);
            _cbTranslateType.SelectedIndex = translateTypeModel.Index;
            //네이버.
            bool naverFound = false;
            foreach(var obj in naverTransComboBox.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if(MySettingManager.NaverTransCode == data.naverCode)
                {
                    naverFound = true;
                    naverTransComboBox.SelectedItem = obj;
                    break;
                }
            }

            if(!naverFound)
            {
                naverTransComboBox.SelectedItem = naverTransComboBox.Items[0];
                var data = (TransManager.TransCodeData)((ComboboxItem)naverTransComboBox.SelectedItem).Value;
                MySettingManager.NaverTransCode = data.naverCode;
            }

            naverFound = false;
            //네이버 번역기
            foreach(var obj in cbNaverResultCode.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if(MySettingManager.NaverResultCode == data.naverCode)
                {
                    naverFound = true;
                    cbNaverResultCode.SelectedItem = obj;
                    break;
                }
            }

            if(!naverFound)
            {
                cbNaverResultCode.SelectedItem = cbNaverResultCode.Items[0];
                var data = (TransManager.TransCodeData)((ComboboxItem)cbNaverResultCode.SelectedItem).Value;
                MySettingManager.NaverTransCode = data.naverCode;
            }

            bool foundCode = false;

            //구글.
            foreach(var obj in googleTransComboBox.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if(MySettingManager.GoogleTransCode == data.googleCode)
                {
                    googleTransComboBox.SelectedItem = obj;
                    foundCode = true;
                    break;
                }
            }

            //디플
            foreach(var obj in cbDeepLLanguage.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if(MySettingManager.DeepLTransCode == data.DeepLCode)
                {
                    cbDeepLLanguage.SelectedItem = obj;
                    foundCode = true;
                    break;
                }
            }

            if(!foundCode)
            {
                googleTransComboBox.SelectedIndex = 0;
            }
            foundCode = false;

            //구글 번역기.
            foreach(var obj in googleResultCodeComboBox.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if(MySettingManager.GoogleResultCode == data.googleCode)
                {
                    foundCode = true;
                    googleResultCodeComboBox.SelectedItem = obj;
                    break;
                }
            }

            //디플 번역기.
            foreach(var obj in cbDeepLLanguageTo.Items)
            {
                TransManager.TransCodeData data = (TransManager.TransCodeData)((ComboboxItem)obj).Value;
                if(MySettingManager.DeepLResultCode == data.DeepLCode)
                {
                    foundCode = true;
                    cbDeepLLanguageTo.SelectedItem = obj;
                    break;
                }
            }

            if(!foundCode)
            {
                googleResultCodeComboBox.SelectedIndex = 0;
            }
        }


        //설정 파일로 저장.
        private void SaveSetting(string fileName)
        {
            MySettingManager.OcrLanguageType = _currentOcrLanguage;
            MySettingManager.NowColorGroupCount = groupCombo.Items.Count - 2;
            MySettingManager.NowColorGroup = colorGroup;
            MySettingManager.NowOCRGroupcount = _locationXList.Count;
            MySettingManager.NowLocationXList = _locationXList;
            MySettingManager.NowLocationYList = _locationYList;
            MySettingManager.NowSizeXList = _sizeXList;
            MySettingManager.NowSizeYList = _sizeYList;

            //제외 영역
            MySettingManager.nowExceptionGroupCount = _exceptionLocationXList.Count;
            MySettingManager.nowExceptionLocationXList = _exceptionLocationXList;
            MySettingManager.nowExceptionLocationYList = _exceptionLocationYList;
            MySettingManager.nowExceptionSizeXList = _exceptionSizeXList;
            MySettingManager.nowExceptionSizeYList = _exceptionSizeYList;

            //번역창 위치 설정 - 디폴트는 모두 없애고 초기화 땐 저장을 안 한다.
            if(_currentState == CurrentStateType.SetDefault)
            {
                MySettingManager.transFormLocationX = -1;
                MySettingManager.transFormLocationY = -1;

                MySettingManager.transFormSizeX = -1;
                MySettingManager.transFormSizeY = -1;
            }
            else if(_currentState != CurrentStateType.Init)
            {
                if(FormManager.Instace.MyLayerTransForm != null)
                {
                    TransFormLayer transForm = FormManager.Instace.MyLayerTransForm;
                    MySettingManager.transFormLocationX = transForm.Location.X;
                    MySettingManager.transFormLocationY = transForm.Location.Y;

                    MySettingManager.transFormSizeX = transForm.Size.Width;
                    MySettingManager.transFormSizeY = transForm.Size.Height;

                }

            }


            MySettingManager.saveSetting(fileName);

        }



        //UI 값을 설정 매니저에 저장 -> 실제로 적용
        public void ApplyUIValueToSetting()
        {
            try
            {
                ChangeSkin();
                MySettingManager.NowIsShowOcrResultFlag = showOcrCheckBox.Checked;
                MySettingManager.NowIsSaveOcrReulstFlag = saveOCRCheckBox.Checked;
                IsUseClipBoardFlag = isClipBoardcheckBox1.Checked;

                var translateTypeModel = _translateTypListService.GetModel(_cbTranslateType.SelectedIndex);

                if(translateTypeModel == null)
                {
                    MySettingManager.NowTransType = SettingManager.TransType.google_url;
                }
                else
                {
                    MySettingManager.NowTransType = translateTypeModel.TransType;

                    if(translateTypeModel.TransType == TransType.customApi)
                    {
                        MySettingManager.TranslateTypeSubKey = translateTypeModel.Key;
                    }
                    else
                    {
                        MySettingManager.TranslateTypeSubKey = "";
                    }
                }

                if(rbDeepLAPIEndpointFree.Checked == true)
                {
                    MySettingManager.nowDeepLAPIEndpointType = DeepLAPIEndpointType.Free;
                }
                else if(rbDeepLAPIEndpointPaid.Checked == true)
                {
                    MySettingManager.nowDeepLAPIEndpointType = DeepLAPIEndpointType.Paid;
                }



                MySettingManager.IsUseStringUpper = checkStringUpper.Checked;
                MySettingManager.NowIsUseRGBFlag = checkRGB.Checked;
                MySettingManager.NowIsUseHSVFlag = checkHSV.Checked;
                MySettingManager.NowIsUseErodeFlag = checkErode.Checked;
                MySettingManager.isUseThreshold = cbThreshold.Checked;

                if(speedRadioButton1.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 1;
                    _ocrProcessSpeed = 300;
                }
                else if(speedRadioButton2.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 2;
                    _ocrProcessSpeed = 1000;
                }
                else if(speedRadioButton3.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 3;
                    _ocrProcessSpeed = 1500;
                }
                else if(speedRadioButton4.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 4;
                    _ocrProcessSpeed = 2000;
                }
                else if(speedRadioButton5.Checked == true)
                {
                    MySettingManager.NowOCRSpeed = 5;
                    _ocrProcessSpeed = 2500;
                }

                MySettingManager.NowDBFile = dbFileTextBox.Text;
                MySettingManager.NowTessData = tessDataTextBox.Text;
                MySettingManager.nowIsUsePartialDB = cbDBMultiGet.Checked;
                MySettingManager.nowIsFastTess = cbFastTess.Checked;

                SaveHotKeyFile();

                _versionCheckLogic.SetCheckUpdate(checkUpdateCheckBox.Checked);

                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@SET_BASIC_DEFAULT_PAGE ", cbSetBasicDefaultPage.Checked.ToString());

                //OCR 설정.
                MySettingManager.OCRType = SettingManager.GetOcrType(OCR_Type_comboBox.SelectedIndex);

                //번역 코드 설정.
                TransManager.TransCodeData codeData = (TransManager.TransCodeData)((ComboboxItem)naverTransComboBox.SelectedItem).Value;
                MySettingManager.NaverTransCode = codeData.naverCode;

                codeData = (TransManager.TransCodeData)((ComboboxItem)cbNaverResultCode.SelectedItem).Value;
                MySettingManager.NaverResultCode = codeData.naverCode;

                codeData = (TransManager.TransCodeData)((ComboboxItem)googleTransComboBox.SelectedItem).Value;
                MySettingManager.GoogleTransCode = codeData.googleCode;

                codeData = (TransManager.TransCodeData)((ComboboxItem)googleResultCodeComboBox.SelectedItem).Value;
                MySettingManager.GoogleResultCode = codeData.googleCode;

                codeData = (TransManager.TransCodeData)((ComboboxItem)cbDeepLLanguage.SelectedItem).Value;
                MySettingManager.DeepLTransCode = codeData.DeepLCode;

                codeData = (TransManager.TransCodeData)((ComboboxItem)cbDeepLLanguageTo.SelectedItem).Value;
                MySettingManager.DeepLResultCode = codeData.DeepLCode;


                NaverTranslateAPI.instance.SetTransCode(MySettingManager.NaverTransCode, MySettingManager.NaverResultCode);
                GoogleBasicTranslateAPI.instance.SetTransCode(MySettingManager.GoogleTransCode, MySettingManager.GoogleResultCode);

                //윈도우 10 OCR 관련.
                if(isAvailableWinOCR && winLanguageCodeList.Count > WinOCR_Language_comboBox.SelectedIndex)
                {
                    MySettingManager.WindowLanguageCode = winLanguageCodeList[WinOCR_Language_comboBox.SelectedIndex];
                    loader.InitOcr(MySettingManager.WindowLanguageCode);
                }
                else
                {
                    MySettingManager.WindowLanguageCode = "";
                }

                //Easy Ocr 관련
                MySettingManager.EasyOcrCode = OcrManager.Instace.ConvertToEasyOcrCode(cbEasyOcrCode.SelectedIndex);

                //TTS 설정
                MySettingManager.IsUseTTS = cbUseTTS.Checked;
                MySettingManager.IsWaitTTSEnd = cbTTSWaitEnd.Checked;

                //언어 설정.
                MySettingManager.NowIsUseEngFlag = false;
                MySettingManager.NowIsUseJpnFlag = false;
                MySettingManager.NowIsUseOtherLangFlag = false;

                if(MySettingManager.OCRType == SettingManager.OcrType.Tesseract || MySettingManager.OCRType == SettingManager.OcrType.OneOcr
                    || MySettingManager.OCRType == SettingManager.OcrType.Google)
                {
                    if(tesseractLanguageComboBox.SelectedIndex == 0)
                    {
                        //영어.
                        MySettingManager.NowIsUseEngFlag = true;
                    }
                    else if(tesseractLanguageComboBox.SelectedIndex == 1)
                    {
                        //일본어
                        MySettingManager.NowIsUseJpnFlag = true;

                    }
                    else if(tesseractLanguageComboBox.SelectedIndex == 2)
                    {
                        //기타
                        MySettingManager.NowIsUseOtherLangFlag = true;
                    }
                }
                else if(MySettingManager.OCRType == SettingManager.OcrType.Window && isAvailableWinOCR)
                {
                    string selectCode = winLanguageCodeList[WinOCR_Language_comboBox.SelectedIndex];
                    if(selectCode == "en" || selectCode == "en-US")
                    {
                        MySettingManager.NowIsUseEngFlag = true;
                    }
                    else if(selectCode == "ja")
                    {
                        MySettingManager.NowIsUseJpnFlag = true;
                    }
                    else
                    {
                        MySettingManager.NowIsUseOtherLangFlag = true;
                    }
                }
                else if(MySettingManager.OCRType == SettingManager.OcrType.EasyOcr)
                {
                    string selectCode = OcrManager.Instace.EasyOcrCodeList[cbEasyOcrCode.SelectedIndex];
                    if(selectCode == "en" || selectCode == "en-US")
                    {
                        MySettingManager.NowIsUseEngFlag = true;
                    }
                    else if(selectCode == "ja")
                    {
                        MySettingManager.NowIsUseJpnFlag = true;
                    }
                    else
                    {
                        MySettingManager.NowIsUseOtherLangFlag = true;
                    }
                }

                //폰트 관련
                textFont = new Font(textFont.FontFamily, (int)fontSizeUpDown.Value);
                MySettingManager.TextFont = textFont;
                MySettingManager.TextColor = textColor;
                MySettingManager.OutLineColor1 = outlineColor1;
                MySettingManager.OutLineColor2 = outlineColor2;
                MySettingManager.BackgroundColor = backgroundColor;

                MySettingManager.NowIsUseBackColor = this.useBackColorCheckBox.Checked;
                MySettingManager.IsShowOCRIndex = this.cbShowOCRIndex.Checked;

                if(this.alignmentCenterCheckBox.Checked)
                    MySettingManager.NowSortType = SettingManager.SortType.Center;
                else
                    MySettingManager.NowSortType = SettingManager.SortType.Normal;
                MySettingManager.NowIsRemoveSpace = this.removeSpaceCheckBox.Checked;

                MySettingManager.NowIsActiveWindow = activeWinodeCheckBox.Checked;


                //번역창 최상단
                isTranslateFormTopMostFlag = topMostcheckBox.Checked;
                setTranslateTopMostToolStripMenuItem.Checked = topMostcheckBox.Checked;

                Util.ChangeFileData(GlobalDefine.USER_OPTION_SETTING_FILE, "@USE_TOP_MOST ", isTranslateFormTopMostFlag.ToString());

                //Util.ShowLog("Bing : " + transCode.ToString() + " Naver : " + MySettingManager.NaverTransCode);
                if(MySettingManager.NowSkin == SettingManager.Skin.dark)
                {
                    //더이상 이곳에서 번역키 설정 안 함
                }
                else if(MySettingManager.NowSkin == SettingManager.Skin.layer)
                {
                    FormManager.Instace.MyLayerTransForm.UpdateTransform();
                }
                else if(MySettingManager.NowSkin == SettingManager.Skin.over)
                {
                    FormManager.Instace.MyOverTransForm.UpdateTransform();
                }

                ApplyTransSetting();

                MySettingManager.ImgZoomSize = (float)imgZoomsizeUpDown.Value;


            }
            catch(Exception e)
            {
                Util.ShowLog(e.Message + System.Environment.NewLine + e.StackTrace);
                MessageBox.Show(e.Message + System.Environment.NewLine + e.StackTrace);
            }

            //색 리스트
            int[] valueRArray = new int[groupCombo.Items.Count];
            int[] valueGArray = new int[groupCombo.Items.Count];
            int[] valueBArray = new int[groupCombo.Items.Count];

            int[] valueS1Array = new int[groupCombo.Items.Count];
            int[] valueS2Array = new int[groupCombo.Items.Count];
            int[] valueV1Array = new int[groupCombo.Items.Count];
            int[] valueV2Array = new int[groupCombo.Items.Count];

            colorGroup[nowColorGroupIndex].setRGBValuse(Convert.ToInt32(rTextBox.Text), Convert.ToInt32(gTextBox.Text), Convert.ToInt32(bTextBox.Text));
            colorGroup[nowColorGroupIndex].setHSVValuse(Convert.ToInt32(s1TextBox.Text), Convert.ToInt32(s2TextBox.Text), Convert.ToInt32(v1TextBox.Text), Convert.ToInt32(v2TextBox.Text));

            int value = 127;
            Int32.TryParse(tbThreshold.Text, out value);
            MySettingManager.thresholdValue = value;

            //교정사전.
            MySettingManager.NowIsUseDicFileFlag = checkDic.Checked;
            MySettingManager.NowDicFile = dicFileTextBox.Text;
            MySettingManager.isUseMatchWordDic = cbPerWordDic.Checked;
            for(int i = 0; i < colorGroup.Count; i++)
            {
                colorGroup[i].checkHSVRange();
                valueRArray[i] = colorGroup[i].getValueR();
                valueGArray[i] = colorGroup[i].getValueG();
                valueBArray[i] = colorGroup[i].getValueB();

                valueS1Array[i] = colorGroup[i].getValueS1();
                valueS2Array[i] = colorGroup[i].getValueS2();
                valueV1Array[i] = colorGroup[i].getValueV1();
                valueV2Array[i] = colorGroup[i].getValueV2();
            }


            groupLabel.Text = (groupCombo.Items.Count - 2).ToString();  //색 그룹 개수 표시

            try
            {

                if(MySettingManager.OCRType == SettingManager.OcrType.Tesseract)
                {
                    bool isUseUnicode = false;
                    if(MySettingManager.NowIsUseJpnFlag)
                    {
                        isUseUnicode = true;
                    }

                    if(MySettingManager.NowIsUseOtherLangFlag)
                    {
                        isUseUnicode = true;
                    }

                    string tessData = MySettingManager.NowTessData;
                    if(MySettingManager.nowIsFastTess)
                    {
                        if(tessData == "eng" || tessData == "jpn")
                        {
                            tessData = tessData + "_fast";
                        }
                    }

                    setTessdata(tessData, isUseUnicode);
                }
                else if(MySettingManager.OCRType is SettingManager.OcrType.Window or SettingManager.OcrType.EasyOcr)
                {
                    bool isUseJpn = false;

                    if(MySettingManager.NowIsUseJpnFlag)
                    {
                        isUseJpn = true;
                    }

                    SetIsUseJpn(isUseJpn);

                }
                else if(MySettingManager.OCRType is SettingManager.OcrType.OneOcr)
                {
                    bool isUseJpn = false;
                    if(MySettingManager.NowIsUseJpnFlag)
                    {
                        isUseJpn = true;
                    }
                    SetIsUseJpn(_currentOcrLanguage != OcrLanguageType.English);
                }


                setFiducialValue(valueRArray, valueGArray, valueBArray, valueS1Array, valueS2Array, valueV1Array, valueV2Array, groupCombo.Items.Count - 2);

                //교정사전 관련

                setUseCheckSpelling(MySettingManager.NowIsUseDicFileFlag, MySettingManager.isUseMatchWordDic, MySettingManager.NowDicFile);


                bool isUseDBFlag = false;
                if(MySettingManager.NowTransType == SettingManager.TransType.db)
                {
                    isUseDBFlag = true;
                }
                SetRemoveSpace(MySettingManager.NowIsRemoveSpace);
                SetShowOCRIndex(MySettingManager.IsShowOCRIndex);
                SetIsStringUpper(MySettingManager.IsUseStringUpper);

                setUseDB(isUseDBFlag, MySettingManager.nowIsUsePartialDB, MySettingManager.NowDBFile);
                setAdvencedImgOption(MySettingManager.NowIsUseRGBFlag, MySettingManager.NowIsUseHSVFlag, MySettingManager.NowIsUseErodeFlag,
                    MySettingManager.ImgZoomSize, MySettingManager.isUseThreshold, MySettingManager.thresholdValue);
                SetCaptureArea();
                SetIsActiveWindow(MySettingManager.NowIsActiveWindow);


                if(MySettingManager.isDebugMode)
                {
                    SetIsDebugMode(MySettingManager.isDebugMode, cbShowReplace.Checked, cbSaveCapture.Checked, cbSaveCaptureResult.Checked);
                }

                ApplyCoreAdvencedOption();

            }
            catch(Exception e)
            {
                Util.ShowLog(e.Message + System.Environment.NewLine + e.StackTrace);
                MessageBox.Show(e.Message + System.Environment.NewLine + e.StackTrace);
            }
        }

        private void ApplyTransSetting()
        {
            string naverApiType = MORT.NaverTranslateAPI.API_NMT;
            naverApiType = MORT.NaverTranslateAPI.API_NMT;

            MySettingManager.NaverApiType = naverApiType;

            //공백 제거
            NaverIDKeyTextBox.Text = NaverIDKeyTextBox.Text.Replace(" ", "");
            NaverSecretKeyTextBox.Text = NaverSecretKeyTextBox.Text.Replace(" ", "");
            naverIDKey = NaverIDKeyTextBox.Text;
            naverSecretKey = NaverSecretKeyTextBox.Text;
            tbDeeplApi.Text = tbDeeplApi.Text.Replace(" ", "");

            var data = TransManager.Instace.GetNaverKey();

            //번역기 초기화
            NaverTranslateAPI.instance.Init(naverIDKey, naverSecretKey, naverApiType, data.isPaid);

            //구글 토큰 성공 여부.
            SettingManager.IsErrorEmptyGoogleToken = false;
            if(MySettingManager.NowTransType == SettingManager.TransType.google)
            {
                //구글 시트 처리
                string sheet = googleSheet_textBox.Text.Replace(" ", "");
                if(sheet.Contains("/d/"))
                {
                    int startIndex = sheet.LastIndexOf("/d/");
                    if(startIndex >= 0)
                    {
                        sheet = sheet.Remove(0, startIndex + 3);
                    }
                }

                if(sheet.Contains("/edit"))
                {
                    int startIndex = sheet.LastIndexOf("/edit");
                    if(startIndex >= 0)
                    {
                        sheet = sheet.Remove(startIndex);
                    }
                }


                googleSheet_textBox.Text = sheet;
                textBox_GoogleClientID.Text = textBox_GoogleClientID.Text.Replace(" ", "");
                textBox_GoogleSecretKey.Text = textBox_GoogleSecretKey.Text.Replace(" ", "");
                Logo.SetTopmost(false);
                TransManager.Instace.InitGtrans(googleSheet_textBox.Text, textBox_GoogleClientID.Text, textBox_GoogleSecretKey.Text, MySettingManager.GoogleTransCode, MySettingManager.GoogleResultCode);
            }
            else if(MySettingManager.NowTransType == SettingManager.TransType.ezTrans)
            {
                TransManager.Instace.InitEzTrans();
            }
            else if(MySettingManager.NowTransType == SettingManager.TransType.deepl)
            {
                TransManager.Instace.InitDeepL(MySettingManager.DeepLTransCode, MySettingManager.DeepLResultCode, GlobalDefine.DeeplFrontUrl, GlobalDefine.DeeplFormat,
                    GlobalDefine.DeeplElementTarget);
            }
            else if(MySettingManager.NowTransType == SettingManager.TransType.papago_web)
            {
                TransManager.Instace.InitPapagoWeb(MySettingManager.NaverTransCode, MySettingManager.NaverResultCode);
            }
            else if(MySettingManager.NowTransType == TransType.gemini)
            {
                TransManager.Instace.InitGemini(MySettingManager.GoogleTransCode, MySettingManager.GoogleResultCode);
                TransManager.Instace.InitializeGeminiModel(cbGeminiModel.Text, tbGeminiApi.Text);
                TransManager.Instace.InitGeminiCustom(AdvencedOptionManager.GeminiModel, AdvencedOptionManager.GeminiCommand, AdvencedOptionManager.GeminiDisableDefaultCommand);
            }
            else if(MySettingManager.NowTransType == SettingManager.TransType.customApi)
            {
                string url = AdvencedOptionManager.CustomApiUrl;
                string source = AdvencedOptionManager.CustomApiLanguageSource;
                string target = AdvencedOptionManager.CustomApiLanguageTarget;

                if(AdvencedOptionManager.UseGoogleLanguageCode)
                {
                    source = MySettingManager.GoogleTransCode;
                    target = MySettingManager.GoogleResultCode;
                }

                TransManager.Instace.InitCustomApi(url, source, target, MySettingManager.TranslateTypeSubKey);
            }
            else if(MySettingManager.NowTransType == SettingManager.TransType.chromeBridge)
            {
                //여기는 설정 적용 경로다. 언어와 엔진만 넘기고 크롬 창은 띄우지 않는다.
                //창을 띄우는 건 번역을 실제로 시작할 때(TranslationProcessInitializationService)로 미룬다.
                TransManager.Instace.InitChromeBridge(MySettingManager.GoogleTransCode, MySettingManager.GoogleResultCode,
                    AdvencedOptionManager.ChromeBridgeMode);
            }
            else if(MySettingManager.NowTransType == SettingManager.TransType.deeplApi)
            {
                string source = MySettingManager.DeepLTransCode;
                string target = MySettingManager.DeepLResultCode;
                DeepLAPIEndpointType endpointType = MySettingManager.nowDeepLAPIEndpointType;
                //TODO : API 가져오기
                TransManager.Instace.InitDeepLAPI(source, target, endpointType);
                TransManager.Instace.InitDeeplApiKey(tbDeeplApi.Text);
            }
            else if(MySettingManager.NowTransType == SettingManager.TransType.db)
            {
                // DB 파일 로드
                string dbFilePath = GlobalDefine.DB_PATH + MySettingManager.NowDBFile;
                if(File.Exists(dbFilePath))
                {
                    Dictionary<string, string> dbDictionary = new Dictionary<string, string>();
                    TransManager.Instace.TempDbDic = Util.LoadDBFile(dbFilePath, dbDictionary);
                    Util.ShowLog($"DB 파일 로드 완료: {dbFilePath} (항목 수: {dbDictionary.Count})");
                }
                else
                {
                    Util.ShowLog($"DB 파일을 찾을 수 없습니다: {dbFilePath}");
                }
            }

            SaveNaverKeyFile();
            SaveGoogleKeyFile();
            SaveGeminiKeyFile();
            Util.SaveFile(GlobalDefine.DeeplApiFile, tbDeeplApi.Text);
        }

        private bool CheckAndStopTransThread()
        {
            if(_processTranslateService.ProcessingState)
            {
                _processTranslateService.StopTranslate();
                return true;
            }

            return false;

        }

        public void ApplyTransTypeFromHotKey(SettingManager.TransType transType, string notice)
        {
            if(_currentState != CurrentStateType.None)
            {
                return;
            }

            Action callback = delegate
            {
                bool needStart = CheckAndStopTransThread();

                _currentState = CurrentStateType.LoadFile;
                MySettingManager.NowTransType = transType;

                BeginInvoke((Action)(() => SetTranslatorUIValue()));
                ApplyTransSetting();
                SaveSetting(GlobalDefine.USER_SETTING_FILE);

                bool isError = false;
                if(transType == SettingManager.TransType.google)
                {
                    isError = GetIsHasError();

                    if(isError)
                    {
                        StopTrans();
                    }
                }

                FormManager.Instace.AddText(notice);
                _currentState = CurrentStateType.None;

                if(needStart && !isError && _processTrans)
                {
                    ProcessTrans(OcrManager.OcrMethodType.Normal);
                }
            };

            if(InvokeRequired)
            {
                BeginInvoke(callback);
            }
            else
            {
                callback();
            }

        }

        public void ApplyFromQuickSetting(QuickSettingData data)
        {
            _currentState = CurrentStateType.LoadFile;
            //1. 번역이 진행중이면 먼저 중단한다
            //2. SettingData를 바꾼다
            //3. ui 값을 바꾼다
            //4. 적용한다

            //바꿀사항
            //-OCR 타입
            //-번역기 타입
            //-이미지 보정 사용 여부 / HSV 값
            //활성화 된 윈도우에서 추출 -> 강제 비활성화
            //번역창 -> 강제 레이어 번역창
            //OCR 속도 -> 가장 빠름
            StopTrans();

            //OCR 타입
            MySettingManager.OCRType = data.ocrType;

            //언어 코드
            if(data.languageType == OcrLanguageType.Japen)
            {
                MySettingManager.NowTessData = "jpn";
                MySettingManager.NowIsRemoveSpace = true;
                MySettingManager.NowIsUseJpnFlag = true;
            }
            else
            {
                MySettingManager.NowTessData = "eng";
                MySettingManager.NowIsRemoveSpace = false;
                MySettingManager.NowIsUseJpnFlag = false;
            }
            MySettingManager.NowIsUseOtherLangFlag = false;
            MySettingManager.nowIsFastTess = false;
            MySettingManager.WindowLanguageCode = data.LanguageCode;

            //번역기 타입
            MySettingManager.NowTransType = data.transType;
            if(data.languageType == OcrLanguageType.Japen)
            {
                MySettingManager.GoogleTransCode = "ja";
                MySettingManager.NaverTransCode = "ja";
                MySettingManager.DeepLTransCode = "ja";
            }
            else
            {
                MySettingManager.GoogleTransCode = "en";
                MySettingManager.NaverTransCode = "en";
                MySettingManager.DeepLTransCode = "en";
            }

            string resultCode = MySettingManager.GetDefaultResultCode();
            MySettingManager.GoogleResultCode = resultCode;
            MySettingManager.NaverResultCode = resultCode;
            MySettingManager.DeepLResultCode = resultCode;

            //색 보정
            MySettingManager.NowIsUseRGBFlag = false;
            MySettingManager.NowIsUseHSVFlag = false;
            MySettingManager.NowIsUseErodeFlag = false;
            MySettingManager.isUseThreshold = false;


            if(data.HsvList.Count > 0)
            {
                MySettingManager.NowIsUseHSVFlag = true;

                List<ColorGroup> list = new List<ColorGroup>();
                MySettingManager.NowColorGroupCount = data.HsvList.Count;

                for(int i = 0; i < data.HsvList.Count; i++)
                {
                    var color = new ColorGroup();
                    color.SetHSV(data.HsvList[i]);

                    list.Add(color);
                }

                MySettingManager.NowColorGroup = list;
            }

            //활성화 된 윈도우에서 추출
            MySettingManager.NowIsActiveWindow = false;

            //번역창
            MySettingManager.NowSkin = SettingManager.Skin.layer;

            //OCR 속도
            MySettingManager.NowOCRSpeed = 1;


            SetValueToUIValue();
            ApplyUIValueToSetting();

            SaveSetting(GlobalDefine.USER_SETTING_FILE);

        }

        public void ApplyBasicFont()
        {
            try
            {
                ITransform transform = FormManager.Instace.GetITransform();

                if(transform != null && transform is TransForm)
                {
                    if(!string.IsNullOrEmpty(AdvencedOptionManager.BasicFontData))
                    {
                        XmlSerializer deserializer = new XmlSerializer(typeof(SerializableFont));
                        using(TextReader tr = new StringReader(AdvencedOptionManager.BasicFontData))
                        {
                            SerializableFont font = (SerializableFont)deserializer.Deserialize(tr);
                            ((TransForm)(transform)).ApplyFont(font.ToFont());
                        }

                    }
                    else
                    {
                        ((TransForm)transform).SetDefaultFont();
                    }
                }

            }
            catch
            {

            }
        }

        private void ApplyRTL(bool enableRTL)
        {
            ITransform transform = FormManager.Instace.GetITransform();
            if(transform != null)
            {
                transform.ApplyRTL(enableRTL);
            }
        }

        private void ApplyLayerTextAlignmentBottom(bool isBottom)
        {
            ITransform transform = FormManager.Instace.GetITransform();
            if(transform is TransFormLayer layer)
            {
                layer.ApplyTextAlignmentBottom(isBottom);
            }
        }

        private void ApplyLayerTextAlignmentRight(bool isRight)
        {
            ITransform transform = FormManager.Instace.GetITransform();
            if(transform is TransFormLayer layer)
            {
                layer.ApplyTextAlignmentRight(isRight);
            }
        }

        private void ApplyAutoMergeText()
        {
            bool autoMerge = true;

            if(MySettingManager.NowSkin == SettingManager.Skin.over)
            {
                autoMerge = AdvencedOptionManager.UseAutoMerge;
            }

            OCRDataManager.Instace.MergeLine = autoMerge;
        }

        private void ApplyTopMostOptionWhenTranslate(bool useOption)
        {
            ITransform transform = FormManager.Instace.GetITransform();

            if(transform != null)
            {
                transform.ApplyUseTopMostOptionWhenTranslate(useOption);
            }
        }

        private void ApplyTranslateMemory()
        {
            _translateResultMemoryService.Init(AdvencedOptionManager.EnableTranslateMemory, AdvencedOptionManager.TranslateMemoryLimit, AdvencedOptionManager.TranslateMemoryRemainTime);
        }

        public void ApplyAdvencedOption()
        {
            bool isTrans = false;
            _processTranslateService.PauseAndRestartTranslate(() =>
            {
                //고급 설정값을 적용한다
                //번역집을 불러온다.
                TransManager.Instace.LoadUserTranslation(AdvencedOptionManager.TranslationFileList);
                TransManager.Instace.InitGeminiCustom(AdvencedOptionManager.GeminiModel, AdvencedOptionManager.GeminiCommand, AdvencedOptionManager.GeminiDisableDefaultCommand);

                FormManager.Instace.MyRemoteController?.ApplyTopMost(AdvencedOptionManager.EnableRttTopMost);


                //교정사전 추가 횟수를 지정한다.
                SetReCheckSpellingCount(AdvencedOptionManager.DicReProcessCount);

                ApplyRTL(AdvencedOptionManager.EnableRTL);
                ApplyLayerTextAlignmentBottom(AdvencedOptionManager.LayerTextAlignmentBottom);
                ApplyLayerTextAlignmentRight(AdvencedOptionManager.LayerTextAlignmentRight);

                ApplyTopMostOptionWhenTranslate(AdvencedOptionManager.UseTopMostOptionWhenTranslate);
                ApplyBasicFont();
                ApplyAutoMergeText();
                ShowResultFont();
                ApplyTranslateMemory();

                // 번역 타입 목록 업데이트
                UpdateTranslateType();

                //클립보드 설정
                InitClipboardMonitor(AdvencedOptionManager.IsUseClipboardTrans);
            });
        }


        public void ApplyCoreAdvencedOption()
        {
            TransManager.Instace.LoadUserTranslation(AdvencedOptionManager.TranslationFileList);
        }
    }
}
