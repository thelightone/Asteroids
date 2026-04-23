public class ShipModel
{
    public UnityEngine.Vector2 Position;
    public UnityEngine.Vector2 Velocity;

    public float Rotation; // угол в градусах

    public int Health;
    public bool IsInvulnerable;
    public float InvulnerableTimeLeft;

    public bool IsControlLocked;

    public float Radius;

    public bool IsDead;

    public int LaserCharges;
    public int MaxLaserCharges;
    public float LaserCooldownLeft;
}