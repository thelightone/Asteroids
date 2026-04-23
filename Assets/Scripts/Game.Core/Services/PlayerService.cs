using System;
using UnityEngine;

public class PlayerService : IGameTickable
{
    public ShipModel ShipModel { get; }

    public PlayerInputData CurrentInput { get; private set; }

    public event Action<int> HealthChanged;
    public event Action Died;
    public event Action<int, float> LaserStateChanged;

    private readonly PlayerConfig _playerConfig;
    private readonly IInputReader _inputReader;
    private readonly WorldConfig _worldConfig;
    private readonly ShipMovementService _shipMovementService;
    private readonly WorldWrapService _worldWrapService;

    public float CollisionBounceForce => _playerConfig.CollisionBounceForce;

    public float LaserDistance => _playerConfig.LaserDistance;

    public Vector2 Forward => _shipMovementService.GetForward(ShipModel.Rotation);

    public PlayerService(PlayerConfig playerConfig, IInputReader inputReader, WorldConfig worldConfig,
        ShipMovementService shipMovementService, WorldWrapService worldWrapService)
    {
        _playerConfig = playerConfig;
        _inputReader = inputReader;

        ShipModel = new ShipModel
        {
            Position = UnityEngine.Vector2.zero,
            Velocity = UnityEngine.Vector2.zero,
            Rotation = 0f,
            Health = _playerConfig.MaxHealth,
            IsInvulnerable = false,
            InvulnerableTimeLeft = 0f,
            IsControlLocked = false,
            Radius = _playerConfig.Radius,
            MaxLaserCharges = _playerConfig.MaxLaserCharges,
            LaserCharges = _playerConfig.MaxLaserCharges,
            LaserCooldownLeft = 0f,
        };

        NotifyHealthChanged();

        _worldConfig = worldConfig;
        _shipMovementService = shipMovementService;
        _worldWrapService = worldWrapService;

        NotifyLaserStateChanged();
    }

    private void NotifyHealthChanged()
    {
        HealthChanged?.Invoke(ShipModel.Health);
    }

    public void Tick(float deltaTime)
    {
        UpdateInvulnerability(deltaTime);
        UpdateLaserCooldown(deltaTime);

        Vector2 forward = _shipMovementService.GetForward(ShipModel.Rotation);

        if (!ShipModel.IsControlLocked)
        {
            CurrentInput = _inputReader.GetInput();
            PlayerInputData input = CurrentInput;

            _shipMovementService.ApplyRotation(
                ShipModel,
                input.TurnDirection,
                _playerConfig.RotationSpeed,
                deltaTime
            );

            forward = _shipMovementService.GetForward(ShipModel.Rotation);

            if (input.Thrust > 0f)
            {
                _shipMovementService.ApplyAcceleration(
                    ShipModel,
                    forward,
                    _playerConfig.Acceleration,
                    input.Thrust,
                    deltaTime
                );
            }
            else
            {
                _shipMovementService.ApplyBrake(
                    ShipModel,
                    _playerConfig.BrakeFactor,
                    deltaTime
                );
            }
        }
        else
        {
            CurrentInput = default;
        }

        _shipMovementService.ClampSpeed(ShipModel, _playerConfig.MaxSpeed);
        _shipMovementService.Move(ShipModel, deltaTime);
        _worldWrapService.WrapPosition(ShipModel, _worldConfig);
    }

    private void UpdateInvulnerability(float deltaTime)
    {
        if (!ShipModel.IsInvulnerable)
        {
            return;
        }

        ShipModel.InvulnerableTimeLeft -= deltaTime;

        if (ShipModel.InvulnerableTimeLeft <= 0f)
        {
            ShipModel.InvulnerableTimeLeft = 0f;
            ShipModel.IsInvulnerable = false;

            if (!ShipModel.IsDead)
            {
                ShipModel.IsControlLocked = false;
            }
        }


    }

    public void ApplyDamage()
    {
        if (ShipModel.Health <= 0)
        {
            return;
        }

        if (ShipModel.IsInvulnerable)
        {
            return;
        }

        ShipModel.Health--;
        NotifyHealthChanged();

        Debug.Log("Player damaged. Health = " + ShipModel.Health);

        if (ShipModel.Health <= 0)
        {
            ShipModel.Health = 0;
            ShipModel.IsDead = true;
            ShipModel.IsControlLocked = true;
            ShipModel.Velocity = Vector2.zero;
            Died?.Invoke();

            Debug.Log("Player died");
            return;
        }

        ShipModel.IsInvulnerable = true;
        ShipModel.InvulnerableTimeLeft = _playerConfig.InvulnerabilityDuration;
        ShipModel.IsControlLocked = true;
    }

    public void ApplyBounce(Vector2 bounceDirection)
    {
        if (bounceDirection == Vector2.zero)
        {
            return;
        }

        ShipModel.Velocity = bounceDirection.normalized * _playerConfig.CollisionBounceForce;
    }

    private void NotifyLaserStateChanged()
    {
        LaserStateChanged?.Invoke(ShipModel.LaserCharges, ShipModel.LaserCooldownLeft);
    }

    private void UpdateLaserCooldown(float deltaTime)
    {
        if (ShipModel.LaserCharges >= ShipModel.MaxLaserCharges)
        {
            ShipModel.LaserCooldownLeft = 0f;
            return;
        }

        if (ShipModel.LaserCooldownLeft > 0f)
        {
            ShipModel.LaserCooldownLeft -= deltaTime;

            if (ShipModel.LaserCooldownLeft < 0f)
            {
                ShipModel.LaserCooldownLeft = 0f;
            }

            NotifyLaserStateChanged();
        }

        if (ShipModel.LaserCooldownLeft <= 0f)
        {
            ShipModel.LaserCharges++;

            if (ShipModel.LaserCharges > ShipModel.MaxLaserCharges)
            {
                ShipModel.LaserCharges = ShipModel.MaxLaserCharges;
            }

            if (ShipModel.LaserCharges < ShipModel.MaxLaserCharges)
            {
                ShipModel.LaserCooldownLeft = _playerConfig.LaserCooldownDuration;
            }
            else
            {
                ShipModel.LaserCooldownLeft = 0f;
            }

            NotifyLaserStateChanged();
        }
    }

    public bool TryUseLaser()
    {
        if (ShipModel.IsDead)
        {
            return false;
        }

        if (ShipModel.LaserCharges <= 0)
        {
            return false;
        }

        ShipModel.LaserCharges--;

        if (ShipModel.LaserCooldownLeft <= 0f)
        {
            ShipModel.LaserCooldownLeft = _playerConfig.LaserCooldownDuration;
        }

        NotifyLaserStateChanged();
        return true;
    }
}