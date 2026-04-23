public class GameConfigService
{
    public PlayerConfig PlayerConfig { get; private set; }
    public BulletConfig BulletConfig { get; private set; }
    public LaserConfig LaserConfig { get; private set; }
    public EnemyConfig EnemyConfig { get; private set; }
    public WorldConfig WorldConfig { get; private set; }

    private readonly JsonConfigLoader _loader;

    public GameConfigService(JsonConfigLoader loader)
    {
        _loader = loader;
    }

    public void LoadAll()
    {
        PlayerConfig = _loader.Load<PlayerConfig>("playerConfig.json");
        BulletConfig = _loader.Load<BulletConfig>("bulletConfig.json");
        LaserConfig = _loader.Load<LaserConfig>("laserConfig.json");
        EnemyConfig = _loader.Load<EnemyConfig>("enemyConfig.json");
        WorldConfig = _loader.Load<WorldConfig>("worldConfig.json");
    }
}