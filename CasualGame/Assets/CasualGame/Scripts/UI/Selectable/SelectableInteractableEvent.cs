using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectableInteractableEvent : MonoBehaviour
{
    public abstract void OnAction();
    public abstract void OffAction();
}
