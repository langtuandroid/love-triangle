using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoveTriangle;
using System.Linq;

public class EndSceneSetter : MonoBehaviour {
    public Transform wifePosition;
    public Transform mistressPosition;

    public List<GameObject> lockGates = new List<GameObject>();

    public Transform furnitureParent;
    public List<UnlockableFurniture> roomFurnitures = new List<UnlockableFurniture>();
    public List<int> furniturePrices = new List<int>();

    public GameObject goDeposit;
    public GameObject goWithdraw;
    public GameObject goConfetti;
    public GameObject goBankTutorial;

    private void OnEnable() {
        Gentleman.onDone += OnDone;
        Entrance.onEnter += OnEnter;
        GameManager.OnLevelInstantiated += OnLevelInstantiated;
	}

    private void OnDisable() {
        Gentleman.onDone -= OnDone;
        Entrance.onEnter -= OnEnter;
        GameManager.OnLevelInstantiated -= OnLevelInstantiated;
    }

	private void Start() {
        lockGates.ForEach((eachGate) => eachGate.SetActive(false));
    }

    void OnLevelInstantiated(Level p_level) {
        transform.position = p_level.splineEndPos;
        //transform.position = p_level.endScenePlacement.position;
        ProcessWithdrawAndDeposit();
    }

    void OnEnter(AnimatorController p_controller, LadiesProgressBar.LadyType p_ladyType, bool p_isEntered) {
        Entrance.onEnter -= OnEnter;
        lockGates.ForEach((eachGate) => eachGate.SetActive(true));
        Debug.LogError("Chosen: " + p_ladyType + " Mistresee done?: " + PlayerData.IsMistressSpeechDone + " Wife done?: " + PlayerData.IsWifeSpeechDone);
        if (p_ladyType == LadiesProgressBar.LadyType.MISTRESS) {
            if (!PlayerData.IsMistressSpeechDone) {
                SpeechBubbleUI.Instance.DisplayMessageTrigger(new List<string>() { "I knew you would choose me! Buy me something!" }, "Mistress", p_controller.transform);
                //PlayerData.FinishMistressSpeech();
            }
            
        }
        else {
            if (!PlayerData.IsWifeSpeechDone) {
                SpeechBubbleUI.Instance.DisplayMessageTrigger(new List<string>() { "Oh you chose me over her! You're the best!" }, "Wife", p_controller.transform);
                //PlayerData.FinishWifeSpeech();
            }
        }
    }

    void OnDone(GameObject p_man, GameObject p_wife, GameObject p_mistress) {
        goConfetti.SetActive(true);
        AnimatorController wifeController = p_wife.GetComponentInChildren<AnimatorController>();
        AnimatorController mistressController = p_mistress.GetComponentInChildren<AnimatorController>();

        wifeController.transform.LookAt(wifePosition);
        wifeController.PlayWalk();
        wifeController.GetComponent<InteractionIKHandler>().DisableInterActionsRightHand();
        wifeController.GetComponent<InteractionIKHandler>().DisableInterActionsLeftHand();
        LeanTween.move(wifeController.gameObject, wifePosition, 3f).setOnComplete(() => {
            wifeController.PlayIdle(true);
            wifeController.transform.LookAt(p_man.transform);
        });

        mistressController.PlayWalk();
        mistressController.transform.LookAt(mistressPosition);
        mistressController.GetComponent<InteractionIKHandler>().DisableInterActionsRightHand();
        mistressController.GetComponent<InteractionIKHandler>().DisableInterActionsLeftHand();
        LeanTween.move(mistressController.gameObject, mistressPosition, 3f).setOnComplete(() => {
            mistressController.PlayIdle(true);
            mistressController.transform.LookAt(p_man.transform);
        });

        if (PlayerData.CurrentStage == 1) {
            goBankTutorial.SetActive(true);
        }
    }

    void ProcessWithdrawAndDeposit() {
        if (PlayerData.CurrentStage <= 0) {
            goWithdraw.SetActive(false);
            goDeposit.SetActive(false);
        }
    }

    [ContextMenu("Set Wife Items Prices")]
    public void SetWifePrices() {
        roomFurnitures.Clear();
        roomFurnitures = furnitureParent.GetComponentsInChildren<UnlockableFurniture>().ToList();
        furniturePrices.Shuffle();
        for (int x = 0; x < roomFurnitures.Count; ++x) {
            roomFurnitures[x].price = furniturePrices[x];
        }
    }
}