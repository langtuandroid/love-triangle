using UnityEngine;
using UnityEngine.UI;
using AppCentralAPI;
using AppCentralCore;
using UnityEngine.Purchasing;
using System;
using System.Collections;

public class AC_PlayOnAdsController : MonoBehaviour
{
#if AC_PLAYON

    public static AC_PlayOnAdsController instance;

    private void Awake()
    {
        instance = this;
    }



    public PlayOnAdUnitSetting playOnAdUnitSetting;

    private PlayOnSDK.Position popupLocation = (PlayOnSDK.Position)8;
    //private PlayOnSDK.Position adLocation = (PlayOnSDK.Position) 5;
    //private PlayOnSDK.AdUnitType adType = PlayOnSDK.AdUnitType.AudioBannerAd;
    //private PlayOnSDK.AdUnitActionButtonType bBehavior = PlayOnSDK.AdUnitActionButtonType.Mute;

    private AdUnit adUnit;

    private Color visualizationMainColor = Color.white;
    private Color visualizationBackgroundColor = Color.green;
    private Color adUnitProgressBarColor = Color.white;
    private float actionBtnDelayShowTime = 5f;
    private int logoAdSize = 100; //In pixels
    private int logoAdXOffset = 50; //In pixels
    private int logoAdYOffset = 50; //In pixels
    private int popupXOffset = 15; //In pixels
    private int popupYOffset = 15; //In pixels

    private PlayOnSDK.AdUnitRewardType rewardType = PlayOnSDK.AdUnitRewardType.InLevel;




    private static string sdk = "Audio Ad";

    public void Start()
    {
        StartCoroutine(ContiniousCheckForHiddingAds());
    }

    public void OnEnable()
    {
        if (!IsAudioAdsEnableFromServer()) return;

        AppCentralUnityApi_Internal.IAPInitializationCompleted += AC_InitializationComplete;

        GH.UI_PayWall.PaywallOpen += OnPaywallOpen;
        GH.UI_PayWall.PaywallClose += OnPaywallClose;
        GH.UI_PayWall.PaywallUserSubscribed += OnPaywallUserSubscribed;

#if AC_ROBLOX

        ACRBCore.RobloxEvensManager.OnRobloxMenuOpen += OnPaywallOpen_RB;
        ACRBCore.RobloxEvensManager.OnRobloxMenuClose += OnPaywallClose_RB;

#endif


    }

    public void OnDisable()
    {
        if (!IsAudioAdsEnableFromServer()) return;

        AppCentralUnityApi_Internal.IAPInitializationCompleted -= AC_InitializationComplete;

        GH.UI_PayWall.PaywallOpen -= OnPaywallOpen;
        GH.UI_PayWall.PaywallClose -= OnPaywallClose;
        GH.UI_PayWall.PaywallUserSubscribed -= OnPaywallUserSubscribed;

#if AC_ROBLOX

        ACRBCore.RobloxEvensManager.OnRobloxMenuOpen -= OnPaywallOpen_RB;
        ACRBCore.RobloxEvensManager.OnRobloxMenuClose -= OnPaywallClose_RB;

#endif

    }


    private void AC_InitializationComplete()
    {
        InitializeAudioAd();
    }




    private bool wasAdShowingBeforePaywallOpen = false;
    private bool PayWallIsStillOpen = false;
    private bool RBIsStillOpen = false;

    private void OnPaywallOpen(PAYWALL_TYPE pAYWALL_TYPE)
    {
        if (pAYWALL_TYPE == PAYWALL_TYPE.popup)
            return;

        if (adUnit != null)
        {
            wasAdShowingBeforePaywallOpen = true;
        }
        PayWallIsStillOpen = true;
        ForceCloseAd();
    }

    private void OnPaywallOpen_RB()
    {
        if (adUnit != null)
        {
            wasAdShowingBeforePaywallOpen = true;
        }
        Debug.LogError("OnPaywallOpen_RB");
        RBIsStillOpen = true;
        ForceCloseAd();
    }

    private void OnPaywallClose(PAYWALL_TYPE pAYWALL_TYPE)
    {
        if (pAYWALL_TYPE == PAYWALL_TYPE.popup)
            return;

        if (wasAdShowingBeforePaywallOpen)
        {
            if (playOnAdUnitSetting != null)
                CreateLogoAd(playOnAdUnitSetting, ShowAd);
        }
        PayWallIsStillOpen = false;
        wasAdShowingBeforePaywallOpen = false;
    }


    private void OnPaywallClose_RB()
    {

        if (wasAdShowingBeforePaywallOpen)
        {
            if (playOnAdUnitSetting != null)
                CreateLogoAd(playOnAdUnitSetting, ShowAd);
        }
        Debug.LogError("OnPaywallClose_RB");
        RBIsStillOpen = false;
        wasAdShowingBeforePaywallOpen = false;
    }


    IEnumerator ContiniousCheckForHiddingAds()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            if (RBIsStillOpen || PayWallIsStillOpen)
            {
                ACLogger.UserDebug($": {sdk} Force Close Audio Ad");
                ForceCloseAd();
            }
        }
    }

    private void OnPaywallUserSubscribed(PAYWALL_TYPE pAYWALL_TYPE, PurchaseEventArgs args)
    {
        ForceCloseAd();
    }



    private void InitializeAudioAd()
    {
        if (!IsAudioAdsEnableFromServer() || AppCentralAPI.AppCentral.IsSuscriptionActive()) return;

        ACLogger.UserDebug($"Initialize {sdk} called");

        PlayOnConfig playOnConfig = AppCentralSettings.LoadSetting().playOnConfig;

        if (!playOnConfig.UsePlayOnSDK)
        {
            ACLogger.UserDebug($": {sdk} is Disabled from AppCentral Settings");
            return;
        }



        string API_KEY = playOnConfig.ApiKey;
        string STORE_ID = playOnConfig.storeID;

        API_KEY = GetAppId();


        ACLogger.UserDebug($": {sdk} is enabled from AppCentral Settings with API_KEY=" + API_KEY);
        ACLogger.UserDebug($": {sdk} is enabled from AppCentral Settings with STORE_ID=" + STORE_ID);

        if (string.IsNullOrEmpty(API_KEY)) return;

        PlayOnSDK.OnInitializationFinished += OnInitializationFinished;
        PlayOnSDK.Initialize(API_KEY, STORE_ID);
        PlayOnSDK.SetLogLevel(PlayOnSDK.LogLevel.Debug);

    }

    Action onAdLoadedCallback;
    public void CreateLogoAd(PlayOnAdUnitSetting _playOnAdUnitSetting, Action _onAdLoadedCallback = null)
    {


        if (adUnit != null)
        {
            adUnit.CloseAd();
            adUnit.Dispose();
            adUnit = null;
        }


        if (_playOnAdUnitSetting == null)
        {
            ACLogger.UserError($": {sdk} _playOnAdUnitSetting=null");

            return;
        }

        if (_playOnAdUnitSetting._adUnitAnchor == null)
        {
            ACLogger.UserError($": {sdk} _playOnAdUnitSetting._adUnitAnchor == null");

            return;
        }


        playOnAdUnitSetting = _playOnAdUnitSetting;

        adUnit = new AdUnit(playOnAdUnitSetting._adUnitType);
        onAdLoadedCallback = _onAdLoadedCallback;


        //if AudioLogoAd then that would be position of your logo
        //Three methods are used to set the position: LinkLogoToPrefab, SetLogo, LinkLogoToRectTransform
        //LinkLogoToPrefab - Set the AD Unit to the same position and size as the AdUnitAnchor object. Make sure the AdUnitAnchor object is a child of your canvas
        adUnit.LinkLogoToPrefab(playOnAdUnitSetting._adUnitAnchor);
        //SetLogo - Set the AD Unit position relative to the screen with offsets. Offsets and size must be specified in density pixels
        //adUnit.SetLogo(adLocation, logoAdXOffset, logoAdYOffset, logoAdSize);
        //LinkLogoToRectTransform - Set theAD Unit position in the RectTransform. PlayOnSDK.Position specifies the location inside RectTransform. Size must be specified in density pixels
        //adUnit.LinkLogoToRectTransform(PlayOnSDK.Position.BottomLeft, rect, canvas);

        adUnit.SetVisualization(visualizationMainColor, visualizationBackgroundColor);
        //if rewarded ad type, turn on, turn off popup
        adUnit.SetPopup(popupLocation, popupXOffset, popupYOffset);
        adUnit.SetReward(rewardType, 100.0f);
        adUnit.SetProgressBar(adUnitProgressBarColor);
        adUnit.SetActionButton(playOnAdUnitSetting._adUnitActionButtonType, actionBtnDelayShowTime);
        //if banner ad type
        adUnit.SetBanner(playOnAdUnitSetting.position);

        //Adding callbacks
        adUnit.AdCallbacks.OnAvailabilityChanged += AdOnAvailabilityChanged;
        adUnit.AdCallbacks.OnShow += AdOnShow;
        adUnit.AdCallbacks.OnClose += AdOnClose;
        adUnit.AdCallbacks.OnClick += AdOnClick;

        //If rewarded ad type, rewarded callback
        adUnit.AdCallbacks.OnReward += AdOnReward;

        //If Impression turned on
        adUnit.AdCallbacks.OnImpression += AdOnImpression;



    }

    public void CreateBannerAd(PlayOnAdUnitSetting _playOnAdUnitSetting, Action _onAdLoadedCallback = null)
    {
        if (adUnit != null)
        {
            adUnit.CloseAd();
            adUnit.Dispose();
            adUnit = null;
        }

        onAdLoadedCallback = _onAdLoadedCallback;
        playOnAdUnitSetting = _playOnAdUnitSetting;

        adUnit = new AdUnit(playOnAdUnitSetting._adUnitType);


        //if AudioLogoAd then that would be position of your logo
        //Three methods are used to set the position: LinkLogoToPrefab, SetLogo, LinkLogoToRectTransform
        //LinkLogoToPrefab - Set the AD Unit to the same position and size as the AdUnitAnchor object. Make sure the AdUnitAnchor object is a child of your canvas
        //adUnit.LinkLogoToPrefab(adUnitAnchor);
        //SetLogo - Set the AD Unit position relative to the screen with offsets. Offsets and size must be specified in density pixels
        //adUnit.SetLogo(adLocation, logoAdXOffset, logoAdYOffset, logoAdSize);
        //LinkLogoToRectTransform - Set theAD Unit position in the RectTransform. PlayOnSDK.Position specifies the location inside RectTransform. Size must be specified in density pixels
        //adUnit.LinkLogoToRectTransform(adLocation, rect, canvas);

        adUnit.SetVisualization(visualizationMainColor, visualizationBackgroundColor);
        //if rewarded ad type, turn on, turn off popup
        adUnit.SetPopup(popupLocation, popupXOffset, popupYOffset);
        adUnit.SetReward(rewardType, 100.0f);
        adUnit.SetProgressBar(adUnitProgressBarColor);
        adUnit.SetActionButton(playOnAdUnitSetting._adUnitActionButtonType, actionBtnDelayShowTime);
        //if banner ad type
        adUnit.SetBanner(playOnAdUnitSetting.position);

        //Adding callbacks
        adUnit.AdCallbacks.OnAvailabilityChanged += AdOnAvailabilityChanged;
        adUnit.AdCallbacks.OnShow += AdOnShow;
        adUnit.AdCallbacks.OnClose += AdOnClose;
        adUnit.AdCallbacks.OnClick += AdOnClick;

        //If rewarded ad type, rewarded callback
        adUnit.AdCallbacks.OnReward += AdOnReward;

        //If Impression turned on
        adUnit.AdCallbacks.OnImpression += AdOnImpression;



    }

    public void IsAdAvailable()
    {
        if (adUnit != null)
        {
            bool flag = adUnit.IsAdAvailable();
            if (flag)
            {
                ACLogger.UserDebug("Unity PlayOnDemo IsAdAvailable: True");
            }
            else
            {
                ACLogger.UserDebug("Unity PlayOnDemo IsAdAvailable: False");
            }
        }
    }

    public void ShowAd()
    {
        if (!IsAudioAdsEnableFromServer() || AppCentralAPI.AppCentral.IsSuscriptionActive()) return;

        forceClosed = false;

        if (adUnit != null)
        {

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            adUnit.ShowAd();
        }
    }

    private bool forceClosed = false;

    public void ForceCloseAd()
    {
        if (adUnit != null)
        {
            forceClosed = true;
            adUnit.CloseAd();
        }
    }


    IEnumerator AutoRefreshAd()
    {
        //while (true)
        {
            yield return new WaitForSeconds(1);
            if (!forceClosed)
            {
                AppCentral.Instance.ShowAudioAd(playOnAdUnitSetting._adUnitAnchor);
            }
        }
    }


    private void CloseAd()
    {
        if (adUnit != null)
        {
            adUnit.CloseAd();
        }
    }

    public void OnInitializationFinished()
    {
        ACLogger.UserDebug("Unity PlayOnDemo OnInitializationFinished Callback");
    }

    public void AdOnAvailabilityChanged(bool flag)
    {
        Debug.Log("Unity PlayOnDemo AdOnAvailabilityChanged Callback " + flag);
        if (flag)
        {
            ACLogger.UserDebug("Unity PlayOnDemo AdOnAvailabilityChanged: True");

            //if (PayWallIsStillOpen || RBIsStillOpen)
            //{
            //    CloseAd();
            //}
            //else
            {
                onAdLoadedCallback?.Invoke();
            }
        }
        else
        {
            ACLogger.UserDebug("Unity PlayOnDemo AdOnAvailabilityChanged: False");
        }

        onAdLoadedCallback = null;
    }

    public void AdOnClick()
    {
        ACLogger.UserDebug("Unity PlayOnDemo AdOnClick Callback");
    }


    Coroutine coroutine;

    public void AdOnClose()
    {
        ACLogger.UserDebug("Unity PlayOnDemo AdOnClose Callback");

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(AutoRefreshAd());

    }

    public void AdOnShow()
    {
        ACLogger.UserDebug("Unity PlayOnDemo AdOnShow Callback");
    }

    public void AdOnReward(float amount)
    {
        ACLogger.UserDebug("Unity PlayOnDemo AdOnReward Callback");
    }

    public void AdOnImpression(AdUnit.ImpressionData data)
    {
        ACLogger.UserDebug("Unity PlayOnDemo AdOnImpression Callback " + data.GetCountry());
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        PlayOnSDK.onApplicationPause(pauseStatus);
    }
    private bool IsAudioAdsEnableFromServer()
    {
        return PlayerPrefs.GetInt(AppCentralPrefsManager.display_audio_ads) == 1 ? true : false;
    }

    public AudioAdsConfig GetAudioAdsConfigFromServer()
    {
        int config = PlayerPrefs.GetInt(AppCentralPrefsManager.audio_ads_display_config);
        AudioAdsConfig audioAdsConfig = (AudioAdsConfig)config;
        return audioAdsConfig;
    }

    public string GetAppId()
    {
        return PlayerPrefs.GetString(AppCentralPrefsManager.odeeo_app_key);
    }

#endif

}

#if AC_PLAYON
[System.Serializable]
public class PlayOnAdUnitSetting
{
    public PlayOnSDK.AdUnitType _adUnitType;
    public PlayOnSDK.AdUnitActionButtonType _adUnitActionButtonType = PlayOnSDK.AdUnitActionButtonType.None;
    public PlayOnSDK.Position position = PlayOnSDK.Position.TopLeft;
    public AdUnitAnchor _adUnitAnchor = null;
    public Action _onAdLoadedCallback = null;

    public PlayOnAdUnitSetting(PlayOnSDK.AdUnitType adUnitType)
    {
        _adUnitType = adUnitType;
    }

    public PlayOnAdUnitSetting(PlayOnAdUnitSetting playOnAdUnitySetting)
    {
        _adUnitType = playOnAdUnitySetting._adUnitType;
        _adUnitActionButtonType = playOnAdUnitySetting._adUnitActionButtonType;
        _adUnitAnchor = playOnAdUnitySetting._adUnitAnchor;
        _onAdLoadedCallback = playOnAdUnitySetting._onAdLoadedCallback;
    }

    public PlayOnAdUnitSetting(PlayOnSDK.AdUnitType adUnitType, AdUnitAnchor adUnitAnchor = null, PlayOnSDK.AdUnitActionButtonType adUnitActionButtonType = PlayOnSDK.AdUnitActionButtonType.None, Action onAdLoadedCallback = null)
    {
        _adUnitType = adUnitType;
        _adUnitActionButtonType = adUnitActionButtonType;
        _adUnitAnchor = adUnitAnchor;
        _onAdLoadedCallback = onAdLoadedCallback;
    }



}

public enum AudioAdsConfig
{
    LogoAdNoOverlap = 0,
    LogoAdOverlap = 1,

    BannerAdNoOverlap = 2,
    BannerAdHalfOverlap = 3,
    BannerAdFullOverlap = 4,
}

#endif