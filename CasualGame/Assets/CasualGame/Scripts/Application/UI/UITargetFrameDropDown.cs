using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Dropdown))]
public class UITargetFrameDropDown : MonoBehaviour
{
    TMP_Dropdown m_Dropdown;

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();

        m_Dropdown.ClearOptions();

        foreach (ApplicationTargetFrame targetFrame in Enum.GetValues(typeof(ApplicationTargetFrame)))
        {
            string str = string.Empty;
            switch (targetFrame)
            {
                case ApplicationTargetFrame.Auto:
                    str = "txt-auto";
                    break;
                case ApplicationTargetFrame.FPS30:
                    str = "txt-fps30";
                    break;
                case ApplicationTargetFrame.FPS60:
                    str = "txt-fps60";
                    break;
            }
            m_Dropdown.options.Add(new TMP_Dropdown.OptionData(str));
        }

        m_Dropdown.onValueChanged.AddListener(OnvalueChanged);
    }

    private void OnEnable()
    {
        m_Dropdown.SetValueWithoutNotify((int)ApplicationManager.Instance.TargetFrame);
    }

    void OnvalueChanged(int value)
    {
        ApplicationTargetFrame targetFrame = (ApplicationTargetFrame)value;
        ApplicationManager.Instance.TargetFrame = targetFrame;
    }
}
