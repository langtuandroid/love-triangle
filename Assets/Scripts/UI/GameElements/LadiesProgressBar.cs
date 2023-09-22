using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoveTriangle;
using System.Collections;

public class LadiesProgressBar : MonoBehaviour {

    public static Action<LadyType, SatisfactionLevel, float> updateSatisfactionLevel;
    public enum LadyType { WIFE = 0, MISTRESS = 1, NONE }
    public enum SatisfactionLevel { CARELESS = 0, LIKES, INLOVE, NONE }
    public LadyType ladyType = LadyType.WIFE;
    public SatisfactionLevel currentSatisFactionLevel = SatisfactionLevel.NONE;

    private LadyType m_currentHeld;
    private Canvas m_canvas;

    public GameObject ladyObject;
    private Transform m_initialparent;

    public ProgressVisuals progressBarVisuals;
    public Vector3 m_uiInitPosition;

    public GameObject goNegativeVFX;
    public GameObject goPositiveVFX;

    public ModelTransformer transformer;

    void Awake() {
        m_canvas = GetComponent<Canvas>();
        m_initialparent = transform.parent;
        m_uiInitPosition = GetComponent<RectTransform>().transform.localPosition;
    }

    void OnEnable() {
        CarCollider.OnCollideWithGate += UpdateProgress;
        Gentleman.onLaneEnter += onLaneEnter;
        Gentleman.onHeldHands += OnHeldHand;
        GateSceneAnimationManager.onAnimationStart += ChangeUIPositionToAnimation;
        GateSceneAnimationManager.onAnimationEnd += ReturnToOriginalPosition;
        PowerUp.onPickedUp += OnPickUpPowerUp;
        Gentleman.onDone += OnDone;
    }


    private void OnDisable() {
        CarCollider.OnCollideWithGate -= UpdateProgress;
        Gentleman.onLaneEnter -= onLaneEnter;
        Gentleman.onHeldHands -= OnHeldHand;
        GateSceneAnimationManager.onAnimationStart -= ChangeUIPositionToAnimation;
        GateSceneAnimationManager.onAnimationEnd -= ReturnToOriginalPosition;
        PowerUp.onPickedUp -= OnPickUpPowerUp;
        Gentleman.onDone -= OnDone;
    }

	void OnDestroy() {
        if (progressBarVisuals != null) {
            progressBarVisuals.onStatusChanged -= OnStatusChanged;
        }
        Gentleman.onDone -= OnDone;
    }

	private void Start() {
        progressBarVisuals.onStatusChanged += OnStatusChanged;
    }

    void OnDone(GameObject p_guy, GameObject p_wife, GameObject p_mistress) {
        Gentleman.onDone -= OnDone;
        progressBarVisuals.onStatusChanged -= OnStatusChanged;
        Gentleman.onHeldHands -= OnHeldHand;
        goNegativeVFX?.gameObject.SetActive(false);
        goPositiveVFX?.gameObject.SetActive(false);
        if (goNegativeVFX != null) {
            goNegativeVFX?.gameObject.SetActive(false);
        }
        if (goPositiveVFX != null) {
            goPositiveVFX?.gameObject.SetActive(false);
        }
        m_canvas.gameObject.SetActive(false);
    }

    void OnStatusChanged(int p_level, float p_amount, Color p_col) {
        if (currentSatisFactionLevel != (SatisfactionLevel)p_level) {
            transformer.SwapModel(p_level);
            VFXDisplayer.DisplayVFX(ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_WHITE, ladyObject.transform.position, ladyObject.transform, 1f);
            currentSatisFactionLevel = (SatisfactionLevel)p_level;
            updateSatisfactionLevel?.Invoke(ladyType, currentSatisFactionLevel, progressBarVisuals.imgProgressBar.fillAmount);
        }
        
    }
	void OnPickUpPowerUp(PowerUp p_powerUp, float p_amount) {
        if (p_powerUp is HeartPowerUp) {
            if (m_currentHeld == ladyType) {
                progressBarVisuals.IncreaseProgressBar(p_amount);
            }
        }
    }
    public void UpdateProgress(Gate p_gate){
        if(m_currentHeld == ladyType){
            progressBarVisuals.IncreaseProgressBar(p_gate.bonusPoints);
            //progressBarVisuals.imgProgressBar.fillAmount += p_gate.bonusPoints;
        }
        //updateSatisfactionLevel?.Invoke(ladyType, currentSatisFactionLevel, progressBarVisuals.imgProgressBar.fillAmount);
    }


    private void onLaneEnter(LadyType p_type) {
        m_currentHeld = p_type;
    }

    void OnHeldHand(LadyType p_type){
        //progressBarVisuals.StopAllProgressBarActions();
        if (m_currentHeld == ladyType) {
            progressBarVisuals.AddProgressBarPerSecondTrigger();
            TapticPlayer.PlayTapticMedium();
            //LeanTween.scale(gameObject, progressBarVisuals.m_tweenScale, 0.25f);
            ShowPositiveVFX();
            m_canvas.enabled = true;
        } 
        
        if(p_type==LadyType.NONE || m_currentHeld != ladyType){
            progressBarVisuals.ReduceProgressBarPerSecondTrigger();
            ShowNegativeVFX();
            m_canvas.enabled = false;
            //LeanTween.scale(gameObject, progressBarVisuals.m_origScale, 0.25f);
        }
    }

    void ChangeUIPositionToAnimation(Transform p_lady, Vector3 p_uiBarScale){
        if (m_currentHeld == ladyType){
            Vector3 pos = p_lady.transform.position;
            transform.SetParent(p_lady);
            pos.y = -1f;
            m_canvas.transform.GetComponent<RectTransform>().position = pos;
            if(p_uiBarScale != Vector3.zero){
                //m_canvas.transform.GetComponent<RectTransform>().localScale = p_uiBarScale;
            } else {
                //m_canvas.transform.GetComponent<RectTransform>().localScale = new Vector3(0.02f, 0.02f, 0.02f);
            }
            
        }
    }
    void ReturnToOriginalPosition(){
        transform.SetParent(m_initialparent);
        m_canvas.transform.GetComponent<RectTransform>().localPosition = m_uiInitPosition;
        //OnHeldHand(m_currentHeld);
        //if (m_currentHeld == ladyType) {
        //    progressBarVisuals.AddProgressBarPerSecondTrigger();
        //    ShowPositiveVFX();
        //}
    }

    void ShowPositiveVFX() {
        //goNegativeVFX.gameObject.SetActive(false);
        goPositiveVFX.gameObject.SetActive(true);
    }

    void ShowNegativeVFX() {
        //goNegativeVFX.gameObject.SetActive(true);
        goPositiveVFX.gameObject.SetActive(false);
    }
}