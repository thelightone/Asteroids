using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private Vector3 _baseScale;
    private bool _isBaseScaleInitialized;

    private void Awake()
    {
        EnsureBaseScale();
    }

    public void OnSpawn()
    {
    }

    public void OnDespawn()
    {
    }

    public void SetScaleMultiplier(float multiplier)
    {
        EnsureBaseScale();
        transform.localScale = _baseScale * multiplier;
    }

    private void EnsureBaseScale()
    {
        if (_isBaseScaleInitialized)
        {
            return;
        }

        _baseScale = transform.localScale;
        _isBaseScaleInitialized = true;
    }
}