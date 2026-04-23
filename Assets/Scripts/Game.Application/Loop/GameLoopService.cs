using System.Collections.Generic;

public class GameLoopService
{
    private readonly List<IGameTickable> _tickables = new();

    public void Register(IGameTickable tickable)
    {
        if (!_tickables.Contains(tickable))
        {
            _tickables.Add(tickable);
        }
    }

    public void Unregister(IGameTickable tickable)
    {
        _tickables.Remove(tickable);
    }

    public void Tick(float deltaTime)
    {
        for (int i = 0; i < _tickables.Count; i++)
        {
            _tickables[i].Tick(deltaTime);
        }
    }
}