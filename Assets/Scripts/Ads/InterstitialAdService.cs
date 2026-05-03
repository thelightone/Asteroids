using Game.Core;

public sealed class InterstitialAdService : IInterstitialAdService
{
    private readonly UnityAdsService _unityAdsService;

    public InterstitialAdService(UnityAdsService unityAdsService)
    {
        _unityAdsService = unityAdsService;
    }

    public void RequestInterstitial()
    {
        _unityAdsService.ShowInterstitial();
    }
}
