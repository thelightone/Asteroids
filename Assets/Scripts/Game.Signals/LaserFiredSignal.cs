using UnityEngine;

public readonly struct LaserFiredSignal
{
    public readonly Vector2 Start;
    public readonly Vector2 End;

    public LaserFiredSignal(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;
    }
}