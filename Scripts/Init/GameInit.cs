using System.Collections;
using Dev;
using Plugins.UI;
using Store;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Init
{
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
            SceneManager.sceneLoaded += OnNewScene;
            OnInit();
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
            OnSettled();
        }
    }
}