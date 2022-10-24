using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
    public class OfflineBonusTimer : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private ParticleSystem _fire;

        public void StartTimer(float time)
        {
            StartCoroutine(TimerOut(time));
            _iconImage.enabled = true;
        }

        private IEnumerator TimerOut(float time)
        {
            float step = 0.01f;
            float currentTime = time / 100;
            var wait = new WaitForSeconds(currentTime);

            for (float i = 1; i > 0; i -= step)
            {
                _iconImage.fillAmount -= step;
                yield return wait;
            }

            _iconImage.enabled = false;
            _iconImage.fillAmount = 1;
            _fire.Stop();
        }
    }
}
