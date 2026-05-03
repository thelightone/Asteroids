using UnityEngine;

namespace Game.Core
{
public class BulletModel
{
    public Vector2 Position { get; internal set; }
    public Vector2 Velocity { get; internal set; }

    public float LifeTime { get; internal set; }
    public float Radius { get; internal set; }

    public bool IsActive { get; internal set; }
}
}
