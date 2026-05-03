using UnityEngine;

namespace Game.Core
{
public class EnemyMovementService : IGameTickable
{
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly WorldWrapService _worldWrapService;
    private readonly WorldConfig _worldConfig;
    private readonly EnemyConfig _enemyConfig;
    private readonly PlayerService _playerService;

    public EnemyMovementService(
    EnemyCollectionService enemyCollectionService,
    WorldWrapService worldWrapService,
    WorldConfig worldConfig,
    EnemyConfig enemyConfig,
    PlayerService playerService)
    {
        _enemyCollectionService = enemyCollectionService;
        _worldWrapService = worldWrapService;
        _worldConfig = worldConfig;
        _enemyConfig = enemyConfig;
        _playerService = playerService;
    }

    public void Tick(float deltaTime)
    {
        for (int i = 0; i < _enemyCollectionService.Enemies.Count; i++)
        {
            EnemyModel enemy = _enemyCollectionService.Enemies[i];

            if (!enemy.IsActive)
            {
                continue;
            }

            if (enemy.BounceTimeLeft > 0f)
            {
                enemy.BounceTimeLeft -= deltaTime;

                if (enemy.BounceTimeLeft < 0f)
                {
                    enemy.BounceTimeLeft = 0f;
                }
            }
            else if (enemy.Type == EnemyType.Ufo)
            {
                UpdateUfoVelocity(enemy);
            }

            enemy.Position += enemy.Velocity * deltaTime;
            _worldWrapService.WrapPosition(enemy, _worldConfig);
        }
    }

    private void UpdateUfoVelocity(EnemyModel enemy)
    {
        Vector2 directionToPlayer = _playerService.ShipModel.Position - enemy.Position;

        if (directionToPlayer == Vector2.zero)
        {
            return;
        }

        directionToPlayer = directionToPlayer.normalized;
        enemy.Velocity = directionToPlayer * _enemyConfig.UfoSpeed;
    }
}
}
