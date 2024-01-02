using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public abstract class Manager<T> : PersistentSingleton<T> where T : Component
{
    protected override void Awake()
    { 
        base.Awake();
        Initialize();
    }
    public abstract void Initialize();
}
