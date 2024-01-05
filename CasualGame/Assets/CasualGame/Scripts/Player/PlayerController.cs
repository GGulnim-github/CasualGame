using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerState, PlayerController> StateMachine { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public CapsuleCollider CapsuleCollider { get; private set; }
    public Animator Animator { get; private set; }

    public Vector3 MoveInput { get; private set; }
    public Vector3 InputDirection { get; private set; }

    public bool IsGrounded { get; private set; }

    [HideInInspector] public float speed = 0f;
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 0.12f;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        CapsuleCollider = GetComponent<CapsuleCollider>();
        Animator = GetComponent<Animator>();

        StateMachine = new StateMachine<PlayerState, PlayerController>(this);
        StateMachine.AddState(PlayerState.Move, new PlayerMoveState());

        StateMachine.ChangeState(PlayerState.Move);
    }

    private void Update()
    {
        CheckGround();
        SetInput();

        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    void CheckGround()
    {
    }

    void SetInput()
    {
        MoveInput = InputManager.Instance.moveInput;
        InputDirection = new Vector3(MoveInput.x, 0, MoveInput.y).normalized;
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
}
