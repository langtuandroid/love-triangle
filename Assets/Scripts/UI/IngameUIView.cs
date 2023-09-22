using System;
using LoveTriangle.UI;
using UnityEngine;
using System.Collections;

public class IngameUIView : MVCUIView {
    public static Action onDoneMoneyAnimation;

    #region interface for listener
    public interface IListener {
        void OnClickRestart();
        void OnClickExit();
    }
    #endregion
    #region MVC Properties and functions to override
    /*
     * this will be the reference to the model 
     * */
    public IngameUIModel UIModel {
        get {
            return _baseAssetModel as IngameUIModel;
        }
    }

    /*
     * Call this Create method to Initialize and instantiate the UI.
     * There's a callback on the controller if you want custom initialization
     * */
    public static void Create(Canvas p_canvas, IngameUIModel p_assets, Action<IngameUIView> p_onCreate) {
        var go = new GameObject(typeof(IngameUIView).ToString());
        var gui = go.AddComponent<IngameUIView>();
        var assetsInstance = Instantiate(p_assets);
        gui.Init(p_canvas, assetsInstance);
        if (p_onCreate != null) {
            p_onCreate.Invoke(gui);
        }
    }
    #endregion

    public void HideAllTutorials() {
        UIModel.goBG.gameObject.SetActive(false);
        UIModel.newRoomUnlocked.gameObject.SetActive(false);
        UIModel.goBank.SetActive(false);
    }
    public void HideProgressBars() {
        UIModel.fillWife.transform.parent.parent.parent.gameObject.SetActive(false);
    }
    public void AddCashWithFlyingMoney(int p_newMoney, int p_newMoneyInBank, Vector3 p_payerPos, Camera p_cam) {
        UIModel.poppingCash.gameObject.SetActive(true);
        Vector3 pos = p_cam.WorldToViewportPoint(p_payerPos);
        //pos.x -= 0.5f;
        //pos.y -= 0.5f;
        UIModel.poppingCash.transform.position = pos;
        //UIModel.poppingCash.transform.position = GameObject.Find("UI_Camera").GetComponent<Camera>().ViewportToScreenPoint(UIModel.poppingCash.transform.position);
        LeanTween.move(UIModel.poppingCash.gameObject, UIModel.txtMoney.transform.position, 0.35f).setOnComplete(() => {
            ShowMoneyInBank();
            UpdateMoney(p_newMoney);
            UpdateMoneyInBank(p_newMoneyInBank);
            UIModel.poppingCash.gameObject.SetActive(false);
            onDoneMoneyAnimation?.Invoke();
            SFXPlayer.Instance.PlaySFX(SFXPlayer.SFX_TYPE.Cash_Register);
        });
    }

    public void AddCashWithFlyingMoneyReverse(int p_newMoney, int p_newMoneyInBank, Vector3 p_payerPos, Camera p_cam) {
        UIModel.poppingCash.gameObject.SetActive(true);
        Vector3 pos = p_cam.WorldToViewportPoint(p_payerPos);
        //pos.x -= 0.5f;
        //pos.y -= 0.5f;
        UIModel.poppingCash.transform.position = UIModel.txtMoney.transform.position;
        //UIModel.poppingCash.transform.position = GameObject.Find("UI_Camera").GetComponent<Camera>().ViewportToScreenPoint(UIModel.poppingCash.transform.position);
        LeanTween.move(UIModel.poppingCash.gameObject, pos, 0.15f).setOnComplete(() => {
            ShowMoneyInBank();
            UpdateMoney(p_newMoney);
            UpdateMoneyInBank(p_newMoneyInBank);
            UIModel.poppingCash.gameObject.SetActive(false);
            onDoneMoneyAnimation?.Invoke();
            SFXPlayer.Instance.PlaySFX(SFXPlayer.SFX_TYPE.Cash_Register);
        });
    }

    public void UpdateMoneyInBank(int p_money) {
        UIModel.txtMoneyInBank.text = LGGUtility.FormatMoney(p_money);
    }
    public void ShowMoneyInBank() {
        UIModel.goMoneyInTheBank.SetActive(true);
    }
    public void HideMoneyInBank() {
        UIModel.goMoneyInTheBank.SetActive(false);
    }
    public void ShowExitButton() {
        LeanTween.moveX(UIModel.btnExit.image.rectTransform, -114f, 0.5f);
        //UIModel.btnExit.gameObject.SetActive(true);
    }

    public void HideLevelText() {
        UIModel.txtLevel.gameObject.SetActive(false);
    }

    public void SetWifeProgressBar(int p_status, float p_val) {
        //Debug.LogError("WIFE: " + p_status);
        UIModel.fillWife.fillAmount = p_val;
        if (p_val >= .5f) {
            UIModel.fillWife.color = Color.green;
        }
        else {
            UIModel.fillWife.color = Color.red;
        }
        for (int x = 0; x < UIModel.heartsWife.Count; ++x) {
            UIModel.heartsWife[x].gameObject.SetActive(false);
        }
        UIModel.heartsWife[p_status].gameObject.SetActive(true);
    }

    public void SetMistressProgressBar(int p_status, float p_val, Color p_col) {
        UIModel.fillMistress.fillAmount = p_val;
        if (p_val >= .5f) {
            UIModel.fillMistress.color = Color.green;
        }
        else {
            UIModel.fillMistress.color = Color.red;
        }
        for (int x = 0; x < UIModel.heartsMistress.Count; ++x) {
            UIModel.heartsMistress[x].gameObject.SetActive(false);
        }
        UIModel.heartsMistress[p_status].gameObject.SetActive(true);
    }

    public void SetPartnerProgressBar(LadiesProgressBar.LadyType p_ladyType, int p_status, float p_val, Color p_col) {
        bool isMistress = p_ladyType == LadiesProgressBar.LadyType.MISTRESS;
        var fill = isMistress ? UIModel.fillMistress : UIModel.fillWife;
        var hearts = isMistress ? UIModel.heartsMistress : UIModel.heartsWife;
        fill.fillAmount = p_val;
        fill.color = p_col;
        for (int x = 0; x < UIModel.heartsMistress.Count; x++) {
            hearts[x].gameObject.SetActive(false);
        }
        hearts[p_status].gameObject.SetActive(true);
    }

    public void UpdateMoney(int p_money){
        UIModel.txtMoney.text = LGGUtility.FormatMoney(p_money);
    }
    public void UpdateLevelDisplay(string p_level) {
        UIModel.txtLevel.text = "Level " + p_level;
    }

    public void ShowUnlockRoomText() {
        UIModel.goBG.gameObject.SetActive(true);
        UIModel.newRoomUnlocked.gameObject.SetActive(true);
    }

    public void ShowBankDepositText(string p_text) {
        UIModel.goBG.gameObject.SetActive(true);
        UIModel.goBank.gameObject.SetActive(true);
        UIModel.goBank.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = p_text;
    }

    public void ShowMoneyMultiplier(string p_string) {
        UIModel.txtMoneyMultiplier.text = p_string;
        LeanTween.scale(UIModel.txtMoneyMultiplier.rectTransform, UIModel.txtMoneyMultiplier.transform.localScale + Vector3.one, 0.5f).setLoopPingPong(1);
        LeanTween.scale(UIModel.txtMoneyMultiplier.rectTransform, Vector3.zero, 0.5f).setDelay(1f).setOnComplete(() => UpdateMoney(PlayerData.CurrentMoney));
    }

    #region Subscribe/Unsubscribe for IListener
    public void Subscribe(IListener p_listener) {
        UIModel.onClickRestart += p_listener.OnClickRestart;
        UIModel.onClickExit += p_listener.OnClickExit;
    }
    public void Unsubscribe(IListener p_listener) {
        UIModel.onClickRestart -= p_listener.OnClickRestart;
        UIModel.onClickExit -= p_listener.OnClickExit;
    }
    #endregion
}

public static class LGGUtility {
    public static string FormatMoney(int p_money) {
        string ret = string.Empty;
        ret = "$" + p_money;
        if (p_money >= 1000) {
            int div = (p_money / 1000);
            ret = "$" + (div).ToString("D");
            if (((p_money - 1000) / 100) > 0) {
                ret += "." + ((p_money - (1000 * div)) / 100).ToString("D");
            }
            ret += "k";
        }
        else {
            ret = "$" + (p_money);
        }
        return ret;
    }

    public static string FormatAdsLevelName(int p_level) {
        if (p_level < 10) {
            return "Level000" + p_level;
        } else if (p_level < 100) {
            return "Level00" + p_level;
        } else if (p_level < 1000) {
            return "Level0" + p_level;
        } else if (p_level < 10000) {
            return "Level" + p_level;
        }
        return string.Empty;
    }
}