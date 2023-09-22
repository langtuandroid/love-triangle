using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableFurniture : UnlockableItem {
    public List<GameObject> goFurnitures = new List<GameObject>();
    private Canvas m_canvas;

	private void Awake() {
        m_canvas = GetComponentInChildren<Canvas>();
        m_canvas.gameObject.SetActive(false);
	}

	private void OnEnable() {
        LoveTriangle.Gentleman.onDone += OnDone;
	}

    private void OnDisable() {
        LoveTriangle.Gentleman.onDone -= OnDone;
    }

    public override void UnlockItem(bool p_playSFX = true) {
        base.UnlockItem(p_playSFX);
        if (p_playSFX) {
            SFXPlayer.Instance.PlaySFX(SFXPlayer.SFX_TYPE.Item_Place);
            if (PlayerData.EnteredHouse == LadiesProgressBar.LadyType.WIFE) {
                SFXPlayer.Instance.PlayRandomWifeUpgradeSFX();
            }
            else {
                SFXPlayer.Instance.PlayRandomMistressUpgradeSFX();
            }
            SFXPlayer.Instance.PlaySFX(SFXPlayer.SFX_TYPE.Cash_Register, 2);
            onUnlock?.Invoke(m_canvas.transform);
        }
        
        goFurnitures.ForEach((eachObject) => {
            Vector3 targetScale = eachObject.transform.localScale;
            eachObject.transform.localScale = new Vector3(eachObject.transform.localScale.x, 0f, eachObject.transform.localScale.z);
            LeanTween.scale(eachObject, targetScale, 0.25f).setEaseInOutBounce();
            if (p_playSFX) {
                VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_WHITE, eachObject.transform.position, 1f);
            }
            eachObject.SetActive(true);
        });
    }

    void OnDone(GameObject goGuy, GameObject goWife, GameObject goMistress) {
        m_canvas.gameObject.SetActive(true);
    }

    [ContextMenu("GetAllItems")]
    public void GetAllUnlocks() {
        goFurnitures.Clear();
        foreach (Transform t in transform.parent) {
            if (t.gameObject.GetComponent<UnlockableFurniture>() == null) {
                goFurnitures.Add(t.gameObject);
            }   
        }
    }

    [ContextMenu("ShowAllItems")]
    public void ShowAllUnlocks() {
        goFurnitures.ForEach((eachUnlock) => eachUnlock.SetActive(true));
    }

    [ContextMenu("HideAllItems")]
    public void HideAllUnlocks() {
        goFurnitures.ForEach((eachUnlock) => eachUnlock.SetActive(false));
    }
}
