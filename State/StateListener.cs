using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using System.Linq;

[Serializable]
public class StateListenEvent : UnityEvent<StateChange>
{}

public class StateListener : MonoBehaviour
{
    public StateMachine StateMachine;
    public State[] States;
    public StateListenEvent OnState;

    private void Awake()
    {
        StateMachine.OnChange += OnChange;
    }

    private void OnDestroy()
    {
        StateMachine.OnChange -= OnChange;
    }

    private void OnChange(object sender, StateChange e)
    {
        if (States.Contains(e.New))
            OnState.Invoke(e);
    }
}
