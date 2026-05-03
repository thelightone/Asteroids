using Zenject;

namespace Game.Presentation
{
public sealed class ShipStatsPresenter : ITickable
{
    private readonly ShipStatsView _view;
    private readonly ShipStatsViewModel _viewModel;

    public ShipStatsPresenter(ShipStatsView view, ShipStatsViewModel viewModel)
    {
        _view = view;
        _viewModel = viewModel;
    }

    public void Tick()
    {
        _view.Apply(
            new ShipHudStats(
                _viewModel.ShipPosition,
                _viewModel.ShipRotation,
                _viewModel.ShipSpeed,
                _viewModel.LaserCharges,
                _viewModel.MaxLaserCharges,
                _viewModel.LaserCooldownLeft));
    }
}
}
