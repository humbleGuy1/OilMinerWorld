using System;
using UnityEngine;
using GameAnalyticsSDK;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [Obsolete]
    public class GameAnalitycsHandler : MonoBehaviour
    {
        [SerializeField] private EnternetConnetionHandler _enternetConnetionHandler;
        [SerializeField] private AdsView _ads;
        [SerializeField] private Image _iconNoAds;

        private bool _isAdsWork = false;

        private const string Key = "RemoveAds";
        private const string False = "false";

        private const float Delay = 1f;
        private const float Repeat = 0.3f;

        private void OnEnable()
        {
            StartCoroutine(DelayedAdsInitializer());
        }

        private IEnumerator DelayedAdsInitializer()
        {
            string disableAds;
            var wait = new WaitForSeconds(Repeat);
            var delay = new WaitForSeconds(Delay);
            yield return delay;
#if UNITY_EDITOR
#else
            while (GameAnalytics.IsRemoteConfigsReady() == false)
                yield return wait;
#endif
            yield return delay;
            disableAds = GameAnalytics.GetRemoteConfigsValueAsString(Key, False);

            if (disableAds == False)
                yield return _enternetConnetionHandler.TestConnection(access => ShowAds(access));

            StartCoroutine(ChekingEnternetConnect());
        }

        private IEnumerator ChekingEnternetConnect()
        {
            var wait = new WaitForSeconds(Delay);

            while (_isAdsWork)
            {
                yield return wait;
                yield return _enternetConnetionHandler.TestConnection(access => OnEnternetAccessLost(access));
            }

            while(_isAdsWork == false)
            {
                yield return wait;
                yield return _enternetConnetionHandler.TestConnection(access => OnEnternetAccessLost(access));
            }

            StartCoroutine(DelayedAdsInitializer());
        }

        private void ShowAds(bool access)
        {
            if (access == false)
                return;

            _ads.ShowAds();
            _isAdsWork = true;
            _iconNoAds.enabled = false;
        }

        private void OnEnternetAccessLost(bool access)
        {
            if(access == false)
            {
                _iconNoAds.enabled = true;
                _isAdsWork = false;
            }
            else
            {
                _iconNoAds.enabled = false;
                _isAdsWork = true;
            }
        }
    }
}
