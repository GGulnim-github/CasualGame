using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFPSDisplay : MonoBehaviour
{
    public enum DISPLAY_POS_H { LEFT, RIGHT };
    public enum DISPLAY_POS_V { TOP, BOTTOM };

    float deltaTime = 0.0f;

    [SerializeField] DISPLAY_POS_H dispPosH = DISPLAY_POS_H.LEFT;
    [SerializeField] DISPLAY_POS_V dispPosV = DISPLAY_POS_V.TOP;

    [SerializeField] private bool show = true;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if (show)
        {
            int fontSize = Screen.height * 2 / 100;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0.0f, 0.0f, Screen.width, Screen.height);

            if (dispPosH == DISPLAY_POS_H.LEFT && dispPosV == DISPLAY_POS_V.TOP)
                style.alignment = TextAnchor.UpperLeft;
            else if (dispPosH == DISPLAY_POS_H.LEFT && dispPosV == DISPLAY_POS_V.BOTTOM)
                style.alignment = TextAnchor.LowerLeft;
            else if (dispPosH == DISPLAY_POS_H.RIGHT && dispPosV == DISPLAY_POS_V.TOP)
                style.alignment = TextAnchor.UpperRight;
            else if (dispPosH == DISPLAY_POS_H.RIGHT && dispPosV == DISPLAY_POS_V.BOTTOM)
                style.alignment = TextAnchor.LowerRight;

            style.fontSize = fontSize;
            style.normal.textColor = Color.white;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }

}
