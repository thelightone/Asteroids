using System.Collections.Generic;
using UnityEngine;

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
        if (_pool.Count > 0)
        {
            int lastIndex = _pool.Count - 1;
            BulletView view = _pool[lastIndex];
            _pool.RemoveAt(lastIndex);

            view.gameObject.SetActive(true);
            return view;
        }

        return Object.Instantiate(_prefab, _root);
    }

    public void Release(BulletView view)
    {
        view.gameObject.SetActive(false);
        _pool.Add(view);
    }
}