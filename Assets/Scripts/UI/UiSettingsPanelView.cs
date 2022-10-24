using UnityEngine;

namespace Assets.Scripts
{
    public class UiSettingsPanelView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private UISettingsButtons _buttons;

        private void OnEnable()
        {
            _buttons.PanelOpenClicked += OnOpenButtonClicked;
            _buttons.PanelCloseClicked += OnCloseButtonClicked;
        }

        private void OnDisable()
        {
            _buttons.PanelOpenClicked -= OnOpenButtonClicked;            
            _buttons.PanelCloseClicked -= OnCloseButtonClicked;
        }

        private void OnOpenButtonClicked()
        {
            Extentions.EnableGroup(_canvasGroup);
        }

        private void OnCloseButtonClicked()
        {
            Extentions.DisableGroup(_canvasGroup);
        }
    }
}
