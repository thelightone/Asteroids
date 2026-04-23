using TMPro;
using UnityEngine;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private PlayerService _playerService;

    public void Initialize(PlayerService playerService)
    {
        _playerService = playerService;
        _playerService.Died += OnPlayerDied;

        _text.enabled = _playerService.ShipModel.IsDead;
    }

    private void OnPlayerDied()
    {
        _text.enabled = true;
    }

    private void OnDestroy()
    {
        if (_playerService != null)
        {
            _playerService.Died -= OnPlayerDied;
        }
    }
}