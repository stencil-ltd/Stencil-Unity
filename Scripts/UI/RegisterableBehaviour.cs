using UnityEngine;

namespace UI
{
    public abstract class RegisterableBehaviour : MonoBehaviour
    {
        public abstract void Register();
        public virtual void DidRegister() {}
        
        public abstract void Unregister();        
        public virtual void WillUnregister() {}
    }
}
