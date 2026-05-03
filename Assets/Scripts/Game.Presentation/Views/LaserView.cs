using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Presentation
{
[RequireComponent(typeof(LineRenderer))]
public class LaserView : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private int _showVersion;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _lineRenderer.positionCount = 2;
    }

    private void OnDisable()
    {
        _showVersion++;

        if (_lineRenderer != null)
        {
            _lineRenderer.enabled = false;
        }
    }

    public void Show(Vector2 start, Vector2 end, float visibleDurationSeconds)
    {
        _lineRenderer.SetPosition(0, new Vector3(start.x, start.y, 0f));
        _lineRenderer.SetPosition(1, new Vector3(end.x, end.y, 0f));
        _lineRenderer.enabled = true;

        _showVersion++;
        ShowRoutine(_showVersion, visibleDurationSeconds).Forget();
    }

    private async UniTask ShowRoutine(int showVersion, float visibleDurationSeconds)
    {
        await UniTask.Delay((int)(visibleDurationSeconds * 1000f));

        if (showVersion != _showVersion || _lineRenderer == null)
        {
            return;
        }

        _lineRenderer.enabled = false;
    }
}

}
