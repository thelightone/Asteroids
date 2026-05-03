using System;
using Zenject;

using Game.Core;

namespace Game.Presentation
{
public sealed class LivesPresenter : IInitializable, IDisposable
{
    private readonly LivesView _view;
    private readonly PlayerService _playerService;

    public LivesPresenter(LivesView view, PlayerService playerService)
    {
        _view = view;
        _playerService = playerService;
    }

    public void Initialize()
    {
        _playerService.HealthChanged += OnHealthChanged;
        _view.SetLives(_playerService.ShipModel.Health);
    }

    public void Dispose()
    {
        _playerService.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int health)
    {
        _view.SetLives(health);
    }
}
}
