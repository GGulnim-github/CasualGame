using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleEventTrigger : MonoBehaviour
{
    ToggleEvent[] _toggleEvents;

    delegate void OnActionDelegate();
    OnActionDelegate _onAction;

    delegate void OffActionDelegate();
    OnActionDelegate _offAction;

    public void Initialize()
    {
        _toggleEvents = GetComponentsInChildren<ToggleEvent>();

        foreach(ToggleEvent toggleEvent in _toggleEvents)
        {
            _onAction += toggleEvent.OnAction;
            _offAction += toggleEvent.OffAction;
        }
    }

    public void Set(bool value)
    {
        if (value)
        {
            OnAction();
        }
        else
        {
            OffAction();
        }
    }

    void OnAction()
    {
        _onAction?.Invoke();
    }

    void OffAction()
    {
        _offAction?.Invoke();
    }
}
