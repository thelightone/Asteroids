using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Zenject;

public class GameRestartService : MonoBehaviour
{
    private PlayerService _playerService;
    private IInterstitialAdService _interstitialAdService;
    private int _restartDelayMilliseconds;

    private bool _isRestarting;

    [Inject]
    public void Construct(
        PlayerService playerService,
        IInterstitialAdService interstitialAdService,
        GameConfigService gameConfigService)
    {
        _playerService = playerService;
        _interstitialAdService = interstitialAdService;
        _restartDelayMilliseconds = gameConfigService.GameFlowConfig.RestartDelayMilliseconds;
    }

    private void Update()
    {
        if (_playerService.ShipModel.IsDead && !_isRestarting)
        {
            RestartAfterDelay(this.GetCancellationTokenOnDestroy()).Forget(Debug.LogException);
        }
    }

    private async UniTask RestartAfterDelay(CancellationToken cancellationToken)
    {
        _isRestarting = true;
        try
        {
            await UniTask.Delay(_restartDelayMilliseconds, cancellationToken: cancellationToken);
            _interstitialAdService.RequestInterstitial();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        catch (OperationCanceledException)
        {
        }
    }
}
