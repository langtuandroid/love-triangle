using System.Collections;
using System;
using UnityEngine;

public class Gate : MonoBehaviour {
    public enum GateAnimationEvent { MAKE_LOVE, PARTY, MOVIES, VACATION, BABY, DINNER, DATE, JEWELRY, FIGHT, BURPING, CAMPING, COOKING,
        DRUNK, FARTING, KISSING, MASSAGE, MOTHER_IN_LAW, PLAYING_GAMES, PRANK, ROAD_TRIP, SHOPPING, SHOWER, CAR_MODE }
    
    public GateAnimationEvent animationEvent;
    public bool isLastGate;
    public float bonusPoints;
    public float reductionPoints;
    public bool hideOnTouch;
	private void Start() {
        if (bonusPoints > 0) {
            bonusPoints = .3f;
        }
        else if (bonusPoints < 0) {
            bonusPoints = -.3f;
        }
	}

	[ContextMenu("Align Correctly")]
    public void AlignCorrectly() {
        if (transform.localPosition.x < 0) {
            transform.localPosition = new Vector3(-3f, transform.localPosition.y, transform.localPosition.z);
        }
        else {
            transform.localPosition = new Vector3(3f, transform.localPosition.y, transform.localPosition.z);
        }
    }

    public void ProcessDisplay() {
        if (hideOnTouch) {
            VFXDisplayer.DisplayVFX( LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_LARGE, transform.position, 1f);
            Destroy(this.gameObject);
        }
    }
}
