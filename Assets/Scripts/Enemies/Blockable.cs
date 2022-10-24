using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Blockable
{
    public Cell Cell { get; private set; }

    public bool IsBlocked { get; private set; }

    public void SetCell(Cell cell)
    {
        Cell = cell;
    }

    public void Block()
    {
        IsBlocked = true;
    }

    public void UnBlock()
    {
        IsBlocked = false;
    }
}
