using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;

namespace MORT
{
    public partial class TransFormOver
    {
        //UI 스레드에서만 사용한다. 색상·배경 픽셀은 보관하지 않는다.
        private readonly record struct OverlayPathKey(string Text, string Family, FontStyle Style,
            float EmSize, float DpiX, float DpiY, Rectangle Rect, StringFormatFlags Flags,
            StringAlignment Alignment, StringAlignment LineAlignment, StringTrimming Trimming);
        private readonly record struct OverlayFitKey(OverlayPathKey Path, bool Vertical, bool Outline);
        private Dictionary<OverlayPathKey, GraphicsPath> _textPathCache;
        private Dictionary<OverlayFitKey, bool> _textFitCache;
        private int _cachedPathPoints;
        private MORT.Service.Overlay.OverlayPaintScheduler _paintScheduler;
        private readonly record struct OverlayLayoutContext(OverlayPathKey Font, bool AutoSize, int Minimum,
            int Maximum, bool Outline, bool Rtl, float Zoom);
        private readonly record struct OverlayLayoutInput(OCRDataManager.ResultData Owner, string Text,
            Rectangle Source, Rectangle Capture, bool Vertical, bool Title, string LineSizes);
        private readonly record struct OverlayLayoutResult(Rectangle View, Rectangle Content, float FontSize,
            float Preferred, float Minimum, bool Clipped);
        private OverlayLayoutContext _layoutContext;
        private OverlayLayoutInput[] _layoutInputs;
        private OverlayLayoutResult[] _layoutResults;

        private OverlayLayoutContext GetLayoutContext(Graphics g, Font font, StringFormat format) =>
            new OverlayLayoutContext(GetOverlayPathKey(g, "", font, Rectangle.Empty, format),
                AdvencedOptionManager.IsAutoFontSize, AdvencedOptionManager.MinAutoFontSize,
                AdvencedOptionManager.MaxAutoFontSize, AdvencedOptionManager.OverlayUseFontOutline,
                _enableRTL, FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);

        private static OverlayLayoutInput[] GetLayoutInputs(List<OverlayRenderBlock> blocks) => blocks.Select(block =>
            new OverlayLayoutInput(block.TargetData, block.TransData.trans, block.SourceRect, block.CaptureRect,
                block.VerticalMode, block.TransData.TitleData,
                string.Join(",", block.TransData.lineDataList.Select(GetSourceLineSize)))).ToArray();

        private static OverlayPathKey GetOverlayPathKey(Graphics g, string text, Font font, Rectangle rect, StringFormat format) =>
            new OverlayPathKey(text, font.FontFamily.Name, font.Style, g.DpiY * font.SizeInPoints / 72f,
                g.DpiX, g.DpiY, rect, format.FormatFlags, format.Alignment, format.LineAlignment, format.Trimming);

        private GraphicsPath GetOverlayTextPath(Graphics g, string text, Font font, Rectangle rect, StringFormat format)
        {
            var key = GetOverlayPathKey(g, text, font, rect, format);
            _textPathCache ??= new Dictionary<OverlayPathKey, GraphicsPath>();
            if(_textPathCache.TryGetValue(key, out var saved)) return (GraphicsPath)saved.Clone();
            var path = new GraphicsPath();
            try
            {
                path.AddString(text, font.FontFamily, (int)font.Style, key.EmSize, rect, format);
                //호출자는 복제본을 소유한다. 캐시 제거 중에도 사용 중인 경로의 수명은 독립적이다.
                if(text.Length <= 2048 && path.PointCount <= 16384)
                {
                    if(_textPathCache.Count >= 128 || _cachedPathPoints + path.PointCount > 65536)
                        ClearOverlayPathCache();
                    _textPathCache.Add(key, (GraphicsPath)path.Clone());
                    _cachedPathPoints += path.PointCount;
                }
                return path;
            }
            catch { path.Dispose(); throw; }
        }

        private static bool NeedsTextElementWrap(string text)
        {
            foreach(char c in text)
            {
                if(c < 128) continue;
                if(char.IsSurrogate(c) || c == '\u200d'
                    || c is >= '\u1100' and <= '\u11ff' or >= '\ua960' and <= '\ua97f' or >= '\ud7b0' and <= '\ud7ff') return true;
                var category = char.GetUnicodeCategory(c);
                if(category == System.Globalization.UnicodeCategory.NonSpacingMark
                    || category == System.Globalization.UnicodeCategory.SpacingCombiningMark
                    || category == System.Globalization.UnicodeCategory.EnclosingMark) return true;
            }
            return false;
        }
        private void ClearOverlayPathCache()
        {
            if(_textPathCache != null)
            {
                foreach(var path in _textPathCache.Values) path.Dispose();
                _textPathCache.Clear();
            }
            _cachedPathPoints = 0;
        }

        private void ClearOverlayRenderCaches()
        {
            _layoutInputs = null;
            _layoutResults = null;
            ClearOverlayPathCache();
            _fitSizeCache?.Clear();
            _wrapCache?.Clear();
            _textFitCache?.Clear();
        }

        //BeginInvoke 내부 큐를 계속 채우지 않는다. 입력 처리 후 WM_TIMER에서 최신 상태만 그린다.
        private void RequestOverlayPaint()
        {
            if(IsDisposed || isDestroyFormFlag || !IsHandleCreated || _forceLock) return;
            _paintScheduler?.Request();
        }
    }
}
