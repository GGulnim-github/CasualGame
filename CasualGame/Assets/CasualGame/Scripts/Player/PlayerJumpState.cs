using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : State<PlayerController>
{
    public override void OnEnter(PlayerController controller)
    {
        controller.Rigidbody.useGravity = true;
        controller.Rigidbody.AddForce(Vector3.up * Mathf.Sqrt(controller.jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }
}
