using UnityEngine;

using Game.Core;
using Game.Infrastructure;
using Game.Signals;

namespace Game.Presentation
{
public sealed class AsteroidViewPool : EnemyViewPool
{
    public AsteroidViewPool(EnemyView prefab, Transform root)
        : base(prefab, root)
    {
    }
}

}
