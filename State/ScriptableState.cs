using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/State")]
public class ScriptableState : ScriptableObject
{
    public string Name;

    public override string ToString()
    {
        return Name;
    }
}
