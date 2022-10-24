using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HouseStatsView : MonoBehaviour
{
    [SerializeField] private TMP_Text _value;
    [SerializeField] private TMP_Text _valueNextGrade;
    [SerializeField] private TMP_Text _valueDiffrence;
    [SerializeField] private TMP_Text _maxLevel;
    [SerializeField] private Image _image;

    public void UpdateInfo(float currentValue, float addedValue, bool isMaxed, Sprite sprite)
    {
        _image.sprite = sprite;
        _value.text = $"{currentValue}";
        float valueNextGrade = currentValue + addedValue;
        _valueNextGrade.text = $"{valueNextGrade}";
        _valueDiffrence.text = $"(+{addedValue})";

        if (isMaxed)
            EnableMaxLevelView(currentValue);
        else
            DisableMaxLevelView();
    }

    public void EnableMaxLevelView(float currentValue)
    {
        _maxLevel.text = $"{currentValue}";
        _maxLevel.gameObject.SetActive(true);
        _value.gameObject.SetActive(false);
        _valueNextGrade.gameObject.SetActive(false);
        _valueDiffrence.gameObject.SetActive(false);
    }

    public void DisableMaxLevelView()
    {
        _maxLevel.gameObject.SetActive(false);
        _value.gameObject.SetActive(true);
        _valueNextGrade.gameObject.SetActive(true);
        _valueDiffrence.gameObject.SetActive(true);
    }
}
