using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppCentralSDKVersionTracker : MonoBehaviour
{
    private static string aCMediaSDKPhase1Version = "3.1.1";

    // private static string aCMediaSDKPhase2Version = "0.1.10";
    private static string oneSignalVersion = "3.0.10"; // "3.0.4";
    private static string appLovinVersion = "11.9.0";
    private static string gameAnalyticsVersion = "7.4.1";
    private static string appsFlyerVersion = "6.10.10";
    private static string adjustSDKVersion = "4.33.0";
    private static string smartLookVersion = "1.0";
    private static string brightSDKVeriosn = "1.397.395"; // previous version "1.372.299";
    private static string brightSDKImplemetationVersion = "5.1.0";
    private static string playOnSDKVersion = "2.0.9";

    public static string ACSDKVersion
    {
        get => aCMediaSDKPhase1Version;
        private set => aCMediaSDKPhase1Version = value;
    }
    public static string ACMediaSDKPhase2Version
    {
        get => aCMediaSDKPhase1Version;
        private set => aCMediaSDKPhase1Version = value;
    }
    public static string OneSignalVersion
    {
        get => oneSignalVersion;
        private set => oneSignalVersion = value;
    }
    public static string AppLovinVersion
    {
        get => appLovinVersion;
        private set => appLovinVersion = value;
    }
    public static string GameAnalyticsVersion
    {
        get => gameAnalyticsVersion;
        private set => gameAnalyticsVersion = value;
    }
    public static string SmartLookVersion
    {
        get => smartLookVersion;
        private set => smartLookVersion = value;
    }
    public static string AppsFlyerVersion
    {
        get => appsFlyerVersion;
        private set => appsFlyerVersion = value;
    }

    public static string AdjustSDKVersion
    {
        get => adjustSDKVersion;
        private set => adjustSDKVersion = value;
    }
    public static string BrightSDKVeriosn
    {
        get => brightSDKVeriosn;
        private set => brightSDKVeriosn = value;
    }

    public static string BrightSDKImplemetationVersion
    {
        get => brightSDKImplemetationVersion;
    }

    public static string PlayOnSDKVeriosn
    {
        get => playOnSDKVersion;
        private set => playOnSDKVersion = value;
    }
}
