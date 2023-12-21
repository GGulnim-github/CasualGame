using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOpenPopupButton : MonoBehaviour
{
    public string popupName;

    Button m_Button;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        UIManager.Instance.OpenPopupUI(popupName);
    }
}
