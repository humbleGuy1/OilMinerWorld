using UnityEngine;

public abstract class HoleMaterialReqierable : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public void SetMaterial(Material targetMaterial)
    {
        _meshRenderer.material = targetMaterial;
    }
}
