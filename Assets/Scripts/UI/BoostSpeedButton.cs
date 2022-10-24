using TMPro;
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
    public class BoostSpeedButton : MonoBehaviour
    {
        [SerializeField] private Button _boostButton;
        [SerializeField] private Image _boostImage;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TMP_Text _x5Text;
        [SerializeField] private TMP_Text _x5BigText;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ParticleSystem _fireParticle;

        public event Action BoostSpeedClicked;

        private void OnEnable()
        {
            _boostButton.onClick.AddListener(OnBoostSpeedClicked);
        }

        private void OnDisable()
        {
            _boostButton.onClick.RemoveListener(OnBoostSpeedClicked);
        }

        public void Enable()
        {
            DisableBigText();
            Extentions.EnableGroup(_canvasGroup);
        }

        public void Disable()
        {
            Extentions.DisableGroup(_canvasGroup);
        }

        public void EnableBigText(float duration)
        {
            _fireParticle.Play();
            _boostImage.color = Color.gray;
            _boostButton.enabled = false;
            _rewardIcon.enabled = false;
            _x5Text.enabled = false;
            _speedText.enabled = false;
            _x5BigText.enabled = true;
            StartCoroutine(Animation(duration));
        }

        private void DisableBigText()
        {
            _fireParticle.Stop();
            _boostImage.color = Color.white;
            _rewardIcon.enabled = true;
            _boostButton.enabled = true;
            _x5Text.enabled = true;
            _speedText.enabled = true;
            _x5BigText.enabled = false;
        }

        private void OnBoostSpeedClicked()
        {
            BoostSpeedClicked?.Invoke();
        }

        private IEnumerator Animation(float duration)
        {
            float oneSecond = 1;
            float halfSecond = 0.5f;
            var wait = new WaitForSeconds(oneSecond);
            Vector3 punch = new Vector3(1.1f, 1.1f, 1.1f);

            for (int i = 0; i < duration; i++)
            {
                _x5BigText.transform.DOScale(punch, halfSecond);
                _x5BigText.transform.DOScale(Vector3.one, halfSecond).SetDelay(halfSecond);
                yield return wait;
            }
        }
    }
}
