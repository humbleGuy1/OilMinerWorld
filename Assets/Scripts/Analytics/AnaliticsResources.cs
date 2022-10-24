using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;

public class AnaliticsResources : MonoBehaviour
{
    private const string CollectedResourceCount = "CollectedResourceCount";

    [SerializeField] private Anthill _anthill;

    private List<Food> _resources = new List<Food>();
    private Analytics _analytics;
    private int _collectedResourceCount;


    private void Awake()
    {
        _analytics = Singleton<Analytics>.Instance;
    }

    private void OnEnable()
    {
        foreach (Food food in _resources)
        {
            food.Regrowed += OnResourceCollected;
            food.StartEating += OnResourceCollectionStart;
            food.Eaten += (f) => OnResourceCollectionComplete();
        }

        _collectedResourceCount = PlayerPrefs.GetInt(CollectedResourceCount);
    }

    private void OnDisable()
    {
        foreach (Food food in _resources)
        {
            food.Regrowed -= OnResourceCollected;
            food.StartEating -= OnResourceCollectionStart;
            food.Eaten -= (f) => OnResourceCollectionComplete();
        }

        PlayerPrefs.SetInt(CollectedResourceCount, _collectedResourceCount);
    }

    private void OnResourceCollectionStart()
    {
        _analytics.OnResourceAnalitics("resource_collection_start", _collectedResourceCount);
    }

    private void OnResourceCollected()
    {
        _collectedResourceCount++;
        _analytics.OnResourceAnalitics("resource_collected", _collectedResourceCount);
    }

    private void OnResourceCollectionComplete()
    {
        _analytics.OnResourceAnalitics("resource_collection_complete", _collectedResourceCount);
    }

#if UNITY_EDITOR
    [ContextMenu("Initialize")]
    private void Initialize()
    {
        _resources?.Clear();
        _resources.AddRange(FindObjectsOfType<Food>(true));

        EditorUtility.SetDirty(gameObject);
    }
#endif
}
