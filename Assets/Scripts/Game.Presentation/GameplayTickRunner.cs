using UnityEngine;
using Zenject;

[DefaultExecutionOrder(-50)]
public class GameplayTickRunner : MonoBehaviour
{
    private GameplayFacade _gameplayFacade;
    private PlayerWeaponFacade _playerWeaponFacade;

    [Inject]
    public void Construct(GameplayFacade gameplayFacade, PlayerWeaponFacade playerWeaponFacade)
    {
        _gameplayFacade = gameplayFacade;
        _playerWeaponFacade = playerWeaponFacade;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        _gameplayFacade.Tick(deltaTime);
        _playerWeaponFacade.Tick(deltaTime);
    }
}
