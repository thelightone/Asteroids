using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private ScoreViewModel _viewModel;

    public void Initialize(ScoreViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.ScoreChanged += OnScoreChanged;

        OnScoreChanged(_viewModel.Score);
    }

    private void OnScoreChanged(int score)
    {
        _text.text = $"Score: {score}";
    }

    private void OnDestroy()
    {
        if (_viewModel != null)
        {
            _viewModel.ScoreChanged -= OnScoreChanged;
        }
    }
}