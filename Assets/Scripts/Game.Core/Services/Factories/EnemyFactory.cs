using UnityEngine;

namespace Game.Core
{
public class EnemyFactory
{
    public EnemyModel CreateLargeAsteroid(Vector2 position, Vector2 velocity, float radius)
    {
        return new EnemyModel
        {
            Type = EnemyType.AsteroidLarge,
            Position = position,
            Velocity = velocity,
            Rotation = 0f,
            Radius = radius,
            IsActive = true
        };
    }

    public EnemyModel CreateSmallAsteroid(Vector2 position, Vector2 velocity, float radius)
    {
        return new EnemyModel
        {
            Type = EnemyType.AsteroidSmall,
            Position = position,
            Velocity = velocity,
            Rotation = 0f,
            Radius = radius,
            IsActive = true
        };
    }

    public EnemyModel CreateUfo(Vector2 position, Vector2 velocity, float radius)
    {
        return new EnemyModel
        {
            Type = EnemyType.Ufo,
            Position = position,
            Velocity = velocity,
            Rotation = 0f,
            Radius = radius,
            IsActive = true
        };
    }
}
}
