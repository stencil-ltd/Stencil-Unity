using System;
using System.Linq;
using UnityEngine;

namespace Plugins.State
{
    public abstract class StateVisibility<T> : MonoBehaviour where T : struct
    {
        public static T State { get; private set; }
        public static event EventHandler<T> OnChange;
        public static void RequestState(T state, bool force = false)
        {
            if (!force && state.Equals(State)) return;
            State = state;
            OnChange?.Invoke(null, State);
        }

        public T[] States;
        public bool Invert;
        
        public StateVisibility()
        {
            if (!typeof(T).IsEnum)
                throw new Exception("StateVisibility can only handle enums.");
        }

        public void Register()
        {
            OnChange += UpdateVisibility;
            UpdateVisibility(null, State);
        }

        private void OnDestroy()
        {
            OnChange -= UpdateVisibility;
        }

        private void UpdateVisibility(object sender, T e)
        {
            var visible = States.Contains(State);
            if (Invert) visible = !visible;
            gameObject.SetActive(visible);
        }
    }
}