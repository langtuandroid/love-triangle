using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressVisuals : MonoBehaviour {

    public System.Action<int, float, Color> onStatusChanged;
    public Image imgProgressBar;
    public Animator m_animator;
    public Vector3 m_origScale;
    public Vector3 m_tweenScale;
    private int m_currentStatus;
    public bool IsOnGateAnimation { private set; get; }
    public ParticleSystem psFlirty;
    public ParticleSystem psInlove;

    private Coroutine m_increaseCoroutine;
    private Coroutine m_decreaseCoroutine;

    private IEnumerator m_processOnAnimEnd;

    private void OnEnable() {
        GateSceneAnimationManager.onAnimationStart += OnAnimationStart;
        GateSceneAnimationManager.onAnimationEnd += OnAnimationEnd;
        
    }

    private void OnDisable() {
        GateSceneAnimationManager.onAnimationStart -= OnAnimationStart;
        GateSceneAnimationManager.onAnimationEnd -= OnAnimationEnd;
        psFlirty.gameObject.SetActive(false);
        psInlove.gameObject.SetActive(false);
    }

    void Start() {
        imgProgressBar.fillAmount = 0f;
        m_origScale = transform.localScale;
        m_tweenScale = m_origScale + new Vector3(0.01f, 0.01f, 0.01f);
    }

    void OnAnimationStart(Transform p_lady, Vector3 p_uiBarScale) {
        IsOnGateAnimation = true;
        m_processOnAnimEnd = null;
        if (m_increaseCoroutine != null) {
            StopCoroutine(m_increaseCoroutine);
            m_increaseCoroutine = null;
        }
    }

    void OnAnimationEnd() {
        IsOnGateAnimation = false;
    }

    public void AddProgressBarPerSecondTrigger() {
        if (m_decreaseCoroutine != null) {
            StopCoroutine(m_decreaseCoroutine);
            m_decreaseCoroutine = null;
        }
        if (m_increaseCoroutine == null) {
            m_increaseCoroutine = StartCoroutine(AdjustProgressBarPerSecond(0.4f));
        }
    }

    void ProcessStatus() {
        if (imgProgressBar.fillAmount <= 0.33f) {
            m_currentStatus = 0; 
        }
        else if (imgProgressBar.fillAmount <= 0.66f) {
            if (m_currentStatus != 1) {
                if (IsOnGateAnimation) {
                    m_processOnAnimEnd = ShowFlirty();
                } else {
                    StartCoroutine(ShowFlirty());
                }
            }
            m_currentStatus = 1;
            
        }
        else if(imgProgressBar.fillAmount <= 1f){
            if (m_currentStatus != 2) {
                if (IsOnGateAnimation) {
                    m_processOnAnimEnd = ShowInlove();
                } else {
                    StartCoroutine(ShowInlove());
                }
            }
            m_currentStatus = 2;
        }
        onStatusChanged?.Invoke(m_currentStatus, imgProgressBar.fillAmount, imgProgressBar.color);
    }

    IEnumerator AdjustProgressBarPerSecond(float p_adder) {
        while (true) {
            if (!IsOnGateAnimation) {
                if (p_adder > 0) {
                    imgProgressBar.fillAmount += (0.30f * p_adder * Time.deltaTime);
                }
                else {
                    imgProgressBar.fillAmount += (0.075f * p_adder * Time.deltaTime);
                }
            
                imgProgressBar.fillAmount = Mathf.Clamp(imgProgressBar.fillAmount, 0f, 1f);

                UpdateProgressColor();
                if (p_adder > 0) {
                    //TapticPlayer.PlayTapticLight();
                }
                ProcessStatus();
            }
            yield return 0;
        }
    }

    private void UpdateProgressColor() {
        if (imgProgressBar.fillAmount >= 0.5f) {
            imgProgressBar.color = Color.Lerp(Color.yellow, Color.green, imgProgressBar.fillAmount);
        } else {
            imgProgressBar.color = Color.Lerp(Color.red, Color.yellow, imgProgressBar.fillAmount * 2.0f);
        }
    }

    IEnumerator ShowFlirty() {
        psFlirty.gameObject.SetActive(true);
        psFlirty.Play();
        yield return new WaitForSeconds(2f);
        psFlirty.Stop();
        psFlirty.gameObject.SetActive(false);
    }

    IEnumerator ShowInlove() {
        psInlove.gameObject.SetActive(true);
        psInlove.Play();
        yield return new WaitForSeconds(2f);
        psInlove.Stop();
        psInlove.gameObject.SetActive(false);
    }

    public void ReduceProgressBarPerSecondTrigger() {
        if (m_increaseCoroutine != null) {
            StopCoroutine(m_increaseCoroutine);
            m_increaseCoroutine = null;
        }
        if (m_decreaseCoroutine == null) {
            m_decreaseCoroutine = StartCoroutine(AdjustProgressBarPerSecond(-0.25f));
        }
    }

    public void StopAllProgressBarActions() {
        StopAllCoroutines();
    }

    public void IncreaseProgressBar(float p_amount) {
        imgProgressBar.fillAmount += p_amount;
        UpdateProgressColor();
        ProcessStatus();
    }
}