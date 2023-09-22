using System;
using LoveTriangle.UI;
using UnityEngine;

public class SettingsUIView : MVCUIView {
    #region interface for listener
    public interface IListener {
        void OnClickClose();
        void OnClickTaptic();
        void OnClickSound();
        void OnClickMusic();
    }
    #endregion
    #region MVC Properties and functions to override
    /*
     * this will be the reference to the model 
     * */
    public SettingsUIModel UIModel {
        get {
            return _baseAssetModel as SettingsUIModel;
        }
    }

    /*
     * Call this Create method to Initialize and instantiate the UI.
     * There's a callback on the controller if you want custom initialization
     * */
    public static void Create(Canvas p_canvas, SettingsUIModel p_assets, Action<SettingsUIView> p_onCreate) {
        var go = new GameObject(typeof(SettingsUIView).ToString());
        var gui = go.AddComponent<SettingsUIView>();
        var assetsInstance = Instantiate(p_assets);
        gui.Init(p_canvas, assetsInstance);
        if (p_onCreate != null) {
            p_onCreate.Invoke(gui);
        }
    }
    #endregion

    #region Subscribe/Unsubscribe for IListener
    public void Subscribe(IListener p_listener) {
        UIModel.onClickClose += p_listener.OnClickClose;
        UIModel.onClickTaptic += p_listener.OnClickTaptic;
        UIModel.onClickMusic += p_listener.OnClickMusic;
        UIModel.onClickSound += p_listener.OnClickSound;
    }
    public void Unsubscribe(IListener p_listener) {
        UIModel.onClickClose -= p_listener.OnClickClose;
        UIModel.onClickTaptic -= p_listener.OnClickTaptic;
        UIModel.onClickMusic -= p_listener.OnClickMusic;
        UIModel.onClickSound -= p_listener.OnClickSound;
    }
    #endregion
}