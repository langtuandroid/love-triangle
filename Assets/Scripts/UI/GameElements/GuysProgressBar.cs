using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LoveTriangle;
public class GuysProgressBar : MonoBehaviour {

    public TextMeshProUGUI txtMoneyCollected;
    private Color m_initColor;
    void OnEnable() {
        PowerUp.onPickedUp += UpdateProgress;
        MergingHearts.onGoToIdleArcade += OnDone;
    }
    void OnDisable(){
        PowerUp.onPickedUp -= UpdateProgress;
        MergingHearts.onGoToIdleArcade -= OnDone;
    }

	private void Start() {
        m_initColor = txtMoneyCollected.color;
        txtMoneyCollected.text = "$" + PlayerData.CurrentLevelMoney.ToString();
    }

	void OnDone() {
        txtMoneyCollected.text = string.Empty;
        txtMoneyCollected.gameObject.SetActive(false);
    }

    public void UpdateProgress(PowerUp p_up, float p_val){
        if(p_up is MoneyPowerUp){
            txtMoneyCollected.text = "$" + PlayerData.CurrentLevelMoney.ToString();
        }
    }
}