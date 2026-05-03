using UnityEngine;

namespace Game.Core
{
public class BulletMovementService : IGameTickable
{
    private readonly BulletCollectionService _bulletCollectionService;

    public BulletMovementService(BulletCollectionService bulletCollectionService)
    {
        _bulletCollectionService = bulletCollectionService;
    }

    public void Tick(float deltaTime)
    {
        for (int i = _bulletCollectionService.Bullets.Count - 1; i >= 0; i--)
        {
            BulletModel bullet = _bulletCollectionService.Bullets[i];

            if (!bullet.IsActive)
            {
                _bulletCollectionService.Remove(bullet);
                continue;
            }

            bullet.Position += bullet.Velocity * deltaTime;
            bullet.LifeTime -= deltaTime;

            if (bullet.LifeTime <= 0f)
            {
                bullet.IsActive = false;
                _bulletCollectionService.Remove(bullet);
            }
        }
    }
}
}
