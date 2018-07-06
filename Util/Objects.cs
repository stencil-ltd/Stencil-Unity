using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util
{
    public static class Objects
    {
        public static bool IsMainThread => UnityMainThreadDispatcher.IsMainThread;

        public static string ShortName(this Type t)
        {
            return t.Name.Split('.').Last();
        }
        
        public static void Configure()
        {
            new GameObject("Main Thread Dispatch")
                .AddComponent<UnityMainThreadDispatcher>();
        }
        
        public static void Enqueue(this object any, Action action)
        {
            Enqueue(action);
        }

        public static void StartCoroutine(IEnumerator coroutine)
        {
            UnityMainThreadDispatcher.Instance().StartCoroutine(coroutine);
            
        }

        public static void OnMain(Action Action)
        {
            if (IsMainThread)
                Action();
            else Enqueue(Action);
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
        
        public static void DestroyAllChildren(this Transform transform)
        {
            foreach (var child in transform.GetChildren())
                Object.Destroy(child.gameObject);
            transform.DetachChildren();
        }
        
        public static RectTransform[] GetChildren(this RectTransform transform)
        {
            var retval = new RectTransform[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                retval[i] = (RectTransform) transform.GetChild(i);
            }
            return retval;
        }
        
        public static Transform[] GetChildren(this Transform transform)
        {
            var retval = new Transform[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                retval[i] = transform.GetChild(i);
            }
            return retval;
        }
    }
}