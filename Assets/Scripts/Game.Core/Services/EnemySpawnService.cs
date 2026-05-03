namespace Game.Core
{
public class EnemySpawnService : IGameTickable
{
    private readonly EnemySpawnScheduler _scheduler;
    private readonly EnemySpawnRules _spawnRules;

    public EnemySpawnService(EnemySpawnScheduler scheduler, EnemySpawnRules spawnRules)
    {
        _scheduler = scheduler;
        _spawnRules = spawnRules;
    }

    public void Tick(float deltaTime)
    {
        _scheduler.Tick(deltaTime);
    }

    public void SplitLargeAsteroid(EnemyModel destroyedAsteroid)
    {
        _spawnRules.SplitLargeAsteroid(destroyedAsteroid);
    }
}
}
