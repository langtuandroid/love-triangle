using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Common;

[RequireComponent(typeof(LeanClamper))]
[RequireComponent(typeof(LeanManualTranslate))]
public class PathBrancher : MonoBehaviour {

	public static bool IsMoving { private set; get; }
    public LeanClamper leanClamper;
	public LeanManualTranslate leanManualTranslate;
	private float m_defaultMinX;
	private float m_defaultMaxX;

	private void Awake() {
		leanClamper = GetComponent<LeanClamper>();
		leanManualTranslate = GetComponent<LeanManualTranslate>();
		m_defaultMinX = leanClamper.minXPos;
		m_defaultMaxX = leanClamper.maxXPos;
	}

	private void OnEnable() {
		EnterBranchPath.OnEnterBranch += OnEnterBranch;
		ExitBranchPath.OnExitBranch += OnExitBranch;
	}

	private void OnDisable() {
		EnterBranchPath.OnEnterBranch -= OnEnterBranch;
		ExitBranchPath.OnExitBranch -= OnExitBranch;
	}

	void OnEnterBranch(Gate p_targetGate, bool p_isLeftGate) {
		IsMoving = true;
		leanManualTranslate.enabled = false;
		float targetMinX;
		float targetMaxX;
		if (p_isLeftGate) {
			targetMaxX = -1f;
			targetMinX = m_defaultMinX;
		}
		else {
			targetMinX = 1f;
			targetMaxX = m_defaultMaxX;
		}
		if (transform.localPosition.x <= targetMaxX && transform.localPosition.x >= targetMinX) {
			if (p_isLeftGate) {
				leanClamper.maxXPos = -1f;
				leanClamper.minXPos = m_defaultMinX;
			}
			else {
				leanClamper.minXPos = 1f;
				leanClamper.maxXPos = m_defaultMaxX;
			}
			leanManualTranslate.enabled = true;
		}
		else {
			LeanTween.moveX(gameObject, p_targetGate.transform.position.x, 0.5f).setOnComplete(() => {
				if (p_isLeftGate) {
					leanClamper.maxXPos = -1f;
					leanClamper.minXPos = m_defaultMinX;
				}
				else {
					leanClamper.minXPos = 1f;
					leanClamper.maxXPos = m_defaultMaxX;
				}
				leanManualTranslate.enabled = true;
			});
		}
		
	}

	void OnExitBranch() {
		IsMoving = false;
		leanClamper.maxXPos = m_defaultMaxX;
		leanClamper.minXPos = m_defaultMinX;
	}

	void OnDoneMoving() {
		IsMoving = false;
	}
}
