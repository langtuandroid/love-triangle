using com.adjust.sdk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppCentralCore
{
    public class AppCentralAdjustController : MonoBehaviour
    {
        public const string Implementatin = "AC_ADJUST";

        public static bool IsAdjustEnableFromServer { get => PlayerPrefs.GetInt(AppCentralPrefsManager.adjust_initialize_adjust) == 1 ? true : false; }

#if AC_ADJUST
        public static com.adjust.sdk.AdjustEnvironment adjustEnvironment { get => PlayerPrefs.GetInt(AppCentralPrefsManager.adjust_environment) == 1 ? com.adjust.sdk.AdjustEnvironment.Production : com.adjust.sdk.AdjustEnvironment.Sandbox; }
#endif

        public static void InitializeAdjust()
        {
#if AC_ADJUST


	        ACLogger.UserDebug($":{Implementatin} initiaization is Called");

            newSetting settings = AppCentralSettings.LoadSetting();
            AdjustSDKConfig adjustSDKConfig = settings.adjustSDKConfig;

            if (!settings.canUseAppsFlyer)
            {
                ACLogger.UserDebug($":{Implementatin} is Disabled from AppCentral Settings");
                return;
            }

            ACLogger.UserDebug($":{Implementatin} is AdjustEnableFromServe: " + IsAdjustEnableFromServer);
            ACLogger.UserDebug($":{Implementatin} server side adjustEnvironment: " + adjustEnvironment);


            ACLogger.UserDebug($":{Implementatin} is Enable from AppCentral Settings with AppToken=" + adjustSDKConfig.YourAppToken);
            ACLogger.UserDebug($":{Implementatin} is Enable from AppCentral Settings with Environmnet=" + adjustEnvironment.ToString());
            ACLogger.UserDebug($":{Implementatin} is Enable from AppCentral Settings with freeTrialToken=" + adjustSDKConfig.freeTrialToken);
            ACLogger.UserDebug($":{Implementatin} is Enable from AppCentral Settings with fullScbscriptionPurchaseToken=" + adjustSDKConfig.fullScbscriptionPurchaseToken);

            ACLogger.UserDebug($":{Implementatin} is Enable from AppCentral Settings with AppSecret=" + adjustSDKConfig.AppSecret_SecretID);
            ACLogger.UserDebug($":{Implementatin} is Enable from AppCentral Settings with AppSecret_Info1=" + adjustSDKConfig.AppSecret_Info1);
            ACLogger.UserDebug($":{Implementatin} is Enable from AppCentral Settings with AppSecret_Info2=" + adjustSDKConfig.AppSecret_Info2);
            ACLogger.UserDebug($":{Implementatin} is Enable from AppCentral Settings with AppSecret_Info3=" + adjustSDKConfig.AppSecret_Info3);
            ACLogger.UserDebug($":{Implementatin} is Enable from AppCentral Settings with AppSecret_Info4=" + adjustSDKConfig.AppSecret_Info4);


            if (IsAdjustEnableFromServer)
            {
                com.adjust.sdk.Adjust adjust = AppCentralUnityApi_Internal.Instance.appcentral_ReferenceManager.AdjustPrefeb.GetComponent<com.adjust.sdk.Adjust>();

                adjust.appToken = adjustSDKConfig.YourAppToken;

                adjust.environment = adjustEnvironment;
                
                adjust.secretId = adjustSDKConfig.AppSecret_SecretID;

                adjust.info1 = adjustSDKConfig.AppSecret_Info1;
                adjust.info2 = adjustSDKConfig.AppSecret_Info2;
                adjust.info3 = adjustSDKConfig.AppSecret_Info3;
                adjust.info4 = adjustSDKConfig.AppSecret_Info4;

                Instantiate(adjust);
            }

#else

            ACLogger.UserDebug($":{Implementatin} disabled in the AppCentral SDK Setting");

#endif
        }
    }
}