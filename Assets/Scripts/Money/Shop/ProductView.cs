using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ProductView : MonoBehaviour
    {
        [SerializeField] private FingerHint _fingerHint;

        private Product _product;

        [field: SerializeField] public TMP_Text PriceText { get; protected set; }
        [field: SerializeField] public TMP_Text CountText { get; protected set; }
        [field: SerializeField] public Color LockOfMoneyColor { get; protected set; }
        [field: SerializeField] public Color Gray { get; private set; }
        [field: SerializeField] public Image Image { get; protected set; }

        public WalletPresenter WalletPresenter { get; private set; }

        private void OnDisable()
        {
            WalletPresenter.ValueChanged -= UpdateView;
        }

        public void Initialize(Product speedProduct, WalletPresenter walletPresenter)
        {
            _product = speedProduct;
            WalletPresenter = walletPresenter;
            WalletPresenter.ValueChanged += UpdateView;
            UpdateView();
        }

        public void UpdateView()
        {
            int currentPrice = _product.Price;
            PriceText.text = currentPrice.ToString();
            CountText.text = $"{Math.Round(_product.ProductValue.Value, 2) * 100}%";

            UpdateBackground();
        }

        protected void UpdateBackground()
        {
            if (WalletPresenter.IsWalletInitialized == false)
                return;

            int currentPrice = _product.Price;
            float currentMoney = WalletPresenter.Value;

            if (currentPrice > currentMoney)
            {
                Image.color = LockOfMoneyColor;
            }
            else if (_product.ProductValue.CanNext == false)
            {
                if (_fingerHint != null && _fingerHint.IsTutorialFinished)
                    Image.color = LockOfMoneyColor;
                else
                    Image.color = Gray;
            }
            else
            {
                Image.color = Color.white;
            }
        }
    }
}
