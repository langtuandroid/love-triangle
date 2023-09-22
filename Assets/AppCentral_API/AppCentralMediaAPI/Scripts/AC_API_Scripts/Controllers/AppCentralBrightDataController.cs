using System.Collections.Generic;
using System.Collections;
using AppCentralAPI;
using UnityEngine;
using System.Linq;
using System;

#if AC_BRIGHTDATA
using Brdsdk;
#endif

namespace AppCentralCore
{
    public class AppCentralBrightDataController : MonoBehaviour
    {
        public static AppCentralBrightDataController instance;

        private const string Imple = "[BD] ";

        private void Awake()
        {
            instance = this;
        }

        private const string ACBD_UserConsentPref = "ACBD_UserConsent";

        int CurrentlevelID = 0;
        bool isInitialized = false;
        bool skipOneTime = true;
        string opt_in_mode = "bright_data";

        static bool is_opt_out = false;
        static bool is_firstLaunch = false;
        static bool is_showingConsent = false;
        static bool is_silent_opt_in = false;
        static bool is_silent_opt_in_onetime = false;

        const string BD_FirstLaunch_Pref = "BD_FirstLaunch";
        const string BD_OneTime_Pref = "BD_OneTime";

        bool IsEnableFromServer = false;
        bool IspreviouslyAllowed = false;
        bool IsAValidLevel = false;
        bool IsEnabledFromACSettings = false;

        BrightDataScreenType brightDataScreenType;

        Action _onCloseCallback;

        void OnEnable()
        {
            AppCentralUnityApi_Internal.RecievedAppCentralApiResponse +=
                RecievedAppCentralApiResponse;
            //AppCentralUnityApi_Internal.OnLevelCompleteEvent += OnLevelComplete;
        }

        void OnDisable()
        {
            AppCentralUnityApi_Internal.RecievedAppCentralApiResponse -=
                RecievedAppCentralApiResponse;
            //AppCentralUnityApi_Internal.OnLevelCompleteEvent -= OnLevelComplete;
        }

        private void Start()
        {
#if UNITY_EDITOR

            skipOneTime = true;
            choiceChanged(0);
#endif
        }

        void RecievedAppCentralApiResponse(bool responce)
        {
            CurrentlevelID = 0; // If one of the values in brightdata_show_in_levels is "0", show the BrightData permission as soon as the loading screen ends and the game scene loads (Make sure the brightData initializaiton is happening before showing the dynamic paywall when the showLocation is equal to onLoad).

            is_firstLaunch = PlayerPrefs.GetInt(BD_FirstLaunch_Pref, 0) == 0 ? true : false;

            AppCentralCore.newSetting setting = AppCentralCore.AppCentralSettings.LoadSetting();
            IsEnabledFromACSettings = setting.UseBrightDataSDK;

            IsEnableFromServer =
                PlayerPrefs.GetInt(AppCentralPrefsManager.brightdata_show_on_start) == 1
                    ? true
                    : false;
            IspreviouslyAllowed =
                PlayerPrefs.GetInt(AppCentralPrefsManager.brightdata_previously_allowed) == 1
                    ? true
                    : false;
            IsAValidLevel = IsValideLevel(CurrentlevelID);

            ACLogger.UserDebug(Imple + "is_firstLaunch: " + is_firstLaunch);
            ACLogger.UserDebug(Imple + "IsEnableFromServer: " + IsEnableFromServer);
            ACLogger.UserDebug(Imple + "IsAValidLevel: " + IsAValidLevel);
            ACLogger.UserDebug(Imple + "IspreviouslyAllowed: " + IspreviouslyAllowed);

            skipOneTime = true;

#if UNITY_EDITOR
            isInitialized = true;
            IsEnableFromServer = true;
            IspreviouslyAllowed = true;
            is_firstLaunch = true;
#endif

            if (!IsEnableFromServer || !IsEnabledFromACSettings)
            {
                ACLogger.UserDebug(
                    Imple + "NOT initializing becasue IsEnableFromServer: " + IsEnableFromServer
                );
                ACLogger.UserDebug(
                    Imple
                        + "NOT initializing becasue  IsEnabledFromACSettings: "
                        + IsEnabledFromACSettings
                );
                return;
            }

            InitializeBrightData();

            bool is_BDConsentShownOneTime =
                PlayerPrefs.GetInt(BD_OneTime_Pref, 0) == 0 ? false : true;

            if (is_BDConsentShownOneTime)
            {
                ACLogger.UserDebug(
                    Imple + "request at onLoad is decline as it already shown once."
                );
                return;
            }

            if (IsAValidLevel && is_firstLaunch)
            {
#if !UNITY_EDITOR

                skipOneTime = true;
#endif

                showConsent(0);
            }
        }

        void OnLevelComplete(int levelID)
        {

            return;

#if AC_BRIGHTDATA

            brightDataScreenType = BrightDataScreenType.bright_levelCompleteScreen;

            bool is_BDConsentShownOneTime =
                PlayerPrefs.GetInt(BD_OneTime_Pref, 0) == 0 ? false : true;

            if (is_BDConsentShownOneTime)
            {
                ACLogger.UserDebug(Imple + "request Decline as it already shown once.");
            }
            else
            {
                ACLogger.UserDebug(Imple + "requested OnLevelComplete");
                showConsent(levelID);
            }

#else
            ACLogger.UserDebug(
                Imple + "ShowConcentOnLevelComplete is dicline as AC_BRIGHTDATA SDS is missing."
            );

#endif
        }

        void showConsent(int levelID)
        {
            if (!IsEnableFromServer || !IsEnabledFromACSettings)
            {
                ACLogger.UserDebug(
                    Imple + " NOT initializing becasue IsEnableFromServer: " + IsEnableFromServer
                );
                ACLogger.UserDebug(
                    Imple
                        + "NOT initializing becasue IsEnabledFromACSettings: "
                        + IsEnabledFromACSettings
                );
                return;
            }

            CurrentlevelID = levelID;

            IsAValidLevel = IsValideLevel(CurrentlevelID);

            ACLogger.UserDebug(Imple + "IsEnableFromServer: " + IsEnableFromServer);
            ACLogger.UserDebug(Imple + "IsAValidLevel: " + IsAValidLevel);

            if (!isInitialized)
            {
                InitializeBrightData();
                ACLogger.UserDebug(Imple + "Is Not Initialized");
                //return;
            }

            ACLogger.UserDebug(Imple + "IsEnableFromServer:" + IsEnableFromServer);
            ACLogger.UserDebug(Imple + "IsAValidLevel:" + IsAValidLevel);
            ACLogger.UserDebug(Imple + "GetConcentStatus:" + GetConcentStatus());

            if (IsEnableFromServer && IsAValidLevel && !GetConcentStatus())
            {
                ACLogger.UserDebug(Imple + "Requesting ShowConcent");
                ACLogger.UserDebug(Imple + "Requesting is_firstLaunch:" + is_firstLaunch);
                ACLogger.UserDebug(Imple + "Requesting IspreviouslyAllowed:" + IspreviouslyAllowed);

                if (is_firstLaunch && IspreviouslyAllowed)
                {
                    ACLogger.UserDebug(Imple + "Requesting ShowConcent_A");
                    ShowConcentCustom();
                }
                else
                {
                    ACLogger.UserDebug(Imple + "Requesting ShowConcent_B");
                    opt_in_mode = "bright_data";
                    ShowBDNativeConcent();
                }
            }
        }

        void InitializeBrightData()
        {
#if AC_BRIGHTDATA

            /// Initialize the SDK
            string language = null;
            bool skip_consent = true;
            BrdsdkBridge.init(
                "To support this app",
                "I Agree",
                "I disagree",
                "From the settings menu.",
                choiceChanged,
                skip_consent,
                language
            );
            isInitialized = true;
            ACLogger.UserDebug(Imple + "SDK Status: " + BrdsdkBridge.get_choice());

#endif
        }

        /// <summary>
        /// The method is called when user's choice is changed.
        /// </summary>
        /// <param name="choice">Value representing the user's choice:
        /// - BrdsdkBridge.CHOICE_NONE - the consent screen is not yet shown;
        /// - BrdsdkBridge.CHOICE_AGREED - user accepted the consent screen;
        /// - BrdsdkBridge.CHOICE_DISAGREED - user declined the consent screen.
        /// </param>
        void choiceChanged(int choice)
        {
            bool requested_silentoptIn = false;

#if AC_BRIGHTDATA

            ACLogger.UserDebug(Imple + "choiceChanged to:" + choice);

            if (skipOneTime || choice == 0)
            {
                ACLogger.UserDebug(Imple + "choiceChanged SDK StatusskipOneTime");
                skipOneTime = false;
                return;
            }

            ACLogger.UserDebug(
                Imple + "choiceChanged SDK Status orgnal: " + BrdsdkBridge.get_choice()
            );

            BrightDataUserConsentChoice _choice =
                choice == 1
                    ? BrightDataUserConsentChoice.accepted
                    : BrightDataUserConsentChoice.declined;

            ACLogger.UserDebug(
                Imple + "choiceChanged SDK Status is_showingConsent: " + is_showingConsent
            );

            AppCentral.OnBrightSDKConsentStatusChange?.Invoke(_choice);

            if (is_silent_opt_in_onetime)
            {
                ACLogger.UserDebug(Imple + "choiceChanged SDK Status is_silent_opt_in_onetime");
                is_silent_opt_in_onetime = false;
            }
            else
            {
                opt_in_mode = "bright_data";
                SendBrightDataConsentPixel(
                    _choice,
                    brightDataScreenType,
                    CurrentlevelID,
                    opt_in_mode
                );
            }

            AppCentral.OnBrightSDKConsentClose?.Invoke();
            ACLogger.UserDebug(Imple + "_onCloseCallback");
            _onCloseCallback?.Invoke();
#endif

            PlayerPrefs.SetInt(BD_OneTime_Pref, 1);
        }

        void choiceChanged_Custom(int choice)
        {
            bool requested_silentoptIn = false;

#if AC_BRIGHTDATA


            ACLogger.UserDebug(
                Imple + "choiceChanged SDK Status orgnal: " + BrdsdkBridge.get_choice()
            );

            BrightDataUserConsentChoice _choice =
                choice == 1
                    ? BrightDataUserConsentChoice.accepted
                    : BrightDataUserConsentChoice.declined;

            ACLogger.UserDebug(
                Imple + "choiceChanged SDK Status is_showingConsent: " + is_showingConsent
            );

            AppCentral.OnBrightSDKConsentStatusChange?.Invoke(_choice);

            ACLogger.UserDebug(Imple + "_onCloseCallback");
            _onCloseCallback?.Invoke();

            SendBrightDataConsentPixel(_choice, brightDataScreenType, CurrentlevelID, opt_in_mode);

            //if (GetConcentChoice() != BrightDataUserConsentChoice.accepted)
            {
                ACLogger.UserDebug(Imple + "choiceChanged B ");
                StartCoroutine(AppCentralBrightDataController.instance.WaitAndOptInSilently());
            }

            ////if (is_silent_opt_in_onetime)
            //{
            //}

            ////is_silent_opt_in_onetime = false;

#endif
            AppCentral.OnBrightSDKConsentClose?.Invoke();

            PlayerPrefs.SetInt(BD_OneTime_Pref, 1);
        }

        public IEnumerator WaitAndOptInSilently()
        {
#if AC_BRIGHTDATA


            is_firstLaunch = false;
            is_silent_opt_in = true;
            is_silent_opt_in_onetime = true;
            opt_in_mode = "previously_allowed";

            ACLogger.UserDebug(Imple + "WaitAndOptInSilently C");

            brightDataScreenType = BrightDataScreenType.custom_openScreen;

            bool isShown = BrdsdkBridge.consent_shown();

            yield return new WaitForSeconds(1f);

            ACLogger.UserDebug(Imple + "WaitAndOptInSilently isShown:" + isShown);

            yield return new WaitForSeconds(2f);

#if UNITY_EDITOR

            UI_BrightDataCustomScreen.Instance.ForceOptin((x) => { ACLogger.UserDebug(Imple + "BrightData ForceOptin: " + x); });
#else

            BrdsdkBridge.silent_opt_in();

#endif

#endif

            //yield return new WaitForSeconds(2);


            yield return null;
        }

        private void UserOptOut()
        {
            is_opt_out = true;

#if AC_BRIGHTDATA

#if UNITY_EDITOR

            UI_BrightDataCustomScreen.Instance.ForceOptOut(choiceChanged);

#else

            BrdsdkBridge.opt_out();
#endif
#endif
        }

        private void ShowBDNativeConcent(Action onCloseCallback = null)
        {
            ACLogger.UserDebug(Imple + "ShowConcent");

            _onCloseCallback = onCloseCallback;

#if AC_BRIGHTDATA

            is_showingConsent = true;
            AppCentral.OnBrightSDKConsentOpen?.Invoke();

#if UNITY_EDITOR


            if (IspreviouslyAllowed && is_firstLaunch)
            {
                UI_BrightDataCustomScreen.Instance.ShowBrightDataConsentDialog(choiceChanged_Custom);
            }
            else
            {
                UI_BrightDataCustomScreen.Instance.ShowBrightDataConsentDialog(choiceChanged);
            }

#else

            ACLogger.UserDebug(Imple + "ShowConcent is_firstLaunch:" + is_firstLaunch);
            ACLogger.UserDebug(Imple + "ShowConcent IspreviouslyAllowed:" + IspreviouslyAllowed);

            if (IspreviouslyAllowed && is_firstLaunch)
            {
                UI_BrightDataCustomScreen.Instance.ShowBrightDataConsentDialog(
                    choiceChanged_Custom
                );
            }
            else
            {
                BrdsdkBridge.show_consent();
            }

#endif

#else

            _onCloseCallback?.Invoke();

#endif

            is_firstLaunch = false;
            PlayerPrefs.SetInt(BD_FirstLaunch_Pref, 1);
        }

        public void ShowConcentFromSettings(Action onCloseCallback = null)
        {
#if AC_BRIGHTDATA

            is_firstLaunch = false;
            is_silent_opt_in = false;

            brightDataScreenType = BrightDataScreenType.bright_settingsScreen;
            ShowBDNativeConcent(onCloseCallback);
#else
            ACLogger.UserDebug(
                Imple + "ShowConcentFromSettings is dicline as AC_BRIGHTDATA SDS is missing."
            );

            onCloseCallback?.Invoke();
#endif
        }

        private void ShowConcentCustom(Action onCloseCallback = null)
        {
            is_silent_opt_in = true;

            opt_in_mode = "previously_allowed";

            brightDataScreenType = BrightDataScreenType.custom_openScreen;

            ShowBDNativeConcent(onCloseCallback);
        }

        public void ShowConcentAtLevelComplete(Action onCloseCallback = null)
        {
            is_firstLaunch = false;
            is_silent_opt_in = false;

            brightDataScreenType = BrightDataScreenType.bright_settingsScreen;
            ShowBDNativeConcent(onCloseCallback);
        }

        public void optOutFromSettings()
        {
#if AC_BRIGHTDATA

            brightDataScreenType = BrightDataScreenType.optOut_settingsScreen;
            UserOptOut();
#else
            ACLogger.UserDebug(
                Imple + "optOutFromSettings is dicline as AC_BRIGHTDATA SDS is missing."
            );

#endif
        }

        private void optOutCustom()
        {
            brightDataScreenType = BrightDataScreenType.custom_openScreen;
            UserOptOut();
        }

        public static BrightDataUserConsentChoice GetConcentChoice()
        {
            BrightDataUserConsentChoice choice = BrightDataUserConsentChoice.undefined;

#if AC_BRIGHTDATA

#if UNITY_EDITOR

            choice = UI_BrightDataCustomScreen.ConsentStatus == true ? choice = BrightDataUserConsentChoice.accepted : BrightDataUserConsentChoice.declined;

#else

            choice =
                BrdsdkBridge.get_choice() == 1
                    ? choice = BrightDataUserConsentChoice.accepted
                    : BrightDataUserConsentChoice.declined;

#endif
#endif

            return choice;
        }

        public static bool GetConcentStatus()
        {
#if AC_BRIGHTDATA

#if UNITY_EDITOR

            ACLogger.UserDebug(Imple + " UI_BrightDataCustomScreen.ConsentStatus:" + UI_BrightDataCustomScreen.ConsentStatus);
            return UI_BrightDataCustomScreen.ConsentStatus;

#else

            int choice = BrdsdkBridge.get_choice();
            if (choice == BrdsdkBridge.CHOICE_AGREED)
            {
                return true;
            }
            else
            {
                return false;
            }
#endif

#else
            ACLogger.UserDebug(
                Imple + "GetConcentStatus returned false as AC_BRIGHTDATA SDS is missing."
            );

            return false;
#endif
        }

        public static bool CanShowWebIndexingUIInSettingsMenu()
        {
            bool UIEnableStatus = false;

#if AC_BRIGHTDATA
            AppCentralCore.newSetting settings = AppCentralCore.AppCentralSettings.LoadSetting();

            bool IsEnableFromServer =
                PlayerPrefs.GetInt(AppCentralPrefsManager.brightdata_show_on_start) == 1
                    ? true
                    : false;
            bool IsEnableFromACSettings = settings.UseBrightDataSDK;

            if (IsEnableFromServer && IsEnableFromACSettings)
            {
                UIEnableStatus = true;
            }
#else

            ACLogger.UserDebug(
                Imple
                    + "CanShowWebIndexingUIInSettingsMenu returned false as AC_BRIGHTDATA SDS is missing."
            );

#endif

            return UIEnableStatus;
        }

        private bool IsValideLevel(int CurrentLevelIndex)
        {

            return true;

            bool canShow = false;

            string levelsList = PlayerPrefs.GetString(
                AppCentralPrefsManager.brightdata_show_in_levels
            );

            bool containComas = levelsList.Contains(',');

            ACLogger.UserDebug(Imple + "CurrentLevelIndex: " + CurrentLevelIndex);
            ACLogger.UserDebug(Imple + "levelsList: " + levelsList);
            ACLogger.UserDebug(Imple + "containComas: " + containComas);

            if (!string.IsNullOrEmpty(levelsList))
            {
                if (containComas)
                {
                    List<string> allLevels = levelsList.Split(',').ToList<string>();

                    if (allLevels.Contains(CurrentLevelIndex.ToString()))
                    {
                        canShow = true;
                    }
                }
                else
                {
                    if (CurrentLevelIndex == int.Parse(levelsList))
                    {
                        canShow = true;
                    }
                }
            }

            ACLogger.UserDebug(Imple + "canShow: " + canShow);
            return canShow;
        }

        private void SendBrightDataConsentPixel(BrightDataUserConsentChoice userChoice, BrightDataScreenType type, int levelID, string opt_in_mode)
        {
            string _type = type.ToString();
            string choice = userChoice.ToString(); // accepted/declined
            string choiceLocation = levelID.ToString(); // 0 = onStart, other integers refer to the level number
            int sessionNum = PlayerPrefs.GetInt(
                AppCentralPrefsManager.userData_app_session_number,
                1
            ); // the number of the current session
            string bd_sdk_orig_version = AppCentralSDKVersionTracker.BrightSDKVeriosn;
            string bd_sdk_imple_version = AppCentralSDKVersionTracker.BrightSDKImplemetationVersion;


            if (levelID == 0)
            {
                choiceLocation = "onStart";
            }

            string[] attributes = new string[]
{
                "type",
                "action",
                "choice_location",
                "session_number",
                "opt_in_mode",
                "bd_sdk_orig_version",
                "bd_sdk_imple_version"
};

            string[] values = new string[]
            {
                _type,
                choice,
                choiceLocation,
                sessionNum.ToString(),
                opt_in_mode,
                bd_sdk_orig_version,
                bd_sdk_imple_version
            };


            Debug.Log(
                "Saving AppCentralPixel. Parameters: "
                + "Pixel Name: permission_pixel, "
                + $"Field Names: [type:{_type}, action:{choice}, choice_location:{choiceLocation}, session_number:{sessionNum}, opt_in_mode:{opt_in_mode}, bd_sdk_orig_version:{bd_sdk_orig_version}, bd_sdk_imple_version:{bd_sdk_imple_version}]");

            AppCentralPixelController.Instance.SaveAppCentralPixel("permission_pixel", attributes, values);

        }
    }
}
