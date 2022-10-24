using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class CellBlocker : MonoBehaviour
{
    [SerializeField] private List<Cell> _cells;

    public void Block()
    {
        foreach (var cell in _cells)
        {
            cell.Block();
        }
    }

    public void UnBlock()
    {
        foreach (var cell in _cells)
        {
            cell.Unblock();
        }
    }
}
