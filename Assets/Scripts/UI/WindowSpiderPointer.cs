using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class WindowSpiderPointer : MonoBehaviour
{
    [SerializeField] private Image _pointer;
    [SerializeField] private UiCamera _uiCamera;
    [SerializeField] private float _borderSizeX;
    [SerializeField] private float _borderSizeY;
    [SerializeField] private Transform _targetPoint;

    private Vector3 _targetPosition;
    private Camera _camera;
    private Camera _ui;

    private void Start()
    {
        _camera = Camera.main;
        _uiCamera = FindObjectOfType<UiCamera>();
        _ui = _uiCamera.GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 targetPositionScreenPoint = _camera.WorldToScreenPoint(_targetPosition);

        if (isOffScreen(targetPositionScreenPoint))
        {
            ShowPointer();
            RotatePointer();

            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, _borderSizeX, Screen.width - _borderSizeX);
            cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, _borderSizeY, Screen.height - _borderSizeY);

            Vector3 pointerWorldPosition = _ui.ScreenToWorldPoint(cappedTargetScreenPosition);
            _pointer.rectTransform.position = pointerWorldPosition;
            _pointer.rectTransform.localPosition = new Vector3(_pointer.rectTransform.localPosition.x,
                _pointer.rectTransform.localPosition.y, 0);
        }
        else
        {
            HidePointer();
        }
    }

    public void Initialize(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        //StartCoroutine(EnableDelay());
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void HidePointer()
    {
        _pointer.gameObject.SetActive(false);
    }

    private void ShowPointer()
    {
        _pointer.gameObject.SetActive(true);
    }

    private void RotatePointer()
    {
        Vector3 toPosition = _targetPosition;
        Vector3 fromPosition = _camera.transform.position;
        fromPosition.z = 0;
        Vector3 direction = (toPosition - fromPosition).normalized;
        float angle = GetAngleFromVectorFloat(direction);
        _pointer.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private float GetAngleFromVectorFloat(Vector3 direction)
    {
        direction = direction.normalized;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        if (angle < 0)
            angle += 360;

        return angle;
    }

    private bool isOffScreen(Vector3 targetPositionScreenPoint)
    {
        return targetPositionScreenPoint.x <= 0 || targetPositionScreenPoint.x >= Screen.width
        || targetPositionScreenPoint.y <= 0 || targetPositionScreenPoint.y >= Screen.height;
    }

    private IEnumerator EnableDelay()
    {
        yield return new WaitForSeconds(0.5f);

        _targetPoint.gameObject.SetActive(true);
    }
}
