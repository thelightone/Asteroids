public class GameplayFacade : IGameTickable
{
    private readonly PlayerService _playerService;
    private readonly EnemySpawnService _enemySpawnService;
    private readonly EnemyMovementService _enemyMovementService;
    private readonly BulletMovementService _bulletMovementService;
    private readonly BulletEnemyCollisionService _bulletEnemyCollisionService;
    private readonly PlayerEnemyCollisionService _playerEnemyCollisionService;
    private readonly EnemyCleanupService _enemyCleanupService;

    public GameplayFacade(
        PlayerService playerService,
        EnemySpawnService enemySpawnService,
        EnemyMovementService enemyMovementService,
        BulletMovementService bulletMovementService,
        BulletEnemyCollisionService bulletEnemyCollisionService,
        PlayerEnemyCollisionService playerEnemyCollisionService,
        EnemyCleanupService enemyCleanupService)
    {
        _playerService = playerService;
        _enemySpawnService = enemySpawnService;
        _enemyMovementService = enemyMovementService;
        _bulletMovementService = bulletMovementService;
        _bulletEnemyCollisionService = bulletEnemyCollisionService;
        _playerEnemyCollisionService = playerEnemyCollisionService;
        _enemyCleanupService = enemyCleanupService;
    }

    public void Tick(float deltaTime)
    {
        _playerService.Tick(deltaTime);
        _enemySpawnService.Tick(deltaTime);
        _enemyMovementService.MoveEnemies(deltaTime);
        _bulletMovementService.Tick(deltaTime);
        _bulletEnemyCollisionService.Tick(deltaTime);
        _playerEnemyCollisionService.Tick(deltaTime);
        _enemyCleanupService.Tick(deltaTime);
    }
}