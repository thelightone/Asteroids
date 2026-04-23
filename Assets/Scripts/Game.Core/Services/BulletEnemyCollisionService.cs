using UnityEngine;
using Zenject;

public class BulletEnemyCollisionService : IGameTickable
{
    private readonly BulletCollectionService _bulletCollectionService;
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly EnemySpawnService _enemySpawnService;
    private readonly SignalBus _signalBus;

    public BulletEnemyCollisionService(
    BulletCollectionService bulletCollectionService,
    EnemyCollectionService enemyCollectionService,
    EnemySpawnService enemySpawnService,
    SignalBus signalBus)
    {
        _bulletCollectionService = bulletCollectionService;
        _enemyCollectionService = enemyCollectionService;
        _enemySpawnService = enemySpawnService;
        _signalBus = signalBus;
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
                    _signalBus.Fire(new EnemyDestroyedSignal((int)enemy.Type));

                    if (enemy.Type == EnemyType.AsteroidLarge)
                    {
                        _enemySpawnService.SplitLargeAsteroid(enemy);
                    }

                    bullet.IsActive = false;
                    enemy.IsActive = false;
                    return;
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