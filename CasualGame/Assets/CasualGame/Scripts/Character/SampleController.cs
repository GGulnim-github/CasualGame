//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.Interactions;
//using CharacterController;


//[RequireComponent(typeof(Player))]
//public class SampleController : MonoBehaviour
//{
//    public Player player { get; private set; }
//    public Vector3 inputDirection { get; private set; } // 키보드 입력으로 들어온 이동 방향
//    public Vector3 calculatedDirection { get; private set; } // 경사 지형 등을 계산한 방향
//    public Vector3 gravity { get; private set; }

//    #region #경사 체크 변수
//    [Header("경사 지형 검사")]
//    [SerializeField, Tooltip("캐릭터가 등반할 수 있는 최대 경사 각도입니다.")]
//    float maxSlopeAngle;
//    [SerializeField, Tooltip("경사 지형을 체크할 Raycast 발사 시작 지점입니다.")]
//    Transform raycastOrigin;

//    private const float RAY_DISTANCE = 2f;
//    private RaycastHit slopeHit;
//    private bool isOnSlope;
//    #endregion

//    #region #바닥 체크 변수
//    [Header("땅 체크")]
//    [SerializeField, Tooltip("캐릭터가 땅에 붙어 있는지 확인하기 위한 CheckBox 시작 지점입니다.")]
//    Transform groundCheck;
//    private int groundLayer;
//    private bool isGrounded;
//    #endregion

//    #region #UNITY_FUNCTIONS
//    void Start()
//    {
//        player = GetComponent<Player>();
//        groundLayer = 1 << LayerMask.NameToLayer("Ground");
//    }

//    void Update()
//    {
//        calculatedDirection = GetDirection(player.MoveSpeed * MoveState.CONVERT_UNIT_VALUE);
//        ControlGravity();
//    }
//    #endregion

//    public void OnDashInput(InputAction.CallbackContext context)
//    {
//        if (context.performed)
//        {
//            // 상태를 추가한 후, 여기서는 상태 전환만 해주도록 합니다.
//        }
//    }

//    protected Vector3 GetMouseWorldPosition()
//    {
//        Vector3 mousePosition = Mouse.current.position.ReadValue();
//        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
//        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 5f);

//        if (Physics.Raycast(ray, out RaycastHit HitInfo, Mathf.Infinity))
//        {
//            Vector3 target = HitInfo.point;
//            Vector3 myPosition = new Vector3(transform.position.x, 0f, transform.position.z);
//            target.Set(target.x, 0f, target.z);
//            return (target - myPosition).normalized;
//        }
//        return Vector3.zero;
//    }

//    protected Vector3 GetDirection(float currentMoveSpeed)
//    {
//        isOnSlope = IsOnSlope();
//        isGrounded = IsGrounded();
//        Vector3 calculatedDirection =
//            CalculateNextFrameGroundAngle(currentMoveSpeed) < maxSlopeAngle ?
//            inputDirection : Vector3.zero;

//        calculatedDirection = (isGrounded && isOnSlope) ?
//           AdjustDirectionToSlope(calculatedDirection) : calculatedDirection.normalized;
//        return calculatedDirection;
//    }

//    protected void ControlGravity()
//    {
//        gravity = Vector3.down * Mathf.Abs(player.rigidBody.velocity.y);

//        if (isGrounded && isOnSlope)
//        {
//            gravity = Vector3.zero;
//            player.rigidBody.useGravity = false;
//            return;
//        }
//        player.rigidBody.useGravity = true;
//    }

//    private float CalculateNextFrameGroundAngle(float moveSpeed)
//    {
//        // 다음 프레임 캐릭터 앞 부분 위치
//        Vector3 nextFramePlayerPosition =
//           raycastOrigin.position + inputDirection * moveSpeed * Time.fixedDeltaTime;

//        if (Physics.Raycast(nextFramePlayerPosition, Vector3.down,
//            out RaycastHit hitInfo, RAY_DISTANCE, groundLayer))
//        {
//            return Vector3.Angle(Vector3.up, hitInfo.normal);
//        }
//        return 0f;
//    }

//    public bool IsGrounded()
//    {
//        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.4f, transform.lossyScale.z);
//        return Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity,
//               groundLayer);
//    }

//    public bool IsOnSlope()
//    {
//        Ray ray = new Ray(transform.position, Vector3.down);
//        if (Physics.Raycast(ray, out slopeHit, RAY_DISTANCE, groundLayer))
//        {
//            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
//            return angle != 0f && angle < maxSlopeAngle;
//        }
//        return false;
//    }

//    public void OnMoveInput(InputAction.CallbackContext context)
//    {
//        Vector2 input = context.ReadValue<Vector2>();
//        inputDirection = new Vector3(input.x, 0f, input.y);
//    }

//    public void LookAt(Vector3 direction)
//    {
//        if (direction != Vector3.zero)
//        {
//            Quaternion targetAngle = Quaternion.LookRotation(direction);
//            transform.rotation = targetAngle;
//        }
//    }

//    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
//    {
//        Vector3 adjustVelocityDirection =
//            Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
//        return adjustVelocityDirection;
//    }
//}