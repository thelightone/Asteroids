using System.Collections.Generic;
using Zenject;

using Game.Core;
using Game.Infrastructure;
using Game.Presentation;
using Game.Signals;

namespace Game.Installers
{
public static class GameplayInstaller
{
    public static void Install(DiContainer container)
    {
        container.BindInterfacesAndSelfTo<ScoreService>().FromMethod(context =>
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

        container.Bind<EnemyCollectionService>().AsSingle();
        container.Bind<BulletCollectionService>().AsSingle();
        container.Bind<WorldWrapService>().AsSingle();
        container.Bind<ShipMovementService>().AsSingle();
        container.Bind<EnemyFactory>().AsSingle();
        container.Bind<BulletFactory>().AsSingle();
        container.Bind<EnemyCleanupService>().AsSingle();
        container.Bind<BulletMovementService>().AsSingle();

        container.Bind<PlayerService>().FromMethod(context =>
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

        container.Bind<PlayerEnemyCollisionService>().FromMethod(context =>
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

        container.Bind<EnemyMovementService>().FromMethod(context =>
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

        container.Bind<EnemySpawnRules>().FromMethod(context =>
        {
            var configs = context.Container.Resolve<GameConfigService>();
            var enemyFactory = context.Container.Resolve<EnemyFactory>();
            var enemyCollection = context.Container.Resolve<EnemyCollectionService>();
            var player = context.Container.Resolve<PlayerService>();

            return new EnemySpawnRules(
                configs.WorldConfig,
                configs.EnemyConfig,
                enemyFactory,
                enemyCollection,
                player
            );
        }).AsSingle();

        container.Bind<EnemySpawnScheduler>().FromMethod(context =>
        {
            var configs = context.Container.Resolve<GameConfigService>();
            var enemyCollection = context.Container.Resolve<EnemyCollectionService>();
            var rules = context.Container.Resolve<EnemySpawnRules>();

            return new EnemySpawnScheduler(
                configs.WorldConfig,
                configs.EnemyConfig,
                enemyCollection,
                rules
            );
        }).AsSingle();

        container.Bind<EnemySpawnService>().AsSingle();

        container.Bind<EnemyDestroyService>().AsSingle();

        container.Bind<BulletEnemyCollisionService>().FromMethod(context =>
        {
            var bullets = context.Container.Resolve<BulletCollectionService>();
            var enemies = context.Container.Resolve<EnemyCollectionService>();
            var enemyDestroy = context.Container.Resolve<EnemyDestroyService>();

            return new BulletEnemyCollisionService(
                bullets,
                enemies,
                enemyDestroy
            );
        }).AsSingle();

        container.Bind<LaserService>().FromMethod(context =>
        {
            var enemies = context.Container.Resolve<EnemyCollectionService>();
            var enemyDestroy = context.Container.Resolve<EnemyDestroyService>();

            return new LaserService(
                enemies,
                enemyDestroy
            );
        }).AsSingle();

        container.Bind<PlayerWeaponFacade>().FromMethod(context =>
        {
            var configs = context.Container.Resolve<GameConfigService>();
            var player = context.Container.Resolve<PlayerService>();
            var bulletFactory = context.Container.Resolve<BulletFactory>();
            var bulletCollection = context.Container.Resolve<BulletCollectionService>();
            var laserService = context.Container.Resolve<LaserService>();
            var signalBus = context.Container.Resolve<SignalBus>();

            return new PlayerWeaponFacade(
                player,
                configs.LaserConfig,
                configs.BulletConfig,
                bulletFactory,
                bulletCollection,
                laserService,
                signalBus
            );
        }).AsSingle();

        container.Bind<GameplayFacade>().FromMethod(context =>
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

        container.BindInterfacesAndSelfTo<GameplayTickCoordinator>().AsSingle().NonLazy();
    }
}

}
