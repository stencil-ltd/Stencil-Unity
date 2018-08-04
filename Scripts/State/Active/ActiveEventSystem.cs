using System.Linq;
using UI;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace State.Active
{
    public class ActiveEventSystem : MonoBehaviour
    {
        private ActiveManager[] _managers;
        private RegisterableBehaviour[] _registers;
        
        public ScriptableObject[] PermanentObjects;

        private void Awake()
        {
            Debug.Log("ActiveEventSystem Awake");
            _managers = Resources.FindObjectsOfTypeAll<ActiveManager>();
            _registers = Resources.FindObjectsOfTypeAll<RegisterableBehaviour>();
#if UNITY_EDITOR
            _managers = _managers.Where((arg) => !EditorUtility.IsPersistent(arg))
                .ToArray();
            _registers = _registers.Where((arg) => !EditorUtility.IsPersistent(arg))
                .ToArray();
#endif
            if (Application.isPlaying)
            {
                foreach (var res in _registers)
                    res.Register();
                foreach (var res in _managers)
                    res.Register();
                foreach (var res in _registers)
                    res.DidRegister();
            }
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                foreach (var res in _registers)
                    res.WillUnregister();
                foreach (var res in _managers)
                    res.Unregister();
                foreach (var res in _registers)
                    res.Unregister();
            }            
        }

        public void Check() 
        {
            foreach (var res in _managers)
                res.Check();   
        }
    }
}