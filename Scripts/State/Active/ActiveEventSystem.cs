using System.Linq;
using CustomOrder;
using UI;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace State.Active
{
    [ExecutionOrder(-20)]
    public class ActiveEventSystem : MonoBehaviour
    {
        private RegisterableBehaviour[] _registers;
        
        private void Awake()
        {
            Debug.Log("ActiveEventSystem Awake");
            _registers = Resources.FindObjectsOfTypeAll<RegisterableBehaviour>();
#if UNITY_EDITOR
            _registers = _registers.Where((arg) => !EditorUtility.IsPersistent(arg))
                .ToArray();
#endif
            if (Application.isPlaying)
            {
                foreach (var res in _registers)
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
                foreach (var res in _registers)
                    res.Unregister();
            }            
        }

        public void Check() 
        {
            foreach (var res in _registers)
                (res as ActiveManager)?.Check();   
        }
    }
}