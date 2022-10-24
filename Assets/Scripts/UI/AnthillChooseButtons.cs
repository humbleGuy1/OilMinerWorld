using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AnthillChooseButtons : MonoBehaviour
    {
        [SerializeField] private TMP_Text _loadingText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [Header("Buttons")]
        [SerializeField] private Button _1Anthill;
        [SerializeField] private Button _2Anthill;
        [SerializeField] private Button _3Anthill;
        [SerializeField] private Button _4Anthill;
        [SerializeField] private Button _5Anthill;
        [SerializeField] private Button _6Anthill;

        public event Action AnthillClicked;
        public event Action FirstAnthillClicked;
        public event Action SecondAnthillClicked;
        public event Action ThirdAnthillClicked;
        public event Action FourthAnthillClicked;
        public event Action FifthAnthillClicked;
        public event Action SixthAnthillClicked;

        private void OnEnable()
        {
            _1Anthill.onClick.AddListener(OnFirstAnthillClicked);
            _2Anthill.onClick.AddListener(OnSecondAnthillClicked);
            _3Anthill.onClick.AddListener(OnThirdAnthillClicked);
            _4Anthill.onClick.AddListener(OnFourthAnthillClicked);
            _5Anthill.onClick.AddListener(OnFifthAnthillClicked);
            _6Anthill.onClick.AddListener(OnSixthAnthillClicked);
        }

        private void OnDisable()
        {
            _1Anthill.onClick.RemoveListener(OnFirstAnthillClicked);
            _2Anthill.onClick?.RemoveListener(OnSecondAnthillClicked);
            _3Anthill.onClick?.RemoveListener(OnThirdAnthillClicked);
            _4Anthill.onClick?.RemoveListener(OnFourthAnthillClicked);
            _5Anthill.onClick?.RemoveListener(OnFifthAnthillClicked);
            _6Anthill.onClick?.RemoveListener(OnSixthAnthillClicked);
        }

        private void OnFirstAnthillClicked()
        {
            FirstAnthillClicked?.Invoke();
            OnButtonClicked();
        }

        private void OnSecondAnthillClicked()
        {
            SecondAnthillClicked?.Invoke();
            OnButtonClicked();
        }

        private void OnThirdAnthillClicked()
        {
            ThirdAnthillClicked?.Invoke();
            OnButtonClicked();
        }

        private void OnFourthAnthillClicked()
        {
            FourthAnthillClicked?.Invoke();
            OnButtonClicked();
        }

        private void OnFifthAnthillClicked()
        {
            FifthAnthillClicked?.Invoke();
            OnButtonClicked();
        }

        private void OnSixthAnthillClicked()
        {
            SixthAnthillClicked?.Invoke();
            OnButtonClicked();
        }

        private void OnButtonClicked()
        {
            Extentions.DisableGroup(_canvasGroup);
            AnthillClicked?.Invoke();
            _loadingText.enabled = true;
        }
    }
}
