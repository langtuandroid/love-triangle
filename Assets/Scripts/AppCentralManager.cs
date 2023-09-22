using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppCentralAPI;
using GameAnalyticsSDK;

public class AppCentralManager : MonoBehaviour
{
    public static AppCentralManager Instance = null;
    public static bool isInitialized = false;

    void OnEnable() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void Start() {
        if (!isInitialized) {
            AppCentral.StartPlay();
            isInitialized = true;
        }
        
    }

    public void CallStartEvent(int p_level) {
        AppCentral.SetLevelStartID(p_level);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, LGGUtility.FormatAdsLevelName(p_level));
    }

    public void CallWinEvent(int p_level)
    {
        AppCentral.OnLevelComplete();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, LGGUtility.FormatAdsLevelName(p_level));
    }

    public void CallFailEvent(int p_level) {
        //AppCentral.OnLevelFail();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, LGGUtility.FormatAdsLevelName(p_level));
    }

}
 