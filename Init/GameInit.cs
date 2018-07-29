using System.Collections;
using Plugins.UI;
using UI;
using UnityEngine;

namespace Plugins.Init
{
    public class GameInit : Permanent<GameInit>
    {
        public bool Started { get; private set; }
        
        protected sealed override void Awake()
        {
            base.Awake();
            if (!Valid) return;
            Application.targetFrameRate = 60;
            gameObject.AddComponent<Gestures>();
            gameObject.AddComponent<GestureReport>();
            OnInit();
        }
        
        protected virtual void OnInit()
        {}

        protected virtual void OnSettled()
        {}
        
        protected virtual IEnumerator Start()
        {
            Started = true;
            yield return null;
            OnSettled();
        }
    }
}