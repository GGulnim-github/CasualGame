using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Look Settings")]
    public float lookOffsetY = 1.4f;
    public float topClamp = 70.0f;
    public float bottomClamp = -30.0f;
    public float farDistance = 7.5f;
    public float nearDistance = 2f;
    public float xAxisSpeed = 0.1f;
    public float yAxisSpeed = 0.1f;

    [Header("Ground Settings")]
    public LayerMask groundLayerMask;
    public float groundCheckRadius = 0.1f;
    public float groundCheckHeighOfsset = 0.1f;
    public float groundCheckSize = 0.5f;
    public float groundMaxSlopeAngle = 45f;

    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 0.12f;
    public float jumpFoce = 1.8f;
    public float jumpDelay = 0.2f;

    [Header("States")]
    public bool isMoving;
    public bool isGrounded;
    public bool isJumping;
    public bool canJump;
    public bool isSlope;

    float _targetRotation;
    float _rotationVelocity;

    float _groundAngle;
    Vector3 _groundNormal;
    Vector3 _groundPoint;

    Vector2 _moveInput;
    bool _jumpInput;
    Vector2 _lookInput;
    float _zoomInput;

    Transform _lookTransform;
    Quaternion _lastLookTransformRotation;

    PlayerFollowCamera _followCamera;

    Rigidbody m_Rigidbody;
    CapsuleCollider m_Collider;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<CapsuleCollider>();

        _lookTransform = CreateEmptyTransform("Look Transform", position: new Vector3(0f, lookOffsetY, 0f), parent: transform);
        _lastLookTransformRotation = _lookTransform.rotation;

        _followCamera = CameraManager.Instance.PlayerFollowCamera;
        _followCamera.SetTarget(_lookTransform);
    }

    private void Update()
    {
        CheckGround();
        SetInput();

        Zoom();

        Jump();       
        Rotate();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void LateUpdate()
    {
        Look();
    }

    void CheckGround()
    {
        Collider[] groundcheck = Physics.OverlapBox(transform.position + transform.up * groundCheckHeighOfsset, 
            new Vector3(groundCheckRadius, groundCheckSize, groundCheckRadius), 
            transform.rotation, 
            groundLayerMask);

        if (groundcheck.Length != 0) 
        {
            isGrounded = true;

            if (canJump == false && isJumping == false && IsInvoking(nameof(EnableCanJump)) == false)
            {
                Invoke(nameof(EnableCanJump), jumpDelay);
            }
        }
        else
        {
            isGrounded = false;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out hit, 2, groundLayerMask))
        {
            _groundAngle = Vector3.Angle(Vector3.up, hit.normal);
            _groundNormal = hit.normal;
            _groundPoint = hit.point;
        }
        else
        {
            _groundAngle = 0f;
            _groundNormal = Vector3.zero;
            _groundPoint = Vector3.zero;
        }
        isSlope = _groundAngle != 0f && _groundAngle < groundMaxSlopeAngle;
    }
    void SetInput()
    {
        _moveInput = InputManager.Instance.moveInput;
        _jumpInput = InputManager.Instance.jumpInput;
        _lookInput = InputManager.Instance.lookInput;
        _zoomInput = InputManager.Instance.zoomInput;

        isMoving = (_moveInput != Vector2.zero);
    }

    void Jump()
    {
        if (_jumpInput == false || isGrounded == false || isJumping == true || canJump == false)
        {
            return;
        }

        isGrounded = false;
        isJumping = true;
        canJump = false;

        m_Rigidbody.AddForce(200 * jumpFoce * transform.up, ForceMode.Impulse);
        Invoke(nameof(DisableJump), 0.3f);
    }
    void DisableJump()
    {
        isJumping = false;
    }
    void EnableCanJump()
    {
        canJump = true;
    }

    void Rotate()
    {
        if (isMoving == false)
        {
            return;
        }

        Vector3 inputDirection = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;
        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _lookTransform.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSpeed);
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
    void Movement()
    {
        if (isMoving == false)
        {
            m_Rigidbody.velocity = transform.up * m_Rigidbody.velocity.y;
            return;
        }

        Vector3 direction = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        Vector3 velocity = (isGrounded && isSlope && !isJumping) ? AdjustDirectionToSlope(direction) : direction;
        
        //m_Rigidbody.velocity = velocity * moveSpeed + transform.up * m_Rigidbody.velocity.y;        
        transform.Translate(moveSpeed * Time.deltaTime * velocity, Space.World);
    }

    void Zoom()
    {
        _followCamera.Zoom(_zoomInput, farDistance, nearDistance);
    }
    void Look()
    {
        _lookTransform.rotation = _lastLookTransformRotation;

        Vector3 targetEulerAngles = _lookTransform.rotation.eulerAngles;
        targetEulerAngles.x -= _lookInput.y * yAxisSpeed;
        targetEulerAngles.y += _lookInput.x * xAxisSpeed;

        targetEulerAngles.x = ClampAngleX(targetEulerAngles.x, bottomClamp, topClamp);
        targetEulerAngles.y = ClampAngleY(targetEulerAngles.y, float.MinValue, float.MaxValue);

        _lookTransform.transform.rotation = Quaternion.Euler(targetEulerAngles.x, targetEulerAngles.y, 0f);
        _lastLookTransformRotation = _lookTransform.rotation;
    }

    public void Teleport(Vector3 position = default(Vector3), float eulerAngleY = 0f)
    {
        Quaternion rotation = Quaternion.Euler(0f, eulerAngleY, 0f);

        transform.SetPositionAndRotation(position, rotation);
        _lastLookTransformRotation = rotation;
    }

    Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _groundNormal).normalized;
    }

    Transform CreateEmptyTransform(string name = "New Transform", Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parent = null, bool hide = false)
    {
        Transform newTransform = new GameObject(name).transform;

        newTransform.SetLocalPositionAndRotation(position, rotation);
        newTransform.SetParent(parent);

        if (hide)
        {
            newTransform.hideFlags = HideFlags.HideInHierarchy;
            newTransform.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        return newTransform;
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        Gizmos.DrawCube(transform.position + transform.up * groundCheckHeighOfsset, new Vector3(groundCheckRadius, groundCheckSize, groundCheckRadius));
    }
#endif
}
