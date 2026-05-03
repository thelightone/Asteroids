using UnityEngine;

[CreateAssetMenu(fileName = "UnityAdsConfig", menuName = "Game/Ads/Unity Ads Config")]
public sealed class UnityAdsConfig : ScriptableObject
{
    [SerializeField] private string gameId;
    [SerializeField] private string interstitialAdUnitId;
    [SerializeField] private bool testMode;

    public string GameId => gameId;
    public string InterstitialAdUnitId => interstitialAdUnitId;
    public bool TestMode => testMode;
}
