using System.Collections;
using System;
using UnityEngine;
using Dreamteck.Splines;
using LoveTriangle;

public class IntroSequence : MonoBehaviour {
    public static Action onStartMoving;
    public static Action onTransitionDone;
    public float speed;
    [SerializeField]
    private SplineFollower m_target;
    [SerializeField]
    private SplineFollower m_camera;

    public bool dontTraverseBack;
    public Transform targetPosAndRot;

    [SerializeField] private Cinemachine.CinemachineVirtualCamera m_endCamera;


    private void OnEnable() {
        IngameUIView.onDoneMoneyAnimation += OnDone;
        Gentleman.onPause += OnPause;
    }

	private void OnDisable() {
        IngameUIView.onDoneMoneyAnimation -= OnDone;
        Gentleman.onPause -= OnPause;
    }

	public void StartDoSequence() {
        if(!dontTraverseBack){
            m_camera.enabled = true;
            m_camera.direction = Spline.Direction.Backward;
            m_camera.followSpeed = speed;
            m_camera.onBeginningReached += OnClickPlay;
        } else {
            OnClickPlay(0f);
            m_camera.enabled = true;
        }
    }
    IEnumerator DoSequence() {
        yield return new WaitForSeconds(3f);
        //OnClickPlay();
    }

    public void OnClickPlay(double p_val) {
        
        m_camera.onBeginningReached -= OnClickPlay;
        m_target.enabled = true;
        m_camera.enabled = true;
        if(dontTraverseBack){
            m_camera.spline = m_target.spline;
            m_camera.SetPercent(0f);
        }   
        m_camera.direction = Spline.Direction.Forward;
        m_camera.followSpeed = m_target.followSpeed;
        //ClikManager.Instance.CallClikEventGameStart(PlayerData.CurrentLevel + 1);
        onStartMoving?.Invoke();
    }

    void OnDone() {
        //MergingHearts.onGoToIdleArcade -= OnDone;
        //if (m_camera.GetComponentInChildren<LeanClamper>() != null) {
        //    m_camera.GetComponentInChildren<LeanClamper>().enabled = false;
        //}

        //m_camera.spline = null;
        //m_camera.enabled = false;
        //targetPosAndRot.transform.SetParent(null);
        //LeanTween.move(gameObject, targetPosAndRot.position, 1f);
        //LeanTween.rotate(gameObject, targetPosAndRot.eulerAngles, 1f).setOnComplete(() => onTransitionDone?.Invoke());
        //CameraFollow cameraFollow = GetComponentInChildren<CameraFollow>();
        //cameraFollow.distanceZ = 4;
        //cameraFollow.distanceY = -3.5f;
        //cameraFollow.distanceX = 0;
        //StartCoroutine(GradualIncreaseFOV(72f));
        m_endCamera.Priority = 11;
        LeanTween.delayedCall(1.0f, () => onTransitionDone?.Invoke());
    }

    IEnumerator GradualIncreaseFOV(float p_targetValue) {
        float speed = 2f;
        while (Camera.main.fieldOfView < p_targetValue) {
            Camera.main.fieldOfView += speed * Time.deltaTime;
            yield return 0;
        }
        Camera.main.fieldOfView = p_targetValue;
    }

	private void OnPause(bool p_pause, SplineFollower p_follower) {
        if (p_pause) {
            m_camera.followSpeed = 0f;
        }
        else {
            m_camera.followSpeed = p_follower.followSpeed;
        }
	}
}
