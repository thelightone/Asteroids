using Zenject;

using Game.Core;
using Game.Infrastructure;
using Game.Presentation;
using Game.Signals;

namespace Game.Installers
{
public static class ConfigInstaller
{
    public static void Install(DiContainer container)
    {
        container.Bind<JsonConfigLoader>().AsSingle();
        container.Bind<GameConfigService>().AsSingle();
    }
}

}
