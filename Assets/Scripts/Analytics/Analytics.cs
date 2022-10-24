using System;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UnityEngine;

public class Analytics : Singleton<Analytics>
{
    [SerializeField] private GameObject _gameAnalyticsSDK;

    private const string RegDayKey = "RegDay";
    private const string SessionCountKey = "SessionCount";
    private const string SoftSpentEventsCountKey = "SoftSpentEventsCount";

    private AnalyticsPlayTimeLogger _timeLogger;

    public event Action EventSent;

    public TimeSpan AllPlayTime => _timeLogger.AllPlayTime;

    private string _regDay
    {
        get { return PlayerPrefs.GetString(RegDayKey, DateTime.Today.ToString()); }
        set { PlayerPrefs.SetString(RegDayKey, value); }
    }

    private int _sessionCount
    {
        get { return PlayerPrefs.GetInt(SessionCountKey, 0); }
        set { PlayerPrefs.SetInt(SessionCountKey, value); }
    }

    private int _softSpentEventsCount
    {
        get { return PlayerPrefs.GetInt(SoftSpentEventsCountKey, 0); }
        set { PlayerPrefs.SetInt(SoftSpentEventsCountKey, value); }
    }

    protected override void OnAwake()
    {
        _timeLogger = Singleton<AnalyticsPlayTimeLogger>.Instance;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(RegDayKey) == false)
        {
            DateTime regDay = DateTime.Today;
            _regDay = regDay.ToString();

            YandexAppMetricaUserProfile userProfile = new YandexAppMetricaUserProfile();
            userProfile.Apply(YandexAppMetricaAttribute.CustomString("dd/MM/yy").WithValue(_regDay));
            AppMetrica.Instance.SetUserProfileID(new DuckyID().Value());
            AppMetrica.Instance.ReportUserProfile(userProfile);
        }

        _sessionCount += 1;
        SetBasicProperty(_sessionCount);
        
        FireEvent("game_start", new Dictionary<string, object>() { { "count", _sessionCount } });
    }

    public void OnSoftSpent(string productType, string productName, int amount)
    {
        _softSpentEventsCount += 1;

        var properties = new Dictionary<string, object>()
        {
            {"type", productType},
            {"name", productName},
            {"amount", amount},
            {"count",  _softSpentEventsCount}
        };

        FireEvent("soft_spent", properties);
    }

    public void OnEventDone(string eventName, int timeSpent)
    {
        _softSpentEventsCount += 1;

        var properties = new Dictionary<string, object>()
        {
            {"name", eventName},
            {"timeSpent", timeSpent}
        };

        FireEvent("soft_spent", properties);
    }

    public void OnNestUpgraded(int level)
    {
        _softSpentEventsCount += 1;

        var properties = new Dictionary<string, object>()
        {
            {"level", level}
        };

        FireEvent("nest_upgraded", properties);
    }

    public void OnLevelCompleted(int level_number, float playTime)
    {
        _softSpentEventsCount += 1;

        var properties = new Dictionary<string, object>()
        {
            {"level_number", level_number},
            {"playTime", playTime}
        };

        FireEvent("level_finished", properties);
    }

    public void OnTutorialStepCompleted(int tutorialStep)
    {
        _softSpentEventsCount += 1;

        var properties = new Dictionary<string, object>()
        {
            {"tutorial_step", tutorialStep},
        };

        FireEvent("tutorial_step_completed", properties);
    }

    public void OnSoftSpent(string productType, string productName, float multiply)
    {
        var properties = new Dictionary<string, object>()
        {
            {"type", productType},
            {"name", productName},
            {"amount", multiply},
            {"count",  _softSpentEventsCount}
        };

        FireEvent("soft_spent", properties);
    }

    public void OnVideoAdsTriggered(string adType, string placement, string ad_network, string result = "success", bool internet = true)
    {
        var properties = new Dictionary<string, object>()
        {
            {"ad_type", adType},
            {"placement", placement},
            {"result", result},
            {"internet", internet},
            {"ad_network", ad_network}
        };

        FireEvent("video_ads_triggered", properties);
    }

    public void OnVideoAdsStarted(string adType, string placement, string ad_network, string result = "start", bool internet = true)
    {
        var properties = new Dictionary<string, object>()
        {
            {"ad_type", adType},
            {"placement", placement},
            {"result", result},
            {"internet", internet},
            {"ad_network", ad_network}
        };

        FireEvent("video_ads_started", properties);
    }

    public void OnVideoAdsCompleted(string adType, string placement, string ad_network, string result = "complete", bool internet = true)
    {
        var properties = new Dictionary<string, object>()
        {
            {"ad_type", adType},
            {"placement", placement},
            {"result", result},
            {"internet", internet},
            {"ad_network", ad_network}
        };

        FireEvent("video_ads_complete", properties);
    }

    public void OnDigAnalitics(string eventName, int totalHexCount, int price)
    {
        _softSpentEventsCount += 1;

        var properties = new Dictionary<string, object>()
        {
            {"total_hex_count", totalHexCount},
            {"price", price},
        };

        FireEvent("dig_started", properties);
    }

    public void OnResourceAnalitics(string eventName, int collectedResourceCount)
    {
        _softSpentEventsCount += 1;

        var properties = new Dictionary<string, object>()
        {
            {"collected_resource_count", collectedResourceCount},
        };

        FireEvent(eventName, properties);
    }

    public void OnRegionOpen(string eventName, int collectedResourceCount)
    {
        var properties = new Dictionary<string, object>()
        {
            {"open_region_index", collectedResourceCount},
        };

        FireEvent(eventName, properties);
    }

    public void LogTime(int elapsedMinutes)
    {
        var properties = new Dictionary<string, object>()
        {
            {"elapsed_minutes", elapsedMinutes}
        };

        FireEvent("play_time", properties);
    } 

    public void FireEvent(string eventName, Dictionary<string, object> eventProps = null)
    {
        eventProps ??= new Dictionary<string, object>();

        eventProps.Add("total_playtime_min", AllPlayTime.Minutes);
        eventProps.Add("total_playtime_sec", AllPlayTime.Minutes * 60 + AllPlayTime.Seconds);

#if UNITY_EDITOR
        //Debug.Log($"FireEvent: {eventName},\n{string.Join(Environment.NewLine ,eventProps)}");
#endif

        AppMetrica.Instance.ReportEvent(eventName, eventProps);

        EventSent?.Invoke();
    }

    private void SetBasicProperty(int sessionCount)
    {
        int daysInGame = DateTime.Today.Subtract(DateTime.Parse(_regDay)).Days;

        YandexAppMetricaUserProfile userProfile = new YandexAppMetricaUserProfile();
        userProfile.Apply(YandexAppMetricaAttribute.CustomCounter("days_in_game").WithDelta(daysInGame));
        userProfile.Apply(YandexAppMetricaAttribute.CustomCounter("session_count").WithDelta(sessionCount));

        AppMetrica.Instance.SetUserProfileID(new DuckyID().Value());
        AppMetrica.Instance.ReportUserProfile(userProfile);
    }
}
