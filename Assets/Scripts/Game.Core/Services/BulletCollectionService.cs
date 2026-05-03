using System.Collections.Generic;

namespace Game.Core
{
public class BulletCollectionService
{
    private readonly List<BulletModel> _bullets = new();

    public IReadOnlyList<BulletModel> Bullets => _bullets;

    public void Add(BulletModel bullet)
    {
        if (!_bullets.Contains(bullet))
        {
            _bullets.Add(bullet);
        }
    }

    public void Remove(BulletModel bullet)
    {
        _bullets.Remove(bullet);
    }

    public void Clear()
    {
        _bullets.Clear();
    }
}
}
