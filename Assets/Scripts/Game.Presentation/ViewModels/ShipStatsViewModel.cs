using UnityEngine;

public class ShipStatsViewModel
{
    private readonly PlayerService _playerService;

    public ShipStatsViewModel(PlayerService playerService)
    {
        _playerService = playerService;
    }

    public string PositionText
    {
        get
        {
            Vector2 position = _playerService.ShipModel.Position;
            return $"Pos: X {position.x:F2}, Y {position.y:F2}";
        }
    }

    public string RotationText
    {
        get
        {
            return $"Rot: {_playerService.ShipModel.Rotation:F1}";
        }
    }

    public string SpeedText
    {
        get
        {
            float speed = _playerService.ShipModel.Velocity.magnitude;
            return $"Speed: {speed:F2}";
        }
    }

    public string LaserChargesText
    {
        get
        {
            ShipModel ship = _playerService.ShipModel;
            return $"Laser Charges: {ship.LaserCharges}/{ship.MaxLaserCharges}";
        }
    }

    public string LaserCooldownText
    {
        get
        {
            return $"Laser Cooldown: {_playerService.ShipModel.LaserCooldownLeft:F2}";
        }
    }
}