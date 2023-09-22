using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;

/// <summary>
	/// Simple GUI for quickly testing out interactions.
	/// </summary>
	[RequireComponent(typeof(InteractionSystem))]
	public class InteractionIKHandler : MonoBehaviour {

		[Tooltip("The object to interact to right hand")]
		public InteractionObject rightHandInteractionObject;
		[Tooltip("The effectors to interact with right hand")]
        public FullBodyBipedEffector[] rightHandEffectors;

        [Tooltip("The object to interact to right hand")]
		public InteractionObject leftHandInteractionObject;
		[Tooltip("The effectors to interact with right hand")]
        public FullBodyBipedEffector[] leftHandEffectors;

		private InteractionSystem interactionSystem;
		
		void Awake() {
			interactionSystem = GetComponent<InteractionSystem>();
		}

        public void CallInteraActionsRightHand(){
            foreach (FullBodyBipedEffector e in rightHandEffectors) {
					interactionSystem.StartInteraction(e, rightHandInteractionObject, true);
				}
        }

        public void DisableInterActionsRightHand(){
            foreach (FullBodyBipedEffector e in rightHandEffectors) {
					interactionSystem.StopInteraction(e);
				}
        }

        public void CallInteraActionsLeftHand(){
            foreach (FullBodyBipedEffector e in leftHandEffectors) {
					interactionSystem.StartInteraction(e, leftHandInteractionObject, true);
				}
        }

        public void DisableInterActionsLeftHand(){
            foreach (FullBodyBipedEffector e in leftHandEffectors) {
					interactionSystem.StopInteraction(e);
				}
        }
	}