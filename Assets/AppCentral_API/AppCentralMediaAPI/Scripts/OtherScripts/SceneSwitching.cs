using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.iOS;
using AppCentralCore;
using System;

namespace AppCentralAPI
{
    public class SceneSwitching : MonoBehaviour
    {
        private int NextSceneIndex = 1;
        private int currentSceneIndex;

        public Image LoadingBar;

        private bool WaitForAppCentral_API = true;
        private bool WaitForRoblox_API = true;
        private bool KeepWaiting = true;

        private float MaxFillLimit_ACSDK = 0.4f, MaxFillLimit_RBSDK = 0.75f,
            LoadingFinishTime = 5;

        private AsyncOperation async;

        private void OnEnable()
        {
            AppCentralUnityApi_Internal.OnLoadEvent += AppCentralApiInitializationCompleted;


#if AC_ROBLOX

            ACRBCore.RobloxEvensManager.OnRobloxDatainitialized += RBApiInitializationCompleted;

#endif

        }

        private void Start()
        {
            StartSceneLoading();
        }

        private void StartSceneLoading()
        {
            KeepWaiting = true;
            StartCoroutine(loadingSceneAsync());
        }

        private void AppCentralApiInitializationCompleted()
        {
            WaitForAppCentral_API = false;

            ACLogger.UserDebug("AppCentralApiInitializationCompleted");
            //KeepWaiting = false;
        }

        private void RBApiInitializationCompleted()
        {
            WaitForRoblox_API = false;
            ACLogger.UserDebug("RBApiInitializationCompleted");
            //KeepWaiting = false;
        }

        IEnumerator loadingSceneAsync()
        {
            ACLogger.UserDebug("SceneSwitching Start");

            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            ACLogger.UserDebug("SceneSwitching WaitForAppCentral_API=" + WaitForAppCentral_API);

            float T = 0;
            LoadingBar.fillAmount = T;

            while (WaitForAppCentral_API)
            {
                if (T <= MaxFillLimit_ACSDK / 2)
                {
                    T += Time.deltaTime / LoadingFinishTime;
                    LoadingBar.fillAmount = T;
                }

                yield return null;
            }

            async = SceneManager.LoadSceneAsync(NextSceneIndex);
            async.allowSceneActivation = false;
            KeepWaiting = true;

            while (KeepWaiting)
            {
                if (T <= MaxFillLimit_ACSDK)
                {
                    T += Time.deltaTime / LoadingFinishTime;
                    LoadingBar.fillAmount = T;
                }
                else
                {
                    KeepWaiting = false;
                }

                yield return null;
            }




#if AC_ROBLOX

            LoadingBar.fillAmount = MaxFillLimit_RBSDK;

            while (WaitForRoblox_API)
            {
                yield return null;
            }

#endif


            LoadingBar.fillAmount = 0.95f;

            yield return new WaitForSeconds(0.5f);

            ActivateNextScene();
        }

        bool oneTime = true;

        public void ActivateNextScene()
        {
            ACLogger.UserDebug("Scene Activation Requested");

            if (oneTime)
            {
                oneTime = false;
                async.allowSceneActivation = true;
                ACLogger.UserDebug("Activated Next Scene");
            }
        }

        private void OnDisable()
        {
            AppCentralUnityApi_Internal.OnLoadEvent -= AppCentralApiInitializationCompleted;

#if AC_ROBLOX

            ACRBCore.RobloxEvensManager.OnRobloxDatainitialized -= RBApiInitializationCompleted;

#endif
        }
    }
}
