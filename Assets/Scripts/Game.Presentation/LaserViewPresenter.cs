using UnityEngine;
using Zenject;

using Game.Core;
using Game.Infrastructure;
using Game.Signals;

namespace Game.Presentation
{
public class LaserViewPresenter : MonoBehaviour
{
    [SerializeField] private LaserView _laserView;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Start()
    {
        _signalBus.Subscribe<LaserFiredSignal>(OnLaserFired);
    }

    private void OnLaserFired(LaserFiredSignal signal)
    {
        _laserView.Show(signal.Start, signal.End, signal.VisibleDuration);
    }

    private void OnDestroy()
    {
        if (_signalBus != null)
        {
            _signalBus.TryUnsubscribe<LaserFiredSignal>(OnLaserFired);
        }
    }
}

}
