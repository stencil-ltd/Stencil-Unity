using System;
using System.Linq;
using Plugins.State;

public class StateGateScriptable : ActiveGate
{
    public StateMachineScriptable Machine;

    public ScriptableState[] States;
    public bool Invert;
    public bool AndDestroy;

    public ScriptableState State => Machine.State;

    public override void Register(ActiveManager manager)
    {
        base.Register(manager);
        Machine.OnChange += Changed;
    }

    public override void Unregister()
    {
        Machine.OnChange -= Changed;
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

    private void Changed(object sender, ScriptableState e)
    {
        ActiveManager.Check();
    }
}