using UnityEngine;

namespace Game.Core
{
public class EnemyModel
{
    public EnemyType Type { get; internal set; }

    public Vector2 Position { get; internal set; }
    public Vector2 Velocity { get; internal set; }

    public float Rotation { get; internal set; }
    public float Radius { get; internal set; }

    public bool IsActive { get; internal set; }

    public float BounceTimeLeft { get; internal set; }
}
}
