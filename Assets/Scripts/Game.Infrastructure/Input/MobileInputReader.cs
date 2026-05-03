using Game.Core;
using UnityEngine;

namespace Game.Infrastructure
{
public class MobileInputReader : IInputReader
{
    private const float DeadZone = 0.1f;

    private Vector2 _movement;
    private bool _fireBulletQueued;
    private bool _fireLaserQueued;

    public void SetMovement(Vector2 joystickDirection)
    {
        _movement = Vector2.ClampMagnitude(joystickDirection, 1f);
    }

    public void QueueFireBullet()
    {
        _fireBulletQueued = true;
    }

    public void QueueFireLaser()
    {
        _fireLaserQueued = true;
    }

    public PlayerInputData GetInput()
    {
        float intensity = _movement.magnitude;
        bool hasDirection = intensity > DeadZone;
        Vector2 direction = hasDirection ? _movement / intensity : Vector2.zero;

        PlayerInputData input = new PlayerInputData
        {
            TurnDirection = 0f,
            Thrust = hasDirection ? intensity : 0f,
            HasDirectionalInput = hasDirection,
            Direction = direction,
            FireBullet = _fireBulletQueued,
            FireLaser = _fireLaserQueued
        };

        _fireBulletQueued = false;
        _fireLaserQueued = false;

        return input;
    }
}

}
