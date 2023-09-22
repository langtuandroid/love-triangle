using System.Collections;
using System;
using UnityEngine;

public class ExitGate : MonoBehaviour {
	public static Action onForcedGateAnimExit;
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<CarCollider>() != null) {
			onForcedGateAnimExit?.Invoke();
		}
	}
}
