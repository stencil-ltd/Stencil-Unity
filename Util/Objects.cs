using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util
{
    public static class Objects
    {
        public static void Enqueue(this object any, Action action)
        {
            Enqueue(action);
        }

        public static void StartCoroutine(IEnumerator coroutine)
        {
            UnityMainThreadDispatcher.Instance().StartCoroutine(coroutine);
            
        }
        
        public static void Enqueue(Action action)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(action);
        }
        
        public static void Invoke([CanBeNull] this EventHandler handler, object sender = null)
        {
            handler?.Invoke(sender, EventArgs.Empty);
        }

        public static T SafeDestroy<T>(T obj) where T : Object
        {
            if (Application.isEditor)
                Object.DestroyImmediate(obj);
            else
                Object.Destroy(obj);
     
            return null;
        } 
        
        public static T SafeDestroyGameObject<T>(T component) where T : Component
        {
            if (component != null)
                SafeDestroy(component.gameObject);
            return null;
        }
    }
}