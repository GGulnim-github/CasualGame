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
        if (string.IsNullOrEmpty(version))
        {
            version = Application.version;
        }
        m_Text.text = $"ver {version}";
    }
}
