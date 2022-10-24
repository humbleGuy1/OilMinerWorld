using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class FoodFingerTutorial : MonoBehaviour
    {
        [SerializeField] private Food _food;
        [SerializeField] private Image _finger;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _tapToCollect;
        [SerializeField] private GameObject _speedTextHint;
        [SerializeField] private GameObject _strenghtTextHint;
        [SerializeField] private GameObject _incomeTextHint;

        private const string FoodActivated = "FoodActivated";
        private const int True = 1;

        public event Action HintHasBegun;

        public bool IsFoodHintFinished => PlayerPrefs.GetInt(FoodActivated) == True;

        private void OnEnable()
        {
            if (PlayerPrefs.GetInt(FoodActivated) == True)
                return;

            _food.Regrowed += OnFoodRegrowed;
            _food.Eaten += (f) => OnFoodEaten();
        }

        private void OnDisable()
        {
            if (PlayerPrefs.GetInt(FoodActivated) == True)
                return;

            _food.Regrowed -= OnFoodRegrowed;
            _food.Eaten -= (f) => OnFoodEaten();
        }

        private void OnFoodRegrowed()
        {
            PlayerPrefs.SetInt(FoodActivated, True);

            _food.Regrowed -= OnFoodRegrowed;
            _food.Eaten -= (f) => OnFoodEaten();

            _finger.gameObject.SetActive(false);
            _animator.enabled = false;
            _tapToCollect.SetActive(false);
        }

        private void OnFoodEaten()
        {
            if (PlayerPrefs.GetInt(FoodActivated) == True)
                return;

            HintHasBegun?.Invoke();
            _finger.enabled = true;
            _animator.enabled = true;
            _speedTextHint.gameObject.SetActive(false);
            _strenghtTextHint.gameObject.SetActive(false);
            _incomeTextHint.gameObject.SetActive(false);
            _tapToCollect.SetActive(true);
        }
    }
}
