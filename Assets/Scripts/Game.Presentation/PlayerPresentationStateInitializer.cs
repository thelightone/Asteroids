using Zenject;

using Game.Core;

namespace Game.Presentation
{
public sealed class PlayerPresentationStateInitializer : IInitializable
{
    private readonly PlayerService _playerService;

    public PlayerPresentationStateInitializer(PlayerService playerService)
    {
        _playerService = playerService;
    }

    public void Initialize()
    {
        _playerService.PublishPresentationState();
    }
}

}
