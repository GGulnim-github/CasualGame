using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(ToggleEventTrigger))]
public class UILocalizationToggle : MonoBehaviour
{
    public LocalizationLanguage language;
    public bool IsOn { get { return m_Toggle.isOn; } }

    Toggle m_Toggle;
    ToggleEventTrigger m_EventTrigger;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_EventTrigger = GetComponent<ToggleEventTrigger>();
        m_EventTrigger.Initialize();

        m_Toggle.onValueChanged.AddListener(OnValueChanged);
        SetIsOnWithoutNotify(language == LocalizationManager.Instance.Language);
        LocalizationManager.Instance.AddUIToggle(this);
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance == null) return;
        LocalizationManager.Instance.RemoveUIToggle(this);
    }

    public void SetIsOnWithoutNotify(bool value)
    {
        m_Toggle.SetIsOnWithoutNotify(value);
        m_EventTrigger.Set(value);
    }

    void OnValueChanged(bool value)
    {
        if (value)
        {
            LocalizationManager.Instance.Language = language;
        }
        m_EventTrigger.Set(value);
    }
}
