using System;
using UnityEngine;

public class MouseInput : MonoBehaviour, IInput
{
    [SerializeField] private float _moveSensitivity = 11f;
    [SerializeField] private float _zoomSensitivity = 1f;

    private Camera _camera;
    private Vector3 _lastPosition;

    public event Action<float> Zooming;
    public event Action<Vector2> StartedClick;
    public event Action<Vector2> Moving;
    public event Action<Vector2> EndedClick;

    public void Initialize(Camera camera)
    {
        _camera = camera;
    }

    private void Update()
    {
        Zooming?.Invoke(Math.Sign(Input.mouseScrollDelta.y) * _zoomSensitivity);

        if (Input.GetMouseButtonDown(0))
        {
            _lastPosition = Input.mousePosition;
            StartedClick?.Invoke(_lastPosition);
        }

        if (Input.GetMouseButton(0))
        {
            var inputPosition = Input.mousePosition;
            Moving?.Invoke(CalculateDeltaPosition(inputPosition));
            _lastPosition = inputPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndedClick?.Invoke(Input.mousePosition);
        }
    }

    private Vector2 CalculateDeltaPosition(Vector3 inputPosition) => -(_camera.ScreenToViewportPoint(inputPosition - _lastPosition) * _moveSensitivity);
}
