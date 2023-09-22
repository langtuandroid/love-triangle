using UnityEngine;
using AppCentralCore;

public class ACLogger : MonoBehaviour
{
    private const string SdkTag = "[AC]: ";

    private static bool IsLogsDisabled = false;
    static Color color = new Color(0.5f, 0.75f, 1f, 1.0f);

    [System.Diagnostics.DebuggerStepThrough]
    public static void UserDebug(object message)
    {
        newSetting settings = AppCentralSettings.LoadSetting();

        if (settings.ShowLogs)
        {
#if UNITY_EDITOR
            string hexColor = ColorUtility.ToHtmlStringRGBA(color);
            Debug.Log($"<color=#{hexColor}>{SdkTag}</color>" + message);
#else
            Debug.Log(SdkTag + message);
#endif
        }
    }

    public static void UserDebug(object message, Color debugColor)
    {
        newSetting settings = AppCentralSettings.LoadSetting();

        if (settings.ShowLogs)
        {
#if UNITY_EDITOR
            string hexColor = ColorUtility.ToHtmlStringRGBA(debugColor);
            Debug.Log($"<color=#{hexColor}>{SdkTag}</color>" + message);
#else
            UserDebug(message);
#endif
        }
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static void UserWarning(object message)
    {
        Debug.LogWarning(SdkTag + message);
    }

    [System.Diagnostics.DebuggerStepThrough]
    public static void UserError(object message)
    {
        Debug.LogError(SdkTag + message);
    }
}
