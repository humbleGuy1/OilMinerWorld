using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraMoventToTargetUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeMenu _upgradeMenu;
        [SerializeField] private CinemachineVirtualCamera _currentCamera;
        [SerializeField] private float _high = 4f;

        private Vector3 _defaultTransform;
        public static bool IsOnCell { get; private set; }
        private const float OffsetZ = 1f;
        private const float Duration = 0.3f;

        public event Action<bool> UpgradePanelActivated;

        private void OnEnable()
        {
            _upgradeMenu.CellPanelOpened += MoveToCell;
            _upgradeMenu.PanelClosed += ResetPosition;
        }

        private void OnDisable()
        {
            _upgradeMenu.CellPanelOpened -= MoveToCell;            
            _upgradeMenu.PanelClosed -= ResetPosition;
        }

        private void MoveToCell(Cell cell)
        {
            if (_currentCamera == null || cell == null || IsOnCell)
                return;

            IsOnCell = true;
            UpgradePanelActivated?.Invoke(true);
            _defaultTransform = _currentCamera.transform.position;

            Vector3 newCameraPosition;
            newCameraPosition.x = cell.transform.position.x;
            newCameraPosition.z = cell.transform.position.z - OffsetZ;
            newCameraPosition.y = _high;

            DOTween.CompleteAll();
            _currentCamera.transform.DOMove(newCameraPosition, Duration);
        }

        private void ResetPosition(bool moving)
        {
            if (moving == false)
            {
                UpgradePanelActivated?.Invoke(false);
                return;
            }

            _currentCamera.transform.DOMove(_defaultTransform, Duration).OnComplete(() => OnReset());
            
        }

        private void OnReset()
        {
            UpgradePanelActivated?.Invoke(false);
            IsOnCell = false;
        }
    }
}
