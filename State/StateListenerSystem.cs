using System;
using TypeReferences;
using UnityEngine;

namespace Plugins.State
{
    public class StateListenerSystem : MonoBehaviour
    {
        [ClassImplements(typeof(IStateListener))]
        public ClassTypeReference[] Types;
        
        private void Start()
        {
            foreach (var type in Types)
            {
                var t = type.Type;
                var method = t.GetMethod("Register");
                foreach (var res in Resources.FindObjectsOfTypeAll(t))
                    method.Invoke(res, null);
            }
        }
    }
}