using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ProgressbarRegrowing : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        [SerializeField] private FoodRegrower _foodRegrower;

        private Coroutine _coroutine;

        private void OnEnable()
        {
            _foodRegrower.WaitingRegrow += OnWaitingRegrow;
        }

        private void OnDisable()
        {
            _foodRegrower.WaitingRegrow -= OnWaitingRegrow;

            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private void OnWaitingRegrow(float duration)
        {
            _coroutine = StartCoroutine(ShowBar());

            IEnumerator ShowBar()
            {
                float time = 0;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    _bar.fillAmount = Mathf.Lerp(1, 0, Mathf.InverseLerp(0, duration, time));
                    yield return null;
                }
            }
        }
    }
}
