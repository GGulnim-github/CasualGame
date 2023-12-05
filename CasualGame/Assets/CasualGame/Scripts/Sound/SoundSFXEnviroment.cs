using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSFXEnviroment : MonoBehaviour
{
    AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        if (m_audioSource == null)
        {
            m_audioSource.outputAudioMixerGroup = SoundManager.Instance.GetEnviromentMixerGroup();
        }
        m_audioSource.playOnAwake = false;
        m_audioSource.loop = false;
    }

    private void Start()
    {
        m_audioSource.Play();
    }
}
