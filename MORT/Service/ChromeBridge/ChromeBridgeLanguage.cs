using System;
using System.Collections.Generic;

namespace MORT.Service.ChromeBridge
{
    /// <summary>
    /// MORT가 쓰는 언어 코드는 구글 번역기 코드라 형태가 제각각이다(zh-CN, pt-BR 등).
    /// 크롬 내장 AI는 BCP-47을 받으므로 페이지에 넘기기 전에 한 번 정리한다.
    /// </summary>
    public static class ChromeBridgeLanguage
    {
        public const string Auto = "auto";

        private static readonly Dictionary<string, string> _map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "zh-cn", "zh" },
            { "zh-hans", "zh" },
            { "zh-hans-cn", "zh" },
            { "zh-chs", "zh" },
            { "zh-tw", "zh-Hant" },
            { "zh-hant", "zh-Hant" },
            { "zh-hant-tw", "zh-Hant" },
            { "zh-cht", "zh-Hant" },
            { "pt-br", "pt" },
            { "pt-pt", "pt" },
            { "fr-fr", "fr" },
            { "de-de", "de" },
            { "es-es", "es" },
            { "en-us", "en" },
            { "en-gb", "en" },
            { "ko-kr", "ko" },
            { "ja-jp", "ja" },
        };

        public static string Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Auto;
            }

            string trimmed = value.Trim();

            if (string.Equals(trimmed, Auto, StringComparison.OrdinalIgnoreCase))
            {
                return Auto;
            }

            string? mapped;

            if (_map.TryGetValue(trimmed, out mapped))
            {
                return mapped;
            }

            // zh-Hant 처럼 스크립트가 붙은 값은 그대로 두고, 그 외 지역 코드는 앞부분만 쓴다.
            int dash = trimmed.IndexOf('-');

            if (dash > 0 && trimmed.Length - dash - 1 == 2)
            {
                return trimmed.Substring(0, dash).ToLowerInvariant();
            }

            return trimmed;
        }
    }

    /// <summary>
    /// 크롬 내장 AI에는 성격이 다른 두 경로가 있어서 하나로 묶지 않고 모드로 고른다.
    /// llm        : LanguageModel(Prompt API). 공식 지원 언어가 en/ja/es/de/fr 뿐이라 한국어 품질은 보장되지 않는다.
    /// translator : Translator API. 번역 전용 모델이고 한국어를 포함한 38개 언어를 지원한다.
    /// auto       : 언어 쌍을 Translator가 지원하면 Translator, 아니면 LanguageModel.
    /// </summary>
    public static class ChromeBridgeEngineMode
    {
        public const string Auto = "auto";
        public const string Llm = "llm";
        public const string Translator = "translator";

        public static string Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Auto;
            }

            switch (value.Trim().ToLowerInvariant())
            {
                case "llm":
                case "prompt":
                case "languagemodel":
                    return Llm;

                case "translator":
                case "translate":
                    return Translator;

                default:
                    return Auto;
            }
        }
    }
}
