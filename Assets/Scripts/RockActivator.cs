using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using static Assets.Scripts.CellData;
using static Rock;

public class RockActivator : MonoBehaviour
{
    [SerializeField] private List<Rock> _rocks;

    private Dictionary<CellDifficult, Hardness> _dictionary = new Dictionary<CellDifficult, Hardness>();

    public void Activate(CellDifficult difficult)
    {
        SetDictionaryValues();

        Hardness hardness = GetHardness(difficult);

        foreach(var rock in _rocks)
        {
            if (rock.Type == hardness)
                rock.Enable();
            else
                rock.Disable();
        }
    }

    [ContextMenu("SetDictionaryValue")]
    private void SetDictionaryValues()
    {
        if(_dictionary.Count == 0)
        {
            _dictionary.Add(CellDifficult.Ligth, Hardness.Light);
            _dictionary.Add(CellDifficult.Medium, Hardness.Medium);
            _dictionary.Add(CellDifficult.Hard, Hardness.Hard);
        }
    }

    private Hardness GetHardness(CellDifficult difficult)
    {
        print(_dictionary[difficult]);
        return _dictionary[difficult];
    }
}
