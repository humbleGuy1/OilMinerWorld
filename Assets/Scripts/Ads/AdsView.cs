using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AdsView : Ads
    {
        private const string FirstLaunch = "FirstLaunch";
        private const float DelayDeforeLoadingBanner = 10f;
        private const float DelayDeforeLoadingInter = 2f;

        [SerializeField] private Image _background;
        [SerializeField] private bool _editor = false;
        [SerializeField] private bool _banner = true;
        [SerializeField] private ADTimerView _adTimerView;

        private Coroutine _adsCoroutine;
        private float _cooldownSec => RemoteConfig.InterConfig.Cooldown;
        private float _defaultStartTimeSec => RemoteConfig.InterConfig.DefaultAppearTime;
        private float _firstEnterStartTimeSec => RemoteConfig.InterConfig.FirstAppearTime;

        public event Action AdsBannerShow;
        public event Action AdsInterstitialShow;
        public event Action<string, string, string> AdsInterstitialShowTriggered;
        public event Action<string, string, string> AdsInterstitialShowStarted;
        public event Action<string, string, string> AdsInterstitialShowCompleted;

        protected override void Subscribe()
        {
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHidden;
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialShowTriggered;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialShowStarted;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialShowCompleted;
        }
        protected override void UnSubscribe()
        {
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdHidden;
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialShowTriggered;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialShowStarted;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialShowCompleted;
        }

        private IEnumerator ShowAdsOnRepeat()
        {
            float startDelay;

            if (PlayerPrefs.HasKey(FirstLaunch) == false)
            {
                PlayerPrefs.SetInt(FirstLaunch, 1);
                startDelay = _firstEnterStartTimeSec;
            }
            else
            {
                startDelay = _defaultStartTimeSec;
            }

            LoadInterstitial();
            yield return new WaitForSeconds(startDelay);
            ShowInterstisial();
            yield return InterOnRepeat();
        }

        private IEnumerator InterOnRepeat()
        {
            var wait = new WaitForSeconds(_cooldownSec - _adTimerView.Duration);
            var waitAdLoading = new WaitForSeconds(DelayDeforeLoadingInter);

            while (true)
            {
                LoadInterstitial();

                yield return wait;

                _adTimerView.StartCountdown();

                yield return new WaitForSeconds(_adTimerView.Duration);

                ShowInterstisial();

                yield return waitAdLoading;
            }
        }

        public void ShowAds()
        {
            if (_editor == false)
                _adsCoroutine = StartCoroutine(ShowAdsOnRepeat());

            if (_banner && _editor == false)
                ShowBanner();
        }

        protected virtual void OnRewardedAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (_adsCoroutine == null)
                return;

            StopCoroutine(_adsCoroutine);
            _adsCoroutine = StartCoroutine(InterOnRepeat());
        }

        private void OnInterstitialShowTriggered(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            AdsInterstitialShowTriggered?.Invoke("interstitial", adInfo.Placement, adInfo.NetworkName);
        }

        private void OnInterstitialShowStarted(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            AdsInterstitialShowStarted?.Invoke("interstitial", adInfo.Placement, adInfo.NetworkName);
        }

        private void OnInterstitialShowCompleted(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            AdsInterstitialShowCompleted?.Invoke("interstitial", adInfo.Placement, adInfo.NetworkName);
        }

        private void ShowBanner()
        {
            MaxSdk.CreateBanner(AdsInitializer.BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
            MaxSdk.SetBannerExtraParameter(AdsInitializer.BannerAdUnitId, "ad_refresh_seconds", "20");
            MaxSdk.SetBannerExtraParameter(AdsInitializer.BannerAdUnitId, "adaptive_banner", "false");
            MaxSdk.SetBannerBackgroundColor(AdsInitializer.BannerAdUnitId, Color.white);

            Invoke(nameof(ShowBannerWithBackground), DelayDeforeLoadingBanner);
            AdsBannerShow?.Invoke();
        }

        private void ShowBannerWithBackground()
        {
            MaxSdk.ShowBanner(AdsInitializer.BannerAdUnitId);
            _background.enabled = true;
        }

        private void LoadInterstitial()
        {
            MaxSdk.LoadInterstitial(AdsInitializer.InterstitialAdUnitId);
        }

        private void ShowInterstisial()
        {
            if (MaxSdk.IsInterstitialReady(AdsInitializer.InterstitialAdUnitId))
            {
                MaxSdk.ShowInterstitial(AdsInitializer.InterstitialAdUnitId);
                AdsInterstitialShow?.Invoke();
            }
        }
    }
}
