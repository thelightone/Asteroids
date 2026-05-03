using UnityEngine;

using Game.Core;

namespace Game.Presentation
{
public class ShipStatsViewModel
{
    private readonly PlayerService _playerService;

    public ShipStatsViewModel(PlayerService playerService)
    {
        _playerService = playerService;
    }

    public Vector2 ShipPosition => _playerService.ShipModel.Position;

    public float ShipRotation => _playerService.ShipModel.Rotation;

    public float ShipSpeed => _playerService.ShipModel.Velocity.magnitude;

    public int LaserCharges => _playerService.ShipModel.LaserCharges;

    public int MaxLaserCharges => _playerService.ShipModel.MaxLaserCharges;

    public float LaserCooldownLeft => Mathf.Max(
        _playerService.ShipModel.LaserCooldownLeft,
        _playerService.ShipModel.LaserFireCooldownLeft);
}
}
