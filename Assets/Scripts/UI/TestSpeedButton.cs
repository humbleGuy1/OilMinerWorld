using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;
using TMPro;
using System.Collections;
using IJunior.TypedScenes;

namespace Assets.Scripts
{
    public class TestSpeedButton : MonoBehaviour
    {
        [SerializeField] private Button _speed4x;
        [SerializeField] private Button _money;
        [SerializeField] private Button _map;
        [SerializeField] private Button _secondMap;
        [SerializeField] private TMP_Text _tMP_Text2;
        [SerializeField] private TMP_Text _textHasDate;
        [SerializeField] private TMP_Text _textHasCustom;
        [SerializeField] private TMP_Text _textHasInet;
        [SerializeField] private EnternetConnetionHandler _enternetConnetionHandler;

        private bool _hasSpeedUp = false;

        private const string BonusClicked = "BonusClicked";
        private const float Repeat = 0.3f;
        private const float Delay = 4f;

        private void OnEnable()
        {
            StartCoroutine(HasKeyShow());
            StartCoroutine(DelayedAdsInitializer());

            _map.onClick.AddListener(OnMapClicked);
            _speed4x.onClick.AddListener(OnSpeedClick);
            _money.onClick.AddListener(OnMoneyClicked);
            _secondMap.onClick.AddListener(LoadSecondMap);

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplay;
        }

        private void OnDisable()
        {
            _map.onClick.RemoveListener(OnMapClicked);
            _speed4x.onClick.RemoveListener(OnSpeedClick);            
            _money.onClick.RemoveListener(OnMoneyClicked);

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoaded;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplay;
        }

        private void Update()
        {
            _textHasInet.text = _enternetConnetionHandler.EnternetAccess.ToString();
        }

        private void OnRewardedAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            _textHasCustom.text = "Rewarded Ad Loaded";
            Invoke(nameof(ResetText), Delay);
        }

        private void OnRewardedAdFailedToDisplay(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            _textHasCustom.text = "Reward Failed To Display";
            Invoke(nameof(ResetText), Delay);
        }

        private void OnRewardedAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            _textHasCustom.text = "Reward Load Failed";
            Invoke(nameof(ResetText), Delay);
        }

        private void OnRewardedAdDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            _textHasCustom.text = "Reward Done";
            Invoke(nameof(ResetText), Delay);
        }

        private void ResetText()
        {
            _textHasCustom.text = "___";
        }

        private void OnMapClicked()
        {
            LocalMap.Load();
        }

        private void LoadSecondMap()
        {
            Map_2.Load();
        }

        private void OnMoneyClicked()
        {
            StoneWalletPresenter wallet = FindObjectOfType<StoneWalletPresenter>();

            if(wallet != null)
                wallet.AddResource(10000);

            LeafWalletPresenter leafWalletPresenter = FindObjectOfType<LeafWalletPresenter>();

            if(leafWalletPresenter != null)
                leafWalletPresenter.AddResource(10000);
        }

        private void OnSpeedClick()
        {
            if (_hasSpeedUp)
            {
                Time.timeScale = 1;
                _hasSpeedUp = false;
            }
            else
            {
                Time.timeScale = 3;
                _hasSpeedUp = true;
            }
        }

        private IEnumerator DelayedAdsInitializer()
        {
#if UNITY_EDITOR
            yield return null;
#else
            var wait = new WaitForSeconds(Repeat);

            while (GameAnalytics.IsRemoteConfigsReady() == false)
            {
                yield return wait;
                _tMP_Text2.text = GameAnalytics.IsRemoteConfigsReady().ToString();
            }
#endif
        }

        private IEnumerator HasKeyShow()
        {
            var wait = new WaitForSeconds(0.3f);
            var wait2 = new WaitForSeconds(1f);

            yield return wait2;

            while (true)
            {
                _textHasDate.text = PlayerPrefs.HasKey(TimeUtils.LastSaveTime).ToString();
                yield return wait;
            }
        }
    }
}
