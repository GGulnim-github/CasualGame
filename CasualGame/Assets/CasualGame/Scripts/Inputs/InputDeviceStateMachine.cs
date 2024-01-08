using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputDevice
{
    public abstract void OnEnter();
    public abstract void OnUpdate(InputManager manager);
}

public class KeyboardMouseInputDevice : InputDevice
{
    enum LookState
    {
        None,
        Rotate,
    }
    LookState _lookState;

    PlayerInput _playerInput;

    Mouse _mouse;
    Vector2? _mousePositionToWarpToAfterCursorUnlock;

    public KeyboardMouseInputDevice(PlayerInput playerInput)
    {
        _playerInput = playerInput;
    }

    public override void OnEnter()
    {
        _lookState = LookState.None;
    }

    public override void OnUpdate(InputManager manager)
    {
        manager.moveInput = manager.GetInputAction(PlayerInputType.Move).ReadValue<Vector2>();
        manager.jumpInput = manager.GetInputAction(PlayerInputType.Jump).triggered;
        manager.lookInput = manager.GetInputAction(PlayerInputType.Look).ReadValue<Vector2>();
        manager.zoomInput = manager.GetInputAction(PlayerInputType.Zoom).ReadValue<Vector2>().y / 120f;
        Cursor.lockState = CursorLockMode.Locked;
        //switch (_lookState)
        //{
        //    case LookState.None:
        //        if (manager.IsPointerOverUI())
        //        {
        //            break;
        //        }
        //        //Logger.Log($"{manager.GetInputAction(PlayerInputType.LookEnage).WasPressedThisFrame()}_{manager.IsPointerInsideScreen()}");
        //        if (manager.GetInputAction(PlayerInputType.LookEnage).WasPressedThisFrame() && manager.IsPointerInsideScreen())
        //        {
        //            _mouse = _playerInput.GetDevice<Mouse>();
        //            _mousePositionToWarpToAfterCursorUnlock = _mouse?.position.ReadValue();

        //            Cursor.lockState = CursorLockMode.Locked;

        //            _lookState = LookState.Rotate;
        //        }
        //        break;
        //    case LookState.Rotate:
        //        manager.lookInput = manager.GetInputAction(PlayerInputType.Look).ReadValue<Vector2>();

        //        if (_mouse != null)
        //        {
        //            _mousePositionToWarpToAfterCursorUnlock = _mousePositionToWarpToAfterCursorUnlock.Value + _mouse.delta.ReadValue();
        //        }

        //        if (!manager.GetInputAction(PlayerInputType.LookEnage).IsPressed())
        //        {
        //            Cursor.lockState = CursorLockMode.None;

        //            if (_mousePositionToWarpToAfterCursorUnlock != null)
        //                _mouse?.WarpCursorPosition(_mousePositionToWarpToAfterCursorUnlock.Value);

        //            manager.lookInput = Vector2.zero;

        //            _lookState = LookState.None;
        //        }
        //        break;
        //}
    }
}

public class TouchScreenInputDevice : InputDevice
{
    enum LookState
    {
        None,
        Rotate,
        Zoom,
    }
    //LookState _lookState;

    public override void OnEnter()
    {
        //_lookState = LookState.None;
    }

    public override void OnUpdate(InputManager manager)
    {
        manager.moveInput = manager.GetInputAction(PlayerInputType.Move).ReadValue<Vector2>();
        manager.jumpInput = manager.GetInputAction(PlayerInputType.Jump).triggered;
        manager.lookInput = manager.GetInputAction(PlayerInputType.Look).ReadValue<Vector2>();
        manager.zoomInput = manager.GetInputAction(PlayerInputType.Zoom).ReadValue<Vector2>().y / 120f;
    }
}

public class GamepadJoystickInputDevice : InputDevice
{
    public override void OnEnter()
    {
    }
    public override void OnUpdate(InputManager manager)
    {
        manager.moveInput = manager.GetInputAction(PlayerInputType.Move).ReadValue<Vector2>();
        manager.jumpInput = manager.GetInputAction(PlayerInputType.Jump).triggered;
        manager.lookInput = manager.GetInputAction(PlayerInputType.Look).ReadValue<Vector2>();
        manager.zoomInput = manager.GetInputAction(PlayerInputType.Zoom).ReadValue<Vector2>().y / 120f;
    }
}

public class InputDeviceStateMachine
{
    public InputDevice CurrentDevice { get; private set; }

    InputManager _manager;
    Dictionary<InputDeviceType, InputDevice> _devices = new();

    public InputDeviceStateMachine(InputManager manager)
    {
        _manager = manager;
        _devices.Add(InputDeviceType.KeyboardMouse, new KeyboardMouseInputDevice(manager.GetComponent<PlayerInput>()));
        _devices.Add(InputDeviceType.TouchScreen, new TouchScreenInputDevice());
        _devices.Add(InputDeviceType.GamepadJoystick, new GamepadJoystickInputDevice());
    }

    public void UpdateState()
    {
        CurrentDevice?.OnUpdate(_manager);
    }

    public void ChangeState(InputDeviceType deviceType)
    {
        if (CurrentDevice != _devices[deviceType])
        {
            CurrentDevice = _devices[deviceType];
            CurrentDevice.OnEnter();
        }
    }

    public bool IsDevice(InputDeviceType deviceType)
    {
        return CurrentDevice == _devices[deviceType];
    }
}
