using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class AdsReward : Ads
    {
        protected const float Delay = 1f;
        private const float DelayBeforeShow = 0.3f;

        [SerializeField] private CanvasGroup _canceledCanvas;

        protected int RetryAttempt;
        protected bool IsOfflineReward = false;
        protected bool HasAdDisplayed = false;
        protected bool IsBoostSpeed = false;

        protected override void Subscribe()
        {
            Invoke(nameof(LoadRewardInterstitial), Delay);

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHidden;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplay;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedReward;
        }

        protected override void UnSubscribe()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdHidden;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplay;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedReward;
        }

        protected virtual void OnRewardedAdReceivedReward(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo) { }

        protected virtual void OnRewardedAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            HasAdDisplayed = false;
            IsOfflineReward = false;
            IsBoostSpeed = false;
        }

    protected virtual void OnRewardedAdDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            HasAdDisplayed = true;
        }

        protected virtual void OnRewardedAdFailedToDisplay(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            LoadRewardInterstitial();
            Extentions.EnableGroup(_canceledCanvas);
            Extentions.DelayedDisableGroup(_canceledCanvas, 0.3f, 1f);
        }

        protected virtual void OnRewardedAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            RetryAttempt = 0;
        }

        protected void ShowRewardInterstisial()
        {
            if (MaxSdk.IsRewardedAdReady(AdsInitializer.RewardInterstitialAdUnitId))
                MaxSdk.ShowRewardedAd(AdsInitializer.RewardInterstitialAdUnitId);

            Invoke(nameof(LoadRewardInterstitial), DelayBeforeShow);
        }

        protected void OnRewardedAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            RetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, RetryAttempt));

            Invoke(nameof(LoadRewardInterstitial), (float)retryDelay);
        }

        protected void LoadRewardInterstitial()
        {
            MaxSdk.LoadRewardedAd(AdsInitializer.RewardInterstitialAdUnitId);
        }
    }
}
