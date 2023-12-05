using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UILocalizedText : MonoBehaviour
{
    public string key;

    TextMeshProUGUI m_Text;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
        LocalizationManager.Instance.AddUIText(this);
        SetString();
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance == null) return;
        LocalizationManager.Instance.RemoveUIText(this);
    }

    public void SetString()
    {
        m_Text.text = LocalizationManager.Instance.GetLocalizedString(key);
    }
}
