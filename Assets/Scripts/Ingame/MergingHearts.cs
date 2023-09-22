using System;
using System.Collections.Generic;
using UnityEngine;

public class MergingHearts : MonoBehaviour {

	public static Action onGoToIdleArcade; 

    public GameObject goParticles;
    public List<GameObject> multipliers = new List<GameObject>();
	public List<GameObject> wifesHearts = new List<GameObject>();
	public List<GameObject> mistressHearts = new List<GameObject>();

	private void OnEnable() {
		LoveTriangle.Gentleman.onDone += OnDone;
	}

	private void OnDisable() {
		LoveTriangle.Gentleman.onDone -= OnDone;
	}

	void OnDone(GameObject p_goGuy, GameObject p_goWife, GameObject p_goMistress) {
		goParticles.SetActive(true);
		multipliers[(int)PlayerData.GetStarCount() - 1].SetActive(true);
		ProcessHearts();
		Invoke("CallEvent", 2.5f);
	}

	void CallEvent() {
		onGoToIdleArcade?.Invoke();
	}

	void ProcessHearts() {
		switch (PlayerData.WifeSatisfactionLevel) {
			case LadiesProgressBar.SatisfactionLevel.CARELESS:
			wifesHearts[0].SetActive(true);
			break;
			case LadiesProgressBar.SatisfactionLevel.LIKES:
			wifesHearts[1].SetActive(true);
			break;
			case LadiesProgressBar.SatisfactionLevel.INLOVE:
			wifesHearts[2].SetActive(true);
			break;
		}
		switch (PlayerData.MistressSatisfactionLevel) {
			case LadiesProgressBar.SatisfactionLevel.CARELESS:
			mistressHearts[0].SetActive(true);
			break;
			case LadiesProgressBar.SatisfactionLevel.LIKES:
			mistressHearts[1].SetActive(true);
			break;
			case LadiesProgressBar.SatisfactionLevel.INLOVE:
			mistressHearts[2].SetActive(true);
			break;
		}
	}
}
