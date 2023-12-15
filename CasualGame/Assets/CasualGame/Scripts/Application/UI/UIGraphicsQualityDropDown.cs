using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMP_Dropdown))]
public class UIGraphicsQualityDropDown : MonoBehaviour
{
    TMP_Dropdown m_Dropdown;

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();

        m_Dropdown.ClearOptions();

        foreach (ApplicationGraphicsQuality graphicsQuality in Enum.GetValues(typeof(ApplicationGraphicsQuality)))
        {
            string str = string.Empty;
            switch (graphicsQuality)
            {
                case ApplicationGraphicsQuality.Low:
                    str = "txt-낮음";
                    break;
                case ApplicationGraphicsQuality.Medium:
                    str = "txt-중간";
                    break;
                case ApplicationGraphicsQuality.High:
                    str = "txt-높음";
                    break;
            }
            m_Dropdown.options.Add(new TMP_Dropdown.OptionData(str));
        }

        m_Dropdown.onValueChanged.AddListener(OnvalueChanged);
    }

    private void OnEnable()
    {
        m_Dropdown.SetValueWithoutNotify((int)ApplicationManager.Instance.GraphicsQuality);
    }

    void OnvalueChanged(int value)
    {
        ApplicationGraphicsQuality graphicsQuality = (ApplicationGraphicsQuality)value;
        ApplicationManager.Instance.GraphicsQuality = graphicsQuality;
    }
}
