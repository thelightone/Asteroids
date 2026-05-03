using UnityEngine;
using Zenject;

using Game.Core;
using Game.Infrastructure;
using Game.Presentation;
using Game.Signals;

namespace Game.Installers
{
public readonly struct PresentationSceneAssets
{
    public EnemyView EnemyViewPrefab { get; }
    public EnemyView UfoViewPrefab { get; }
    public Transform EnemyRoot { get; }
    public BulletView BulletViewPrefab { get; }
    public Transform BulletRoot { get; }

    public PresentationSceneAssets(
        EnemyView enemyViewPrefab,
        EnemyView ufoViewPrefab,
        Transform enemyRoot,
        BulletView bulletViewPrefab,
        Transform bulletRoot)
    {
        EnemyViewPrefab = enemyViewPrefab;
        UfoViewPrefab = ufoViewPrefab;
        EnemyRoot = enemyRoot;
        BulletViewPrefab = bulletViewPrefab;
        BulletRoot = bulletRoot;
    }
}

public static class PresentationInstaller
{
    public static void InstallSignalBus(DiContainer container)
    {
        SignalBusInstaller.Install(container);
        container.DeclareSignal<LaserFiredSignal>();
        container.DeclareSignal<ScoreChangedSignal>();
        container.DeclareSignal<EnemyDestroyedSignal>();
    }

    public static void Install(
        DiContainer container,
        PresentationSceneAssets assets,
        MobileControlsUiInstallOptions mobileControlsUi)
    {
        container.BindInterfacesAndSelfTo<ScoreViewModel>().AsSingle();
        container.Bind<ShipStatsViewModel>().AsSingle();

        container.Bind<BulletViewPool>().FromMethod(_ =>
            new BulletViewPool(assets.BulletViewPrefab, assets.BulletRoot)
        ).AsSingle();

        container.Bind<BulletViewFactory>().AsSingle();

        container.Bind<AsteroidViewPool>().FromMethod(_ =>
            new AsteroidViewPool(assets.EnemyViewPrefab, assets.EnemyRoot)
        ).AsCached();

        container.Bind<UfoViewPool>().FromMethod(_ =>
            new UfoViewPool(assets.UfoViewPrefab, assets.EnemyRoot)
        ).AsCached();

        container.Bind<EnemyViewFactory>().AsSingle();

        container.Bind<BulletViewSynchronizer>().AsSingle();
        container.Bind<EnemyViewSynchronizer>().AsSingle();

        container.BindInterfacesAndSelfTo<PlayerPresentationStateInitializer>().AsSingle().NonLazy();
        container.BindInterfacesAndSelfTo<LivesPresenter>().AsSingle().NonLazy();
        container.BindInterfacesAndSelfTo<GameOverPresenter>().AsSingle().NonLazy();
        container.BindInterfacesAndSelfTo<ShipInvulnerabilityPresenter>().AsSingle().NonLazy();
        container.BindInterfacesAndSelfTo<PlayerTransformPresenter>().AsSingle().NonLazy();
        container.BindInterfacesAndSelfTo<ShipStatsPresenter>().AsSingle().NonLazy();

        BindSceneUiViews(container, mobileControlsUi);
    }

    private static void BindSceneUiViews(
        DiContainer container,
        MobileControlsUiInstallOptions mobileControlsUi)
    {
        container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle().NonLazy();
        container.Bind<ScoreView>().FromComponentInHierarchy().AsSingle().NonLazy();
        container.Bind<LivesView>().FromComponentInHierarchy().AsSingle().NonLazy();
        container.Bind<ShipInvulnerabilityView>().FromComponentInHierarchy().AsSingle().NonLazy();
        container.Bind<GameOverView>().FromComponentInHierarchy().AsSingle().NonLazy();
        container.Bind<ShipStatsView>().FromComponentInHierarchy().AsSingle().NonLazy();

        if (mobileControlsUi.View != null)
        {
            container.Bind<MobileControlsView>().FromInstance(mobileControlsUi.View).AsSingle();
            container.BindInterfacesAndSelfTo<MobileControlsPresenter>().FromMethod(context =>
            {
                var view = context.Container.Resolve<MobileControlsView>();
                var options = context.Container.Resolve<MobileControlsUiInstallOptions>();
                var reader = options.BindMobileControlsView
                    ? context.Container.Resolve<MobileInputReader>()
                    : null;

                return new MobileControlsPresenter(view, options, reader);
            }).AsSingle().NonLazy();
        }
    }
}

}
