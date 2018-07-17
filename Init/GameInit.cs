﻿using Plugins.UI;
using UI;
using UnityEngine;
using Util;

namespace Plugins.Init
{
    public class GameInit : Permanent<GameInit>
    {
        protected sealed override void Awake()
        {
            base.Awake();
            if (!Valid) return;
            Application.targetFrameRate = 60;
            new GameObject("Main Thread Dispatch")
                .AddComponent<UnityMainThreadDispatcher>();
            gameObject.AddComponent<Gestures>();
            gameObject.AddComponent<GestureReport>();
            OnInit();
        }
        
        protected virtual void OnInit()
        {}
    }
}