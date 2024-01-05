using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerController>
{
    public override void FixedUpdate(PlayerController controller)
    {
        //float targetSpeed = 0f;
        //controller.speed = Mathf.Lerp(controller.speed, 0f, Time.fixedDeltaTime * 10f);

        //controller.Rigidbody.velocity = controller.transform.forward * targetSpeed;
    }
}
