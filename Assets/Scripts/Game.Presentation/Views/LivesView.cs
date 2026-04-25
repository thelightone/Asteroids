using UnityEngine;
using UnityEngine.UI;

public class LivesView : MonoBehaviour
{
    [SerializeField] private Image[] _lifeIcons;
    [SerializeField] private float _inactiveAlpha = 0.25f;

    private PlayerService _playerService;

    public void Initialize(PlayerService playerService)
    {
        _playerService = playerService;
        _playerService.HealthChanged += OnHealthChanged;

        OnHealthChanged(_playerService.ShipModel.Health);
    }

    private void OnHealthChanged(int health)
    {
        if (_lifeIcons == null || _lifeIcons.Length == 0)
        {
            return;
        }

        int clampedHealth = Mathf.Clamp(health, 0, _lifeIcons.Length);

        for (int i = 0; i < _lifeIcons.Length; i++)
        {
            Image lifeIcon = _lifeIcons[i];

            if (lifeIcon == null)
            {
                continue;
            }

            Color iconColor = lifeIcon.color;
            iconColor.a = i < clampedHealth ? 1f : _inactiveAlpha;
            lifeIcon.color = iconColor;
        }
    }

    private void OnDestroy()
    {
        if (_playerService != null)
        {
            _playerService.HealthChanged -= OnHealthChanged;
        }
    }
}