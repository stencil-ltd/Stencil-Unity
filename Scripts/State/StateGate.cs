using System;
using System.Linq;

namespace Plugins.State
{
    public abstract class StateGate<StateType> : ActiveGate where StateType : struct
    {
        public StateType[] States;
        public bool Invert;
        public bool AndDestroy;

        public StateMachine<StateType> Machine;
        public StateType State => Machine.State;

        public override void Register(ActiveManager manager)
        {
            base.Register(manager);
            if (Machine == null)
                Machine = StateMachines.Get<StateType>();
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

        private void Changed(object sender, StateChange<StateType> e)
        {
            ActiveManager.Check();
        }
    }
}