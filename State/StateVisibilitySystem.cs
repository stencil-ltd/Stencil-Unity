using System;
using TypeReferences;
using UnityEngine;

namespace Plugins.State
{
    public class StateVisibilitySystem : MonoBehaviour
    {
        [ClassImplementsAttribute(typeof(IStateVisibility))]
        public ClassTypeReference[] Types;
        
        private void Start()
        {
            foreach (var type in Types)
            {
                var t = type.Type;
                if (t.BaseType?.GetGenericTypeDefinition() != typeof(StateVisibility<>))
                    throw new Exception("Must extend StateVisibility.");
                
                var method = t.GetMethod("Register");
                foreach (var res in Resources.FindObjectsOfTypeAll(t))
                    method.Invoke(res, null);
            }
        }
    }
}