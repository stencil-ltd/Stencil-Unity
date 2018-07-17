using System;
public struct StateChange
{
    public readonly State Old;
    public readonly State New;

    public StateChange(State old, State @new)
    {
        Old = old;
        New = @new;
    }
}