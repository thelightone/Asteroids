using System;
using Zenject;

public class ScoreViewModel : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;

    public int Score { get; private set; }

    public event Action<int> ScoreChanged;

    public ScoreViewModel(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<ScoreChangedSignal>(OnScoreChangedSignal);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<ScoreChangedSignal>(OnScoreChangedSignal);
    }

    private void OnScoreChangedSignal(ScoreChangedSignal signal)
    {
        Score = signal.Score;
        ScoreChanged?.Invoke(Score);
    }
}