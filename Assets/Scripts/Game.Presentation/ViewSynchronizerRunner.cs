using UnityEngine;
using Zenject;

[DefaultExecutionOrder(-40)]
public class ViewSynchronizerRunner : MonoBehaviour
{
    private EnemyViewSynchronizer _enemyViewSynchronizer;
    private BulletViewSynchronizer _bulletViewSynchronizer;

    [Inject]
    public void Construct(
        EnemyViewSynchronizer enemyViewSynchronizer,
        BulletViewSynchronizer bulletViewSynchronizer)
    {
        _enemyViewSynchronizer = enemyViewSynchronizer;
        _bulletViewSynchronizer = bulletViewSynchronizer;
    }

    private void Update()
    {
        _enemyViewSynchronizer.Sync();
        _bulletViewSynchronizer.Sync();
    }
}
