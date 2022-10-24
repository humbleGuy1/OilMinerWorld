using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<Product> _products;
    [SerializeField] private LeafWalletPresenter _wallet;

    public IReadOnlyCollection<Product> Products => _products;

    private void OnEnable()
    {
        foreach (Product product in _products)
        {
            product.Initialize(_wallet);
            product.Clicked += OnProductClicked;
        }
    }

    private void OnDisable()
    {
        foreach (Product product in _products)
            product.Clicked -= OnProductClicked;
    }

    public void OnProductClicked(Product product)
    {
        if (_wallet.Value < product.Price || product.CanBuy == false)
            return;

        _wallet.SpendResource(product.Price);
        product.Buy();
    }
}
