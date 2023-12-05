using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundSFXEffect : MonoBehaviour
{
    AudioSource m_audioSource;

    public void Initialize(AudioMixerGroup audioMixerGroup)
    {
        m_audioSource = GetComponent<AudioSource>();

        m_audioSource.outputAudioMixerGroup = audioMixerGroup;
        m_audioSource.loop = false;
        m_audioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip, Vector3 position)
    {
        transform.position = position;

        m_audioSource.clip = clip;
        m_audioSource.Play();

        Invoke(nameof(Release), clip.length);
    }

    void Release()
    {
        SoundManager.Instance.ReleaseEffect(this);
    }
}
