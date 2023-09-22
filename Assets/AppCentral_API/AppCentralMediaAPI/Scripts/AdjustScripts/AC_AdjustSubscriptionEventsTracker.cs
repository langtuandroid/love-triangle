using System.Collections.Generic;
using System.Collections;
using UnityEngine.Purchasing;
using com.adjust.sdk;
using AppCentralAPI;
using UnityEngine;
using System;
using GH;

public class AC_AdjustSubscriptionEventsTracker : MonoBehaviour
{
#if AC_ADJUST


    private void OnEnable()
    {
        UI_PayWall.PaywallUserSubscribed += AdjustSendSubscriptionEvent;
    }

    private void OnDisable()
    {
        UI_PayWall.PaywallUserSubscribed -= AdjustSendSubscriptionEvent;
    }

    private static void AdjustSendSubscriptionEvent(PAYWALL_TYPE paywallType,PurchaseEventArgs args)
    {
        if (args == null) return;

        double price = (double)args.purchasedProduct.metadata.localizedPrice;
        string currency = args.purchasedProduct.metadata.isoCurrencyCode;
        string transactionId = args.purchasedProduct.transactionID;
        string receipt = args.purchasedProduct.receipt;

        string purchasedToken = "";

        SubscriptionManager subscriptionManager = new SubscriptionManager(args.purchasedProduct,null);
        SubscriptionInfo subscriptionInfo = subscriptionManager.getSubscriptionInfo();

        if (subscriptionInfo.isFreeTrial() == Result.True)
        {
            purchasedToken = AdjustManager.instance.freeTrialToken; // fnfgo9
        }
        else if (subscriptionInfo.isSubscribed() == Result.True)
        {
            purchasedToken = AdjustManager.instance.fullScbscriptionPurchaseToken; // obqmi4
        }

        Debug.Log("purchased_Token:" + purchasedToken);
        Debug.Log("purchased_price:" + price);
        Debug.Log("purchased_currency:" + currency);
        Debug.Log("purchased_transactionId:" + transactionId);
        Debug.Log("purchased_receipt:" + receipt);

        AdjustEvent adjustEvent = new AdjustEvent(purchasedToken);
        adjustEvent.setRevenue(price, currency);
        adjustEvent.setTransactionId(transactionId);
        adjustEvent.receipt = receipt;

        Adjust.trackEvent(adjustEvent);
    }

#endif

}
