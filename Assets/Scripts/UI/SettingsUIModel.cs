using System;
using LoveTriangle.UI;
using UnityEngine.UI;

public class SettingsUIModel : MVCUIModel {

    public Action onClickClose;
    public Action onClickTaptic;
    public Action onClickSound;
    public Action onClickMusic;
    public Button btnClose;
    public Button btnTaptic;
    public Button btnSound;
    public Button btnMusic;

    public Image imgTapticOn;
    public Image imgTapticOff;
    public Image imgSoundOn;
    public Image imgSoundOff;
    public Image imgMusicOn;
    public Image imgMusicOff;
    private void OnEnable() {
        btnClose.onClick.AddListener(OnClickClose);
        btnTaptic.onClick.AddListener(OnClickTaptic);
        btnSound.onClick.AddListener(OnClickSound);
        btnMusic.onClick.AddListener(OnClickMusic);
    }
    private void OnDisable() {
        btnClose.onClick.RemoveListener(OnClickClose);
        btnTaptic.onClick.RemoveListener(OnClickTaptic);
        btnSound.onClick.RemoveListener(OnClickSound);
        btnMusic.onClick.RemoveListener(OnClickMusic);
    }

    private void OnClickClose() {
        onClickClose?.Invoke();
    }
    private void OnClickTaptic() {
        onClickTaptic?.Invoke();
    }
    private void OnClickSound() {
        onClickSound?.Invoke();
    }
    private void OnClickMusic() {
        onClickMusic?.Invoke();
    }
}