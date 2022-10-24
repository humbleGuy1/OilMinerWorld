using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;

public class AnaliticsDig : MonoBehaviour
{
    private const string DiggedCellCount = "DiggedCellCount";

    [SerializeField] private Anthill _anthill;

    private List<Cell> _cells = new List<Cell>();
    private Analytics _analytics;
    private int _diggedCellCount;

    private void Awake()
    {
        _analytics = Singleton<Analytics>.Instance;    
    }


    private void OnEnable()
    {
        foreach (Cell cell in _cells)
        {
            cell.Digging += OnCellStartDigging;
            cell.Digged += OnCellDigged;
        }

        _diggedCellCount = PlayerPrefs.GetInt(DiggedCellCount);
    }

    private void OnDisable()
    {
        foreach (Cell cell in _cells)
        {
            cell.Digging -= OnCellStartDigging;
            cell.Digged -= OnCellDigged;
        }

        PlayerPrefs.SetInt(DiggedCellCount, _diggedCellCount);
    }

    private void OnCellStartDigging(Cell cell)
    {
        _analytics.OnDigAnalitics("dig_started", _diggedCellCount, cell.Price);
    }

    private void OnCellDigged(Cell cell)
    {
        _diggedCellCount++;
        _analytics.OnDigAnalitics("dig_completed", _diggedCellCount, cell.Price);
    }


#if UNITY_EDITOR
    [ContextMenu("Initialize")]
    private void Initialize()
    {
        _cells?.Clear();
        _cells.AddRange(FindObjectsOfType<Cell>(true));

        EditorUtility.SetDirty(gameObject);
    }
#endif
}
