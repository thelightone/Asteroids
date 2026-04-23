public class EnemyCleanupService : IGameTickable
{
    private readonly EnemyCollectionService _enemyCollectionService;

    public EnemyCleanupService(EnemyCollectionService enemyCollectionService)
    {
        _enemyCollectionService = enemyCollectionService;
    }

    public void Tick(float deltaTime)
    {
        for (int i = _enemyCollectionService.Enemies.Count - 1; i >= 0; i--)
        {
            EnemyModel enemy = _enemyCollectionService.Enemies[i];

            if (!enemy.IsActive)
            {
                _enemyCollectionService.Remove(enemy);
            }
        }
    }
}