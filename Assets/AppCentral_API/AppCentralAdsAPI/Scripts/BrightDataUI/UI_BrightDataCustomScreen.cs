using AppCentralCore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UI_BrightDataCustomScreen : MonoBehaviour
{

    private static UI_BrightDataCustomScreen instance;
    public static UI_BrightDataCustomScreen Instance
    {
        get => instance;
    }

    private void Awake()
    {
        instance = this;
    }

    private static bool consentStatus = false;
    public static bool ConsentStatus { get => consentStatus; }

    public Button Accept_btn, Decline_btn;

    public TextMeshProUGUI AppNametxt, ConsentTxt, AgreeBtnTxt, DisAgreeBtnTxt, FooterTxt;

    public Image iconImg;

    public ScrollRect scrollView;

    private bool isOpen = false;

    private string AppName, Consent, Agree, Disagree, Footer;

    private Action<int> _resultCallback;

    private CanvasGroup canvasGroup;


    private string brightDataUrl = "https://www.brightdata.com";
    private string prvivicyUrl = "https://brightdata.com/legal/sdk-privacy";
    private string endUserLicenseAgreement = "https://brightdata.com/legal/sdk-eula";



    public void Start()
    {
        AppName = AppCentralSettings.LoadSetting().GameName;
        iconImg.sprite = AppCentralSettings.LoadSetting().GameIcon;

        Consent = $"To support {AppName}," +
            $" allow  <link={brightDataUrl}><color=#4E7DF4>Bright Data</color></link> to use your IP adress and avaliable resources to download <color=#4E7DF4>public website data</color> from the internet." +
            $" This will not interrupt your usage of {AppName}; it happens in the background, while you use the app.\r\n\r\nBright Data is commited to complying with GDRP," +
            $" CCPA and other applicable privacy laws. None of your personal information is accesses or collected except your IP address.\r\n\r\nOnly Bright Data can use your IP address, " +
            $"and it will be use for <color=#4E7DF4>approved use cases</color>.\r\n\r\n<b>Your participation is totally optional, and you may opt-out anytime!</b>.\r\n";


        Agree = "Agree";
        Disagree = "Disagree";
        Footer = $"\"Read Bright Data's <link={prvivicyUrl}><color=#4E7DF4>Privacy Policy</color></link> and <link={endUserLicenseAgreement}><color=#4E7DF4>End User License Agreement</color></link>\"";


        ConsentTxt.text = Consent;
        AgreeBtnTxt.text = Agree;
        DisAgreeBtnTxt.text = Disagree;
        FooterTxt.text = Footer;
        AppNametxt.text = AppName;

        Accept_btn.onClick.AddListener(Accept_btn_clicked);
        Decline_btn.onClick.AddListener(Decline_btn_clicked);

        gameObject.SetActive(false);

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

    }



    public void ShowBrightDataConsentDialog(Action<int> resultCallback)
    {
#if !UNITY_EDITOR

        resultCallback(1);
        return;

#endif
        if (!isOpen)
        {

            _resultCallback = resultCallback;

            scrollView.verticalNormalizedPosition = 1;
            gameObject.SetActive(true);
            StartCoroutine(OpenAnim(GetComponent<RectTransform>(), new Vector3(0, -Screen.height, 0), new Vector3(0, -Screen.height, 0)));
            ACLogger.UserDebug("ShowBrightDataConsentDialog");
        }

    }

    public void ForceOptin(Action<int> resultCallback)
    {
        _resultCallback = resultCallback;
        Accept_btn_clicked();
    }

    public void ForceOptOut(Action<int> resultCallback)
    {
        _resultCallback = resultCallback;
        Decline_btn_clicked();
    }



    private void HideBrightDataConsentDialog()
    {
        _resultCallback = null;
        isOpen = false;
        gameObject.SetActive(false);
        //StartCoroutine(CloseAnim(GetComponent<RectTransform>(), new Vector3(0, -Screen.height, 0), new Vector3(0, -Screen.height, 0), () => gameObject.SetActive(false)));
    }

    private void Accept_btn_clicked()
    {
        consentStatus = true;
        _resultCallback?.Invoke(1);
        HideBrightDataConsentDialog();
    }

    private void Decline_btn_clicked()
    {
        consentStatus = false;
        _resultCallback?.Invoke(2);
        HideBrightDataConsentDialog();
    }

    private IEnumerator OpenAnim(RectTransform rectTransform, Vector3 startPos, Vector3 endPos)
    {
        isOpen = true;
        float t = 0;
        while (t <= 1)
        {
            t += Time.unscaledDeltaTime / 0.25f;
            rectTransform.anchoredPosition = Vector3.Lerp(new Vector3(0, -rectTransform.rect.height * rectTransform.pivot.y, 0), new Vector3(0, 0, 0), t);

            yield return null;
        }


    }

    private IEnumerator CloseAnim(RectTransform rectTransform, Vector3 startPos, Vector3 endPos, Action callback)
    {
        isOpen = true;
        float t = 0;

        startPos = rectTransform.anchoredPosition;

        while (t <= 1)
        {
            t += Time.deltaTime / 0.5f;
            rectTransform.anchoredPosition = Vector3.Lerp(startPos, new Vector3(0, -rectTransform.rect.height * rectTransform.pivot.y, 0), t);

            yield return null;
        }

        callback?.Invoke();

    }


}
