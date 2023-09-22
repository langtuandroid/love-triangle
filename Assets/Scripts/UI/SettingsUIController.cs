using LoveTriangle.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Dreamteck.Splines;
namespace LoveTriangle {
    public class SettingsUIController : MVCUIController, SettingsUIView.IListener {
        
        [SerializeField]
        private SettingsUIModel m_settingsUIModel;
        private SettingsUIView m_settingsUIView;

        #region Mono Calls
        private void Start() {
            InstantiateUI();
        }

		private void OnDestroy() {
            m_settingsUIView.Unsubscribe(this);
        }
		#endregion
		//Call this function to Instantiate the UI, on the callback you can call initialization code for the said UI
		[ContextMenu("Instantiate UI")]
        public override void InstantiateUI() {
            SettingsUIView.Create(_canvas, m_settingsUIModel, (p_ui) => {
                m_settingsUIView = p_ui;
                m_settingsUIView.Subscribe(this);
                
                InitUI(p_ui.UIModel, p_ui);
                HideUI();
            });
        }

        #region IngameUIView.IListener
        public void OnClickClose() {
            HideUI();
        }
        public void OnClickTaptic() {
            TapticPlayer.IsOn = !TapticPlayer.IsOn;
            if(TapticPlayer.IsOn){
                m_settingsUIView.UIModel.imgTapticOn.gameObject.SetActive(true);
                m_settingsUIView.UIModel.imgTapticOff.gameObject.SetActive(false);
            } else {
                m_settingsUIView.UIModel.imgTapticOn.gameObject.SetActive(false);
                m_settingsUIView.UIModel.imgTapticOff.gameObject.SetActive(true);
            }
        }
        public void OnClickSound(){

        }
        public void OnClickMusic() {

        }
        #endregion
    }
}