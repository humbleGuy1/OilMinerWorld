using System;
using System.Collections;
using UnityEngine;

public class AnalyticsPlayTimeLogger : Singleton<AnalyticsPlayTimeLogger>
{
    private const string AllPlayTimeKey = "AllPlayTime";
    private const float TimeLogDelay = 1f;
    private const int TimeAfterChangeIntervalInMinutes = 10;
    private const int FirstIntervalInMinutes = 1;
    private const int SecondIntervalInMinutes = 5;

    private Analytics _analytics; 
    private TimeSpan _time;

    public TimeSpan AllPlayTime => _time;
    
    private string _allPlayTime
    {
        get { return PlayerPrefs.GetString(AllPlayTimeKey, TimeSpan.Zero.ToString()); }
        set { PlayerPrefs.SetString(AllPlayTimeKey, value); }
    }

    private void Start()
    {
        _analytics = Singleton<Analytics>.Instance;
        _time = TimeSpan.Parse(_allPlayTime);

        StartCoroutine(TimeLogger());
    }

    private IEnumerator TimeLogger()
    {
        var delay = new WaitForSecondsRealtime(TimeLogDelay);
        while (true)
        {
            yield return delay;

            int interval = _time.TotalMinutes >= TimeAfterChangeIntervalInMinutes ? SecondIntervalInMinutes : FirstIntervalInMinutes;
            double endMinutes = interval * ((int)_time.TotalMinutes / interval) + interval;

            _time = _time.Add(TimeSpan.FromSeconds(TimeLogDelay));
            _allPlayTime = _time.ToString();

            if (_time.TotalMinutes >= endMinutes)
                _analytics.LogTime((int)_time.TotalMinutes);
        }
    }
}
