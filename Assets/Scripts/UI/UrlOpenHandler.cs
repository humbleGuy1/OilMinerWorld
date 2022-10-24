using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UrlOpenHandler : MonoBehaviour
    {
        [SerializeField] private string _urlPrivacyPolicy;
        [SerializeField] private string _urlTermsOfService;
        [SerializeField] private Button _privacyPolicy;
        [SerializeField] private Button _termsOfService;

        private void OnEnable()
        {
            _privacyPolicy.onClick.AddListener(OnPrivacyClicked);
            _termsOfService.onClick.AddListener(OnTermsOfServiceClicked);
        }

        private void OnDisable()
        {
            _privacyPolicy.onClick.RemoveListener(OnPrivacyClicked);            
            _termsOfService.onClick.RemoveListener(OnTermsOfServiceClicked);
        }

        private void OnPrivacyClicked()
        {
            if (_urlPrivacyPolicy == null)
                return;

            Application.OpenURL(_urlPrivacyPolicy);
        }

        private void OnTermsOfServiceClicked()
        {
            if (_urlTermsOfService == null)
                return;

            Application.OpenURL(_urlTermsOfService);
        }
    }
}

