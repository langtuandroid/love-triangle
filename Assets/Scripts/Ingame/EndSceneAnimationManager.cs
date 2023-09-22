using System.Collections.Generic;
using System;
using UnityEngine;

public class EndSceneAnimationManager : SceneAnimation {

    public List<EndingAnimation> animations = new List<EndingAnimation>();
    public static Action onAnimationDone; 
    public float animEventDelay = 0.5f;

    public void PlayBothInLove(GameObject p_wife, GameObject p_mistress){
        Debug.LogError("PlayBothInLove");
        Invoke("CallAnimationEvent", animEventDelay);
        animations[1].gameObject.SetActive(true);
        ParentUnderGuy(animations[1].transform);
        HideCharacters();
        HideUIs();
    }
    public void PlayOneInLoveOneLikes(GameObject p_womanInLove, GameObject p_womanInLikes, bool p_swapModels = false){
        Debug.LogError("PlayOneInLoveOneLikes");
        Invoke("CallAnimationEvent", animEventDelay);
        animations[2].gameObject.SetActive(true);
        if(p_swapModels){
            animations[2].ShowNewAssets();
        }
        ParentUnderGuy(animations[2].transform);
        HideCharacters();
        HideUIs();
    }

    public void PlayBothCareless(GameObject p_wife, GameObject p_mistress){
        Debug.LogError("PlayBothCareless");
        Invoke("CallAnimationEvent", animEventDelay);
        animations[0].gameObject.SetActive(true);
        ParentUnderGuy(animations[0].transform);
        HideCharacters();
        HideUIs();
    }

    public void PlayBothWOmanInLikes(GameObject p_wife, GameObject p_mistress){
        Debug.LogError("PlayBothWOmanInLikes");
        Invoke("CallAnimationEvent", animEventDelay);
        animations[3].gameObject.SetActive(true);
        ParentUnderGuy(animations[3].transform);
        HideCharacters();
        HideUIs();
    }

    public void PlayOneInLikesOneCareless(GameObject p_womanInLikes, GameObject p_womanICareless, bool p_swapModels = false){
        Debug.LogError("PlayOneInLikesOneCareless");
        Invoke("CallAnimationEvent", animEventDelay);
        animations[4].gameObject.SetActive(true);
        if(p_swapModels){
            animations[4].ShowNewAssets();
        }
        ParentUnderGuy(animations[4].transform);
        HideCharacters();
        HideUIs();
    }

    void ParentUnderGuy(Transform p_transform){
        p_transform.SetParent(goGuy.transform);
        p_transform.localPosition = new Vector3(0f, -4.31f, 0f);
        VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_LARGE, p_transform.position, 1.5f);
    }

    void CallAnimationEvent(){
        onAnimationDone?.Invoke();
    }
}