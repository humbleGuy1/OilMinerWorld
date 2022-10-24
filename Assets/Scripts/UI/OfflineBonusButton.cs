using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class OfflineBonusButton : MonoBehaviour
    {
        [SerializeField] private Button _getOfflineBonus;
        [SerializeField] private Button _cancelOfflineBonus;
        [SerializeField] private CanvasGroup _canvasGroup;

        public event Action OfflineBonusCliked;

        private void OnEnable()
        {
            _cancelOfflineBonus.onClick.AddListener(OnCanceled);
            _getOfflineBonus.onClick.AddListener(Disable);
            _getOfflineBonus.onClick.AddListener(OnOfflineBonusCliked);
        }

        private void OnDisable()
        {
            _cancelOfflineBonus.onClick.RemoveListener(OnCanceled);
            _getOfflineBonus.onClick.RemoveListener(Disable);            
            _getOfflineBonus.onClick.RemoveListener(OnOfflineBonusCliked);            
        }

        public void Enable()
        {
            Extentions.EnableGroup(_canvasGroup);
        }

        private void Disable()
        {
            Extentions.DisableGroup(_canvasGroup);
        }

        private void OnCanceled()
        {
            Extentions.DisableGroup(_canvasGroup);
            Destroy(gameObject,Extentions.Delay);
        }

        private void OnOfflineBonusCliked()
        {
            OfflineBonusCliked?.Invoke();
        }
    }
}
