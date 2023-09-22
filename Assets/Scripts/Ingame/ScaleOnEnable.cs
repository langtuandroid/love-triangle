using System.Collections;
using System;
using UnityEngine;

public class ScaleOnEnable : MonoBehaviour {

	public Action<ScaleOnEnable> OnScalingDone;

	[SerializeField]
	private float m_scaleFactor;
	
	private Vector3 m_initScale;
	[SerializeField]
	private float m_leanTime;

	public bool hideOnPlay;
	private void Awake() {
		m_initScale = transform.localScale;
	}
	private void OnEnable() {
		if (m_initScale == Vector3.zero) {
			m_initScale = transform.localScale;
		} else {
			transform.localScale = m_initScale;
		}
		
		if (hideOnPlay) {
			gameObject.SetActive(false);
			hideOnPlay = false;
		}
		AnimateScale();
	}

	private void OnDisable() {
		transform.localScale = m_initScale;
	}
	public void AnimateScale() {
		LeanTween.scale(gameObject, new Vector3(m_initScale.x + m_scaleFactor, m_initScale.y + m_scaleFactor, m_initScale.z + m_scaleFactor), m_leanTime).setLoopPingPong(1).setOnComplete(() => {
			transform.localScale = m_initScale;
			OnScalingDone?.Invoke(this);
		});
	}

	public void ScaleThenHide(Action p_onDone) {
		LeanTween.scale(gameObject, new Vector3(m_initScale.x + m_scaleFactor, m_initScale.y + m_scaleFactor, m_initScale.z + m_scaleFactor), m_leanTime).setLoopPingPong(1).setOnComplete(() => {
			transform.localScale = m_initScale;
			OnScalingDone?.Invoke(this);
			p_onDone?.Invoke();
		});
	}
}
