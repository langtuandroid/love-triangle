/* Swipe manager by Aaron Aranas
 * 
 * this script can be used or extend to add your own swipe/gesture event
 * 
 * just listen to the events ex. onSwipeLeft
 * 
 * make sure to set threshold on inspector
 * 
 * */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace LoveTriangle {

	public class GestureController : MonoBehaviour {

		/* +++++++++++++++ available events to listen +++++++++++++++ */
		/* TODO: on your code you can listen to these events by doing the following
		*
		* GestureController.onSwipeLeft += yourFunctionToListenWithSameSignature
		* GestureController.onSwipeRight += yourFunctionToListenWithSameSignature
		* 
		*/
		public delegate void GestureEvent();
		public delegate void GestureEventWithParam(float p_param);
		public static event GestureEventWithParam onSwipeLeft;
		public static event GestureEventWithParam onSwipeRight;
		public static event GestureEvent onSwipeUp;
		public static event GestureEvent onSwipeDown;
		public static event GestureEvent onMouseUp;
		public static event GestureEvent onMouseUp2;
		public static event GestureEventWithParam onUpdateDelta;
		/* +++++++++++++++ available events to listen +++++++++++++++ */

		Vector3 inPos;// initial pressed position
		Vector3 outPos;// (released pressed position)
		Vector3 clickPos;
		/* IMPORTANT MAKE SURE TO SET THIS ON INSPECTOR */
		public float threshold;// threshold for swipe length to determine if action is done.

		// Update is called once per frame
		bool m_onUp;

		float m_timer = 0f;
		void Update() {
			if (EventSystem.current.IsPointerOverGameObject() || IsPointerOverUIObject()) {
				inPos = Input.mousePosition;
				outPos = Input.mousePosition;
				return;
			}
			if (Input.GetMouseButtonDown(0)) {
				clickPos = inPos = Input.mousePosition;
			}
			
			

			if (Vector3.Distance(inPos, Input.mousePosition) < threshold && !m_onUp) {
				if (onMouseUp2 != null) {
					onMouseUp2();
				}
				m_onUp = true;
				//Debug.LogError(clickPos + " -- " + inPos + " -- " + outPos);
			}

			if (Input.GetMouseButton(0) && Vector3.Distance(inPos, Input.mousePosition) > threshold) {
				m_timer += Time.deltaTime;
				if (m_timer > 0.35f) {
					m_timer = 0f;
					if (onUpdateDelta != null) {
						onUpdateDelta(inPos.x - outPos.x);
					}
				}
				
			} else {
				if (Input.GetMouseButtonUp(0)) {
					
					outPos = Input.mousePosition;
					ProcessSwipe();
					if (onMouseUp != null) {
						onMouseUp();
					}
					if (onMouseUp2 != null) {
						onMouseUp2();
					}
					
				}
			}
			if (Input.GetMouseButtonUp(0)) {
				m_timer = 0f;
				if (onUpdateDelta != null) {
					onUpdateDelta(inPos.x - outPos.x);
				}
			}
		}

		private bool IsPointerOverUIObject() {
			PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
			eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
			return results.Count > 0;
		}

		/* 
		 * Check delata movement (x and y axis) to determine direction of swipe.
		 * then fire event for swipe directions or gesture detected
		 * */
		void ProcessSwipe() {
			float deltaX = inPos.x - outPos.x;
			float deltaY = inPos.y - outPos.y;

			if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) {
				if (deltaX < -threshold) {
					Debug.Log("SWIPE RIGHT");
					if (onSwipeRight != null) {
						onSwipeRight(deltaX); //fire the onSwipeRight event, so all the listeners will be notify/called
					}
				} else if (deltaX > threshold) {
					Debug.Log("SWIPE LEFT");
					if (onSwipeLeft != null) {
						onSwipeLeft(deltaX); //fire the onSwipeLeft event, so all the listeners will be notify/called
					}
				}
			} else {
				if (deltaY < -threshold) {
					Debug.Log("SWIPE UP");
					if (onSwipeUp != null) {
						onSwipeUp(); //fire the onSwipeUp event, so all the listeners will be notify/called
					}
				} else if (deltaY > threshold) {
					Debug.Log("SWIPE DOWN");
					if (onSwipeDown != null) {
						onSwipeDown(); //fire the onSwipeDown event, so all the listeners will be notify/called
					}
				}
			}
		}
	}
}