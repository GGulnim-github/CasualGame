using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TPSCameraController : MonoBehaviour
{
    public Transform target;
    public float defaultXAxis = 10f;

    public float topClamp = 70.0f;
    public float bottomClamp = -30.0f;

    public float xAxisSpeed = 0.1f;
    public float yAxisSpeed = 0.1f;

    Vector2 _lookInput;
    Quaternion _previousRotation;

    CinemachineVirtualCamera m_Camera;

    private void Awake()
    {
        m_Camera = GetComponent<CinemachineVirtualCamera>();
        target.transform.localEulerAngles = new Vector3(defaultXAxis, 0f, 0f);
        _previousRotation = target.transform.rotation;
    }

    private void LateUpdate()
    {
        _lookInput = InputManager.Instance.lookInput;

        if (target == null)
        {
            return;
        }
        target.transform.rotation = _previousRotation;

        Vector3 targetEulerAngles = target.rotation.eulerAngles;
        targetEulerAngles.x -= _lookInput.y * yAxisSpeed;
        targetEulerAngles.y += _lookInput.x * xAxisSpeed;

        targetEulerAngles.x = ClampAngleX(targetEulerAngles.x, bottomClamp, topClamp);
        targetEulerAngles.y = ClampAngleY(targetEulerAngles.y, float.MinValue, float.MaxValue);
       
        target.transform.rotation = Quaternion.Euler(targetEulerAngles.x, targetEulerAngles.y, 0f);
        _previousRotation = target.transform.rotation;
    }

    public void SetRotation(Vector3 eulerAngles)
    {
        if (eulerAngles == Vector3.zero)
        {
            target.transform.localEulerAngles = new Vector3(defaultXAxis, 0f, 0f);
        }
        else
        {
            target.transform.localEulerAngles = eulerAngles;
        }
        _previousRotation = target.transform.rotation;
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
