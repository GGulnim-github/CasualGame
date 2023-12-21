using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPopup : UIBase
{
    public virtual void Close()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
