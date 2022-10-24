using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AddedResourceFX : MonoBehaviour
{
    [SerializeField] private Image _resourceSprite; 
    [SerializeField] private TMP_Text _value;

    float _scaleDuration = 0.2f;
    float _moveDuration = 2;
    float _targetScale = 0.3f;
    Vector3 _targetPosition = new Vector3(0, 0.25f, 0.8f);

    public void Init(float value, Sprite resourceSprite)
    {
        _value.text = $"{Mathf.Round(value)}";
        _resourceSprite.sprite = resourceSprite;

        float colorDuration = _moveDuration - _scaleDuration;
        transform.DOScale(_targetScale, _scaleDuration);
        transform.DOMove(transform.position + _targetPosition, _moveDuration).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
    }
}
