using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Util;

[CreateAssetMenu(menuName = "State/Machine")]
public class StateMachineScriptable : Singleton<StateMachineScriptable>
{
    public string Name;
    [NotNull] public ScriptableState DefaultState;

    public ScriptableState[] ValidStates;
    HashSet<ScriptableState> _valid;

    [NotNull] public ScriptableState State;
    public event EventHandler<ScriptableState> OnChange;

    void Awake()
    {
        if (Application.isPlaying)
            Reset();
    }

    public void Reset()
    {
        _valid = new HashSet<ScriptableState>(ValidStates);
        RequestState(DefaultState, true, true);
    }

    public void Click_RequestState(ScriptableState state)
    {
        RequestState(state);
    }

    public void RequestState([NotNull] ScriptableState state, bool force = false, bool notify = true)
    {
        if (!force && state.Equals(State)) return;
        Validate(state);
        State = state;
        if (notify) Objects.OnMain(NotifyChanged); 
    }

    void Validate(ScriptableState state) 
    {
        if (state == null) throw new Exception("Default state cannot be null. Create a null instance if you want.");
        if (!_valid.Contains(state)) throw new Exception($"Don't recognize {state}");
    }

    void NotifyChanged()
    {
        Debug.Log($"<color=blue>{typeof(ScriptableState).ShortName()}: {State}</color>");
        OnChange?.Invoke(this, State);
    }

    public override string ToString()
    {
        return Name;
    }
}