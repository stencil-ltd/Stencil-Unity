using UnityEngine.Events;

namespace Plugins.Util
{
    public static class UnityEventExtensions
    {
        public static void Once(this UnityEvent ev, UnityAction action)
        {
            UnityAction listener = null;
            listener = () =>
            {
                ev.RemoveListener(listener);
                action();
            };
            ev.AddListener(listener);
        }
        
        public static void Once<T0>(this UnityEvent<T0> ev, UnityAction<T0> action)
        {
            UnityAction<T0> listener = null;
            listener = arg0 =>
            {
                ev.RemoveListener(listener);
                action(arg0);
            };
            ev.AddListener(listener);
        }
        
        public static void Once<T0, T1>(this UnityEvent<T0, T1> ev, UnityAction<T0, T1> action)
        {
            UnityAction<T0, T1> listener = null;
            listener = (arg0, arg1) =>
            {
                ev.RemoveListener(listener);
                action(arg0, arg1);
            };
            ev.AddListener(listener);
        }
    }
}