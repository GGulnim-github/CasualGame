using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIApplicationDeviceTypeText : MonoBehaviour
{
    TextMeshProUGUI m_Text;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
        m_Text.text = $"{ApplicationManager.Instance.DeviceType}";
    }
}