using UnityEngine;
using UnityEngine.UI;

namespace Game.Presentation
{
public class LivesView : MonoBehaviour
{
    [SerializeField] private Image[] _lifeIcons;
    [SerializeField] private float _inactiveAlpha = 0.25f;

    public void SetLives(int health)
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
}
}
