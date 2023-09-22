using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform m_camView;

    private void Awake() {
        if (!m_camView) {
            m_camView = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + m_camView.forward, m_camView.up);
    }
}
