using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ProgressbarDigging : MonoBehaviour
    {
        [SerializeField] private GameObject _diggable;
        [SerializeField] private SlicedHex _slicedHex;
        [SerializeField] private Image _bar;

        private void OnValidate()
        {
            //_slicedHex = _diggable.GetComponentInChildren<SlicedHex>();
        }

        private void OnEnable()
        {
            _slicedHex.PartsChanged += OnPartsCountChanged;
        }

        private void OnDisable()
        {
            _slicedHex.PartsChanged -= OnPartsCountChanged;            
        }

        private void OnPartsCountChanged(int maxParts, int partsCount)
        {
            _bar.fillAmount = Mathf.Lerp(1, 0, Mathf.InverseLerp(0, maxParts, partsCount));
        }
    }
}
