using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class GentleManCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_mainVcam;
    [SerializeField] private CinemachineVirtualCamera m_loopVCam;
    [SerializeField] private Transform m_playerMeshTransform;
    [SerializeField] private bool m_rotateLightOnLoop = false;
    [SerializeField] private Light m_mainLight;

    private Quaternion m_preLoopLightRot;

    private void OnEnable() {
        Level.OnSplineLoopEnter += OnSplineLoopEnter;
        Level.OnSplineLoopExit += OnSplineLoopExit;
    }

    private void OnDisable() {
        Level.OnSplineLoopEnter -= OnSplineLoopEnter;
        Level.OnSplineLoopExit -= OnSplineLoopExit;
    }

    private void OnSplineLoopEnter() {
        m_loopVCam.Priority = m_mainVcam.Priority + 1;
        if (m_rotateLightOnLoop) {
            m_preLoopLightRot = m_mainLight.transform.rotation;
            StartCoroutine(OnLoopLightRotCoroutine());
        }
    }

    private void OnSplineLoopExit() {
        m_loopVCam.Priority = m_mainVcam.Priority - 1;

        StopAllCoroutines();
        if (m_rotateLightOnLoop) {
            m_mainLight.transform.rotation = m_preLoopLightRot;
        }
    }

    private IEnumerator OnLoopLightRotCoroutine() {
        while (true) {
            Quaternion newRot = Quaternion.FromToRotation(Vector3.up, m_playerMeshTransform.up);
            m_mainLight.transform.rotation = newRot * m_preLoopLightRot;
            yield return null;
        }
    }
}
