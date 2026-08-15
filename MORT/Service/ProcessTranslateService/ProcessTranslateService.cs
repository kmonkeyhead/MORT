using MORT.Manager;
using MORT.Model;
using MORT.OcrApi.OneOcr;
using MORT.OcrApi.WindowOcr;
using MORT.Service.Overlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MORT.Form1;
using static MORT.Manager.OcrManager;
using static MORT.SettingManager;

namespace MORT.Service.ProcessTranslateService
{
    internal class ProcessTranslateService
    {
        //번역 쓰레드
        private Thread thread;
        public bool IdleState => !thread?.IsAlive ?? true || isEndFlag;
        public bool ProcessingState => thread != null && thread.IsAlive;

        public int OcrProcessSpeed { get; set; } = 2000; //ocr 처리 딜레이 시간

        //OCR 결과가 직전과 같을 때 번역창을 다시 그리는 최소 간격.
        //내용이 같아도 창 기하가 바뀔 수 있어 완전히 멈추지는 않는다.
        private const int IdleRepaintIntervalMs = 1000;

        //작업을 기다리는 도중 중단 요청이 왔는지 확인하는 간격
        private const int StopCheckIntervalMs = 50;

        //정지 요청 후 이 스레드가 끝나기를 기다리는 한도.
        //저수준 키보드 훅 프로시저가 300ms 넘게 붙잡히면 윈도우가 훅을 제거해
        //이후 MORT 단축키가 전부 죽는다. 그래서 훅에서 온 요청은 더 짧게 기다린다.
        public const int KeyHookJoinTimeoutMs = 250;
        public const int DefaultJoinTimeoutMs = 3000;
        public bool ClipeBoardReady { get; private set; } = true;

        public bool DebugUnlockOCRSpeed
        {
            get { return _isDebugUnlockOCRSpeed; }
            set { _isDebugUnlockOCRSpeed = value; }
        }

        private bool _isDebugUnlockOCRSpeed = false;

        // TODO : 하나로 통합해야 함
        public bool IsUseClipBoardFlag
        {
            get { return _settingManager.NowIsSaveInClipboardFlag; }
        }

        public string NowOcrString { get; set; } = ""; //현재 ocr 문장
        public SettingManager.TransType TransType => _settingManager.NowTransType;
        public Action<bool> OnStopTranslate { get; }

        volatile bool isEndFlag = false; //번역 끝내는 플레그
        private readonly Form _parent;
        private readonly SettingManager _settingManager;
        private readonly TranslateResultMemoryService _memoryService;
        private readonly WindowOcr _winOcr;
        private readonly OneOcr _oneOcr;
        private readonly bool _isAvailableWinOCR;
        private readonly TranslationProcessInitializationService _initializationService;
        private readonly TranslationImageModelService _imageModelService;
        private readonly MORT.Service.Debug.OcrDebugSnapshotService _debugSnapshotService;
        private OcrMethodType _ocrMethodType = OcrMethodType.None;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        private string LocalizeString(string key, bool replaceLine = false)
        {
            return LocalizeManager.LocalizeManager.GetLocalizeString(key).Replace("[]", "");
        }

        //TODO : DI 를 이용해보자?
        public ProcessTranslateService(Form parent, TranslateResultMemoryService memoryService, SettingManager settingManager, WindowOcr loader, bool isAvailableWinOCR, Action<bool> OnStopTranslate)
        {
            _parent = parent;
            _memoryService = memoryService;
            _settingManager = settingManager;
            _winOcr = loader;
            _oneOcr = new OneOcr();
            _isAvailableWinOCR = isAvailableWinOCR;
            _initializationService = new TranslationProcessInitializationService(settingManager);
            _imageModelService = new TranslationImageModelService(
                () => isEndFlag,
                () => isEndFlag = true);
            _debugSnapshotService = Program.ServiceContainer?.GetService(typeof(MORT.Service.Debug.OcrDebugSnapshotService))
                as MORT.Service.Debug.OcrDebugSnapshotService;
            this.OnStopTranslate = OnStopTranslate;
        }

        /// <summary>
        /// 작업이 끝나기를 기다리되, 중단 요청이 오면 기다리기를 멈춘다.
        /// 작업 자체를 취소하지는 못하지만, 여기서 계속 붙잡고 있으면 isEndFlag 를 확인할 기회가 없어
        /// 이 스레드가 끝나지 않는다. UI 는 thread.Join() 으로 이 스레드를 기다리는 중이라 같이 멈춘다.
        /// </summary>
        private TResult WaitForResult<TResult>(Task<TResult> task)
        {
            WaitForCompletion(task);
            return task.Result;
        }

        private void WaitForCompletion(Task task)
        {
            while (true)
            {
                bool completed;
                try
                {
                    completed = task.Wait(StopCheckIntervalMs);
                }
                catch (AggregateException e) when (e.InnerException is OperationCanceledException)
                {
                    //작업이 취소로 끝난 것은 오류가 아니다.
                    //그대로 두면 아래 일반 예외 처리로 흘러가 정지할 때마다 오류창이 뜬다.
                    throw new OperationCanceledException();
                }

                if (completed)
                {
                    return;
                }

                if (isEndFlag)
                {
                    throw new OperationCanceledException();
                }
            }
        }

        /// <summary>
        /// 번역 스레드가 끝나기를 기다린다. 시간 안에 끝나지 않으면 false.
        /// 시간 초과 시 호출부는 뒷작업을 진행하면 안 된다. 아직 살아있는 스레드와 설정 변경이 겹친다.
        /// </summary>
        private bool JoinThread(int timeoutMs)
        {
            if (thread == null || !thread.IsAlive)
            {
                return true;
            }

            isEndFlag = true;
            if (thread.Join(timeoutMs))
            {
                return true;
            }

            Util.ShowLog($"ProcessTranslateService: 번역 스레드가 {timeoutMs}ms 안에 끝나지 않았다");
            return false;
        }

        private string AdjustText(string text)
        {
            string result = text;

            if (result == null)
            {
                result = "";
            }

            if (_settingManager.NowIsRemoveSpace == true)
            {
                result = result.Replace(" ", "");
            }

            //교정 사전 사용 여부 체크.
            if (_settingManager.NowIsUseDicFileFlag)
            {
                StringBuilder sb = new StringBuilder(result, 8192);
                ProcessGetSpellingCheck(sb, _settingManager.isUseMatchWordDic);
                result = sb.ToString(); //ocr 결과
                sb.Clear();
            }


            //------------------OCR 줄바꿈 없애기 처리---------------------

            //over는 줄바꿈 처리 안 한다.

            bool isRequireReplace = true;

            if (IsDebugTransOneLine)
            {
                isRequireReplace = false;
            }
            else if (_settingManager.NowTransType == SettingManager.TransType.db || _settingManager.NowSkin == SettingManager.Skin.over)
            {
                isRequireReplace = false;
            }

            if (isRequireReplace)
            {
                if (_settingManager.NowIsRemoveSpace)
                {
                    result = result.Replace("\r\n", "");
                }
                else
                {
                    result = result.Replace("\r\n", " ");
                }
            }

            //---------------------------------------------------------

            return result;
        }

        /// <summary>
        /// OCR이 인식한 데이터 기반으로 최종 OCR / 번역문을 ref 로 저장한다
        /// </summary>
        /// <param name="index">ocr 영역 인덱스</param>
        /// <param name="ocrResultData">win ocr 결과</param>
        /// <param name="imgDataList">화면 데이터</param>
        /// <param name="currentOcr">현재 ocr이 인식한 ocr 문장</param>
        /// <param name="ocrResult">가공한 ocr 문장</param>
        /// <param name="finalTransResult">번역 결과</param>
        private void MakeFinalOcrAndTrans(int index, OCRDataManager.ResultData ocrResultData, List<ImgData> imgDataList, string currentOcr, ref string ocrResult, ref string finalTransResult)
        {
            List<string> ocrList = null;
            if (_settingManager.NowSkin == SettingManager.Skin.over)
            {
                if (ocrResultData != null)
                {
                    ocrList = ocrResultData.GetOcrText();
                    currentOcr = "";

                    for (int i = 0; i < ocrList.Count; i++)
                    {
                        ocrList[i] = AdjustText(ocrList[i]);

                        //OCR 영역 처리를 위해 한줄로 변환한다
                        currentOcr += System.Environment.NewLine + Util.GetSpliteToken(TransType) + ocrList[i];
                    }
                }
            }
            else
            {
                currentOcr = AdjustText(currentOcr);
            }

            System.Threading.Tasks.Task<string> transTask = null;

            transTask = TransManager.Instace.StartTrans(currentOcr, _settingManager.NowTransType, ocrList);
            //번역 결과를 적용한다
            var transResult = WaitForResult(transTask);

            if (ocrResultData != null)
            {
                ocrResultData.ApplyTransResult(transResult, TransType);

                if (imgDataList[index].UseAutoColor)
                {
                    var item = imgDataList[index];
                    if(Form1.IsDebugShowWordArea)
                    {
                        Util.ShowLog(
                            $"Overlay auto color buffer: snapshot={ocrResultData.SnapShot}, " +
                            $"ocr={item.x}x{item.y}x{item.channels}, original={item.originalX}x{item.originalY}x{item.originalChannels}");
                    }

                    for (int i = 0; i < ocrResultData.TransDataList.Count; i++)
                    {
                        var transData = ocrResultData.TransDataList[i];
                        var rect = ScaleSourceRect(transData.SourceRect, item);
                        var wordRects = transData.lineDataList
                            .SelectMany(line => line.wordRectList)
                            .Select(wordRect => ScaleSourceRect(wordRect, item))
                            .Where(wordRect => wordRect.Width > 0 && wordRect.Height > 0)
                            .ToList();
                        OverlayColorAnalysis colors = OverlayColorAnalyzer.Analyze(
                            item.originalData,
                            item.originalChannels,
                            item.originalX,
                            item.originalY,
                            rect,
                            wordRects);

                        if (!colors.Success)
                        {
                            ocrResultData.AddAutoColor(_settingManager.TextColor, _settingManager.BackgroundColor);
                            if(Form1.IsDebugShowWordArea)
                            {
                                Util.ShowLog($"Overlay auto color fallback: invalid samples, rect={rect}, text={transData.lineDataList.FirstOrDefault()?.lineString}");
                            }
                            continue;
                        }

                        ocrResultData.AddAutoColor(colors.Font, colors.Background);
                        if(Form1.IsDebugShowWordArea)
                        {
                            Util.ShowLog(
                                $"Overlay auto color: font={colors.Font.R},{colors.Font.G},{colors.Font.B}, " +
                                $"background={colors.Background.R},{colors.Background.G},{colors.Background.B}, " +
                                $"wordSupport={colors.ForegroundWordSupport}, contrast={colors.ForegroundContrast:0.00}, " +
                                $"fontFallback={colors.UsedFontFallback}, configuredAlpha={_settingManager.BackgroundColor.A}, " +
                                $"effectiveAlpha={(AdvencedOptionManager.OverlayUseBackgroundAlpha ? _settingManager.BackgroundColor.A : 255)}, rect={rect}");
                        }
                    }
                }
            }

            if (imgDataList.Count > 1)
            {
                if (_settingManager.IsShowOCRIndex)
                {
                    if (!string.IsNullOrEmpty(currentOcr))
                    {
                        if (transResult != "not thing")
                        {
                            finalTransResult += (imgDataList[index].index + 1).ToString() + " : " + transResult + System.Environment.NewLine;
                        }
                    }

                    ocrResult += (imgDataList[index].index + 1).ToString() + " : " + currentOcr + System.Environment.NewLine;
                }
                else
                {
                    if (!string.IsNullOrEmpty(currentOcr))
                    {
                        if (transResult != "not thing")
                        {
                            finalTransResult += "- " + transResult;

                            if (index + 1 < imgDataList.Count)
                            {
                                finalTransResult += System.Environment.NewLine;
                            }
                        }

                        ocrResult += "- " + currentOcr;

                        if (index + 1 < imgDataList.Count)
                        {
                            ocrResult += System.Environment.NewLine;
                        }
                    }
                }
            }
            else
            {
                finalTransResult = transResult;
                ocrResult = currentOcr;
            }
        }

        private static Rectangle ScaleSourceRect(Rectangle sourceRect, ImgData item)
        {
            if(item.x <= 0 || item.y <= 0 || item.originalX <= 0 || item.originalY <= 0)
            {
                return Rectangle.Empty;
            }

            double scaleX = item.originalX / (double)item.x;
            double scaleY = item.originalY / (double)item.y;
            int left = Math.Clamp((int)Math.Floor(sourceRect.Left * scaleX), 0, item.originalX);
            int top = Math.Clamp((int)Math.Floor(sourceRect.Top * scaleY), 0, item.originalY);
            int right = Math.Clamp((int)Math.Ceiling(sourceRect.Right * scaleX), left, item.originalX);
            int bottom = Math.Clamp((int)Math.Ceiling(sourceRect.Bottom * scaleY), top, item.originalY);
            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        //클립보드에 ocr/결과 저장
        private void SetClipBoard(string transText, string result)
        {
            if (transText != null)
            {
                try
                {
                    ClipeBoardReady = false;
                    string replaceOcrText = transText.Replace(" ", "");
                    replaceOcrText = transText.Replace("not thing", " ");

                    string clipboardText = "";

                    // 0 - OCR , 1 - RESULT, 2 - OCR + RESULT
                    clipboardText = AdvencedOptionManager.ClipboardSaveType switch
                    {
                        0 => replaceOcrText,
                        1 => result,
                        2 => $"{replaceOcrText}{Environment.NewLine}{Environment.NewLine}{result}",
                        _ => "",
                    };

                    if (!string.IsNullOrEmpty(replaceOcrText))
                    {
                        Clipboard.SetText(clipboardText); //인시로 둠
                    }

                    ClipeBoardReady = true;
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    ClipeBoardReady = true;
                    return;
                }
            }

            ClipeBoardReady = true;
        }

        public void DoTextToSpeach(string text)
        {
            if (_isAvailableWinOCR && _settingManager.IsUseTTS)
            {
                int type = 0;
                if (_settingManager.IsWaitTTSEnd)
                {
                    type = 1;
                }

                _winOcr.TextToSpeach(text, type);
            }
        }

        /// <summary>
        /// 실제 OCR 번역을 시작한다.
        /// 이 메서드는 전용 스레드 위에서 끝까지 동기로 돌아야 한다.
        /// async Task 로 두고 여기에 await 를 넣으면 스레드가 첫 await 에서 끝나버려
        /// thread.Join() 이 즉시 돌아오고 IdleState / ProcessingState 가 거짓을 보고한다.
        /// 그러면 정지했다고 판단한 쪽이 아직 살아있는 파이프라인과 동시에 설정을 바꾼다.
        /// </summary>
        /// <param name="ocrMethodType"></param>
        private void DoTrans(OcrMethodType ocrMethodType, TranslationProcessInitializationResult initialization)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();

            var token = _cts.Token;

            bool isOnce = initialization.IsOnce;
            bool useGoogleOcr = initialization.UseGoogleOcr;
            bool requireOriginalScreen = initialization.RequireOriginalScreen;

            //캡쳐할 클라이언트 위치.
            int clientPositionX = 0;
            int clientPositionY = 0;

            string formerOcrString = ""; //바로 이전에 가져온 문장
            ClipeBoardReady = true;
            int lastTick = 0;
            //OCR 결과가 그대로일 때 다시 그리는 간격
            int lastIdleRepaintTick = 0;
            try
            {
                while (isEndFlag == false)
                {
                    Logger.Logger.IncrementOcr();
                    int diff = Math.Abs(System.Environment.TickCount - lastTick);

                    //TODO :빠른 속도를 원하면 저 주석 해제하면 됨
                    if (diff >= OcrProcessSpeed /* / 10*/ || _isDebugUnlockOCRSpeed)
                    {
                        lastTick = System.Environment.TickCount;

                        if (FormManager.Instace.MyBasicTransForm != null || FormManager.Instace.MyLayerTransForm != null || FormManager.Instace.MyOverTransForm != null)
                        {
                            string finalTransResult = "";

                            if (useGoogleOcr)
                            {
                                unsafe
                                {
                                    int ocrAreaCount = FormManager.Instace.GetOcrAreaCount();
                                    List<ImgData> imgDataList = new List<ImgData>();

                                    _imageModelService.CreateModels(
                                        ocrAreaCount,
                                        imgDataList,
                                        ref clientPositionX,
                                        ref clientPositionY,
                                        _settingManager.IsUseAttachedCapture,
                                        requireOriginalScreen);

                                    if (isEndFlag)
                                    {
                                        break;
                                    }

                                    string ocrResult = "";
                                    string transResult = "";
                                    finalTransResult = "";

                                    OCRDataManager.Instace.ClearData();

                                    for (int j = 0; j < imgDataList.Count; j++)
                                    {
                                        var task = OcrManager.Instace.ProcessGoogleAsync(imgDataList[j]);
                                        string currentOcr = "";

                                        var result = WaitForResult(task);

                                        currentOcr = result.MainText;
                                        currentOcr = currentOcr.Replace("\r\n", "\n");
                                        currentOcr = currentOcr.Replace("\n", "\r\n");

                                        OcrResult point = new OcrResult(result);

                                        OCRDataManager.ResultData winOcrResultData = OCRDataManager.Instace.AddData(point, j, ocrMethodType == OcrMethodType.Snap, _settingManager.NowIsRemoveSpace);

                                        MakeFinalOcrAndTrans(j, winOcrResultData, imgDataList, currentOcr, ref ocrResult, ref finalTransResult);
                                        imgDataList[j].Clear();
                                        imgDataList[j].ClearOriginalData();
                                    }

                                    NowOcrString = ocrResult;
                                    imgDataList.Clear();
                                    imgDataList = null;
                                }
                            }

                            #region :::::::::: 윈도우 OCR 처리 :::::::::::

                            //win ocr 처리.
                            else if (_settingManager.OCRType == SettingManager.OcrType.Window)
                            {
                                if (_winOcr.GetIsAvailable())
                                {
                                    unsafe
                                    {
                                        Util.CheckTimeSpan(true);
                                        int ocrAreaCount = FormManager.Instace.GetOcrAreaCount();
                                        List<ImgData> imgDataList = new List<ImgData>();

                                        _imageModelService.CreateModels(
                                            ocrAreaCount,
                                            imgDataList,
                                            ref clientPositionX,
                                            ref clientPositionY,
                                            _settingManager.IsUseAttachedCapture,
                                            requireOriginalScreen);

                                        if (isEndFlag)
                                        {
                                            break;
                                        }

                                        string ocrResult = "";
                                        string transResult = "";
                                        finalTransResult = "";

                                        OCRDataManager.Instace.ClearData();
                                        for (int j = 0; j < imgDataList.Count; j++)
                                        {
                                            //잠시 막음 - 원래 이게 성장임
                                            _winOcr.SetBitMap(imgDataList[j].data, imgDataList[j].channels, imgDataList[j].x, imgDataList[j].y);

                                            Util.CheckTimeSpan(false);

                                            _winOcr.StartMakeBitmap();
                                            imgDataList[j].Clear();
                                            _winOcr.ProcessOCR();


                                            while (!isEndFlag && !_winOcr.GetIsAvailable())
                                            {
                                                Thread.Sleep(2);
                                            }

                                            string currentOcr = _winOcr.GetText();
                                            var winOcrResult = _winOcr.MakeResultData();


                                            var winOcrResultData = OCRDataManager.Instace.AddData(new OcrResult(winOcrResult), j, ocrMethodType == OcrMethodType.Snap, _settingManager.NowIsRemoveSpace);

                                            MakeFinalOcrAndTrans(j, winOcrResultData, imgDataList, currentOcr, ref ocrResult, ref finalTransResult);

                                            imgDataList[j].ClearOriginalData();
                                        }

                                        NowOcrString = ocrResult;
                                        imgDataList.Clear();
                                        imgDataList = null;
                                    }
                                }
                                else
                                {
                                    //준비되지 않았으면 이전과 같게 처리.
                                    NowOcrString = formerOcrString;
                                }
                            }

                            #endregion

                            #region::::::::: One OCR :::::::::::

                            else if (_settingManager.OCRType == SettingManager.OcrType.OneOcr)
                            {
                                unsafe
                                {
                                    Util.CheckTimeSpan(true);
                                    int ocrAreaCount = FormManager.Instace.GetOcrAreaCount();
                                    List<ImgData> imgDataList = new List<ImgData>();

                                    _imageModelService.CreateModels(
                                        ocrAreaCount,
                                        imgDataList,
                                        ref clientPositionX,
                                        ref clientPositionY,
                                        _settingManager.IsUseAttachedCapture,
                                        requireOriginalScreen);

                                    if (isEndFlag)
                                    {
                                        break;
                                    }

                                    string ocrResult = "";
                                    string transResult = "";
                                    finalTransResult = "";

                                    OCRDataManager.Instace.ClearData();
                                    for (int j = 0; j < imgDataList.Count; j++)
                                    {
                                        Util.CheckTimeSpan(false);

                                        var task = _oneOcr.ConvertToTextAsync(imgDataList[j].data, imgDataList[j].channels, imgDataList[j].x, imgDataList[j].y, imgDataList[j].Clear).AsTask();

                                        var result = WaitForResult(task);

                                        if (result == null)
                                        {
                                            // 백그라운드에서 UI를 직접 호출하지 않음. UI 스레드에서 알리고 종료 트리거.
                                            _parent.BeginInvoke((Action)(() =>
                                            {
                                                if (MessageBox.Show(LocalizeString("Unable Use OCR Snipping Tool OCR Error"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                                {
                                                    Util.OpenURL("https://blog.naver.com/killkimno/224097385261");
                                                }

                                                OnStopTranslate?.Invoke(true);
                                            }));

                                            // 작업 취소 플래그 설정 후 안전히 반환
                                            _cts.Cancel();
                                            isEndFlag = true;
                                            return;
                                        }

                                        string currentOcr = "";
                                        foreach (var line in result)
                                        {
                                            currentOcr += line.Text + System.Environment.NewLine;
                                        }

                                        OCRDataManager.ResultData resultModel = OCRDataManager.Instace.AddData(new OcrResult(result), j, ocrMethodType == OcrMethodType.Snap, _settingManager.NowIsRemoveSpace);

                                        MakeFinalOcrAndTrans(j, resultModel, imgDataList, currentOcr, ref ocrResult, ref finalTransResult);

                                        imgDataList[j].ClearOriginalData();
                                    }

                                    NowOcrString = ocrResult;
                                    imgDataList.Clear();
                                    imgDataList = null;
                                }
                            }

                            #endregion

                            #region:::::::::: Easy OCR 처리 ::::::::::

                            else if (_settingManager.OCRType == OcrType.EasyOcr)
                            {
                                unsafe
                                {
                                    bool installed = OcrManager.Instace.IsPipInstalled();

                                    if (!installed)
                                    {
                                        //설치가 안 되어 있으면 중단해야 한다
                                        //메세지 창을 뛰우고 설치할지 물어본다
                                        //isEndFlag = true;
                                        //_parent.BeginInvoke((Action)(() => OnStopTranslate(true)));
                                        //return;

                                        //지금은 그냥 설치한다
                                    }

                                    var prepareTask = OcrManager.Instace.PrepareEasyOcrAsync(_settingManager.EasyOcrCode, false, "torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu121");

                                    //pip 설치는 오래 걸린다. 중단 요청이 오면 기다리기를 멈춘다
                                    WaitForCompletion(prepareTask);


                                    Util.CheckTimeSpan(true);
                                    int ocrAreaCount = FormManager.Instace.GetOcrAreaCount();
                                    List<ImgData> imgDataList = new List<ImgData>();

                                    _imageModelService.CreateModels(
                                        ocrAreaCount,
                                        imgDataList,
                                        ref clientPositionX,
                                        ref clientPositionY,
                                        _settingManager.IsUseAttachedCapture,
                                        requireOriginalScreen);

                                    if (isEndFlag)
                                    {
                                        break;
                                    }

                                    string ocrResult = "";
                                    string transResult = "";
                                    finalTransResult = "";

                                    OCRDataManager.Instace.ClearData();

                                    for (int j = 0; j < imgDataList.Count; j++)
                                    {
                                        var model = OcrManager.Instace.ProcessEasyOcr(imgDataList[j].data, imgDataList[j].channels, imgDataList[j].x, imgDataList[j].y);
                                        //ocrResult = model.MainText;
                                        Util.CheckTimeSpan(false);

                                        imgDataList[j].Clear();

                                        //TODO : EasyOCR 도 ResultData 형식으로 만들어야 한다
                                        OCRDataManager.ResultData resultModel = OCRDataManager.Instace.AddData(new OcrResult(model), j, ocrMethodType == OcrMethodType.Snap, _settingManager.NowIsRemoveSpace);

                                        MakeFinalOcrAndTrans(j, resultModel, imgDataList, model.MainText, ref ocrResult, ref finalTransResult);

                                        imgDataList[j].ClearOriginalData();
                                    }

                                    NowOcrString = ocrResult;
                                    imgDataList.Clear();
                                    imgDataList = null;
                                }
                            }

                            #endregion

                            else
                            {
                                //Tessreact OCR
                                StringBuilder sb = new StringBuilder(8192);
                                StringBuilder sb2 = new StringBuilder(8192);
                                IntPtr hdc = IntPtr.Zero;

                                if (_settingManager.IsUseAttachedCapture)
                                {
                                    byte[] byteData = default(byte[]);
                                    int width = 0;
                                    int height = 0;

                                    int positionX = 0;
                                    int positionY = 0;

                                    _imageModelService.GetImageBytesFromCapture(
                                        ref byteData,
                                        ref width,
                                        ref height,
                                        ref positionX,
                                        ref positionY);

                                    if (isEndFlag)
                                    {
                                        break;
                                    }

                                    processOcrWithData(sb, sb2, width, height, positionX, positionY, byteData);
                                }
                                else
                                {
                                    processOcr(sb, sb2);
                                }


                                NowOcrString = sb.ToString(); //ocr 결과

                                //------------------OCR 줄바꿈 없애기 처리---------------------
                                NowOcrString = NowOcrString.Replace("\r\n", "\n");


                                if (!IsDebugTransOneLine) //디버그 - 한 줄씩 번역이 켜져 있으면 -> 줄바꿈 없애기를 안 한다
                                {
                                    if (_settingManager.NowIsRemoveSpace)
                                    {
                                        NowOcrString = NowOcrString.Replace("\n", "");
                                    }
                                    else
                                    {
                                        NowOcrString = NowOcrString.Replace("\n", " ");
                                    }
                                }

                                //---------------------------------------
                                NowOcrString = NowOcrString.Replace("\t", System.Environment.NewLine);

                                finalTransResult = sb2.ToString(); //번역 결과.
                                sb.Clear();
                                sb2.Clear();


                                if (_settingManager.NowTransType != SettingManager.TransType.db && formerOcrString.CompareTo(NowOcrString) != 0)
                                {
                                    System.Threading.Tasks.Task<string> test = TransManager.Instace.StartTrans(NowOcrString, _settingManager.NowTransType);
                                    finalTransResult = WaitForResult(test);
                                }
                            }

                            token.ThrowIfCancellationRequested();

                            //TODO : Async 문으로 변경하자

                            //OCR, 번역 끝 화면에 뿌리기
                            //새로 데이터 갱신해야 함.
                            if (formerOcrString.CompareTo(NowOcrString) != 0 || NowOcrString == "")
                            {
                                formerOcrString = NowOcrString;
                                if (IsUseClipBoardFlag == true && ClipeBoardReady)
                                {
                                    _parent.BeginInvoke(() => SetClipBoard(NowOcrString, finalTransResult));
                                }

                                string currentTranslateResult = finalTransResult;

                                finalTransResult = _memoryService.CheckMemoryResult(finalTransResult);

                                //디버깅 : 이미지 인식 결과를 파일로 남긴다.
                                //오버레이는 아래 UpdateText 이후 실제로 그릴 때 최종값이 채워진다.
                                if (Form1.IsDebugSaveAnalysisResult && _debugSnapshotService != null)
                                {
                                    _debugSnapshotService.CaptureOcrResult(
                                        OCRDataManager.Instace.GetData(),
                                        _settingManager.NowSkin,
                                        _settingManager.OCRType,
                                        _settingManager.NowTransType,
                                        NowOcrString,
                                        finalTransResult);
                                }

                                if (_settingManager.NowSkin == SettingManager.Skin.dark && FormManager.Instace.MyBasicTransForm != null)
                                {
                                    FormManager.Instace.MyBasicTransForm.updateText(finalTransResult, NowOcrString, TransType, _settingManager.NowIsShowOcrResultFlag);
                                }
                                else if (_settingManager.NowSkin == SettingManager.Skin.layer && FormManager.Instace.MyLayerTransForm != null)
                                {
                                    Action action = delegate
                                    {
                                        if (FormManager.Instace.MyLayerTransForm != null)
                                        {
                                            FormManager.Instace.MyLayerTransForm.updateText(finalTransResult, NowOcrString, _settingManager.NowIsShowOcrResultFlag);
                                        }
                                    };
                                    _parent.BeginInvoke(action);
                                }
                                else if (_settingManager.NowSkin == SettingManager.Skin.over && FormManager.Instace.MyOverTransForm != null)
                                {
                                    Action action = delegate
                                    {
                                        if (FormManager.Instace.MyOverTransForm != null)
                                        {
                                            List<OCRDataManager.ResultData> dataList = OCRDataManager.Instace.GetData();
                                            //argv3, nowOcrString
                                            FormManager.Instace.MyOverTransForm.UpdateText(dataList, _settingManager.NowIsShowOcrResultFlag, clientPositionX, clientPositionY);
                                        }
                                    };

                                    _parent.BeginInvoke(action);
                                }

                                if (_settingManager.NowIsSaveOcrReulstFlag)
                                {
                                    SaveOcrResult(currentTranslateResult, NowOcrString);
                                }

                                //TTS 처리
                                if (_settingManager.NowSkin == SettingManager.Skin.over)
                                {
                                    string transResult = finalTransResult.Replace(Util.GetSpliteToken(TransType), "", StringComparison.InvariantCulture);
                                    DoTextToSpeach(transResult);
                                }
                                else
                                {
                                    DoTextToSpeach(finalTransResult);
                                }

                                if (isOnce)
                                {
                                    isEndFlag = true;
                                    _parent.BeginInvoke((Action)(() => OnStopTranslate(true)));
                                }
                            }
                            else
                            {
                                //이전과 같아서 그래픽만 갱신함.
                                //같은 내용을 다시 그리는 것은 낭비지만, 데이터가 그대로여도 창 기하가
                                //바뀌는 경우(OCR 영역 이동, 대상 창 이동)가 있어 완전히 멈출 수는 없다.
                                //그래서 매번이 아니라 일정 간격으로만 다시 그린다.
                                int idleDiff = Math.Abs(System.Environment.TickCount - lastIdleRepaintTick);
                                if (idleDiff >= IdleRepaintIntervalMs)
                                {
                                    lastIdleRepaintTick = System.Environment.TickCount;

                                    if (_settingManager.NowSkin == SettingManager.Skin.layer && FormManager.Instace.MyLayerTransForm != null)
                                    {
                                        FormManager.Instace.MyLayerTransForm.UpdatePaint();
                                    }

                                    if (_settingManager.NowSkin == SettingManager.Skin.over && FormManager.Instace.MyOverTransForm != null)
                                    {
                                        FormManager.Instace.MyOverTransForm.UpdatePaint();
                                    }
                                }

                                if (isOnce)
                                {
                                    isEndFlag = true;
                                    _parent.BeginInvoke((Action)(() => OnStopTranslate(true)));
                                }
                            }
                        }
                    }
                    else
                    {
                        //시스템 과부화를 위해 대기탄다
                        Thread.Sleep(100);
                    }
                }

                TransManager.Instace.SaveFormerResultFile(_settingManager.NowTransType);
            }
            catch (OperationCanceledException)
            {
                if (isOnce)
                {
                    isEndFlag = true;
                    _parent.BeginInvoke((Action)(() => OnStopTranslate(true)));
                }
            }
            catch (Exception e)
            {
                //이 스레드에서 MessageBox 를 띄우면 이 스레드에 모달 루프가 생겨 스레드가 끝나지 않는다.
                //UI 가 thread.Join() 으로 이 스레드를 기다리는 중이면 서로 못 빠져나온다.
                //알림은 UI 스레드에 넘기고 이 스레드는 그대로 끝낸다.
                string message = $"{e.Message} / {e.StackTrace}";
                Util.ShowLog(message);

                try
                {
                    _parent.BeginInvoke((Action)(() => MessageBox.Show(message)));
                }
                catch (Exception reportException)
                {
                    Util.ShowLog($"ProcessTranslateService: failed to report error - {reportException.Message}");
                }
            }
        }

        private void StartTranslationThread(OcrMethodType ocrMethodType)
        {
            // 번역창 Prepare는 캡처보다 먼저 끝나야 한다. 작업 스레드에서 UI Invoke를
            // 기다리면 UI가 기존 작업 스레드를 Join하는 설정 적용 경로와 교착된다.
            _ocrMethodType = ocrMethodType;
            TranslationProcessInitializationResult initialization = _initializationService.Initialize(ocrMethodType);
            if(!initialization.CanStart)
            {
                return;
            }

            thread = new Thread(() => DoTrans(ocrMethodType, initialization));
            thread.Start();
        }

        public static void CompareStrings(string a, string b)
        {
            Logger.Logger.AddLog("Equal (Ordinal): " + string.Equals(a, b, StringComparison.Ordinal));
            Logger.Logger.AddLog("Equal (OrdinalIgnoreCase): " + string.Equals(a, b, StringComparison.OrdinalIgnoreCase));

            var na = a?.Normalize(System.Text.NormalizationForm.FormC) ?? string.Empty;
            var nb = b?.Normalize(System.Text.NormalizationForm.FormC) ?? string.Empty;
            Logger.Logger.AddLog("Normalized equal: " + string.Equals(na, nb, StringComparison.Ordinal));

            var aBytes = Encoding.UTF8.GetBytes(na);
            var bBytes = Encoding.UTF8.GetBytes(nb);
            Logger.Logger.AddLog($"UTF8 byte length A: {aBytes.Length}, B: {bBytes.Length}");
            Logger.Logger.AddLog("UTF8 bytes equal: " + StructuralComparisons.StructuralEqualityComparer.Equals(aBytes, bBytes));

            // 간단 추정: 토큰 수 대략 = bytes / 4 (정확하지 않음)
            Logger.Logger.AddLog($"Rough token estimate A: {aBytes.Length / 4.0:F1}, B: {bBytes.Length / 4.0:F1}");
        }

        private void SaveOcrResult(string transText, string ocrText)
        {
            if (transText.CompareTo("not thing") == 0)
            {
                transText = "";
            }

            ocrText = ocrText.Replace("\r\n", "\n");
            System.IO.StreamWriter file;
            try
            {
                using (file = new System.IO.StreamWriter(@"ocrResult.txt", true))
                {
                    file.WriteLine("/s");
                    file.WriteLine(ocrText);
                    file.WriteLine("/t");
                    file.WriteLine(transText);
                    file.WriteLine("/e");
                    file.WriteLine(System.Environment.NewLine);
                }
            }
            catch (FileNotFoundException)
            {
                using (System.IO.FileStream fs = System.IO.File.Create(@"ocrResult.txt"))
                {
                    fs.Close();
                    fs.Dispose();
                    file = new System.IO.StreamWriter(@"ocrResult.txt", true);
                    file.WriteLine("/s");
                    file.WriteLine(ocrText);
                    file.WriteLine("/t");
                    file.WriteLine(transText);
                    file.WriteLine("/e");
                    file.WriteLine(System.Environment.NewLine);
                }
            }

            file.Close();
            file.Dispose();
        }


        public void ProcessTrans(OcrMethodType ocrMethodType) //번역 시작 쓰레드
        {
            if (!JoinThread(DefaultJoinTimeoutMs))
            {
                //이전 스레드가 아직 살아있다. 여기서 새로 시작하면 둘이 같이 돌게 된다.
                //isEndFlag 는 되돌리지 않는다. 되돌리면 그 스레드가 계속 돈다.
                return;
            }

            isEndFlag = false;
            StartTranslationThread(ocrMethodType);
        }

        public void StopTranslate(int joinTimeoutMs = DefaultJoinTimeoutMs)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
            TransManager.Instace.StopTrans();

            if (JoinThread(joinTimeoutMs))
            {
                thread = null;
                isEndFlag = false;
            }

            //시간 초과면 isEndFlag 를 true 로 남겨둔다.
            //되돌리면 아직 살아있는 스레드가 정지 요청을 못 보고 계속 번역한다.
        }

        /// <summary>
        /// 작업을 처리한 후 번역 다시 시작 - 기존 번역이 없으면 무시
        /// </summary>
        /// <param name="callback"></param>
        /// <returns>
        /// 돌고 있던 번역을 멈췄다가 다시 시작했으면 true.
        /// 제한 시간 안에 멈추지 못하면 callback 을 실행하지 않고 false 를 돌려준다.
        /// 호출부는 "번역이 돌고 있지 않았다"와 같게 취급하게 되는데,
        /// 그 경로가 대개 ProcessTrans 로 이어져 정지를 한 번 더 시도하므로 회복될 여지가 있다.
        /// </returns>
        public bool PauseAndRestartTranslate(Action callback, OcrMethodType ocrMethodType = OcrMethodType.None,
            int joinTimeoutMs = DefaultJoinTimeoutMs)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
            TransManager.Instace.StopTrans();
            bool requireRestart = thread != null && thread.IsAlive;
            if (requireRestart)
            {
                if (!JoinThread(joinTimeoutMs))
                {
                    //번역 스레드가 아직 살아있다. 여기서 callback 을 실행하면
                    //그 스레드가 쓰고 있는 설정과 캡쳐 영역을 동시에 바꾸게 된다.
                    //isEndFlag 도 되돌리지 않는다.
                    return false;
                }

                isEndFlag = false;
            }

            callback();

            if (requireRestart)
            {
                StartTranslationThread(ocrMethodType == OcrMethodType.None ? _ocrMethodType : ocrMethodType);
            }

            return requireRestart;
        }
    }
}
