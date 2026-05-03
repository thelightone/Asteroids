using Game.Core;
using Game.Infrastructure;
using Game.Signals;

namespace Game.Presentation
{
public readonly struct MobileControlsUiInstallOptions
{
    public bool BindMobileControlsView { get; }
    public MobileControlsView View { get; }

    public MobileControlsUiInstallOptions(bool bindMobileControlsView, MobileControlsView view)
    {
        BindMobileControlsView = bindMobileControlsView;
        View = view;
    }
}

}
