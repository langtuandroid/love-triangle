using System;
using LoveTriangle.UI;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class IngameUIModel : MVCUIModel {

    public Action onClickRestart;
    public Action onClickExit;

    public Button btnRestart;
    public Button btnExit;
    public TextMeshProUGUI txtMoney;
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtMoneyMultiplier;
    public GameObject goMoneyInTheBank;
    public TextMeshProUGUI txtMoneyInBank;
    public RectTransform poppingCash;
    public TextMeshProUGUI newRoomUnlocked;

    public Image fillWife;
    public Image fillMistress;

    public List<Image> heartsWife = new List<Image>();
    public List<Image> heartsMistress = new List<Image>();

    [Space]
    [Header("Tutorials")]
    public GameObject goBG;
    public GameObject goBank;

    private void OnEnable() {
        btnRestart.onClick.AddListener(OnClickRestart);
        btnExit.onClick.AddListener(OnClickExit);
    }
    private void OnDisable() {
        btnRestart.onClick.RemoveListener(OnClickRestart);
        btnExit.onClick.RemoveListener(OnClickExit);
    }
    private void OnClickRestart() {
        onClickRestart?.Invoke();
    }
    private void OnClickExit() {
        onClickExit?.Invoke();
    }
}