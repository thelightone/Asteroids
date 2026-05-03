using System.Collections.Generic;
using UnityEngine;

namespace Game.Presentation
{
public class BulletViewPool
{
    private readonly BulletView _prefab;
    private readonly Transform _root;
    private readonly List<BulletView> _pool = new();

    public BulletViewPool(BulletView prefab, Transform root)
    {
        _prefab = prefab;
        _root = root;
    }

    public BulletView Get()
    {
        BulletView view;

        if (_pool.Count > 0)
        {
            int lastIndex = _pool.Count - 1;
            view = _pool[lastIndex];
            _pool.RemoveAt(lastIndex);

            view.gameObject.SetActive(true);
        }
        else
        {
            view = Object.Instantiate(_prefab, _root);
        }

        return view;
    }

    public void Release(BulletView view)
    {
        if (!view)
            return;

        view.gameObject.SetActive(false);
        _pool.Add(view);
    }
}
}
