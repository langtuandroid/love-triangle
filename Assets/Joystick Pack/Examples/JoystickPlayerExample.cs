using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoveTriangle;

public class JoystickPlayerExample : MonoBehaviour
{
    public AnimatorController animator;
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public Transform ladyPosition;
    private AnimatorController m_lady;

    Vector3 direction;
    bool m_startNow;

	private void OnEnable() {
        IntroSequence.onTransitionDone += OnStartArcadeIdle;
        PostGameUIController.onShowUI += OnPostGameShow;
        Entrance.onEnter += OnEnterHouse;
        Gentleman.onDone += OnReachEndLevel;
        SpeechBubbleUI.onStartSpeechBubble += DisableController;
        SpeechBubbleUI.onDoneSpeechBubble += EnableController;
    }

    private void OnDisable() {
        IntroSequence.onTransitionDone -= OnStartArcadeIdle;
        Entrance.onEnter -= OnEnterHouse;
        Gentleman.onDone -= OnReachEndLevel;
        SpeechBubbleUI.onStartSpeechBubble -= DisableController;
        SpeechBubbleUI.onDoneSpeechBubble -= EnableController;
        PostGameUIController.onShowUI -= OnPostGameShow;
    }

    void OnReachEndLevel(GameObject p_goGuy, GameObject p_goWife, GameObject p_goMistress) {
        animator.PlayIdle();
    }

    void EnableController() {
        m_startNow = true;
        if (m_selectedLadyType == LadiesProgressBar.LadyType.MISTRESS) {
            if (!PlayerData.IsMistressSpeechDone) {
                Debug.LogError("LOVE TRIANGLE: FinishMistressSpeech from joystick ");
                PlayerData.FinishMistressSpeech();
            }
            
        }
        else {
            if (!PlayerData.IsWifeSpeechDone) {
                Debug.LogError("LOVE TRIANGLE: FinishWifeSpeech from joystick ");
                PlayerData.FinishWifeSpeech();
            }
        }
        m_lady.PlayWalk();
        m_lady.transform.LookAt(m_lady.transform.position + direction);
    }

    void DisableController() {
        m_startNow = false;
    }


    public void Update()
    {
        if (!m_startNow) {
            return;
        }
        if (variableJoystick.Horizontal == 0f && variableJoystick.Vertical == 0f) {
            direction = Vector3.zero;
            animator.PlayIdle();
            m_lady?.PlayIdle();
            rb.Sleep();
        }
        else {
            if (rb.IsSleeping()) {
                rb.WakeUp();
            }
            animator.PlayWalk();
            
            direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            transform.LookAt(transform.position + direction);
            
            transform.position = Vector3.MoveTowards(transform.position, (transform.position + direction), speed * Time.deltaTime);
            if (m_lady != null) {
                m_lady.PlayWalk();
                m_lady.transform.LookAt(m_lady.transform.position + direction);
            }
            //transform.move(rb.position + (direction * speed * Time.fixedDeltaTime));
            
        }
    }

    LadiesProgressBar.LadyType m_selectedLadyType;
    void OnEnterHouse(AnimatorController p_controller, LadiesProgressBar.LadyType p_ladyType, bool p_isEntered) {
        Entrance.onEnter -= OnEnterHouse;
        m_lady = p_controller;
        m_selectedLadyType = p_ladyType;
        if ((p_ladyType == LadiesProgressBar.LadyType.WIFE && !PlayerData.IsWifeSpeechDone) || (p_ladyType == LadiesProgressBar.LadyType.MISTRESS && !PlayerData.IsMistressSpeechDone))  {
            SpeechBubbleUI.onDoneSpeechBubble += ContinueToArcadeIdle;
            
        }
        else {
            ContinueToArcadeIdle();
        }
    }

    void ContinueToArcadeIdle() {
        m_startNow = true;
        SpeechBubbleUI.onDoneSpeechBubble -= ContinueToArcadeIdle;
        ladyPosition.gameObject.SetActive(true);
        m_lady.transform.parent.SetParent(ladyPosition);
        m_lady.transform.parent.localPosition = Vector3.zero;
        m_lady.transform.localPosition = Vector3.zero;
    }

    void OnPostGameShow() {
        PostGameUIController.onShowUI -= OnPostGameShow;
        m_startNow = false;
        variableJoystick.GetComponentInParent<Canvas>().gameObject.SetActive(false);
        variableJoystick.gameObject.SetActive(false);
    }

    void OnStartArcadeIdle() {
        variableJoystick.gameObject.SetActive(true);
        rb.freezeRotation = true;
        rb.GetComponent<Collider>().isTrigger = false;
        m_startNow = true;
    }
}