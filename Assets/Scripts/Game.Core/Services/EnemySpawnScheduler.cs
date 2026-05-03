namespace Game.Core
{
public class EnemySpawnScheduler
{
    private readonly WorldConfig _worldConfig;
    private readonly EnemyConfig _enemyConfig;
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly EnemySpawnRules _spawnRules;

    private float _largeAsteroidSpawnTimer;
    private float _ufoSpawnTimer;

    public EnemySpawnScheduler(
        WorldConfig worldConfig,
        EnemyConfig enemyConfig,
        EnemyCollectionService enemyCollectionService,
        EnemySpawnRules spawnRules)
    {
        _worldConfig = worldConfig;
        _enemyConfig = enemyConfig;
        _enemyCollectionService = enemyCollectionService;
        _spawnRules = spawnRules;
        _ufoSpawnTimer = _enemyConfig.UfoSpawnInterval;
    }

    public void Tick(float deltaTime)
    {
        if (_enemyCollectionService.Enemies.Count >= _worldConfig.MaxEnemies)
        {
            return;
        }

        TickLargeAsteroidSpawn(deltaTime);
        TickUfoSpawn(deltaTime);
    }

    private void TickLargeAsteroidSpawn(float deltaTime)
    {
        _largeAsteroidSpawnTimer += deltaTime;

        if (_largeAsteroidSpawnTimer < _enemyConfig.LargeAsteroidSpawnInterval)
        {
            return;
        }

        _largeAsteroidSpawnTimer = 0f;
        _spawnRules.SpawnPeriodicLargeAsteroid();
    }

    private void TickUfoSpawn(float deltaTime)
    {
        _ufoSpawnTimer -= deltaTime;

        if (_ufoSpawnTimer > 0f)
        {
            return;
        }

        _spawnRules.SpawnUfo();
        _ufoSpawnTimer = _enemyConfig.UfoSpawnInterval;
    }
}
}
