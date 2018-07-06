using UnityEngine;

namespace Plugins.UI
{
    public abstract class Controller<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void ControllerAwake()
        {}

        protected void Awake()
        {
            Instance = (T) (object) this;
            ControllerAwake();
        }

        protected virtual void ControllerDestroyed()
        {}

        protected void OnDestroy()
        {
            Instance = Instance == this ? null : Instance;
            ControllerDestroyed();
        }
    }
}