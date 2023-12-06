using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(SelectableEventTrigger))]
public class UISoundBGMSlider : MonoBehaviour
{
    public float Value { get { return m_Slider.value; } }
    public bool Interactable { get { return m_Slider.interactable; } }

    Slider m_Slider;
    SelectableEventTrigger m_EventTrigger;

    private void Awake()
    {
        m_Slider = GetComponent<Slider>();
        m_EventTrigger = GetComponent<SelectableEventTrigger>();
        m_EventTrigger.Initialize();

        m_Slider.onValueChanged.AddListener(OnValueChanged);
        m_Slider.maxValue = SoundManager.Instance.MAX_VALUE;
        SetValueWithoutNotify(SoundManager.Instance.VolumeBGM);
        SetInteractable(!SoundManager.Instance.MuteBGM);
        SoundManager.Instance.AddBGMSlider(this);
    }

    private void OnDestroy()
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.RemoveBGMSlider(this);
    }

    public void SetValueWithoutNotify(float value)
    {
        m_Slider.SetValueWithoutNotify(value);
    }

    public void SetInteractable(bool value)
    {
        m_Slider.interactable = value;
        m_EventTrigger.SetInteractable(value);
    }

    void OnValueChanged(float value)
    {
        SoundManager.Instance.VolumeBGM = value;
    }
}
