using Python.Deployment;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT.Service.PythonService
{
    public class PythonModouleService
    {
        private string _installPath = Path.GetFullPath(".");
        //3.12.10이 embed zip이 제공되는 마지막 3.12 버전이다. 배포 폴더에 같은 이름의 zip이 있어야 한다
        private string _embedPythonFile = "python-3.12.10-embed-amd64.zip";
        private string _embedPython = "python-3.12.10-embed-amd64";
        private string _pythonDll = "python312.dll";
        private bool _ininted;
        public bool Ininted { get { return _ininted;} }

        public async Task InitAsync()
        {
            if(_ininted)
            {
                return;
            }

            try
            {
                Runtime.PythonDLL = _installPath + $"\\{_embedPython}\\{_pythonDll}";
                Python.Deployment.Installer.Source = new Python.Deployment.Installer.EmbeddedResourceInstallationSource()
                {
                    Assembly = typeof(Program).Assembly,
                    ResourceName = _embedPythonFile,
                };

                Python.Deployment.Installer.InstallPath = _installPath;

                //Console.WriteLine(PythonEngine.PythonPath);


                Installer.InstallPath = _installPath;
                Python.Deployment.Installer.LogMessage += Console.WriteLine;
                await Python.Deployment.Installer.SetupPython();
                await Installer.TryInstallPip();
                 PythonEngine.Initialize();
                var _threadState = PythonEngine.BeginAllowThreads();
                _ininted = true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                Python.Deployment.Installer.LogMessage -= Console.WriteLine;
            }         
           
        }

        public void DeletePip()
        {
            string path = _installPath + $"\\{_embedPython}";
            if (Path.Exists(path))
            {
                Console.WriteLine("Exist");
                Directory.Delete(path, true);
            }
        }

        public async Task InstallModouleAsync(string modoule, bool force = false)
        {
            if(!_ininted)
            {
                return;
            }

            await Installer.PipInstallModule(modoule, force:force);
        }

        public bool IsPipInstalled()
        {
            string path = _installPath + $"\\{_embedPython}";

            if (!Path.Exists(path))
            {
                return false;
            }

            return true;
        }

        public bool IsInstalled(string modoule)
        {
            string path = _installPath + $"\\{_embedPython}";

            if (!Path.Exists(path))
            {
                return false;
            }

            if (!Installer.IsModuleInstalled(modoule))
            {
                return false;
            }
            
            return true;
        }

    }
}
