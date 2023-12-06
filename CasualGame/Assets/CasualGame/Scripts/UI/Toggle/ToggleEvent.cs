using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToggleEvent : MonoBehaviour
{
    public abstract void OnAction();
    public abstract void OffAction();
}
