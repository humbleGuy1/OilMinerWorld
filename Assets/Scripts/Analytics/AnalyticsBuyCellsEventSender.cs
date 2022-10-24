using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnalyticsBuyCellsEventSender : MonoBehaviour
{
    [SerializeField] private Anthill _anthill;

    private List<Cell> _cells = new List<Cell>();

    private Analytics _analytics;

    private void Awake()
    {
        if (_anthill == null)
            throw new NullReferenceException(name);

        for (int i = 0; i < _anthill.DefaultCells.Count; i++)
            _cells.Add(_anthill.DefaultCells[i]);

        _analytics = Singleton<Analytics>.Instance;
    }

    private void OnEnable()
    {
        foreach (Cell cell in _cells)
            cell.FirstTimeDigged += OnCellDigged;
    }

    private void OnDisable()
    {
        foreach (Cell cell in _cells)
            cell.FirstTimeDigged -= OnCellDigged;
    }

    private void OnCellDigged(Cell cell)
    {
        _analytics.OnSoftSpent("Cell", cell.CellType.ToString(), cell.Price);
        _analytics.OnSoftSpent("Cell", cell.DiggingDifficult.ToString(), cell.Price);
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
