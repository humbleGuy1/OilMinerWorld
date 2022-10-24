using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Scripts
{
    public class RegionViewGoal : MonoBehaviour
    {
        [SerializeField] private TMP_Text _value;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _targetSprite;

        private void OnEnable()
        {
            _canvas.enabled = false;
        }

        public void SetValue(int current, int total)
        {
            _canvas.enabled = true;
            //_value.text = $"{current}/{total}";
            _value.text = $"{total}";
        }

        public void SwitchToCompletedState()
        {
            ChangeSprite();
            Animate();
        }

        public void HideCanvas()
        {
            _canvas.enabled = false;
        }

        private void ChangeSprite()
        {
            _image.sprite = _targetSprite;
        }

        public void Animate()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOShakeRotation(1.0f, 4f, 5));
            sequence.AppendInterval(2f);
            sequence.SetLoops(-1, LoopType.Restart);
            sequence.SetEase(Ease.Linear);
        }
    }
}
