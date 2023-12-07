using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ApplicationManager : PersistentSingleton<ApplicationManager>
{
    [ReadOnly][SerializeField] ApplicationInfo _info = new();
    ApplicationLogSystem _logSystem = new();
    ApplicationSettingSystem _settingSystem = new();

    public string Version { get { return _info.version; } }
    public RuntimePlatform Platform { get { return _info.platform; } }
    public bool IsEmulator { get { return _info.isEmulator; } }

    public ApplicationTargetFrame TargetFrame 
    { 
        get { return _settingSystem.TargetFrame; }
        set { _settingSystem.SetTargetFrameRate(value); }
    }

    private void OnEnable()
    {
        _logSystem.OnEnable();
    }
    private void OnDisable()
    {
        _logSystem.OnDisable();
    }

    public void Initialize()
    {
        _info.Initialize();
        _settingSystem.Initialize();

        if (IsRooted())
        {
            Quit();
        }
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    bool IsRooted()
    {
        bool isRoot = false;

        if (Platform == RuntimePlatform.Android)
        {
            if (IsRootedPrivate("/system/bin/su"))
                isRoot = true;
            if (IsRootedPrivate("/system/xbin/su"))
                isRoot = true;
            if (IsRootedPrivate("/system/app/SuperUser.apk"))
                isRoot = true;
            if (IsRootedPrivate("/data/data/com.noshufou.android.su"))
                isRoot = true;
            if (IsRootedPrivate("/sbin/su"))
                isRoot = true;
        }
        return isRoot;
    }
    bool IsRootedPrivate(string path)
    {
        bool boolTemp = false;

        if (File.Exists(path))
        {
            boolTemp = true;
        }

        return boolTemp;
    }
}
