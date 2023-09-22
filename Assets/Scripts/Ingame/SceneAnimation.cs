using System;
using System.Collections.Generic;
using UnityEngine;
using LoveTriangle;

public class SceneAnimation : MonoBehaviour {
    
    public static Action<Transform, Vector3> onAnimationStart;
    public static Action onAnimationEnd;
    public GameObject goGuy;
    public List<AnimatorController> ladies = new List<AnimatorController>();
    public List<LadiesProgressBar> ladiesProressBars = new List<LadiesProgressBar>();
    protected Gentleman m_gentleMan;

    void Awake(){
        m_gentleMan = GameObject.FindObjectOfType<Gentleman>();
    }
    public virtual void ShowAnimation(Gate p_gate){}
    public virtual void HideAnimations(){}

    public virtual void ShowCharacters(){
        onAnimationEnd?.Invoke();
        m_gentleMan.ShowOnLayer();
        ladiesProressBars.ForEach((eachProgress) => {
            foreach (Transform child in eachProgress.ladyObject.transform.GetChild(0)) {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        });
        
    }
    public virtual void HideCharacters(){
        m_gentleMan.HideOnLayer();
        ladiesProressBars.ForEach((eachProgress) => {
            foreach (Transform child in eachProgress.ladyObject.transform.GetChild(0)) {
                child.gameObject.layer = LayerMask.NameToLayer("Invisible");
            }
        });
    }

    public virtual void HideUIs(){
        for(int x = 0; x < ladiesProressBars.Count; ++x){
            ladiesProressBars[x].gameObject.SetActive(false);
        }
    }
}
