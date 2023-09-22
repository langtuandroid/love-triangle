using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoveTriangle;
public class GateAnimation : MonoBehaviour {

    public ModelTransformer guy;
    public ModelTransformer wife;
    public ModelTransformer mistress;
    private Transform m_selectedLady;
    public Vector3 specificProgressBarScale;
    public Transform uiPlaceHolder;

    public bool fixDress;

    public GameObject goWifeFixDress;
    public GameObject goMistressFixDress;

    public bool showBothGirls;
    public bool requireExitGates;
    public Transform InitializeElements(LadiesProgressBar.LadyType p_currentHeld, PlayerData.StatusLevel p_guyStatusLevel){
        if (guy != null || fixDress) {
            if (guy != null) {
                guy.SwapModel((int)p_guyStatusLevel);
            }
            if (fixDress) {
                if (p_currentHeld == LadiesProgressBar.LadyType.MISTRESS) {
                    goMistressFixDress.gameObject.SetActive(true);
                    m_selectedLady = goMistressFixDress.transform;
                }
                else if (p_currentHeld == LadiesProgressBar.LadyType.WIFE) {
                    goWifeFixDress.gameObject.SetActive(true);
                    m_selectedLady = goWifeFixDress.transform;
                }
            }
            else {
                wife.gameObject.SetActive(false);
                mistress.gameObject.SetActive(false);
                if (showBothGirls) {
                    mistress.gameObject.SetActive(true);
                    wife.gameObject.SetActive(true);
                    mistress.SwapModel((int)PlayerData.MistressSatisfactionLevel);
                    wife.SwapModel((int)PlayerData.WifeSatisfactionLevel);
                    return guy.transform.parent;
                }
                else {
                    if (p_currentHeld == LadiesProgressBar.LadyType.MISTRESS) {
                        mistress.gameObject.SetActive(true);
                        m_selectedLady = mistress.transform;
                        mistress.SwapModel((int)PlayerData.MistressSatisfactionLevel);
                    }
                    else if (p_currentHeld == LadiesProgressBar.LadyType.WIFE) {
                        wife.gameObject.SetActive(true);
                        m_selectedLady = wife.transform;
                        wife.SwapModel((int)PlayerData.WifeSatisfactionLevel);
                    }
                }
            }
            
            if (m_selectedLady != null) {
                return m_selectedLady;
            }
            return guy.transform.parent;
        }
        return uiPlaceHolder.transform;
    }

    [ContextMenu("Get Model Transformers")]
    public void GetModelTransformers() {
        guy = transform.Find("Characters").transform.Find("Char_1").GetComponentInChildren<ModelTransformer>();
        wife = transform.Find("Characters").transform.Find("Char_2").transform.Find("Wife_Gate").GetComponentInChildren<ModelTransformer>();
        mistress = transform.Find("Characters").transform.Find("Char_2").transform.Find("Mistress_Gate").GetComponentInChildren<ModelTransformer>();
        wife.gameObject.SetActive(false);
        mistress.gameObject.SetActive(false);
    }
}
