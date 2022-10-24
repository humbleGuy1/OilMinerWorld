using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class FingerCellTutuorial : MonoBehaviour
{
    [SerializeField] private Cell _firstCell;
    [SerializeField] private OneTimeShow _finger;

    private void OnEnable()
    {
        _firstCell.BecameDiggable += HideFinger;
    }

    private void OnDisable()
    {
        _firstCell.BecameDiggable -= HideFinger;
    }

    public void ShowFinger()
    {
        _finger.Show();
    }

    private void HideFinger(Cell cell)
    {
        _firstCell.BecameDiggable -= HideFinger;

        _finger.Hide();
    }
}
