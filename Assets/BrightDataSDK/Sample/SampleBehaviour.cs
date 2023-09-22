using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Brdsdk;

namespace Brdsdk.Demo
{
    /// <summary>
    /// Sample showing a possible SDK integration.
    /// </summary>
    public class SampleBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            /// Initialize the SDK
			string language = null;
            bool skip_consent = false;
            Debug.Log("Auth SDK Status before init: " + BrdsdkBridge.authorize_device());

            ConsentBackgroundImage backgroundImage = new ConsentBackgroundImage();
            // you should add these images to app's assets in xcode
            backgroundImage.portraitImage = "portrait";
            // or use a path to a file
            backgroundImage.landscapeImage = Path.Combine(Application.streamingAssetsPath, "landscape.jpg");
            backgroundImage.scaleMode = ConsentBackgroundImage.ScaleMode.ScaleAspectFill;

            ConsentActionInfo optIn = new ConsentActionInfo();
            optIn.backgroundImage = Path.Combine(Application.streamingAssetsPath, "opt_in_bg.png");
            optIn.textImage = Path.Combine(Application.streamingAssetsPath, "opt_in_text.png");
            ConsentActionInfo optOut = new ConsentActionInfo();
            optOut.backgroundImage = Path.Combine(Application.streamingAssetsPath, "opt_out_bg.png");
            optOut.textImage = Path.Combine(Application.streamingAssetsPath, "opt_out_text.png");

            BrdsdkBridge.init("To support this app", "I Agree", "I disagree", "Just a test text of opt-out instructions.", choiceChanged, skip_consent, language, 0x000000, 0xFFFFFF, 0x0000FF, backgroundImage, optIn, optOut);
            Debug.Log("SDK Status: " + BrdsdkBridge.get_choice());
            Debug.Log("SDK UUID: " + BrdsdkBridge.get_uuid());
            Debug.Log("Auth SDK Status: " + BrdsdkBridge.authorize_device());
        }

        /// <summary>
        /// The method is called when user's choice is changed.
        /// </summary>
        /// <param name="choice">Value representing the user's choice:
        /// - BrdsdkBridge.CHOICE_NONE - the consent screen is not yet shown;
        /// - BrdsdkBridge.CHOICE_AGREED - user accepted the consent screen;
        /// - BrdsdkBridge.CHOICE_DISAGREED - user declined the consent screen.
        /// </param>
        void choiceChanged(int choice)
        {
            Debug.Log("SDK Status: " + BrdsdkBridge.get_choice());
            if (choice == BrdsdkBridge.CHOICE_AGREED)
            {
                Debug.Log("Skip ads (SDK is enabled)");
            }
            else
            {
                Debug.Log("TODO Initialize ads (SDK is disabled)");
            }
        }
    }
}
