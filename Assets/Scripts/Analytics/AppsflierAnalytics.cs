using UnityEngine;
using AppsFlyerSDK;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace Assets.Scripts
{
    public class AppsflierAnalytics : MonoBehaviour
    {
        private const string CountAdsKey = "CountAdsKey";
        private const string RegDayAppsKey = "RegDayAppsKey";
        private const int SecondsInOneDay = 86400;
        private const int SecondsInTwoDays = 172800;

        [SerializeField] private AdsView _adsView;

        private AnaliticsLevelCompleted _analiticsLevelCompleted;
        private int _countAds;


        private void OnEnable()
        {
            _countAds = PlayerPrefs.GetInt(CountAdsKey);

            if (PlayerPrefs.HasKey(RegDayAppsKey) == false)
            {
                DateTime regDay = DateTime.UtcNow;
                PlayerPrefs.SetString(RegDayAppsKey, regDay.ToString("u", CultureInfo.InvariantCulture));
            }
            else
            {
                string lastDate = PlayerPrefs.GetString(RegDayAppsKey);
                DateTime lastSaveDay = DateTime.ParseExact(lastDate, "u", CultureInfo.InvariantCulture);

                TimeSpan timeSpent = DateTime.UtcNow - lastSaveDay;
                int secondsSpent = (int)timeSpent.TotalSeconds;

                if (secondsSpent > SecondsInTwoDays)
                    return;

                if (secondsSpent < SecondsInOneDay)
                    return;

                if (secondsSpent > SecondsInOneDay)
                    OnNextDayRetention();
            }

            _adsView.AdsInterstitialShow -= OnAdsShow;
        }

        private void OnLevelWasLoaded(int level)
        {
            if(_analiticsLevelCompleted != null)
                _analiticsLevelCompleted.LevelDone -= OnLevelDone;

            _analiticsLevelCompleted = FindObjectOfType<AnaliticsLevelCompleted>();

            if(_analiticsLevelCompleted != null)
                _analiticsLevelCompleted.LevelDone += OnLevelDone;
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt(CountAdsKey, _countAds);

            _adsView.AdsInterstitialShow -= OnAdsShow;

            if(_analiticsLevelCompleted != null)
                _analiticsLevelCompleted.LevelDone -= OnLevelDone;
        }

        private void OnNextDayRetention()
        {
            SendEvents("ret_", "1");
        }

        private void OnLevelDone(string level)
        {
            SendEvents("level_complete_", level);
        }

        private void OnAdsShow()
        {
            _countAds++;

            if(_countAds % 5 == 0)
                SendEvents("ads_viewed_", _countAds.ToString());
        }

        private void SendEvents(string name, string count)
        {
            var purchaseEvent = new Dictionary<string, string>()
            {
                {"name", name},
                {"count",  count}
            };

            AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, purchaseEvent);
        }
    }
}
