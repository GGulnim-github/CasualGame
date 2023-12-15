using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Dropdown))]
public class UILocalizationDropdown : MonoBehaviour
{
    TMP_Dropdown m_Dropdown;

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();

        m_Dropdown.ClearOptions();
        foreach (LocalizationLanguage language in Enum.GetValues(typeof(LocalizationLanguage)))
        {
            string str = string.Empty;
            switch (language)
            {
                case LocalizationLanguage.Korean:
                    str = "ÇÑ±¹¾î";
                    break;
                case LocalizationLanguage.English:
                    str = "English";
                    break;
            }
            m_Dropdown.options.Add(new TMP_Dropdown.OptionData(str));
        }

        m_Dropdown.onValueChanged.AddListener(OnvalueChanged);
    }

    private void OnEnable()
    {
        m_Dropdown.SetValueWithoutNotify((int)LocalizationManager.Instance.Language);
    }

    void OnvalueChanged(int value)
    {
        LocalizationLanguage language = (LocalizationLanguage)value;
        LocalizationManager.Instance.Language = language;
    }
}
