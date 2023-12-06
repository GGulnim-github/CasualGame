using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(LocalizationManager))]
[CanEditMultipleObjects]
public class LocalizationManagerEditor : Editor
{
    LocalizationManager _localizationManager;

    public override void OnInspectorGUI()
    {
        _localizationManager = (LocalizationManager)target;
        base.OnInspectorGUI();

        if (Application.isPlaying == false) return;

        DrawCurrentLanguage();
        DrawLocalizationButton();
    }

    void DrawScriptField()
    {
        GUILayout.BeginVertical();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((LocalizationManager)target), typeof(LocalizationManager), false);
        EditorGUI.EndDisabledGroup();
        GUILayout.EndVertical();
    }

    void DrawCurrentLanguage()
    {
        GUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.EnumPopup(_localizationManager.Language);
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
    }
    void DrawLocalizationButton()
    {
        if (Application.isPlaying)
        {
            GUILayout.BeginHorizontal();
            for (LocalizationLanguage language = LocalizationLanguage.Korean; language < (LocalizationLanguage)System.Enum.GetValues(typeof(LocalizationLanguage)).Length; language++)
            {
                if (GUILayout.Button($"{language}"))
                {
                    _localizationManager.Language = language;
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif
