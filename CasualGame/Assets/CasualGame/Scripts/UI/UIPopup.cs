using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPopup : UIBase
{
    [SerializeField] Button _closeButton;

    private void Awake()
    {
        _closeButton.onClick.AddListener(Close);
    }

    public virtual void Close()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
