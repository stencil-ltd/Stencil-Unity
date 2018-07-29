using UnityEngine;

namespace Plugins.UI
{
    public abstract class Controller<T> : MonoBehaviour where T : Controller<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = (T) this;
        }

        protected virtual void OnDestroy()
        {
            Instance = Instance == this ? null : Instance;
        }
    }
}