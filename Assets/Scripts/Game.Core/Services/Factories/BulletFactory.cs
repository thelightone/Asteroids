using UnityEngine;

namespace Game.Core
{
public class BulletFactory
{
    public BulletModel Create(Vector2 position, Vector2 velocity, float lifeTime, float radius)
    {
        return new BulletModel
        {
            Position = position,
            Velocity = velocity,
            LifeTime = lifeTime,
            Radius = radius,
            IsActive = true
        };
    }
}
}
