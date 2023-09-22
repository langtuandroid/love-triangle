using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using AOT;

#if UNITY_EDITOR_OSX
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Extensions;
#endif

namespace Brdsdk
{
    
    /// <summary>
    /// Meta information of images for sdk consent action.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class ConsentActionInfo {
        /// <summary>
		/// Name in XCAssets of main bundle or path for button's background image.
        /// If null button's background color will be used
		/// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string backgroundImage;
        /// <summary>
		/// Name in XCAssets of main bundle or path for button's text image. 
        /// If null, button's title will be used.
		/// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string textImage;
    };
    
    /// <summary>
    /// Meta information about a consent screen background image.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class ConsentBackgroundImage {
        /// <summary>
        /// Constants that define how a view’s content fills the available 
        /// space.
        /// </summary>
        public enum ScaleMode: Int32 {
            /// <summary>
            /// The option to scale the content to fit the size of itself by 
            /// changing the aspect ratio of the content if necessary.
            /// </summary>
            ScaleToFill = 0,
            /// <summary>
            /// The option to scale the content to fit the size of the view by 
            /// maintaining the aspect ratio. Any remaining area of the view’s 
            /// bounds is transparent.
            /// </summary>
            ScaleAspectFit = 1,
            /// <summary>
            /// The option to scale the content to fill the size of the view. 
            /// Some portion of the content may be clipped to fill the view’s 
            /// bounds.
            /// </summary>
            ScaleAspectFill = 2
        };
        /// <summary>
		/// Name in XCAssets of main bundle or path for portrait image.
		/// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string portraitImage;
        /// <summary>
		/// Name in XCAssets of main bundle or path for landscape image. 
        /// If null, portrait will be used.
		/// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string landscapeImage;
        /// <summary>
		/// Scale mode for image view.
		/// </summary>
        public ScaleMode scaleMode;
    };
    // 1. Call `BrdsdkBridge.init(...)` from `Start()` method of any game
    // object handler, e.g. instead of direct ads initialization.
    // 2. Generate Xcode project.
    // 3. In Settings screen (you should implement it) call `BrdsdkBridge.opt_out()`
    // when user turns off SDK, and call `BrdsdkBridge.show_consent()`
    // if user tries to turn back on.
    public class BrdsdkBridge
    {
        /// <summary>
		/// The consent screen is not yet shown.
		/// </summary>
        public static int CHOICE_NONE = 0;
        /// <summary>
		/// User accepted the consent screen.
		/// </summary>
        public static int CHOICE_AGREED = 1;
        /// <summary>
		/// User declined the consent screen.
		/// </summary>
        public static int CHOICE_DISAGREED = 2;

        /// <summary>
        /// Result of checks of availability of using SDK
        /// </summary>
        public enum AuthorizationStatus: int {
            /// <summary>
            /// Indicates that you are able to use SDK in both cases: either with SDK's consent screen or with your own one
            /// </summary>
            Authorized = 0,
            /// <summary>
            /// Indicates that SDK is not initialized
            /// </summary>
            SDKNotInitialized = -1,
            /// <summary>
            /// Indicates that parent control is enabled and you don't have to use SDK
            /// </summary>
            ParentControlEnabled = -2,
            /// <summary>
            /// Indicates that you can use only SDK's consent screen, what means you don't allowed to call `silent_opt_in`
            /// </summary>
            OnlySDKConsent = -3,
            /// <summary>
            /// Indicates that you call authorization method on unsupported platform
            /// </summary>
            PlatformNotSupported = -999999
        };

        /// <summary>
        /// The choice delegate method. Used to receive the user's choice.
        /// </summary>
        /// <param name="choice">Value representing the choice:
        /// - BrdsdkBridge.CHOICE_NONE - the consent screen is not yet shown;
        /// - BrdsdkBridge.CHOICE_AGREED - user accepted the consent screen;
        /// - BrdsdkBridge.CHOICE_DISAGREED - user declined the consent screen.
        /// </param>
        public delegate void ChoiceCallback(int choice);

        /// <summary>
        /// Stores the reference to ChoiceCallback method used to notify about
        /// changes of the user's choice.
        /// </summary>
        private static ChoiceCallback callback;

#if UNITY_IOS || UNITY_TVOS
        [DllImport("__Internal")]
        private static extern void brdsdk_set_delegate(delegate_message callback);
        [DllImport("__Internal")]
        private static extern void brdsdk_init(
            [MarshalAs(UnmanagedType.LPWStr)]string benefit,
            int benefit_len,
            [MarshalAs(UnmanagedType.LPWStr)]string agree_btn,
            int agree_btn_len,
            [MarshalAs(UnmanagedType.LPWStr)]string disagree_btn,
            int disagree_btn_len,
            [MarshalAs(UnmanagedType.LPWStr)]string opt_out_instructions,
            bool skip_consent,
            [MarshalAs(UnmanagedType.LPWStr)]string language,
            int language_len,
            int text_color,
            int background_color,
            int button_color,
            ConsentBackgroundImage background_image,
            ConsentActionInfo opt_in_info,
            ConsentActionInfo opt_out_info
        );

        [DllImport("__Internal")]
        private static extern bool brdsdk_silent_opt_in();

        [DllImport("__Internal")]
        private static extern void brdsdk_opt_out();

        [DllImport("__Internal")]
        private static extern bool brdsdk_show_consent(
            [MarshalAs(UnmanagedType.LPWStr)]string benefit,
            int benefit_len,
            [MarshalAs(UnmanagedType.LPWStr)]string agree_btn,
            int agree_btn_len,
            [MarshalAs(UnmanagedType.LPWStr)]string disagree_btn,
            int disagree_btn_len,
            string language = null
        );
        [DllImport("__Internal")]
        private static extern int brdsdk_get_choice();
        [DllImport("__Internal")]
        private static extern bool brdsdk_consent_shown();
        [DllImport("__Internal")]
        private static extern IntPtr brdsdk_get_uuid();
        [DllImport("__Internal")]
        private static extern AuthorizationStatus brdsdk_authorize_device();

        public delegate void delegate_message(int choice);

        /// <summary>
	    /// The method called from native SDK code when user's choice
	    /// is changed, or right after preceding `init()` method call.
	    /// </summary>
	    /// <param name="choice">Value representing the choice:
	    /// - 0 (BrdsdkBridge.CHOICE_NONE) - the consent screen is not yet shown;
	    /// - 1 (BrdsdkBridge.CHOICE_AGREED) - user accepted the consent screen;
	    /// - 2 (BrdsdkBridge.CHOICE_DISAGREED) - user declined the consent screen.</param>
        [MonoPInvokeCallback(typeof(BrdsdkBridge.delegate_message))]
        public static void on_choice_change(int choice) {
            Debug.Log("on_choice_change: choice=" + choice);
            if (BrdsdkBridge.callback != null) 
                BrdsdkBridge.callback(choice);
        }
#endif

        /// <summary>
        /// SDK initialization method. Used to initialize the SDK and configure
        /// BrdsdkBridge with the method used to handle the user's choice.
        /// </summary>
        /// <param name="benefit">benefit text which is used in the consent screen</param>
        /// <param name="agree_btn">consent screen “agree” button text</param>
        /// <param name="disagree_btn">consent screen “disagree” button text</param>
		/// <param name="opt_out_instructions">Instructions of how to opt-out</param>
        /// <param name="on_choice_callback">The reference to the callback method</param>
		/// <param name="skip_consent">can be passed to skip showing the consent screen on the initialization of the API. The consent screen can be shown later with show_consent method.</param>
		/// <param name="language">the preferred language (optional): ar_SA, de-DE, en-US, es-ES, fa-AF, fr-FR, he-IL, hi-IN, it-IT, ja-JP, ko-KR, ms-MY, nl-NL, pt-PT, ro-RO, ru-RU, th, tr-TR, vi-VN, zh-CN, zh-TW</param>
		/// <param name="text_color">the tint color for the buttons (optional), e.g. 0xFF0000</param>
		/// <param name="background_color">the tint color for the buttons (optional), e.g. 0xFF0000</param>
		/// <param name="button_color">the tint color for the buttons (optional), e.g. 0xFF0000</param>
		/// <param name="background_image">set of background images of consent screen</param>
		/// <param name="opt_in_info">meta information for opt-in button</param>
		/// <param name="opt_out_info">meta information for opt-out button</param>
        public static void init(
            string benefit,
            string agree_btn,
            string disagree_btn,
            string opt_out_instructions,
            ChoiceCallback on_choice_callback,
            bool skip_consent = false,
            string language = null,
            int text_color = 0x000000,
            int background_color = 0xFFFFFF,
            int button_color = 0x0000FF,
            ConsentBackgroundImage background_image = null,
            ConsentActionInfo opt_in_info = null,
            ConsentActionInfo opt_out_info = null
        )
        {
            BrdsdkBridge.callback = on_choice_callback;
#if UNITY_IOS || UNITY_TVOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS)
            {
                // register the delegate method
                brdsdk_set_delegate(on_choice_change);

                int language_len = 0;
                string language_parameter = "";
                if (language != null)
                {
                    language_len = language.Length;
                    language_parameter = language;
                }

                // initialize the SDK
                brdsdk_init(
                    benefit, benefit.Length,
                    agree_btn, agree_btn.Length,
                    disagree_btn, disagree_btn.Length,
                    opt_out_instructions,
                    skip_consent,
                    language_parameter,
                    language_len,
                    text_color,
                    background_color,
                    button_color,
                    background_image,
                    opt_in_info,
                    opt_out_info
                );
                return;
            }
#endif
            BrdsdkBridge.callback(BrdsdkBridge.CHOICE_NONE);
        }

        /// <summary>
        /// Disable Bright SDK, e.g. from Settings screen
        /// </summary>
        public static void opt_out()
        {
#if UNITY_IOS || UNITY_TVOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS)
                brdsdk_opt_out();
#endif
        }

        /// <summary>
        /// Opts-in and starts Bright SDK process if acceptable.
        /// Opt-in method is NOT allowed by default.
        /// Always use `show_consent()` or consult with BrightData first.
        /// </summary>
        /// <returns>True if operation is permitted by authorization device</returns>
        public static bool silent_opt_in()
        {
#if UNITY_IOS || UNITY_TVOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS)
                return brdsdk_silent_opt_in();
#endif
            return false;
        }

        /// <summary>
        /// Show consent screen, e.g. when trying to turn on from Settings screen
        /// </summary>
        /// <param name="benefit">benefit text which is used in the consent screen</param>
        /// <param name="agree_btn">consent screen “agree” button text</param>
        /// <param name="disagree_btn">consent screen “disagree” button text</param>
        /// <param name="language">the preferred language (optional): ar_SA, de-DE, en-US, es-ES, fa-AF, fr-FR, he-IL, hi-IN, it-IT, ja-JP, ko-KR, ms-MY, nl-NL, pt-PT, ro-RO, ru-RU, th, tr-TR, vi-VN, zh-CN, zh-TW</param>
        /// <returns>True if consent was shown</returns>
        public static bool show_consent(
            string benefit = null,
            string agree_btn = null,
            string disagree_btn = null,
            string language = null
        )
        {
#if UNITY_IOS || UNITY_TVOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS)
            {
                int benefit_len = 0;
                if (benefit != null)
                    benefit_len = benefit.Length;
                int agree_btn_len = 0;
                if (agree_btn != null)
                    agree_btn_len = agree_btn.Length;
                int disagree_btn_len = 0;
                if (disagree_btn != null)
                    disagree_btn_len = disagree_btn.Length;
                return brdsdk_show_consent(
                    benefit, benefit_len,
                    agree_btn, agree_btn_len,
                    disagree_btn, disagree_btn_len,
                    language);
            }
#endif
            return false;
        }

        /// <summary>
        /// Obtains the user's choice:
        /// - <see cref="CHOICE_AGREED">BrdsdkBridge.CHOICE_AGREED</see> - User agreed. SDK is enabled and works;
        /// - <see cref="CHOICE_DISAGREED">BrdsdkBridge.CHOICE_DISAGREED</see> - User disagreed. SDK disabled
        ///     and disconnected;
        /// - <see cref="CHOICE_NONE">BrdsdkBridge.CHOICE_NONE</see> - before the user passes the consent screen
        ///     for the first time.
        /// </summary>
        /// <returns>the user's choice</returns>
        public static int get_choice()
        {
#if UNITY_IOS || UNITY_TVOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS)
                return brdsdk_get_choice();
            else
                return BrdsdkBridge.CHOICE_NONE;
#else
            return BrdsdkBridge.CHOICE_NONE;
#endif
        }

        /// <summary>
        /// Triggers post actions when custom consent screen was shown.
        /// When you implement custom consent screen you must call this method when the screen is presented.
        /// </summary>
        /// <returns>True if no errors, false when sdk authorization status is not `Authorized`</returns>
        public static bool consent_shown()
        {
#if UNITY_IOS || UNITY_TVOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS)
                return brdsdk_consent_shown();
            else
                return false;
#else
            return false;
#endif
        }

        /// <summary>
        /// Checks availability of running sdk on the device. It allows you to determine what you are able to do with sdk.
        /// You should check the status and decide your reaction on it before showing your custom consent screen.
        /// 
        /// This method is required to call when you attempt to use your own consent screen. In the other case it's optional.
        /// </summary>
        /// <returns>Authorization status</returns>
        public static AuthorizationStatus authorize_device()
        {
#if UNITY_IOS || UNITY_TVOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS)
                return brdsdk_authorize_device();
#endif
            return AuthorizationStatus.PlatformNotSupported;
        }

        /// <summary>
        /// Retrieves current sdk uuid value.
        /// </summary>
        /// <returns>Nil in case sdk is not initialized, or stored uuid value</returns>
        public static string get_uuid()
        {
#if UNITY_IOS || UNITY_TVOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.tvOS)
            {
                IntPtr ptr = brdsdk_get_uuid();
                return Marshal.PtrToStringAuto(ptr);
            }
            else
                return null;
#else
            return null;
#endif
        }

#if UNITY_EDITOR_OSX
        private const string FRAMEWORK_ORIGIN_PATH = "Assets/BrightDataSDK/";
        private const string FRAMEWORK_TARGET_PATH = "Frameworks";
        private const string coreFrameworkName = "brdsdk.framework";

        /// <summary>
	    /// The method that tweaks the generated Xcode project -
	    /// links the target with brdsdk.framework
	    /// </summary>
	    /// <param name="target">The build target</param>
	    /// <param name="pathToBuiltProject">The path to the project</param>
        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuildTODO(BuildTarget target, string pathToBuiltProject)
        {
            bool use_xcframework_for_unity = false;
            if (!use_xcframework_for_unity)
            {
                string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);            
                PBXProject proj = new PBXProject();            
                proj.ReadFromString(File.ReadAllText(projPath));            
                string targetGuid = proj.GetUnityMainTargetGuid();            
                const string defaultLocationInProj = "BrightDataSDK";            
                const string coreFrameworkName = "brdsdk.framework";            
                string framework = Path.Combine(defaultLocationInProj, coreFrameworkName);            
                string fileGuid = proj.AddFile(framework,            
                    "Frameworks/" + framework, PBXSourceTree.Sdk);            
                PBXProjectExtensions.AddFileToEmbedFrameworks(proj, targetGuid, fileGuid);            
                proj.SetBuildProperty(targetGuid,            
                    "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");            
                proj.WriteToFile(projPath);
            }
            else {
            string sourcePath = FRAMEWORK_ORIGIN_PATH;
            string destPath = Path.Combine(FRAMEWORK_TARGET_PATH, coreFrameworkName);
            
            iOSSdkVersion targetSdk = PlayerSettings.iOS.sdkVersion;
            tvOSSdkVersion targetTvSdk = PlayerSettings.tvOS.sdkVersion;

            string deviceFrameworkPath = "ios-arm64" + coreFrameworkName;
            string simulatorFrameworkPath = "ios-arm64_x86_64-simulator" + coreFrameworkName;
            string deviceTvFrameworkPath = "tvos-arm64" + coreFrameworkName;
            string simulatorTvFrameworkPath = "tvos-arm64_x86_64-simulator" + coreFrameworkName;
#if UNITY_IOS
            if(targetSdk == iOSSdkVersion.DeviceSDK)
                sourcePath = Path.Combine(sourcePath, deviceFrameworkPath);
            else if (targetSdk == iOSSdkVersion.SimulatorSDK)
                sourcePath = Path.Combine(sourcePath, simulatorFrameworkPath);
#endif
#if UNITY_TVOS
            if(targetTvSdk == tvOSSdkVersion.Device)
                sourcePath = Path.Combine(sourcePath, deviceTvFrameworkPath);
            else if (targetTvSdk == tvOSSdkVersion.Simulator)
                sourcePath = Path.Combine(sourcePath, simulatorTvFrameworkPath);
#endif
            CopyDirectory(sourcePath, Path.Combine(pathToBuiltProject, destPath));

            Debug.Log("Copied: " + sourcePath + " -> " + Path.Combine(pathToBuiltProject, destPath));

            // obtain the xcode project
            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string targetGuid = proj.GetUnityMainTargetGuid();
            string unityFrameworkGuid = proj.GetUnityFrameworkTargetGuid();
            string fileGuid = proj.AddFile(destPath, destPath, PBXSourceTree.Source);

            // delete temporary files
            string pathFrameworks = FRAMEWORK_TARGET_PATH;
            string pathBrightData = Path.Combine(pathFrameworks, "BrightDataSDK");
            string pathTmp1 = Path.Combine(pathBrightData, deviceFrameworkPath);
            string tmp1Guid = proj.FindFileGuidByRealPath(pathTmp1, PBXSourceTree.Source);
            proj.RemoveFile(tmp1Guid);
            string pathTmp2 = Path.Combine(pathBrightData, simulatorFrameworkPath);
            string tmp2Guid = proj.FindFileGuidByRealPath(pathTmp2, PBXSourceTree.Source);
            proj.RemoveFile(tmp2Guid);
            string pathTmp3 = Path.Combine(pathBrightData, deviceTvFrameworkPath);
            string tmp3Guid = proj.FindFileGuidByRealPath(pathTmp3, PBXSourceTree.Source);
            proj.RemoveFile(tmp3Guid);
            string pathTmp4 = Path.Combine(pathBrightData, simulatorTvFrameworkPath);
            string tmp4Guid = proj.FindFileGuidByRealPath(pathTmp4, PBXSourceTree.Source);
            proj.RemoveFile(tmp4Guid);
            
            // tweak project settings
            proj.AddFileToBuild(targetGuid, fileGuid);
            proj.AddFrameworkToProject(targetGuid, coreFrameworkName, false);
            proj.SetBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(SRCROOT)/Frameworks");
            proj.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
            proj.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
            PBXProjectExtensions.AddFileToEmbedFrameworks(proj, targetGuid, fileGuid);

            proj.WriteToFile(projPath);
            }
        }

        private static void CopyDirectory(string sourcePath, string destPath)
        {
            if (Directory.Exists(destPath))
                Directory.Delete(destPath);
            if (File.Exists(destPath))
                File.Delete(destPath);
            Directory.CreateDirectory(destPath);
 
            foreach (string file in Directory.GetFiles(sourcePath))
                File.Copy(file, Path.Combine(destPath, Path.GetFileName(file)));
 
            foreach (string dir in Directory.GetDirectories(sourcePath))
                CopyDirectory(dir, Path.Combine(destPath, Path.GetFileName(dir)));
        }
#endif
    }
}
