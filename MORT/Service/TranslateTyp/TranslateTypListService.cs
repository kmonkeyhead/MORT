using System;
using System.Collections.Generic;
using System.Linq;
using MORT.Model.CustomApi;
using MORT.Model.TranslateType;
using MORT.Service.CustomApi;

namespace MORT.Service.TranslateTyp;

public class TranslateTypListService
{
    private readonly List<TranslateTypeModel> _modelList = new List<TranslateTypeModel>();
    private readonly CustomApiPresetService _customApiPresetService;

    public List<string> GetTitles() => _modelList.Select(r => r.DisplayTitle).ToList();
    public TranslateTypeModel GetModel(int index)
    {
        if(index < 0 || index >= _modelList.Count)
        {
            return null;
        }
        return _modelList[index];
    }

    public TranslateTypeModel GetModel(SettingManager.TransType translateType, string subKey)
    {

        if(translateType == SettingManager.TransType.customApi)
        {
            var model = _modelList.FirstOrDefault(r => r.TransType == translateType && r.Key == subKey);

            if(model == null)
            {
                return _modelList.First(r => r.TransType == translateType);
            }

            return model;
        }

        return _modelList.FirstOrDefault(r => r.TransType == translateType) ?? _modelList.First(r => r.TransType == SettingManager.TransType.google_url);
    }


    public TranslateTypListService(CustomApiPresetService customApiPresetService)
    {
        _customApiPresetService = customApiPresetService;
    }
    public void Initialize()
    {
        _modelList.Clear();

        _modelList.Add(new TranslateTypeModel(
            0,
            "TRANSLATE GOOGLE",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE GOOGLE"),
            SettingManager.TransType.google_url));

        _modelList.Add(new TranslateTypeModel(
                1,
            "TRANSLATE CHROME BRIDGE",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE CHROME BRIDGE"),
            SettingManager.TransType.chromeBridge));

        _modelList.Add(new TranslateTypeModel(
                2,
            "TRANSLATE DB",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE DB"),
            SettingManager.TransType.db));

        _modelList.Add(new TranslateTypeModel(
                3,
            "TRANSLATE PAPAGO WEB",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE PAPAGO WEB"),
            SettingManager.TransType.papago_web));

        _modelList.Add(new TranslateTypeModel(
                4,
            "TRANSLATE NAVER",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE NAVER"),
            SettingManager.TransType.naver));

        _modelList.Add(new TranslateTypeModel(
                5,
            "TRANSLATE GOOGLE SHEET",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE GOOGLE SHEET"),
            SettingManager.TransType.google));

        _modelList.Add(new TranslateTypeModel(
                6,
            "TRANSLATE DEEPL",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE DEEPL"),
            SettingManager.TransType.deepl));

        _modelList.Add(new TranslateTypeModel(
                7,
            "TRANSLATE DEEPLAPI",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE DEEPLAPI"),
            SettingManager.TransType.deeplApi));

        _modelList.Add(new TranslateTypeModel(
                8,
            "TRANSLATE GEMINI API",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE GEMINI API"),
            SettingManager.TransType.gemini));

        _modelList.Add(new TranslateTypeModel(
                9,
            "TRANSLATE EZTRANS",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE EZTRANS"),
            SettingManager.TransType.ezTrans));

        _modelList.Add(new TranslateTypeModel(
                10,
            "TRANSLATE CUSTOM API",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE CUSTOM API"),
            SettingManager.TransType.customApi));

        InitializeCustomApi();
    }

    private void InitializeCustomApi()
    {
        _customApiPresetService.RefreshAdditional();
        int index = _modelList.Count;

        // 파일 기반 프리셋 우선, 동일 이름의 AdditionalList 항목은 제외
        var customApiPresets = new List<CustomApiModel>();
        customApiPresets.AddRange(_customApiPresetService.FilePresetList);

        var fileNames = new HashSet<string>(
            _customApiPresetService.FilePresetList.Select(p => p.Name),
            StringComparer.OrdinalIgnoreCase);

        foreach(var preset in _customApiPresetService.AdditionalList)
        {
            if(preset == null)
            {
                continue;
            }
            if(fileNames.Contains(preset.Name))
            {
                continue;
            }
            customApiPresets.Add(preset);
        }

        foreach(var preset in customApiPresets)
        {
            _modelList.Add(new TranslateTypeModel(
                index++,
                preset.Name,
                $"Custom - {preset.Name}",
                SettingManager.TransType.customApi));
        }
    }

    public SettingManager.TransType GetTransType(int index) => _modelList[index].TransType;

    public int GetIndexByTransType(SettingManager.TransType transType)
    {
        for(int i = 0; i < _modelList.Count; i++)
        {
            if(_modelList[i].TransType == transType)
            {
                return i;
            }
        }
        return -1;
    }

    public int GetIndexByTransType(SettingManager.TransType transType, string subKey)
    {
        if(transType == SettingManager.TransType.customApi && !string.IsNullOrEmpty(subKey))
        {
            for(int i = 0; i < _modelList.Count; i++)
            {
                if(_modelList[i].TransType == transType && _modelList[i].Key == subKey)
                {
                    return i;
                }
            }
        }
        return GetIndexByTransType(transType);
    }
}