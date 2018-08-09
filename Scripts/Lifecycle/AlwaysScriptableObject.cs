using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Lifecycle
{
    public class AlwaysScriptableObject : ScriptableObject
    {
        [MenuItem("Stencil/Always Loaded")]
        public static void Refresh()
        {
            var obj = GameObject.Find("Always Loaded");
            if (obj == null)
            {
                obj = new GameObject("Always Loaded");
                obj.AddComponent<AlwaysLoaded>();
            }

            var always = Resources.FindObjectsOfTypeAll<AlwaysScriptableObject>()
                .Select(it => (Object) it)
                .ToArray();
            obj.GetComponent<AlwaysLoaded>().Load = always;
        }
    }
}