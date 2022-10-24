using System;
using UnityEngine;
using IJunior.TypedScenes;

namespace Assets.Scripts
{
    public class LocalMapLoader : MonoBehaviour
    {
        [SerializeField] private MapButtons _mapButtons;
        [SerializeField] private EndGame _endGame;

        private void OnEnable()
        {
            _endGame.NextButtonClicked += OnMapClikced;
            _mapButtons.LocalMapCliked += OnMapClikced;
        }

        private void OnDisable()
        {
            _endGame.NextButtonClicked -= OnMapClikced;
            _mapButtons.LocalMapCliked -= OnMapClikced;            
        }

        private void OnMapClikced()
        {
            LocalMap.Load();
        }
    }
}
