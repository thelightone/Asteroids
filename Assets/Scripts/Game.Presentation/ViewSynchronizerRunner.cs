using UnityEngine;
using Zenject;

using Game.Core;
using Game.Infrastructure;
using Game.Signals;

namespace Game.Presentation
{
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

}
