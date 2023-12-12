using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class VirtualButton : MonoBehaviour {
    void Start() {
        m_VirtualButton.RegisterOnButtonPressed(OnButtonPressed);
    }

    void Update() {
        if (!m_AnimationPlaying)
            return;

        Vector3 rotation = m_CubeTransform.localEulerAngles;
        rotation.y += 30.0f * Time.deltaTime;

        m_CubeTransform.localEulerAngles = rotation;
    }

    public void OnButtonPressed(VirtualButtonBehaviour button) {
        m_AnimationPlaying = !m_AnimationPlaying;
    }

    bool m_AnimationPlaying = false;

    public VirtualButtonBehaviour m_VirtualButton;
    public Transform m_CubeTransform;
}
