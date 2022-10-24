using System.Collections;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    public class AdsIcreaseIncomeReward : AdsReward
    {
        [SerializeField] private BoostSpeedButton _boostButton;
        [SerializeField] private OfflineBonusTimer _offlineBonusTimer;
        [SerializeField] private SpeedProduct _speedProduct;
        [SerializeField] private StrenghtProduct _strenghtProduct;
        [SerializeField] private EndGame _endGame;

        private bool _isActivated = false;

        private float Cooldown => RemoteConfig.RewardConfig.Cooldown;
        private float ActionTime => RemoteConfig.RewardConfig.Duration;

        protected override void Subscribe()
        {
            base.Subscribe();

            var save = new LevelSave(_endGame.GUID);
            save.Load();

            if (save.Done)
                _boostButton.Disable();

            _boostButton.BoostSpeedClicked += OnBoostSpeed;
        }

        protected override void UnSubscribe()
        {
            base.UnSubscribe();

            _boostButton.BoostSpeedClicked -= OnBoostSpeed;
        }

        protected override void OnRewardedAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (_isActivated)
                return;

            _boostButton.Enable();
        }

        protected override void OnRewardedAdReceivedReward(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            if (IsBoostSpeed && HasAdDisplayed)
            {
                _isActivated = true;
                StartCoroutine(TimerOfBoostSpeed());
            }

            IsBoostSpeed = false;
            IsOfflineReward = false;
        }

        protected override void OnRewardedAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            base.OnRewardedAdHidden(adUnitId, adInfo);
        }

        private void OnBoostSpeed()
        {
            IsBoostSpeed = true;
            ShowRewardInterstisial();
            TimeUtils.SetLastDateTime(TimeUtils.LastSaveTime, DateTime.UtcNow);

            if (MaxSdk.IsRewardedAdReady(AdsInitializer.RewardInterstitialAdUnitId) == false)
                LoadRewardInterstitial();
        }

        private IEnumerator TimerOfBoostSpeed()
        {
            _boostButton.EnableBigText(ActionTime);
            _offlineBonusTimer.StartTimer(ActionTime);
            _speedProduct.BoostSpeed();
            _strenghtProduct.BoostStrenght();

            var wait = new WaitForSeconds(ActionTime);
            yield return wait;

            _boostButton.Disable();
            _speedProduct.OnBoostSpeedEnd();
            _strenghtProduct.OnBoostSpeedEnd();
            StartCoroutine(DelayBettweenIncreaseIncome());
        }

        private IEnumerator DelayBettweenIncreaseIncome()
        {
            yield return new WaitForSeconds(Cooldown);
            _isActivated = false;
            _boostButton.Enable();
        }
    }
}
