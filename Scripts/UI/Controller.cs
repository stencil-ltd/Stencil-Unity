using UnityEngine;

namespace UI
{
    public abstract class Controller<T> : RegisterableBehaviour where T : Controller<T>
    {
        public static T Instance { get; private set; }

        public override void Register()
        {
            Debug.Log($"Register {this}");
            Instance = (T)this;
        }

        protected virtual void Awake()
        {
            Register();
        }

        public override void Unregister()
        {}

        protected virtual void OnDestroy()
        {
            Instance = Instance == this ? null : Instance;
        }
    }
}