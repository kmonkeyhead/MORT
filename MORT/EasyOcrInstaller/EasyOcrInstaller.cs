using MORT.Manager;
using MORT.Service.PythonService;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT.EasyOcrInstaller
{
    public partial class EasyOcrInstaller : Form
    {
        private List<string> _cudaCommandLineLinst = new List<string>()
        {
            "pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu118",
            "pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu121",
            "pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu124",
            "pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu126",
            //RTX 50 시리즈는 cu128 이상이 필요하다
            "pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu128"
        };

        private Action _closeCallback;
        private PythonModouleService _pythonModouleService;
        private OcrManager _ocrManager;
        private bool _linkedLog;

        private string _commandLine;
        private bool _useCustomCommandLine;

        private bool _locked;

        public EasyOcrInstaller()
        {
            InitializeComponent();
        }

        private void LocalizeComponents()
        {
            btnInstall.LocalizeLabel("Easy OCR Basic Install Button");
            btnEnableGpuInstallStep.LocalizeLabel("Easy OCR Gpu Install Button");
            btnGuide.LocalizeLabel("Easy OCR Install Guide");
            cbForceInstall.LocalizeLabel("Easy OCR Fore Install Check Box");

            rbCuda.LocalizeLabel("Select Cuda");
            rbGpuCommandLine.LocalizeLabel("Select Gpu Command");
            lbCudaVersion.LocalizeLabel("Select Cuda Version");
            lbGpuCommandLine.LocalizeLabel("Input Command Line");
            btnEnableGpuInstall.LocalizeLabel("Install Easy OCR Cuda");

        }

        public void Show(OcrManager ocrManager, PythonModouleService pythonModouleService, Action closeCallback)
        {
            if(_locked) return;

            LocalizeComponents();
            _closeCallback = closeCallback;
            if(!_linkedLog)
            {
                Python.Deployment.Installer.LogMessage += OnUpdateLog;
                _linkedLog = true;
            }

            tbLog.ScrollToCaret();

            _pythonModouleService = pythonModouleService;
            _ocrManager = ocrManager;
            this.Activate();
            this.Show();

            pnMain.Visible = true;
            pnEnableGPU.Visible = false;
            pnLog.Visible = false;
        }

        private bool ShowInstallMessage()
        {
            bool accept = false;
            string message = LocalizeManager.LocalizeManager.GetLocalizeString("Install Easy OCR Warning Message");
            FormManager.ShowTwoButtonPopupMessage("", message, () => accept = true);

            return accept;
        }

        private async Task InstallEasyOcrAsync(bool enableGpu, string commandLine = "")
        {
            if(cbForceInstall.Checked)
            {
                if(!_ocrManager.CheckAvailableUninstallPython())
                {
                    string message = LocalizeManager.LocalizeManager.GetLocalizeString("Unavailable Python Force Delete Message");
                    FormManager.ShowPopupMessage("", message);
                    return;
                }
            }

            if(!ShowInstallMessage())
            {
                return;
            }

            if(cbForceInstall.Checked)
            {
                _pythonModouleService.DeletePip();
            }

            pnMain.Visible = false;
            pnEnableGPU.Visible = false;
            pnLog.Visible = true;

            var installTask = Task.Run(async () =>
            {
                _locked = true;
                this.ControlBox = false;
                try
                {
                    OnUpdateLog(LocalizeManager.LocalizeManager.GetLocalizeString("Start Install Easy OCR"));
                    OnUpdateLog(System.Environment.NewLine);

                    await _ocrManager.PrepareEasyOcrAsync("en", enableGpu, commandLine);

                    OnUpdateLog(System.Environment.NewLine);
                    OnUpdateLog(LocalizeManager.LocalizeManager.GetLocalizeString("End Install Easy OCR"));

                    string message = LocalizeManager.LocalizeManager.GetLocalizeString("Install Easy OCR Complete Message");
                    FormManager.ShowPopupMessage("", message, Close);

                }
                catch(Exception ex)
                {
                    OnUpdateLog(LocalizeManager.LocalizeManager.GetLocalizeString("Failed Install Easy OCR") + ex.ToString());
                }
                _locked = false;
                this.ControlBox = true;
            });
        }

        private void ShowGpuSetting()
        {

            pnMain.Visible = false;
            pnEnableGPU.Visible = true;
            pnLog.Visible = false;
            ShowCudaVersion();

            cbCuda.SelectedIndex = 0;
            SetGpuCommand();
        }

        private void SetGpuCommand()
        {
            if(_useCustomCommandLine)
            {
                _commandLine = tbGpuLine.Text;
            }
            else
            {
                _commandLine = _cudaCommandLineLinst[cbCuda.SelectedIndex];
            }

            _commandLine = _commandLine.Replace("pip3 install ", "");
            _commandLine = _commandLine.Replace("pip install ", "");

            if(_commandLine.IndexOf("pip3") == 0)
            {
                var regex = new Regex("pip3");
                _commandLine = regex.Replace(_commandLine, "pip", 1);
            }
            _commandLine = "\"" + _commandLine + "\"";

        }

        private void OnUpdateLog(string log)
        {
            tbLog.AppendText(log);
            //this.BeginInvoke(() => tbLog.AppendText(log));
        }


        private void EasyOcrInstaller_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(_linkedLog)
            {
                Python.Deployment.Installer.LogMessage -= OnUpdateLog;
                _linkedLog = false;
            }
            _closeCallback?.Invoke();
        }

        private void ShowCudaVersion()
        {
            _useCustomCommandLine = false;
            cbCuda.Enabled = true;
            tbGpuLine.Enabled = false;
        }

        private void ShowGpuCommandLineVerison()
        {
            _useCustomCommandLine = true;
            cbCuda.Enabled = false;
            tbGpuLine.Enabled = true;
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            _ = InstallEasyOcrAsync(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ShowCudaVersion();
        }

        private void rbGpuCommandLine_CheckedChanged(object sender, EventArgs e)
        {
            ShowGpuCommandLineVerison();
        }

        private void btnEnableGpuInstallStep_Click(object sender, EventArgs e)
        {
            ShowGpuSetting();
        }

        private void btnEnableGpuInstall_Click(object sender, EventArgs e)
        {
            SetGpuCommand();
            _ = InstallEasyOcrAsync(true, _commandLine);
        }

        private void btnGuide_Click(object sender, EventArgs e)
        {
            Util.OpenURL("https://blog.naver.com/killkimno/223288271576");
        }
    }
}
