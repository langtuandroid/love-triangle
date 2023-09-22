using System.Collections;
using UnityEngine;
using AlmostEngine.Screenshot;
using System;
public static class SimpleScreenShotTaker {

    public static Action<Texture2D> onCaptureDone; 

    public static Texture2D lastCapturedImage;

    private static bool m_isLoaded;
    private static Camera m_captureCamera;

    public static void Capture() {
        m_captureCamera = Camera.main;
        lastCapturedImage = SimpleScreenshotCapture.CaptureCameraToTexture(Screen.width, Screen.height, m_captureCamera);
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(WaitForCapture());
    }

    static IEnumerator WaitForCapture(){
        while(lastCapturedImage == null){
            yield return 0;
        }
        onCaptureDone?.Invoke(lastCapturedImage);
    }
}
