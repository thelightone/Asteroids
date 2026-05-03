using UnityEngine;

namespace Game.Signals
{
public readonly struct LaserFiredSignal
{
    public readonly Vector2 Start;
    public readonly Vector2 End;
    public readonly float VisibleDuration;

    public LaserFiredSignal(Vector2 start, Vector2 end, float visibleDuration)
    {
        Start = start;
        End = end;
        VisibleDuration = visibleDuration;
    }
}
}
