using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundBGM : MonoBehaviour
{
    AudioSource m_audioSource;

    public void Initialize(AudioMixerGroup audioMixerGroup)
    {
        m_audioSource = GetComponent<AudioSource>();

        m_audioSource.outputAudioMixerGroup = audioMixerGroup;
        m_audioSource.loop = true;
        m_audioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip)
    {
        m_audioSource.clip = clip;
        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }
        m_audioSource.Play();
    }

    public void Stop()
    {
        m_audioSource.Stop();
    }
}
