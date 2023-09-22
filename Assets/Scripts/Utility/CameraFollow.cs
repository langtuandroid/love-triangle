using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour {
    [SerializeField]
    private Transform m_target;
    public float distanceZ;
    public float distanceX;
    public float distanceY;

    private Camera m_camera;
    private bool m_dontFollow;
    private Vector3 m_camPos;

    [SerializeField]
    private float m_followSpeed;

    [SerializeField]
    private LeanClamper m_leanClamper;

    private float m_xAddOn;

    void OnEnable(){
        LadiesTrigger.onTriggerEvent += OnGentlemanChooseSide;
    }
    void OnDisble(){
        LadiesTrigger.onTriggerEvent -= OnGentlemanChooseSide;
    }
    public void DisableMe(){
        m_dontFollow = true;
    }

    public void EnableMe(){
        m_dontFollow = false;
    }

    void Update() {
        if(m_target == null || m_dontFollow){
            return;
        }
        FollowTarget();
    }

    void FollowTarget() {
        m_camPos = transform.position;
        m_camPos.z = m_target.position.z - distanceZ;
        m_camPos.y = m_target.position.y - distanceY;
        m_camPos.x = (m_target.position.x - distanceX) + m_xAddOn;
        //m_camPos.x = m_target.position.x - distanceX;
        //m_camPos.x = Mathf.Clamp(m_camPos.x, m_leanClamper.minXPos, m_leanClamper.maxXPos);
		transform.position = Vector3.MoveTowards(transform.position, m_camPos, m_followSpeed * Time.deltaTime);
    }

    void OnGentlemanChooseSide(bool p_isInTrigger, LadiesProgressBar.LadyType p_type){
        if(p_type == LadiesProgressBar.LadyType.WIFE){
            m_xAddOn = 0.015f;
        } else if(p_type == LadiesProgressBar.LadyType.MISTRESS){
            m_xAddOn = -0.015f;
        } else {
            m_xAddOn = 0f;
        }
    }
}