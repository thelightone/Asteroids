using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystickView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _baseRect;
    [SerializeField] private RectTransform _handleRect;
    [SerializeField] private float _handleRange = 1f;

    public event Action<Vector2> DirectionChanged;

    private Camera _uiCamera;
    private Vector2 _currentDirection;

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateDirection(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateDirection(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _currentDirection = Vector2.zero;
        UpdateHandle();
        DirectionChanged?.Invoke(_currentDirection);
    }

    private void Awake()
    {
        Canvas rootCanvas = GetComponentInParent<Canvas>();
        _uiCamera = rootCanvas != null && rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? rootCanvas.worldCamera
            : null;
    }

    private void UpdateDirection(PointerEventData eventData)
    {
        if (_baseRect == null)
        {
            return;
        }

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _baseRect,
                eventData.position,
                _uiCamera,
                out Vector2 localPoint))
        {
            return;
        }

        Vector2 radius = _baseRect.sizeDelta * 0.5f;
        if (radius.x <= 0f || radius.y <= 0f)
        {
            return;
        }

        Vector2 normalized = new Vector2(localPoint.x / radius.x, localPoint.y / radius.y);
        _currentDirection = Vector2.ClampMagnitude(normalized, 1f);

        UpdateHandle();
        DirectionChanged?.Invoke(_currentDirection);
    }

    private void UpdateHandle()
    {
        if (_handleRect == null || _baseRect == null)
        {
            return;
        }

        Vector2 radius = _baseRect.sizeDelta * 0.5f * _handleRange;
        _handleRect.anchoredPosition = new Vector2(_currentDirection.x * radius.x, _currentDirection.y * radius.y);
    }
}
