using MORT.Model;
using MORT.Service.Overlay;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace MORT
{
    public partial class TransFormOver
    {
        private OverlayBackdropWindow _windowBlur;
        private readonly List<Rectangle> _windowBlurRects = new();
        private readonly Dictionary<int, (OverlayBlurFrame Frame, Bitmap Image)> _captureBlurImages = new();

        public bool RefreshOverlayOutput()
        {
            StopAdaptiveBackground();
            _windowBlur?.Dispose();
            _windowBlur = null;
            _windowBlurRects.Clear();
            ClearCaptureBlurImages();
            if(AdvencedOptionManager.OverlayOutputMode == OverlayBlurMode.Window)
            {
                _windowBlur = new OverlayBackdropWindow();
                if(!_windowBlur.TryInitialize())
                {
                    UpdatePaint();
                    return false;
                }
            }
            UpdatePaint();
            return true;
        }

        private bool BeginWindowBlur()
        {
            _windowBlurRects.Clear();
            if(!_isStart || !Visible || AdvencedOptionManager.OverlayOutputMode != OverlayBlurMode.Window)
            {
                _windowBlur?.Hide();
                return false;
            }
            _windowBlur ??= new OverlayBackdropWindow();
            return _windowBlur.TryInitialize();
        }

        private void PresentWindowBlur()
        {
            if(_windowBlur == null) return;
            if(!_isStart || !Visible || AdvencedOptionManager.OverlayOutputMode != OverlayBlurMode.Window)
            {
                _windowBlur.Hide();
                return;
            }
            try
            {
                _windowBlur.Update(Handle, Bounds, _windowBlurRects, !_attchedCapture && !_forceLock, AdvencedOptionManager.OverlayBlurLevel);
            }
            catch(Exception ex)
            {
                _windowBlur.Disable();
                _windowBlurRects.Clear();
                Util.ShowLog($"Overlay window blur failed: {ex.Message}");
                if(IsHandleCreated && !IsDisposed) BeginInvoke(new Action(UpdatePaint));
            }
        }

        private HashSet<int> DrawLiveRestoredBackgrounds(Graphics graphics, bool hasDisplayedText)
        {
            var drawn = new HashSet<int>();
            if(!hasDisplayedText) return drawn;
            //한 페인트 안에서는 모든 영역이 같은 완성 결과를 사용한다.
            var live = _adaptiveRefresh?.Latest;
            if(_adaptiveScene == null || live == null) return drawn;

            Point offset = new Point(-Location.X, -Location.Y);
            foreach(var area in _adaptiveScene.Areas)
            {
                if(!live.Frames.TryGetValue(area.Index, out var frame)) continue;
                var completedArea = live.Scene.Areas.First(oldArea => oldArea.Index == area.Index);
                if(!SameAdaptiveArea(area, completedArea)) continue;
                Rectangle imageRect = area.ScreenRect;
                imageRect.Offset(offset);
                Rectangle clip = Rectangle.Intersect(ClientRectangle, imageRect);
                if(clip.Width <= 0 || clip.Height <= 0) continue;
                DrawRestoredBackground(graphics, area.Index, frame, imageRect, clip);
                drawn.Add(area.Index);
            }
            return drawn;
        }

        private void DrawSnapshotRestoredBackgrounds(Graphics graphics, List<OverlayRenderBlock> blocks, HashSet<int> drawn)
        {
            if(_allowLiveAdaptive || !_isStart || AdvencedOptionManager.OverlayOutputMode != OverlayBlurMode.Restore) return;
            //영역별 패치를 먼저 한 번만 그린다. 뒤쪽 블록의 배경이 앞쪽 번역문을 덮으면 안 된다.
            foreach(var block in blocks)
            {
                OverlayBlurFrame frame = block.TargetData.BlurFrame;
                Rectangle imageRect = block.CaptureImageRect;
                if(frame == null || frame.Mode != OverlayBlurMode.Restore
                    || imageRect.Width <= 0 || imageRect.Height <= 0
                    || !drawn.Add(block.TargetData.Index)) continue;
                DrawRestoredBackground(graphics, block.TargetData.Index, frame, imageRect, block.CaptureRect);
            }
        }

        private void DrawRestoredBackground(Graphics graphics, int index, OverlayBlurFrame frame, Rectangle imageRect, Rectangle clip)
        {
            Bitmap image = GetCaptureBackgroundImage(index, frame);
            GraphicsState saved = graphics.Save();
            try
            {
                graphics.SetClip(clip, CombineMode.Intersect);
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.PixelOffsetMode = PixelOffsetMode.Half;
                RectangleF destination = imageRect;
                if(frame.SourceRect.Width > 0 && frame.SourceRect.Height > 0 && frame.SourceSize.Width > 0 && frame.SourceSize.Height > 0)
                {
                    float sx = destination.Width / frame.SourceSize.Width, sy = destination.Height / frame.SourceSize.Height;
                    destination = new RectangleF(destination.X + frame.SourceRect.X * sx, destination.Y + frame.SourceRect.Y * sy,
                        frame.SourceRect.Width * sx, frame.SourceRect.Height * sy);
                }
                graphics.DrawImage(image, destination, new RectangleF(0, 0, frame.Width, frame.Height), GraphicsUnit.Pixel);
            }
            finally { graphics.Restore(saved); }
        }

        private Bitmap GetCaptureBackgroundImage(int index, OverlayBlurFrame frame)
        {
            if(!_captureBlurImages.TryGetValue(index, out var cached) || !ReferenceEquals(cached.Frame, frame))
            {
                var image = cached.Image;
                _captureBlurImages.Remove(index);
                if(image == null || image.Width != frame.Width || image.Height != frame.Height)
                {
                    image?.Dispose();
                    image = new Bitmap(frame.Width, frame.Height, PixelFormat.Format32bppPArgb);
                }
                try
                {
                    BitmapData locked = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
                    try
                    {
                        for(int y = 0; y < image.Height; y++)
                            Marshal.Copy(frame.Pixels, y * image.Width * 4, locked.Scan0 + y * locked.Stride, image.Width * 4);
                    }
                    finally { image.UnlockBits(locked); }
                }
                catch { image.Dispose(); throw; }
                cached = (frame, image);
                _captureBlurImages[index] = cached;
            }

            return cached.Image;
        }

        private List<Rectangle> GetBlurTextRegions(Graphics graphics, OverlayRenderBlock block, Font font, StringFormat format)
        {
            var regions = new List<Rectangle>();
            void Add(Rectangle rect)
            {
                if(rect.Width <= 0 || rect.Height <= 0) return;
                //페더와 둥근 모서리는 원문(외곽선 포함) 밖에 둔다.
                //ViewRect로 잘라 버리면 좁은 줄의 원문 끝이 다시 비쳐 보인다.
                int padding = Math.Clamp(Math.Min(rect.Width, rect.Height) / 6 + 2, 4, 10) + 6;
                rect.Inflate(padding, padding);
                rect = Rectangle.Intersect(rect, block.CaptureRect);
                if(rect.Width <= 0 || rect.Height <= 0 || regions.Any(existing => existing.Contains(rect))) return;
                regions.RemoveAll(existing => rect.Contains(existing));
                regions.Add(rect);
            }

            //원문은 OCR 줄 위치, 번역문은 실제 AddString 잉크 위치를 쓴다.
            //확장된 문단 사각형의 빈 여백을 블러로 채우지 않는다.
            double zoom = Math.Max(0.01, FormManager.Instace.MyMainForm.MySettingManager.ImgZoomSize);
            foreach(var line in block.TransData.lineDataList)
            {
                Rectangle rect = line.lineRect;
                if(rect.Width <= 0 || rect.Height <= 0) continue;
                Add(Rectangle.FromLTRB(
                    block.CaptureImageRect.X + (int)Math.Floor(rect.Left / zoom),
                    block.CaptureImageRect.Y + (int)Math.Floor(rect.Top / zoom),
                    block.CaptureImageRect.X + (int)Math.Ceiling(rect.Right / zoom),
                    block.CaptureImageRect.Y + (int)Math.Ceiling(rect.Bottom / zoom)));
            }

            if(block.ContentRect.Width <= 0 || block.ContentRect.Height <= 0) return regions;
            var lines = GetWrappedLinesByAddString(graphics, block.TransData.trans, font,
                block.ContentRect.Width, block.ContentRect.Height, format, block.VerticalMode);
            float advance = font.GetHeight(graphics) * 1.2f;
            for(int index = 0; index < lines.Count; index++)
            {
                if(string.IsNullOrWhiteSpace(lines[index])) continue;
                Rectangle slot = Rectangle.Intersect(GetLineRect(block.ContentRect, index, advance, block.VerticalMode), block.ContentRect);
                if(slot.Width <= 0 || slot.Height <= 0) continue;
                using var path = new GraphicsPath();
                path.AddString(lines[index], font.FontFamily, (int)font.Style, graphics.DpiY / 72f * font.SizeInPoints, slot, format);
                if(path.PointCount > 0) Add(Rectangle.Ceiling(path.GetBounds()));
            }
            return regions;
        }

        private void TrimCaptureBlurImages(HashSet<int> active)
        {
            foreach(int key in _captureBlurImages.Keys.ToArray())
            {
                if(!active.Contains(key))
                {
                    _captureBlurImages[key].Image.Dispose();
                    _captureBlurImages.Remove(key);
                }
            }
        }

        private void ClearCaptureBlurImages()
        {
            foreach(var cached in _captureBlurImages.Values) cached.Image.Dispose();
            _captureBlurImages.Clear();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if(!Visible)
            {
                StopAdaptiveBackground();
                _windowBlur?.Hide();
            }
            else if(_isStart && IsHandleCreated && AdvencedOptionManager.OverlayOutputMode != OverlayBlurMode.None)
            {
                BeginInvoke(new Action(UpdatePaint));
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            //다음 페인트에서 새 좌표를 게시한다. 그때까지 완성 배경의 화면 좌표를 유지한다.
            if(IsHandleCreated) RequestOverlayPaint();

            if(IsHandleCreated && _windowBlurRects != null) PresentWindowBlur();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DisposeAdaptiveBackground();
            _paintScheduler?.Dispose();
            _paintScheduler = null;
            ClearOverlayRenderCaches();
            _windowBlur?.Dispose();
            _windowBlur = null;
            ClearCaptureBlurImages();
            base.OnHandleDestroyed(e);
        }
    }
}
