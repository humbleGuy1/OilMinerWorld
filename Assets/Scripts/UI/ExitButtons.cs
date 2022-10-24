using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ExitButtons : MonoBehaviour
    {
        [SerializeField] private float _saveTime = 4f;
        [SerializeField] private CanvasGroup _exitPanel;
        [SerializeField] private Button _yes;
        [SerializeField] private Button _no;
        [SerializeField] private CanvasGroup _anthillsButtons;

        private bool _isOpen = false;
        private bool _firstTapBack = false;
        private float _elapsedTime;

        private void OnEnable()
        {
            _yes.onClick.AddListener(OnYesClicked);
            _no.onClick.AddListener(HidePanel);
        }

        private void OnDisable()
        {
            _yes.onClick.RemoveListener(OnYesClicked);            
            _no.onClick.RemoveListener(HidePanel);
        }

        private void Update()
        {
            if (_isOpen)
                return;

            if (Input.GetKeyUp(KeyCode.Escape) && _firstTapBack == false)
            {
                _firstTapBack = true;
                _elapsedTime = _saveTime;
            }
            else
            {
                _elapsedTime -= Time.deltaTime;

                if (Input.GetKeyUp(KeyCode.Escape))
                    ShowPanel();

                if (_elapsedTime <= 0)
                {
                    _firstTapBack = false;
                    _elapsedTime = 0;
                }
            }
        }

        private void ShowPanel()
        {
            _isOpen = true;
            Extentions.EnableGroup(_exitPanel);
            Extentions.DisableGroup(_anthillsButtons);
        }

        private void HidePanel()
        {
            _isOpen = false;
            Extentions.DisableGroup(_exitPanel);
            Extentions.EnableGroup(_anthillsButtons);
        }

        public void OnYesClicked()
        {
            Application.Quit();
        }
    }
}
