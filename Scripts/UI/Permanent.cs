using UnityEngine;

namespace Plugins.UI
{
    public class Permanent<T> : MonoBehaviour where T : Permanent<T>
    { 
        public static T Instance { get; private set; }
        protected bool Valid;

        protected virtual void Awake()
        {
            if (Instance != null && Application.isPlaying)
            {
                Destroy(gameObject);
                return;
            }

            Valid = true;
            Instance = (T) this;
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}