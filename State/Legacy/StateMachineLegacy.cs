using System;
using UnityEngine;
using Util;

public abstract class StateMachineLegacy<T1, T2> where T1 : StateMachineLegacy<T1, T2>, new()
{
    public static readonly T1 Default = new T1();

    public T2 State { get; private set; }
    public event EventHandler<T2> OnChange;

    public void RequestState(T2 state, bool force = false, bool notify = true)
    {
        if (!force && state.Equals(State)) return;
        State = state;
        if (notify) Objects.OnMain(NotifyChanged);
    }

    private void NotifyChanged()
    {        
        Debug.Log($"<color=blue>{typeof(T2).ShortName()}: {State}</color>");
        OnChange?.Invoke(this, State);
    }
}