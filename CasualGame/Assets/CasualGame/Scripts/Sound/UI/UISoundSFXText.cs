using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UISoundSFXText : MonoBehaviour
{
    TextMeshProUGUI m_Text;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
        SoundManager.Instance.AddSFXText(this);
        SetString();
    }

    private void OnDestroy()
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.RemoveSFXText(this);
    }

    public void SetString()
    {
        m_Text.text = ((int)SoundManager.Instance.VolumeSFX).ToString();
    }
}
