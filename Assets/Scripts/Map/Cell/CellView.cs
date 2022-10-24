using UnityEngine;
using System.Collections.Generic;
using static Assets.Scripts.CellViewData;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Cell))]
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Outline _topOutline;
        [SerializeField] private SlicedHex _slicedHex;
        [SerializeField] private MeshRenderer _wholeHex;
        [SerializeField] private SpriteRenderer _topHexSpriteRenderer;
        [SerializeField] private CellPriceView _price;
        [SerializeField] private CanvasRenderer _cancel;
        [SerializeField] private Color _lockColor;
        [SerializeField] private Color _unblockedColor;
        [SerializeField] private List<Material> _materials;
        [SerializeField] private CellViewObjects _cellViewObjects;

        private Color LightColor => _materials[0].color;

        public SlicedHex SlicedHex => _slicedHex;

        private void OnValidate()
        {
            //_slicedHex = GetComponentInChildren<SlicedHex>();
        }

        public void Init(int diggingDifficulty)
        {
            //if(_wholeHex != null)
            //    _wholeHex.material = _materials[diggingDifficulty];

            _slicedHex.Init(LightColor);
            //_slicedHex.ChangePartsColor(_materials[diggingDifficulty]);
        }

        public SlicedHex GetHex()
        {
            return _slicedHex;
        }

        public void RenderBlock()
        {
            _topHexSpriteRenderer.color = _lockColor;
            _topHexSpriteRenderer?.gameObject.SetActive(true);
            SwitchText(TextState.Disabled);
            _cellViewObjects.DisableRenderObjects();
        }

        public void RenderUnlock()
        {
            if(_topHexSpriteRenderer != null)
                _topHexSpriteRenderer.gameObject.SetActive(false);

            SwitchText(TextState.OnlyPrice);
            _topOutline.Disable();

            //if (_slicedHex.IsInfinite)
            //    _infinite.Enable();

            //if (_slicedHex != null)
            //    SwithHex(_slicedHex.PartsCount == _slicedHex.MaxPartsCount ? HexType.Whole : HexType.Sliced);

            _cellViewObjects.EnableRenderObjects();
        }

        public void RenderDigging()
        {
            SwitchText(TextState.OnlyCancel);
            RenderLockWithTopHex();
        }

        public void RenderLockWithTopHex() => SwithHex(HexType.Sliced);

        public void RenderOpen()
        {
            SwitchText(TextState.Disabled);
            SwithHex(HexType.Sliced);
            _topOutline.Disable();
        }

        public void RenderLock()
        {
            if (_topHexSpriteRenderer == null)
                return;

            _topHexSpriteRenderer.color = _lockColor;
        }

        public void RenderUnblocked()
        {
            _topHexSpriteRenderer.color = _unblockedColor;
            //_topOutline.Disable();
        }

        private void SwithHex(HexType type)
        {
            //if(_wholeHex != null)
            //    _wholeHex.gameObject.SetActive(type == HexType.Whole);

            //if (_slicedHex != null)
            //    _slicedHex.gameObject.SetActive(type == HexType.Sliced);
        }

        private void SwitchText(TextState state)
        {
            if(state == TextState.OnlyPrice)
                _price.ShowLeafPricePanel();
            else
                _price.HideLeafPricePanel();

            _cancel.gameObject.SetActive(state == TextState.OnlyCancel);
        }
    }
}
