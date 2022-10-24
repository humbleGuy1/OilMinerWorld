using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public class FingerHint : MonoBehaviour
    {
        [SerializeField] private Cell _firstDigCell;
        [SerializeField] private Cell _firstDigHardCell;
        [SerializeField] private StoneWalletPresenter _stoneWallet;
        [SerializeField] private AntHouseUpgradeView _antHouseUpgradeView;
        [SerializeField] private FoodRegrower _foodRegrower;
        [SerializeField] private FoodFingerTutorial _foodFingerTutorial;
        [Header("TutorialGoals")]
        [SerializeField] private StarGoal _starGoal;
        [SerializeField] private float _tutorialSpeedGoal;
        [SerializeField] private float _tutorialStrenghtGoal;
        [SerializeField] private float _tutorialIncomeGoal;
        [Header("TextHints")]
        [SerializeField] private GameObject _diggerHouseTextHint;
        [SerializeField] private GameObject _loaderHouseTextHint;
        [SerializeField] private GameObject _digTextHint;
        [SerializeField] private GameObject _speedTextHint;
        [SerializeField] private GameObject _strenghtTextHint;
        [SerializeField] private GameObject _incomeTextHint;
        [Header("Fingers")]
        [SerializeField] private GameObject _finger;
        [SerializeField] private Image _fingerClick;
        [SerializeField] private Image _fingerMapMenu;
        [Header("Buttons")]
        [SerializeField] private Button _strenghtButton;
        [SerializeField] private Button _incomeButton;
        [SerializeField] private Button _speedButton;
        [SerializeField] private Button _increaseIncomeButton;
        [Header("Ant Components")]
        [SerializeField] private LoaderHouse _loaderHouse;
        [SerializeField] private DiggersHouse _diggerHouse;
        [Header("Shop")]
        [SerializeField] private SpeedProduct _speedProduct;
        [SerializeField] private StrenghtProduct _strenghtProduct;
        [SerializeField] private IncomeProduct _incomeProduct;
        [Header("Camera Settings")]
        [SerializeField] private InputRoot _inputRoot;
        [SerializeField] private CameraBounds _tutorBounds;
        [SerializeField] private CameraBounds _defaultBounds;
        [SerializeField] private Region _targetRegion;

        private Vector3 _offsetBetweenFingers;
        private Coroutine _hintCoroutine;
        private Coroutine _fingerAnchorCoroutine;
        private Camera _camera;
        private List<Button> _buttons;

        private const string FirstCellDigHintStarted = nameof(FirstCellDigHintStarted);
        private const string DiggerHouseHintStarted = nameof(DiggerHouseHintStarted);
        private const string SpeedHintFinished = nameof(SpeedHintFinished);
        private const string TutorialFinished = nameof(TutorialFinished);
        private const int True = 1;

        private delegate bool Condition();
        private Condition _stopHintCondition;

        public event Action<int> StepCompleted;

        public bool IsFirstDigHintActivated => PlayerPrefs.GetInt(FirstCellDigHintStarted) == True;
        public bool IsDiggerHouseHintStarted => PlayerPrefs.GetInt(DiggerHouseHintStarted) == True;
        public bool IsSpeedHintFinished => PlayerPrefs.GetInt(SpeedHintFinished) == True;
        public bool IsTutorialFinished => PlayerPrefs.GetInt(TutorialFinished) == True;

        private void Awake()
        {
            _increaseIncomeButton.gameObject.SetActive(false);
            _offsetBetweenFingers = new Vector3(-7.2f, 12, 0);
            _camera = Camera.main;
            _buttons = new List<Button>() { _speedButton, _incomeButton, _strenghtButton };
        }

        private void OnDisable()
        {
            StopCoroutines();

            _firstDigCell.Digged -= (Cell cell) => PlayerPrefs.SetInt(FirstCellDigHintStarted, 0);
            _firstDigCell.Digging -= OnEndDigFirstCellHint;
            _loaderHouse.Cell.Digged -= OnLoaderHouseDigged;
            _speedButton.onClick.RemoveListener(OnEndSpeedHint);
            _strenghtButton.onClick.RemoveListener(OnEndStrenghtHint);
            _incomeButton.onClick.RemoveListener(OnEndIncomeHint);
            _foodRegrower.Regrowed -= OnFoodRegrowed;
        }

        private void Start()
        {
            if (IsTutorialFinished)
            {
                SwitchOnButtons(_buttons.ToArray());
                _inputRoot.SetCameraBounds(_defaultBounds);
                _increaseIncomeButton.gameObject.SetActive(true);

                if (_foodFingerTutorial.IsFoodHintFinished)
                {
                    _foodRegrower.SetAutoRegrowing(true);
                }
                else
                {
                    _foodRegrower.SetAutoRegrowing(false);
                    _foodRegrower.Regrowed += OnFoodRegrowed;
                }

                return;
            }
            else
            {
                _speedProduct.SetTutorialSettings();
                _strenghtProduct.SetTutorialSettings();
                _incomeProduct.SetTutorialSettings();

                if (_loaderHouse.Level >= 2)
                {
                    _inputRoot.SetCameraBounds(_defaultBounds);
                }
                else
                {
                    _inputRoot.SetCameraBounds(_tutorBounds);
                    _targetRegion.Opened += OnTargetRegionOpened;

                    if (_foodFingerTutorial.IsFoodHintFinished)
                        _foodRegrower.SetAutoRegrowing(true);
                    else
                        _foodRegrower.SetAutoRegrowing(false);
                }
            }

            if (IsDiggerHouseHintStarted == false)
            {
                StartDigFirstCellHint();
            }
            else if (_diggerHouse.Level < 3)
            {
                StartDiggerHouseHint();
            }
            else if (_speedProduct.ProductValue.Value < _tutorialSpeedGoal && _loaderHouse.Level >= 2)
            {
                StartSpeedHint();
            }
            else if (_incomeProduct.ProductValue.Value < _tutorialIncomeGoal && _loaderHouse.Level >= 2)
            {
                StartIncomeHint();
            }
            else if (_strenghtProduct.ProductValue.Value < _tutorialStrenghtGoal && _loaderHouse.Level >= 2)
            {
                StartStrenghtHint();
            }
        }

        private void StartDigFirstCellHint()
        {
            _firstDigCell.Digging += OnEndDigFirstCellHint;
            _digTextHint.SetActive(true);
            PlayerPrefs.SetInt(FirstCellDigHintStarted, True);
            _stopHintCondition = () => _firstDigCell.CellState != CellState.Digging;
            _hintCoroutine = StartCoroutine(TapOnPointHint(_firstDigCell.transform));
        }

        private void StartDiggerHouseHint()
        {
            PlayerPrefs.SetInt(DiggerHouseHintStarted, True);
            _diggerHouse.LevelIncreased += OnEndDiggerHouseHint;
            _diggerHouseTextHint.SetActive(true);
            _stopHintCondition = () => _diggerHouse.Level <= 2;
            _hintCoroutine = StartCoroutine(TapOnPointHint(_diggerHouse.Cell.transform));
        }

        private void StartStrenghtHint()
        {
            _strenghtTextHint.SetActive(true);
            SwitchOnButtons(_buttons.ToArray());
            _stopHintCondition = () => _strenghtProduct.ProductValue.Value < _tutorialStrenghtGoal;
            _hintCoroutine = StartCoroutine(ShowButtonHint(_strenghtButton, OnEndStrenghtHint));
        }

        private void StartSpeedHint()
        {
            _speedTextHint.SetActive(true);
            SwitchOnButtons(_speedButton);
            _stopHintCondition = () => _speedProduct.ProductValue.Value < _tutorialSpeedGoal;
            _hintCoroutine = StartCoroutine(ShowButtonHint(_speedButton, OnEndSpeedHint));
        }

        private void StartIncomeHint()
        {
            _incomeButton.onClick.AddListener(OnEndIncomeHint);
            _incomeTextHint.SetActive(true);
            SwitchOnButtons(_speedButton);
            _stopHintCondition = () => _incomeProduct.ProductValue.Value < _tutorialIncomeGoal;
            _hintCoroutine = StartCoroutine(ShowButtonHint(_incomeButton, OnEndIncomeHint));
        }

        private void OnEndDigFirstCellHint(Cell cell = null)
        {
            StepCompleted?.Invoke(Convert.ToInt32(StepNumbers.DigFirstCellHint));
            StopCoroutines();
            _finger.gameObject.SetActive(false);
            _fingerClick.gameObject.SetActive(false);
            _digTextHint.SetActive(false);

            Invoke(nameof(StartDiggerHouseHint), 5);
        }

        private void OnEndDiggerHouseHint()
        {
            if (_diggerHouse.Level < 2)
                return;

            StopCoroutines();
            StepCompleted?.Invoke(Convert.ToInt32(StepNumbers.DiggerHouseHint));
            _diggerHouse.LevelIncreased -= OnEndDiggerHouseHint;
            _finger.gameObject.SetActive(false);
            _fingerClick.gameObject.SetActive(false);
            _diggerHouseTextHint.SetActive(false);

            _loaderHouse.Cell.Digged += OnLoaderHouseDigged;
        }

        private void OnLoaderHouseDigged(Cell cell = null)
        {
            _foodRegrower.Regrowed += OnFoodRegrowed;
            _foodRegrower.SetAutoRegrowing(false);
        }

        private void OnFoodRegrowed() => _foodRegrower.SetAutoRegrowing(true);

        private void OnEndSpeedHint()
        {
            if (_speedProduct.ProductValue.Value < _tutorialSpeedGoal)
                return;

            StopCurrentButtonHint(_speedButton, OnEndSpeedHint);
            StepCompleted?.Invoke(Convert.ToInt32(StepNumbers.SpeedHint));
            _finger.gameObject.SetActive(false);
            _fingerClick.gameObject.SetActive(false);
            _speedTextHint.SetActive(false);

            Invoke(nameof(StartIncomeHint), 1);
        }

        private void OnEndIncomeHint()
        {
            if (_incomeProduct.ProductValue.Value < _tutorialIncomeGoal)
                return;

            StopCurrentButtonHint(_incomeButton, OnEndSpeedHint);
            StepCompleted?.Invoke(Convert.ToInt32(StepNumbers.IncomeHint));
            _finger.gameObject.SetActive(false);
            _fingerClick.gameObject.SetActive(false);
            _incomeTextHint.SetActive(false);

            Invoke(nameof(StartStrenghtHint), 1);
        }

        private void OnEndStrenghtHint()
        {
            if (_strenghtProduct.ProductValue.Value < _tutorialStrenghtGoal)
                return;

            StepCompleted?.Invoke(Convert.ToInt32(StepNumbers.StrenghtHint));
            StopCurrentButtonHint(_strenghtButton, OnEndStrenghtHint);
            _strenghtTextHint.SetActive(false);

            EndTutorial();
        }

        private void EndTutorial()
        {
            SwitchOnButtons(_buttons.ToArray());
            PlayerPrefs.SetInt(TutorialFinished, True);
            _inputRoot.SetCameraBounds(_defaultBounds);
            _starGoal.ShowImages();
            _speedProduct.SetDefaultSetings();
            _incomeProduct.SetDefaultSetings();
            _strenghtProduct.SetDefaultSetings();
            _increaseIncomeButton.gameObject.SetActive(true);
        }

        private void OnTargetRegionOpened()
        {
            StepCompleted?.Invoke(Convert.ToInt32(StepNumbers.TutorialRegionPassed));
            _targetRegion.Opened -= OnTargetRegionOpened;
            _inputRoot.SetCameraBounds(_defaultBounds);

            Invoke(nameof(StartSpeedHint), 1);
        }


        private void StopCurrentButtonHint(Button activeButton, UnityAction action)
        {
            activeButton.onClick.RemoveListener(action);
            StopCoroutines();

            _finger.SetActive(false);
            _fingerClick.gameObject.SetActive(false);
        }

        private IEnumerator ShowButtonHint(Button activeButton, UnityAction action)
        {
            activeButton.GetComponent<ProductView>().UpdateView();
            activeButton.gameObject.SetActive(true);
            _finger.SetActive(true);
            activeButton.onClick.AddListener(action);

            while (_stopHintCondition.Invoke())
            {
                _finger.transform.position = activeButton.transform.position;
                yield return ShowFingerClick();
            }
        }

        private void SwitchOnButtons(params Button[] activeButtons)
        {
            foreach (Button button in activeButtons)
                button.gameObject.SetActive(true);
        }

        private IEnumerator TapOnPointHint(Transform hintPoint)
        {
            _fingerAnchorCoroutine = StartCoroutine(BackFingerToPoint(hintPoint));

            while (_stopHintCondition.Invoke())
            {
                if (IsFingerOnCell())
                    yield return ShowFingerClick();

                yield return new WaitForSeconds(0.01f);
            }

            bool IsFingerOnCell() => Vector3.Distance(_finger.transform.position, _camera.WorldToScreenPoint(hintPoint.position)) < 355;
        }

        private IEnumerator ShowFingerClick()
        {
            float clickDelay = 0.3f;
            WaitForSeconds wait = new WaitForSeconds(clickDelay);

            yield return wait;
            _fingerClick.transform.position = _finger.transform.position + _offsetBetweenFingers;
            _finger.gameObject.SetActive(false);
            _fingerClick.gameObject.SetActive(true);
            yield return wait;
            _fingerClick.gameObject.SetActive(false);
            _finger.gameObject.SetActive(true);
            yield return wait;
        }

        private IEnumerator BackFingerToPoint(Transform hintPoint)
        {
            float speed = 10;
            Vector3 offset = new Vector3(40f, 0, 350f);

            while (true)
            {
                _finger.transform.position = Vector3.Lerp(_finger.transform.position, _camera.WorldToScreenPoint(hintPoint.transform.position) + offset, Time.deltaTime * speed);
                yield return null;
            }
        }

        private void StopCoroutines()
        {
            if (_hintCoroutine != null)
                StopCoroutine(_hintCoroutine);

            if (_fingerAnchorCoroutine != null)
                StopCoroutine(_fingerAnchorCoroutine);
        }
    }
}
