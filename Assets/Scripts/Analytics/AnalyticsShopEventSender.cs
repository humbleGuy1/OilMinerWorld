using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class AnalyticsShopEventSender : MonoBehaviour
    {
        [SerializeField] private SpeedProduct _speedProduct;
        [SerializeField] private Region _region;
        [SerializeField] private bool _tutorialLevel = false;

        private Analytics _analytics;
        private bool _isTutorialFinished = false;

        private const int True = 1;
        private const string TutorFinishedSave = "TutorFinishedSave";

        private void Awake()
        {
            _analytics = Singleton<Analytics>.Instance;
        }

        private void OnEnable()
        {
            _speedProduct.Buyed += OnSpeedBuyed;

            if (_tutorialLevel)
                _region.Opened += OnTargetRegionOpened;
        }

        private void OnDisable()
        {
            _speedProduct.Buyed -= OnSpeedBuyed;

            if (_tutorialLevel)
                _region.Opened -= OnTargetRegionOpened;
        }

        private void OnTargetRegionOpened()
        {
            if (PlayerPrefs.GetInt(TutorFinishedSave) == True)
                return;

            _isTutorialFinished = true;
            _analytics.OnEventDone("Tutorial Finished", (int)Time.timeSinceLevelLoad);
            PlayerPrefs.SetInt(TutorFinishedSave, True);
        }

        private void OnSpeedBuyed(Product product)
        {
            string productName = null;

            if (product is SpeedProduct)
                productName = "Speed";
            else if (product is IncomeProduct)
                productName = "Income";
            else if (product is StrenghtProduct)
                productName = "Strenght";
            else
                throw new NullReferenceException(nameof(Product));

            if (_tutorialLevel)
            {
                if (_isTutorialFinished == false)
                    _analytics.OnSoftSpent(productName + "FromTutorial", productName.ToLower(), product.ProductValue.Value);
                else
                    _analytics.OnSoftSpent(productName, productName.ToLower(), product.ProductValue.Value);
            }
            else
            {
                _analytics.OnSoftSpent(productName, productName.ToLower(), product.ProductValue.Value);
            }
        }
    }
}
