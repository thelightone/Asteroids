public class EnemyViewFactory
{
    private readonly EnemyViewPool _asteroidPool;
    private readonly EnemyViewPool _ufoPool;

    public EnemyViewFactory(
        EnemyViewPool asteroidPool,
        EnemyViewPool ufoPool)
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