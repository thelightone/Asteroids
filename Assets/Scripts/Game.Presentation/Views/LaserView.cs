using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserView : MonoBehaviour
{
    [SerializeField] private float _visibleDuration = 0.08f;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _lineRenderer.positionCount = 2;
    }

    public void Show(Vector2 start, Vector2 end)
    {
        _lineRenderer.SetPosition(0, new Vector3(start.x, start.y, 0f));
        _lineRenderer.SetPosition(1, new Vector3(end.x, end.y, 0f));

        ShowRoutine().Forget();
    }

    private async UniTaskVoid ShowRoutine()
    {
        _lineRenderer.enabled = true;
        await UniTask.Delay((int)(_visibleDuration * 1000));
        _lineRenderer.enabled = false;
    }
}