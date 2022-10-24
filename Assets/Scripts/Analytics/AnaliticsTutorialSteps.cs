using Assets.Scripts;
using UnityEngine;

public class AnaliticsTutorialSteps : MonoBehaviour
{
    [SerializeField] private FingerHint _fingerHint;

    private Analytics _analytics;

    private void Awake()
    {
        _analytics = Singleton<Analytics>.Instance;
    }

    private void OnEnable()
    {
        _fingerHint.StepCompleted += OnStepCompleted;
    }

    private void OnDisable()
    {
        _fingerHint.StepCompleted -= OnStepCompleted;
    }

    private void OnStepCompleted(int stepNumber)
    {
        _analytics.OnTutorialStepCompleted(stepNumber);
    }
}

public enum StepNumbers
{
    DiggerHouseHint = 1,
    DigFirstCellHint = 2,
    SpeedHint = 3,
    StrenghtHint = 4,
    LoaderHouseHint = 5,
    IncomeHint = 6,
    TutorialRegionPassed = 7
}
