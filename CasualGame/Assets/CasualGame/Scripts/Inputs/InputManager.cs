using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Manager<InputManager>
{
    public Vector2 moveInput;
    public bool jumpInput;

    public Vector2 lookInput;

    InputActions _inputActions;

    public override void Initialize()
    {
        _inputActions = new InputActions();
        _inputActions.Enable();
    }

    private void Update()
    {
        moveInput = _inputActions.Player.Move.ReadValue<Vector2>();
        jumpInput = _inputActions.Player.Jump.triggered;

        lookInput = _inputActions.Player.Look.ReadValue<Vector2>();
    }
}
