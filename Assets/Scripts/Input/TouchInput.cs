using System;
using UnityEngine;

public class TouchInput : MonoBehaviour, IInput
{
    [SerializeField] private float _moveSensitivity = 11f;
    [SerializeField] private float _zoomSensitivity = 0.01f;

    private Camera _camera;
    private Touch _firstTouch;
    private Touch _secondTouch;

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
        if (Input.touchCount == 0)
            return;

        if (Input.touchCount < 2)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartedClick?.Invoke(touch.position);
                    break;
                case TouchPhase.Moved:
                    Moving?.Invoke(CalculateDeltaPosition(touch));
                    break;
                case TouchPhase.Ended:
                    EndedClick?.Invoke(touch.position);
                    break;
            }
        }
        else
        {
            var newFirstTouch = Input.GetTouch(0);
            var newSecondTouch = Input.GetTouch(1);

            if (newFirstTouch.phase == TouchPhase.Began || newSecondTouch.phase == TouchPhase.Began)
            {
                _firstTouch = newFirstTouch;
                _secondTouch = newSecondTouch;
                return;
            }

            float startDistance = (_firstTouch.position - _secondTouch.position).magnitude;
            float newDistance = (newFirstTouch.position - newSecondTouch.position).magnitude;
            float offset = newDistance - startDistance;

            Zooming?.Invoke(offset * _zoomSensitivity);

            _firstTouch = newFirstTouch;
            _secondTouch = newSecondTouch;
        }
    }

    private Vector2 CalculateDeltaPosition(Touch touch) => -(_camera.ScreenToViewportPoint(touch.deltaPosition) * _moveSensitivity);
}
