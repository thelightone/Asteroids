using System;

namespace Game.Core
{
public class LaserChargeService
{
    private readonly ShipModel _ship;
    private readonly LaserConfig _laserConfig;

    public event Action<int, float> LaserStateChanged;

    public float LaserDistance => _laserConfig.Length;

    public LaserChargeService(ShipModel ship, LaserConfig laserConfig)
    {
        _ship = ship;
        _laserConfig = laserConfig;
    }

    public void PublishLaserState()
    {
        NotifyLaserStateChanged();
    }

    public void Tick(float deltaTime)
    {
        UpdateLaserFireCooldown(deltaTime);
        UpdateLaserCooldown(deltaTime);
    }

    public bool TryUseLaser()
    {
        if (_ship.IsDead)
        {
            return false;
        }

        if (_ship.LaserFireCooldownLeft > 0f)
        {
            return false;
        }

        if (_ship.LaserCharges <= 0)
        {
            return false;
        }

        _ship.LaserCharges--;

        if (_ship.LaserCooldownLeft <= 0f)
        {
            _ship.LaserCooldownLeft = _laserConfig.RechargeInterval;
        }

        _ship.LaserFireCooldownLeft = _laserConfig.Cooldown;

        NotifyLaserStateChanged();
        return true;
    }

    private void NotifyLaserStateChanged()
    {
        LaserStateChanged?.Invoke(_ship.LaserCharges, _ship.LaserCooldownLeft);
    }

    private void UpdateLaserFireCooldown(float deltaTime)
    {
        if (_ship.LaserFireCooldownLeft <= 0f)
        {
            return;
        }

        _ship.LaserFireCooldownLeft -= deltaTime;

        if (_ship.LaserFireCooldownLeft < 0f)
        {
            _ship.LaserFireCooldownLeft = 0f;
        }
    }

    private void UpdateLaserCooldown(float deltaTime)
    {
        if (_ship.LaserCharges >= _ship.MaxLaserCharges)
        {
            _ship.LaserCooldownLeft = 0f;
            return;
        }

        if (_ship.LaserCooldownLeft > 0f)
        {
            _ship.LaserCooldownLeft -= deltaTime;

            if (_ship.LaserCooldownLeft < 0f)
            {
                _ship.LaserCooldownLeft = 0f;
            }

            NotifyLaserStateChanged();
        }

        if (_ship.LaserCooldownLeft <= 0f)
        {
            _ship.LaserCharges++;

            if (_ship.LaserCharges > _ship.MaxLaserCharges)
            {
                _ship.LaserCharges = _ship.MaxLaserCharges;
            }

            if (_ship.LaserCharges < _ship.MaxLaserCharges)
            {
                _ship.LaserCooldownLeft = _laserConfig.RechargeInterval;
            }
            else
            {
                _ship.LaserCooldownLeft = 0f;
            }

            NotifyLaserStateChanged();
        }
    }
}
}
