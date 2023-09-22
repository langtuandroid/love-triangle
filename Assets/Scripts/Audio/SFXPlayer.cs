using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoveTriangle;

public class SFXPlayer : MonoBehaviour {

	public enum SFX_TYPE { Get_Cash = 0, Cash_Register, Upgrade, Item_Place, Bank_Transaction, Fire_Works }

	public List<AudioClip> wifeUpgradeSFX = new List<AudioClip>();
	public List<AudioClip> mistressUpgradeSFX = new List<AudioClip>();

	public static SFXPlayer Instance = null;
	public List<AudioSource> channels = new List<AudioSource>();

	public List<AudioClip> clips = new List<AudioClip>();
	private void OnEnable() {
		if (Instance == null) {
			Instance = this;
		}
	}

	private void OnDisable() {
		if (Instance == this) {
			Instance = null;
		}
	}

	public void PlaySFX(SFX_TYPE p_type, int p_channel = 0) {
		channels[p_channel].Stop();
		channels[p_channel].clip = clips[(int)p_type];
		channels[p_channel].Play();
	}

	public void PlaySFXWait(SFX_TYPE p_type, int p_channel = 0) {
		if (channels[p_channel].clip == clips[(int)p_type] && channels[p_channel].isPlaying) {
			return;
		}
		channels[p_channel].Stop();
		channels[p_channel].clip = clips[(int)p_type];
		channels[p_channel].Play();
	}

	public void PlayRandomWifeUpgradeSFX() {
		int index = UnityEngine.Random.Range(0, 3);
		channels[3].Stop();
		channels[3].clip = wifeUpgradeSFX[index];
		channels[3].Play();
	}

	public void PlayRandomMistressUpgradeSFX() {
		int index = UnityEngine.Random.Range(0, 3);
		channels[3].Stop();
		channels[3].clip = mistressUpgradeSFX[index];
		channels[3].Play();
	}
}
