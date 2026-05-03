using Zenject;
using Game.Signals;

namespace Game.Core
{
public class EnemyDestroyService
{
    private readonly EnemySpawnService _enemySpawnService;
    private readonly SignalBus _signalBus;

    public EnemyDestroyService(EnemySpawnService enemySpawnService, SignalBus signalBus)
    {
        _enemySpawnService = enemySpawnService;
        _signalBus = signalBus;
    }

    public void DestroyEnemy(EnemyModel enemy)
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
