using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Zenject;

public class PlayerTestRunner : MonoBehaviour
{
    private PlayerService _playerService;
    private GameplayFacade _gameplayFacade;
    private PlayerWeaponFacade _playerWeaponFacade;
    private EnemyViewSynchronizer _enemyViewSynchronizer;
    private BulletViewSynchronizer _bulletViewSynchronizer;
    private ShipStatsViewModel _shipStatsViewModel;
    private SignalBus _signalBus;
    private ScoreViewModel _scoreViewModel;

    [SerializeField] private PlayerView _playerView;
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private LivesView _livesView;
    [SerializeField] private ShipInvulnerabilityView _shipInvulnerabilityView;
    [SerializeField] private GameOverView _gameOverView;
    [SerializeField] private ShipStatsView _shipStatsView;
    [SerializeField] private LaserView _laserView;

    private bool _isRestarting;

    [Inject]
    public void Construct(
        PlayerService playerService,
        GameplayFacade gameplayFacade,
        PlayerWeaponFacade playerWeaponFacade,
        EnemyViewSynchronizer enemyViewSynchronizer,
        BulletViewSynchronizer bulletViewSynchronizer,
        ShipStatsViewModel shipStatsViewModel,
        SignalBus signalBus,
        ScoreViewModel scoreViewModel)
    {
        _playerService = playerService;
        _gameplayFacade = gameplayFacade;
        _playerWeaponFacade = playerWeaponFacade;
        _enemyViewSynchronizer = enemyViewSynchronizer;
        _bulletViewSynchronizer = bulletViewSynchronizer;
        _shipStatsViewModel = shipStatsViewModel;
        _signalBus = signalBus;
        _scoreViewModel = scoreViewModel;
    }

    private void Start()
    {
        _playerView.Initialize(_playerService);
        _livesView.Initialize(_playerService);
        _shipInvulnerabilityView.Initialize(_playerService);
        _gameOverView.Initialize(_playerService);
        _shipStatsView.Initialize(_shipStatsViewModel);
        _scoreView.Initialize(_scoreViewModel);

        _signalBus.Subscribe<LaserFiredSignal>(OnLaserFired);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        _gameplayFacade.Tick(deltaTime);
        _playerWeaponFacade.Tick(deltaTime);

        _enemyViewSynchronizer.Sync();
        _bulletViewSynchronizer.Sync();

        if (_playerService.ShipModel.IsDead && !_isRestarting)
        {
            RestartAfterDelay().Forget();
        }
    }

    private async UniTaskVoid RestartAfterDelay()
    {
        _isRestarting = true;
        await UniTask.Delay(2000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnLaserFired(LaserFiredSignal signal)
    {
        _laserView.Show(signal.Start, signal.End);
    }

    private void OnDestroy()
    {
        if (_signalBus != null)
        {
            _signalBus.TryUnsubscribe<LaserFiredSignal>(OnLaserFired);
        }
    }
}