using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerController>
{
    public override void Update(PlayerController controller)
    {
        Rotate(controller);
    }

    public override void FixedUpdate(PlayerController controller)
    {
        float currentHorizontalSpeed = new Vector3(controller.Rigidbody.velocity.x, 0.0f, controller.Rigidbody.velocity.z).magnitude;
        //controller.speed = Mathf.SmoothDamp(currentHorizontalSpeed, 0f, ref controller.speedVelocity, controller.speedSmoothTime);
        controller.speed = Mathf.Lerp(currentHorizontalSpeed, 0f, Time.fixedDeltaTime * 10f);
        controller.animationBlend = Mathf.Lerp(controller.animationBlend, 0f, Time.fixedDeltaTime * 10f);

        Vector3 targetDirection = Quaternion.Euler(0.0f, controller.targetRotation, 0.0f) * Vector3.forward;
        Vector3 gravity = Vector3.down * Mathf.Abs(controller.Rigidbody.velocity.y);
        if (controller.IsGrounded && controller.IsSlope)
        {
            targetDirection = controller.AdjustDirectionToSlope(targetDirection);
            gravity = Vector3.zero;
            controller.Rigidbody.useGravity = false;
        }
        else
        {
            controller.Rigidbody.useGravity = true;
        }

        controller.Rigidbody.velocity = targetDirection * controller.speed + gravity;
    }

    void Rotate(PlayerController controller)
    {
        float rotation = Mathf.SmoothDampAngle(controller.transform.eulerAngles.y, controller.targetRotation, ref controller.rotationVelocity, controller.rotationSmoothTime);
        controller.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
}
