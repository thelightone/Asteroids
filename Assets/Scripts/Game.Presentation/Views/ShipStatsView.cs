using TMPro;
using UnityEngine;

namespace Game.Presentation
{
public class ShipStatsView : MonoBehaviour
{
    private const float PositionEpsilon = 0.005f;
    private const float RotationEpsilon = 0.05f;
    private const float SpeedEpsilon = 0.005f;
    private const float CooldownEpsilon = 0.005f;

    [SerializeField] private TMP_Text _positionText;
    [SerializeField] private TMP_Text _rotationText;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _laserChargesText;
    [SerializeField] private TMP_Text _laserCooldownText;

    private Vector2 _lastPosition;
    private float _lastRotation;
    private float _lastSpeed;
    private int _lastLaserCharges;
    private int _lastMaxLaserCharges;
    private float _lastLaserCooldown;
    private bool _hasSnapshot;

    private void OnEnable()
    {
        _hasSnapshot = false;
    }

    public void Apply(in ShipHudStats stats)
    {
        if (!_hasSnapshot)
        {
            ApplyAll(in stats);
            _hasSnapshot = true;
            return;
        }

        if (PositionChanged(stats.Position))
        {
            _positionText.SetText("Pos: X {0:0.00}, Y {1:0.00}", stats.Position.x, stats.Position.y);
            _lastPosition = stats.Position;
        }

        if (Mathf.Abs(stats.Rotation - _lastRotation) > RotationEpsilon)
        {
            _rotationText.SetText("Rot: {0:0.0}", stats.Rotation);
            _lastRotation = stats.Rotation;
        }

        if (Mathf.Abs(stats.Speed - _lastSpeed) > SpeedEpsilon)
        {
            _speedText.SetText("Speed: {0:0.00}", stats.Speed);
            _lastSpeed = stats.Speed;
        }

        if (stats.LaserCharges != _lastLaserCharges || stats.MaxLaserCharges != _lastMaxLaserCharges)
        {
            _laserChargesText.SetText(
                "Laser Charges: {0:0}/{1:0}",
                (float)stats.LaserCharges,
                (float)stats.MaxLaserCharges);
            _lastLaserCharges = stats.LaserCharges;
            _lastMaxLaserCharges = stats.MaxLaserCharges;
        }

        if (Mathf.Abs(stats.LaserCooldown - _lastLaserCooldown) > CooldownEpsilon)
        {
            _laserCooldownText.SetText("Laser Cooldown: {0:0.00}", stats.LaserCooldown);
            _lastLaserCooldown = stats.LaserCooldown;
        }
    }

    private bool PositionChanged(Vector2 position)
    {
        return Mathf.Abs(position.x - _lastPosition.x) > PositionEpsilon
            || Mathf.Abs(position.y - _lastPosition.y) > PositionEpsilon;
    }

    private void ApplyAll(in ShipHudStats stats)
    {
        _positionText.SetText("Pos: X {0:0.00}, Y {1:0.00}", stats.Position.x, stats.Position.y);
        _rotationText.SetText("Rot: {0:0.0}", stats.Rotation);
        _speedText.SetText("Speed: {0:0.00}", stats.Speed);
        _laserChargesText.SetText(
            "Laser Charges: {0:0}/{1:0}",
            (float)stats.LaserCharges,
            (float)stats.MaxLaserCharges);
        _laserCooldownText.SetText("Laser Cooldown: {0:0.00}", stats.LaserCooldown);

        _lastPosition = stats.Position;
        _lastRotation = stats.Rotation;
        _lastSpeed = stats.Speed;
        _lastLaserCharges = stats.LaserCharges;
        _lastMaxLaserCharges = stats.MaxLaserCharges;
        _lastLaserCooldown = stats.LaserCooldown;
    }
}
}
