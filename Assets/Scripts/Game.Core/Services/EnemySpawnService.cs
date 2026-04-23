using UnityEngine;

public class EnemySpawnService : IGameTickable
{
    private readonly WorldConfig _worldConfig;
    private readonly EnemyConfig _enemyConfig;
    private readonly EnemyFactory _enemyFactory;
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly PlayerService _playerService;

    private float _spawnTimer;
    private readonly float _spawnInterval = 4f;

    private float _ufoSpawnTimer;

    public EnemySpawnService(
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
        _ufoSpawnTimer = _enemyConfig.UfoSpawnInterval;
    }

    public void SpawnLargeAsteroid()
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



    private Vector2 GetSpawnPositionAroundPlayer()
    {
        Vector2 playerPosition = _playerService.ShipModel.Position;

        float spawnDistance = 12f;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        if (randomDirection == Vector2.zero)
        {
            randomDirection = Vector2.right;
        }

        return playerPosition + randomDirection * spawnDistance;
    }

    private Vector2 GetAsteroidVelocityTowardsPlayer(Vector2 spawnPosition)
    {
        Vector2 playerPosition = _playerService.ShipModel.Position;
        Vector2 targetDirection = (playerPosition - spawnPosition).normalized;

        Vector2 randomOffset = Random.insideUnitCircle * 0.25f;
        Vector2 finalDirection = (targetDirection + randomOffset).normalized;

        if (finalDirection == Vector2.zero)
        {
            finalDirection = targetDirection;
        }

        return finalDirection * _enemyConfig.LargeAsteroidSpeed;
    }

    private Vector2 GetRandomAsteroidVelocity()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;

        if (direction == Vector2.zero)
        {
            direction = Vector2.right;
        }

        return direction * _enemyConfig.LargeAsteroidSpeed;
    }

    public void Tick(float deltaTime)
    {
        if (_enemyCollectionService.Enemies.Count >= _worldConfig.MaxEnemies)
        {
            return;
        }

        _spawnTimer += deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnLargeAsteroid();
        }

        UpdateUfoSpawn(deltaTime);
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

        EnemyModel smallAsteroid1 = _enemyFactory.CreateSmallAsteroid(
            spawnPosition,
            direction1 * _enemyConfig.SmallAsteroidSpeed,
            _enemyConfig.SmallAsteroidRadius
        );

        EnemyModel smallAsteroid2 = _enemyFactory.CreateSmallAsteroid(
            spawnPosition,
            direction2 * _enemyConfig.SmallAsteroidSpeed,
            _enemyConfig.SmallAsteroidRadius
        );

        _enemyCollectionService.Add(smallAsteroid1);
        _enemyCollectionService.Add(smallAsteroid2);
    }

    private void UpdateUfoSpawn(float deltaTime)
    {
        _ufoSpawnTimer -= deltaTime;

        if (_ufoSpawnTimer > 0f)
        {
            return;
        }

        SpawnUfo();
        _ufoSpawnTimer = _enemyConfig.UfoSpawnInterval;
    }

    private void SpawnUfo()
    {
        float halfWidth = _worldConfig.Width * 0.5f;
        float halfHeight = _worldConfig.Height * 0.5f;

        bool spawnFromLeft = Random.value < 0.5f;

        float x = spawnFromLeft ? -halfWidth : halfWidth;
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

        Debug.Log("UFO spawned at " + position);
    }
}