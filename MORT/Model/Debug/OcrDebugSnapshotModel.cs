using System.Collections.Generic;
using System.Drawing;

namespace MORT.Model.Debug
{
    /// <summary>
    /// 이미지 분석 결과를 나중에 다시 확인하기 위한 디버깅 스냅샷 모델.
    /// 저장 전용이며 다시 읽어들이지 않기 때문에 키 이름은 호환성 제약이 없다.
    /// </summary>
    public record OcrDebugRect
    {
        public int X { get; init; }
        public int Y { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public int Right { get; init; }
        public int Bottom { get; init; }

        public static OcrDebugRect From(Rectangle rect)
        {
            return new OcrDebugRect
            {
                X = rect.X,
                Y = rect.Y,
                Width = rect.Width,
                Height = rect.Height,
                Right = rect.Right,
                Bottom = rect.Bottom,
            };
        }
    }

    public record OcrDebugWord
    {
        public string Text { get; init; } = "";
        public OcrDebugRect Rect { get; init; }
    }

    public record OcrDebugLine
    {
        public int GroupIndex { get; init; }
        public string LineString { get; init; } = "";
        public string TransString { get; init; } = "";
        public string AngleType { get; init; } = "";
        public OcrDebugRect LineRect { get; init; }
        public List<OcrDebugWord> Words { get; init; } = new();
        public List<string> TransWords { get; init; } = new();
    }

    /// <summary>
    /// 오버레이가 하나의 블록으로 그리는 번역 문장 단위.
    /// </summary>
    public record OcrDebugTransBlock
    {
        public int Index { get; init; }
        public string Trans { get; init; } = "";
        public bool IsTitle { get; init; }
        public string AngleType { get; init; } = "";
        public OcrDebugRect LineRect { get; init; }
        public OcrDebugRect SourceRect { get; init; }
        public OcrDebugRect ViewRect { get; init; }
        public OcrDebugRect ContentRect { get; init; }
        public List<OcrDebugLine> Lines { get; init; } = new();
    }

    public record OcrDebugAutoColor
    {
        public string Font { get; init; } = "";
        public string Background { get; init; } = "";
    }

    /// <summary>
    /// OCR 영역 하나의 인식 결과.
    /// </summary>
    public record OcrDebugArea
    {
        public int Index { get; init; }
        public bool SnapShot { get; init; }
        /// <summary>화면 좌표 기준 OCR 영역.</summary>
        public OcrDebugRect AreaRect { get; init; }
        /// <summary>캡쳐 이미지 좌표 기준 인식 결과 전체 영역.</summary>
        public OcrDebugRect ResultRect { get; init; }
        public string OcrText { get; init; } = "";
        public string TransText { get; init; } = "";
        public bool UseAutoColor { get; init; }
        public List<OcrDebugAutoColor> AutoColors { get; init; } = new();
        public List<OcrDebugLine> Lines { get; init; } = new();
        public List<OcrDebugTransBlock> TransBlocks { get; init; } = new();
    }

    /// <summary>
    /// 오버레이가 실제로 그린 블록 하나의 최종값.
    /// </summary>
    public record OcrDebugOverlayBlock
    {
        public int AreaIndex { get; init; }
        public int ColorIndex { get; init; }
        public string Text { get; init; } = "";
        public bool IsTitle { get; init; }
        public bool VerticalMode { get; init; }

        /// <summary>이하 모두 오버레이 폼 클라이언트 좌표.</summary>
        public OcrDebugRect CaptureRect { get; init; }
        public OcrDebugRect SourceRect { get; init; }
        public OcrDebugRect ViewRect { get; init; }
        public OcrDebugRect ContentRect { get; init; }

        public string FontFamily { get; init; } = "";
        public string FontStyle { get; init; } = "";
        /// <summary>실제로 그린 폰트 크기(pt).</summary>
        public float FontSize { get; init; }
        /// <summary>자동 크기 계산의 목표값(pt).</summary>
        public float PreferredFontSize { get; init; }
        public float MinimumFontSize { get; init; }
        /// <summary>원문에서 추정한 폰트 크기(pt).</summary>
        public float SourceFontSize { get; init; }

        public string FontColor { get; init; } = "";
        public string BackgroundColor { get; init; } = "";
        public bool DrawBackground { get; init; }
        public bool UseAutoColor { get; init; }
        public bool ContrastCorrected { get; init; }
        public bool UseOutline { get; init; }
        public string OutlineColor1 { get; init; } = "";
        public string OutlineColor2 { get; init; } = "";

        /// <summary>줄바꿈까지 끝난 최종 출력 줄.</summary>
        public List<string> WrappedLines { get; init; } = new();
        public float LineAdvance { get; init; }
        /// <summary>최소 폰트로도 들어가지 않아 잘린 상태.</summary>
        public bool Clipped { get; init; }
    }

    /// <summary>
    /// 페인트 한 번의 구간별 시간. 오버레이가 UI 스레드를 얼마나 잡고 있는지 확인용.
    /// </summary>
    public record OcrDebugPaintTiming
    {
        /// <summary>DoUpdatePaint 전체.</summary>
        public double TotalMs { get; init; }
        /// <summary>창 크기·위치 계산.</summary>
        public double CheckSizeMs { get; init; }
        /// <summary>레이아웃·폰트 탐색·드로잉(AddText).</summary>
        public double LayoutAndDrawMs { get; init; }
        /// <summary>GetHbitmap + UpdateLayeredWindow.</summary>
        public double PresentMs { get; init; }
        /// <summary>이 페인트에서 텍스트 측정을 재사용한 횟수.</summary>
        public int MeasureCacheHit { get; init; }
        /// <summary>실제로 GDI+ 측정을 돌린 횟수.</summary>
        public int MeasureCacheMiss { get; init; }
    }

    public record OcrDebugOverlayInfo
    {
        public string BlurMode { get; init; } = "None";
        public OcrDebugPaintTiming Timing { get; init; }
        public OcrDebugRect FormRect { get; init; }
        public bool IsAutoFontSize { get; init; }
        public int MinAutoFontSize { get; init; }
        public int MaxAutoFontSize { get; init; }
        public bool KeepSourceDirection { get; init; }
        public bool UseFontOutline { get; init; }
        public bool AutoColor { get; init; }
        public bool AutoBackgroundColor { get; init; }
        public bool AutoFontColor { get; init; }
        public bool UseBackColor { get; init; }
        public List<OcrDebugOverlayBlock> Blocks { get; init; } = new();
    }

    public record OcrDebugSnapshotModel
    {
        public string CapturedAt { get; init; } = "";
        public string Skin { get; init; } = "";
        public string OcrType { get; init; } = "";
        public string TransType { get; init; } = "";
        public string OcrText { get; init; } = "";
        public string TransText { get; init; } = "";
        public List<OcrDebugArea> Areas { get; init; } = new();
        /// <summary>오버레이 스킨일 때만 채워진다.</summary>
        public OcrDebugOverlayInfo Overlay { get; init; }
    }
}
