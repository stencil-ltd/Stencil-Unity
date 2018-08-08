using Binding;
using Plugins.UI;

namespace State
{
    public class StateMachineObject : Permanent<StateMachineObject>
    {
        protected override void Awake()
        {
            base.Awake();
            this.BindStates();
        }

        private void Start()
        {
            StateMachines.Initialize();
        }
    }
}