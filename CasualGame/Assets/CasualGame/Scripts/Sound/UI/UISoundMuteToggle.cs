using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UISoundMuteToggle : MonoBehaviour
{
    public SoundType soundType;
    public UISoundVolumeSlider volumeSlider;
    Toggle m_Toggle;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        bool mute = false;
        switch (soundType)
        {
            case SoundType.BGM:
                mute = SoundManager.Instance.MuteBGM;
                break;
            case SoundType.SFX:
                mute = SoundManager.Instance.MuteSFX;
                break;
        }
        m_Toggle.SetIsOnWithoutNotify(mute);
    }

    void OnValueChanged(bool value)
    {
        switch (soundType)
        {
            case SoundType.BGM:
                SoundManager.Instance.MuteBGM = value;
                break;
            case SoundType.SFX:
                SoundManager.Instance.MuteSFX = value;
                break;
        }

        if (volumeSlider != null)
        {
            volumeSlider.GetComponent<Slider>().interactable = !value;
            volumeSlider.SetColor();
        }
    }
}
