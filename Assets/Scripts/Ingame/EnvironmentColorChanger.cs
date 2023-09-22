using System.Collections;
using System;
using UnityEngine;

public class EnvironmentColorChanger : MonoBehaviour {
	public Material maxSpeedFog;
	public Material maxShieldFog;
	public Material defaultFog;

	public MeshRenderer fog;

	public GameObject goBuildingsParent;
	public GameObject goMountainsParent;

	public Vector3 heightMaxPos;
	public Vector3 heightMinPos;

	bool m_isShieldMaxLevel;

	private void Start() {
		RenderSettings.fogColor = new Color(225f / 255f, 223f / 255f, 251f / 255f);
	}
	void OnPowerUpShield(int p_shieldLevel) {
		if (p_shieldLevel == 4) {
			fog.material = maxShieldFog;
			RenderSettings.fogColor = new Color(191f / 255f, 245f / 255f, 255f / 255f);
			StopAllCoroutines();
			StartCoroutine(MoveVertically(goMountainsParent.transform, heightMaxPos));
			StartCoroutine(MoveVertically(goBuildingsParent.transform, heightMinPos));
			if (!m_isShieldMaxLevel) {
				m_isShieldMaxLevel = true;
				CameraShaker.Instance.DoShake();
			}
		} else {
			m_isShieldMaxLevel = false;
			fog.material = defaultFog;
			RenderSettings.fogColor = new Color(225f / 255f, 223f / 255f, 251f / 255f);
			StopAllCoroutines();
			StartCoroutine(MoveVertically(goMountainsParent.transform, heightMinPos));
			StartCoroutine(MoveVertically(goBuildingsParent.transform, heightMaxPos));
		}
		RenderSettings.fog = true;
	}
	void OnPowerUpSpeed(int p_speedLevel) {
		if (p_speedLevel == 4) {
			fog.material = maxSpeedFog;
			RenderSettings.fogColor = new Color(191f / 255f, 255f / 255f, 216f / 255f);
			StopAllCoroutines();
			StartCoroutine(MoveVertically(goMountainsParent.transform, heightMinPos));
			StartCoroutine(MoveVertically(goBuildingsParent.transform, heightMaxPos));
			m_isShieldMaxLevel = false;
		} else {
			fog.material = defaultFog;
			RenderSettings.fogColor = new Color(225f / 255f, 223f / 255f, 251f / 255f);
			StopAllCoroutines();
			StartCoroutine(MoveVertically(goMountainsParent.transform, heightMinPos));
			StartCoroutine(MoveVertically(goBuildingsParent.transform, heightMaxPos));
			m_isShieldMaxLevel = false;
		}
		RenderSettings.fog = true;
	}

	IEnumerator MoveVertically(Transform p_objectToMove, Vector3 p_targetPos, Action p_onDone = null) {
		//Debug.LogError(p_objectToMove.position + " -- " + p_targetPos);
		while(Vector3.Distance(p_objectToMove.position, p_targetPos) > 0.1f) {
			p_objectToMove.position = Vector3.MoveTowards(p_objectToMove.position, p_targetPos, 35f * Time.deltaTime);
			yield return 0;
		}
		p_onDone?.Invoke();
	}
}