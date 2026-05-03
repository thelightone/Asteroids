using UnityEngine;
using Zenject;

using Game.Core;
using Game.Infrastructure;
using Game.Presentation;
using Game.Signals;

namespace Game.Installers
{
public static class InputInstaller
{
    public static MobileControlsUiInstallOptions Install(
        DiContainer container,
        MobileControlsView mobileControlsView,
        bool forceMobileInputInEditor)
    {
        bool shouldUseMobileInput = ShouldUseMobileInput(forceMobileInputInEditor);

        MobileControlsUiInstallOptions options;

        if (shouldUseMobileInput && mobileControlsView != null)
        {
            var mobileInputReader = new MobileInputReader();
            container.Bind<MobileInputReader>().FromInstance(mobileInputReader).AsSingle();
            container.Bind<IInputReader>().FromInstance(mobileInputReader).AsSingle();

            options = new MobileControlsUiInstallOptions(true, mobileControlsView);
        }
        else
        {
            container.Bind<IInputReader>().To<KeyboardInputReader>().AsSingle();

            if (shouldUseMobileInput)
            {
                Debug.LogWarning(
                    "Mobile platform detected, but MobileControlsView is not assigned. KeyboardInputReader fallback is used.");
            }

            options = new MobileControlsUiInstallOptions(false, mobileControlsView);
        }

        if (mobileControlsView != null)
        {
            container.Bind<MobileControlsUiInstallOptions>().FromInstance(options).AsSingle();
        }

        return options;
    }

    private static bool ShouldUseMobileInput(bool forceMobileInputInEditor)
    {
        if (Application.isMobilePlatform)
        {
            return true;
        }

#if UNITY_EDITOR
        return forceMobileInputInEditor;
#else
        return false;
#endif
    }
}

}
