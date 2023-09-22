using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityMono : MonoBehaviour {
    public static UtilityMono Instance = null;
    private IEnumerator fovUpgrade;
	private void OnEnable() {
		if(Instance == null) {
			Instance = this;
		}
	}
	private void OnDisable() {
		if (Instance == this) {
			Instance = null;
		}
	}

    public void DoFovEffectTrigger(float p_fovUpgrade) {
        if (fovUpgrade != null) {
            StopCoroutine(fovUpgrade);
        }
        fovUpgrade = DoFovEffect(p_fovUpgrade);
        StartCoroutine(fovUpgrade);
    }

    IEnumerator DoFovEffect(float p_fovUpgrade) {
        if (Camera.main.fieldOfView != p_fovUpgrade) {
            while (Mathf.Abs(Camera.main.fieldOfView - p_fovUpgrade) > 1f) {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, p_fovUpgrade, 7f * Time.deltaTime);
                yield return 0;
            }
        }
        Camera.main.fieldOfView = p_fovUpgrade;
    }
}
