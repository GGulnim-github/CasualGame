using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State<PlayerController>
{
    float _rotationVelocity;

    public override void Update(PlayerController controller)
    {
        float targetRotation = Mathf.Atan2(controller.InputDirection.x, controller.InputDirection.z) * Mathf.Rad2Deg;
        float rotation = Mathf.SmoothDampAngle(controller.transform.eulerAngles.y, targetRotation, ref _rotationVelocity, controller.rotationSpeed);
        controller.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    public override void FixedUpdate(PlayerController controller)
    {
        //controller.Rigidbody.velocity = controller.transform.forward * controller.speed;
    }
}
