using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(Toggle))]
public class ToggleEventTrigger : MonoBehaviour
{
    [Serializable]
    public class ToggleOnEvent : UnityEvent { }

    [SerializeField]
    [FormerlySerializedAs("onEvent")]
    ToggleOnEvent _onEvent = new();

    [Serializable]
    public class ToggleOffEvent : UnityEvent { }

    [SerializeField]
    [FormerlySerializedAs("offEvent")]
    ToggleOffEvent _offEvent = new();

    Toggle m_Toggle;

    private void Awake()
    {
        m_Toggle = GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        OnValueChanged(m_Toggle.isOn);
    }

    void OnValueChanged(bool value)
    {
        if (value)
        {
            _onEvent?.Invoke();
        }
        else
        {
            _offEvent?.Invoke();
        }
    }
}
