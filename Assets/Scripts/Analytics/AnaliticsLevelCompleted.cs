using System;
using Assets.Scripts;
using UnityEngine;

public class AnaliticsLevelCompleted : MonoBehaviour
{
    [SerializeField] private StarGoal _starGoal;
    [SerializeField] private Anthill _anthill;
    [SerializeField] private BoostSpeedButton _inreaseIncomeButton;

    private AnalyticsPlayTimeLogger _timeLogger;
    private Analytics _analytics;

    public event Action<string> LevelDone;

    private void Awake()
    {
        _analytics = Singleton<Analytics>.Instance;
        _timeLogger = Singleton<AnalyticsPlayTimeLogger>.Instance;
    }

    private void OnEnable()
    {
        _starGoal.ProgressReached += OnLevelCompleted;
        _inreaseIncomeButton.BoostSpeedClicked += OnBoostClicked;
    }

    private void OnDisable()
    {
        _starGoal.ProgressReached -= OnLevelCompleted;
        _inreaseIncomeButton.BoostSpeedClicked -= OnBoostClicked;
    }

    private void OnLevelCompleted()
    {
        _analytics.OnLevelCompleted(_anthill.LevelNumber, _timeLogger.AllPlayTime.Minutes);
        LevelDone?.Invoke(_anthill.LevelNumber.ToString());
    }

    private void OnBoostClicked()
    {
        _analytics.OnEventDone("Boost Speed Clicked", (int)Time.timeSinceLevelLoad);
    }
}
