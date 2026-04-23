using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScoreService : IInitializable, IDisposable
{
    public int Score { get; private set; }

    private readonly Dictionary<EnemyType, int> _rewards;
    private readonly SignalBus _signalBus;

    public ScoreService(Dictionary<EnemyType, int> rewards, SignalBus signalBus)
    {
        _rewards = rewards;
        _signalBus = signalBus;
        Score = 0;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
    }

    private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
    {
        AddReward((EnemyType)signal.EnemyTypeValue);
    }

    public void AddReward(EnemyType type)
    {
        Debug.Log("AddReward called for type: " + type);

        if (_rewards.TryGetValue(type, out int reward))
        {
            Score += reward;
            Debug.Log("Reward found: " + reward + ", total score: " + Score);
            _signalBus.Fire(new ScoreChangedSignal(Score));
        }
        else
        {
            Debug.LogError("No reward configured for enemy type: " + type);
        }
    }

    public void Reset()
    {
        Score = 0;
        _signalBus.Fire(new ScoreChangedSignal(Score));
    }
}