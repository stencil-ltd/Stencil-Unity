using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Binding;
using System;

public enum StateTextType
{
    All, Prefix, Suffix
}

[RequireComponent(typeof(Text))]
public class StateText : MonoBehaviour
{
    public StateTextType StateTextType;
    public StateMachine StateMachine;

    [Bind] private Text _text;

    private void Awake()
    {
        this.Bind();
        StateMachine.OnChange += OnState;
    }

    private void OnDestroy()
    {
        StateMachine.OnChange -= OnState;   
    }

    private void OnState(object sender, StateChange e)
    {
        var text = _text.text;
        if (e.Old != null)
            text = StripOld(text, e.Old);
        text = AppendNew(text, e.New);
        _text.text = text;
    }

    private string AppendNew(string text, State @new)
    {
        var newName = @new.Name;
        switch (StateTextType)
        {
            case StateTextType.All:
                return newName;
            case StateTextType.Prefix:
                return $"{newName} {text}";
            case StateTextType.Suffix:
                return $"{text} {newName}";
            default:
                throw new Exception("Wat");
        }
    }

    private string StripOld(string text, State old)
    {
        var oldName = old.Name;
        switch (StateTextType)
        {
            case StateTextType.Prefix:
                text = text.Replace($"{oldName} ", "");
                break;
            case StateTextType.Suffix:
                text = text.Replace($" {oldName}", "");
                break;
        }
        return text;
    }

    // Use this for initialization
    void Start()
    {
        OnState(null, new StateChange(StateMachine.State, StateMachine.State));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
