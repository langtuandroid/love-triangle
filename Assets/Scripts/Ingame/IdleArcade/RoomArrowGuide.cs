using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomArrowGuide : MonoBehaviour {
    private Transform m_targetRoom;
    public GameObject goArrowObject;

	private void OnEnable() {
        Room.OnRoomWaitingForUnlock += OnRoomReadyForUnlocking;
        Room.OnRoomUnlocked += OnRoomDoneUnlocking;
    }

	private void OnDisable() {
        Room.OnRoomWaitingForUnlock -= OnRoomReadyForUnlocking;
        Room.OnRoomUnlocked -= OnRoomDoneUnlocking;
    }

	void OnRoomReadyForUnlocking(Room p_room) {
        m_targetRoom = p_room.goDoor.transform;
        goArrowObject.SetActive(true);
    }

    void OnRoomDoneUnlocking() {
        m_targetRoom = null;
        goArrowObject.SetActive(false);
    }

    private void Update() {
        if (m_targetRoom != null && goArrowObject.activeSelf) {
            transform.LookAt(m_targetRoom);

            // Euler angles are easier to deal with. You could use Quaternions here also
            // C# requires you to set the entire rotation variable. You can't set the individual x and z (UnityScript can), so you make a temp Vec3 and set it back
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.z = 0;

            // Set the altered rotation back
            transform.rotation = Quaternion.Euler(eulerAngles);
        }
	}
}
