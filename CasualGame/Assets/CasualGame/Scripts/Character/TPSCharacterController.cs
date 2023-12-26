using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class TPSCharacterController : MonoBehaviour
{
    [Header("Ground Check Settings")]
    public LayerMask groundLayerMask;
    public float groundCheckRadius = 0.1f;
    public float groundCheckHeighOfsset = 0.1f;
    public float groundCheckSize = 0.5f;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float jumpFoce = 3f;
    public float jumpDelay = 0.2f;

    [Header("States")]
    public bool isGrounded;
    public bool isJumping;
    public bool canJump;

    Vector2 _moveInput;
    Vector3 _moveInputDirection;

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

        if (InputManager.Instance.jumpInput && isGrounded == true)
        {
            Jump();           
        }

        _moveInput = InputManager.Instance.moveInput;
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

    void Jump()
    {
        if (isGrounded == false || isJumping == true || canJump == false)
        {
            return;
        }

        //Logger.Log("Jump");
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

    void Movement()
    {
        _moveInputDirection = new Vector3(_moveInput.x, 0, _moveInput.y);

        transform.Translate(_moveInputDirection * moveSpeed * Time.deltaTime, Space.World);
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
