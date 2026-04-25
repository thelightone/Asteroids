using System.Collections.Generic;
using UnityEngine;
using Zenject;

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
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<LaserFiredSignal>();
        Container.DeclareSignal<ScoreChangedSignal>();
        Container.DeclareSignal<EnemyDestroyedSignal>();

        Container.BindInterfacesAndSelfTo<ScoreViewModel>().AsSingle();

        var loader = new JsonConfigLoader();
        var configService = new GameConfigService(loader);
        configService.LoadAll();

        Container.Bind<JsonConfigLoader>().FromInstance(loader).AsSingle();
        Container.Bind<GameConfigService>().FromInstance(configService).AsSingle();

        Container.BindInterfacesAndSelfTo<ScoreService>().FromMethod(context =>
        {
            var configs = context.Container.Resolve<GameConfigService>();
            var enemyConfig = configs.EnemyConfig;
            var signalBus = context.Container.Resolve<SignalBus>();

            var rewards = new Dictionary<EnemyType, int>
        {
            { EnemyType.AsteroidLarge, enemyConfig.LargeAsteroidReward },
            { EnemyType.AsteroidSmall, enemyConfig.SmallAsteroidReward },
            { EnemyType.Ufo, enemyConfig.UfoReward }
        };

            return new ScoreService(rewards, signalBus);
        }).AsSingle();

        Container.Bind<EnemyCollectionService>().AsSingle();
        Container.Bind<BulletCollectionService>().AsSingle();
        Container.Bind<WorldWrapService>().AsSingle();
        Container.Bind<ShipMovementService>().AsSingle();
        Container.Bind<EnemyFactory>().AsSingle();
        Container.Bind<BulletFactory>().AsSingle();
        Container.Bind<EnemyCleanupService>().AsSingle();
        Container.Bind<BulletMovementService>().AsSingle();
        BindInputReader();

        Container.Bind<PlayerService>().FromMethod(context =>
        {
            var configs = context.Container.Resolve<GameConfigService>();
            var inputReader = context.Container.Resolve<IInputReader>();
            var shipMovementService = context.Container.Resolve<ShipMovementService>();
            var worldWrapService = context.Container.Resolve<WorldWrapService>();

            return new PlayerService(
                configs.PlayerConfig,
                configs.LaserConfig,
                inputReader,
                configs.WorldConfig,
                shipMovementService,
                worldWrapService
            );
        }).AsSingle();

        Container.Bind<PlayerEnemyCollisionService>().FromMethod(context =>
        {
            var player = context.Container.Resolve<PlayerService>();
            var enemies = context.Container.Resolve<EnemyCollectionService>();
            var configs = context.Container.Resolve<GameConfigService>();

            return new PlayerEnemyCollisionService(
                player,
                enemies,
                configs.EnemyConfig
            );
        }).AsSingle();

        Container.Bind<EnemyMovementService>().FromMethod(context =>
        {
            var enemies = context.Container.Resolve<EnemyCollectionService>();
            var worldWrap = context.Container.Resolve<WorldWrapService>();
            var player = context.Container.Resolve<PlayerService>();
            var configs = context.Container.Resolve<GameConfigService>();

            return new EnemyMovementService(
                enemies,
                worldWrap,
                configs.WorldConfig,
                configs.EnemyConfig,
                player
            );
        }).AsSingle();

        Container.Bind<EnemySpawnService>().FromMethod(context =>
        {
            var configs = context.Container.Resolve<GameConfigService>();
            var enemyFactory = context.Container.Resolve<EnemyFactory>();
            var enemyCollection = context.Container.Resolve<EnemyCollectionService>();
            var player = context.Container.Resolve<PlayerService>();

            return new EnemySpawnService(
                configs.WorldConfig,
                configs.EnemyConfig,
                enemyFactory,
                enemyCollection,
                player
            );
        }).AsSingle();

        Container.Bind<BulletEnemyCollisionService>().FromMethod(context =>
        {
            var bullets = context.Container.Resolve<BulletCollectionService>();
            var enemies = context.Container.Resolve<EnemyCollectionService>();
            var enemySpawn = context.Container.Resolve<EnemySpawnService>();
            var signalBus = context.Container.Resolve<SignalBus>();

            return new BulletEnemyCollisionService(
                bullets,
                enemies,
                enemySpawn,
                signalBus
            );
        }).AsSingle();

        Container.Bind<LaserService>().FromMethod(context =>
        {
            var enemies = context.Container.Resolve<EnemyCollectionService>();
            var enemySpawn = context.Container.Resolve<EnemySpawnService>();
            var signalBus = context.Container.Resolve<SignalBus>();

            return new LaserService(
                enemies,
                enemySpawn,
                signalBus
            );
        }).AsSingle();

        Container.Bind<PlayerWeaponFacade>().FromMethod(context =>
        {
            var configs = context.Container.Resolve<GameConfigService>();
            var player = context.Container.Resolve<PlayerService>();
            var bulletFactory = context.Container.Resolve<BulletFactory>();
            var bulletCollection = context.Container.Resolve<BulletCollectionService>();
            var laserService = context.Container.Resolve<LaserService>();
            var signalBus = context.Container.Resolve<SignalBus>();

            return new PlayerWeaponFacade(
                player,
                configs.BulletConfig,
                bulletFactory,
                bulletCollection,
                laserService,
                signalBus
            );
        }).AsSingle();

        Container.Bind<GameplayFacade>().FromMethod(context =>
        {
            var player = context.Container.Resolve<PlayerService>();
            var enemySpawn = context.Container.Resolve<EnemySpawnService>();
            var enemyMovement = context.Container.Resolve<EnemyMovementService>();
            var bulletMovement = context.Container.Resolve<BulletMovementService>();
            var bulletEnemyCollision = context.Container.Resolve<BulletEnemyCollisionService>();
            var playerEnemyCollision = context.Container.Resolve<PlayerEnemyCollisionService>();
            var enemyCleanup = context.Container.Resolve<EnemyCleanupService>();

            return new GameplayFacade(
                player,
                enemySpawn,
                enemyMovement,
                bulletMovement,
                bulletEnemyCollision,
                playerEnemyCollision,
                enemyCleanup
            );
        }).AsSingle();

        Container.Bind<ShipStatsViewModel>().AsSingle();

        Container.Bind<BulletViewPool>().FromMethod(_ =>
        {
            return new BulletViewPool(_bulletViewPrefab, _bulletRoot);
        }).AsSingle();

        Container.Bind<BulletViewFactory>().AsSingle();

        Container.Bind<EnemyViewPool>().WithId("AsteroidPool").FromMethod(_ =>
        {
            return new EnemyViewPool(_enemyViewPrefab, _enemyRoot);
        }).AsCached();

        Container.Bind<EnemyViewPool>().WithId("UfoPool").FromMethod(_ =>
        {
            return new EnemyViewPool(_ufoViewPrefab, _enemyRoot);
        }).AsCached();

        Container.Bind<EnemyViewFactory>().FromMethod(context =>
        {
            var asteroidPool = context.Container.ResolveId<EnemyViewPool>("AsteroidPool");
            var ufoPool = context.Container.ResolveId<EnemyViewPool>("UfoPool");

            return new EnemyViewFactory(asteroidPool, ufoPool);
        }).AsSingle();

        Container.Bind<BulletViewSynchronizer>().AsSingle();
        Container.Bind<EnemyViewSynchronizer>().AsSingle();
    }

    private void BindInputReader()
    {
        bool shouldUseMobileInput = ShouldUseMobileInput();

        if (shouldUseMobileInput && _mobileControlsView != null)
        {
            var mobileInputReader = new MobileInputReader();
            Container.Bind<MobileInputReader>().FromInstance(mobileInputReader).AsSingle();
            Container.Bind<IInputReader>().FromInstance(mobileInputReader).AsSingle();

            _mobileControlsView.Initialize(mobileInputReader);
            _mobileControlsView.SetVisible(true);
            return;
        }

        Container.Bind<IInputReader>().To<KeyboardInputReader>().AsSingle();

        if (shouldUseMobileInput)
        {
            Debug.LogWarning("Mobile platform detected, but MobileControlsView is not assigned. KeyboardInputReader fallback is used.");
        }

        if (_mobileControlsView != null)
        {
            _mobileControlsView.SetVisible(false);
        }
    }

    private bool ShouldUseMobileInput()
    {
        if (Application.isMobilePlatform)
        {
            return true;
        }

#if UNITY_EDITOR
        return _forceMobileInputInEditor;
#else
        return false;
#endif
    }
}