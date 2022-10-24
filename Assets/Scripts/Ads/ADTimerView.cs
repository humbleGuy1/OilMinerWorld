using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ADTimerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private Image _timerImage;

    public float Duration { get; private set; } = 5f;

    private void OnEnable()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
        gameObject.SetActive(true);
        StartCoroutine(CountingDown());
    }

    private  IEnumerator CountingDown()
    {
        float elapsedTime = 0f;
        float timerTime = Duration;
        float startTime = Duration;
        _timer.text = $"{Duration}";

        while(elapsedTime < Duration)
        {
            elapsedTime+= Time.deltaTime;
            timerTime = Mathf.Lerp(startTime, 0, elapsedTime/Duration);
            _timerImage.fillAmount = timerTime / Duration;

            _timer.text = $"{(int)timerTime}";

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
