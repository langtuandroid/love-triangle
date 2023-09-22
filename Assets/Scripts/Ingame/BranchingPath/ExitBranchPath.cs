using System.Collections;
using System;
using UnityEngine;

public class ExitBranchPath : MonoBehaviour {
	public static Action OnExitBranch;

	private void OnTriggerEnter(Collider other) {
		CarCollider player = other.GetComponent<CarCollider>();
		if (player != null) {
			OnExitBranch?.Invoke();
		}
	}
}
