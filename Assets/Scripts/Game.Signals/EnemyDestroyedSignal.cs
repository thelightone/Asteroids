namespace Game.Signals
{
public readonly struct EnemyDestroyedSignal
{
    public readonly int EnemyTypeValue;

    public EnemyDestroyedSignal(int enemyTypeValue)
    {
        EnemyTypeValue = enemyTypeValue;
    }
}
}
