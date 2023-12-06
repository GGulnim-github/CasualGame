using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(ToggleEventTrigger))]
public class UISoundBGMToggle : MonoBehaviour
{
    public bool IsOn { get { return m_Toggle.isOn; } }

    Toggle m_Toggle;
    ToggleEventTrigger m_EventTrigger;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_EventTrigger = GetComponent<ToggleEventTrigger>();
        m_EventTrigger.Initialize();

        m_Toggle.onValueChanged.AddListener(OnValueChanged);
        SetIsOnWithoutNotify(!SoundManager.Instance.MuteBGM);
        SoundManager.Instance.AddBGMToggle(this);
    }

    private void OnDestroy()
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.RemoveBGMToggle(this);
    }

    public void SetIsOnWithoutNotify(bool value)
    {
        m_Toggle.SetIsOnWithoutNotify(value);
        m_EventTrigger.Set(value);
    }

    void OnValueChanged(bool value)
    {
        SoundManager.Instance.MuteBGM = !value;
        m_EventTrigger.Set(value);
    }
}
