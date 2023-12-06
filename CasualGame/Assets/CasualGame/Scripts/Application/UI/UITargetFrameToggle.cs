using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(ToggleEventTrigger))]
public class UITargetFrameToggle : MonoBehaviour
{
    public ApplicationTargetFrame targetFrame;

    Toggle m_Toggle;
    ToggleEventTrigger m_EventTrigger;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_EventTrigger = GetComponent<ToggleEventTrigger>();
        m_EventTrigger.Initialize();

        m_Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        SetIsOnWithoutNotify(targetFrame == ApplicationManager.Instance.TargetFrame);
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
            ApplicationManager.Instance.TargetFrame = targetFrame;
        }
        m_EventTrigger.Set(value);
    }
}
