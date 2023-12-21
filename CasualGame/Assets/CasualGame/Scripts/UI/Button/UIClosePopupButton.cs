using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClosePopupButton : MonoBehaviour
{
    Button m_Button;
    UIPopup _popup;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        _popup = GetComponentInParent<UIPopup>();

        m_Button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        _popup.Close();
    }
}
