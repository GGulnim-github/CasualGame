using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<Type, Controller> where Type : Enum where Controller : class
{
    public State<Controller> State { get; private set; }

    Controller _controller;
    Dictionary<Type, State<Controller>> _states = new();

    public StateMachine(Controller controller)
    {
        _controller = controller;
    }

    public void Update()
    {
        State?.Update(_controller);
    }

    public void FixedUpdate()
    {
        State?.FixedUpdate(_controller);
    }

    public void AddState(Type type, State<Controller> state)
    {
        if (_states.ContainsKey(type) == false)
        {
            _states.Add(type, state);
        }
    }

    public void DeleteState(Type type)
    {
        if (_states.ContainsKey(type) == true)
        {
            _states.Remove(type);
        }
    }

    public State<Controller> GetState(Type type)
    {
        if (_states.TryGetValue(type, out State<Controller> state))
        {
            return state;
        }
        return null;
    }

    public void ChangeState(Type type)
    {
        if (State == _states[type]) return;
        State?.OnExit(_controller);
        State = _states[type];
        State?.OnEnter(_controller);
    }

    public void Clear()
    {
        _states.Clear();
    }
}
