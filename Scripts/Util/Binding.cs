using UnityEngine;

namespace Util
{
    public static class Binding
    {
        public static void BindMany<T>(
            this Component any, 
            ref T[] field)
        {
            field = any.GetComponentsInChildren<T>();
        }
        
        public static void Bind<T>(
            this Component any, 
            ref T field)
        {
            field = any.GetComponent<T>();
        }
    }
}