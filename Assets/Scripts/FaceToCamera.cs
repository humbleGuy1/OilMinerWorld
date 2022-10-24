using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    [SerializeField] private bool _xRotate = true;

    private Transform _camera;

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        transform.forward = _camera.forward;

        if (_xRotate == false)
            transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
}