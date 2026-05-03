using Game.Core;
using Game.Infrastructure;
using Game.Signals;

namespace Game.Presentation
{
public class EnemyViewFactory
{
    private readonly AsteroidViewPool _asteroidPool;
    private readonly UfoViewPool _ufoPool;

    public EnemyViewFactory(
        AsteroidViewPool asteroidPool,
        UfoViewPool ufoPool)
    {
        _asteroidPool = asteroidPool;
        _ufoPool = ufoPool;
    }

    public EnemyView Create(EnemyModel model)
    {
        return model.Type == EnemyType.Ufo
            ? _ufoPool.Get()
            : _asteroidPool.Get();
    }

    public void Release(EnemyModel model, EnemyView view)
    {
        if (model.Type == EnemyType.Ufo)
        {
            _ufoPool.Release(view);
        }
        else
        {
            _asteroidPool.Release(view);
        }
    }
}
}
