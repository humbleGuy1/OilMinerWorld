using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeModelView : MonoBehaviour
{
    private Coroutine _coroutine;

    public void Animate(bool playAnmation)
    {
        gameObject.SetActive(true);

        if(_coroutine == null && playAnmation)
            _coroutine = StartCoroutine(Appearance());
    }

    private IEnumerator Appearance()
    {
        Vector3 initialScale = transform.localScale;
        Vector3 startScale = initialScale + Vector3.up;
        Vector3 lowScale = initialScale - Vector3.up * 0.5f;

        float duration = 0.2f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            transform.localScale = Vector3.Lerp(startScale, lowScale, elapsedTime / duration);

            yield return null;
        }

        duration = 0.2f;
        elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            transform.localScale = Vector3.Lerp(lowScale, initialScale, elapsedTime / duration);

            yield return null;
        }

        transform.localScale = initialScale;

        _coroutine = null;
    }

}
