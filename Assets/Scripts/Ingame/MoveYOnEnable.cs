using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveYOnEnable : MonoBehaviour {

	public Vector3 targetPosition;

	public float leanTime;

	public void LeanMovePosition() {
		LeanTween.moveLocal(gameObject, targetPosition, leanTime);
	}
}
