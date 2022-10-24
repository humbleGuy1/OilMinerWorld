using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SlicedHexPart : Resource
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Mesh _mesh;

        public MeshRenderer MeshRenderer => _meshRenderer;

        public void ChangeMaterial(Material material)
        {
            _meshRenderer.material = material;
        }

        private void OnValidate()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _mesh = _meshFilter.sharedMesh;
        }

        public void SwitchToDymanic()
        {
            gameObject.isStatic = false;
            _meshFilter.sharedMesh = _mesh;
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(Initialize))]
        public void Initialize()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _mesh = _meshFilter.sharedMesh;
            gameObject.isStatic = true;
            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}
