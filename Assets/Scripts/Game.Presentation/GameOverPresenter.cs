using System;
using Zenject;

using Game.Core;

namespace Game.Presentation
{
public sealed class GameOverPresenter : IInitializable, IDisposable
{
    private readonly GameOverView _view;
    private readonly PlayerService _playerService;

    public GameOverPresenter(GameOverView view, PlayerService playerService)
    {
        _view = view;
        _playerService = playerService;
    }

    public void Initialize()
    {
        _playerService.Died += OnPlayerDied;
        _view.SetGameOverVisible(_playerService.ShipModel.IsDead);
    }

    public void Dispose()
    {
        _playerService.Died -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        _view.SetGameOverVisible(true);
    }
}
}
