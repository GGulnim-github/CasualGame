using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<Controller> where Controller : class
{
    public virtual void OnEnter(Controller controller) { }
    public virtual void OnExit(Controller controller) { }

    public virtual void Update(Controller controller) { }
    public virtual void FixedUpdate(Controller controller) { }
    public virtual void LateUpdate(Controller controller) { }
}
