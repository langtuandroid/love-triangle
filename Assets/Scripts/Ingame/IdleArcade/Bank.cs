using System.Collections;
using System;
using UnityEngine;
using TMPro;
using LoveTriangle;

public class Bank : MonoBehaviour {
	
    public TextMeshProUGUI txtMoneyDisplay;
	private void OnEnable() {
		BankTransferMode.onMoneyUpdate += OnUpdateMoney;
		PlayerData.onLoadDone += OnUpdateMoney;
	}

	private void OnDisable() {
		BankTransferMode.onMoneyUpdate -= OnUpdateMoney;
		PlayerData.onLoadDone -= OnUpdateMoney;
	}

	void OnUpdateMoney() {
        txtMoneyDisplay.text = "$" + PlayerData.BankMoney.ToString();
		PlayerData.Save();
	}
}
