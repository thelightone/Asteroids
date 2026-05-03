using UnityEngine;

namespace Game.Core
{
public class ShipMovementService
{
    public void ApplyRotation(ShipModel shipModel, float turnDirection, float rotationSpeed, float deltaTime)
    {
        shipModel.Rotation -= turnDirection * rotationSpeed * deltaTime;
    }

    public Vector2 GetForward(float rotation)
    {
        float rotationInRadians = rotation * Mathf.Deg2Rad;

        return new Vector2(
            -Mathf.Sin(rotationInRadians),
            Mathf.Cos(rotationInRadians)
        );
    }

    public void ApplyAcceleration(ShipModel shipModel, Vector2 forward, float acceleration, float thrust, float deltaTime)
    {
        shipModel.Velocity += forward * (acceleration * thrust * deltaTime);
    }

    public void ApplyBrake(ShipModel shipModel, float brakeFactor, float deltaTime)
    {
        shipModel.Velocity = Vector2.Lerp(
            shipModel.Velocity,
            Vector2.zero,
            brakeFactor * deltaTime
        );

        if (shipModel.Velocity.magnitude < 0.01f)
        {
            shipModel.Velocity = Vector2.zero;
        }
    }

    public void ClampSpeed(ShipModel shipModel, float maxSpeed)
    {
        if (shipModel.Velocity.magnitude > maxSpeed)
        {
            shipModel.Velocity = shipModel.Velocity.normalized * maxSpeed;
        }
    }

    public void Move(ShipModel shipModel, float deltaTime)
    {
        shipModel.Position += shipModel.Velocity * deltaTime;
    }
}
}
