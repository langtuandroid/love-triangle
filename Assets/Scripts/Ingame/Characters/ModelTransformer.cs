using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTransformer : MonoBehaviour {
    public List<GameObject> models = new List<GameObject>();
    public List<GameObject> specialItems = new List<GameObject>();
    private int m_currentActiveIndex = 0;

    public void SwapModel(int p_index) {
        models.ForEach((eachModel) => eachModel.SetActive(false));
        specialItems.ForEach((eachItem) => {
            if (eachItem != null) {
                eachItem.SetActive(false);
            }
        });
        
        m_currentActiveIndex = p_index;
        
        models[p_index].SetActive(true);
        if (specialItems[p_index] != null) {
            specialItems[p_index].SetActive(true);
        }
        
    }
}
