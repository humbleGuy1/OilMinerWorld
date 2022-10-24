using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFightButton : MonoBehaviour
{
    [SerializeField] private Button _figth;

    public event Action FightCliked;

    private void OnEnable()
    {
        _figth.onClick.AddListener(OnFightClicked);
    }

    private void OnDisable()
    {
        _figth.onClick.RemoveListener(OnFightClicked);
    }

    public void Enable()
    {
        _figth.image.enabled = true;
    }

    public void Disable()
    {
        _figth.image.enabled = false;
    }

    private void OnFightClicked()
    {
        FightCliked?.Invoke();
    }
}
