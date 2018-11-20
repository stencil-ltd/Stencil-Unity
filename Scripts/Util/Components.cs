using UnityEngine;

namespace Util
{
    public static class Components
    {
        public static T GetOrAddComponent<T>(this Component component) where T : Component 
            => component.GetComponent<T>() ?? component.gameObject.AddComponent<T>();
    }
}