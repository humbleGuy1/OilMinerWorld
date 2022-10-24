using System;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class NextButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    public event Action Pressed;

    public void Start()
    {
        PulseText(_textMeshProUGUI, -1);

        Sequence textSequence = DOTween.Sequence();
        textSequence.Append(_textMeshProUGUI.gameObject.transform.DOScale(1.05f, 0.2f));
        textSequence.Append(_textMeshProUGUI.gameObject.transform.DOScale(1, 0.2f));
        textSequence.SetLoops(-1, LoopType.Yoyo);        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed?.Invoke();
    }

    private void PulseText(TextMeshProUGUI typeText, int loopsCount)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(typeText.transform.DOScale(1.1f, 0.25f))
            .Append(typeText.transform.DOScale(1f, 0.25f))
            .SetLoops(loopsCount);
    }
}
