using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIApplicationVersionText : MonoBehaviour
{
    TextMeshProUGUI m_Text;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
        string version = ApplicationManager.Instance.Version;
        m_Text.text = $"ver {version}";
    }
}
