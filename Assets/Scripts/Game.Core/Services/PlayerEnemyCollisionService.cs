using UnityEngine;

public class PlayerEnemyCollisionService : IGameTickable
{
    private readonly PlayerService _playerService;
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly EnemyConfig _enemyConfig;

    public PlayerEnemyCollisionService(
    PlayerService playerService,
    EnemyCollectionService enemyCollectionService,
    EnemyConfig enemyConfig)
    {
        _playerService = playerService;
        _enemyCollectionService = enemyCollectionService;
        _enemyConfig = enemyConfig;
    }

    public void Tick(float deltaTime)
    {
        ShipModel ship = _playerService.ShipModel;

        if (ship.Health <= 0)
        {
            return;
        }

        for (int i = 0; i < _enemyCollectionService.Enemies.Count; i++)
        {
            EnemyModel enemy = _enemyCollectionService.Enemies[i];

            if (!enemy.IsActive)
            {
                continue;
            }

            if (HasCollision(ship, enemy))
            {
                Vector2 bounceDirection = ship.Position - enemy.Position;

                if (bounceDirection == Vector2.zero)
                {
                    bounceDirection = Random.insideUnitCircle.normalized;

                    if (bounceDirection == Vector2.zero)
                    {
                        bounceDirection = Vector2.up;
                    }
                }
                
                bounceDirection = bounceDirection.normalized;

                float enemySpeed = enemy.Velocity.magnitude;

                if (enemySpeed <= 0f)
                {
                    enemySpeed = 1f;
                }

                enemy.Velocity = -bounceDirection * enemySpeed;

                if (enemy.Type == EnemyType.Ufo)
                {
                    enemy.BounceTimeLeft = _enemyConfig.UfoBounceDuration;
                }

                _playerService.ApplyBounce(bounceDirection);
                _playerService.ApplyDamage();

                return;
            }
        }
    }

    private bool HasCollision(ShipModel ship, EnemyModel enemy)
    {
        float distance = Vector2.Distance(ship.Position, enemy.Position);
        return distance <= ship.Radius + enemy.Radius;
    }
}