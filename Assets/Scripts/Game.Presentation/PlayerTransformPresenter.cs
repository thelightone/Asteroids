using UnityEngine;
using Zenject;

using Game.Core;

namespace Game.Presentation
{
public sealed class PlayerTransformPresenter : ITickable
{
    private readonly PlayerService _playerService;
    private readonly PlayerView _playerView;

    public PlayerTransformPresenter(PlayerService playerService, PlayerView playerView)
    {
        _playerService = playerService;
        _playerView = playerView;
    }

    public void Tick()
    {
        Vector2 position = _playerService.ShipModel.Position;
        _playerView.transform.position = new Vector3(position.x, position.y, 0f);
        _playerView.transform.rotation = Quaternion.Euler(0f, 0f, _playerService.ShipModel.Rotation);
    }
}
}
