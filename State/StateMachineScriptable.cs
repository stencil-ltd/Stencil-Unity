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
    ISet<ScriptableState> _valid;

    [NotNull] public ScriptableState State;
    public event EventHandler<ScriptableState> OnChange;

    void Awake()
    {
        Debug.Log($"{Name} is awake...");
        if (Application.isPlaying) 
            RequestState(DefaultState, true, true); 
    }

#if UNITY_EDITOR
    void Update() 
    {
        if (!Application.isPlaying)
        {
            _valid = ValidStates.ToSet();
            Validate(DefaultState);
        }
    }
#endif

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