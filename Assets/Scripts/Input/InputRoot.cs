using UnityEditor;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static Assets.Scripts.CellData;
using System;

namespace Assets.Scripts
{
    public class InputRoot : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private EndGame _endGame;
        [SerializeField] private CellBuyer _cellBuyer;
        [SerializeField] private MouseInput _mouseInput;
        [SerializeField] private TouchInput _touchInput;
        [SerializeField] private FingerHint _fingerHint;
        [SerializeField] private CameraMoventToTargetUpgrade _upgradeMenu;
        [SerializeField] private CameraBounds _cameraBounds;
        [SerializeField] private CinemachineVirtualCamera _currentCamera;

        private bool _hasUpgradeMenuActive = false;
        private IInput _input;
        private float ZoomModifier => _camera.transform.position.y * 0.1f;
        private float _currentZoom;
        private Vector2 _screenModifier;
        private Vector2 _startClickPosition;

        private const float Epsilon = 50f;

        public float NormalizedCurrentZoom => Mathf.Abs(_cameraBounds.NearZoomLimit - _currentZoom) / Mathf.Abs(_cameraBounds.NearZoomLimit - _cameraBounds.FarZoomLimit);

        public event Action<Cell> UpgradeClicked;

        private void Awake()
        {
            _screenModifier = new Vector2((float)_camera.pixelWidth / _camera.pixelHeight, 1);
#if UNITY_EDITOR
            _input = _mouseInput;
#else
            _input = _touchInput;
#endif
            _input.Initialize(_camera);
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            _upgradeMenu.UpgradePanelActivated += OnUpgradeActivated;
            _input.StartedClick += OnStartedClick;
            _input.EndedClick += OnEndedClick;
            _input.Zooming += OnZooming;
            _input.Moving += OnMoving;
        }

        private void OnDisable()
        {
            _upgradeMenu.UpgradePanelActivated -= OnUpgradeActivated;
            _input.StartedClick -= OnStartedClick;
            _input.EndedClick -= OnEndedClick;
            _input.Zooming -= OnZooming;
            _input.Moving -= OnMoving;
        }

        public void SetCameraBounds(CameraBounds cameraBounds)
        {
            _cameraBounds = cameraBounds;
        }

        public Vector3 ConvertInputPosition(Vector2 inputPosition)
            => new Vector3(inputPosition.x * _screenModifier.x, 0, inputPosition.y * _screenModifier.y) * ZoomModifier;

        private void OnUpgradeActivated(bool active)
        {
            _hasUpgradeMenuActive = active;
        }

        private void OnStartedClick(Vector2 screenPosition)
        {
            if (_endGame.IsMenuShowed)
                return;

            _startClickPosition = screenPosition;
        }

        private void OnMoving(Vector2 deltaPosition)
        {
            if (_endGame.IsMenuShowed || _currentCamera == null || _hasUpgradeMenuActive)
                return;

            Vector3 newCameraPosition = _currentCamera.transform.position + ConvertInputPosition(deltaPosition);
            newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, _cameraBounds.Left, _cameraBounds.Right);
            newCameraPosition.z = Mathf.Clamp(newCameraPosition.z, _cameraBounds.Bottom, _cameraBounds.Top);

            _currentCamera.transform.position = newCameraPosition;
        }

        private void OnEndedClick(Vector2 screenPosition)
        {
            if (_endGame.IsMenuShowed)
                return;

            if ((screenPosition - _startClickPosition).magnitude > Epsilon)
                return;

            Ray ray = _camera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (IsPointerOverUIObject(screenPosition) == false && hit.transform.TryGetComponent(out Cell cell))
                {
                    ClickOn(cell);
                }

            }
        }

        public void OnZooming(float zoomDelta)
        {
            if (_endGame.IsMenuShowed || _currentCamera == null || _hasUpgradeMenuActive)
                return;
            
            if (zoomDelta == 0)
                return;

            float oldZoom = _currentZoom;

            _currentZoom = Mathf.Clamp(_currentZoom + zoomDelta, _cameraBounds.NearZoomLimit, _cameraBounds.FarZoomLimit);
            Vector3 newCameraPosition = _currentCamera.transform.position + _currentCamera.transform.forward * (_currentZoom - oldZoom);

            _currentCamera.transform.position = newCameraPosition;
            
        }

        public void ClickOn(Cell cell)
        {
            if (cell.IsBlocked)
                return;

            if (cell.CellState == CellState.Opened && (cell.CellType == CellType.LoaderHouse || cell.CellType == CellType.DiggersHouse) && (_fingerHint == null || _fingerHint.gameObject.activeSelf == false || _fingerHint.IsDiggerHouseHintStarted))
            {
                if(UpgradeMenu.IsOpen == false)
                    UpgradeClicked?.Invoke(cell);   
            }
            else if (cell.CanStartDig)
                if (_fingerHint == null || _fingerHint.gameObject.activeSelf == false || _fingerHint.IsFirstDigHintActivated || _fingerHint.IsSpeedHintFinished)
                    _cellBuyer.TryBuy(cell);
        }

        private bool IsPointerOverUIObject(Vector2 inputPosition)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = inputPosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            for (int i = 0; i < results.Count; i++)
                if (results[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;

            return false;
        }


#if UNITY_EDITOR
        [ContextMenu("InitializeComponentsOnScene")]
        private void InitializeComponentsOnScene()
        {
            _camera = FindObjectOfType<Camera>();
            _endGame = FindObjectOfType<EndGame>();
            _cellBuyer = FindObjectOfType<CellBuyer>();
            _mouseInput = FindObjectOfType<MouseInput>();
            _touchInput = FindObjectOfType<TouchInput>();
            _fingerHint = FindObjectOfType<FingerHint>();
            _cameraBounds = FindObjectOfType<CameraBounds>();
            _currentCamera = FindObjectOfType<CinemachineVirtualCamera>();

            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}
