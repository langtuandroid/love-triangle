using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoveTriangle;
public static class VFXDisplayer {
    public static void DisplayVFX(ObjectPoolLibraryCommon.PoolType p_type, Vector3 p_SpawnPos, float yOffset = 0f) {
        GameObject go = ObjectPoolLibraryCommon.instance.GetObjectPooler(p_type).GiveGameObject();
        p_SpawnPos.y += yOffset;
        go.transform.position = p_SpawnPos;
    }

    public static GameObject DisplayVFX(ObjectPoolLibraryCommon.PoolType p_type, Vector3 p_SpawnPos, Transform p_parent, float yOffset = 0f, float xOffset = 0f) {
        GameObject go = ObjectPoolLibraryCommon.instance.GetObjectPooler(p_type).GiveGameObject();
        go.transform.parent = p_parent;
        p_SpawnPos.y += yOffset;
        p_SpawnPos.x += xOffset;
        go.transform.position = p_SpawnPos;
        
        return go;
    }

    public static void DisplayMovingVFXWOrldToUI(ObjectPoolLibraryCommon.PoolType p_type, Vector3 p_SpawnPos, Vector3 p_targetPos, float yOffset = 0f) {
        GameObject go = ObjectPoolLibraryCommon.instance.GetObjectPooler(p_type).GiveGameObject();
        go.transform.SetParent(ObjectPoolLibraryCommon.instance.uiCanvas.transform, true);
        go.transform.localPosition = Vector3.zero;
        p_SpawnPos.y += yOffset;
        //p_SpawnPos.z = 10;
        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform>().anchoredPosition = ObjectPoolLibraryCommon.instance.cam.WorldToViewportPoint(p_SpawnPos);
        //p_targetPos = ObjectPoolLibraryCommon.instance.cam.ScreenToViewportPoint(p_targetPos);
        LeanTween.move(go, p_targetPos, 0.35f).setEaseInBounce().setOnComplete(() => SFXPlayer.Instance.PlaySFX(SFXPlayer.SFX_TYPE.Get_Cash));
    }

    public static void DisplayMovingVFXUIToWOrld(ObjectPoolLibraryCommon.PoolType p_type, Vector3 p_SpawnPos, Vector3 p_targetPos, float yOffset = 0f) {
        GameObject go = ObjectPoolLibraryCommon.instance.GetObjectPooler(p_type).GiveGameObject();
        go.transform.SetParent(ObjectPoolLibraryCommon.instance.uiCanvas.transform, true);
        go.transform.localPosition = Vector3.zero;
        p_SpawnPos.y += yOffset;
        //p_SpawnPos.z = 10;
        go.transform.localScale = Vector3.one;
        go.GetComponent<RectTransform>().position = p_SpawnPos;
        p_targetPos = ObjectPoolLibraryCommon.instance.cam.WorldToViewportPoint(p_targetPos);
        p_targetPos.z = 10f;
        p_targetPos.y -= 50f;
        //p_targetPos = ObjectPoolLibraryCommon.instance.cam.ScreenToViewportPoint(p_targetPos);
        LeanTween.move(go.GetComponent<RectTransform>(), p_targetPos, 0.35f).setEaseInBounce().setOnComplete(() => SFXPlayer.Instance.PlaySFX(SFXPlayer.SFX_TYPE.Get_Cash));
    }

}