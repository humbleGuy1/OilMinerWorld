using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class RegionCostHandler : MonoBehaviour
    {
        [SerializeField] private Region[] _regions;
        [SerializeField] private UnblockButton[] _unblockButton;

        public event Action CoastIncreased;

        private void OnEnable()
        {
            for (int i = 0; i < _unblockButton.Length; i++)
                _unblockButton[i].Clicked += OnRegionBuyed;
        }

        private void OnDisable()
        {
            for (int i = 0; i < _unblockButton.Length; i++)
                _unblockButton[i].Clicked -= OnRegionBuyed;
        }

        private void OnRegionBuyed()
        {
            CoastIncreased?.Invoke();
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(Init))]
        private void Init()
        {
            _regions = GetComponentsInChildren<Region>();
            _unblockButton = GetComponentsInChildren<UnblockButton>();
        }
#endif
    }
}
