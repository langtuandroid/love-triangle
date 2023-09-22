using System.Collections;
using LoveTriangle;
using UnityEngine;
using Lean;

public class CameraZoomer : MonoBehaviour {
    public Transform mainTarget;
    public float zoomOutSubtracter;
    public float speedAdjuster;

    public Camera targetCamera;
    Camera m_cam;
    void Awake(){
        m_cam = Camera.main;
    }

    void OnEnable(){
        //Gentleman.onDone += OnDone;
    }

    void OnDisable(){
        //Gentleman.onDone -= OnDone;
    }

    void OnDone(GameObject p_guy, GameObject p_wife, GameObject p_mistress){
        if(targetCamera == null){
            StartCoroutine(AnimateFOV());
        } else {
            StartCoroutine(DucplicateOthercameraSettings());
        }
    }

    IEnumerator AnimateFOV(){
        float adjuster = 0f;
        float currentFOV = m_cam.fieldOfView;
        while(adjuster < zoomOutSubtracter){
            adjuster += speedAdjuster * Time.deltaTime;
            m_cam.fieldOfView = currentFOV - adjuster;
            transform.LookAt(mainTarget);
            yield return 0;
        }
    }

    IEnumerator DucplicateOthercameraSettings(){
        LeanTween.rotate(m_cam.gameObject, targetCamera.transform.eulerAngles, 1f);
        LeanTween.moveLocal(m_cam.gameObject, targetCamera.transform.localPosition, 1f);
        float adjuster = 0f;
        float currentFOV = m_cam.fieldOfView;
        while(m_cam.fieldOfView > zoomOutSubtracter){
            adjuster += speedAdjuster * Time.deltaTime;
            m_cam.fieldOfView = currentFOV - adjuster;
            transform.LookAt(mainTarget);
            yield return 0;
        }
        yield return new WaitForSeconds(1f);
        SimpleScreenShotTaker.Capture();
    }
}
