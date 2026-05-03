using System.Collections.Generic;
using UnityEngine;

using Game.Core;
using Game.Infrastructure;

namespace Game.Presentation
{
public class EnemyViewSynchronizer
{
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly EnemyViewFactory _enemyViewFactory;
    private readonly GameConfigService _gameConfigService;

    private readonly Dictionary<EnemyModel, EnemyView> _views = new();

    public EnemyViewSynchronizer(
        EnemyCollectionService enemyCollectionService,
        EnemyViewFactory enemyViewFactory,
        GameConfigService gameConfigService)
    {
        _enemyCollectionService = enemyCollectionService;
        _enemyViewFactory = enemyViewFactory;
        _gameConfigService = gameConfigService;
    }

    public void Sync()
    {
        var enemies = _enemyCollectionService.Enemies;

        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyModel enemy = enemies[i];

            if (!_views.TryGetValue(enemy, out EnemyView view))
            {
                view = _enemyViewFactory.Create(enemy);
                _views.Add(enemy, view);
            }

            view.transform.position = enemy.Position;
            view.transform.rotation = Quaternion.Euler(0f, 0f, enemy.Rotation);
            view.SetScaleMultiplier(GetScaleMultiplier(enemy));
        }

        RemoveMissingViews(enemies);
    }

    private void RemoveMissingViews(IReadOnlyList<EnemyModel> enemies)
    {
        var active = new HashSet<EnemyModel>(enemies);
        List<EnemyModel> toRemove = null;

        foreach (var pair in _views)
        {
            if (!active.Contains(pair.Key))
            {
                toRemove ??= new List<EnemyModel>();
                toRemove.Add(pair.Key);
            }
        }

        if (toRemove == null)
            return;

        for (int i = 0; i < toRemove.Count; i++)
        {
            EnemyModel enemy = toRemove[i];
            EnemyView view = _views[enemy];

            _enemyViewFactory.Release(enemy, view);
            _views.Remove(enemy);
        }
    }

    private float GetScaleMultiplier(EnemyModel enemy)
    {
        if (enemy.Type != EnemyType.AsteroidSmall)
        {
            return 1f;
        }

        EnemyConfig enemyConfig = _gameConfigService.EnemyConfig;

        if (enemyConfig.LargeAsteroidRadius <= 0f)
        {
            return 1f;
        }

        return enemyConfig.SmallAsteroidRadius / enemyConfig.LargeAsteroidRadius;
    }
}
}
