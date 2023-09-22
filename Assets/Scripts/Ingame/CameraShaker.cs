using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {    // Start is called before the first frame update

	public static CameraShaker Instance = null;
	[SerializeField]
	private float m_shakeTime;

	Vector3 m_initLocalPos;
	Quaternion m_initRotation;
	private void OnEnable() {
		if (Instance == null) {
			Instance = this;
		}
	}

	private void OnDisable() {
		if (Instance == this) {
			Instance = null;
		}
	}
	private void Start() {
		m_initLocalPos = transform.localPosition;
		m_initRotation = transform.localRotation;
	}

	public void DoShake() {
		SetNewInitPos();
		iTween.ShakeRotation(gameObject, new Vector3(1f, 1f, 1f), m_shakeTime);
		StartCoroutine(OnDoneShaking());
	}

	public void SetNewInitPos() {
		m_initLocalPos = transform.localPosition;
		m_initRotation = transform.localRotation;
	}
	IEnumerator OnDoneShaking() {
		yield return new WaitForSeconds(m_shakeTime + 0.05f);
		//transform.localPosition = m_initLocalPos;
		transform.localRotation = m_initRotation;
	}

	public void ResetCamera() {
		transform.localRotation = m_initRotation;
	}
}
