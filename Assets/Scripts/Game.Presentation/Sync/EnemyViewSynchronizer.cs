using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyViewSynchronizer
{
    private readonly EnemyCollectionService _enemyCollectionService;
    private readonly EnemyViewFactory _enemyViewFactory;

    private readonly Dictionary<EnemyModel, EnemyView> _views = new();

    public EnemyViewSynchronizer(
        EnemyCollectionService enemyCollectionService,
        EnemyViewFactory enemyViewFactory)
    {
        _enemyCollectionService = enemyCollectionService;
        _enemyViewFactory = enemyViewFactory;
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
        }

        RemoveMissingViews(enemies);
    }

    private void RemoveMissingViews(IReadOnlyList<EnemyModel> enemies)
    {
        List<EnemyModel> toRemove = null;

        foreach (var pair in _views)
        {
            if (!enemies.Contains(pair.Key))
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
}