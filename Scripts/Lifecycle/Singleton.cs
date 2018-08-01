using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    /// <summary>
    /// Abstract class for making reload-proof singletons out of ScriptableObjects
    /// Returns the asset created on the editor, or null if there is none
    /// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
    /// </summary>
    /// <typeparam name="T">Singleton type</typeparam>
    public abstract class Singleton<T> : ScriptableObject where T : ScriptableObject {
        static T _instance = null;
        public static T Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                return _instance;
            }
        }

        protected virtual void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        protected virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }

        public void OnSceneLoad(Scene arg0, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            Debug.Log($"Singleton Awake: {typeof(T).Name}");
            ((Singleton<T>) (object) Instance).OnFirstLoad();
        }
        
        protected virtual void OnFirstLoad()
        {}        
    }
}