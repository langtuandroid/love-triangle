using System.Collections;
using System;
using UnityEngine;
using LoveTriangle;

public class LadiesTrigger : MonoBehaviour {

    public static Action<bool, LadiesProgressBar.LadyType> onTriggerEvent;
    public LadiesProgressBar.LadyType ladyType;

    bool m_onDone;

	private void OnEnable() {
        Gentleman.onDone += OnDone;
	}

    private void OnDisable() {
        Gentleman.onDone -= OnDone;
    }
    void OnDone(GameObject p_player, GameObject p_wife, GameObject p_mistress){
        m_onDone = true;
    }

    void OnTriggerEnter(Collider p_col){
        if (m_onDone) {
            return;
        }
        if (p_col.GetComponent<CarCollider>() != null){
            onTriggerEvent?.Invoke(true, ladyType);
        }
    }

    void OnTriggerExit(Collider p_col){
        if (m_onDone) {
            return;
        }
        if (p_col.GetComponent<CarCollider>() != null){
            onTriggerEvent?.Invoke(false, ladyType);
        }
    }
}
