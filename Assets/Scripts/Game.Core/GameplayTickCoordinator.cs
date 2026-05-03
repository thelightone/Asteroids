using UnityEngine;
using Zenject;

namespace Game.Core
{
public sealed class GameplayTickCoordinator : ITickable
{
    private readonly GameplayFacade _gameplayFacade;
    private readonly PlayerWeaponFacade _playerWeaponFacade;

    public GameplayTickCoordinator(
        GameplayFacade gameplayFacade,
        PlayerWeaponFacade playerWeaponFacade)
    {
        _gameplayFacade = gameplayFacade;
        _playerWeaponFacade = playerWeaponFacade;
    }

    public void Tick()
    {
        float deltaTime = Time.deltaTime;
        PlayerInputData playerInput = _gameplayFacade.Tick(deltaTime);
        _playerWeaponFacade.Tick(deltaTime, playerInput);
    }
}

}
