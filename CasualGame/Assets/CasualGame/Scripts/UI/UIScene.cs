using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIScene : UIBase
{
    protected virtual void Awake()
    {
        SetCanvasSortOrder(-1);
        UIManager.Instance.SetSceneUI(this);
    }
}
