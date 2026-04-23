using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletViewSynchronizer
{
    private readonly BulletCollectionService _bulletCollectionService;
    private readonly BulletViewFactory _bulletViewFactory;

    private readonly Dictionary<BulletModel, BulletView> _views = new();

    public BulletViewSynchronizer(
        BulletCollectionService bulletCollectionService,
        BulletViewFactory bulletViewFactory)
    {
        _bulletCollectionService = bulletCollectionService;
        _bulletViewFactory = bulletViewFactory;
    }

    public void Sync()
    {
        var bullets = _bulletCollectionService.Bullets;

        for (int i = 0; i < bullets.Count; i++)
        {
            BulletModel bullet = bullets[i];

            if (!_views.TryGetValue(bullet, out BulletView view))
            {
                view = _bulletViewFactory.Create();
                _views.Add(bullet, view);
            }

            view.transform.position = bullet.Position;
        }

        RemoveMissingViews(bullets);
    }

    private void RemoveMissingViews(IReadOnlyList<BulletModel> bullets)
    {
        List<BulletModel> toRemove = null;

        foreach (var pair in _views)
        {
            if (!bullets.Contains(pair.Key))
            {
                toRemove ??= new List<BulletModel>();
                toRemove.Add(pair.Key);
            }
        }

        if (toRemove == null)
            return;

        for (int i = 0; i < toRemove.Count; i++)
        {
            BulletModel bullet = toRemove[i];
            BulletView view = _views[bullet];

            _bulletViewFactory.Release(view);
            _views.Remove(bullet);
        }
    }
}