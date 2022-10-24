using UnityEngine;

namespace Assets.Scripts
{
    public class CameraLocalMapSwitcher : CamerasSwitcher
    {
        [SerializeField] private AnthillChooseButtons _anthillChooseButtons;

        protected override void Init()
        {
            _anthillChooseButtons.AnthillClicked += OnAnthillAreaOpened;

            EnableCamera(_mainAnthillMapCamera1);
        }

        private void OnDisable()
        {
            _anthillChooseButtons.AnthillClicked -= OnAnthillAreaOpened;
        }

        private void OnAnthillAreaOpened()
        {
            EnableCamera(_mainAnthillMapCamera2);
        }
    }
}
