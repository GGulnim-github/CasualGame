using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : PersistentSingleton<LocalizationManager>
{
    public LocalizationLanguage Language 
    { 
        get { return _localizationTableSO ? _localizationTableSO.language : GetDefaultLanguage(); } 
        set
        {
            SetLanguage(value);
        }
    }

    LocalizationTableSO _localizationTableSO;
    List<UILocalizedText> _uiTextList = new();

    public void Initialize()
    {
        Language = GetDefaultLanguage();
    }

    void SetLanguage(LocalizationLanguage language)
    {
        _localizationTableSO = GetLocalizationTable(language);
        UpdateString();
    }   

    LocalizationLanguage GetDefaultLanguage()
    {
        return Application.systemLanguage switch
        {
            SystemLanguage.Korean => LocalizationLanguage.Korean,
            _ => LocalizationLanguage.English,
        };
    }

    string GetTablePath(LocalizationLanguage language)
    {
        return $"Localization/{language}";
    }

    LocalizationTableSO GetLocalizationTable(LocalizationLanguage language)
    {
        // TODO : Addressable
        return Resources.Load<LocalizationTableSO>(GetTablePath(language)) 
            ?? throw new MissingReferenceException($"localization Table not found for {language}");
    }

    void UpdateString()
    {
        foreach (var text in _uiTextList)
        {
            text.SetString();
        }
    }

    public string GetLocalizedString(string key)
    {
        if (_localizationTableSO == null) return string.IsNullOrEmpty(key) ? "txt-keyNull" : key;
        if (_localizationTableSO.localizedStrings.TryGetValue(key, out string localizedString))
        {
            return localizedString;
        }
        else
        {
            return string.IsNullOrEmpty(key) ? "txt-keyNull" : key;
        }
    }

    public void AddUIText(UILocalizedText text)
    {
        if (_uiTextList.Contains(text)) return;
        _uiTextList.Add(text);
    }

    public void RemoveUIText(UILocalizedText text)
    {
        if (_uiTextList.Contains(text) == false) return;
        _uiTextList.Remove(text);
    }
}
