using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSettingSystem
{
    public ApplicationTargetFrame TargetFrame { get; private set; }

    public void Initialize()
    {
        Application.runInBackground = true;
        SetTargetFrameRate(ApplicationTargetFrame.Auto);
    }

    /// <summary>
    ///  targetFrameRate = -1 (auto)
    /// </summary>
    /// <param name="targetFrameRate"></param>
    public void SetTargetFrameRate(ApplicationTargetFrame targetFrameRate)
    {
        TargetFrame = targetFrameRate;
        Application.targetFrameRate = (int)targetFrameRate;
    }
}
