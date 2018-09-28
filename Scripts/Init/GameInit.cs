using System;
using System.Collections;
using Ads;
using CustomOrder;
using Dev;
using Firebase;
using Firebase.RemoteConfig;
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
        public static event EventHandler OnRemoteConfig;
        public bool Started { get; private set; }
        
        protected sealed override void Awake()
        {
            base.Awake();
            if (!Valid) return;
            Application.targetFrameRate = 60;
            gameObject.AddComponent<Gestures>();
            gameObject.AddComponent<GestureReport>();
            new GameObject("Main Thread Dispatch").AddComponent<UnityMainThreadDispatcher>();
            BuyableManager.Init();
            SceneManager.sceneLoaded += _OnNewScene;
            OnInit();
            SetupFirebase();
            SetupFacebook();
        }

        private static void SetupFacebook()
        {
#if !EXCLUDE_FACEBOOK
            Facebook.Unity.FB.Mobile.FetchDeferredAppLinkData();
            Facebook.Unity.FB.Init();
#endif
        }

        private void SetupFirebase()
        {
#if !EXCLUDE_FIREBASE
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                var success = dependencyStatus == DependencyStatus.Available;
                if (!success)
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");

                Objects.Enqueue(() =>
                {
                    if (success)
                    {
                        var settings = FirebaseRemoteConfig.Settings;
                        settings.IsDeveloperMode = Developers.Enabled;
                        FirebaseRemoteConfig.Settings = settings;
                        var cache = settings.IsDeveloperMode ? TimeSpan.Zero : TimeSpan.FromHours(12);
                        FirebaseRemoteConfig.FetchAsync(cache).ContinueWith(task1 =>
                        {
                            if (task1.IsFaulted) return;
                            FirebaseRemoteConfig.ActivateFetched();
                            Objects.Enqueue(() => OnRemoteConfig?.Invoke());
                        });
                    }

                    OnFirebase(success);
                });
            });
#endif
        }

        private void _OnNewScene(Scene arg0, LoadSceneMode arg1)
        {
            StencilAds.CheckReload();
            OnNewScene(arg0, arg1);
        }

        protected virtual void OnFirebase(bool success)
        {}

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