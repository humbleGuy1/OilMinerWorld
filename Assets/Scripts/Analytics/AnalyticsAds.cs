using GameAnalyticsSDK;
using UnityEngine;

namespace Assets.Scripts
{
    public class AnalyticsAds : MonoBehaviour
    {
        [SerializeField] private AdsView _adsView;
        [SerializeField] private Analytics _analytics;

        private void OnEnable()
        {
            _adsView.AdsInterstitialShowTriggered += OnAdsInterstitialShowTriggered;
            _adsView.AdsInterstitialShowStarted += OnAdsInterstitialShowStarted;
            _adsView.AdsInterstitialShowCompleted += OnAdsInterstitialShowCompleted;
        }

        private void Start()
        {
            GameAnalyticsILRD.SubscribeIronSourceImpressions();
            GameAnalyticsILRD.SubscribeMaxImpressions();
        }

        private void OnDisable()
        {
            _adsView.AdsInterstitialShowTriggered -= OnAdsInterstitialShowTriggered;
            _adsView.AdsInterstitialShowStarted -= OnAdsInterstitialShowStarted;
            _adsView.AdsInterstitialShowCompleted -= OnAdsInterstitialShowCompleted;
        }

        private void OnAdsInterstitialShowTriggered(string ad_type, string placement, string ad_network)
        {
            _analytics.OnVideoAdsTriggered(ad_type, placement, ad_network);
        }

        private void OnAdsInterstitialShowStarted(string ad_type, string placement, string ad_network)
        {
            _analytics.OnVideoAdsStarted(ad_type, placement, ad_network);
        }

        private void OnAdsInterstitialShowCompleted(string ad_type, string placement, string ad_network)
        {
            _analytics.OnVideoAdsCompleted(ad_type, placement, ad_network);
        }
    }
}
