using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ApplicationInfo
{
    public string version;
    public RuntimePlatform platform;
    public bool isEmulator;
    public DeviceType deviceType;

    public void Initialize()
    {
        version = Application.version;
        platform = Application.platform;
        isEmulator = CheckEmulator();
        deviceType = SystemInfo.deviceType;
    }

    bool CheckEmulator()
    {
        if (platform == RuntimePlatform.Android)
        {
            AndroidJavaClass osBuild;
            osBuild = new AndroidJavaClass("android.os.Build");
            string fingerPrint = osBuild.GetStatic<string>("FINGERPRINT");
            string model = osBuild.GetStatic<string>("MODEL");
            string menufacturer = osBuild.GetStatic<string>("MANUFACTURER");
            string brand = osBuild.GetStatic<string>("BRAND");
            string device = osBuild.GetStatic<string>("DEVICE");
            string product = osBuild.GetStatic<string>("PRODUCT");

            return fingerPrint.Contains("generic")
                || fingerPrint.Contains("unknown")
                || model.Contains("google_sdk")
                || model.Contains("Emulator")
                || model.Contains("Android SDK built for x86")
                || menufacturer.Contains("Genymotion")
                || (brand.Contains("generic") && device.Contains("generic"))
                || product.Equals("google_sdk")
                || product.Equals("unknown");
        }
        if (platform == RuntimePlatform.OSXEditor)
        {
            return true;
        }
        return false;
    }
}
