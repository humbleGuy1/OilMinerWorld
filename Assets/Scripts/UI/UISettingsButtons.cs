using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UISettingsButtons : MonoBehaviour
    {
        [SerializeField] private UpgradeMenu _upgradeMenu;
        [SerializeField] private Button _settingsOpen;
        [SerializeField] private Button _back;

        private bool _isPanelOpen = false;

        public event Action PanelOpenClicked;
        public event Action PanelCloseClicked;

        private void OnEnable()
        {
            _back.onClick.AddListener(OnBackClicked);
            _settingsOpen.onClick.AddListener(OnOpenClicked);
        }

        private void OnDisable()
        {
            _back.onClick.RemoveListener(OnBackClicked);
            _settingsOpen.onClick.RemoveListener(OnOpenClicked);
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape) && _isPanelOpen && _upgradeMenu.HasOpened == false)
                OnBackClicked();
            else if(Input.GetKeyUp(KeyCode.Escape) && _isPanelOpen == false && _upgradeMenu.HasOpened == false)
                OnOpenClicked();
        }

        private void OnOpenClicked()
        {
            _isPanelOpen = true;
            PanelOpenClicked?.Invoke();
        }

        private void OnBackClicked()
        {
            _isPanelOpen = false;
            PanelCloseClicked?.Invoke();
        }
    }
}
