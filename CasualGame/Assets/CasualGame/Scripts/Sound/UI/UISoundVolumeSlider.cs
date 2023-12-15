using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISoundVolumeSlider : MonoBehaviour
{
    public SoundType soundType;
    public TextMeshProUGUI valueText;

    public Color enableColor;
    public Color disableColor;

    public Color textEnableColor;
    public Color textDisableColor;

    Slider m_Slider;

    private void Awake()
    {
        m_Slider = GetComponent<Slider>();
        m_Slider.maxValue = SoundManager.Instance.MAX_VALUE;
        m_Slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        float volume = 100f;
        bool mute = false;
        switch (soundType)
        {
            case SoundType.BGM:
                volume = SoundManager.Instance.VolumeBGM;
                mute = SoundManager.Instance.MuteBGM;
                break;
            case SoundType.SFX:
                volume = SoundManager.Instance.VolumeSFX;
                mute = SoundManager.Instance.MuteSFX;
                break;
        }
        m_Slider.SetValueWithoutNotify(volume);
        m_Slider.interactable = !mute;
        SetColor();

        if (valueText != null)
        {
            valueText.text = ((int)volume).ToString();
        }
    }

    void OnValueChanged(float value)
    {
        switch (soundType)
        {
            case SoundType.BGM:
                SoundManager.Instance.VolumeBGM = value;
                break;
            case SoundType.SFX:
                SoundManager.Instance.VolumeSFX = value;
                break;
        }

        if (valueText != null)
        {
            valueText.text = ((int)value).ToString();
        }
    }

    public void SetColor()
    {
        if (m_Slider.interactable)
        {
            m_Slider.fillRect.GetComponent<Image>().color = enableColor;
            m_Slider.handleRect.GetComponent<Image>().color = enableColor;

            if (valueText != null)
            {
                valueText.color = textEnableColor;
            }
        }
        else
        {
            m_Slider.fillRect.GetComponent<Image>().color = disableColor;
            m_Slider.handleRect.GetComponent<Image>().color = disableColor;

            if (valueText != null)
            {
                valueText.color = textDisableColor;
            }
        }
    }
}
