using UnityEngine;
using Zenject;

public class LaserService
{
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly EnemySpawnService _enemySpawnService;
    private readonly SignalBus _signalBus;

    public LaserService(
        EnemyCollectionService enemyCollectionService,
        EnemySpawnService enemySpawnService,
        SignalBus signalBus)
    {
        _enemyCollectionService = enemyCollectionService;
        _enemySpawnService = enemySpawnService;
        _signalBus = signalBus;
    }

    public void FireLaser(Vector2 origin, Vector2 direction, float maxDistance)
    {
        direction = direction.normalized;

        for (int i = 0; i < _enemyCollectionService.Enemies.Count; i++)
        {
            EnemyModel enemy = _enemyCollectionService.Enemies[i];

            if (!enemy.IsActive)
            {
                continue;
            }

            if (IsEnemyHitByLaser(origin, direction, maxDistance, enemy))
            {
                _signalBus.Fire(new EnemyDestroyedSignal((int)enemy.Type));

                if (enemy.Type == EnemyType.AsteroidLarge)
                {
                    _enemySpawnService.SplitLargeAsteroid(enemy);
                }

                enemy.IsActive = false;
            }
        }
    }

    private bool IsEnemyHitByLaser(Vector2 origin, Vector2 direction, float maxDistance, EnemyModel enemy)
    {
        Vector2 toEnemy = enemy.Position - origin;

        float projection = Vector2.Dot(toEnemy, direction);

        if (projection < 0f || projection > maxDistance)
        {
            return false;
        }

        Vector2 closestPoint = origin + direction * projection;
        float distanceToLine = Vector2.Distance(enemy.Position, closestPoint);

        return distanceToLine <= enemy.Radius;
    }
}