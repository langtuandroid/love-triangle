using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour {
	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetComponent<CarCollider>() != null) {
			collision.gameObject.transform.parent.GetComponent<LoveTriangle.Gentleman>().DisableDrag();
		}
	}

	private void OnCollisionExit(Collision collision) {
		if (collision.gameObject.GetComponent<CarCollider>() != null) {
			collision.gameObject.transform.parent.GetComponent<LoveTriangle.Gentleman>().EnableDrag();
		}
	}
}
