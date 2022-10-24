using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class StarGoal : MonoBehaviour
    {
        [SerializeField] private GoalView _view;
        [SerializeField] private Anthill _anthill;
        [SerializeField] private int _starsForWin;

        [Header("Images")]
        [SerializeField] private GameObject[] _canvasObjects;

        private bool _isHided = false;
        private const float DealyInit = 0.2f;
        private AddCurrencyObjectsAnimation _addCurrencyObjectsAnimation;

        public int StarsCount { get; private set; }

        public event Action ProgressReached;
        public event Action StarCollected;

        [Inject]
        private void Construct(AddCurrencyObjectsAnimation addCurrencyAnimation)
        {
            _addCurrencyObjectsAnimation = addCurrencyAnimation;
        }

        private void OnEnable()
        {
            for (int i = 0; i < _anthill.AllCells.Count; i++)
            {
                _anthill.AllCells[i].LoaderHouse.LevelIncreased += OnStarCollected;
                _anthill.AllCells[i].DiggersHouse.LevelIncreased += OnStarCollected;
                _anthill.AllCells[i].LoaderHouse.LevelIncreasedCellPosition += PlayFX;
                _anthill.AllCells[i].DiggersHouse.LevelIncreasedCellPosition += PlayFX;
            }

            Invoke(nameof(OnStarCollected), DealyInit);
        }

        private void OnDisable()
        {
            for (int i = 0; i < _anthill.AllCells.Count; i++)
            {
                _anthill.AllCells[i].LoaderHouse.LevelIncreased -= OnStarCollected;
                _anthill.AllCells[i].DiggersHouse.LevelIncreased -= OnStarCollected;
                _anthill.AllCells[i].LoaderHouse.LevelIncreasedCellPosition -= PlayFX;
                _anthill.AllCells[i].DiggersHouse.LevelIncreasedCellPosition -= PlayFX;
            }
        }

        public void ShowImages()
        {
            for (int i = 0; i < _canvasObjects.Length; i++)
                _canvasObjects[i].SetActive(true);

            _view.Render(StarsCount, _starsForWin);
            _isHided = false;
        }

        public void HideImages()
        {
            for (int i = 0; i < _canvasObjects.Length; i++)
                _canvasObjects[i].SetActive(false);

            _isHided = true;
        }

        private void OnStarCollected()
        {
            IReadOnlyList<Cell> cells = _anthill.AllCells;
            int count = 0;

            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i].LoaderHouse.gameObject.activeSelf)
                {
                    count += cells[i].LoaderHouse.Level - 1;
                }
                else if (cells[i].DiggersHouse.gameObject.activeSelf)
                {
                    count += cells[i].DiggersHouse.Level - 1;
                }
            }

            StarsCount = count;
            Render(_isHided);
            StarCollected?.Invoke();

            if (StarsCount >= _starsForWin)
                ProgressReached?.Invoke();
        }

        private void Render(bool hided)
        {
            if (hided == false)
                _view.Render(StarsCount, _starsForWin);
        }

        private void PlayFX(Transform cell)
        {
            if(_isHided == false)
                _addCurrencyObjectsAnimation.PlayStars(cell.position);
        }
    }
}
