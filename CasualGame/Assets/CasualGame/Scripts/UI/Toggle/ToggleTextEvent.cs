using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleTextEvent : ToggleEvent
{
    public TextMeshProUGUI targetText;
    public bool useLocalization;

    public string onString;
    public string offString;

    public override void OnAction()
    {
        if (targetText != null)
        {
            if (useLocalization)
            {
                if (targetText.TryGetComponent(out UILocalizedText localizedText))
                {
                    localizedText.SetString(onString);
                }
                else
                {
                    targetText.text = onString;
                }
            }
            else
            {
                targetText.text = onString;
            }
        }
    }
    public override void OffAction()
    {
        if (targetText != null)
        {
            if (useLocalization)
            {
                if (targetText.TryGetComponent(out UILocalizedText localizedText))
                {
                    localizedText.SetString(offString);
                }
                else
                {
                    targetText.text = offString;
                }
            }
            else
            {
                targetText.text = offString;
            }
        }
    }
}
