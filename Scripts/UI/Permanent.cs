using UnityEngine;

namespace Plugins.UI
{
    public class Permanent<T> : MonoBehaviour where T : Permanent<T>
    { 
        public static T Instance { get; private set; }
        protected bool Valid;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Valid = true;
            Instance = (T) this;
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}