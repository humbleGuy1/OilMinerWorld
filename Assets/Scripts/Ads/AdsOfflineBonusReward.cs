using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class AdsOfflineBonusReward : AdsReward
    {
        [SerializeField] private CanvasGroup _errorScreen;
        [SerializeField] private CanvasGroup _loadingScreen;
        [SerializeField] private OfflineBonusButton _offlineIncomeButton;
        [SerializeField] private OfflineBonus _offlineBonus;
        [SerializeField] private Boot _boot;

        private LeafWalletPresenter _leafWalletPresenter;
        private bool _adHasLoaded = false;

        private const string BootScene = "Boot_2_Ads";
        private const string MapLocalScene = "LocalMap";

        private void OnLevelWasLoaded(int level)
        {
            float waitTime = 1f;
            var wait = new WaitForSecondsRealtime(waitTime);

            Scene scene = SceneManager.GetActiveScene();

            if (scene.name != BootScene && scene.name != MapLocalScene)
                StartCoroutine(FindWallet());

            IEnumerator FindWallet()
            {
                while (_leafWalletPresenter == null)
                {
                    _leafWalletPresenter = FindObjectOfType<LeafWalletPresenter>();
                    yield return wait;
                }

                if (_leafWalletPresenter == null)
                    Destroy(gameObject);

                _offlineBonus.Activate();
                RetryAttempt = 0;
            }
        }

        protected override void Subscribe()
        {
            base.Subscribe();

            _offlineIncomeButton.OfflineBonusCliked += OnOfflineBonusClicked;
        }

        protected override void UnSubscribe()
        {
            base.UnSubscribe();

            _offlineIncomeButton.OfflineBonusCliked -= OnOfflineBonusClicked;
        }

        protected override void OnRewardedAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Scene scene = SceneManager.GetActiveScene();

            if (scene.name != BootScene && scene.name != MapLocalScene)
            {
                _adHasLoaded = true;
                return;
            }

            if (_boot == null)
                return;

            //if (scene.name == BootScene)
            //    _boot.LoadMap();
        }

        protected override void OnRewardedAdReceivedReward(string adUnitId, MaxSdkBase.Reward rewarded, MaxSdkBase.AdInfo adInfo)
        {
            if (IsOfflineReward && HasAdDisplayed)
            {
                int reward = _offlineBonus.Bonus;

                _leafWalletPresenter.AddResource(reward);
                Destroy(gameObject);
            }

            IsOfflineReward = false;
            IsBoostSpeed = false;
        }

        protected override void OnRewardedAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            base.OnRewardedAdHidden(adUnitId, adInfo);
        }

        private void OnOfflineBonusClicked()
        {
            float waitTime = 0.5f;
            float spentTime = 3f;
            var wait = new WaitForSecondsRealtime(waitTime);

            Extentions.EnableGroup(_loadingScreen);
            IsOfflineReward = true;

            ShowRewardInterstisial();
            StartCoroutine(WaitForRewardAds());

            IEnumerator WaitForRewardAds()
            {
                while (_adHasLoaded == false)
                {
                    spentTime -= waitTime;

                    if (spentTime < 0)
                    {
                        Extentions.EnableGroup(_errorScreen);
                        Destroy(gameObject, 1f);
                        yield break;
                    }

                    yield return wait;
                }
            }
        }
    }
}
