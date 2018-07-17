using System;
using System.Linq;
using System.Reflection;
using Plugins.State;
using UnityEngine;

public class StateGateLegacy<T1, T2> : ActiveGate where T1 : StateMachineLegacy<T1, T2>, new() where T2 : struct
{
    private static readonly T1 _machine;

    public T2[] States;
    public bool Invert;
    public bool AndDestroy;

    public T2 State => _machine.State;

    static StateGateLegacy()
    {
        if (!typeof(T2).IsEnum)
            throw new Exception("StateVisibility can only handle enums.");
        var t = typeof(T1);
        var f = t.BaseType.GetField("Default", BindingFlags.Public | BindingFlags.Static);
        _machine = (T1) f.GetValue(null);
        Debug.Log($"Found Machine: {_machine}"); 
    }

    public override void Register(ActiveManager manager)
    {
        base.Register(manager);
        _machine.OnChange += Changed;
    }

    public override void Unregister()
    {
        _machine.OnChange -= Changed;
    }

    public override bool? Check()
    {
        try 
        {
            var visible = States.Contains(State);
            if (Invert) visible = !visible;
            if (AndDestroy && !visible)
                Destroy(gameObject);
            return visible;
        } catch (Exception) 
        {
            return null;
        }
    }

    private void Changed(object sender, T2 e)
    {
        ActiveManager.Check();
    }
}