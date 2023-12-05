using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(LocalizationTableSO))]
[CanEditMultipleObjects]
public class LocalizationTableSOEditor : Editor
{
    LocalizationTableSO _localizationTableSO;

    public override void OnInspectorGUI()
    {
        _localizationTableSO = (LocalizationTableSO)target;

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.EnumPopup(_localizationTableSO.language);

        foreach (var localizedString in _localizationTableSO.localizedStrings)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.TextField(localizedString.Key);
            EditorGUILayout.TextField(localizedString.Value);
            GUILayout.EndHorizontal();
        }
        EditorGUI.EndDisabledGroup();
    }
}

#endif