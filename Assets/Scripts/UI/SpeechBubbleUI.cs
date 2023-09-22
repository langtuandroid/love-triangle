using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
public class SpeechBubbleUI : MonoBehaviour {
    public static SpeechBubbleUI Instance = null;
    public static Action onStartSpeechBubble;
    public static Action onDoneSpeechBubble; 
    public TextMeshProUGUI txtMessage;
    public TextMeshProUGUI txtSpeaker;

    private List<string> m_activeMessage = new List<string>();
    bool m_waitForTap;

    public GameObject goMessage;
    public Image imgCue;

	private void OnEnable() {
        Instance = Instance == null ? this : null;
	}

    private void OnDisable() {
        Instance = Instance == this ? null : null;
    }
    private void Update() {
        if (m_waitForTap) {
            if (Input.GetMouseButton(0)) {
                m_waitForTap = false;
                onDoneSpeechBubble?.Invoke();
                imgCue.gameObject.SetActive(false);
                goMessage.SetActive(false);
            }
        }
        
    }

    public void DisplayMessageTrigger(List<string> p_messages, string p_speaker, Transform p_parent) {
        transform.SetParent(p_parent);
        Vector3 pos = transform.localPosition = Vector3.zero;
        pos.y += .5f;
        transform.localPosition = pos;
        StartCoroutine(DisplayMessage(p_messages, p_speaker));
    }

	IEnumerator DisplayMessage(List<string> p_messages, string p_speaker) {
        onStartSpeechBubble?.Invoke();
        transform.SetAsLastSibling();
        goMessage.SetActive(true);
        txtMessage.text = string.Empty;
        m_activeMessage = p_messages;
        txtSpeaker.text = p_speaker;
        int countLetters = 0;
        int countSentence = 0;
        while (countSentence < m_activeMessage.Count) {
            while (countLetters < m_activeMessage[countSentence].Length) {
                txtMessage.text += m_activeMessage[countSentence][countLetters];
                countLetters++;
                yield return new WaitForSeconds(0.015f);
            }
            txtMessage.text += " ";
            countLetters = 0;
            countSentence++;
            if (countSentence < m_activeMessage.Count) {
                yield return new WaitForSeconds(0.5f);
            }
        }
        imgCue.gameObject.SetActive(true);
        LeanTween.color(imgCue.rectTransform, new Color(1f, 1f, 1f, 0f), 0.15f).setLoopPingPong(10000);
        yield return new WaitForSeconds(0.5f);
        m_waitForTap = true;
    }
}
