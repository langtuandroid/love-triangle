using System.Collections;
using System;
using UnityEngine;

public class PowerUp : MonoBehaviour {
	public static Action<PowerUp, float> onPickedUp;
	private Vector3 m_initPosition;
    public float points;
	private void Start() {
		m_initPosition = transform.position;
	}
	private void OnEnable() {
		GameManager.OnRestartGame += ReturnOnPlace;
	}

	private void OnDisable() {
		GameManager.OnRestartGame -= ReturnOnPlace;
	}
	public virtual void GetPowerUp() {
        DisplaVFX();
		HideUp();
        
	}
	public void HideUp() {
		transform.position = new Vector3(transform.position.x, transform.position.y + 1000f, transform.position.z);
	}
	public void ReturnOnPlace() {
		transform.position = m_initPosition;
	}
	public virtual void DisplaVFX() { }

	[ContextMenu("Align Correctly")]
	public virtual void AlignCorrectly() {
		if (transform.localPosition.x < 0) {
			Vector3 locPos = transform.localPosition;
			locPos.x = -2.5f;
			transform.localPosition = locPos;
		}
		if (transform.localPosition.x > 0) {
			Vector3 locPos = transform.localPosition;
			locPos.x = 2.5f;
			transform.localPosition = locPos;
		}
	}
}
