using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hex : MonoBehaviour
{
    [SerializeField] private bool _drawGizmos = true;
    [SerializeField, Range(0, 1f)] private float _sizeModifier;
    [SerializeField] private MeshRenderer _meshRenderer;
    [HideInInspector][SerializeField] private List<Vector3> _localCenterSidesPositions = new List<Vector3>();

    public IReadOnlyList<Vector3> LocalCenterSidesPositions => _localCenterSidesPositions;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Initialize();
    }

    private void OnDrawGizmos()
    {
        if (_drawGizmos == false)
            return;

        foreach (var pointPosition in _localCenterSidesPositions)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + pointPosition, 0.05f);
        }
    }

    [ContextMenu("Initialize")]
    private void Initialize()
    {
        _localCenterSidesPositions?.Clear();
        float innerRadius = transform.lossyScale.x * _sizeModifier * 0.866025404f;

        for (int i = 0; i < 6; i++)
        {
            float angle = (Mathf.PI / 3) * i;
            var position = new Vector3(innerRadius * Mathf.Cos(angle), transform.lossyScale.y * 0.1f, innerRadius * Mathf.Sin(angle));

            _localCenterSidesPositions.Add(position);
        }

        EditorUtility.SetDirty(gameObject);
    }
#endif

    public void ChangeMaterial(Material targetMaterial)
    {
        _meshRenderer.material = targetMaterial;
    }

    public void DisableMeshRenderer()
    {
        _meshRenderer.enabled = false;
    }
}
