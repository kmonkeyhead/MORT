#nullable enable
using MORT.Model;
using MORT.Service.Overlay;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MORT
{
    public partial class TransFormOver
    {
        private OverlayAdaptiveRefreshService? _adaptiveRefresh;
        private OverlayAdaptiveScene? _adaptiveScene;
        private OverlayAdaptiveMask[] _adaptiveMasks = Array.Empty<OverlayAdaptiveMask>();
        private Point _adaptiveCaptureOrigin;
        private bool _allowLiveAdaptive;

        internal void SetAdaptiveLiveEnabled(bool enabled)
        {
            _allowLiveAdaptive = enabled;
            if(!enabled) ClearAdaptiveMasks();
        }

        internal void UpdateAdaptiveMasks(OverlayAdaptiveMask[] masks, Point captureOrigin)
        {
            if(!_allowLiveAdaptive || TranslateStatusType != TranslateStatusType.Translate || isDestroyFormFlag
                || AdvencedOptionManager.OverlayOutputMode != OverlayBlurMode.Restore) return;
            _adaptiveMasks = masks;
            _adaptiveCaptureOrigin = captureOrigin;
            RequestOverlayPaint();
        }

        private void ClearAdaptiveMasks()
        {
            _adaptiveMasks = Array.Empty<OverlayAdaptiveMask>();
            StopAdaptiveBackground();
        }

        private void StopAdaptiveBackground()
        {
            _adaptiveScene = null;
            _adaptiveRefresh?.SetScene(null, null);
        }

        private void DisposeAdaptiveBackground()
        {
            _adaptiveMasks = Array.Empty<OverlayAdaptiveMask>();
            _adaptiveScene = null;
            _adaptiveRefresh?.Dispose();
            _adaptiveRefresh = null;
        }

        private void UpdateAdaptiveScene(List<OverlayRenderBlock>? displayedBlocks = null)
        {
            if(!_allowLiveAdaptive || !isActiveGDI || !_isStart || !Visible || !IsHandleCreated || _forceLock
                || TranslateStatusType != TranslateStatusType.Translate
                || AdvencedOptionManager.OverlayOutputMode != OverlayBlurMode.Restore)
            {
                StopAdaptiveBackground();
                return;
            }
            var capture = _attchedCapture ? FormManager.Instace.screenCaptureUI?.AdaptiveCaptureSource : null;
            if(_attchedCapture && capture == null) { StopAdaptiveBackground(); return; }
            var areas = new List<OverlayAdaptiveArea>();
            double zoom = Math.Max(0.01, FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);
            void AddArea(int index, Size size, Rectangle[] mask, Rectangle screen)
            {
                if(screen.Width <= 0 || screen.Height <= 0 || !Bounds.IntersectsWith(screen)
                    || (long)screen.Width * screen.Height > int.MaxValue / 4) return;
                double scaleX = size.Width > 0 ? screen.Width / (double)size.Width : 1 / zoom;
                double scaleY = size.Height > 0 ? screen.Height / (double)size.Height : 1 / zoom;
                var bounds = new Rectangle(Point.Empty, screen.Size);
                var words = mask
                    .Select(rect => Rectangle.Intersect(bounds, Rectangle.FromLTRB(
                        (int)Math.Floor(rect.Left * scaleX), (int)Math.Floor(rect.Top * scaleY),
                        (int)Math.Ceiling(rect.Right * scaleX), (int)Math.Ceiling(rect.Bottom * scaleY))))
                    .Where(rect => rect.Width > 0 && rect.Height > 0).Distinct().ToArray();
                if(words.Length > 0) areas.Add(new OverlayAdaptiveArea(index, screen, words));
            }

            if(displayedBlocks is { Count: > 0 })
            {
                //이전 번역문이 표시되는 동안에는 그 번역문에 대응하는 원문만 복원한다.
                foreach(var group in displayedBlocks.GroupBy(block => block.TargetData.Index))
                {
                    var first = group.First();
                    Rectangle screen = first.CaptureImageRect;
                    screen.Offset(Location);
                    var words = OverlayAdaptiveMask.GetWords(group.SelectMany(block => block.TransData.lineDataList));
                    AddArea(group.Key, first.TargetData.OcrImageSize, words, screen);
                }
            }
            else
            {
                foreach(var mask in _adaptiveMasks)
                    AddArea(mask.Index, mask.ImageSize, mask.Words, GetCaptureScreenRect(mask.Index, false, _adaptiveCaptureOrigin));
            }
            if(areas.Count == 0) { StopAdaptiveBackground(); return; }
            var scene = new OverlayAdaptiveScene(areas.ToArray(), AdvencedOptionManager.OverlayRestoreLevel, capture, SystemInformation.VirtualScreen, AdvencedOptionManager.OverlayTargetFps);
            if(SameAdaptiveScene(_adaptiveScene, scene)) return;
            _adaptiveScene = scene;
            _adaptiveRefresh ??= Program.ServiceContainer?.GetService(typeof(OverlayAdaptiveRefreshService)) as OverlayAdaptiveRefreshService
                ?? new OverlayAdaptiveRefreshService();
            _adaptiveRefresh.SetScene(scene, RequestOverlayPaint);
        }

        private static bool SameAdaptiveScene(OverlayAdaptiveScene? first, OverlayAdaptiveScene second)
        {
            if(first == null || first.TargetFps != second.TargetFps || first.Level != second.Level || first.DesktopBounds != second.DesktopBounds
                || !ReferenceEquals(first.Capture, second.Capture) || first.Areas.Length != second.Areas.Length) return false;
            for(int i = 0; i < first.Areas.Length; i++)
            {
                if(!SameAdaptiveArea(first.Areas[i], second.Areas[i])) return false;
            }
            return true;
        }

        private static bool SameAdaptiveArea(OverlayAdaptiveArea first, OverlayAdaptiveArea second) =>
            first.Index == second.Index && first.ScreenRect == second.ScreenRect && first.Words.SequenceEqual(second.Words);

        //같은 문장이라도 글자 위치가 바뀌면 복원 마스크도 교체해야 한다.
        private static bool SameAdaptiveGeometry(OCRDataManager.ResultData first, OCRDataManager.ResultData second)
        {
            if(first.OcrImageSize != second.OcrImageSize || first.TransDataList.Count != second.TransDataList.Count) return false;
            for(int i = 0; i < first.TransDataList.Count; i++)
            {
                var a = first.TransDataList[i]; var b = second.TransDataList[i];
                if(a.SourceRect != b.SourceRect || a.lineRect != b.lineRect || a.lineDataList.Count != b.lineDataList.Count) return false;
                for(int j = 0; j < a.lineDataList.Count; j++)
                    if(a.lineDataList[j].lineRect != b.lineDataList[j].lineRect
                        || !a.lineDataList[j].wordRectList.SequenceEqual(b.lineDataList[j].wordRectList)) return false;
            }
            return true;
        }
    }
}
