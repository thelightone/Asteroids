using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Signals;

namespace Game.Core
{
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
        if (_rewards.TryGetValue(type, out int reward))
        {
            Score += reward;
            _signalBus.Fire(new ScoreChangedSignal(Score));
        }
        else
        {
            Debug.LogError("No reward configured for enemy type: " + type);
        }
    }
}
}
