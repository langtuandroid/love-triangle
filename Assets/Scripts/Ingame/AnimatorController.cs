using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour {
    private Animator m_animator;

    bool m_isWalk;
    bool m_isIdle;
    void Awake(){
        m_animator = GetComponent<Animator>();
    }
    public void PlayWalk(){
        if (!m_isWalk) {
            m_isWalk = true;
            m_isIdle = false;
            m_animator.SetTrigger("trigWalk");
        }
    }
    public void PlayIdle(bool p_force = false) {
        if (p_force) {
            m_isIdle = false;
        }
        if (!m_isIdle) {
            m_isIdle = true;
            m_isWalk = false;
            m_animator.SetTrigger("trigIdle");
        }
    }

    public void PlayUpset(){
        m_isWalk = false;
        //VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SAD_EMOJI, transform.position, transform, 3f);
        m_animator.SetTrigger("trigUpset");
    }

    public void PlayHappy(){
        //VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.HAPPY_EMOJI, transform.position, transform, 3f);
        m_animator.SetTrigger("trigHappy");
    }

    
}
