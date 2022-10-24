using System;
using IJunior.TypedScenes;
using UnityEngine;

namespace Assets.Scripts
{
    public class AnthillSwitcher : MonoBehaviour
    {
        [SerializeField] private AnthillChooseButtons _anthillChooseButtons;

        public event Action AnthillFirst;
        public event Action AnthillSecond;
        public event Action AnthillThird;
        public event Action AnthillFourth;
        public event Action AnthillFifth;
        public event Action AnthillSixth;

        private void OnEnable()
        {
            _anthillChooseButtons.FirstAnthillClicked += OnFirstAnthillChose;
            _anthillChooseButtons.SecondAnthillClicked += OnSecondAnthillChose;
            _anthillChooseButtons.ThirdAnthillClicked += OnThirdAnthillChose;
            _anthillChooseButtons.FourthAnthillClicked += OnFourthAnthillChose;
            _anthillChooseButtons.FifthAnthillClicked += OnFifthAnthillChose;
            _anthillChooseButtons.SixthAnthillClicked += OnSixthAnthillChose;
        }

        private void OnDisable()
        {
            _anthillChooseButtons.FirstAnthillClicked -= OnFirstAnthillChose;
            _anthillChooseButtons.SecondAnthillClicked -= OnSecondAnthillChose;
            _anthillChooseButtons.ThirdAnthillClicked -= OnThirdAnthillChose;
            _anthillChooseButtons.FourthAnthillClicked -= OnFourthAnthillChose;
            _anthillChooseButtons.FifthAnthillClicked -= OnFifthAnthillChose;
            _anthillChooseButtons.SixthAnthillClicked -= OnSixthAnthillChose;
        }

        private void OnFirstAnthillChose()
        {
            AnthillFirst?.Invoke();
        }

        private void OnSecondAnthillChose()
        {
            AnthillSecond?.Invoke();
        }

        private void OnThirdAnthillChose()
        {
            AnthillThird?.Invoke();
        }

        private void OnFourthAnthillChose()
        {
            AnthillFourth?.Invoke();
        }

        private void OnFifthAnthillChose()
        {
            AnthillFifth?.Invoke();
        }

        private void OnSixthAnthillChose()
        {
            AnthillSixth?.Invoke();
        }
    }
}
