using System;
using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MapButtons : MonoBehaviour
    {
        [SerializeField] private Button _openAnthillMap;
        [SerializeField] private EndGame _endGame;
        [SerializeField] private bool _firstLevel = false;

        private const float Delay = 1f;

        public event Action LocalMapCliked;

        private void OnEnable()
        {
            if(_firstLevel)
            {
                var save = new LevelSave(_endGame.GUID);
                save.Load();

                if (save.Done)
                    _openAnthillMap.gameObject.SetActive(true);
                else
                    _openAnthillMap.gameObject.SetActive(false);
            }

            _openAnthillMap.onClick.AddListener(OnAnthillMapClicked);
        }

        private void OnDisable()
        {
            _openAnthillMap.onClick.RemoveListener(OnAnthillMapClicked);
        }

        private void OnAnthillMapClicked()
        {
            LocalMapCliked?.Invoke();
            Extentions.DisableGroup(_openAnthillMap.GetComponent<CanvasGroup>());
            Invoke(nameof(FullMap), Delay);
        }

        private void LoadMap()
        {
            FullMap.Load();
        }
    }
}
