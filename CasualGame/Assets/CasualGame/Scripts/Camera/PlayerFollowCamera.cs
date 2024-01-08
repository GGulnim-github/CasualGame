using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerFollowCamera : MonoBehaviour
{
    CinemachineVirtualCamera m_Camera;

    Quaternion _lastCameraTargetRotation;
    private void Awake()
    {
        m_Camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetTarget(Transform target)
    {
        m_Camera.Follow = target;
        _lastCameraTargetRotation = target.rotation;
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

    public void Look(Vector2 lookInput)
    {
        Vector3 targetEulerAngles = _lastCameraTargetRotation.eulerAngles;

        targetEulerAngles.x -= lookInput.y * 0.1f;
        targetEulerAngles.y += lookInput.x * 0.1f;

        targetEulerAngles.x = ClampAngleX(targetEulerAngles.x, -30, 70f);
        targetEulerAngles.y = ClampAngleY(targetEulerAngles.y, float.MinValue, float.MaxValue);

        m_Camera.Follow.rotation = Quaternion.Euler(targetEulerAngles.x, targetEulerAngles.y, 0f);
        _lastCameraTargetRotation = m_Camera.Follow.rotation;
    }

    float ClampAngleX(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -180f)
        {
            lfAngle += 360f;
        }
        if (lfAngle > 180f)
        {
            lfAngle -= 360f;
        }
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    float ClampAngleY(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
        {
            lfAngle += 360f;
        }
        if (lfAngle > 360f)
        {
            lfAngle -= 360f;
        }
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
