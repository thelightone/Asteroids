#nullable enable
using System;
using UnityEngine;
using Zenject;

using Game.Core;
using Game.Infrastructure;

namespace Game.Presentation
{
public sealed class MobileControlsPresenter : IInitializable, IDisposable
{
    private readonly MobileControlsView _view;
    private readonly MobileControlsUiInstallOptions _installOptions;
    private readonly MobileInputReader? _mobileInputReader;

    public MobileControlsPresenter(
        MobileControlsView view,
        MobileControlsUiInstallOptions installOptions,
        MobileInputReader? mobileInputReader)
    {
        _view = view;
        _installOptions = installOptions;
        _mobileInputReader = mobileInputReader;
    }

    public void Initialize()
    {
        if (!_installOptions.BindMobileControlsView)
        {
            _view.SetVisible(false);
            return;
        }

        MobileInputReader reader = _mobileInputReader!;
        _view.MovementChanged += reader.SetMovement;
        _view.MovementStopped += OnMovementStopped;
        _view.FireBulletClicked += reader.QueueFireBullet;
        _view.FireLaserClicked += reader.QueueFireLaser;
        _view.SetVisible(true);
    }

    private void OnMovementStopped()
    {
        _mobileInputReader?.SetMovement(Vector2.zero);
    }

    public void Dispose()
    {
        if (!_installOptions.BindMobileControlsView || _mobileInputReader == null)
        {
            return;
        }

        MobileInputReader reader = _mobileInputReader;
        _view.MovementChanged -= reader.SetMovement;
        _view.MovementStopped -= OnMovementStopped;
        _view.FireBulletClicked -= reader.QueueFireBullet;
        _view.FireLaserClicked -= reader.QueueFireLaser;
    }
}

}
