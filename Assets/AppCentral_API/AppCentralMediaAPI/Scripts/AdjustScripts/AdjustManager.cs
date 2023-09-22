using System;
using System.Collections;
using System.Collections.Generic;
using AppCentralCore;
using UnityEngine;
using com.adjust.sdk;
public class AdjustManager : MonoBehaviour
{
#if AC_ADJUST

    public static AdjustManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public AdjustEnvironment adjustEnvironment;

    public string YourAppToken = "oeqajaiswsg0";
    public string freeTrialToken = "fnfgo9";
    public string fullScbscriptionPurchaseToken = "obqmi4";


    public void InitializeSettings()
    {
        newSetting settings = AppCentralSettings.LoadSetting();
        AdjustSDKConfig adjustSDKConfig = settings.adjustSDKConfig;

        YourAppToken = adjustSDKConfig.YourAppToken;
        adjustEnvironment = AppCentralAdjustController.adjustEnvironment;
        fullScbscriptionPurchaseToken = adjustSDKConfig.fullScbscriptionPurchaseToken;
    }


    private void OnEnable()
    {
        AppCentralATTController.ATT_AuthorizationStatusUpdate += EventReceiver;
    }

    private void EventReceiver(bool aTT_Status)
    {

        AdjustConfig adjustConfig = new AdjustConfig(YourAppToken, adjustEnvironment);
        adjustConfig.setSendInBackground(true);
        com.adjust.sdk.Adjust.start(adjustConfig);
    }

    private void OnDisable()
    {

        AppCentralATTController.ATT_AuthorizationStatusUpdate -= EventReceiver;
    }

#endif
}
