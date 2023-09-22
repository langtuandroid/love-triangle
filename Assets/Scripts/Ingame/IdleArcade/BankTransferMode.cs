using System.Collections;
using System;
using UnityEngine;
using TMPro;
using LoveTriangle;

public class BankTransferMode : MonoBehaviour {

	public static Action onMoneyUpdate;

	public enum TransferMode { Withdraw = 0, Deposit }
	public TransferMode transferMode;

	public TextMeshProUGUI txtAction;
	private IngameUIController m_ingameUIController;

	private void Awake() {
		m_ingameUIController = GameObject.FindObjectOfType<IngameUIController>();
	}
	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<CarCollider>() != null) {
			switch (transferMode) {
				case TransferMode.Withdraw:
				StartCoroutine(DoWithdraw());
				break;
				case TransferMode.Deposit:
				StartCoroutine(DoDeposit());
				break;
			}
			txtAction.color = Color.green;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<CarCollider>() != null) {
			StopAllCoroutines();
			txtAction.color = Color.white;
		}
	}

	IEnumerator DoWithdraw() {
		float speed = 0.05f;
		float incrementor = 1;
		int maxClampAmount = PlayerData.BankMoney + PlayerData.CurrentMoney; 
		float timer = 0f;
		VFXDisplayer.DisplayMovingVFXUIToWOrld(LoveTriangle.ObjectPoolLibraryCommon.PoolType.FLOATING_MONEY, m_ingameUIController.moneyUI.position, transform.position);
		
		while (PlayerData.BankMoney > 0) {
			PlayerData.IncreaseMoney((int)incrementor, true, maxClampAmount);
			PlayerData.DecreaseBankMoney((int)incrementor, true);
			onMoneyUpdate?.Invoke();
			yield return new WaitForSeconds(speed);
			speed -= 0.05f;
			speed = Mathf.Clamp(speed, 0f, 10000000f);
			incrementor += 0.25f;
			SFXPlayer.Instance.PlaySFXWait(SFXPlayer.SFX_TYPE.Bank_Transaction, 2);
			if (timer > 0.075f) {
				timer = 0f;
				VFXDisplayer.DisplayMovingVFXUIToWOrld(LoveTriangle.ObjectPoolLibraryCommon.PoolType.FLOATING_MONEY, m_ingameUIController.moneyUI.position, transform.position);
			}
			timer += Time.deltaTime;
		}
	}

	IEnumerator DoDeposit() {
		float speed = 0.05f;
		float incrementor = 1;
		int maxClampAmount = PlayerData.BankMoney + PlayerData.CurrentMoney;
		maxClampAmount = Math.Clamp(maxClampAmount, 0, GuyStatus.Instance.MaxDepositLimit);
		float timer = 0f;
		VFXDisplayer.DisplayMovingVFXWOrldToUI(LoveTriangle.ObjectPoolLibraryCommon.PoolType.FLOATING_MONEY, transform.position, m_ingameUIController.moneyUI.position);
		
		while (PlayerData.CurrentMoney > 0 && PlayerData.BankMoney < GuyStatus.Instance.MaxDepositLimit) {
			PlayerData.DecreaseMoney((int)incrementor, true);
			PlayerData.IncreaseBankMoney((int)incrementor, true, maxClampAmount);
			onMoneyUpdate?.Invoke();
			yield return new WaitForSeconds(speed);
			speed -= 0.05f;
			speed = Mathf.Clamp(speed, 0f, 10000000f);
			incrementor += 0.25f;
			SFXPlayer.Instance.PlaySFXWait(SFXPlayer.SFX_TYPE.Bank_Transaction, 2);
			if (timer > 0.075f) {
				timer = 0f;
				VFXDisplayer.DisplayMovingVFXWOrldToUI(LoveTriangle.ObjectPoolLibraryCommon.PoolType.FLOATING_MONEY, transform.position, m_ingameUIController.moneyUI.position);
			}
			timer += Time.deltaTime;
		}
			
	}
}
