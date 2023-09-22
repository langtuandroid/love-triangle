using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class GateSceneAnimationManager : SceneAnimation {
    public List<GateAnimation> animations = new List<GateAnimation>();
    public LeanClamper leanClamper;

    void OnEnable(){
        CarCollider.OnCollideWithGate += ShowAnimation;
        ExitGate.onForcedGateAnimExit += OnExitAnimationByGate;
    }
    
    void OnDisable(){
        CarCollider.OnCollideWithGate -= ShowAnimation;
        ExitGate.onForcedGateAnimExit -= OnExitAnimationByGate;
    }

    bool isBothGirls = false;
    public override void ShowAnimation(Gate p_gate){
        //VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_LARGE, goGuy.transform.position, -2.35f);
        if (animations[(int)p_gate.animationEvent].requireExitGates) {
            isBothGirls = true;
        }
        else {
            isBothGirls = false;
        }
        HideAnimations();
        HideCharacters();
        StartCoroutine(PlayAnimationLate(() => animations[(int)p_gate.animationEvent].gameObject.SetActive(true)));
        Transform lady = animations[(int)p_gate.animationEvent].InitializeElements(m_gentleMan.CurrentHeld, PlayerData.CurrentStatusLevel);
        if(animations[(int)p_gate.animationEvent].uiPlaceHolder != null){
            lady = animations[(int)p_gate.animationEvent].uiPlaceHolder;
        }
        onAnimationStart?.Invoke(lady, animations[(int)p_gate.animationEvent].specificProgressBarScale);
        if (animations[(int)p_gate.animationEvent].requireExitGates) {
            isBothGirls = true;
        }
        else {
            isBothGirls = false;
            Invoke("HideAnimations", 3.5f - 1.15f);
            StartCoroutine(ShowCharactersTrigger(3.2f - 1.15f));
        }
        
        
        //leanClamper.minXPos += 0.5f;
        //leanClamper.maxXPos -= 0.5f;
    }

    IEnumerator PlayAnimationLate(Action p_action = null){
        yield return 0;
        p_action?.Invoke();
    }

    public override void  HideAnimations(){
        animations.ForEach((eachAnimation) => eachAnimation.gameObject.SetActive(false));
    }

    IEnumerator ShowCharactersTrigger(float p_timer){
        yield return new WaitForSeconds(p_timer);
        VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_LARGE, goGuy.transform.position, goGuy.transform, 1f);
        ladiesProressBars.ForEach((eachProgress) => {
            if (m_gentleMan.CurrentHeld == eachProgress.ladyType) {
                VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_LARGE, eachProgress.transform.position, goGuy.transform, -1f);
            }
        });
        yield return new WaitForSeconds(0.2f);
        ShowCharacters();
    }

    public override void ShowCharacters(){
        base.ShowCharacters();
    }

    public override void HideCharacters(){
        m_gentleMan.HideOnLayer();
        if (isBothGirls) {
            ladiesProressBars.ForEach((eachProgress) => {
                foreach (Transform child in eachProgress.ladyObject.transform.GetChild(0)) {
                    child.gameObject.layer = LayerMask.NameToLayer("Invisible");
                }
            });
        }
        else {
            ladiesProressBars.ForEach((eachProgress) => {
                if (m_gentleMan.CurrentHeld == eachProgress.ladyType) {
                    foreach (Transform child in eachProgress.ladyObject.transform.GetChild(0)) {
                        child.gameObject.layer = LayerMask.NameToLayer("Invisible");
                    }
                }
            });
        }
        
    }

    void OnExitAnimationByGate() {
        StartCoroutine(ShowCharactersTrigger(0f));
        HideAnimations();
    }
}