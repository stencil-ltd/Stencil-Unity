using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Util;

namespace Plugins.State
{
    public abstract class StateVisibilityLegacy<T> : MonoBehaviour, IStateListenerLegacy where T : struct
    {
        public static T State { get; private set; }
        public static event EventHandler<T> OnChange;
        public static void RequestState(T state, bool force = false, bool notify = true)
        {
            if (!force && state.Equals(State)) return;
            State = state;
            if (notify) Objects.OnMain(NotifyChanged);
        }
        
        public bool DestroyOnInactive;
        public bool Invert;
        public T[] States;

        private bool _registered;
        private bool _destroyed;

        public static void NotifyChanged()
        {
            Debug.Log($"<color=blue>{typeof(T).ShortName()}: {State}</color>");
            OnChange?.Invoke(null, State);
        }

        public StateVisibilityLegacy()
        {
            if (!typeof(T).IsEnum)
                throw new Exception("StateVisibility can only handle enums.");
        }

        private void Start() {
            Register();
        }

        public void Register()
        {
            if (_registered) return;
            _registered = true;
            OnChange += Changed;
            Changed(null, State);
        }

        private void OnDestroy()
        {
            _destroyed = true;
            OnChange -= Changed;
        }

        private void Changed(object sender, T e)
        {
            try {
                if (_destroyed) return;
                var visible = States.Contains(State);
                if (Invert) visible = !visible;
                gameObject.SetActive(visible);
                if (DestroyOnInactive && !visible)
                {
                    Destroy(gameObject);
                }                
            } catch (Exception) {}
        }
    }
}