using UnityEngine;

namespace Game.Core
{
public class EnemySpawnRules
{
    private readonly WorldConfig _worldConfig;
    private readonly EnemyConfig _enemyConfig;
    private readonly EnemyFactory _enemyFactory;
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly PlayerService _playerService;

    public EnemySpawnRules(
        WorldConfig worldConfig,
        EnemyConfig enemyConfig,
        EnemyFactory enemyFactory,
        EnemyCollectionService enemyCollectionService,
        PlayerService playerService)
    {
        _worldConfig = worldConfig;
        _enemyConfig = enemyConfig;
        _enemyFactory = enemyFactory;
        _enemyCollectionService = enemyCollectionService;
        _playerService = playerService;
    }

    public void SpawnPeriodicLargeAsteroid()
    {
        Vector2 spawnPosition = GetSpawnPositionAroundPlayer();
        Vector2 velocity = GetAsteroidVelocityTowardsPlayer(spawnPosition);

        EnemyModel asteroid = _enemyFactory.CreateLargeAsteroid(
            spawnPosition,
            velocity,
            _enemyConfig.LargeAsteroidRadius
        );

        _enemyCollectionService.Add(asteroid);
    }

    public void SplitLargeAsteroid(EnemyModel destroyedAsteroid)
    {
        if (destroyedAsteroid.Type != EnemyType.AsteroidLarge)
        {
            return;
        }

        Vector2 spawnPosition = destroyedAsteroid.Position;

        Vector2 direction1 = Random.insideUnitCircle.normalized;

        if (direction1 == Vector2.zero)
        {
            direction1 = Vector2.right;
        }

        Vector2 direction2 = -direction1;

        int roomForNewEnemies =
            _worldConfig.MaxEnemies - (_enemyCollectionService.Enemies.Count - 1);
        int smallCount = Mathf.Clamp(roomForNewEnemies, 0, 2);

        if (smallCount <= 0)
        {
            return;
        }

        EnemyModel smallAsteroid1 = _enemyFactory.CreateSmallAsteroid(
            spawnPosition,
            direction1 * _enemyConfig.SmallAsteroidSpeed,
            _enemyConfig.SmallAsteroidRadius
        );

        _enemyCollectionService.Add(smallAsteroid1);

        if (smallCount < 2)
        {
            return;
        }

        EnemyModel smallAsteroid2 = _enemyFactory.CreateSmallAsteroid(
            spawnPosition,
            direction2 * _enemyConfig.SmallAsteroidSpeed,
            _enemyConfig.SmallAsteroidRadius
        );

        _enemyCollectionService.Add(smallAsteroid2);
    }

    public void SpawnUfo()
    {
        float halfWidth = _worldConfig.Width * 0.5f;
        float halfHeight = _worldConfig.Height * 0.5f;

        bool spawnFromLeft = Random.value < 0.5f;
        float outside = _worldConfig.SpawnOutsideOffset;

        float x = spawnFromLeft ? -halfWidth - outside : halfWidth + outside;
        float y = Random.Range(-halfHeight, halfHeight);

        Vector2 position = new Vector2(x, y);

        Vector2 directionToPlayer = (_playerService.ShipModel.Position - position).normalized;
        Vector2 velocity = directionToPlayer * _enemyConfig.UfoSpeed;

        EnemyModel ufo = _enemyFactory.CreateUfo(
            position,
            velocity,
            _enemyConfig.UfoRadius
        );

        _enemyCollectionService.Add(ufo);
    }

    private Vector2 GetSpawnPositionAroundPlayer()
    {
        Vector2 playerPosition = _playerService.ShipModel.Position;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        if (randomDirection == Vector2.zero)
        {
            randomDirection = Vector2.right;
        }

        return playerPosition + randomDirection * _worldConfig.LargeAsteroidSpawnRingDistance;
    }

    private Vector2 GetAsteroidVelocityTowardsPlayer(Vector2 spawnPosition)
    {
        Vector2 playerPosition = _playerService.ShipModel.Position;
        Vector2 targetDirection = (playerPosition - spawnPosition).normalized;

        Vector2 randomOffset = Random.insideUnitCircle * _enemyConfig.LargeAsteroidSpawnDirectionSpread;
        Vector2 finalDirection = (targetDirection + randomOffset).normalized;

        if (finalDirection == Vector2.zero)
        {
            finalDirection = targetDirection;
        }

        return finalDirection * _enemyConfig.LargeAsteroidSpeed;
    }
}
}
