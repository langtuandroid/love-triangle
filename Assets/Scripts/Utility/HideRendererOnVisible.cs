using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRendererOnVisible : MonoBehaviour {
	private void OnBecameVisible() {
		GetComponent<MeshRenderer>().enabled = false;
	}
}
