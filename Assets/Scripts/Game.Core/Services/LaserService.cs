using UnityEngine;

namespace Game.Core
{
public class LaserService
{
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly EnemyDestroyService _enemyDestroyService;

    public LaserService(
        EnemyCollectionService enemyCollectionService,
        EnemyDestroyService enemyDestroyService)
    {
        _enemyCollectionService = enemyCollectionService;
        _enemyDestroyService = enemyDestroyService;
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
                _enemyDestroyService.DestroyEnemy(enemy);
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
}
