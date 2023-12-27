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

    InputActions _inputActions;

    [SerializeField] bool _isOverUI;

    public override void Initialize()
    {
        _inputActions = new InputActions();
        _inputActions.Enable();
    }

    private void Update()
    {
        moveInput = _inputActions.Player.Move.ReadValue<Vector2>();
        jumpInput = _inputActions.Player.Jump.triggered;

        switch (ApplicationManager.Instance.DeviceType)
        {
            case DeviceType.Unknown:
                break;
            case DeviceType.Handheld:
                Touch[] touches = Input.touches;
                foreach (Touch touch in touches)
                {
                    if (touch.phase == UnityEngine.TouchPhase.Began)
                    {
                        _isOverUI = EventSystem.current.IsPointerOverGameObject();
                    }
                }
                break;
            case DeviceType.Console:
                break;
            case DeviceType.Desktop:
                if (Input.GetMouseButtonDown(0))
                {
                    _isOverUI = EventSystem.current.IsPointerOverGameObject();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    _isOverUI = false;
                }
                if (_isOverUI == false)
                {
                    lookInput = _inputActions.Player.Look.ReadValue<Vector2>();
                }
                break;
        }
    }
}
