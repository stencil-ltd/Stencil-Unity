using System;
using TypeReferences;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Plugins.State
{
    public class StateListenerSystem : MonoBehaviour
    {
        [ClassImplements(typeof(IStateListenerLegacy))]
        public ClassTypeReference[] Types;
        
        private void Awake()
        {
            foreach (var type in Types)
            {
                var t = type.Type;
                var method = t.GetMethod("Register");
                foreach (var res in Resources.FindObjectsOfTypeAll(t))
                {
                    #if UNITY_EDITOR
                    if (EditorUtility.IsPersistent(res)) continue;
                    #endif
                    method.Invoke(res, null);
                }
            }
        }
    }
}