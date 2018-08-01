using UnityEngine;

namespace UI
{
    public abstract class RegisterableBehaviour : MonoBehaviour
    {
        public abstract void Register();
        public abstract void Unregister();
    }
}
