using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundSFXUI : MonoBehaviour
{
    AudioSource m_audioSource;

    public void Initialize(AudioMixerGroup audioMixerGroup)
    {
        m_audioSource = GetComponent<AudioSource>();

        m_audioSource.outputAudioMixerGroup = audioMixerGroup;
        m_audioSource.loop = false;
        m_audioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip)
    {
        m_audioSource.PlayOneShot(clip);
    }
}
