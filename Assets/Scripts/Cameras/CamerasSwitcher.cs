using UnityEngine;
using Cinemachine;

namespace Assets.Scripts
{
    public abstract class CamerasSwitcher : MonoBehaviour
    {
        [Header("Cameras")]
        [SerializeField] protected CinemachineVirtualCamera _mainAnthillMapCamera1;
        [SerializeField] protected CinemachineVirtualCamera _mainAnthillMapCamera2;

        protected CinemachineVirtualCamera[] _cameras;

        private const float Delay = 0.5f;
        private const int MinPrioritet = 1;
        private const int MaxPrioritet = 2;

        private void OnEnable()
        {
            _cameras = GetComponentsInChildren<CinemachineVirtualCamera>();
            Invoke(nameof(Init), Delay);
        }

        protected virtual void Init() { }

        protected void EnableCamera(CinemachineVirtualCamera mainCamera)
        {
            ResetPriority();
            mainCamera.Priority = MaxPrioritet;
        }

        private void ResetPriority()
        {
            foreach (CinemachineVirtualCamera camera in _cameras)
                camera.Priority = MinPrioritet;
        }
    }
}
