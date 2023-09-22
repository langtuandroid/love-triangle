using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.adjust.sdk;
using GH;

public class AC_AdjustRevenueTracker : MonoBehaviour
{
#if AC_ADJUST


    private void OnEnable()
    {
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnMaxAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnMaxAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnMaxAdRevenuePaidEvent;
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnMaxAdRevenuePaidEvent;
    }

    private void OnDisable()
    {
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnMaxAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnMaxAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnMaxAdRevenuePaidEvent;
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent -= OnMaxAdRevenuePaidEvent;
    }


    // pass MAX SDK ad revenue data to Adjust SDK
    private static void OnMaxAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
	{
		var info = MaxSdk.GetAdInfo(adUnitId);

		var adRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
		adRevenue.setRevenue(info.Revenue, "USD");
		adRevenue.setAdRevenueNetwork(info.NetworkName);
		adRevenue.setAdRevenueUnit(info.AdUnitIdentifier);
		adRevenue.setAdRevenuePlacement(info.Placement);

		Adjust.trackAdRevenue(adRevenue);
	}

#endif
}
