using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : Manager<InputManager>
{
    public Vector2 moveInput;
    public bool jumpInput;
    public Vector2 lookInput;
    public float zoomInput;

    PlayerInput m_PlayerInput;

    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _lookAction;
    InputAction _lookEngageAction;
    InputAction _zoomAction;

    Mouse _mouse;
    Vector2? _mousePositionToWarpToAfterCursorUnlock;

    enum State
    {
        InGame,
        InGameControllingCamera,
    }

    State _state;

    enum ControlStyle
    {
        None,
        KeyboardMouse,
        Touch,
        GamepadJoystick,
    }

    ControlStyle _controlStyle;

    public override void Initialize()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        m_PlayerInput.controlsChangedEvent.AddListener(OnControlsChanged);

        _moveAction = m_PlayerInput.actions["Player/Move"];
        _jumpAction = m_PlayerInput.actions["Player/Jump"];
        _lookAction = m_PlayerInput.actions["Player/Look"];
        _lookEngageAction = m_PlayerInput.actions["Player/LookEngage"];
        _zoomAction = m_PlayerInput.actions["Player/Zoom"];
    }

    private void Update()
    {
        moveInput = _moveAction.ReadValue<Vector2>();
        jumpInput = _jumpAction.triggered;

        switch (_state)
        {
            case State.InGame:
                if (IsPointerOverUI())
                {
                    break;
                }
                if (_controlStyle != ControlStyle.GamepadJoystick && _lookEngageAction.WasPressedThisFrame() && IsPointerInsideScreen())
                {
                    EngageCameraControl();
                }
                if (_controlStyle == ControlStyle.GamepadJoystick)
                {
                    ProcessCameraLook();
                }
                break;
            case State.InGameControllingCamera:
                ProcessCameraLook();

                if (_mouse != null)
                {
                    _mousePositionToWarpToAfterCursorUnlock = _mousePositionToWarpToAfterCursorUnlock.Value + _mouse.delta.ReadValue();
                }
                    
                if (!_lookEngageAction.IsPressed())
                {
                    DisengageCameraControl();
                }
                break;
        }

        zoomInput = _zoomAction.ReadValue<Vector2>().y / 120;
    }

    public void OnControlsChanged(PlayerInput playerInput)
    {
        if (playerInput.GetDevice<Touchscreen>() != null) // Note that Touchscreen is also a Pointer so check this first.
            _controlStyle = ControlStyle.Touch;
        else if (playerInput.GetDevice<Pointer>() != null)
            _controlStyle = ControlStyle.KeyboardMouse;
        else if (playerInput.GetDevice<Gamepad>() != null || playerInput.GetDevice<Joystick>() != null)
            _controlStyle = ControlStyle.GamepadJoystick;
        else
            Logger.LogError("Control scheme not recognized: " + playerInput.currentControlScheme);

        _mouse = default;
        _mousePositionToWarpToAfterCursorUnlock = default;
    }

    void EngageCameraControl()
    {
        _mouse = m_PlayerInput.GetDevice<Mouse>();
        _mousePositionToWarpToAfterCursorUnlock = _mouse?.position.ReadValue();

         Cursor.lockState = CursorLockMode.Locked;

        _state = State.InGameControllingCamera;
    }
    void DisengageCameraControl()
    {
        Cursor.lockState = CursorLockMode.None;

        if (_mousePositionToWarpToAfterCursorUnlock != null)
            _mouse?.WarpCursorPosition(_mousePositionToWarpToAfterCursorUnlock.Value);

        lookInput = Vector2.zero;

        _state = State.InGame;
    }

    void ProcessCameraLook()
    {
        lookInput = _lookAction.ReadValue<Vector2>();
    }

    bool IsPointerOverUI()
    {
        if (_controlStyle == ControlStyle.GamepadJoystick)
            return false;

        return EventSystem.current.IsPointerOverGameObject();
    }
    bool IsPointerInsideScreen()
    {
        var pointer = m_PlayerInput.GetDevice<Pointer>();
        if (pointer == null)
            return true;

        return Screen.safeArea.Contains(pointer.position.ReadValue());
    }
}
