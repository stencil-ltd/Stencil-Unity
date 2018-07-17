using System;
using UnityEngine;

[CreateAssetMenu(menuName = "State/State")]
public class State : ScriptableObject
{
    public string Name;

    public override string ToString()
    {
        return Name;
    }
}
