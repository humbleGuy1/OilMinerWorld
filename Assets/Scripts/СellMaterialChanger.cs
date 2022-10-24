#if UNITY_EDITOR
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class СellMaterialChanger : MonoBehaviour
{
    [Header("CellColorize")]
    [SerializeField] private List<Cell> _cells;
    [SerializeField] private List<Material> _yellowMaterials;
    [SerializeField] private List<Material> _greenMaterials;
    [SerializeField] private List<Material> _greyMaterials;

    [Header("RocksColorize")]
    [SerializeField] private Rock[] _rocks;
    [SerializeField] private Material _yellow;
    [SerializeField] private Material _green;
    [SerializeField] private Material _grey;
    [SerializeField] private Material _infiniteHexMaterial;

    [Space]
    [SerializeField] private Biom _biom;

    private Dictionary<Biom, List<Material>> _materialDictionary = new Dictionary<Biom, List<Material>>();
    private Dictionary<Biom, Material> _rocksDictionary = new Dictionary<Biom, Material>();

    private void OnValidate()
    {
        //_rocks = GetComponentsInChildren<Rock>();
    }

    public void ColorizeCells()
    {
        SetDictionaryValues();

        List<Material> materialList = GetMaterialList(_biom);

        foreach (var cell in _cells)
        {
            cell.Hex.ChangeMaterial(materialList[Random.Range(0, materialList.Count - 1)]);
        }

        var holeMaterialCells = GetComponentsInChildren<HoleMaterialReqierable>();

        foreach (var holeMaterialCell in holeMaterialCells)
        {
            var holeMaterial = materialList[materialList.Count - 1];
            holeMaterialCell.SetMaterial(holeMaterial);
        }
    }

    public void ColorizeRocks()
    {
        SetRocksDicionaryValuse();

        var material = GetMaterial(_biom);

        foreach(var rock in _rocks)
        {
            rock.SetMaterial(material);
        }
    }

    public void ColoRizeInfiniteRock()
    {
        foreach (var rock in _rocks)
        {
            if(rock.IsInfinite)
                rock.SetMaterial(_infiniteHexMaterial);
        }
    }

    private void SetDictionaryValues()
    {
        if (_materialDictionary.Count == 0)
        {
            _materialDictionary.Add(Biom.Yellow, _yellowMaterials);
            _materialDictionary.Add(Biom.Green, _greenMaterials);
            _materialDictionary.Add(Biom.Grey, _greyMaterials);
        }
    }

    private void SetRocksDicionaryValuse()
    {
        if(_rocksDictionary.Count == 0)
        {
            _rocksDictionary.Add(Biom.Yellow, _yellow);
            _rocksDictionary.Add(Biom.Green, _green);
            _rocksDictionary.Add(Biom.Grey, _grey);
        }
    }

    private List<Material> GetMaterialList(Biom biom)
    {
        return _materialDictionary[biom];
    }

    private Material GetMaterial(Biom biom)
    {
        return _rocksDictionary[biom];
    }

    public enum Biom
    {
        Yellow,
        Green,
        Grey
    }
}
#endif
