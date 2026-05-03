using Game.Core;
using UnityEngine;
using Zenject;

public sealed class AdsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<UnityAdsService>().FromComponentOn(gameObject).AsSingle();
        Container.Bind<IInterstitialAdService>().To<InterstitialAdService>().AsSingle();
    }
}
