using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class RocksFinder : MonoBehaviour
{
    [SerializeField] private Cell[] _cells;
    [SerializeField] private Rock[] _rocks;

    private void OnValidate()
    {
        //_cells = FindObjectsOfType<Cell>();
        //_rocks = FindObjectsOfType<Rock>();
    }
}
