using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace State.Active
{
    public class ActiveEventSystem : MonoBehaviour
    {
        private ActiveManager[] _managers;
        public ScriptableObject[] PermanentObjects;

        private void Awake()
        {
            Debug.Log("ActiveEventSystem Awake");
            _managers = Resources.FindObjectsOfTypeAll<ActiveManager>();
#if UNITY_EDITOR
            _managers = _managers.Where((arg) => !EditorUtility.IsPersistent(arg))
                .ToArray();
#endif



            if (Application.isPlaying)
            {
                foreach (var res in _managers)
                    res.Register();

                var registers = Resources.FindObjectsOfTypeAll<RegisterableBehaviour>();
                foreach (var res in registers)
                    res.Register();
            }
        }

        public void Check() 
        {
            foreach (var res in _managers)
                res.Check();   
        }
    }
}