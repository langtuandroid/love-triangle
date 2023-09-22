using System;
using LoveTriangle.UI;
using UnityEngine;

public class PostGameUIView : MVCUIView {
    #region interface for listener
    public interface IListener {
        void OnClickPlay();
    }
    #endregion
    #region MVC Properties and functions to override
    /*
     * this will be the reference to the model 
     * */
    public PostGameUIModel UIModel {
        get {
            return _baseAssetModel as PostGameUIModel;
        }
    }

    /*
     * Call this Create method to Initialize and instantiate the UI.
     * There's a callback on the controller if you want custom initialization
     * */
    public static void Create(Canvas p_canvas, PostGameUIModel p_assets, Action<PostGameUIView> p_onCreate) {
        var go = new GameObject(typeof(PostGameUIView).ToString());
        var gui = go.AddComponent<PostGameUIView>();
        var assetsInstance = Instantiate(p_assets);
        gui.Init(p_canvas, assetsInstance);
        if (p_onCreate != null) {
            p_onCreate.Invoke(gui);
        }
    }
    #endregion

    public void SetLevelText(string p_level){
        UIModel.txtLevel.text = "Level " + p_level.ToString();
    }

    public void PlaySuccess() {
        UIModel.animator.SetTrigger("trigSuccess");
    }

    public void PlayFail() {
        UIModel.animator.SetTrigger("trigFail");
    }
    #region Subscribe/Unsubscribe for IListener
    public void Subscribe(IListener p_listener) {
        UIModel.onClickPlay += p_listener.OnClickPlay;
    }
    public void Unsubscribe(IListener p_listener) {
        UIModel.onClickPlay -= p_listener.OnClickPlay;
    }
    #endregion
}