using UnityEngine;

namespace Game.Core
{
public class PlayerMovementService
{
    private readonly ShipModel _ship;
    private readonly PlayerConfig _playerConfig;
    private readonly IInputReader _inputReader;
    private readonly WorldConfig _worldConfig;
    private readonly ShipMovementService _shipMovementService;
    private readonly WorldWrapService _worldWrapService;

    public float CollisionBounceForce => _playerConfig.CollisionBounceForce;

    public PlayerMovementService(
        ShipModel ship,
        PlayerConfig playerConfig,
        IInputReader inputReader,
        WorldConfig worldConfig,
        ShipMovementService shipMovementService,
        WorldWrapService worldWrapService)
    {
        _ship = ship;
        _playerConfig = playerConfig;
        _inputReader = inputReader;
        _worldConfig = worldConfig;
        _shipMovementService = shipMovementService;
        _worldWrapService = worldWrapService;
    }

    public Vector2 GetForward()
    {
        return _shipMovementService.GetForward(_ship.Rotation);
    }

    public PlayerInputData Tick(float deltaTime)
    {
        Vector2 forward = _shipMovementService.GetForward(_ship.Rotation);
        PlayerInputData input;

        if (!_ship.IsControlLocked)
        {
            input = _inputReader.GetInput();
            float turnDirection = input.HasDirectionalInput
                ? CalculateTurnDirectionToTarget(input.Direction, deltaTime)
                : input.TurnDirection;

            _shipMovementService.ApplyRotation(
                _ship,
                turnDirection,
                _playerConfig.RotationSpeed,
                deltaTime
            );

            forward = _shipMovementService.GetForward(_ship.Rotation);

            if (input.Thrust > 0f)
            {
                _shipMovementService.ApplyAcceleration(
                    _ship,
                    forward,
                    _playerConfig.Acceleration,
                    input.Thrust,
                    deltaTime
                );
            }
            else
            {
                _shipMovementService.ApplyBrake(
                    _ship,
                    _playerConfig.BrakeFactor,
                    deltaTime
                );
            }
        }
        else
        {
            input = default;
        }

        _shipMovementService.ClampSpeed(_ship, _playerConfig.MaxSpeed);
        _shipMovementService.Move(_ship, deltaTime);
        _worldWrapService.WrapPosition(_ship, _worldConfig);

        return input;
    }

    public void ApplyBounce(Vector2 bounceDirection)
    {
        if (bounceDirection == Vector2.zero)
        {
            return;
        }

        _ship.Velocity = bounceDirection.normalized * _playerConfig.CollisionBounceForce;
    }

    private float CalculateTurnDirectionToTarget(Vector2 desiredDirection, float deltaTime)
    {
        if (desiredDirection.sqrMagnitude <= 0f)
        {
            return 0f;
        }

        float targetRotation = Mathf.Atan2(-desiredDirection.x, desiredDirection.y) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(_ship.Rotation, targetRotation);
        float maxTurnStep = Mathf.Max(0.0001f, _playerConfig.RotationSpeed * deltaTime);

        return Mathf.Clamp(-deltaAngle / maxTurnStep, -1f, 1f);
    }
}
}
