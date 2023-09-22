using System.Collections;
using TMPro;
using System;
using UnityEngine;
using Dreamteck.Splines;
using Lean.Common;
using Lean.Touch;
namespace LoveTriangle {
	public class Gentleman : MonoBehaviour {
        public static Action<GameObject, GameObject, GameObject> onDone;
        public static Action<LadiesProgressBar.LadyType> onLaneEnter;
        public static Action<LadiesProgressBar.LadyType> onHeldHands;
        public static Action<bool, SplineFollower> onPause;
        public SplineFollower splineFollower;
        public Lady wife;
        public Lady mistress;
        public GameObject wifePositionWithGuy;
        public GameObject mistressPositionWithGuy;
        private Vector3 m_wifeInitLocalPos;
        private Vector3 m_mistressInitLocalPos;
        public InteractionIKHandler guyInterActions;
		public LadiesProgressBar.LadyType CurrentHeld {private set; get;}
        public float walkTiming;
        [SerializeField]
        private Collider m_collider;
        [SerializeField]
        private LeanManualTranslate m_leanManualTranslate;

        private bool m_firstHeldHandsDone;
        private float m_initSpeed;
        private bool m_isAnimating;

        private LadiesProgressBar.LadyType m_currentLane;
        private bool m_isOnGateAnimation;

        #region mono behaviour
        private void Start() {
            m_wifeInitLocalPos = wife.transform.localPosition;
            m_mistressInitLocalPos = mistress.transform.localPosition;
            PlayerData.CurrentStatusLevel = PlayerData.StatusLevel.Hobo;
            m_currentLane = LadiesProgressBar.LadyType.NONE;
            DisableDrag();
            m_leanManualTranslate.GetComponent<LeanMultiUpdate>().enabled = false;
            m_initSpeed = splineFollower.followSpeed;
            GuyStatus.Instance.Initialize();
            PlayerData.CurrentStatusLevel = GuyStatus.Instance.CurrentStatus;
        }
		private void OnEnable() {
            CarCollider.OnCollideWithGate += OnEnterGate;
            IntroSequence.onStartMoving += AnimateWalkingCharacters;
            LadiesTrigger.onTriggerEvent += OnGentlemanChooseSide;
            splineFollower.onEndReached += OnDone;
            GateSceneAnimationManager.onAnimationEnd += OnAnimationEnd;
            MergingHearts.onGoToIdleArcade += SeparateMistress;
            MergingHearts.onGoToIdleArcade += SeparateWife;
            MergingHearts.onGoToIdleArcade += DisableALlIKOfGuy;
        }

		private void OnDisable() {
            CarCollider.OnCollideWithGate -= OnEnterGate;
            splineFollower.onEndReached -= OnDone;
            IntroSequence.onStartMoving -= AnimateWalkingCharacters;
            LadiesTrigger.onTriggerEvent -= OnGentlemanChooseSide;
            GateSceneAnimationManager.onAnimationEnd -= OnAnimationEnd;
            MergingHearts.onGoToIdleArcade -= SeparateMistress;
            MergingHearts.onGoToIdleArcade -= SeparateWife;
            MergingHearts.onGoToIdleArcade -= DisableALlIKOfGuy;
        }
        #endregion

        void DisableALlIKOfGuy() {
            guyInterActions.DisableInterActionsLeftHand();
            guyInterActions.DisableInterActionsRightHand();
        }

        public void EnableDrag() {
            m_leanManualTranslate.enabled = true;
        }
        public void DisableDrag() {
            m_leanManualTranslate.enabled = false;
        }

        void OnGentlemanChooseSide(bool p_isInTrigger, LadiesProgressBar.LadyType p_type){
            m_currentLane = p_isInTrigger ? p_type : LadiesProgressBar.LadyType.NONE;
            onLaneEnter?.Invoke(m_currentLane);
            if (p_isInTrigger && m_isAnimating) {
                return;    
            }
            LeanTween.cancel(wife.gameObject);
            LeanTween.cancel(mistress.gameObject);
            StopAllCoroutines();
            CancelInvoke("AnimateWife");
            CancelInvoke("AnimateMistress");
            if (p_isInTrigger){
                switch(p_type){
                    case LadiesProgressBar.LadyType.WIFE:
                    CurrentHeld = p_type;
                    SeparateMistress();
                    OnWifeMoveForward();
                    break;
                    case LadiesProgressBar.LadyType.MISTRESS:
                    CurrentHeld = p_type;
                    SeparateWife();
                    OnMistressMoveForward();
                    break;
                }
            } else {
                if(CurrentHeld== LadiesProgressBar.LadyType.MISTRESS) {
                    SeparateMistress();
                }else if(CurrentHeld == LadiesProgressBar.LadyType.WIFE) {
                    SeparateWife();
                }
                CurrentHeld = LadiesProgressBar.LadyType.NONE;
                onHeldHands?.Invoke(LadiesProgressBar.LadyType.NONE);
            }
        }
        void PauseSpeed(){
            splineFollower.followSpeed = 0f;
            onPause?.Invoke(true, splineFollower);
            wife.animatorController.PlayIdle();
            mistress.animatorController.PlayIdle();
            guyInterActions.GetComponent<AnimatorController>().PlayIdle();
            Invoke("ResumeSpeed", 2f);
        }

        void ResumeSpeed(){
            splineFollower.followSpeed = m_initSpeed;
            onPause?.Invoke(false, splineFollower);
            wife.animatorController.PlayWalk();
            mistress.animatorController.PlayWalk();
            guyInterActions.GetComponent<AnimatorController>().PlayWalk();
        }
        void OnEnterGate(Gate p_gate){
            if (m_isOnGateAnimation) {
                return;
            }
            if (p_gate.bonusPoints > 0){
                if(CurrentHeld == LadiesProgressBar.LadyType.WIFE){
                    mistress.animatorController.PlayUpset();
                } else if(CurrentHeld == LadiesProgressBar.LadyType.MISTRESS){
                    wife.animatorController.PlayUpset();
                }
            } else {
                if(CurrentHeld == LadiesProgressBar.LadyType.WIFE){
                    mistress.animatorController.PlayHappy();
                } else if(CurrentHeld == LadiesProgressBar.LadyType.MISTRESS){
                    wife.animatorController.PlayHappy();
                }
            }
            if (p_gate.animationEvent == Gate.GateAnimationEvent.CAR_MODE) {

                splineFollower.followSpeed *= 2f;
                ExitGate.onForcedGateAnimExit += OnExitCarMode;
                (m_collider as CapsuleCollider).radius *= 3f;
                StartCoroutine(FlagOngateAnimation(0f));
            }
            else {
                StartCoroutine(FlagOngateAnimation());
            }
            
        }

        void OnExitCarMode() {
            ExitGate.onForcedGateAnimExit -= OnExitCarMode;
            splineFollower.followSpeed = m_initSpeed;
            (m_collider as CapsuleCollider).radius /= 3f;
        }
        IEnumerator FlagOngateAnimation(float p_timer = 0.5f) {
            m_isOnGateAnimation = true;
            //DisableDrag();
            yield return new WaitForSeconds(p_timer);
            m_isOnGateAnimation = false;
        }
        void OnAnimationEnd(){
            mistress.animatorController.PlayWalk();
            wife.animatorController.PlayWalk();
        }
        void OnWifeMoveForward(){
            if (m_isDone) {
                return;
            }
            StartCoroutine(GoToGuysPosition(wife.transform, wifePositionWithGuy.transform));
            Invoke("AnimateWife", walkTiming + 0.25f);
        }

        bool m_isDone;
        void OnDone(Double p_val){
            m_isDone = true;
            DisableALlIKOfGuy();
            SeparateMistress();
            SeparateWife();
            splineFollower.onEndReached -= OnDone;
            m_leanManualTranslate.transform.localPosition = Vector3.zero; 
            GetComponentInChildren<LeanClamper>().enabled = false;
            DisableDrag();
            GetComponentInChildren<LeanMultiUpdate>().enabled = false;
            splineFollower.spline = null;
            onDone?.Invoke(gameObject, wife.gameObject, mistress.gameObject);
            enabled = false;
        }
        void AnimateWife() {
            if (m_isDone) {
                SeparateWife();
                return;
            }
            onHeldHands?.Invoke(CurrentHeld);
            wife.AnimateHoldHandsLeft();
            guyInterActions.CallInteraActionsRightHand();
        }
        void SeparateWife() {
            if (m_isDone) {
                return;
            }
            //onHeldHands?.Invoke(LadiesProgressBar.LadyType.NONE);
            guyInterActions.DisableInterActionsRightHand();
            wife.SeparateHoldHands(m_wifeInitLocalPos, walkTiming);
        }
        void OnMistressMoveForward(){
            StartCoroutine(GoToGuysPosition(mistress.transform, mistressPositionWithGuy.transform));
            Invoke("AnimateMistress", walkTiming + 0.25f);
            //AnimateMistress();
        }

        void AnimateMistress(){
            if (m_isDone) {
                SeparateMistress();
                return;
            }
            onHeldHands?.Invoke(CurrentHeld);
            mistress.AnimateHoldHandsRight();
            guyInterActions.CallInteraActionsLeftHand();
        }

        void SeparateMistress(){
            //onHeldHands?.Invoke(LadiesProgressBar.LadyType.NONE);
            guyInterActions.DisableInterActionsLeftHand();
            mistress.SeparateHoldHands(m_mistressInitLocalPos, walkTiming);
        }
        IEnumerator GoToGuysPosition(Transform p_object, Transform p_target, Action onActionDone = null){
            float speed = 6f;
            while(Vector3.Distance(p_object.position, p_target.position) > 0.1f){
                p_object.position = Vector3.MoveTowards(p_object.position, p_target.position, speed * Time.deltaTime);
                yield return 0;
            }
            if(!m_firstHeldHandsDone && !GameManager.Instance.dontShowProgressbarTutorial && !PlayerData.IsTutorialDone){
                m_firstHeldHandsDone = true;
                PauseSpeed();
            }
            onActionDone?.Invoke();
        }
        void AnimateWalkingCharacters(){
            guyInterActions.GetComponent<AnimatorController>().PlayWalk();
            wife.animatorController.PlayWalk();
            mistress.animatorController.PlayWalk();
            EnableDrag();
            m_leanManualTranslate.GetComponent<LeanMultiUpdate>().enabled = true;
        }
        public void HideOnLayer(){
            m_isAnimating = true;
            foreach (Transform child in guyInterActions.transform) {
                child.gameObject.layer = LayerMask.NameToLayer("Invisible");
            }
        }
        public void ShowOnLayer(){
            foreach (Transform child in guyInterActions.transform) {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            m_isAnimating = false;
            if (m_currentLane == LadiesProgressBar.LadyType.NONE) {
                SeparateMistress();
                SeparateWife();
            }
            OnGentlemanChooseSide(m_currentLane != LadiesProgressBar.LadyType.NONE, m_currentLane);
            //StartCoroutine(OnOffCollider());
        }

        IEnumerator OnOffCollider(){
            m_collider.enabled = false;
            yield return null;
            yield return null;
            m_collider.enabled = true;
        }
    }
}