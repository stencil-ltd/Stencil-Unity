using UnityEngine;

namespace UI
{
    public abstract class RegisterableBehaviour : MonoBehaviour
    {
        public bool Registered { get; internal set; }
        public bool Unregistered { get; internal set; }

        protected virtual void Awake()
        {
            if (!Registered)
            {
                Register();
                Registered = true;
                DidRegister();
            }
        }

        protected virtual void OnDestroy()
        {
            if (!Unregistered)
            {
                WillUnregister();
                Unregister();
                Unregistered = true;
            }
        }

        public virtual void Register() {}
        public virtual void DidRegister() {}
        
        public virtual void Unregister() {}        
        public virtual void WillUnregister() {}
    }
}
