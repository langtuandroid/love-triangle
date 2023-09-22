using UnityEngine;

namespace LoveTriangle.UI {
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class MVCUIModel : MonoBehaviour {
		protected CanvasGroup _canvasGroup;

		public Transform parentDisplay;
		
		private void Awake() { _canvasGroup = GetComponent<CanvasGroup>(); }

		protected void SetInteractable(bool interactable) { _canvasGroup.interactable = interactable; }
	}
}