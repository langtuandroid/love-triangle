using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lady : MonoBehaviour {
    public AnimatorController animatorController;
    public InteractionIKHandler ikInteractionHandler;

    public void AnimateHoldHandsRight() {
        ikInteractionHandler.CallInteraActionsRightHand();
        animatorController.PlayWalk();
    }

    public void AnimateHoldHandsLeft() {
        ikInteractionHandler.CallInteraActionsLeftHand();
        animatorController.PlayWalk();
    }

    public void SeparateHoldHands(Vector3 p_pos, float p_timing) {
        ikInteractionHandler.DisableInterActionsRightHand();
        ikInteractionHandler.DisableInterActionsLeftHand();
        animatorController.PlayUpset();
        LeanTween.moveLocal(gameObject, p_pos, p_timing);
    }
}
