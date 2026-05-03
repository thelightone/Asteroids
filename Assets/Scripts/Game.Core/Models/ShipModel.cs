using UnityEngine;

namespace Game.Core
{
public class ShipModel
{
    public Vector2 Position { get; internal set; }
    public Vector2 Velocity { get; internal set; }

    public float Rotation { get; internal set; }

    public int Health { get; internal set; }
    public bool IsInvulnerable { get; internal set; }
    public float InvulnerableTimeLeft { get; internal set; }

    public bool IsControlLocked { get; internal set; }

    public float Radius { get; internal set; }

    public bool IsDead { get; internal set; }

    public int LaserCharges { get; internal set; }
    public int MaxLaserCharges { get; internal set; }
    public float LaserCooldownLeft { get; internal set; }
    public float LaserFireCooldownLeft { get; internal set; }
}
}
