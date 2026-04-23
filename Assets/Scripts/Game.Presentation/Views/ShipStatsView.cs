using TMPro;
using UnityEngine;

public class ShipStatsView : MonoBehaviour
{
    [SerializeField] private TMP_Text _positionText;
    [SerializeField] private TMP_Text _rotationText;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _laserChargesText;
    [SerializeField] private TMP_Text _laserCooldownText;

    private ShipStatsViewModel _viewModel;

    public void Initialize(ShipStatsViewModel viewModel)
    {
        _viewModel = viewModel;
        Refresh();
    }

    private void Update()
    {
        if (_viewModel == null)
        {
            return;
        }

        Refresh();
    }

    private void Refresh()
    {
        _positionText.text = _viewModel.PositionText;
        _rotationText.text = _viewModel.RotationText;
        _speedText.text = _viewModel.SpeedText;
        _laserChargesText.text = _viewModel.LaserChargesText;
        _laserCooldownText.text = _viewModel.LaserCooldownText;
    }
}