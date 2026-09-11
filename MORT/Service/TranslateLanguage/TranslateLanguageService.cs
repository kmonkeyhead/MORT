using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MORT.Service.TranslateLanguage
{
    /// <summary>
    /// 번역기가 쓰는 언어 코드 묶음. 같은 언어라도 번역기마다 코드가 다르다.
    /// 예를 들어 중국어 간체는 파파고가 zh-CN, DeepL이 ZH-HANS 다.
    /// </summary>
    public enum TranslateEngineFamily
    {
        /// <summary>번역 언어를 쓰지 않는 번역기(DB, EzTrans).</summary>
        None,
        Naver,
        Google,
        DeepL,
    }

    /// <summary>
    /// 언어 하나. 사용자가 고르는 것은 Key 이고, 번역기에 넘어가는 것은 각 번역기 코드다.
    /// </summary>
    public sealed class TranslateLanguageModel
    {
        public string Key { get; }
        public string OcrCode { get; }
        public string NaverCode { get; }
        public string GoogleCode { get; }
        public string DeepLCode { get; }
        public bool IsCustom { get; }

        private readonly string _title;

        public TranslateLanguageModel(string key, string title, string ocrCode,
            string naverCode, string googleCode, string deepLCode, bool isCustom)
        {
            Key = key;
            _title = title;
            OcrCode = ocrCode;
            NaverCode = naverCode;
            GoogleCode = googleCode;
            DeepLCode = deepLCode;
            IsCustom = isCustom;
        }

        /// <summary>
        /// 사용자에게 보이는 이름. 사용자가 직접 넣은 코드는 번역할 이름이 없으므로 적은 그대로 쓴다.
        /// </summary>
        public string Title
        {
            get
            {
                if (IsCustom)
                {
                    return _title;
                }

                return LocalizeManager.LocalizeManager.GetLocalizeString(Key, _title);
            }
        }

        public string GetCode(TranslateEngineFamily family)
        {
            switch (family)
            {
                case TranslateEngineFamily.Naver:
                    return NaverCode;

                case TranslateEngineFamily.DeepL:
                    return DeepLCode;

                default:
                    return GoogleCode;
            }
        }
    }

    /// <summary>
    /// 번역 언어를 한 곳에서 정하고, 번역기별 코드로 바꿔 주는 중간 변환 서비스.
    ///
    /// 예전에는 번역 설정 탭에 파파고용·구글용·DeepL용 언어 칸이 따로 있었다. 세 벌을 다 맞춰 둬야
    /// 번역기를 바꿔도 같은 언어가 나왔고, 하나만 어긋나 있으면 번역기를 바꾼 순간 엉뚱한 언어로 번역됐다.
    /// 이제 "어디에서 어디로" 하나만 고르고, 번역기 코드로 바꾸는 일은 전부 여기서 한다.
    ///
    /// 표는 예전 TransManager.InitTransCode 에 있던 것을 그대로 옮겼다. 코드 값을 바꾸면
    /// 기존 사용자의 번역 언어가 조용히 달라지므로 옮기면서 값은 손대지 않았다.
    /// </summary>
    public sealed class TranslateLanguageService
    {
        /// <summary>
        /// SettingManager 는 DI로 만들어지지 않고 Form1이 직접 new 한다. 그래서 생성자에서 정적 접근점을
        /// 같이 채운다. TransManager 가 DI와 싱글톤을 함께 두는 것과 같은 방식이다.
        /// </summary>
        public static TranslateLanguageService Instance { get; private set; }

        private readonly List<TranslateLanguageModel> _languages = new List<TranslateLanguageModel>();

        public TranslateLanguageService()
        {
            Instance = this;
            Reload();
        }

        public IReadOnlyList<TranslateLanguageModel> Languages => _languages;

        /// <summary>기본 원문 언어. 예전 SettingManager 기본값과 같다.</summary>
        public const string DefaultFromKey = "en";

        public void Reload()
        {
            _languages.Clear();

            //(키, 이름, OCR 코드, 파파고 코드, 구글 코드, DeepL 코드)
            Add("en", "영어", "en", "en", "en", "en");
            Add("ja", "일본어", "ja", "ja", "ja", "ja");
            Add("zh-CN", "중국어 간체", "zh-Hans-CN", "zh-CN", "zh-CN", "ZH-HANS");
            Add("zh-TW", "중국어 번체", "zh-Hant-TW", "zh-TW", "zh-TW", "ZH-HANT");
            Add("es", "스페인어", "es", "es", "es", "es");
            Add("fr", "프랑스어", "fr-FR", "fr", "fr", "fr");
            Add("vi", "베트남어", "vi", "vi", "vi", "vi");
            Add("th", "태국어", "th", "th", "th", "th");
            Add("id", "인도네시아어", "id", "id", "id", "id");
            Add("ko", "한국어", "ko", "ko", "ko", "ko");
            Add("ru", "러시아어", "ru", "ru", "ru", "ru");
            Add("de", "독일어", "de-DE", "de", "de", "de");
            Add("pt-BR", "브라질어", "pt-BR", "", "pt-BR", "pt-BR");
            Add("pt-PT", "포르투갈어", "pt-PT", "pt", "pt-PT", "pt-PT");
            Add("tr", "터키어", "tr", "", "tr", "tr");
            Add("it", "이탈리아어", "it", "", "it", "it");
            Add("pl", "폴란드어", "pl", "", "pl", "pl");
            Add("ar", "아랍어", "ar", "ar", "ar", "ar");
            Add("hu", "헝가리어", "hu", "", "hu", "hu");
            Add("uk", "우크라이나어", "uk", "", "uk", "uk");
            Add("cs", "체코어", "cs", "", "cs", "cs");
            Add("fa", "페르시아어", "fa", "", "fa", "");
            Add("el", "그리스어", "el", "", "el", "el");
            Add("nl", "네덜란드어", "nl", "", "nl", "nl");

            LoadCustomCode();
        }

        private void Add(string key, string title, string ocrCode,
            string naverCode, string googleCode, string deepLCode, bool isCustom = false)
        {
            if (_languages.Any(r => r.Key == key))
            {
                //중복 방지
                return;
            }

            _languages.Add(new TranslateLanguageModel(key, title, ocrCode, naverCode, googleCode, deepLCode, isCustom));
        }

        /// <summary>
        /// 표에 없는 언어를 사용자가 직접 넣는 파일. 번역기별 코드를 구분해 적을 방법이 없으므로
        /// 적은 코드를 모든 번역기에 그대로 쓴다.
        /// </summary>
        private void LoadCustomCode()
        {
            try
            {
                string path = Util.ToCurrentFilePath(GlobalDefine.CUSTOM_TRANSCODE_FILE);

                using (StreamReader reader = new StreamReader(path))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        string[] keys = line.Split(',');

                        if (keys.Length == 2 && !keys[0].Contains("//"))
                        {
                            string code = keys[0].Trim();
                            string title = keys[1].Trim();

                            Add(code, title, "", code, code, code, isCustom: true);
                        }
                    }
                }
            }
            catch
            {
                //파일이 없는 것이 보통이다.
            }
        }

        public TranslateLanguageModel Find(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            return _languages.FirstOrDefault(r => r.Key == key);
        }

        /// <summary>OCR 언어를 바꿨을 때 원문 언어를 따라 맞추는 데 쓴다.</summary>
        public TranslateLanguageModel FindByOcrCode(string ocrCode)
        {
            if (string.IsNullOrEmpty(ocrCode))
            {
                return null;
            }

            return _languages.FirstOrDefault(r => !string.IsNullOrEmpty(r.OcrCode)
                && Util.GetIsEqualMainOcrCode(ocrCode, r.OcrCode));
        }

        public static TranslateEngineFamily GetFamily(SettingManager.TransType transType)
        {
            switch (transType)
            {
                case SettingManager.TransType.naver:
                case SettingManager.TransType.papago_web:
                    return TranslateEngineFamily.Naver;

                case SettingManager.TransType.deepl:
                case SettingManager.TransType.deeplApi:
                    return TranslateEngineFamily.DeepL;

                case SettingManager.TransType.google:
                case SettingManager.TransType.google_url:
                case SettingManager.TransType.gemini:
                case SettingManager.TransType.chromeBridge:
                case SettingManager.TransType.customApi:
                    return TranslateEngineFamily.Google;

                default:
                    //DB와 EzTrans는 번역 언어를 쓰지 않는다.
                    return TranslateEngineFamily.None;
            }
        }

        /// <summary>
        /// 통합 언어 키를 번역기 코드로 바꾼다.
        ///
        /// 그 번역기가 지원하지 않는 언어면 구글 코드로, 그것도 없으면 키를 그대로 돌려준다.
        /// 빈 문자열을 넘기면 번역기가 아무 말 없이 엉뚱하게 동작하기 때문이다.
        /// 지원하지 않는다는 사실은 IsSupported 로 UI에서 따로 알린다.
        /// </summary>
        public string ToEngineCode(string key, TranslateEngineFamily family)
        {
            TranslateLanguageModel model = Find(key);

            if (model == null)
            {
                return key ?? "";
            }

            string code = model.GetCode(family);

            if (!string.IsNullOrEmpty(code))
            {
                return code;
            }

            if (!string.IsNullOrEmpty(model.GoogleCode))
            {
                return model.GoogleCode;
            }

            return model.Key;
        }

        public string ToEngineCode(string key, SettingManager.TransType transType)
        {
            return ToEngineCode(key, GetFamily(transType));
        }

        public bool IsSupported(string key, TranslateEngineFamily family)
        {
            if (family == TranslateEngineFamily.None)
            {
                return true;
            }

            TranslateLanguageModel model = Find(key);

            if (model == null)
            {
                return true;
            }

            return !string.IsNullOrEmpty(model.GetCode(family));
        }

        /// <summary>
        /// 예전 설정을 읽을 때 쓴다. 번역기 코드가 어느 언어였는지 거꾸로 찾는다.
        /// 코드가 겹치는 언어는 없지만, 못 찾으면 null을 돌려주고 부르는 쪽이 기본값을 쓴다.
        /// </summary>
        public TranslateLanguageModel FindByEngineCode(string code, TranslateEngineFamily family)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            TranslateLanguageModel model = _languages.FirstOrDefault(
                r => string.Equals(r.GetCode(family), code, StringComparison.OrdinalIgnoreCase));

            if (model != null)
            {
                return model;
            }

            //번역기를 바꾸기 전에 쓰던 코드가 남아 있을 수 있어서 다른 번역기 코드로도 찾아 본다.
            return _languages.FirstOrDefault(r =>
                string.Equals(r.Key, code, StringComparison.OrdinalIgnoreCase)
                || string.Equals(r.GoogleCode, code, StringComparison.OrdinalIgnoreCase)
                || string.Equals(r.NaverCode, code, StringComparison.OrdinalIgnoreCase)
                || string.Equals(r.DeepLCode, code, StringComparison.OrdinalIgnoreCase));
        }
    }
}
