using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UILocalizedText : MonoBehaviour
{
    public string key;
    public bool dontSetStringInAwake;
    TextMeshProUGUI m_Text;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
        LocalizationManager.Instance.AddUIText(this);

        if (dontSetStringInAwake == false)
        {
            SetString();
        }
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance == null) return;
        LocalizationManager.Instance.RemoveUIText(this);
    }

    public void SetString()
    {
        if (m_Text == null) m_Text = GetComponent<TextMeshProUGUI>();
        m_Text.text = LocalizationManager.Instance.GetLocalizedString(key);
    }

    public void SetString(string key)
    {
        this.key = key;
        SetString();
    }
}
