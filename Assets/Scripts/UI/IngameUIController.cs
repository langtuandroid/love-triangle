using LoveTriangle.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace LoveTriangle {
    public class IngameUIController : MVCUIController, IngameUIView.IListener {

        public static Action OnExit;

        [SerializeField]
        private IngameUIModel m_ingameUIModel;
        private IngameUIView m_ingameUIView;

        public GameObject goWifeArrowProgressBar;
        public GameObject goMistressArrowProgressBar;

        public ProgressVisuals barWife;
        public ProgressVisuals barMistress;

        public RectTransform moneyUI;
        #region Mono Calls

        void OnEnable(){
            Gentleman.onHeldHands += OnHeldHands;
            barWife.onStatusChanged += OnWifeStatusChanged;
            barMistress.onStatusChanged += OnMistressStatusChanged;
            Room.OnRoomUnlocked += HideAllTutorials;
        }
        void OnDisable(){
            Gentleman.onHeldHands -= OnHeldHands;
            BankTransferMode.onMoneyUpdate -= UpdateMoney;
            UnlockableItem.onUpdateMoney -= UpdateMoney;
            UnlockableItem.onUnlock -= OnUnlockFurniture;
            Entrance.onEnter -= OnEnterHouse;
            barWife.onStatusChanged -= OnWifeStatusChanged;
            barMistress.onStatusChanged -= OnMistressStatusChanged;
            Gentleman.onDone -= OnLevelEndReached;
            LadiesProgressBar.updateSatisfactionLevel -= OnEnterGate;
            IntroSequence.onTransitionDone -= OnCameraTransitionDone;
            Room.OnRoomWaitingForUnlock -= ShowRoomUnlockText;
            Room.OnRoomUnlocked -= HideAllTutorials;
            BankTutorialCollider.onExitBankTutorial -= HideAllTutorials;
            MergingHearts.onGoToIdleArcade -= OnMergingeHeartsDone;
        }
        private void OnDestroy() {
            if (m_ingameUIView != null) {
                m_ingameUIView.Unsubscribe(this);
                
                BankTutorialCollider.onEnterBankTutorial -= m_ingameUIView.ShowBankDepositText;
                
            }
        }

        void HideAllTutorials() {
            m_ingameUIView.HideAllTutorials();
        }

        #endregion
        //Call this function to Instantiate the UI, on the callback you can call initialization code for the said UI
        [ContextMenu("Instantiate UI")]
        public override void InstantiateUI() {
            IngameUIView.Create(_canvas, m_ingameUIModel, (p_ui) => {
                m_ingameUIView = p_ui;
                m_ingameUIView.Subscribe(this);
                InitUI(p_ui.UIModel, p_ui);
                m_ingameUIView.UpdateMoney(PlayerData.CurrentMoney);
                m_ingameUIView.UpdateLevelDisplay(PlayerData.LevelText);
                //m_ingameUIView.ShowMakeThemBothHappyMessage(() => OnHideTutorial?.Invoke());
                BankTransferMode.onMoneyUpdate += UpdateMoney;
                UnlockableItem.onUpdateMoney += UpdateMoney;
                MergingHearts.onGoToIdleArcade += OnMergingeHeartsDone;
                UnlockableItem.onUnlock += OnUnlockFurniture;
                Entrance.onEnter += OnEnterHouse;
                Gentleman.onDone += OnLevelEndReached;
                LadiesProgressBar.updateSatisfactionLevel += OnEnterGate;
                Room.OnRoomWaitingForUnlock += ShowRoomUnlockText;

                if (PlayerData.CurrentStage == 1) {
                    BankTutorialCollider.onEnterBankTutorial += m_ingameUIView.ShowBankDepositText;
                    BankTutorialCollider.onExitBankTutorial += HideAllTutorials;
                }
                IntroSequence.onTransitionDone += OnCameraTransitionDone;
                //moneyUI = m_ingameUIView.UIModel.txtMoney.rectTransform;
            });
        }

        void ShowRoomUnlockText(Room p_room) {
            m_ingameUIView.ShowUnlockRoomText();
        }

        void OnCameraTransitionDone() {
            m_ingameUIView.ShowExitButton();
            
        }

        void OnEnterGate(LadiesProgressBar.LadyType p_type, LadiesProgressBar.SatisfactionLevel p_satisfactionLevel, float p_fillAmount) {
            switch (p_type) {
                case LadiesProgressBar.LadyType.MISTRESS:
                m_ingameUIView.UIModel.fillMistress.fillAmount = p_fillAmount;
                break;
                case LadiesProgressBar.LadyType.WIFE:
                m_ingameUIView.UIModel.fillWife.fillAmount = p_fillAmount;
                break;
            }
        }

        void OnWifeStatusChanged(int p_level, float p_amount, Color p_col) {
            m_ingameUIView.SetPartnerProgressBar(LadiesProgressBar.LadyType.WIFE, p_level, p_amount, p_col);
            //m_ingameUIView.SetWifeProgressBar(p_level, p_amount);
        }

        void OnMistressStatusChanged(int p_level, float p_amount, Color p_col) {
            m_ingameUIView.SetPartnerProgressBar(LadiesProgressBar.LadyType.MISTRESS, p_level, p_amount, p_col);
            //m_ingameUIView.SetMistressProgressBar(p_level, p_amount);
        }

        void OnLevelEndReached(GameObject goGuy, GameObject goWife, GameObject goMistress) {
            Gentleman.onDone -= OnLevelEndReached;
            m_ingameUIView.HideProgressBars();
            PlayerData.GoToNextLevel(GameManager.Instance.levels.Count);
        }

        void OnMergingeHeartsDone() {
            MergingHearts.onGoToIdleArcade -= OnMergingeHeartsDone;
            //m_ingameUIView.ShowMoneyMultiplier(PlayerData.CurrentLevelMoney.ToString() + " x " + PlayerData.GetStarCount().ToString());
            PlayerData.IncreaseMoney(PlayerData.CurrentLevelMoney * PlayerData.GetStarCount());
            //UpdateMoney();
            m_ingameUIView.AddCashWithFlyingMoney(PlayerData.CurrentMoney, PlayerData.BankMoney, GameObject.FindObjectOfType<CarCollider>().transform.position, Camera.main);
            m_ingameUIView.HideLevelText();
            
        }

        void OnEnterHouse(AnimatorController p_controller, LadiesProgressBar.LadyType p_ladyType, bool p_isEntered) {
            
            m_ingameUIView.HideAllTutorials();
            m_ingameUIView.HideMoneyInBank();
        }

        void OnUnlockFurniture(Transform p_furniture) {
            m_ingameUIView.AddCashWithFlyingMoneyReverse(PlayerData.CurrentMoney, PlayerData.BankMoney, p_furniture.transform.position, Camera.main);
        }

        void UpdateMoney() {
            m_ingameUIView.UpdateMoney(PlayerData.CurrentMoney);
            m_ingameUIView.UpdateMoneyInBank(PlayerData.BankMoney);
        }
        void OnHeldHands(LadiesProgressBar.LadyType p_type){
            if(GameManager.Instance.dontShowProgressbarTutorial || PlayerData.IsTutorialDone){
                return;
            }
            
        }

        #region IngameUIView.IListener
        public void OnClickRestart() {            //ClikManager.instance.CallClikEventGameLose(PlayerData.CurrentStage + 1);
            AppCentralManager.Instance.CallFailEvent(PlayerData.CurrentStage + 1);
            SceneManager.LoadScene("GameScene"); 
        }
        public void OnClickExit() { OnExit?.Invoke(); }
        #endregion
    }
}