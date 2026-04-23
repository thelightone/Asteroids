using UnityEngine;
using Zenject;

public class PlayerWeaponFacade : IGameTickable
{
    private readonly PlayerService _playerService;
    private readonly BulletConfig _bulletConfig;
    private readonly BulletFactory _bulletFactory;
    private readonly BulletCollectionService _bulletCollectionService;
    private readonly LaserService _laserService;

    private float _bulletCooldownLeft;

    private readonly SignalBus _signalBus;

    public PlayerWeaponFacade(
        PlayerService playerService,
        BulletConfig bulletConfig,
        BulletFactory bulletFactory,
        BulletCollectionService bulletCollectionService,
        LaserService laserService,
        SignalBus signalBus
        )
    {
        _playerService = playerService;
        _bulletConfig = bulletConfig;
        _bulletFactory = bulletFactory;
        _bulletCollectionService = bulletCollectionService;
        _laserService = laserService;

        _bulletCooldownLeft = 0f;

        _signalBus = signalBus;
    }

    public void Tick(float deltaTime)
    {
        if (_playerService.ShipModel.IsDead)
        {
            return;
        }

        if (_bulletCooldownLeft > 0f)
        {
            _bulletCooldownLeft -= deltaTime;
        }

        Vector2 forward = _playerService.Forward;

        PlayerInputData input = _playerService.CurrentInput;

        if (input.FireBullet)
        {
            TryFireBullet(forward);
        }

        if (input.FireLaser)
        {
            TryFireLaser(forward);
        }
    }

    private void TryFireBullet(Vector2 forward)
    {
        if (_bulletCooldownLeft > 0f)
        {
            return;
        }

        BulletModel bullet = _bulletFactory.Create(
            _playerService.ShipModel.Position,
            forward * _bulletConfig.Speed,
            _bulletConfig.LifeTime,
            _bulletConfig.Radius
        );

        _bulletCollectionService.Add(bullet);
        _bulletCooldownLeft = _bulletConfig.FireInterval;
    }

    private void TryFireLaser(Vector2 forward)
    {
        if (!_playerService.TryUseLaser())
        {
            return;
        }

        Vector2 start = _playerService.ShipModel.Position;
        Vector2 end = start + forward.normalized * _playerService.LaserDistance;

        _laserService.FireLaser(start, forward, _playerService.LaserDistance);

        _signalBus.Fire(new LaserFiredSignal(start, end));
    }
}