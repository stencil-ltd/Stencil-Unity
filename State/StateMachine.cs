using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Plugins.Util;
using UnityEngine;
using Util;

[CreateAssetMenu(menuName = "State/Machine")]
public class StateMachine : Singleton<StateMachine>
{
    public string Name;
    public Color Color;

    [NotNull] public State DefaultState;

    public State[] ValidStates;
    HashSet<State> _valid;

    [NotNull] public State State;
    public event EventHandler<State> OnChange;

    void Awake()
    {
        if (Application.isPlaying)
            Reset();
    }

    public void Reset()
    {
        _valid = ValidStates == null ? new HashSet<State>() : new HashSet<State>(ValidStates);
        RequestState(DefaultState, true, true);
    }

    public void Click_RequestState(State state)
    {
        RequestState(state);
    }

    public void RequestState([NotNull] State state, bool force = false, bool notify = true)
    {
        if (!force && state.Equals(State)) return;
        Validate(state);
        State = state;
        if (notify) Objects.OnMain(NotifyChanged); 
    }

    public void Validate(State state) 
    {
        if (state == null) throw new Exception("Default state cannot be null. Create a null instance if you want.");
        if (!_valid.Contains(state)) throw new Exception($"Don't recognize {state}"); 
    }

    void NotifyChanged()
    {
        var color = Color;
        Debug.Log($"<color={color.LogString()}>{Name} -> {State.Name}</color>");
        OnChange?.Invoke(this, State);
    }

    public override string ToString()
    {
        return Name;
    }
}