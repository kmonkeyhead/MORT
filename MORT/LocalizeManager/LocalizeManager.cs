
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MORT.LocalizeManager
{
    public enum AppLanguage
    {
        Auto, Korea, English, Japanese, SimplifiedChinese, Indonesian, Russian, Portuguese, Ukrainian, Turkish
    }

    public class LocalizeData
    {
        public LocalizeData(string key, string ko, string en, string jpn, string zhCN, string id, string ru, string pt, string uk, string tr)
        {
            Key = key;
            Ko = ko;
            En = en;
            Jpn = jpn;
            ZhCN = zhCN;
            Id = id;
            Ru = ru;
            Pt = pt;
            Uk = uk;
            Tr = tr;
        }


        public string Key { get; }
        public string Ko { get; }
        public string En { get; }
        public string Jpn { get; }
        public string ZhCN { get; }
        public string Id { get; }
        public string Ru { get; }
        public string Pt { get; }
        public string Uk { get; }
        public string Tr { get; }
    }

    public interface ILocalize
    {
        string GetLocalizeString(string key, string defaultText);
    }


    internal class LocalizeManager
    {
        public static AppLanguage Language { get; private set; } = AppLanguage.Korea;
        private static List<LocalizeData> LocalizeDatas = new List<LocalizeData>();
        public static void Init(string data, AppLanguage language)
        {
            Language = language;
            using var stream = typeof(LocalizeManager).Assembly.GetManifestResourceStream("MORT.PlamoLocalization");
            using var plamo = new StreamReader(stream);
            using(TextReader sr = new StringReader(data + "\n" + plamo.ReadToEnd()))
            using(TextFieldParser parser = new TextFieldParser(sr))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while(!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    if(fields.Length >= 4)
                    {
                        LocalizeDatas.Add(new LocalizeData(fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7], fields[8], fields[9], fields[10]));
                    }
                }
            }
        }

        public static AppLanguage ConvertAppLanguage(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                CultureInfo ci = CultureInfo.CurrentUICulture;
                value = ci.TwoLetterISOLanguageName;
            }

            if(value == "ko")
            {
                return AppLanguage.Korea;
            }
            else if(value == "ja")
            {
                return AppLanguage.Japanese;
            }
            else if(value == "zh")
            {
                return AppLanguage.SimplifiedChinese;
            }
            else if(value == "id")
            {
                return AppLanguage.Indonesian;
            }
            else if(value == "ru")
            {
                return AppLanguage.Russian;
            }
            else if(value == "pt")
            {
                return AppLanguage.Portuguese;
            }
            else if(value == "uk")
            {
                return AppLanguage.Ukrainian;
            }
            else if(value == "tr")
            {
                return AppLanguage.Turkish;
            }
            else
            {
                return AppLanguage.English;
            }
        }

        public static string GetLocalizeString(string key, string defaultText = "", AppLanguage appLanguage = AppLanguage.Auto)
        {
            var data = LocalizeDatas.FirstOrDefault(r => r.Key == key);

            if(appLanguage == AppLanguage.Auto)
            {
                appLanguage = Language;
            }

            if(data == null)
            {
                return key;
            }

            switch(appLanguage)
            {
                case AppLanguage.Korea:
                    return data.Ko;

                case AppLanguage.English:
                    return data.En;

                case AppLanguage.Japanese:
                    return data.Jpn;

                case AppLanguage.SimplifiedChinese:
                    return data.ZhCN;

                case AppLanguage.Indonesian:
                    return data.Id;

                case AppLanguage.Russian:
                    return data.Ru;

                case AppLanguage.Portuguese:
                    return data.Pt;

                case AppLanguage.Ukrainian:
                    return data.Uk;

                case AppLanguage.Turkish:
                    return data.Tr;

                default:
                    return defaultText;
            }
        }
    }
}
