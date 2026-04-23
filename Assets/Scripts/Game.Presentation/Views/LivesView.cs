using TMPro;
using UnityEngine;

public class LivesView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private PlayerService _playerService;

    public void Initialize(PlayerService playerService)
    {
        _playerService = playerService;
        _playerService.HealthChanged += OnHealthChanged;

        OnHealthChanged(_playerService.ShipModel.Health);
    }

    private void OnHealthChanged(int health)
    {
        _text.text = $"Lives: {health}";
    }

    private void OnDestroy()
    {
        if (_playerService != null)
        {
            _playerService.HealthChanged -= OnHealthChanged;
        }
    }
}