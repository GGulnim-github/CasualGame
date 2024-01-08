using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : State<PlayerController>
{
    public override void Update(PlayerController controller)
    {
        Rotate(controller);
    }

    public override void FixedUpdate(PlayerController controller)
    {
        Move(controller);
    }

    void Rotate(PlayerController controller)
    {
        controller.targetRotation = Mathf.Atan2(controller.InputDirection.x, controller.InputDirection.z) * Mathf.Rad2Deg + controller.cameraLookTrasform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(controller.transform.eulerAngles.y, controller.targetRotation, ref controller.rotationVelocity, controller.rotationSmoothTime);
        controller.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }

    void Move(PlayerController controller)
    {
        float targetSpeed = controller.IsRun ? controller.runSpeed : controller.walkSpeed;
        float currentHorizontalSpeed = new Vector3(controller.Rigidbody.velocity.x, 0.0f, controller.Rigidbody.velocity.z).magnitude;
        //controller.speed = Mathf.SmoothDamp(currentHorizontalSpeed, targetSpeed, ref controller.speedVelocity, controller.speedSmoothTime);
        controller.speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.fixedDeltaTime * 10f);
        controller.animationBlend = Mathf.Lerp(controller.animationBlend, targetSpeed, Time.fixedDeltaTime * 10f);

        Vector3 targetDirection = Quaternion.Euler(0.0f, controller.targetRotation, 0.0f) * Vector3.forward;
        Vector3 gravity = Vector3.down * Mathf.Abs(controller.Rigidbody.velocity.y);
        if(controller.IsGrounded && controller.IsSlope)
        {
            targetDirection = controller.AdjustDirectionToSlope(targetDirection);
            gravity = Vector3.zero;
            controller.Rigidbody.useGravity = false;
        }
        else
        {
            controller.Rigidbody.useGravity = true;
        }
        controller.Rigidbody.velocity = controller.speed * targetDirection.normalized + gravity;
    }
}
