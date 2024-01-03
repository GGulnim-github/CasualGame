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

    Dictionary<PlayerInputType, InputAction> _inputActions = new();

    PlayerInput m_PlayerInput;
    InputDeviceStateMachine _deviceStateMachine;

    public override void Initialize()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        m_PlayerInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
        m_PlayerInput.controlsChangedEvent.AddListener(OnControlsChanged);

        _inputActions.Add(PlayerInputType.Move, m_PlayerInput.actions["Player/Move"]);
        _inputActions.Add(PlayerInputType.Jump, m_PlayerInput.actions["Player/Jump"]);
        _inputActions.Add(PlayerInputType.Look, m_PlayerInput.actions["Player/Look"]);
        _inputActions.Add(PlayerInputType.LookEnage, m_PlayerInput.actions["Player/LookEngage"]);
        _inputActions.Add(PlayerInputType.Zoom, m_PlayerInput.actions["Player/Zoom"]);

        if (_deviceStateMachine == null)
        {
            _deviceStateMachine = new InputDeviceStateMachine(this);
        }
    }

    private void Update()
    {
        _deviceStateMachine.UpdateState();
    }

    public InputAction GetInputAction(PlayerInputType type)
    {
        if (_inputActions.TryGetValue(type, out InputAction action))
        {
            return action;
        }
        return null;
    }

    public void OnControlsChanged(PlayerInput playerInput)
    {
        if (_deviceStateMachine == null)
        {
            _deviceStateMachine = new InputDeviceStateMachine(this);
        }

        if (playerInput.GetDevice<Touchscreen>() != null)
        {
            _deviceStateMachine.ChangeState(InputDeviceType.TouchScreen);
        }
        else if (playerInput.GetDevice<Pointer>() != null)
        {
            _deviceStateMachine.ChangeState(InputDeviceType.KeyboardMouse);
        }
        else if (playerInput.GetDevice<Gamepad>() != null || playerInput.GetDevice<Joystick>() != null)
        {
            _deviceStateMachine.ChangeState(InputDeviceType.GamepadJoystick);
        }
        else
        {
            Logger.LogError("Control scheme not recognized: " + playerInput.currentControlScheme);
        }
    }

    public bool IsPointerOverUI()
    {
        if (_deviceStateMachine.IsDevice(InputDeviceType.GamepadJoystick))
            return false;

        return EventSystem.current.IsPointerOverGameObject();
    }
    public bool IsPointerInsideScreen()
    {
        var pointer = m_PlayerInput.GetDevice<Pointer>();
        if (pointer == null)
            return true;

        return Screen.safeArea.Contains(pointer.position.ReadValue());
    }
}
