using UnityEngine;

using Game.Core;
using Game.Infrastructure;
using Game.Signals;

namespace Game.Presentation
{
public sealed class UfoViewPool : EnemyViewPool
{
    public UfoViewPool(EnemyView prefab, Transform root)
        : base(prefab, root)
    {
    }
}

}
