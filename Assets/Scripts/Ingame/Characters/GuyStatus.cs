using LoveTriangle;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuyStatus : MonoBehaviour {

    public static GuyStatus Instance = null;
    public PlayerData.StatusLevel CurrentStatus;

    //private List<int> m_moneyValues = new List<int>() { 0, 1500, 3000, 5000, 7500 };
    [SerializeField] private List<GuyStatusState> m_statusStates;
    public GameObject goProgressBar;
    public Image imgProgressBar;
    public TextMeshProUGUI txtStatus;

    [SerializeField] private RectTransform m_progressBarCanvasRect;
    [SerializeField] private RectTransform[] m_divisorRects;

    [System.Serializable]
    public struct GuyStatusState {
        public int moneyValue;
        public Color statusColor;
    }
    private enum StatusDisplayType { Divisions, OneBar }
    [SerializeField] StatusDisplayType m_currentStatusDisplayType = StatusDisplayType.Divisions;

    //public int MaxDepositLimit => m_moneyValues[m_moneyValues.Count - 1];
    public int MaxDepositLimit => m_statusStates[m_statusStates.Count - 1].moneyValue;
    [SerializeField]
    private ModelTransformer m_modelTransformer;

    private int m_currentStatusIndex = 0;
    private int m_targetStatusIndex = 1;

    private void OnEnable() {
        Instance = Instance == null ? this : null;
        BankTransferMode.onMoneyUpdate += OnMoneyUpdate;
        MergingHearts.onGoToIdleArcade += OnRunnerDone;
        Entrance.onEnter += OnEnterHouse;
    }

    private void OnDisable() {
        Instance = Instance == this ? null : null;
        BankTransferMode.onMoneyUpdate -= OnMoneyUpdate;
        MergingHearts.onGoToIdleArcade -= OnRunnerDone;
        Entrance.onEnter -= OnEnterHouse;
    }

    private void Awake() {
        int maxDep = MaxDepositLimit;
        if (m_currentStatusDisplayType == StatusDisplayType.Divisions) {
            for(int i = 0; i < m_statusStates.Count - 2; i++) {
                GuyStatusState nextState = m_statusStates[i+1];
                m_divisorRects[i].gameObject.SetActive(true);
                m_divisorRects[i].anchoredPosition = new Vector2((1.0f - (nextState.moneyValue / (float)maxDep)) * m_progressBarCanvasRect.rect.width, 0);
            }
        } else {
            foreach(RectTransform rect in m_divisorRects) {
                rect.gameObject.SetActive(false);
            }
        }
    }

    void OnRunnerDone() {
        ShowUI();
    }

    void OnEnterHouse(AnimatorController p_chosenGirlAnimator, LadiesProgressBar.LadyType p_ladyType, bool p_isEntered) {
        HideUI();
    }

    public void ShowUI() {
        goProgressBar.SetActive(true);
        UpdateProgressBar();
    }

    public void HideUI() {
        goProgressBar.SetActive(false);
    }

    public void Initialize() {
        Load();
        ProcessStatusVisual(false);
        UpdateProgressBar();
    }

    public void ProcessStatusVisual(bool p_doVFX = true) {
        m_modelTransformer.SwapModel((int)CurrentStatus);
        if (p_doVFX) {
            VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_WHITE, m_modelTransformer.transform.position, m_modelTransformer.transform, 1f);

        }
        txtStatus.text = CurrentStatus.ToString();
        Save();
    }

    void OnMoneyUpdate() {
        int bm = PlayerData.BankMoney;
        //int result =
        //    bm <= m_moneyValues[1] ? 0 :
        //    bm <= m_moneyValues[2] ? 1 :
        //    bm <= m_moneyValues[3] ? 2 :
        //    bm <= m_moneyValues[4] ? 3 :
        //    4;
        int result = 0;
        for (int i = m_statusStates.Count-1; i >0; i--) {
            if (PlayerData.BankMoney >= m_statusStates[i].moneyValue) {
                result = i;
                break;
            }
        }

        if (CurrentStatus != (PlayerData.StatusLevel)result) {
            CurrentStatus = (PlayerData.StatusLevel)result;
            ProcessStatusVisual();
        }

        m_currentStatusIndex = result;
        m_targetStatusIndex = Mathf.Clamp(result + 1, 0, m_statusStates.Count-1);

        UpdateProgressBar();
    }

    void UpdateProgressBar() {
        if (m_currentStatusDisplayType == StatusDisplayType.Divisions) {
            imgProgressBar.fillAmount = (float)(PlayerData.BankMoney) / (float)(MaxDepositLimit);
        } else {
            float oneProgresss = (PlayerData.BankMoney - m_statusStates[m_currentStatusIndex].moneyValue) / (float)(m_statusStates[m_targetStatusIndex].moneyValue - m_statusStates[m_currentStatusIndex].moneyValue);
            imgProgressBar.fillAmount = oneProgresss;
        }
        imgProgressBar.color = m_statusStates[m_currentStatusIndex].statusColor;
    }

	public void Save() {
        PlayerPrefs.SetInt("CurrentStatus", (int)CurrentStatus);
    }

    public void Load() {
        CurrentStatus = (PlayerData.StatusLevel)PlayerPrefs.GetInt("CurrentStatus", 0);
        m_currentStatusIndex = (int)CurrentStatus;
        m_targetStatusIndex = Mathf.Clamp(m_currentStatusIndex + 1, 0, m_statusStates.Count - 1);
        Debug.LogError("Status: " + CurrentStatus);
    }
}
