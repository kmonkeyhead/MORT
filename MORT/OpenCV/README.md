# OpenCV-derived C# background restoration

This directory compiles into MORT.dll. No OpenCV native DLL, SDK, NuGet wrapper,
C++ toolchain, or separate setup step is required.

The MORT pipeline groups OCR rectangles with 16-pixel context, detects bright
and dark strokes with opening/closing, thresholds at half the Otsu level with
an 8-value noise floor, detects strokes up to two pixels outside the OCR box,
expands the mask by two to six pixels (four by default), and fills the masked
pixels with the radius-2 Navier-Stokes algorithm ported from official OpenCV
4.14.0. Only restored pixels are written to the existing pooled BGRA output.

Morphology.cs applies separable rectangular min/max filters using Vector<byte>
SIMD over contiguous rows, with scalar tails and replicated isolated borders.
NavierStokesInpaint.cs retains the official marching order, stable priority
ties and numerical precision. On SSE4.1-capable CPUs it computes B/G/R weights
in parallel with SIMD; other CPUs use the equivalent scalar path. Its state,
arrival times, result and binary heap
are reused along with the mask/grayscale/intermediate arrays. OCR rectangle
changes do not discard those buffers. Capacity grows geometrically to amortize small
changes. Only growth allocates; frame-dependent
mask/state values are initialized over the active region to prevent stale data.
Instances are serialized by OverlayTextRestorationService; separate workers do
not share writable buffers. Dispose releases managed buffer references.

The supported subset is byte BGR/BGRA input and radius-2 NS restoration, not a
complete OpenCV API. OpenCV notices are copied to build/publish output. No
implementation from the previously evaluated translator is included.

## Background coverage

The background-quality setting keeps its serialized key and original values:
0 = low quality, 1 = high quality (default). Previously saved experimental
values 2/3/4 are read as high quality. The high-quality dilation radius is
4 pixels at background strength 3. These are processing-resolution pixels; the
existing downsampling/compositor maps them to the original capture size.
The NS radius remains 2 and the threshold is unchanged.

Input/comparison padding is shared at 16 pixels and covers both the two-pass
morphology support (2 * 7 + 2) and the mask plus repair margin
(2 + 6 + 6). Detection reads the full morphology support, and inpainting gets
six pixels of context outside the mask. The comparison and downsample regions
therefore include every source pixel used by restoration. The previous complete
output is retained until the new result is ready. Scratch/output buffers are reused.

Background processing strength has five stages, saved as @OverlayBackgroundStrength.
The default is stage 3. MORT's HighQualityBackgroundRestoration selects the mask
dilation radius from its preset table (2/3/4/5/6 pixels) and passes that pixel value
to BackgroundRestorer. The OpenCV processing layer does not interpret user stages.
Stages 1/2/3/4 match the former stages 2/3/4/5, and new stage 5 uses six
pixels. Low-quality stage values are unchanged; it adjusts its previous OCR-box
padding by two pixels per stage above/below 3 (at least one pixel).
The old @OverlayRestoreLevel setting is not reused: it had a different scale.
Strength changes invalidate the adaptive scene and low-quality mask/input-region
caches. Old mask spans are cleared using their old strength; buffers remain reusable.
