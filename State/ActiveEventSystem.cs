using Plugins.State;
using TypeReferences;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ActiveEventSystem : MonoBehaviour
{
        private void Awake()
        {
                Debug.Log("ActiveEventSystem Awake");
                foreach (var res in Resources.FindObjectsOfTypeAll<ActiveManager>())
                {
                        #if UNITY_EDITOR
                        if (EditorUtility.IsPersistent(res)) continue;
                        #endif
                        res.Register();
                }
        }
}