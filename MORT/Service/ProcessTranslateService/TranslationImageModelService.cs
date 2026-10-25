using MORT.Manager;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using static MORT.Form1;

namespace MORT.Service.ProcessTranslateService
{
    internal sealed class TranslationImageModelService
    {
        private readonly Func<bool> _isStopRequested;
        private readonly Action _requestStop;

        public TranslationImageModelService(Func<bool> isStopRequested, Action requestStop)
        {
            _isStopRequested = isStopRequested;
            _requestStop = requestStop;
        }

        public void CreateModels(
            int ocrAreaCount,
            List<ImgData> imgDataList,
            ref int positionX,
            ref int positionY,
            bool useAttachedCapture,
            bool requireOriginalScreen)
        {
            requireOriginalScreen |= AdvencedOptionManager.OverlayOutputMode == Model.OverlayBlurMode.Restore;
            if(useAttachedCapture)
            {
                CreateModelsFromCapture(
                    ocrAreaCount,
                    imgDataList,
                    ref positionX,
                    ref positionY,
                    requireOriginalScreen);
                return;
            }

            CreateModelsFromScreen(
                ocrAreaCount,
                imgDataList,
                ref positionX,
                ref positionY,
                requireOriginalScreen);
        }

        public void GetImageBytesFromCapture(
            ref byte[] byteData,
            ref int width,
            ref int height,
            ref int positionX,
            ref int positionY)
        {
            FormManager.Instace.screenCaptureUI?.DoPrepare();

            while(true)
            {
                if(FormManager.Instace.screenCaptureUI == null)
                {
                    _requestStop();
                    return;
                }

                FormManager.Instace.screenCaptureUI.DoCapture();
                bool isSuccess = FormManager.Instace.screenCaptureUI.GetData(
                    ref byteData,
                    ref width,
                    ref height,
                    ref positionX,
                    ref positionY);
                if(_isStopRequested())
                {
                    return;
                }

                if(isSuccess)
                {
                    return;
                }

                Thread.Sleep(2);
            }
        }

        private void CreateModelsFromCapture(
            int ocrAreaCount,
            List<ImgData> imgDataList,
            ref int positionX,
            ref int positionY,
            bool requireOriginalScreen)
        {
            byte[] byteData = null;
            int width = 0;
            int height = 0;
            GetImageBytesFromCapture(ref byteData, ref width, ref height, ref positionX, ref positionY);
            if(byteData == null || byteData.Length == 0)
            {
                return;
            }

            for(int index = 0; index < ocrAreaCount; index++)
            {
                int x = 15;
                int y = 0;
                int channels = 4;
                IntPtr data = ProcessGetImgDataFromByte(
                    index,
                    width,
                    height,
                    positionX,
                    positionY,
                    byteData,
                    ref x,
                    ref y,
                    ref channels,
                    false);
                if(data == IntPtr.Zero)
                {
                    continue;
                }

                ImgData imgData = CopyImageData(data, x, y, channels, index);
                imgDataList.Add(imgData);
                if(requireOriginalScreen)
                {
                    CopyOriginalImageFromCapture(
                        imgData,
                        index,
                        width,
                        height,
                        positionX,
                        positionY,
                        byteData);
                }
            }
        }

        private static void CreateModelsFromScreen(
            int ocrAreaCount,
            List<ImgData> imgDataList,
            ref int positionX,
            ref int positionY,
            bool requireOriginalScreen)
        {
            for(int index = 0; index < ocrAreaCount; index++)
            {
                int x = 15;
                int y = 0;
                int channels = 4;
                IntPtr data = processGetImgData(
                    index,
                    ref x,
                    ref y,
                    ref channels,
                    ref positionX,
                    ref positionY,
                    false);
                if(data == IntPtr.Zero)
                {
                    continue;
                }

                ImgData imgData = CopyImageData(data, x, y, channels, index, isGdiCapture: true);
                imgDataList.Add(imgData);
                if(requireOriginalScreen)
                {
                    CopyOriginalImageFromScreen(imgData, index, ref positionX, ref positionY);
                }
            }
        }

        private static ImgData CopyImageData(IntPtr source, int width, int height, int channels, int index, bool isGdiCapture = false)
        {
            var data = new byte[width * height * channels];
            Marshal.Copy(source, data, 0, data.Length);
            Marshal.FreeHGlobal(source);
            if(isGdiCapture)
            {
                NormalizeGdiCaptureAlpha(data, channels);
            }
            return new ImgData
            {
                channels = channels,
                data = data,
                x = width,
                y = height,
                index = index
            };
        }

        private static void CopyOriginalImageFromCapture(
            ImgData imgData,
            int index,
            int width,
            int height,
            int positionX,
            int positionY,
            byte[] byteData)
        {
            int originalX = imgData.x;
            int originalY = imgData.y;
            int originalChannels = 4;
            IntPtr original = ProcessGetImgDataFromByte(
                index,
                width,
                height,
                positionX,
                positionY,
                byteData,
                ref originalX,
                ref originalY,
                ref originalChannels,
                true);
            CopyOriginalImage(imgData, original, originalX, originalY, originalChannels);
        }

        private static void CopyOriginalImageFromScreen(
            ImgData imgData,
            int index,
            ref int positionX,
            ref int positionY)
        {
            int originalX = imgData.x;
            int originalY = imgData.y;
            int originalChannels = 4;
            IntPtr original = processGetImgData(
                index,
                ref originalX,
                ref originalY,
                ref originalChannels,
                ref positionX,
                ref positionY,
                true);
            CopyOriginalImage(imgData, original, originalX, originalY, originalChannels, isGdiCapture: true);
        }

        private static void CopyOriginalImage(
            ImgData imgData,
            IntPtr source,
            int width,
            int height,
            int channels,
            bool isGdiCapture = false)
        {
            if(source == IntPtr.Zero)
            {
                return;
            }

            var originalData = new byte[width * height * channels];
            Marshal.Copy(source, originalData, 0, originalData.Length);
            Marshal.FreeHGlobal(source);
            if(isGdiCapture)
            {
                NormalizeGdiCaptureAlpha(originalData, channels);
            }
            imgData.originalX = width;
            imgData.originalY = height;
            imgData.originalChannels = channels;
            imgData.originalData = originalData;
        }

        private static void NormalizeGdiCaptureAlpha(byte[] pixels, int channels)
        {
            //GetDIBits의 32비트 BI_RGB는 BGRA가 아닌 BGRX다. 활성 창/화면 캡처의
            //예약 바이트를 알파로 읽으면 정상 RGB도 투명 픽셀로 제외된다.
            //WGC 및 이미지 입력의 실제 알파는 이 변환을 거치지 않는다.
            if(channels != 4)
            {
                return;
            }

            for(int index = 3; index < pixels.Length; index += 4)
            {
                pixels[index] = 255;
            }
        }
    }
}
