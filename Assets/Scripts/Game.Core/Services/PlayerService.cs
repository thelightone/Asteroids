using System;
using UnityEngine;

namespace Game.Core
{
public class PlayerService
{
    public ShipModel ShipModel { get; }

    public event Action<int> HealthChanged
    {
        add => _health.HealthChanged += value;
        remove => _health.HealthChanged -= value;
    }

    public event Action Died
    {
        add => _health.Died += value;
        remove => _health.Died -= value;
    }

    public event Action<bool> InvulnerabilityChanged
    {
        add => _health.InvulnerabilityChanged += value;
        remove => _health.InvulnerabilityChanged -= value;
    }

    public event Action<int, float> LaserStateChanged
    {
        add => _laser.LaserStateChanged += value;
        remove => _laser.LaserStateChanged -= value;
    }

    private readonly PlayerMovementService _movement;
    private readonly PlayerHealthService _health;
    private readonly LaserChargeService _laser;

    public float CollisionBounceForce => _movement.CollisionBounceForce;

    public float LaserDistance => _laser.LaserDistance;

    public Vector2 Forward => _movement.GetForward();

    public PlayerService(
        PlayerConfig playerConfig,
        LaserConfig laserConfig,
        IInputReader inputReader,
        WorldConfig worldConfig,
        ShipMovementService shipMovementService,
        WorldWrapService worldWrapService)
    {
        ShipModel = new ShipModel
        {
            Position = Vector2.zero,
            Velocity = Vector2.zero,
            Rotation = 0f,
            Health = playerConfig.MaxHealth,
            IsInvulnerable = false,
            InvulnerableTimeLeft = 0f,
            IsControlLocked = false,
            Radius = playerConfig.Radius,
            MaxLaserCharges = laserConfig.MaxCharges,
            LaserCharges = laserConfig.MaxCharges,
            LaserCooldownLeft = 0f,
            LaserFireCooldownLeft = 0f,
        };

        _health = new PlayerHealthService(ShipModel, playerConfig);
        _movement = new PlayerMovementService(
            ShipModel,
            playerConfig,
            inputReader,
            worldConfig,
            shipMovementService,
            worldWrapService);
        _laser = new LaserChargeService(ShipModel, laserConfig);
    }

    public PlayerInputData Tick(float deltaTime)
    {
        _health.Tick(deltaTime);
        _laser.Tick(deltaTime);
        return _movement.Tick(deltaTime);
    }

    public void ApplyDamage()
    {
        _health.ApplyDamage();
    }

    public void ApplyBounce(Vector2 bounceDirection)
    {
        _movement.ApplyBounce(bounceDirection);
    }

    public bool TryUseLaser()
    {
        return _laser.TryUseLaser();
    }

    public void PublishPresentationState()
    {
        _health.PublishHealthState();
        _laser.PublishLaserState();
    }
}
}
