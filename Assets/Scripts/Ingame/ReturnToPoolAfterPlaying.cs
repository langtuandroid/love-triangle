using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoveTriangle;
public class ReturnToPoolAfterPlaying : MonoBehaviour {

	public float timer = 1f;
	public ObjectPoolLibraryCommon.PoolType poolType;

	Vector3 m_initialScale;
	Quaternion m_initialRotate;
	public bool resetScale;
	private void Awake() {
		m_initialScale = transform.localScale;
		m_initialRotate = transform.rotation;
	}
	private void OnEnable() {
		StartCoroutine(TimerReturned());
	}
	IEnumerator TimerReturned() {
		if (resetScale) {
			ResetScale();
		}
		yield return new WaitForSeconds(timer);
		if (resetScale) {
			ResetScale();
		}
		ObjectPoolLibraryCommon.instance.GetObjectPooler(poolType).ReturnObject(this.gameObject);
	}

	public void ForceReturn() {
		StopAllCoroutines();
		ObjectPoolLibraryCommon.instance.GetObjectPooler(poolType).ReturnObject(this.gameObject);
		if (resetScale) {
			ResetScale();
		}
	}

	void ResetScale() {
		transform.rotation = Quaternion.identity;
		transform.localScale = m_initialScale;
	}
}