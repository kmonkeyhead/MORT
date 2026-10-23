using Microsoft.VisualBasic.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public partial class Updater : Form
    {
        private string newVersion = "";
        private string url = "";
        private string info = "";
        private string expectedHash = "";

        private static readonly HttpClient httpClient = new HttpClient();

        private string _prepare = "Preparing for download";
        private string _downloadError = "Error has occurred! Please manually update";
        private string _downloadComplete = "update completed! Run Mort again";
        private string _completeMessage = "The update has been completed. Would you like to check your update history?";
        private string _completeTitle = "update completed!";
        private string _errorMessage = "Failed to update. Please update Would you like to go to the download page?";
        private string _errorTitle = "Update failure!";
        private string _downloadConfig = "Settings file download";
        private string _downloading = "downloading";
        private string _hashMismatch = "Update file is corrupted or tampered. Please try again later.";


        //다운로드 URL은 외부 version.txt에서 전달되므로 평문 http일 수 있다. 실행 파일을 받기 전 https로 강제한다(MITM 방지)
        private static string ForceHttps(string url)
        {
            if (!string.IsNullOrEmpty(url) && url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                return "https://" + url.Substring("http://".Length);
            }
            return url;
        }

        public void OpenURL(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {

            }

        }

        public Updater(string newVersion, string url, string info, string localize, string expectedHash = "")
        {
            InitializeComponent();
            this.newVersion = newVersion;
            this.url = ForceHttps(url);
            this.info = info;
            this.expectedHash = (expectedHash ?? "").Trim();
            ParseLocalize(localize);
        }

        private void ParseLocalize(string localize)
        {
            try
            {
                var keys = localize.Split('_');


                foreach (string arg in keys)
                {
                    Console.WriteLine(arg);
                }

                string prepare = keys[0];
                string downloadError = keys[1];
                string downloadComplete = keys[2];
                string completeMessage = keys[3];
                string completeTitle = keys[4];
                string errorMessage = keys[5];
                string errorTitle = keys[6];
                string downloadConfig = keys[7];
                string downloading = keys[8];

                _prepare = prepare;
                _downloadError = downloadError;
                _downloadComplete = downloadComplete;
                _completeMessage = completeMessage;
                _completeTitle = completeTitle;
                _errorMessage = errorMessage;
                _downloadConfig = downloadConfig;
                _downloading = downloading;
            }
            catch (Exception e)
            {
                _downloading = "에러 떴음 " + e.ToString();
            }

            //keys[9] (HashMismatch) - 구 호환: 길이 부족하거나 빈 문자열이면 영어 디폴트 유지
            try
            {
                var extra = localize.Split('_');
                if (extra.Length > 9 && !string.IsNullOrWhiteSpace(extra[9]))
                {
                    _hashMismatch = extra[9];
                }
            }
            catch
            {
            }
        }

        //exe -> config 순서로 받은 뒤 AfterAsync로 넘긴다. 어느 쪽이든 실패하면 에러로 처리한다.
        public async void DoDownload()
        {
            bool isError = false;

            try
            {
                lbStatus.Text = _prepare;
                await DownloadFileAsync(url, "MORT_backup.exe");

                lbStatus.Text = _downloadConfig;
                string configUrl = url.Replace("MORT.exe", "MORT.dll.config");
                await DownloadFileAsync(configUrl, "MORT_backup.dll.config");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                isError = true;
            }

            DoDownloadAfter(isError);
        }

        //받은 크기만큼 진행률을 갱신하며 파일로 저장한다. 큰 exe도 받으므로 본문은 스트림으로 읽는다.
        private async Task DownloadFileAsync(string fileUrl, string filePath)
        {
            using (HttpResponseMessage response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                long totalBytes = response.Content.Headers.ContentLength ?? -1;
                long receivedBytes = 0;
                int lastPercent = -1;
                byte[] buffer = new byte[81920];

                using (Stream source = await response.Content.ReadAsStreamAsync())
                using (FileStream target = File.Create(filePath))
                {
                    int read;
                    while ((read = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await target.WriteAsync(buffer, 0, read);
                        receivedBytes += read;

                        int percent = totalBytes > 0 ? (int)(receivedBytes * 100 / totalBytes) : 0;
                        if (percent != lastPercent)
                        {
                            lastPercent = percent;
                            OnDownloadProgress(receivedBytes, totalBytes, percent);
                        }
                    }
                }
            }
        }

        //다운받은 파일의 SHA256을 계산해 hex 대문자로 반환
        private static string ComputeSha256(string filePath)
        {
            using (var sha = SHA256.Create())
            using (var fs = File.OpenRead(filePath))
            {
                byte[] hash = sha.ComputeHash(fs);
                var sb = new System.Text.StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        //MORT_backup.exe를 expectedHash와 비교한다. 검증 불가/불일치 시 다운로드 파일 삭제 + true 반환(isError 처리).
        //해시 검증은 필수다(fail-closed): expectedHash가 비어 있으면 무결성 보장이 안 되므로 성공이 아니라 에러로 처리한다.
        private bool VerifyDownloadedHash()
        {
            const string downloadFile = "MORT_backup.exe";

            if (string.IsNullOrEmpty(expectedHash))
            {
                Console.WriteLine("expectedHash empty -> cannot verify integrity, treating as error");
                try { File.Delete(downloadFile); } catch { }
                try { File.Delete("MORT_backup.dll.config"); } catch { }
                WriteHashFailCooldown();
                lbStatus.Text = _hashMismatch;
                return true;
            }

            if (!File.Exists(downloadFile))
            {
                Console.WriteLine("Downloaded file not found: " + downloadFile);
                return true;
            }

            string actual = ComputeSha256(downloadFile);
            if (!string.Equals(actual, expectedHash, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Hash mismatch. expected={expectedHash} actual={actual}");
                try { File.Delete(downloadFile); } catch { }
                try { File.Delete("MORT_backup.dll.config"); } catch { }
                WriteHashFailCooldown();
                lbStatus.Text = _hashMismatch;
                return true;
            }

            Console.WriteLine("Hash verified: " + actual);
            return false;
        }

        //해시 불일치 발생 시각(UTC ticks)을 기록한다. MORT가 이 파일을 읽어 10분간 업데이트 재진입을 막는다(손상/변조 파일 재다운로드 루프 방지).
        //Updater는 MORT(Util/GlobalDefine)를 참조하지 않으므로 경로/키/포맷을 MORT 쪽과 동일하게 유지할 것.
        //(GlobalDefine.UPDATE_HASH_FAIL_COOLDOWN_FILE / "@HASH_FAIL_TIME [..]")
        private static void WriteHashFailCooldown()
        {
            try
            {
                const string cooldownFile = @"UserData/updateHashFailCooldown.txt";
                string dir = Path.GetDirectoryName(cooldownFile);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(cooldownFile, "@HASH_FAIL_TIME [" + DateTime.UtcNow.Ticks + "]");
            }
            catch (Exception e)
            {
                Console.WriteLine("WriteHashFailCooldown failed: " + e.Message);
            }
        }

        private async Task AfterAsync(bool isError)
        {
            await Task.Delay(TimeSpan.FromSeconds(2.5f));

            if (!isError)
            {
                //무결성 검증 - 실패 시 RemoveOldFile에 진입조차 않음(기존 MORT.exe 보호)
                if (VerifyDownloadedHash())
                {
                    isError = true;
                }
            }

            if (!isError)
            {
                try
                {
                    RemoveOldFile("MORT.exe", "MORT_backup.exe", "MORT_2.exe");
                    RemoveOldFile("MORT.dll.config", "MORT_backup.dll.config", "MORT_2.dll.config");
                }
                catch (Exception excep)
                {
                    Console.WriteLine(excep);
                    isError = true;
                    lbStatus.Text = _downloadError;
                }
            }

            if (!isError)
            {
                lbStatus.Text = _downloadComplete;


                if (DialogResult.OK == MessageBox.Show(_completeMessage, _completeTitle, MessageBoxButtons.OKCancel))
                {
                    try
                    {
                        OpenURL(info);
                    }
                    catch { }
                    DoClose();
                }
                else
                {
                    DoClose();
                }
            }
            else
            {
                // OK: 다운로드 페이지 열기, Cancel: 그냥 종료 - 어느 쪽이든 Updater는 닫는다
                if (DialogResult.OK == MessageBox.Show(_errorMessage, _errorTitle, MessageBoxButtons.OKCancel))
                {
                    try
                    {
                        OpenURL(info);
                    }
                    catch { }
                }

                // UI 스레드에서 종료 (Task.Run 백그라운드 스레드에서 호출되므로 Invoke 필요)
                this.Invoke(() =>
                {
                    if (Application.MessageLoop)
                        Application.Exit();
                    else
                        Environment.Exit(1);
                });
            }
        }

        private void DoDownloadAfter(bool isError)
        {
            var result = Task.Run(async () => await AfterAsync(isError));

        }

        private void RemoveOldFile(string originalFile, string downloadFile, string backupFile)
        {

            if (File.Exists(backupFile))
            {
                File.Delete(backupFile);
                Console.WriteLine("Delete backup - " + backupFile);
            }

            if(WaitForFile(originalFile))
            {
                File.Move(originalFile, backupFile);
                Console.WriteLine("originalFile to back - " + originalFile);
            }
            else
            {
                throw new Exception("File still locked");
            }


            if (WaitForFile(downloadFile))
            {
                File.Move(downloadFile, originalFile);
                Console.WriteLine("downloadFile to originalFile - " + downloadFile);
            }
            else
            {
                throw new Exception("File still locked");
            }        

        }

        private bool WaitForFile(string fullPath)
        {
            int numTries = 0;
            while (true)
            {
                ++numTries;
                try
                {
                    // Attempt to open the file exclusively.
                    using (FileStream fs = new FileStream(fullPath,
                        FileMode.Open, FileAccess.ReadWrite,
                        FileShare.None, 100))
                    {
                        fs.ReadByte();

                        // If we got this far the file is ready
                        break;
                    }
                }
                catch (Exception ex)
                {

                    if (numTries > 10)
                    {
                        return false;
                    }

                    // Wait for the lock to be released
                    System.Threading.Thread.Sleep(500);
                }
            }

            return true;
        }

        private async void DoClose()
        {
            Process.Start("MORT.exe");

            if (Application.MessageLoop)
                Application.Exit();
            else
                Environment.Exit(1);
        }

        private void OnDownloadProgress(long receivedBytes, long totalBytes, int percent)
        {
            // Displays the transfer progress.
            Console.WriteLine("downloaded {0} of {1} bytes. {2} % complete...",
                receivedBytes,
                totalBytes,
                percent);

            lbStatus.Text = _downloading + percent.ToString() + "%";
            progressBar1.Value = percent;

          
        }

        private void Updater_Load(object sender, EventArgs e)
        {
           DoDownload();
        }
    }
}
