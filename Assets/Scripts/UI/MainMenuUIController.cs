using LoveTriangle.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Dreamteck.Splines;
using AppCentralCore;

namespace LoveTriangle {
    public class MainMenuUIController : MVCUIController, MainMenuUIView.IListener {
        
        [SerializeField]
        private MainMenuUIModel m_mainMenuUIModel;
        private MainMenuUIView m_mainMenuUIView;

        [SerializeField]
        private IngameUIController m_ingameUIController;

        [SerializeField]
        private SettingsUIController m_settingsUIController;

        public IntroSequence introSequence;
        private bool m_gameStarted;
        #region Mono Calls
        private void Start() {

            //PlayerData.Initialize();
            //ClikManager.instance.Initialize();
            InstantiateUI();
        }

        void Update() {
            if (!m_gameStarted) {
                if (Input.GetMouseButtonDown(0)) {
                    m_gameStarted = true;
                    OnClickPlay();
                    HideHappyTutorial();
                }
            }
        }

		private void OnDestroy() {
            m_mainMenuUIView.Unsubscribe(this);
        }
		#endregion
		//Call this function to Instantiate the UI, on the callback you can call initialization code for the said UI
		[ContextMenu("Instantiate UI")]
        public override void InstantiateUI() {
            MainMenuUIView.Create(_canvas, m_mainMenuUIModel, (p_ui) => {
                m_mainMenuUIView = p_ui;
                m_mainMenuUIView.Subscribe(this);
                m_mainMenuUIView.SetLevel(PlayerData.CurrentStage + 1);
                InitUI(p_ui.UIModel, p_ui);
                ShowHappyTutorial();
            });
        }

        void ShowHappyTutorial() {
            if (PlayerData.CurrentStage == 0) {
                m_mainMenuUIView.ShowMakeThemHappyTutorial();
            }
        }

        void HideHappyTutorial() {
            m_mainMenuUIView.HideAllTutorials();
        }

        #region IngameUIView.IListener
        public void OnClickPlay() {
            introSequence.StartDoSequence();
            m_mainMenuUIView.HideUI();
            m_ingameUIController.InstantiateUI();
            AppCentralManager.Instance.CallStartEvent(PlayerData.CurrentStage + 1);
            //ClikManager.instance.CallClikEventGameStart(PlayerData.CurrentStage + 1);
        }

        public void OnClickSettings(){
            m_settingsUIController.ShowUI();
        }
        #endregion
    }
}