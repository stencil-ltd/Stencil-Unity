using System.Collections.Generic;
using UnityEngine;

namespace Plugins.State
{
    public enum Operation
    {
        And, Or
    }

    public class ActiveManager : MonoBehaviour
    {
        public Operation Op = Operation.Or;

        public readonly List<ActiveGate> Gates = new List<ActiveGate>();
        
        protected bool IsRegistered { get; private set; }

        private void Awake() 
        {
            Register();
        }

        public void Register()
        {
            if (IsRegistered) return;
            IsRegistered = true;
            Gates.AddRange(GetComponents<ActiveGate>());
            foreach(var g in Gates)
                g.Register(this);
            Check();
        }

        public void Check() 
        {
            if (Gates.Count == 0) return;
            var active = Op == Operation.And;
            foreach(var g in Gates) 
            {
                var check = g.Check();
                if (check == null) continue;
                switch(Op)
                {
                    case Operation.And:
                        active &= check.Value;
                        break;
                    case Operation.Or:
                        active |= check.Value;
                        break;
                }
            }
            gameObject.SetActive(active);
        }

        private void OnDestroy() 
        {
            foreach(var g in Gates)
                g.Unregister();
        }
    }
}