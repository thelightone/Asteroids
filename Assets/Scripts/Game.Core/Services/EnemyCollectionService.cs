using System.Collections.Generic;

public class EnemyCollectionService
{
    private readonly List<EnemyModel> _enemies = new();

    public IReadOnlyList<EnemyModel> Enemies => _enemies;

    public void Add(EnemyModel enemy)
    {
        if (!_enemies.Contains(enemy))
        {
            _enemies.Add(enemy);
        }
    }

    public void Remove(EnemyModel enemy)
    {
        _enemies.Remove(enemy);
    }

    public void Clear()
    {
        _enemies.Clear();
    }
}