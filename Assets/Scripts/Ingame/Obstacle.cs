using System.Collections;
using System;
using UnityEngine;
using Lean.Common;
public class Obstacle : MonoBehaviour {
	public static Action OnGameOver;
	public bool addForce;
	public bool dontMove;
	[SerializeField]
	private int m_levelDownAmount;
	private void OnTriggerEnter(Collider other) {
		CarCollider car = other.GetComponent<CarCollider>();
		if (car != null) {
			car.GetComponentInChildren<DamageDeformer>(false).DoDeformingEffect();
			CameraShaker.Instance.DoShake();
			if (addForce) {
				car.GetComponent<LeanClamper>().enabled = false;
				car.GetComponent<LeanManualRotate>().enabled = false;
				Rigidbody rb = car.GetComponent<Rigidbody>();
				rb.isKinematic = false;
				rb.useGravity = true;
				if (transform.position.z < car.transform.position.z) {
					rb.AddForce(Vector3.forward * 500f);
				} else {
					rb.AddForce(Vector3.forward * 500f * -1f);
				}
				OnGameOver?.Invoke();
			}
			if (!dontMove) {
				Destroy(gameObject);
			}
		}
	}
}