using Game.Core;
using Game.Infrastructure;
using Game.Signals;

namespace Game.Presentation
{
public class BulletViewFactory
{
    private readonly BulletViewPool _pool;

    public BulletViewFactory(BulletViewPool pool)
    {
        _pool = pool;
    }

    public BulletView Create()
    {
        return _pool.Get();
    }

    public void Release(BulletView view)
    {
        _pool.Release(view);
    }
}
}
