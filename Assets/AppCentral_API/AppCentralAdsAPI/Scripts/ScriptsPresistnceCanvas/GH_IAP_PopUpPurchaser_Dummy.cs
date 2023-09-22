using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Purchasing;
using System.Collections;

namespace PresistanceCanvas
{
    public class GH_IAP_PopUpPurchaser_Dummy : MonoBehaviour
    {
        public static GH_IAP_PopUpPurchaser_Dummy Instance;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }

        public Button OkBtn,
            CancleBtn;

        public GameObject WarningpanelWithOkbtn;
        public Text WarningMessageTxt;

        Action onSuccessCAllback;
        Action onFailCallback;

        PurchaseEventArgs _purchaseEventArgs;

        private void Start()
        {
            WarningpanelWithOkbtn.SetActive(false);

            OkBtn.onClick.AddListener(Ok_button);
            CancleBtn.onClick.AddListener(Cancle_button);
        }

        bool AlreadyShowing = false;

        public void ShowPooUpPurchaser(string Msg)
        {

            if(AlreadyShowing) return;

            ACLogger.UserDebug("Would you like to subscribe to this IAP");
            AlreadyShowing = true;
            WarningMessageTxt.text = Msg;
            StartCoroutine(DelayEnable());


            // _purchaseEventArgs = purchaseEventArgs;
            // onFailCallback = OnFailCallback;
            // onSuccessCAllback = OnSuccessCAllback;
        }


        IEnumerator DelayEnable()
        {

            yield return new WaitForSeconds(0.5f);
            WarningpanelWithOkbtn.SetActive(true);

        }


        private void Ok_button()
        {
            AlreadyShowing = false;

            AppCentralInAppPurchaser.Instance.OnPurchaseSuccess(_purchaseEventArgs);
            WarningpanelWithOkbtn.SetActive(false);
        }

        private void Cancle_button()
        {
            AlreadyShowing = false;

            AppCentralInAppPurchaser.Instance.OnPurchaseCancleOrFail(_purchaseEventArgs);
            WarningpanelWithOkbtn.SetActive(false);
        }


        //IEnumerator WaitAndCOmplete()
        //{
            
        //}
    }
}
