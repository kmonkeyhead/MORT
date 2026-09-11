using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MORT.Service.ChromeBridge
{
    public sealed record ChromeModelEntry(string Name, string Version, long Bytes, string Path);

    public sealed record ChromeModelSnapshot(ChromeModelEntry? Llm, List<ChromeModelEntry> Packs, string PackRoot)
    {
        public static readonly ChromeModelSnapshot Empty
            = new ChromeModelSnapshot(null, new List<ChromeModelEntry>(), "");
    }

    /// <summary>
    /// 크롬이 받아 둔 모델을 디스크에서 읽는다.
    ///
    /// 브릿지 페이지는 웹페이지라 파일을 볼 수 없고, JS API에도 모델 이름을 알려 주는 값이 없다
    /// (LanguageModel.params() 는 topK/temperature 만 준다). 그래서 MORT가 대신 읽어 넘긴다.
    ///
    /// 크롬 기본 사용자 데이터 경로만 본다. 다른 곳에 설치했거나 프로필 경로를 바꿨으면 못 찾는데,
    /// 그때는 목록을 비워서 보낸다. 없다고 번역이 안 되는 것은 아니므로 실패로 다루지 않는다.
    /// </summary>
    public static class ChromeModelInfo
    {
        public static ChromeModelSnapshot Read()
        {
            try
            {
                string userData = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Google", "Chrome", "User Data");

                if (!Directory.Exists(userData))
                {
                    return ChromeModelSnapshot.Empty;
                }

                return new ChromeModelSnapshot(ReadLlm(userData), ReadPacks(userData),
                    Path.Combine(userData, "TranslateKit", "models"));
            }
            catch (Exception ex)
            {
                Util.ShowLog($"[ChromeBridge] 모델 정보를 읽지 못했습니다 : {ex.Message}");
                return ChromeModelSnapshot.Empty;
            }
        }

        /// <summary>
        /// LLM(Prompt API)이 쓰는 기기 내장 모델. manifest.json 의 BaseModelSpec 에 실제 모델 이름이 있다.
        /// 예: name "v3Nano" (Gemini Nano 3세대). 크롬이 모델을 갈아끼우면 이 값이 바뀐다.
        /// </summary>
        private static ChromeModelEntry? ReadLlm(string userData)
        {
            string root = Path.Combine(userData, "OptGuideOnDeviceModel");

            if (!Directory.Exists(root))
            {
                return null;
            }

            //버전 폴더가 여럿이면 가장 최근 것을 본다.
            DirectoryInfo? latest = new DirectoryInfo(root).GetDirectories()
                .OrderByDescending(d => d.CreationTimeUtc)
                .FirstOrDefault();

            if (latest == null)
            {
                return null;
            }

            string name = latest.Name;
            string version = latest.Name;
            string manifest = Path.Combine(latest.FullName, "manifest.json");

            if (File.Exists(manifest))
            {
                try
                {
                    using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(manifest)))
                    {
                        JsonElement spec;

                        if (document.RootElement.TryGetProperty("BaseModelSpec", out spec))
                        {
                            JsonElement value;

                            if (spec.TryGetProperty("name", out value) && value.ValueKind == JsonValueKind.String)
                            {
                                name = value.GetString() ?? name;
                            }

                            if (spec.TryGetProperty("version", out value) && value.ValueKind == JsonValueKind.String)
                            {
                                version = value.GetString() ?? version;
                            }
                        }
                    }
                }
                catch (JsonException)
                {
                }
            }

            return new ChromeModelEntry(name, version, GetSize(latest), latest.FullName);
        }

        /// <summary>
        /// Translator 언어팩. 영어와의 쌍 단위로 저장된다(en_ko 폴더가 en→ko 와 ko→en 을 함께 담는다).
        /// </summary>
        private static List<ChromeModelEntry> ReadPacks(string userData)
        {
            List<ChromeModelEntry> result = new List<ChromeModelEntry>();
            string root = Path.Combine(userData, "TranslateKit", "models");

            if (!Directory.Exists(root))
            {
                return result;
            }

            foreach (DirectoryInfo pack in new DirectoryInfo(root).GetDirectories().OrderBy(d => d.Name))
            {
                DirectoryInfo? version = pack.GetDirectories()
                    .OrderByDescending(d => d.CreationTimeUtc)
                    .FirstOrDefault();

                if (version == null)
                {
                    continue;
                }

                result.Add(new ChromeModelEntry(pack.Name, version.Name, GetSize(version), version.FullName));
            }

            return result;
        }

        private static long GetSize(DirectoryInfo directory)
        {
            try
            {
                return directory.GetFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string ToSizeText(long bytes)
        {
            if (bytes >= 1024L * 1024 * 1024)
            {
                return (bytes / (1024.0 * 1024 * 1024)).ToString("0.0", CultureInfo.InvariantCulture) + " GB";
            }

            return (bytes / (1024.0 * 1024)).ToString("0.0", CultureInfo.InvariantCulture) + " MB";
        }
    }
}
