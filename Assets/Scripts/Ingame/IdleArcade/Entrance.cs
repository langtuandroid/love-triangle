using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour {

	public AnimatorController ladyController;
	public LadiesProgressBar.LadyType ladyType;
	public static System.Action<AnimatorController, LadiesProgressBar.LadyType, bool> onEnter;

	bool m_isEntered;
	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<CarCollider>() != null) {
			onEnter?.Invoke(ladyController, ladyType, m_isEntered);
			PlayerData.EnteredHouse = ladyType;
			//GetComponent<Collider>().enabled = false;
			m_isEntered = true;
		}
	}
}
