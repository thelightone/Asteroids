using System;
using UnityEngine;

namespace Game.Core
{
public class PlayerHealthService
{
    private readonly ShipModel _ship;
    private readonly PlayerConfig _playerConfig;

    public event Action<int> HealthChanged;
    public event Action Died;
    public event Action<bool> InvulnerabilityChanged;

    public PlayerHealthService(ShipModel ship, PlayerConfig playerConfig)
    {
        _ship = ship;
        _playerConfig = playerConfig;
    }

    public void PublishHealthState()
    {
        NotifyHealthChanged();
    }

    public void Tick(float deltaTime)
    {
        UpdateInvulnerability(deltaTime);
    }

    public void ApplyDamage()
    {
        if (_ship.Health <= 0)
        {
            return;
        }

        if (_ship.IsInvulnerable)
        {
            return;
        }

        _ship.Health--;
        NotifyHealthChanged();

        if (_ship.Health <= 0)
        {
            _ship.Health = 0;
            _ship.IsDead = true;
            _ship.IsControlLocked = true;
            _ship.Velocity = Vector2.zero;
            Died?.Invoke();

            return;
        }

        _ship.IsInvulnerable = true;
        _ship.InvulnerableTimeLeft = _playerConfig.InvulnerabilityDuration;
        _ship.IsControlLocked = true;
        InvulnerabilityChanged?.Invoke(true);
    }

    private void UpdateInvulnerability(float deltaTime)
    {
        if (!_ship.IsInvulnerable)
        {
            return;
        }

        _ship.InvulnerableTimeLeft -= deltaTime;

        if (_ship.InvulnerableTimeLeft <= 0f)
        {
            _ship.InvulnerableTimeLeft = 0f;
            _ship.IsInvulnerable = false;
            InvulnerabilityChanged?.Invoke(false);

            if (!_ship.IsDead)
            {
                _ship.IsControlLocked = false;
            }
        }
    }

    private void NotifyHealthChanged()
    {
        HealthChanged?.Invoke(_ship.Health);
    }
}
}
