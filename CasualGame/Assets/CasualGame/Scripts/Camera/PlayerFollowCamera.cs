using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerFollowCamera : MonoBehaviour
{
    CinemachineVirtualCamera m_Camera;

    private void Awake()
    {
        m_Camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Zoom(float zoomInput, float farDistance, float nearDistance)
    {
        CinemachineComponentBase componentBase = m_Camera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is Cinemachine3rdPersonFollow)
        {
            float targetZoom = ((farDistance - nearDistance) / 10) * zoomInput;
            float newDistance = Mathf.Clamp((componentBase as Cinemachine3rdPersonFollow).CameraDistance + targetZoom, nearDistance, farDistance);
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = newDistance;
        }
    }

    public void SetTarget(Transform target)
    {
        m_Camera.Follow = target;
    }
}
