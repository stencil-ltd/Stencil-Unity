using System.Collections;
using Ads;
using CustomOrder;
using Dev;
using Plugins.UI;
using Store;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Init
{
    [ExecutionOrder(-100)]
    public class GameInit : Permanent<GameInit>
    {
        public virtual bool ForceDevMode { get; } = false;
        public bool Started { get; private set; }
        
        protected sealed override void Awake()
        {
            base.Awake();
            if (!Valid) return;
            Developers.ForceEnabled = ForceDevMode;
            Application.targetFrameRate = 60;
            gameObject.AddComponent<Gestures>();
            gameObject.AddComponent<GestureReport>();
            new GameObject("Main Thread Dispatch").AddComponent<UnityMainThreadDispatcher>();
            BuyableManager.Init();
            SceneManager.sceneLoaded += _OnNewScene;
#if STENCIL_FIREBASE
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus != Firebase.DependencyStatus.Available)
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
#endif
            OnInit();
        }

        private void _OnNewScene(Scene arg0, LoadSceneMode arg1)
        {
            StencilAds.CheckReload();
            OnNewScene(arg0, arg1);
        }

        protected virtual void OnInit()
        {}

        protected virtual void OnSettled()
        {}

        protected virtual void OnNewScene(Scene arg0, LoadSceneMode loadSceneMode)
        {}

        protected virtual IEnumerator Start()
        {
            Started = true;
            yield return null;
            StencilAds.Init();
            OnSettled();
        }
    }
}