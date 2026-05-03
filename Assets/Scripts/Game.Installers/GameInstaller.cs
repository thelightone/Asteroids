using UnityEngine;
using Zenject;

using Game.Core;
using Game.Infrastructure;
using Game.Presentation;
using Game.Signals;

namespace Game.Installers
{
public sealed class GameInstaller : MonoInstaller
{
    [SerializeField] private EnemyView _enemyViewPrefab;
    [SerializeField] private EnemyView _ufoViewPrefab;
    [SerializeField] private Transform _enemyRoot;

    [SerializeField] private BulletView _bulletViewPrefab;
    [SerializeField] private Transform _bulletRoot;
    [SerializeField] private MobileControlsView _mobileControlsView;
    [SerializeField] private bool _forceMobileInputInEditor;

    public override void InstallBindings()
    {
        ConfigInstaller.Install(Container);
        PresentationInstaller.InstallSignalBus(Container);

        var mobileUi = InputInstaller.Install(Container, _mobileControlsView, _forceMobileInputInEditor);

        GameplayInstaller.Install(Container);

        var presentationAssets = new PresentationSceneAssets(
            _enemyViewPrefab,
            _ufoViewPrefab,
            _enemyRoot,
            _bulletViewPrefab,
            _bulletRoot);

        PresentationInstaller.Install(Container, presentationAssets, mobileUi);
    }
}

}
