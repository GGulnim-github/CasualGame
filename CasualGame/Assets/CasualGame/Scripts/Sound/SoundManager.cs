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
        }
    }

    float _volumeSFX;
    public float VolumeSFX
    {
        get { return _volumeSFX; }
        set
        {
            SetVolumeSFX(value);
        }
    }

    bool _muteBGM;
    public bool MuteBGM
    {
        get { return _muteBGM; }
        set
        {
            SetMuteBGM(value);;
        }
    }

    bool _muteSFX;
    public bool MuteSFX
    {
        get { return _muteSFX; }
        set
        {
            SetMuteSFX(value);
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
        _soundBGM.Play(GetClip(SoundType.BGM, clipName));
    }
    public void StopBGM()
    {
        _soundBGM.Stop();
    }

    public void PlayEffect(string clipName, Vector3 position)
    {
        SoundSFXEffect soundEffect = _sfxEffectPool.Get();
        soundEffect.Play(GetClip(SoundType.SFX, SoundSFXType.Effect, clipName), position);
    }
    public void ReleaseEffect(SoundSFXEffect effect)
    {
        _sfxEffectPool.Release(effect);
    }

    public void PlayUI(string clipName)
    {
        _soundUI.Play(GetClip(SoundType.SFX, SoundSFXType.UI, clipName));
    }

    string GetClipPath(SoundType type, string clipName)
    {
        return $"Sound/{type}/{clipName}";
    }

    string GetClipPath(SoundType type, SoundSFXType sfxType, string clipName)
    {
        return $"Sound/{type}/{sfxType}/{clipName}";
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
    AudioClip GetClip(SoundType type, SoundSFXType sfxType, string clipName)
    {
        if (_preloadClip.TryGetValue(clipName, out AudioClip clip))
        {
            return clip;
        }
        else
        {
            // TODO : Addressable
            return Resources.Load<AudioClip>(GetClipPath(type, sfxType, clipName)) ?? throw new MissingReferenceException($"{type} Clip not found for {clipName}");
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
}
