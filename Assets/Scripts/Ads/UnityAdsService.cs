using UnityEngine;
using UnityEngine.Advertisements;

public sealed class UnityAdsService : MonoBehaviour,
    IUnityAdsInitializationListener,
    IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    [SerializeField] private UnityAdsConfig config;

    private bool _isLoaded;
    private string AdUnitId => config != null ? config.InterstitialAdUnitId : string.Empty;

    private void Start()
    {
        if (config == null)
        {
            Debug.LogError("[Ads] UnityAdsConfig is not assigned on UnityAdsService.");
            return;
        }

        if (string.IsNullOrWhiteSpace(config.GameId) || string.IsNullOrWhiteSpace(config.InterstitialAdUnitId))
        {
            Debug.LogError("[Ads] UnityAdsConfig: Game Id and Interstitial Ad Unit Id must be set.");
            return;
        }

        Advertisement.Initialize(config.GameId, config.TestMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("[Ads] Initialized");
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"[Ads] Init failed: {error} - {message}");
    }

    private void LoadAd()
    {
        if (config == null || string.IsNullOrEmpty(AdUnitId))
            return;

        Debug.Log("[Ads] Loading interstitial...");
        Advertisement.Load(AdUnitId, this);
    }

    public void ShowInterstitial()
    {
        Debug.Log("[Ads] ShowInterstitial called");

        if (config == null)
        {
            Debug.LogError("[Ads] UnityAdsConfig is not assigned.");
            return;
        }

        if (!_isLoaded)
        {
            Debug.LogWarning("[Ads] Ad is not loaded yet");
            LoadAd();
            return;
        }

        _isLoaded = false;
        Advertisement.Show(AdUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log($"[Ads] Loaded: {adUnitId}");
        _isLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"[Ads] Load failed: {adUnitId} - {error} - {message}");
        _isLoaded = false;
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log($"[Ads] Show started: {adUnitId}");
    }

    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"[Ads] Show complete: {adUnitId}");
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"[Ads] Show failed: {adUnitId} - {error} - {message}");
        LoadAd();
    }
}
