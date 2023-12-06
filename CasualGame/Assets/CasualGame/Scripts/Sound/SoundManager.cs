using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class SoundManager : PersistentSingleton<SoundManager>
{
    const float MAX_VOLUME = 0f;
    const float MIN_VOLUME = -80f;

    [NonSerialized]
    public float MAX_VALUE = 100f;

    float _volumeBGM;
    public float VolumeBGM
    {
        get { return _volumeBGM; }
        set
        {
            SetVolumeBGM(value);
            SetBGMSliderAndText();
        }
    }

    float _volumeSFX;
    public float VolumeSFX
    {
        get { return _volumeSFX; }
        set
        {
            SetVolumeSFX(value);
            SetSFXSliderAndText();
        }
    }

    bool _muteBGM;
    public bool MuteBGM
    {
        get { return _muteBGM; }
        set
        {
            SetMuteBGM(value);
            SetBGMToggle();
        }
    }

    bool _muteSFX;
    public bool MuteSFX
    {
        get { return _muteSFX; }
        set
        {
            SetMuteSFX(value);
            SetSFXToggle();
        }
    }

    [Header("Audio Mixer")]
    [SerializeField]
    AudioMixer _audioMixer;

    AudioMixerGroup _bgmAudioMixerGroup;

    AudioMixerGroup _uiAudioMixerGroup;
    AudioMixerGroup _enviromentMixerGroup;
    AudioMixerGroup _effectMixerGroup;

    [Space(5)]
    [Header("Sound BGM")]
    SoundBGM _soundBGM;

    [Space(5)]
    [Header("Sound SFX")]
    SoundSFXUI _soundUI;

    IObjectPool<SoundSFXEffect> _sfxEffectPool;
    Dictionary<string, AudioClip> _preloadClip;

    List<UISoundBGMSlider> _uiBGMSliderList = new();
    List<UISoundBGMText> _uiBGMTextList = new();
    List<UISoundBGMToggle> _uiBGMToggleList = new();

    List<UISoundSFXSlider> _uiSFXSliderList = new();
    List<UISoundSFXText> _uiSFXTextList = new();
    List<UISoundSFXToggle> _uiSFXToggleList = new();

    public void Initialize()
    {
        if (_audioMixer == null)
        {
            _audioMixer = GetAudioMixer();
        }

        _bgmAudioMixerGroup = _audioMixer.FindMatchingGroups("Master/BGM")[0];

        _uiAudioMixerGroup = _audioMixer.FindMatchingGroups("Master/SFX/UI")[0];
        _enviromentMixerGroup = _audioMixer.FindMatchingGroups("Master/SFX/Enviroment")[0];
        _effectMixerGroup = _audioMixer.FindMatchingGroups("Master/SFX/Effect")[0];

        GameObject bgmGameObject = new GameObject("Sound BGM");
        bgmGameObject.transform.SetParent(transform);
        _soundBGM = bgmGameObject.AddComponent<SoundBGM>();
        _soundBGM.Initialize(_bgmAudioMixerGroup);

        GameObject uiGameObject = new GameObject("Sound UI");
        uiGameObject.transform.SetParent(transform);
        _soundUI = uiGameObject.AddComponent<SoundSFXUI>();
        _soundUI.Initialize(_uiAudioMixerGroup);

        _sfxEffectPool = new ObjectPool<SoundSFXEffect>(CreateSFXEffect, OnGetSFXEffect, OnReleaseSFXEffect, OnClearSFXEffect);

        MuteBGM = false;
        MuteSFX = false;
        VolumeBGM = 100f;
        VolumeSFX = 100f;
    }

    #region SFX Effect Pool
    SoundSFXEffect CreateSFXEffect()
    {
        GameObject gameObject = new("Sound SFX Effect");
        SoundSFXEffect sfx = gameObject.AddComponent<SoundSFXEffect>();
        sfx.Initialize(_effectMixerGroup);
        sfx.transform.SetParent(transform);
        return sfx;
    }

    void OnGetSFXEffect(SoundSFXEffect sfx)
    {
        sfx.gameObject.SetActive(true);
    }

    void OnReleaseSFXEffect(SoundSFXEffect sfx)
    {
        sfx.gameObject.SetActive(false);
    }

    void OnClearSFXEffect(SoundSFXEffect sfx)
    {
        Destroy(sfx.gameObject);
    }
    #endregion

    public void PlayBGM(string clipName)
    {
        _soundBGM.Play(GetClip(SoundType.Bgm, clipName));
    }
    public void StopBGM()
    {
        _soundBGM.Stop();
    }

    public void PlayEffect(string clipName, Vector3 position)
    {
        SoundSFXEffect soundEffect = _sfxEffectPool.Get();
        soundEffect.Play(GetClip(SoundType.Effect, clipName), position);
    }
    public void ReleaseEffect(SoundSFXEffect effect)
    {
        _sfxEffectPool.Release(effect);
    }

    public void PlayUI(string clipName)
    {
        _soundUI.Play(GetClip(SoundType.UI, clipName));
    }

    string GetClipPath(SoundType type, string clipName)
    {
        return $"Sound/{type}/{clipName}";
    }
    AudioClip GetClip(SoundType type, string clipName)
    {
        if (_preloadClip.TryGetValue(clipName, out AudioClip clip))
        {
            return clip;
        }
        else
        {
            // TODO : Addressable
            return Resources.Load<AudioClip>(GetClipPath(type, clipName)) ?? throw new MissingReferenceException($"{type} Clip not found for {clipName}");
        }
    }
    
    AudioMixer GetAudioMixer()
    {
        return Resources.Load<AudioMixer>("Sound/AudioMixer") ?? throw new MissingReferenceException($"AudioMixer not found");
    }
    public AudioMixerGroup GetEnviromentMixerGroup()
    {
        return _enviromentMixerGroup ?? GetAudioMixer().FindMatchingGroups("Master/Sfx/Enviroment")[0];
    }

    void SetMuteBGM(bool value)
    {
        _muteBGM = value;
        if (_muteBGM)
        {
            _audioMixer.SetFloat("BGM", MIN_VOLUME);
        }
        else
        {
            _audioMixer.SetFloat("BGM", GetVolume(_volumeBGM));
        }
    }
    void SetMuteSFX(bool value)
    {
        _muteSFX = value;
        if (_muteSFX)
        {
            _audioMixer.SetFloat("SFX", MIN_VOLUME);
        }
        else
        {
            _audioMixer.SetFloat("SFX", GetVolume(_volumeSFX));
        }
    }

    void SetVolumeBGM(float value)
    {
        _volumeBGM = value;
        if (_muteBGM) return;
        _audioMixer.SetFloat("BGM", GetVolume(_volumeBGM));
    }
    void SetVolumeSFX(float value)
    {
        _volumeSFX = value;
        if (_muteSFX) return;
        _audioMixer.SetFloat("SFX", GetVolume(_volumeSFX));
    }
    float GetVolume(float value)
    {
        return value / MAX_VALUE * (MAX_VOLUME - MIN_VOLUME) + MIN_VOLUME;
    }

    public void AddBGMSlider(UISoundBGMSlider slider)
    {
        if (_uiBGMSliderList.Contains(slider)) return;
        _uiBGMSliderList.Add(slider);
    }
    public void RemoveBGMSlider(UISoundBGMSlider slider)
    {
        if (_uiBGMSliderList.Contains(slider) == false) return;
        _uiBGMSliderList.Remove(slider);
    }
    public void AddBGMText(UISoundBGMText text)
    {
        if (_uiBGMTextList.Contains(text)) return;
        _uiBGMTextList.Add(text);
    }
    public void RemoveBGMText(UISoundBGMText text)
    {
        if (_uiBGMTextList.Contains(text) == false) return;
        _uiBGMTextList.Remove(text);
    }
    public void AddBGMToggle(UISoundBGMToggle toggle)
    {
        if (_uiBGMToggleList.Contains(toggle)) return;
        _uiBGMToggleList.Add(toggle);
    }
    public void RemoveBGMToggle(UISoundBGMToggle toggle)
    {
        if (_uiBGMToggleList.Contains(toggle) == false) return;
        _uiBGMToggleList.Remove(toggle);
    }

    void SetBGMSliderAndText()
    {
        foreach (var slider in _uiBGMSliderList)
        {
            if (slider.Value != _volumeBGM)
            {
                slider.SetValueWithoutNotify(_volumeBGM);
            }
        }
        foreach (var text in _uiBGMTextList)
        {
            text.SetString();
        }
    }
    void SetBGMToggle()
    {
        foreach (var toggle in _uiBGMToggleList)
        {
            if (toggle.IsOn == _muteBGM)
            {
                toggle.SetIsOnWithoutNotify(!_muteBGM);
            }
        }
        foreach (var slider in _uiBGMSliderList)
        {
            if (slider.Interactable == _muteBGM)
            {
                slider.SetInteractable(!_muteBGM);
            }
        }
    }

    public void AddSFXSlider(UISoundSFXSlider slider)
    {
        if (_uiSFXSliderList.Contains(slider)) return;
        _uiSFXSliderList.Add(slider);
    }
    public void RemoveSFXSlider(UISoundSFXSlider slider)
    {
        if (_uiSFXSliderList.Contains(slider) == false) return;
        _uiSFXSliderList.Remove(slider);
    }
    public void AddSFXText(UISoundSFXText text)
    {
        if (_uiSFXTextList.Contains(text)) return;
        _uiSFXTextList.Add(text);
    }
    public void RemoveSFXText(UISoundSFXText text)
    {
        if (_uiSFXTextList.Contains(text) == false) return;
        _uiSFXTextList.Remove(text);
    }
    public void AddSFXToggle(UISoundSFXToggle toggle)
    {
        if (_uiSFXToggleList.Contains(toggle)) return;
        _uiSFXToggleList.Add(toggle);
    }
    public void RemoveSFXToggle(UISoundSFXToggle toggle)
    {
        if (_uiSFXToggleList.Contains(toggle) == false) return;
        _uiSFXToggleList.Remove(toggle);
    }

    void SetSFXSliderAndText()
    {
        foreach (var slider in _uiSFXSliderList)
        {
            if (slider.Value != _volumeSFX)
            {
                slider.SetValueWithoutNotify(_volumeSFX);
            }
        }
        foreach (var text in _uiSFXTextList)
        {
            text.SetString();
        }
    }
    void SetSFXToggle()
    {
        foreach (var toggle in _uiSFXToggleList)
        {
            if (toggle.IsOn == _muteSFX)
            {
                toggle.SetIsOnWithoutNotify(!_muteSFX);
            }
        }
        foreach (var slider in _uiSFXSliderList)
        {
            if (slider.Interactable == _muteSFX)
            {
                slider.SetInteractable(!_muteSFX);
            }
        }
    }
}
