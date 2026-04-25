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
    private readonly LaserConfig _laserConfig;
    private readonly IInputReader _inputReader;
    private readonly WorldConfig _worldConfig;
    private readonly ShipMovementService _shipMovementService;
    private readonly WorldWrapService _worldWrapService;

    public float CollisionBounceForce => _playerConfig.CollisionBounceForce;

    public float LaserDistance => _laserConfig.Length;

    public Vector2 Forward => _shipMovementService.GetForward(ShipModel.Rotation);

    public PlayerService(PlayerConfig playerConfig, LaserConfig laserConfig, IInputReader inputReader, WorldConfig worldConfig,
        ShipMovementService shipMovementService, WorldWrapService worldWrapService)
    {
        _playerConfig = playerConfig;
        _laserConfig = laserConfig;
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
            MaxLaserCharges = _laserConfig.MaxCharges,
            LaserCharges = _laserConfig.MaxCharges,
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
            float turnDirection = input.HasDirectionalInput
                ? CalculateTurnDirectionToTarget(input.Direction, deltaTime)
                : input.TurnDirection;

            _shipMovementService.ApplyRotation(
                ShipModel,
                turnDirection,
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

    private float CalculateTurnDirectionToTarget(Vector2 desiredDirection, float deltaTime)
    {
        if (desiredDirection.sqrMagnitude <= 0f)
        {
            return 0f;
        }

        float targetRotation = Mathf.Atan2(-desiredDirection.x, desiredDirection.y) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(ShipModel.Rotation, targetRotation);
        float maxTurnStep = Mathf.Max(0.0001f, _playerConfig.RotationSpeed * deltaTime);

        return Mathf.Clamp(-deltaAngle / maxTurnStep, -1f, 1f);
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
                ShipModel.LaserCooldownLeft = _laserConfig.RechargeInterval;
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
            ShipModel.LaserCooldownLeft = _laserConfig.RechargeInterval;
        }

        NotifyLaserStateChanged();
        return true;
    }
}