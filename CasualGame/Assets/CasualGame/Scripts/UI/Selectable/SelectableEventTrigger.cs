using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectableEventTrigger : MonoBehaviour
{
    SelectableInteractableEvent[] _interactableEvents;

    delegate void OnInteractableDelegate();
    OnInteractableDelegate _onInteractable;

    delegate void OffInteractableDelegate();
    OffInteractableDelegate _offInteractable;

    public void Initialize()
    {
        _interactableEvents = GetComponentsInChildren<SelectableInteractableEvent>();

        foreach (SelectableInteractableEvent interactableEvent in _interactableEvents)
        {
            _onInteractable += interactableEvent.OnAction;
            _offInteractable += interactableEvent.OffAction;
        }
    }

    public void SetInteractable(bool value)
    {
        if (value)
        {
            OnInteractable();
        }
        else
        {
            OffInteractable();
        }
    }

    void OnInteractable()
    {
        _onInteractable?.Invoke();
    }

    void OffInteractable()
    {
        _offInteractable?.Invoke();
    }
}
