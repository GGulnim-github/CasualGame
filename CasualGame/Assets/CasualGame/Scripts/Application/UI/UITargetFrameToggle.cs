using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UITargetFrameToggle : MonoBehaviour
{
    public ApplicationTargetFrame targetFrame;

    Toggle m_Toggle;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        m_Toggle.SetIsOnWithoutNotify(targetFrame == ApplicationManager.Instance.TargetFrame);
    }

    void OnValueChanged(bool value)
    {
        if (value)
        {
            ApplicationManager.Instance.TargetFrame = targetFrame;
        }
    }
}
