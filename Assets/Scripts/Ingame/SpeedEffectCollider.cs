using System.Collections;
using System;
using UnityEngine;

public class SpeedEffectCollider : MonoBehaviour {
    public static Action<SpeedEffectCollider> onCollided;
    public Transform parent;

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<CarCollider>() != null) {
			onCollided?.Invoke(this);
		}
	}

	public void ShowFlames() {
		parent.gameObject.SetActive(true);
	}

	public void HideFlames() {
		parent.gameObject.SetActive(false);
	}
}
