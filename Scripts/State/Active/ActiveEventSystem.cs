using Plugins.State;
using TypeReferences;
using UnityEngine;
using System.Linq;
using Util;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
            foreach (var res in _managers)
                res.Register();
    }

    public void Check() 
    {
        foreach (var res in _managers)
            res.Check();   
    }
}