using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSettingSystem
{
    public ApplicationTargetFrame TargetFrame { get; private set; }
    public ApplicationGraphicsQuality GraphicsQuality { get; private set; }

    public void Initialize()
    {
        Application.runInBackground = true;
        SetTargetFrameRate(ApplicationTargetFrame.Auto);
        SetGraphicsQuality(ApplicationGraphicsQuality.Medium);
    }

    /// <summary>
    ///  targetFrameRate = -1 (auto)
    /// </summary>
    /// <param name="targetFrameRate"></param>
    public void SetTargetFrameRate(ApplicationTargetFrame targetFrameRate)
    {
        TargetFrame = targetFrameRate;
        switch (targetFrameRate)
        {
            case ApplicationTargetFrame.Auto:
                Application.targetFrameRate = -1;
                break;
            case ApplicationTargetFrame.FPS30:
                Application.targetFrameRate = 30;
                break;
            case ApplicationTargetFrame.FPS60:
                Application.targetFrameRate = 60;
                break;
        }
    }

    public void SetGraphicsQuality(ApplicationGraphicsQuality graphicsQuality)
    {
        GraphicsQuality = graphicsQuality;
        switch (graphicsQuality)
        {
            case ApplicationGraphicsQuality.Low:
                QualitySettings.SetQualityLevel(0);
                break;
            case ApplicationGraphicsQuality.Medium:
                QualitySettings.SetQualityLevel(1);
                break;
            case ApplicationGraphicsQuality.High:
                QualitySettings.SetQualityLevel(2);
                break;
        }
    }
}
