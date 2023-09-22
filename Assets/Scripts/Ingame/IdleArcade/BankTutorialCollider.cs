using System.Collections;
using System;
using UnityEngine;

public class BankTutorialCollider : MonoBehaviour {
	public string message;
	public static Action<string> onEnterBankTutorial;
	public static Action onExitBankTutorial;
	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<CarCollider>() != null) {
			onEnterBankTutorial?.Invoke(message);
		}
	}
	private void OnTriggerExit(Collider other) {
		if (other.GetComponent<CarCollider>() != null) {
			onExitBankTutorial?.Invoke();
		}
	}
}
