using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;
public class DamageDeformer : MonoBehaviour {

    private NoiseDeformer m_noiseDeformer;
	public Vector3 localPosStart;
	public Vector3 localPosEnd;
	private void Awake() {
		m_noiseDeformer = GetComponent<NoiseDeformer>();
	}
	public void DoDeformingEffect() {
		m_noiseDeformer.enabled = true;
		transform.position = localPosStart;
		LeanTween.move(gameObject, localPosEnd, 0.5f).setOnComplete(() => {
			m_noiseDeformer.enabled = false;
		});

	}
}
