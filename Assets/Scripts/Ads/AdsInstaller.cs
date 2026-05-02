using UnityEngine;
using Zenject;

public sealed class AdsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AdsTest>().FromComponentOn(gameObject).AsSingle();
        Container.Bind<IInterstitialAdService>().To<InterstitialAdService>().AsSingle();
    }
}
