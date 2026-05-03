using TMPro;
using UnityEngine;

namespace Game.Presentation
{
public class GameOverView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void SetGameOverVisible(bool visible)
    {
        _text.enabled = visible;
    }
}
}
