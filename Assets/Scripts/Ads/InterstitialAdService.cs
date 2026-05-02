public sealed class InterstitialAdService : IInterstitialAdService
{
    private readonly AdsTest _adsTest;

    public InterstitialAdService(AdsTest adsTest)
    {
        _adsTest = adsTest;
    }

    public void RequestInterstitial()
    {
        _adsTest.ShowInterstitial();
    }
}
