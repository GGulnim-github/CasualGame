using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class TPSCharacterController : MonoBehaviour
{
    [Header("Camera")]
    public TPSCameraController cameraController;
    public Transform cameraTarget;

    [Header("Ground Check Settings")]
    public LayerMask groundLayerMask;
    public float groundCheckRadius = 0.1f;
    public float groundCheckHeighOfsset = 0.1f;
    public float groundCheckSize = 0.5f;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 0.12f;
    public float jumpFoce = 3f;
    public float jumpDelay = 0.2f;

    [Header("States")]
    public bool isMoving;
    public bool isGrounded;
    public bool isJumping;
    public bool canJump;

    float _targetRotation;
    float _rotationVelocity;

    Vector2 _moveInput;
    bool _jumpInput;

    Rigidbody m_Rigidbody;
    CapsuleCollider m_Collider;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        CheckGround();
        SetInput();

        Jump();       
        Rotate();
    }

    private void FixedUpdate()
    {
        Movement();
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
    }
    void SetInput()
    {
        _jumpInput = InputManager.Instance.jumpInput;

        _moveInput = InputManager.Instance.moveInput;
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
        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTarget.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSpeed);
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    void Movement()
    {
        if (isMoving == false)
        {
            return;
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        transform.Translate(moveSpeed * Time.deltaTime * targetDirection, Space.World);
    }

    public void Teleport(Vector3 position = default(Vector3), float eulerAngleY = 0f, float cameraXAxis = 0f, float cameraYAxis = 0f)
    {
        transform.SetPositionAndRotation(position, Quaternion.Euler(0f, eulerAngleY, 0f));
        if (cameraXAxis == 0f && cameraYAxis == 0f)
        {
            cameraController.SetRotation(Vector3.zero);
        }
        else
        {
            cameraController.SetRotation(new Vector3(cameraXAxis, cameraYAxis, 0f));
        }
    }

    Transform CreateEmptyTransform(string name = "New Transform", Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Transform parent = null, bool hide = false)
    {
        Transform newTransform = new GameObject(name).transform;

        newTransform.SetPositionAndRotation(position, rotation);
        newTransform.SetParent(parent);

        if (hide)
        {
            newTransform.hideFlags = HideFlags.HideInHierarchy;
            newTransform.gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        return newTransform;
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
