using System;
using UnityEditor;
using UnityEngine;

namespace Plugins.State
{
    public class StateVisibilitySystem : MonoBehaviour
    {
        public MonoScript[] Scripts;
        
        private void Start()
        {
            foreach (var script in Scripts)
            {
                var t = script.GetClass();
                if (t.BaseType?.GetGenericTypeDefinition() != typeof(StateVisibility<>))
                    throw new Exception("Must extend StateVisibility.");
                
                var method = t.GetMethod("Register");
                foreach (var res in Resources.FindObjectsOfTypeAll(t))
                    method.Invoke(res, null);
            }
        }
    }
}