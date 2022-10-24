using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Zenject;
using System;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Food))]
    public class FoodRegrower : MonoBehaviour
    {
        [SerializeField] private int _bonusRegrow = 200;
        [SerializeField] private ProgressbarRegrowing _progressbar;

        private Food _food;
        private WalletPresenter _wallet;
        private Coroutine _regrowCoroutine;
        private AddCurrencyObjectsAnimation _addMoneyAnimation;
        private FoodRegrowButton _spawnedButton;
        private FoodRegrowButton _regrowButtonTemplate;

        private const float TimeBetweenRegrow = 4f;
        private const string IsRegrowingOn = nameof(IsRegrowingOn);
        private const string Map1 = "Map_1";
        private const int True = 1;

        public event Action<float> WaitingRegrow;
        public event Action Regrowed;

        [Inject]
        private void Construct(LeafWalletPresenter leafWallet, AddCurrencyObjectsAnimation addCurrencyAnimation, FoodRegrowButton regrowButtonTemplate)
        {
            _wallet = leafWallet;
            _addMoneyAnimation = addCurrencyAnimation;
            _regrowButtonTemplate = regrowButtonTemplate;
        }

        private void OnEnable()
        {
            _food = GetComponent<Food>();
            _food.Eaten += OnFoodEaten;

            Scene scene = SceneManager.GetActiveScene();

            if (scene.name != Map1)
                SetAutoRegrowing(state: true);
        }

        private void OnDisable()
        {
            _food.Eaten -= OnFoodEaten;
        }

        public void SetAutoRegrowing(bool state)
        {
            if (state)
                PlayerPrefs.SetInt(IsRegrowingOn, True);
            else
                PlayerPrefs.SetInt(IsRegrowingOn, 0);
        }

        private void OnFoodEaten(Food food)
        {
            _regrowCoroutine = StartCoroutine(WaitingRegrowUpdate());
            _spawnedButton = Instantiate(_regrowButtonTemplate, _food.CellPosition + Vector3.up * 0.5f, Quaternion.identity);
            _spawnedButton.Clicked += OnRegrowButtonClicked;
            _spawnedButton.Init(_food);
        }

        private void OnRegrowButtonClicked()
        {
            if (_regrowCoroutine != null)
                StopCoroutine(_regrowCoroutine);

            _progressbar.gameObject.SetActive(false);
            _spawnedButton.Clicked -= OnRegrowButtonClicked;

            _wallet.AddResource(_bonusRegrow);
            _addMoneyAnimation.PlayLeaf(_spawnedButton.transform.position);
            Regrow(_food);
            Regrowed?.Invoke();
            Destroy(_spawnedButton.gameObject);
        }

        private void Regrow(Food food)
        {
            food.Regrow();
            food.transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up);
            food.transform.localScale = Vector3.zero;
            food.transform.DOScale(1, 1);
        }

        private IEnumerator WaitingRegrowUpdate()
        {
            if (PlayerPrefs.GetInt(IsRegrowingOn) != True)
                yield break;

            _progressbar.gameObject.SetActive(true);
            WaitingRegrow?.Invoke(TimeBetweenRegrow);

            var wait = new WaitForSecondsRealtime(TimeBetweenRegrow);
            yield return wait;

            _progressbar.gameObject.SetActive(false);
            Regrow(_food);
            Destroy(_spawnedButton.gameObject);
        }
    }
}
