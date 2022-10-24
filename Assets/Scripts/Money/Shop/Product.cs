using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public abstract class Product : GUIDObject
    {
        [SerializeField] private Button _button;
        [SerializeField] private FingerHint _fingerHint;
        [SerializeField] private ProductView _view;
        [SerializeField] private float _defaultAddValueStep;
        [SerializeField] private float _defaultMaxValue;
        [SerializeField] private float _tutotialAddValueStep;
        [SerializeField] private float _tutorialMaxValue;
        [SerializeField] private StatsType _statType;

        private ProductPrice _price;
        private float _addValueStep;
        private float _maxValue;

        public event Action<Product> Boosted;
        public event Action<Product> Activated;
        public event Action<Product> Clicked;
        public event Action<Product> Buyed;

        public ProductValue ProductValue { get; private set; }
        public bool CanBuy => ProductValue.CanNext;
        public int Price => _price.Value;
        public StatsType StatsType  => _statType;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
            Activated?.Invoke(this);
            Buyed?.Invoke(this);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Initialize(WalletPresenter walletPresenter)
        {
            _price = CreateProductPrice();
            _price.Load();
            ProductValue = new ProductValue($"{GUID}{nameof(ProductValue)}", _maxValue);
            ProductValue.Load();
            _view.Initialize(this, walletPresenter);

            if (_fingerHint == null || _fingerHint.IsTutorialFinished)
                SetDefaultSetings();
        }

        public void SetTutorialSettings()
        {
            _maxValue = _tutorialMaxValue;
            _addValueStep = _tutotialAddValueStep;

            ProductValue = new ProductValue($"{GUID}{nameof(ProductValue)}", _maxValue);
            ProductValue.Load();
            ProductValue.UpdateMaxValue(_maxValue);
        }

        public void SetDefaultSetings()
        {
            _maxValue = _defaultMaxValue;
            _addValueStep = _defaultAddValueStep;

            ProductValue = new ProductValue($"{GUID}{nameof(ProductValue)}", _maxValue);
            ProductValue.Load();
            ProductValue.UpdateMaxValue(_maxValue);
        }

        public virtual void Buy()
        {
            if (CanBuy == false)
                throw new InvalidOperationException();

            if (ProductValue.Value >= _maxValue)
                return;

            ProductValue = new ProductValue($"{GUID}{nameof(ProductValue)}", _maxValue);
            ProductValue.Load();
            ProductValue.Next(_addValueStep);
            ProductValue.Save();

            int stepsCounter = Mathf.RoundToInt((ProductValue.Value - 1)/ _addValueStep);

            _price = CreateProductPrice();
            _price.Load();
            _price.Next(stepsCounter);
            _price.Save();

            Buyed?.Invoke(this);
            _view.UpdateView();
        }

        protected void OnProductBoosted()
        {
            Boosted?.Invoke(this);
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke(this);
        }

        private ProductPrice CreateProductPrice()
        {
            switch (this)
            {
                case SpeedProduct _:
                    return new SpeedPrice($"{GUID}{nameof(SpeedPrice)}");
                case IncomeProduct _:
                    return new IncomePrice($"{GUID}{nameof(IncomePrice)}");
                case StrenghtProduct _:
                    return new StrenghtPrice($"{GUID}{nameof(StrenghtPrice)}");
                default:
                    break;
            }

            return null;
        }
    }
}
