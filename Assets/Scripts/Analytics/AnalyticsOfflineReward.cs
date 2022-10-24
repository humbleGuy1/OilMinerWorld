using UnityEngine;

namespace Assets.Scripts
{
    public class AnalyticsOfflineReward : MonoBehaviour
    {
        [SerializeField] private Analytics _analytics;
        [SerializeField] private OfflineBonusButton _offlineBonusButton;

        private void OnEnable()
        {
            _offlineBonusButton.OfflineBonusCliked += OnOfflineBonusCliked;
        }

        private void OnDisable()
        {
            _offlineBonusButton.OfflineBonusCliked -= OnOfflineBonusCliked;            
        }

        private void OnOfflineBonusCliked()
        {
            _analytics.OnEventDone("Offline Bonus Rewarded", (int)Time.realtimeSinceStartup);
        }
    }
}
