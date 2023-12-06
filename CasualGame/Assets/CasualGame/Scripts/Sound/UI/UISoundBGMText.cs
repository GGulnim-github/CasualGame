using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UISoundBGMText : MonoBehaviour
{
    TextMeshProUGUI m_Text;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
        SoundManager.Instance.AddBGMText(this);
        SetString();
    }

    private void OnDestroy()
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.RemoveBGMText(this);
    }

    public void SetString()
    {
        m_Text.text = ((int)SoundManager.Instance.VolumeBGM).ToString();
    }
}
