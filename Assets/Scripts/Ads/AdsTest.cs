using UnityEngine;
using UnityEngine.Advertisements;

public class AdsTest : MonoBehaviour,
    IUnityAdsInitializationListener,
    IUnityAdsLoadListener,
    IUnityAdsShowListener
{
    private const string GameId = "6099580";
    private const string InterstitialId = "Interstitial_Android";

    private bool _isLoaded;

    private void Start()
    {
        Advertisement.Initialize(GameId, true, this);
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
        Debug.Log("[Ads] Loading interstitial...");
        Advertisement.Load(InterstitialId, this);
    }

    public void ShowInterstitial()
    {
        Debug.Log("[Ads] ShowInterstitial called");

        if (!_isLoaded)
        {
            Debug.LogWarning("[Ads] Ad is not loaded yet");
            LoadAd();
            return;
        }

        _isLoaded = false;
        Advertisement.Show(InterstitialId, this);
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