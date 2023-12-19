using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPopup : UIBase
{
    public virtual void Close()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
