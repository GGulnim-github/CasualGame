using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(TMP_Dropdown))]
public class UILocalizedDropdown : MonoBehaviour
{
    TMP_Dropdown m_Dropdown;

    int m_SelectedIndex = -1;
    List<string> localizationKeyList = new();

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();
        LocalizationManager.Instance.AddUIDropdown(this);

        foreach (var optionData in m_Dropdown.options)
        {
            localizationKeyList.Add(optionData.text);
        }
        SetString();
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance == null) return;
        LocalizationManager.Instance.RemoveUIDropdown(this);
    }

    public void SetString()
    {
        m_SelectedIndex = m_Dropdown.value;

        m_Dropdown.ClearOptions();
        foreach (string key in localizationKeyList)
        {
            string str = LocalizationManager.Instance.GetLocalizedString(key);
            m_Dropdown.options.Add(new TMP_Dropdown.OptionData(str));
        }

        m_Dropdown.value = m_SelectedIndex;
        m_Dropdown.RefreshShownValue();
    }
}
