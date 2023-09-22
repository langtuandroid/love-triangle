using System;
using LoveTriangle.UI;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
public class PostGameUIModel : MVCUIModel {

    public Action onClickPlay;

    public Button btnPlay;
    public Animator animator;
    public Text txtLevel;

    private void OnEnable() {
        btnPlay.onClick.AddListener(OnClickPlay);
    }
    private void OnDisable() {
        btnPlay.onClick.RemoveListener(OnClickPlay);
    }
    private void OnClickPlay() {
        onClickPlay?.Invoke();
    }
}