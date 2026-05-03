using UnityEngine;

namespace Game.Core
{
public class BulletEnemyCollisionService : IGameTickable
{
    private readonly BulletCollectionService _bulletCollectionService;
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly EnemyDestroyService _enemyDestroyService;

    public BulletEnemyCollisionService(
        BulletCollectionService bulletCollectionService,
        EnemyCollectionService enemyCollectionService,
        EnemyDestroyService enemyDestroyService)
    {
        _bulletCollectionService = bulletCollectionService;
        _enemyCollectionService = enemyCollectionService;
        _enemyDestroyService = enemyDestroyService;
    }

    public void Tick(float deltaTime)
    {
        for (int bulletIndex = 0; bulletIndex < _bulletCollectionService.Bullets.Count; bulletIndex++)
        {
            BulletModel bullet = _bulletCollectionService.Bullets[bulletIndex];

            if (!bullet.IsActive)
            {
                continue;
            }

            for (int enemyIndex = 0; enemyIndex < _enemyCollectionService.Enemies.Count; enemyIndex++)
            {
                EnemyModel enemy = _enemyCollectionService.Enemies[enemyIndex];

                if (!enemy.IsActive)
                {
                    continue;
                }

                if (HasCollision(bullet, enemy))
                {
                    _enemyDestroyService.DestroyEnemy(enemy);
                    bullet.IsActive = false;
                    break;
                }
            }
        }
    }

    private bool HasCollision(BulletModel bullet, EnemyModel enemy)
    {
        float distance = Vector2.Distance(bullet.Position, enemy.Position);
        float combinedRadius = bullet.Radius + enemy.Radius;

        return distance <= combinedRadius;
    }
}
}
