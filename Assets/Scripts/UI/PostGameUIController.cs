using LoveTriangle.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace LoveTriangle {
    public class PostGameUIController : MVCUIController, PostGameUIView.IListener {
        public static System.Action onShowUI;

        [SerializeField]
        private PostGameUIModel m_postGameUIModel;
        private PostGameUIView m_postGameUIView;

        private Texture2D m_texture;
        #region Mono Calls
        private void Start() {
            //InstantiateUI();
        }
        private void OnEnable() {
            IngameUIController.OnExit += OnDone;
            Entrance.onEnter += OnEnterHouse;
        }

        private void OnDisable() {
            IngameUIController.OnExit -= OnDone;
            Entrance.onEnter -= OnEnterHouse;
        }

        private void OnDestroy() {
            if(m_postGameUIView != null){
                m_postGameUIView.Unsubscribe(this);
            }
        }
        #endregion
        void OnEnterHouse(AnimatorController p_controller, LadiesProgressBar.LadyType p_ladyType, bool p_isEntered) {
            if (p_isEntered) {
                OnDone();
            }
        }
        //Call this function to Instantiate the UI, on the callback you can call initialization code for the said UI
        [ContextMenu("Instantiate UI")]
        public override void InstantiateUI() {
            PostGameUIView.Create(_canvas, m_postGameUIModel, (p_ui) => {
                m_postGameUIView = p_ui;
                m_postGameUIView.Subscribe(this);

                InitUI(p_ui.UIModel, p_ui);
                ShowUI();
                m_postGameUIView.SetLevelText(PlayerData.LevelText);
                
                onShowUI?.Invoke();
            });
        }

        void OnDone() {
            AppCentralManager.Instance.CallWinEvent(PlayerData.CurrentStage);
            //ClikManager.instance.CallClikEventGameWin(PlayerData.CurrentStage + 1);
            //m_texture = p_texture;
            InstantiateUI();
            
        }
        
        #region IngameUIView.IListener
        public void OnClickPlay() { SceneManager.LoadScene("GameScene"); }
        #endregion
    }
}