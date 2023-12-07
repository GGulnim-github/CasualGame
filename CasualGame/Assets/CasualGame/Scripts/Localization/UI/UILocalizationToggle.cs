using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UILocalizationToggle : MonoBehaviour
{
    public LocalizationLanguage language; 

    Toggle m_Toggle;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        m_Toggle.SetIsOnWithoutNotify(language == LocalizationManager.Instance.Language);
    }

    void OnValueChanged(bool value)
    {
        if (value)
        {
            LocalizationManager.Instance.Language = language;
        }
    }
}
