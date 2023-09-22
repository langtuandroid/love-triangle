using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingAnimation : MonoBehaviour {
    public GameObject wifeDefault;
    public GameObject mistressDefault;
    public GameObject wifeNew;
    public GameObject mistressNew;

    public void ShowNewAssets(){
        wifeDefault.SetActive(false);
        mistressDefault.SetActive(false);
        wifeNew.SetActive(true);
        mistressNew.SetActive(true);
    }
}
