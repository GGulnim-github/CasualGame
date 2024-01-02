using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIApplicationPlatformText : MonoBehaviour
{
    TextMeshProUGUI m_Text;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
        m_Text.text = $"{ApplicationManager.Instance.Platform}";
    }
}
