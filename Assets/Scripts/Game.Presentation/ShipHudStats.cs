using UnityEngine;

namespace Game.Presentation
{
public readonly struct ShipHudStats
{
    public Vector2 Position { get; }
    public float Rotation { get; }
    public float Speed { get; }
    public int LaserCharges { get; }
    public int MaxLaserCharges { get; }
    public float LaserCooldown { get; }

    public ShipHudStats(
        Vector2 position,
        float rotation,
        float speed,
        int laserCharges,
        int maxLaserCharges,
        float laserCooldown)
    {
        Position = position;
        Rotation = rotation;
        Speed = speed;
        LaserCharges = laserCharges;
        MaxLaserCharges = maxLaserCharges;
        LaserCooldown = laserCooldown;
    }
}
}
