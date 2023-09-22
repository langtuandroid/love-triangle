using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public static List<Color> fogColors = new List<Color>() { new Color(191f / 255f, 248f / 255f, 255f / 255f), new Color(236f / 255f, 236f / 255f, 148f / 255f), new Color(131f / 255f, 245f / 255f, 180f / 255f) };
	public enum EnvironmentType { Sea, Beach, Forest }

	public EnvironmentType environmentType;
	public GameObject blocker;
	public Transform endScenePlacement;

	[SerializeField] private Dreamteck.Splines.SplineComputer m_levelSpline;

	public Vector3 splineEndPos => m_levelSpline.EvaluatePosition(1.0f);

	public static System.Action OnSplineLoopEnter;
	public static System.Action OnSplineLoopExit;

    private void Awake() {
        if (m_levelSpline == null) {
			m_levelSpline = FindObjectOfType<Dreamteck.Splines.SplineComputer>();
        }
    }

    private void Start() {
		blocker.SetActive(false);
	}

	private void OnEnable() {
		LoveTriangle.Gentleman.onDone += OnDone;
	}

	private void OnDisable() {
		LoveTriangle.Gentleman.onDone -= OnDone;
	}

	void OnDone(GameObject p_guy, GameObject p_wife, GameObject p_mistress) {
		blocker.SetActive(true);
	}

	public void OnLoopEnter() {
		OnSplineLoopEnter?.Invoke();
	}

	public void OnLoopExit() {
		OnSplineLoopExit?.Invoke();
	}

}
