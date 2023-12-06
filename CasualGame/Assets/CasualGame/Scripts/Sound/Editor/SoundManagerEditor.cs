using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SoundManager))]
[CanEditMultipleObjects]
public class SoundManagerEditor : Editor
{
    SoundManager _soundManager;
    float _volumeBGM;
    float _volumeSFX;

    bool _muteBGM;
    bool _muteSFX;

    public override void OnInspectorGUI()
    {
        _soundManager = (SoundManager)target;
        base.OnInspectorGUI();

        if (Application.isPlaying == false) return;

        serializedObject.Update();
        DrawBGM();
        DrawSFX();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawBGM()
    {
        GUILayout.Space(5);
        GUILayout.BeginVertical();
       
        EditorGUILayout.LabelField("BGM");
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Mute");
        EditorGUI.BeginChangeCheck();
        _muteBGM = EditorGUILayout.Toggle(_soundManager.MuteBGM);
        if (EditorGUI.EndChangeCheck())
        {
            _soundManager.MuteBGM = _muteBGM;
            serializedObject.ApplyModifiedProperties();
        }
        GUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
        _volumeBGM = EditorGUILayout.Slider(_soundManager.VolumeBGM, 0f, _soundManager.MAX_VALUE);
        if (EditorGUI.EndChangeCheck())
        {
            _soundManager.VolumeBGM = _volumeBGM;
            serializedObject.ApplyModifiedProperties();
        }
        GUILayout.EndVertical();
    }

    void DrawSFX()
    {
        GUILayout.Space(5);
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("SFX");
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Mute");
        EditorGUI.BeginChangeCheck();
        _muteSFX = EditorGUILayout.Toggle(_soundManager.MuteSFX);
        if (EditorGUI.EndChangeCheck())
        {
            _soundManager.MuteSFX = _muteSFX;
            serializedObject.ApplyModifiedProperties();
        }
        GUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
        _volumeSFX = EditorGUILayout.Slider(_soundManager.VolumeSFX, 0f, _soundManager.MAX_VALUE);
        if (EditorGUI.EndChangeCheck())
        {
            _soundManager.VolumeSFX = _volumeSFX;
            serializedObject.ApplyModifiedProperties();
        }
        GUILayout.EndVertical();
    }
}
#endif