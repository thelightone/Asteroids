using System.Collections.Generic;
using UnityEngine;

public class EnemyViewPool
{
    private readonly EnemyView _prefab;
    private readonly Transform _root;
    private readonly List<EnemyView> _pool = new();

    public EnemyViewPool(EnemyView prefab, Transform root)
    {
        _prefab = prefab;
        _root = root;
    }

    public EnemyView Get()
    {
        EnemyView view;

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

        view.OnSpawn();
        return view;
    }

    public void Release(EnemyView view)
    {
        if (!view)
            return;

        view.OnDespawn();
        view.gameObject.SetActive(false);
        _pool.Add(view);
    }
}