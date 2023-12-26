using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public abstract class Manager<T> : PersistentSingleton<T> where T : Component
{
    protected virtual void Start()
    { 
        Initialize();
    }
    public abstract void Initialize();
}
