using UnityEngine;
using UnityEngine.UI;

public class MobileControlsView : MonoBehaviour
{
    [SerializeField] private MobileJoystickView _joystickView;
    [SerializeField] private Button _bulletButton;
    [SerializeField] private Button _laserButton;

    private MobileInputReader _inputReader;

    public void Initialize(MobileInputReader inputReader)
    {
        _inputReader = inputReader;

        if (_joystickView != null)
        {
            _joystickView.DirectionChanged -= OnJoystickDirectionChanged;
            _joystickView.DirectionChanged += OnJoystickDirectionChanged;
        }

        if (_bulletButton != null)
        {
            _bulletButton.onClick.RemoveListener(OnBulletButtonClick);
            _bulletButton.onClick.AddListener(OnBulletButtonClick);
        }

        if (_laserButton != null)
        {
            _laserButton.onClick.RemoveListener(OnLaserButtonClick);
            _laserButton.onClick.AddListener(OnLaserButtonClick);
        }
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    private void OnJoystickDirectionChanged(Vector2 direction)
    {
        if (_inputReader == null)
        {
            return;
        }

        _inputReader.SetMovement(direction);
    }

    private void OnBulletButtonClick()
    {
        if (_inputReader == null)
        {
            return;
        }

        _inputReader.QueueFireBullet();
    }

    private void OnLaserButtonClick()
    {
        if (_inputReader == null)
        {
            return;
        }

        _inputReader.QueueFireLaser();
    }

    private void OnDisable()
    {
        _inputReader?.SetMovement(Vector2.zero);
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
}
