using System;
using LoveTriangle.UI;
using UnityEngine;

public class MainMenuUIView : MVCUIView {
    #region interface for listener
    public interface IListener {
        void OnClickPlay();
        void OnClickSettings();
    }
    #endregion
    #region MVC Properties and functions to override
    /*
     * this will be the reference to the model 
     * */
    public MainMenuUIModel UIModel {
        get {
            return _baseAssetModel as MainMenuUIModel;
        }
    }

    /*
     * Call this Create method to Initialize and instantiate the UI.
     * There's a callback on the controller if you want custom initialization
     * */
    public static void Create(Canvas p_canvas, MainMenuUIModel p_assets, Action<MainMenuUIView> p_onCreate) {
        var go = new GameObject(typeof(MainMenuUIView).ToString());
        var gui = go.AddComponent<MainMenuUIView>();
        var assetsInstance = Instantiate(p_assets);
        gui.Init(p_canvas, assetsInstance);
        if (p_onCreate != null) {
            p_onCreate.Invoke(gui);
        }
    }
    #endregion

    public void ShowMakeThemHappyTutorial() {
        UIModel.goTutorial.SetActive(true);
    }

    public void HideAllTutorials() {
        UIModel.goTutorial.SetActive(false);
    }

    public void SetLevel(int p_level) {
        UIModel.txtLevel.text = "LEVEL " + p_level;
    }

    #region Subscribe/Unsubscribe for IListener
    public void Subscribe(IListener p_listener) {
        UIModel.onClickPlay += p_listener.OnClickPlay;
        UIModel.onClickSettings += p_listener.OnClickSettings;
    }
    public void Unsubscribe(IListener p_listener) {
        UIModel.onClickPlay -= p_listener.OnClickPlay;
        UIModel.onClickSettings -= p_listener.OnClickSettings;
    }
    #endregion
}