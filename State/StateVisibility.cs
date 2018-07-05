using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Util;

namespace Plugins.State
{
    public abstract class StateVisibility<T> : MonoBehaviour, IStateListener where T : struct
    {
        public static T State { get; private set; }
        public static event EventHandler<T> OnChange;
        public static void RequestState(T state, bool force = false, bool notify = true)
        {
            if (!force && state.Equals(State)) return;
            State = state;
            if (notify) Objects.OnMain(NotifyChanged);
        }
        
        public bool Invert;
        public T[] States;

        public static void NotifyChanged()
        {
            OnChange?.Invoke(null, State);
        }

        public StateVisibility()
        {
            if (!typeof(T).IsEnum)
                throw new Exception("StateVisibility can only handle enums.");
        }

        public void Register()
        {
            OnChange += Changed;
            Changed(null, State);
        }

        private void OnDestroy()
        {
            OnChange -= Changed;
        }

        private void Changed(object sender, T e)
        {
            var visible = States.Contains(State);
            if (Invert) visible = !visible;
            gameObject.SetActive(visible);
        }
    }
}