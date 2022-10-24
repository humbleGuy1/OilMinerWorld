using System;
using UnityEngine;

public abstract class WalletPresenter : MonoBehaviour
{
    [SerializeField] private WalletView _view;
    [SerializeField] protected int StartValue;

    private Wallet _wallet;

    public event Action ValueChanged;

    public bool IsWalletInitialized { get; private set; }
    public float Value => _wallet.Value;

    private void OnEnable()
    {
        _wallet = Create();
        _wallet.Load();

        IsWalletInitialized = true;

        _wallet.ValueChanged += OnValueChanged;
        OnValueChanged();
    }

    private void OnDisable()
    {
        _wallet.ValueChanged -= OnValueChanged;
    }

    public void AddResource(float value)
    {
        _wallet.Add(value);
    }

    public void SpendResource(int value)
    {
        _wallet.Spend(value);
    }

    protected abstract Wallet Create();

    private void OnValueChanged()
    {
        _wallet.Save();

        _view.Render(Value);
        ValueChanged?.Invoke();
    }
}
