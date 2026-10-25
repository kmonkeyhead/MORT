using MORT.Model.Debug;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace MORT.Service.Debug
{
    /// <summary>
    /// 이미지 인식 결과를 JSON 파일로 남겨 나중에 다시 확인할 수 있게 한다.
    /// OCR 단계와 오버레이 렌더 단계가 서로 다른 쓰레드에서 끝나기 때문에,
    /// OCR 결과를 먼저 받아두고 오버레이 스킨일 때만 렌더 값이 채워질 때까지 기다렸다가 저장한다.
    /// </summary>
    public class OcrDebugSnapshotService
    {
        private const string DirectoryName = "OcrAnalysis";

        private readonly object _lock = new object();
        private readonly string _debugDirectory;

        private OcrDebugSnapshotModel _pending;
        private bool _waitingOverlay;

        /// <summary>오버레이가 렌더 값을 채워주기를 기다리는 중인지.</summary>
        public bool IsWaitingOverlay
        {
            get
            {
                lock(_lock)
                {
                    return _waitingOverlay && _pending != null;
                }
            }
        }

        public OcrDebugSnapshotService()
        {
            _debugDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData", "Debug", DirectoryName);
        }

        /// <summary>
        /// OCR / 번역이 끝난 시점의 결과를 담아둔다.
        /// 오버레이 스킨이 아니면 이 시점에 바로 저장한다.
        /// </summary>
        public void CaptureOcrResult(
            List<OCRDataManager.ResultData> dataList,
            SettingManager.Skin skin,
            SettingManager.OcrType ocrType,
            SettingManager.TransType transType,
            string ocrText,
            string transText)
        {
            try
            {
                var areas = new List<OcrDebugArea>();
                if(dataList != null)
                {
                    foreach(var data in dataList)
                    {
                        if(data == null)
                        {
                            continue;
                        }

                        areas.Add(ConvertArea(data));
                    }
                }

                var snapshot = new OcrDebugSnapshotModel
                {
                    CapturedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    Skin = skin.ToString(),
                    OcrType = ocrType.ToString(),
                    TransType = transType.ToString(),
                    OcrText = ocrText ?? "",
                    TransText = transText ?? "",
                    Areas = areas,
                };

                OcrDebugSnapshotModel abandoned = null;
                lock(_lock)
                {
                    // 오버레이가 끝내 그려지지 않은 이전 스냅샷은 오버레이 정보 없이라도 남긴다.
                    if(_pending != null)
                    {
                        abandoned = _pending;
                    }

                    _pending = snapshot;
                    _waitingOverlay = skin == SettingManager.Skin.over;
                }

                if(abandoned != null)
                {
                    Save(abandoned);
                }

                if(skin != SettingManager.Skin.over)
                {
                    Flush();
                }
            }
            catch(Exception ex)
            {
                Util.ShowLog($"OcrDebugSnapshotService: CaptureOcrResult failed - {ex.Message}");
            }
        }

        /// <summary>
        /// 오버레이가 실제로 그린 최종값을 채우고 파일로 저장한다.
        /// </summary>
        public void CompleteOverlay(Rectangle formRect, bool useBackColor, List<OcrDebugOverlayBlock> blocks,
            OcrDebugPaintTiming timing = null)
        {
            try
            {
                var overlay = new OcrDebugOverlayInfo
                {
                    Timing = timing,
                    FormRect = OcrDebugRect.From(formRect),
                    IsAutoFontSize = AdvencedOptionManager.IsAutoFontSize,
                    MinAutoFontSize = AdvencedOptionManager.MinAutoFontSize,
                    MaxAutoFontSize = AdvencedOptionManager.MaxAutoFontSize,
                    KeepSourceDirection = AdvencedOptionManager.OverlayKeepSourceDirection,
                    BlurMode = AdvencedOptionManager.OverlayOutputMode.ToString(),
                    UseFontOutline = AdvencedOptionManager.OverlayUseFontOutline,
                    AutoColor = AdvencedOptionManager.OverlayAutoColor,
                    AutoBackgroundColor = AdvencedOptionManager.OverlayAutoBackgroundColor,
                    AutoFontColor = AdvencedOptionManager.OverlayAutoFontColor,
                    UseBackColor = useBackColor,
                    Blocks = blocks ?? new List<OcrDebugOverlayBlock>(),
                };

                lock(_lock)
                {
                    if(_pending == null)
                    {
                        return;
                    }

                    _pending = _pending with { Overlay = overlay };
                    _waitingOverlay = false;
                }

                Flush();
            }
            catch(Exception ex)
            {
                Util.ShowLog($"OcrDebugSnapshotService: CompleteOverlay failed - {ex.Message}");
            }
        }

        private void Flush()
        {
            OcrDebugSnapshotModel target;
            lock(_lock)
            {
                target = _pending;
                _pending = null;
                _waitingOverlay = false;
            }

            if(target != null)
            {
                Save(target);
            }
        }

        private void Save(OcrDebugSnapshotModel snapshot)
        {
            try
            {
                Directory.CreateDirectory(_debugDirectory);

                string fileName = $"ocr_analysis_{DateTime.Now:yyyyMMdd_HHmmss_fff}.json";
                string filePath = Path.Combine(_debugDirectory, fileName);
                string json = JsonConvert.SerializeObject(snapshot, Formatting.Indented);

                Util.SaveFile(filePath, json, false);
                Util.ShowLog($"OcrDebugSnapshotService: saved {filePath}");
            }
            catch(Exception ex)
            {
                Util.ShowLog($"OcrDebugSnapshotService: Save failed - {ex.Message}");
            }
        }

        private static OcrDebugArea ConvertArea(OCRDataManager.ResultData data)
        {
            var autoColors = new List<OcrDebugAutoColor>();
            foreach(var color in data.AutoColor)
            {
                autoColors.Add(new OcrDebugAutoColor
                {
                    Font = ToColorText(color.Font),
                    Background = ToColorText(color.BackGround),
                });
            }

            var lines = new List<OcrDebugLine>();
            foreach(var line in data.LineDataList)
            {
                lines.Add(ConvertLine(line));
            }

            var transBlocks = new List<OcrDebugTransBlock>();
            foreach(var transData in data.TransDataList)
            {
                transBlocks.Add(ConvertTransBlock(transData));
            }

            return new OcrDebugArea
            {
                Index = data.Index,
                SnapShot = data.SnapShot,
                AreaRect = OcrDebugRect.From(GetAreaRect(data)),
                ResultRect = OcrDebugRect.From(data.ResultRect),
                OcrText = data.GetOCR(),
                TransText = data.TransString,
                UseAutoColor = data.UseAutoColor,
                AutoColors = autoColors,
                Lines = lines,
                TransBlocks = transBlocks,
            };
        }

        private static OcrDebugTransBlock ConvertTransBlock(OCRDataManager.TransData transData)
        {
            var lines = new List<OcrDebugLine>();
            foreach(var line in transData.lineDataList)
            {
                lines.Add(ConvertLine(line));
            }

            return new OcrDebugTransBlock
            {
                Index = transData.index,
                Trans = transData.trans,
                IsTitle = transData.TitleData,
                AngleType = transData.angleType.ToString(),
                LineRect = OcrDebugRect.From(transData.lineRect),
                SourceRect = OcrDebugRect.From(transData.SourceRect),
                ViewRect = OcrDebugRect.From(transData.ViewRect),
                ContentRect = OcrDebugRect.From(transData.ContentRect),
                Lines = lines,
            };
        }

        private static OcrDebugLine ConvertLine(OCRDataManager.LineData lineData)
        {
            var words = new List<OcrDebugWord>();
            for(int i = 0; i < lineData.wordList.Count; i++)
            {
                Rectangle wordRect = i < lineData.wordRectList.Count ? lineData.wordRectList[i] : Rectangle.Empty;
                words.Add(new OcrDebugWord
                {
                    Text = lineData.wordList[i],
                    Rect = OcrDebugRect.From(wordRect),
                });
            }

            return new OcrDebugLine
            {
                GroupIndex = lineData.groupIndex,
                LineString = lineData.lineString,
                TransString = lineData.transString,
                AngleType = lineData.angleType.ToString(),
                LineRect = OcrDebugRect.From(lineData.lineRect),
                Words = words,
                TransWords = new List<string>(lineData.transWordList),
            };
        }

        private static Rectangle GetAreaRect(OCRDataManager.ResultData data)
        {
            try
            {
                if(data.SnapShot)
                {
                    return FormManager.Instace.MyMainForm.MySettingManager.LastSnapShotRect;
                }

                return FormManager.Instace.MyMainForm.GetOcrAreaProcessRect(data.Index);
            }
            catch(Exception ex)
            {
                Util.ShowLog($"OcrDebugSnapshotService: GetAreaRect failed - {ex.Message}");
                return Rectangle.Empty;
            }
        }

        public static string ToColorText(Color color)
        {
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
