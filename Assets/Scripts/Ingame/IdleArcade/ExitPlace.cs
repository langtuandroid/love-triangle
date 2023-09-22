using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPlace : MonoBehaviour {
    public static System.Action onExitArcadeIdle;

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<CarCollider>() != null) {
			onExitArcadeIdle?.Invoke();
		}
	}
}
