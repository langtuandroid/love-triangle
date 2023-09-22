using System;
using LoveTriangle.UI;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuUIModel : MVCUIModel {

    public Action onClickPlay;
    public Action onClickSettings;
    public Button btnPlay;
    public Button btnSettings;
    public Text txtLevel;
    public GameObject goTutorial;

    private void OnEnable() {
        btnPlay.onClick.AddListener(OnClickPlay);
        btnSettings.onClick.AddListener(OnClickSettings);
    }
    private void OnDisable() {
        btnPlay.onClick.RemoveListener(OnClickPlay);
        btnSettings.onClick.RemoveListener(OnClickSettings);
    }
    private void OnClickPlay() {
        onClickPlay?.Invoke();
    }
    private void OnClickSettings() {
        onClickSettings?.Invoke();
    }
}