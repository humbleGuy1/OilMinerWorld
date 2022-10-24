using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class SlicedHex : CompositeObject
    {
        [SerializeField] private Infinite _infinite;

        public Infinite Infinite => _infinite;

        private Color _lightColor;

        public void Init(Color lightColor)
        {
            _lightColor = lightColor;
        }

        public void ChangePartsColor(Material material)
        {
            _renderingParts.ForEach(part => (part as SlicedHexPart).ChangeMaterial(material));
        }

        public void LightenPartColor(Resource part, float diggingDifficulty)
        {
            MeshRenderer meshRenderer = (part as SlicedHexPart).MeshRenderer;
            meshRenderer?.material.DOColor(_lightColor, diggingDifficulty).SetEase(Ease.Linear);
        }

        public void DestroyAllPart()
        {
            if (_renderingParts == null) return;

            foreach (SlicedHexPart part in _renderingParts)
                Destroy(part.gameObject);

            _renderingParts.Clear();
        }

        public override void RemovePart(Resource removablePart)
        {
            if (_renderingParts.Contains(removablePart) == false)
                return;

            (removablePart as SlicedHexPart).SwitchToDymanic();
            _renderingParts.Remove(removablePart);

            OnPartsChanged();
        }

        public void AcitvateInfinite()
        {
            _infinite.Enable();
        }
    }
}
