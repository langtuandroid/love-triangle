using UnityEngine;
using System.Collections.Generic;
using System;
using AppCentralAPI;


//#if UNITY_EDITOR
//using UnityEditor;
//#endif

namespace AppCentralCore
{
    public class ScriptableDefineSymbolsPrebuilder : MonoBehaviour
    {
#if UNITY_EDITOR

        public static void SetSDS(newSetting settings)
        {

            List<string> DEFINES = new List<string>();
            List<string> UNDEFINES = new List<string>();





            if (settings.canUseAppsFlyer) DEFINES.Add(AppCentralPlugins.AC_APPSFLYER.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_APPSFLYER.ToString());


            if (settings.adjustSDKConfig.UseAdjustSDK) DEFINES.Add(AppCentralPlugins.AC_ADJUST.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_ADJUST.ToString());


            if (settings.canUseGameAnalytics) DEFINES.Add(AppCentralPlugins.AC_GAMEANALYTICS.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_GAMEANALYTICS.ToString());


            if (settings.canUseSmartLook) DEFINES.Add(AppCentralPlugins.AC_SMARTLOOK.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_SMARTLOOK.ToString());

            
            if (settings.UseOneSignal) DEFINES.Add(AppCentralPlugins.AC_ONESIGNAL.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_ONESIGNAL.ToString());


            if (settings.UseAppLovin) DEFINES.Add(AppCentralPlugins.AC_APPLOVIN.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_APPLOVIN.ToString());


            if (settings.UseAppCentralSDKPhase2) DEFINES.Add(AppCentralPlugins.AC_ADS_SDK.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_ADS_SDK.ToString()); 
            
            
            if (settings.UseInApps) DEFINES.Add(AppCentralPlugins.AC_IAPS.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_IAPS.ToString());


            if (settings.UseBrightDataSDK) DEFINES.Add(AppCentralPlugins.AC_BRIGHTDATA.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_BRIGHTDATA.ToString());

            if (settings.playOnConfig.UsePlayOnSDK) DEFINES.Add(AppCentralPlugins.AC_PLAYON.ToString());
            else UNDEFINES.Add(AppCentralPlugins.AC_PLAYON.ToString());

            DEFINES.Add(AppCentralPlugins.AC_MEDIA_SDK.ToString());


            EnsureAppCentralDefine.EnsureScriptingDefineSymbol(DEFINES.ToArray(), UNDEFINES.ToArray());
        }

#endif
    }



}