using System.Collections;
using System;
using UnityEngine;

public class EnterBranchPath : MonoBehaviour {
	public static Action<Gate, bool> OnEnterBranch;
    public Gate leftGate;
    public Gate rightGate;

	private void OnTriggerEnter(Collider other) {
		CarCollider player = other.GetComponent<CarCollider>();
		if (player != null) {
			if (Vector3.Distance(player.transform.position, leftGate.transform.position) < Vector3.Distance(player.transform.position, rightGate.transform.position)) {
				OnEnterBranch?.Invoke(leftGate, true);
			}
			else {
				OnEnterBranch?.Invoke(rightGate, false);
			}
		}
	}
}
