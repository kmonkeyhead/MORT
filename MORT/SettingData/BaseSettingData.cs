using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MORT;
using MORT.SettingData;

namespace MORT.SettingData
{
    public interface ISettingDataParse
    {
        string ToSave();
        void LoadValue(string fileData);
        void SetDefault();
    }

    public interface ISettingData<T> : ISettingDataParse
    {
        T Value { get; set; }
    }

    public static class SettingDataFactory
    {
        public static ISettingData<T> Create<T>(string key, List<ISettingDataParse> parList, T defaultValue)
        {
            if (typeof(T) == typeof(bool))
            {
                var result = (ISettingData<T>)new BoolSettingData(key, defaultValue);
                parList.Add(result);
                return result;
            }
            else if (typeof(T) == typeof(int))
            {
                var result = (ISettingData<T>)new IntSettingData(key, defaultValue);
                parList.Add(result);
                return result;
            }
            else if (typeof(T) == typeof(string))
            {
                var result = (ISettingData<T>)new StringSettingData(key, defaultValue);
                parList.Add(result);
                return result;
            }
            
            var type = typeof(T);
            if (type.IsClass && type.GetConstructor(Type.EmptyTypes) != null)
            {
                var generic = typeof(JsonSettingData<>).MakeGenericType(type);
                var instance = (ISettingData<T>)Activator.CreateInstance(generic, key, defaultValue)!;
                parList.Add(instance);
                return instance;
            }


            throw new InvalidOperationException();
        }

        public static ISettingData<List<T>> CreateList<T, TData>(string key, List<ISettingDataParse> parList)
        {
            if (typeof(TData) == typeof(TranslationFileSettingData))
            {
                var result = (ISettingData<List<T>>)new TranslationFileSettingData(key);
                parList.Add(result);
                return result;
            }
            else if (typeof(TData) == typeof(HotKeySettingData))
            {
                var result = (ISettingData<List<T>>)new HotKeySettingData();
                parList.Add(result);
                return result;
            }

            throw new InvalidOperationException();
        }
    }

    public class BaseSettingData<T> : ISettingData<T>
    {
        public T Value { get; set; }
        protected T _defaultValue;
        protected string FileKey { get; set; }

        protected string ValueString;

        public virtual string ToSave()
        {
            return $"{FileKey}[{Value.ToString()}]{System.Environment.NewLine}";
        }

        public virtual void SetDefault()
        {
            Value = _defaultValue;
        }

        public virtual void LoadValue(string fileData)
        {
        }
    }
    
    public class JsonSettingData<T> : BaseSettingData<T> where T : class, new()
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = true
        };

        public JsonSettingData(string key, T defaultValue)
        {
            FileKey = key;
            _defaultValue = defaultValue ?? new T();
            Value = _defaultValue;
        }

        public override string ToSave()
        {
            // JSON 안에 개행/특수문자가 들어가도 안전하도록, key<json> 형태로 저장
            var json = JsonSerializer.Serialize(Value, JsonOptions);
            return $"{FileKey}<{json}>{Environment.NewLine}";
        }

        public override void LoadValue(string fileData)
        {
            try
            {
                // 프로젝트에 이미 쓰이는 파싱 유틸 스타일을 그대로 활용
                // (FileKey< ... > 사이의 내용을 꺼낸다고 가정)
                string json = Util.ParseString(fileData, FileKey, '<', '>');
                if (string.IsNullOrWhiteSpace(json))
                {
                    Value = _defaultValue;
                    ValueString = "";
                    return;
                }

                var obj = JsonSerializer.Deserialize<T>(json, JsonOptions);
                Value = obj ?? _defaultValue;
                ValueString = json;
            }
            catch
            {
                // 파싱/역직렬화 실패 시 기본값 복구
                Value = _defaultValue;
                ValueString = "";
            }
        }

        public override void SetDefault()
        {
            Value = _defaultValue;
            ValueString = "";
        }
    }



    public class BoolSettingData : BaseSettingData<bool>
    {
        public BoolSettingData(string key, object defaultValue)
        {
            _defaultValue = (bool)defaultValue;
            FileKey = key;
        }

        public override void LoadValue(string fileData)
        {
            bool result;
            if (Util.TryParseBool(out result, fileData, FileKey))
            {
                Value = result;
            }
            else
            {
                Value = _defaultValue;
            }

            ValueString = Value.ToString();
        }
    }

    public class IntSettingData : BaseSettingData<int>
    {
        public IntSettingData(string key, object defaultValue)
        {
            FileKey = key;
            _defaultValue = (int)defaultValue;
        }

        public override void LoadValue(string fileData)
        {
            int result;
            if (Util.TryParseInt(out result, fileData, FileKey))
            {
                Value = result;
            }
            else
            {
                Value = _defaultValue;
            }

            ValueString = Value.ToString();
        }
    }

    public class StringSettingData : BaseSettingData<string>
    {
        public StringSettingData(string key, object defaultValue)
        {
            FileKey = key;
            _defaultValue = (string)defaultValue;
        }

        public override void LoadValue(string fileData)
        {
            string result;
            if (Util.TryParseString(out result, fileData, FileKey))
            {
                Value = result;
            }
            else
            {
                Value = _defaultValue;
            }

            ValueString = Value.ToString();
        }
    }

    public class TranslationFileSettingData : BaseSettingData<List<string>>
    {
        public TranslationFileSettingData(string key)
        {
            Value = new List<string>();
            FileKey = key;
        }

        public override void LoadValue(string fileData)
        {
            string sep = "\t";
            string content = Util.ParseString(fileData, FileKey, '<', '>');
            string[] keys = content.Split(sep.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Value.Clear();
            foreach (var obj in keys)
            {
                //파일이 존재하는 것 만 넣는다.
                if (File.Exists(GlobalDefine.ADVENCED_TRANSRATION_PATH + obj + ".txt"))
                {
                    Value.Add(obj);
                }
            }
        }

        public override string ToSave()
        {
            string result = "";
            if (Value.Count > 0)
            {
                string files = FileKey + "<";

                for (int i = 0; i < Value.Count; i++)
                {
                    files += Value[i];

                    if (i + 1 != Value.Count)
                    {
                        files += "\t";
                    }
                }

                files += ">";

                result += files;
            }

            return result;
        }

        public override void SetDefault()
        {
            Value.Clear();
        }
    }

    public class HotKeySettingData : BaseSettingData<List<HotKeyData>>
    {
        public const int OPEN_SETTING_MAX = 4;

        public const string KEY_HOTKEY_FORMAT = "@HOTKEY_{0}_{1} ";
        public const string KEY_HOTKEY_EXTRA_VALUE = "@HOTKEY_EXTRA_VALUE_{0}_{1} ";

        public HotKeySettingData()
        {
            Value = new List<HotKeyData>();
            FileKey = "";
        }

        public override void LoadValue(string fileData)
        {
            //설정파일 단축키 가져오기
            for (int i = 0; i < OPEN_SETTING_MAX; i++)
            {
                LoadHotkey(i, KeyInputLabel.KeyType.OpenSetting, fileData, KEY_HOTKEY_EXTRA_VALUE);
            }

            //DBTranslate, NaverTranslate, GoogleTranslate, GoogleSheetTranslate, HdTrans
            LoadHotkey(0, KeyInputLabel.KeyType.LayerTransparency, fileData);

            LoadHotkey(0, KeyInputLabel.KeyType.DBTranslate, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.NaverTranslate, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.GoogleTranslate, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.GoogleSheetTranslate, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.HdTrans, fileData);
            LoadHotkey(0, KeyInputLabel.KeyType.DeepL, fileData);
        }

        public override string ToSave()
        {
            string result = "";

            for (int i = 0; i < Value.Count; i++)
            {
                string key = string.Format(KEY_HOTKEY_FORMAT, Value[i].index, Value[i].keyType.ToString());
                string keyResult = Value[i].keyResult;

                result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;

                if (Value[i].extraData != "")
                {
                    key = string.Format(KEY_HOTKEY_EXTRA_VALUE, i.ToString(), Value[i].keyType);
                    keyResult = Value[i].extraData;

                    result += key + System.Environment.NewLine + keyResult + System.Environment.NewLine + System.Environment.NewLine;
                }
            }

            return result;
        }

        private void LoadHotkey(int index, KeyInputLabel.KeyType keyType, string fileData, string extraKey = "")
        {
            string hotKey = string.Format(KEY_HOTKEY_FORMAT, index, keyType);
            string keyResult = Util.GetNextLine(fileData, hotKey);
            //옛 ezTrans 단축키를 hdTrans에 이어받는다. 새 키가 있으면 새 값을 우선한다.
            if (keyType == KeyInputLabel.KeyType.HdTrans && !fileData.Contains(hotKey))
            {
                keyResult = Util.GetNextLine(fileData, string.Format(KEY_HOTKEY_FORMAT, index, "EzTrans"));
            }

            string extraFileKey = "";
            string extraFileResult = "";

            if (extraKey != "")
            {
                extraFileKey = string.Format(extraKey, index.ToString(), keyType);
                extraFileResult = Util.GetNextLine(fileData, extraFileKey);
            }

            HotKeyData hotKeyData = new HotKeyData(index, keyType, keyResult, extraFileResult);
            Value.Add(hotKeyData);
        }

        public override void SetDefault()
        {
            Value.Clear();
        }
    }
}
