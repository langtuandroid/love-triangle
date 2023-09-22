using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : MonoBehaviour {
    public static Action OnRoomUnlocked;
    public static Action<Room> OnRoomWaitingForUnlock;

    public List<Transform> furnituresParent = new List<Transform>();
    public List<Vector3> furnituresParentScale = new List<Vector3>();


    public List<UnlockableFurniture> furnitures = new List<UnlockableFurniture>();
    public Room nextLinkedRoom;
    public GameObject goDoor;
    public GameObject goFloor;

    public bool IsWaitingForUnlock { set; get; }
    public bool IsAlreadyOpened;
    private void OnEnable() {
        UnlockableItem.onUnlock += CheckNextRoomForunlock;
    }

    private void OnDisable() {
        UnlockableItem.onUnlock -= CheckNextRoomForunlock;
    }

    private void Start() {
        Invoke("Initialize", 0.5f);
    }

    void Initialize() {
        if (IsRoomFullyUnlocked()) {
            nextLinkedRoom.UnlockRoom();
            IsAlreadyOpened = true;
            UnlockableItem.onUnlock -= CheckNextRoomForunlock;
        }
    }

    [ContextMenu("Get All Furnitures")]
    public void GetAllFurnitures() {
        furnitures.Clear();
        furnitures = GetComponentsInChildren<UnlockableFurniture>().ToList();
    }

    void CheckNextUnlock() {
        if (nextLinkedRoom != null && IsRoomFullyUnlocked()) {
            if (!nextLinkedRoom.IsWaitingForUnlock) {
                UnlockableItem.onUnlock -= CheckNextRoomForunlock;
                nextLinkedRoom.MakeDoorReadyForUnlock();
            }
        }
    }
    void CheckNextRoomForunlock(Transform p_unlockedFurniture) {
        CheckNextUnlock();
    }

    public bool IsRoomFullyUnlocked() {
        bool isUnlocked = true;
        for (int x = 0; x < furnitures.Count; ++x) {
            if (!furnitures[x].IsUnlocked) {
                isUnlocked = false;
            }
        }
        return isUnlocked;
    }

    public void UnlockThroughCollider() {
        if (IsWaitingForUnlock) {
            UnlockRoom();
            OnRoomUnlocked?.Invoke();
        }
    }

    void UnlockRoom() {
        
        VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_WHITE, goDoor.transform.position, 1f);
        goDoor.GetComponentInChildren<Animator>().enabled = true;
        Invoke("HidePadLock", 1.75f);
        IsAlreadyOpened = true;
        IsWaitingForUnlock = false;
    }

    void HidePadLock() {
        goDoor.gameObject.SetActive(false);
        for (int x = 0; x < furnituresParent.Count; ++x) {
            Vector3 targetScale = furnituresParentScale[x];
            furnituresParent[x].transform.localScale = new Vector3(furnituresParent[x].transform.localScale.x, 0f, furnituresParent[x].transform.localScale.z);
            LeanTween.scale(furnituresParent[x].gameObject, targetScale, 0.25f).setEaseInOutBounce();
            VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_WHITE, furnituresParent[x].transform.position, 1f);
            furnituresParent[x].gameObject.SetActive(true);
        }
        if (goFloor != null) {
            goFloor.SetActive(false);
        }
        
    }
    public void MakeDoorReadyForUnlock() {
        if (IsAlreadyOpened) {
            return;
        }
        Vector3 targetScale = goDoor.transform.localScale;
        targetScale += new Vector3(0.25f, 0.25f, 0.25f);
        //LeanTween.scale(goDoor, targetScale, 0.25f).setLoopPingPong(10000);
        
        IsWaitingForUnlock = true;
        OnRoomWaitingForUnlock?.Invoke(this);
    }

    [ContextMenu("Get Unlock Parts")]
    public void GetUnlockableParts() {
        furnituresParent.Clear();
        furnituresParentScale.Clear();
        foreach (Transform t in transform) {
            if (t.gameObject.name == "Floor") {
                furnituresParent.Add(t);
                furnituresParentScale.Add(t.localScale);
                t.transform.localScale = new Vector3(1f, 0f, 1f);
            }
            foreach (Transform t2 in t) {
                if (t2.gameObject.name == "Floor") {
                    furnituresParent.Add(t2);
                    furnituresParentScale.Add(t2.localScale);
                    t2.transform.localScale = new Vector3(1f, 0f, 1f);
                }
            }
        }

        foreach (Transform t in transform) {
            if (t.gameObject.name == "Furnitures") {
                furnituresParent.Add(t);
                furnituresParentScale.Add(t.localScale);
                t.transform.localScale = new Vector3(1f, 0f, 1f);
            }
            foreach (Transform t2 in t) {
                if (t2.gameObject.name == "Furnitures") {
                    furnituresParent.Add(t2);
                    furnituresParentScale.Add(t2.localScale);
                    t2.transform.localScale = new Vector3(1f, 0f, 1f);
                }
            }
        }
    }

    [ContextMenu("Get Door")]
    public void GetDoorParts() {
        goDoor = transform.Find("RoomLock").gameObject;
    }
}
