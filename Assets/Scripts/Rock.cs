using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private Hardness _type;
    [SerializeField] private List<GameObject> _pieces;
    [SerializeField] private SlicedHex _slicedHex;

    public bool IsInfinite => _slicedHex.IsInfinite;
    public Hardness Type => _type;

    public void SetMaterial(Material material)
    {
        foreach(var piece in _pieces)
        {
            var meshRenderer = piece.GetComponent<MeshRenderer>();
            meshRenderer.material = material;
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public enum Hardness
    {
        Light,
        Medium,
        Hard
    }
}
