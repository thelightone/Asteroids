using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Presentation
{
public class MobileControlsView : MonoBehaviour
{
    [SerializeField] private MobileJoystickView _joystickView;
    [SerializeField] private Button _bulletButton;
    [SerializeField] private Button _laserButton;

    public event Action<Vector2>? MovementChanged;
    public event Action? MovementStopped;
    public event Action? FireBulletClicked;
    public event Action? FireLaserClicked;

    private void Awake()
    {
        if (_joystickView != null)
        {
            _joystickView.DirectionChanged += OnJoystickDirectionChanged;
        }

        if (_bulletButton != null)
        {
            _bulletButton.onClick.AddListener(OnBulletButtonClick);
        }

        if (_laserButton != null)
        {
            _laserButton.onClick.AddListener(OnLaserButtonClick);
        }
    }

    private void OnDisable()
    {
        MovementStopped?.Invoke();
    }

    private void OnDestroy()
    {
        if (_joystickView != null)
        {
            _joystickView.DirectionChanged -= OnJoystickDirectionChanged;
        }

        if (_bulletButton != null)
        {
            _bulletButton.onClick.RemoveListener(OnBulletButtonClick);
        }

        if (_laserButton != null)
        {
            _laserButton.onClick.RemoveListener(OnLaserButtonClick);
        }
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    private void OnJoystickDirectionChanged(Vector2 direction)
    {
        MovementChanged?.Invoke(direction);
    }

    private void OnBulletButtonClick()
    {
        FireBulletClicked?.Invoke();
    }

    private void OnLaserButtonClick()
    {
        FireLaserClicked?.Invoke();
    }
}

}
