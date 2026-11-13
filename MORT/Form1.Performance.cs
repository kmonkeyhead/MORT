using MORT.Service.Overlay;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MORT
{
    public partial class Form1
    {
        private void InitializeOverlayPerformanceOption()
        {
            var option = new CheckBox
            {
                Name = "cbOverlayPerformance", AutoSize = true, Location = new Point(14, 298),
                ForeColor = Color.White, Text = "오버레이 성능 측정 (체크 해제 시 종료)"
            };
            var status = new Label
            {
                AutoSize = false, Location = new Point(14, 326), Size = new Size(475, 65),
                ForeColor = Color.White, Text = "저장 위치: UserData/Debug/OverlayPerformance"
            };
            option.CheckedChanged += (_, _) =>
            {
                if(option.Checked)
                {
                    try
                    {
                        OverlayPerformanceProfiler.Start($"OCR={MySettingManager.OCRType}\nAttachedCapture={MySettingManager.IsUseAttachedCapture}\nSkin={MySettingManager.NowSkin}\nOutputMode={AdvencedOptionManager.OverlayOutputMode}\nTargetFps={AdvencedOptionManager.OverlayTargetFps}\nRestoreLevel={AdvencedOptionManager.OverlayRestoreLevel}");
                        status.Text = "측정 중: " + OverlayPerformanceProfiler.LastDirectory;
                    }
                    catch(Exception ex) { option.Checked = false; status.Text = "측정 시작 실패: " + ex.Message; }
                }
                else
                {
                    OverlayPerformanceProfiler.Stop();
                    status.Text = OverlayPerformanceProfiler.LastError == null
                        ? "저장 완료: " + OverlayPerformanceProfiler.LastDirectory
                        : "측정 오류: " + OverlayPerformanceProfiler.LastError;
                }
            };
            plDebugOn.Controls.Add(option);
            plDebugOn.Controls.Add(status);
            Disposed += (_, _) => OverlayPerformanceProfiler.Stop();
        }
    }
}
