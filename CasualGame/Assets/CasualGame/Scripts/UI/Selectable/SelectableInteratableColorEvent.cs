using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableInteratableColorEvent : SelectableInteractableEvent
{
    public Graphic targetGraphic;

    public Color onColor;
    public Color offColor;

    public override void OnAction()
    {
        if (targetGraphic != null)
        {
            targetGraphic.color = onColor;
        }
    }
    public override void OffAction()
    {
        if (targetGraphic != null)
        {
            targetGraphic.color = offColor;
        }
    }
}
