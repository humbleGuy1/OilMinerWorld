using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _foregroundProgress;

    public void Render(int currentValue, int goal)
    {
        //_text.text = $"{currentValue} / {goal}";
        _text.text = $"{currentValue}";
        //_foregroundProgress.fillAmount = (float)currentValue / goal;
    }
}
