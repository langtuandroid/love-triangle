using System.Collections;
using System;
using UnityEngine;
using LoveTriangle;

public class BGMPlayer : MonoBehaviour {
    public static BGMPlayer Instance = null;

	public AudioSource audioSource;
	public AudioClip runnerMusic;
	private void Start() {
		//DontDestroyOnLoad(this.gameObject);
	}

	private void OnEnable() {
		if (Instance == null) {
			Instance = this;
		}
		IntroSequence.onStartMoving += GradualVolumeUpTrigger;
		Gentleman.onDone += OnGameDone;
	}

	private void OnDisable() {
		if (Instance == this) {
			Instance = null;
		}
		IntroSequence.onStartMoving -= GradualVolumeUpTrigger;
		Gentleman.onDone -= OnGameDone;
	}

	void OnGameDone(GameObject p_guy, GameObject p_wife, GameObject p_mistress) {
		GradualVolumeDownTrigger(0f, () => {
			Invoke("SwapMusic", 1.25f);
			Invoke("GradualVolumeUpTrigger", 1.3f);
		});
	}

	public void GradualVolumeUpTrigger() {
		StartCoroutine(GradualVolumeUp());
	}

	void SwapMusic() {
		audioSource.Stop();
		audioSource.clip = runnerMusic;
		audioSource.Play();
	}

	IEnumerator GradualVolumeUp() {
		float speed = 0.5f;
		float targetVolume = 0.4f;
		while (audioSource.volume < targetVolume) {
			audioSource.volume += speed * Time.deltaTime;
			yield return 0;
		}
		audioSource.volume = targetVolume;
	}

	public void GradualVolumeDownTrigger(float p_targetVolume = 0.5f, Action p_onDone = null) {
		StartCoroutine(GradualVolumeDown(p_targetVolume, p_onDone));
	}

	IEnumerator GradualVolumeDown(float p_targetVolume = 0.4f, Action p_onDone = null) {
		float speed = 0.5f;
		float targetVolume = p_targetVolume;
		while (audioSource.volume > targetVolume) {
			audioSource.volume -= speed * Time.deltaTime;
			yield return 0;
		}
		p_onDone?.Invoke();
		audioSource.volume = targetVolume;
	}
}
