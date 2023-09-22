using System.Collections;
using System;
using UnityEngine;

public class RoomUnlockerCollider : MonoBehaviour {
	
    public Room parentRoom;

	private void OnTriggerEnter(Collider other) {
		if (other.GetComponent<CarCollider>() != null) {
			Debug.LogError("Collide with me: " + transform.parent.name);
			parentRoom.UnlockThroughCollider();
		}
	}
}
