using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    public class EndGame : GUIDObject
    {
        [SerializeField] private StarGoal _goals;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private UpgradeMenu _upgradeMenu;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private List<Button> _buyButtons;

        public bool IsMenuShowed { get; private set; }

        public event Action AnthillDone;
        public event Action NextButtonClicked;

        private void OnEnable()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
            _goals.ProgressReached += OnGoalProgressReached;
            SetActiveCanvasGroupInteractable(false);
        }

        private void OnDisable()
        {
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClick);
            _goals.ProgressReached -= OnGoalProgressReached;
        }

        private void OnGoalProgressReached()
        {
            var save = new LevelSave(GUID);
            save.Load();

            if (save.Done)
                return;

            save.Done = true;
            save.Save();

            AnthillDone?.Invoke();

            foreach (Button button in _buyButtons)
                button.gameObject.SetActive(false);

            IsMenuShowed = true;
            SetActiveCanvasGroupInteractable(true);
            Extentions.EnableGroup(_canvasGroup);

            _upgradeMenu.OnCloseClicked();
        }

        private void OnNextLevelButtonClick()
        {
            NextButtonClicked?.Invoke();
        }

        private void SetActiveCanvasGroupInteractable(bool isActive)
        {
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
        }
    }
}
