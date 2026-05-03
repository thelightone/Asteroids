using Game.Core;

namespace Game.Infrastructure
{
public class GameConfigService
{
    private const string PlayerConfigFileName = "playerConfig.json";
    private const string BulletConfigFileName = "bulletConfig.json";
    private const string LaserConfigFileName = "laserConfig.json";
    private const string EnemyConfigFileName = "enemyConfig.json";
    private const string WorldConfigFileName = "worldConfig.json";
    private const string GameFlowConfigFileName = "gameFlowConfig.json";

    public PlayerConfig PlayerConfig { get; private set; }
    public BulletConfig BulletConfig { get; private set; }
    public LaserConfig LaserConfig { get; private set; }
    public EnemyConfig EnemyConfig { get; private set; }
    public WorldConfig WorldConfig { get; private set; }
    public GameFlowConfig GameFlowConfig { get; private set; }

    private readonly JsonConfigLoader _loader;

    public GameConfigService(JsonConfigLoader loader)
    {
        _loader = loader;
        LoadAll();
    }

    public void LoadAll()
    {
        PlayerConfig = _loader.Load<PlayerConfig>(PlayerConfigFileName);
        BulletConfig = _loader.Load<BulletConfig>(BulletConfigFileName);
        LaserConfig = _loader.Load<LaserConfig>(LaserConfigFileName);
        EnemyConfig = _loader.Load<EnemyConfig>(EnemyConfigFileName);
        WorldConfig = _loader.Load<WorldConfig>(WorldConfigFileName);
        GameFlowConfig = _loader.Load<GameFlowConfig>(GameFlowConfigFileName);
    }
}
}
