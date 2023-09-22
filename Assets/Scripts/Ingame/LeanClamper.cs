using System.Collections;
using System;
using UnityEngine;

public class LeanClamper : MonoBehaviour {
    public static Action onRightMostReached;
    public static Action onLeftMostReached;
    public static Action onNotEdge;
    public float minXPos;
    public float maxXPos;
	public float minRotYangle;
	public float maxRotYangle;

	private Vector3 currentlocalPos;
	private Vector3 currentLocalRot;
	private float currentXPos;
	public Transform carModel;
	public bool dontMove;
    public bool dontClampRotation;
    public bool touchOnlyX;
    private bool m_isOnEdge;

    public float m_edgeThreshold = 0.35f;
	private void Start() {
		if (carModel == null) {
			carModel = transform;
		}
	}
	private void Update() {
		
	}

	private void LateUpdate() {
		if(carModel == null) {
			return;
		}
		if (dontMove) {
			return;
		}
		currentlocalPos = transform.localPosition;
		currentXPos = Mathf.Clamp(currentlocalPos.x, minXPos, maxXPos);
        if(currentXPos <=  minXPos + m_edgeThreshold){
            if(!m_isOnEdge){
                m_isOnEdge = true;
                onLeftMostReached?.Invoke();
            }
        } else if(currentXPos >= maxXPos - m_edgeThreshold){
            if(!m_isOnEdge){
                m_isOnEdge = true;
                onRightMostReached?.Invoke();
            }
        } else {
            if(m_isOnEdge){
                m_isOnEdge = false;
                onNotEdge?.Invoke();
            }
        }
        if(touchOnlyX){

        } else {
            currentlocalPos.z = 0f;
		    currentlocalPos.y = 0f;
        }

		currentlocalPos.x = currentXPos;
		transform.localPosition = currentlocalPos;
	}
}
