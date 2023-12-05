using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UILocalizationToggle : MonoBehaviour
{
    public LocalizationLanguage language;
    public bool IsOn { get { return m_Toggle.isOn; } }

    Toggle m_Toggle;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnValueChanged);
        m_Toggle.SetIsOnWithoutNotify(language == LocalizationManager.Instance.Language);
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
    }

    void OnValueChanged(bool value)
    {
        if (value)
        {
            LocalizationManager.Instance.Language = language;
        }
    }
}
