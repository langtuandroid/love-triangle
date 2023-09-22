using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AppCentralAPI
{
    public class UI_IAPProcessing : MonoBehaviour
    {
        [SerializeField]
        GameObject processingImage;
        [SerializeField]
        GameObject WaitingPanel;

        [SerializeField]
        float rotationSpeed = 100;

        bool isProcessing = false;

        Vector3 eularangle = Vector3.zero;

        /*
                public void OnEnable()
                {
                    AppCentralInAppPurchaser.PurchaserProcessingStart += StartProcessing;
                    AppCentralInAppPurchaser.PurchaserProcessingEnd += EndProcessing;
                }

                public void OnDisable()
                {
                    AppCentralInAppPurchaser.PurchaserProcessingStart -= StartProcessing;
                    AppCentralInAppPurchaser.PurchaserProcessingEnd -= EndProcessing;
                }


                private void StartProcessing()
                {
                    isProcessing = true;
                    WaitingPanel.SetActive(true);
                    Time.timeScale = 0;
                }

                private void EndProcessing()
                {
                    isProcessing = false;
                    WaitingPanel.SetActive(false);
                    Time.timeScale = 1;
                }


                public void Update()
                {
                    if (isProcessing)
                    {
                        eularangle = processingImage.transform.localEulerAngles;
                        eularangle.z += Time.unscaledDeltaTime * rotationSpeed;
                        processingImage.transform.localEulerAngles = eularangle;
                    }
                }*/
    }
}
