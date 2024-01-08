using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerState, PlayerController> StateMachine { get; private set; }
    [ReadOnly] public PlayerState currentState;

    #region Componet
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    #endregion

    #region Inputs
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Vector3 InputDirection { get; private set; }
    public bool JumpInput { get; private set; }
    #endregion

    #region States
    public bool IsGrounded { get; private set; }
    public bool IsSlope { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsRun { get; private set; } = true;
    public bool IsJumping { get; private set; } 
    public bool CanJump { get; private set; }
    #endregion

    [Space(5)]
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpHeight = 1.2f;

    public float speed;
    [HideInInspector] public float speedVelocity;
    public float speedSmoothTime = 0.12f;
    [HideInInspector] public float rotationVelocity;
    public float rotationSmoothTime = 0.12f;

    [HideInInspector] public float targetRotation;

    [Space(5)]
    [Header("Ground Settings")]
    public LayerMask groundLayerMask;
    Vector3 _groundNormal;
    float _groundAngle;

    [Header("Camera Settings")]
    public Transform cameraLookTrasform;
    public float cameraTopClamp = 70.0f;
    public float cameraBottomClamp = -30.0f;
    public float cameraFarDistance = 7.5f;
    public float cameraNearDistance = 2f;
    public float cameraXAxisSpeed = 0.1f;
    public float cameraYAxisSpeed = 0.1f;
    Quaternion _lastCameraLookRotation;

    #region Animation Params
    public float animationBlend;
    int _animIDSpeed;
    int _animIDGrounded;
    int _animIDMoving;
    #endregion

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();

        _lastCameraLookRotation = cameraLookTrasform.rotation;

        StateMachine = new StateMachine<PlayerState, PlayerController>(this);
        StateMachine.AddState(PlayerState.Idle, new PlayerIdleState());
        StateMachine.AddState(PlayerState.Move, new PlayerMoveState());
        StateMachine.AddState(PlayerState.Jump, new PlayerJumpState());

        StateMachine.ChangeState(PlayerState.Idle);

        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDMoving = Animator.StringToHash("Moving");
    }

    private void Update()
    {
        CheckGround();
        SetInput();
        SetState();
        SetAnimatorParameters();

        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        CameraRotate();
    }

    void CheckGround()
    {
        IsGrounded = Physics.CheckBox(transform.position + Vector3.up * 0.1f, new Vector3(0.05f, 0.25f, 0.05f), Quaternion.identity, groundLayerMask, QueryTriggerInteraction.Ignore);

        Ray ray = new(transform.position + Vector3.up * 0.2f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, groundLayerMask))
        {
            _groundAngle = Vector3.Angle(Vector3.up, hit.normal);
            _groundNormal = hit.normal;
        }
        else
        {
            _groundAngle = 0f;
            _groundNormal = Vector3.zero;
        }

        IsSlope = _groundAngle > 0f;
    }

    void SetInput()
    {
        MoveInput = InputManager.Instance.moveInput;
        LookInput = InputManager.Instance.lookInput;
        InputDirection = new Vector3(MoveInput.x, 0, MoveInput.y).normalized;
        JumpInput = InputManager.Instance.jumpInput;

        IsMoving = MoveInput != Vector2.zero;
    }

    void SetState()
    {
        if (IsGrounded)
        {
            if (CanJump == false && IsInvoking(nameof(EnableCanJump)) == false)
            {
                CanJump = true;
            }

            if (IsJumping == true && CanJump == true)
            {
                IsJumping = false;
            }

            if (JumpInput && IsJumping == false && CanJump == true)
            {
                IsJumping = true;
                CanJump = false;
                Invoke(nameof(EnableCanJump), 0.3f);
                StateMachine.ChangeState(PlayerState.Jump);
            }
            
            if (IsJumping == false)
            {
                if (IsMoving)
                {
                    StateMachine.ChangeState(PlayerState.Move);
                }
                else
                {
                    StateMachine.ChangeState(PlayerState.Idle);
                }
            }
        }
        else
        {
            if (IsJumping == false)
            {

            }
        }

        currentState = StateMachine.CurrentType;
    }

    void EnableCanJump()
    {
        CanJump = true;
    }

    void SetAnimatorParameters()
    {
        Animator.SetFloat(_animIDSpeed, animationBlend);
        Animator.SetBool(_animIDMoving, IsMoving);
        Animator.SetBool(_animIDGrounded, IsGrounded);
    }


    public Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _groundNormal).normalized;
    }

    void CameraRotate()
    {
        Vector3 targetEulerAngles = _lastCameraLookRotation.eulerAngles;

        targetEulerAngles.x -= LookInput.y * cameraXAxisSpeed;
        targetEulerAngles.y += LookInput.x * cameraYAxisSpeed;

        targetEulerAngles.x = ClampAngleX(targetEulerAngles.x, cameraBottomClamp, cameraTopClamp);
        targetEulerAngles.y = ClampAngleY(targetEulerAngles.y, float.MinValue, float.MaxValue);

        cameraLookTrasform.rotation = Quaternion.Euler(targetEulerAngles.x, targetEulerAngles.y, 0f);
        _lastCameraLookRotation = cameraLookTrasform.rotation;
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (IsGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        Gizmos.DrawCube(transform.position + Vector3.up * 0.1f, new Vector3(0.1f, 0.5f, 0.1f));
    }
#endif
}
