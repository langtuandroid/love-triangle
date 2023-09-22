using AppCentralAPI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace AppCentralTesting
{
    public class AC_PlayOnAdsTestingHelper : MonoBehaviour
    {

#if AC_PLAYON

        private PlayOnSDK.AdUnitType adType = PlayOnSDK.AdUnitType.AudioBannerAd;
        private PlayOnSDK.AdUnitActionButtonType bBehavior = PlayOnSDK.AdUnitActionButtonType.Mute;
        private PlayOnSDK.Position adLocation = (PlayOnSDK.Position)5;
        private AudioAdsConfig audioAdsConfig = (AudioAdsConfig.LogoAdNoOverlap);


        private List<PlayOnSDK.AdUnitType> adUnitTypes = new List<PlayOnSDK.AdUnitType>
        {
            PlayOnSDK.AdUnitType.AudioLogoAd,
            PlayOnSDK.AdUnitType.AudioBannerAd 
        };

        private List<PlayOnSDK.AdUnitActionButtonType> adUnitActionButtonType = new List<PlayOnSDK.AdUnitActionButtonType>
        {
            PlayOnSDK.AdUnitActionButtonType.None,
            PlayOnSDK.AdUnitActionButtonType.Mute,PlayOnSDK.AdUnitActionButtonType.Close
        };

        private List<PlayOnSDK.Position> adPositionType = new List<PlayOnSDK.Position> 
        {
            PlayOnSDK.Position.TopLeft,
            PlayOnSDK.Position.TopCenter,
            PlayOnSDK.Position.TopRight,
            PlayOnSDK.Position.CenterLeft,
            PlayOnSDK.Position.Centered,
            PlayOnSDK.Position.CenterRight,
            PlayOnSDK.Position.BottomLeft,
            PlayOnSDK.Position.BottomCenter,
            PlayOnSDK.Position.BottomRight,

        };

        private List<AudioAdsConfig> audioAdsConfigs = new List<AudioAdsConfig>
        {
            AudioAdsConfig.LogoAdNoOverlap,
            AudioAdsConfig.LogoAdOverlap,

            AudioAdsConfig.BannerAdNoOverlap,
            AudioAdsConfig.BannerAdHalfOverlap,
            AudioAdsConfig.BannerAdFullOverlap,

        };

        private List<string> adUnitTypes_string = new List<string>();
        private List<string> adUnitActionButtonType_string = new List<string>();
        private List<string> adPositionType_string = new List<string>();
        private List<string> audioAdsConfigs_string = new List<string>();

        public AC_EnumPegging dropdown_SetAdType;
        public AC_EnumPegging dropdown_SetButtonBehavior;
        public AC_EnumPegging dropdown_SetAdViewPosition;
        public AC_EnumPegging dropdown_SetAudioAdsConfigs;

        public Button ShowLogoAudioAd, ShowBannerAudioAd, CloseAudioAd, ForceCloseAudioAd;

        public Button ShowBanner, HideBanner;

        public AdUnitAnchor adUnitAnchorNoOverlapping, adUnitAnchorOverlapping;



        private void Start()
        {


            foreach (var item in adUnitTypes)
            {
                adUnitTypes_string.Add(item.ToString());
            }
            foreach (var item in adUnitActionButtonType)
            {
                adUnitActionButtonType_string.Add(item.ToString());
            }               
            foreach (var item in adPositionType)
            {
                adPositionType_string.Add(item.ToString());
            }
            foreach (var item in audioAdsConfigs)
            {
                audioAdsConfigs_string.Add(item.ToString());
            }

            dropdown_SetAdType.Initialize(adUnitTypes_string, SetAdType);
            dropdown_SetButtonBehavior.Initialize(adUnitActionButtonType_string, SetButtonBehavior);
            dropdown_SetAdViewPosition.Initialize(adPositionType_string, SetAdViewPosition);
            dropdown_SetAudioAdsConfigs.Initialize(audioAdsConfigs_string, SetAudioAdsConfig);

            ShowLogoAudioAd.onClick.AddListener(OnShowLogoAudioAd);
            ShowBannerAudioAd.onClick.AddListener(OnShowBannerAudioAd);
            CloseAudioAd.onClick.AddListener(OnCloseAudioAd);
            ForceCloseAudioAd.onClick.AddListener(OnForceCloseAudioAd);

            ShowBanner.onClick.AddListener(OnShowbannerPressed);
            HideBanner.onClick.AddListener(OnHidebannerPressed);

            ShowLogoAudioAd.GetComponentInChildren<Text>().text = "ShowLogoAudioAd";
            ShowBannerAudioAd.GetComponentInChildren<Text>().text = "ShowBannerAudioAd";
            CloseAudioAd.GetComponentInChildren<Text>().text = "CloseAudioAd";
            ForceCloseAudioAd.GetComponentInChildren<Text>().text = "ForceCloseAudioAd";

            ShowBanner.GetComponentInChildren<Text>().text = "Show Banner Ad";
            HideBanner.GetComponentInChildren<Text>().text = "Hide Banner Ad";



        }

        private void SetAdType(string newType)
        {
            adType = ParseEnum<PlayOnSDK.AdUnitType>(newType);
        }
        private void SetButtonBehavior(string newType)
        {
            bBehavior = ParseEnum<PlayOnSDK.AdUnitActionButtonType>(newType);
        }

        private void SetAdViewPosition(string newType)
        {
            adLocation = ParseEnum<PlayOnSDK.Position>(newType);
        }

        private void SetAudioAdsConfig(string newType)
        {
            audioAdsConfig = ParseEnum<AudioAdsConfig>(newType);
            PlayerPrefs.SetInt(AppCentralCore.AppCentralPrefsManager.audio_ads_display_config, (int)audioAdsConfig);
        }

        private void OnShowLogoAudioAd()
        {
            if (audioAdsConfig == AudioAdsConfig.LogoAdNoOverlap)
            {
                AppCentral.Instance.ShowAudioAd(adUnitAnchorNoOverlapping);
            }
            else
            {
                AppCentral.Instance.ShowAudioAd(adUnitAnchorOverlapping);
                //AppCentral.Instance.ShowAudioLogoAd(PlayOnSDK.AdUnitType.AudioLogoAd, bBehavior, adUnitAnchorNoOverlapping);
            }
        }

        private void OnShowBannerAudioAd()
        {
            //AppCentral.Instance.ShowAudioAd_banner(adType, adLocation, bBehavior);
            AppCentral.Instance.ShowAudioAd();
        }

        private void OnCloseAudioAd()
        {
            AppCentral.Instance.CloseAudioAd();
        }
        private void OnForceCloseAudioAd()
        {
            AppCentral.Instance.ForceCloseAudioAd();
        }
        private void OnShowbannerPressed()
        {
            AppCentral.ShowBannerAd();
        }
        private void OnHidebannerPressed()
        {
            AppCentral.HideBannerAd();
        }


        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

#endif
    }
}
