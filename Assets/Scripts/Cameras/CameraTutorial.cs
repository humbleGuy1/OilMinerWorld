using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts
{
    public class CameraTutorial : MonoBehaviour
    {
        [SerializeField] private Region _region;
        [SerializeField] private UpgradeMenu _upgradeMenu;
        [SerializeField] private Transform _targetCamera;
        [SerializeField] private Transform _newPositionPoint;
        [SerializeField] private Transform _leafPositionPoint;
        [SerializeField] private FoodFingerTutorial _foodTutorial;

        private const string TutorFinished = "TutorFinished";
        private const float Duration = 1f;
        private const float Delay = 0.1f;
        private const int True = 1;

        private void OnEnable()
        {
            if (PlayerPrefs.GetInt(TutorFinished) == True)
                return;

            _region.Opened += OnRegionOpened;
            _foodTutorial.HintHasBegun += OnFoodHintHasBegun;
        }

        private void OnDisable()
        {
            if (PlayerPrefs.GetInt(TutorFinished) == True)
                return;

            _region.Opened -= OnRegionOpened;            
            _foodTutorial.HintHasBegun -= OnFoodHintHasBegun;
        }

        private void OnRegionOpened()
        {
            Invoke(nameof(HideMenu), Delay);
            _targetCamera.DOMove(_newPositionPoint.position, Duration);
        }

        private void OnFoodHintHasBegun()
        {
            Invoke(nameof(HideMenu), Delay);
            _targetCamera.DOMove(_leafPositionPoint.position, Duration);
        }

        private void HideMenu()
        {
            _upgradeMenu.OnTutorialActionDone();
        }
    }
}
