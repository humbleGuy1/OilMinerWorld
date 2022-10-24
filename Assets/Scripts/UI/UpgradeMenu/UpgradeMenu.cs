using System;
using System.Collections;
using NSubstitute.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UpgradeMenu : MonoBehaviour
    {
        [SerializeField] private InputRoot _inputRoot;
        [SerializeField] private CellBuyer _cellBuyer;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private StoneWalletPresenter _stoneWalletPresenter;
        [Header("Upgrade Button")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _price;
        [Header("Stars")]
        [SerializeField] private Image _star1;
        [SerializeField] private Image _star2;
        [SerializeField] private Image _star3;
        [SerializeField] private Sprite _activeStar;
        [SerializeField] private Sprite _deactiveStar;
        [Header("Stats")]
        [SerializeField] private HouseStatsView _antStatView;
        [SerializeField] private FictionalHouseStatsView _resourcePerSecView;
        [SerializeField] private GameObject _diggersNest;
        [SerializeField] private GameObject _loadersNest;

        private Cell _cell;
        private Coroutine _delayedClose;
        private AntHouse _currentAntHouse;
        //private bool isClosed;

        public bool IsMaxLevel => _currentAntHouse.IsMaxLevel;
        public bool HaveAntHouse => _currentAntHouse != null;
        public bool HasOpened { get; private set; } = false;
        public static bool IsOpen { get; private set; }

        public static UpgradeMenu Instance { get; private set; }

        public event Action PanelOpened;
        public event Action<bool> PanelClosed;
        public event Action<Cell> CellPanelOpened;
        public event Action<bool> CanvasPanelOpened;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            _inputRoot.UpgradeClicked += OpenMenu;
            _stoneWalletPresenter.ValueChanged += ChangeUpgradeButtonColor;
            _closeButton.onClick.AddListener(OnCloseClicked);
            _upgradeButton.onClick.AddListener(OnUpgradeClicked);
        }

        private void OnDisable()
        {
            _inputRoot.UpgradeClicked -= OpenMenu;
            _stoneWalletPresenter.ValueChanged -= ChangeUpgradeButtonColor;
            _closeButton.onClick.RemoveListener(OnCloseClicked);
            _upgradeButton.onClick.RemoveListener(OnUpgradeClicked);
        }

        private void OnApplicationFocus(bool focus)
        {
            IsOpen = HasOpened;
        }

        public void OnTutorialActionDone()
        {
            Close();
            PanelClosed?.Invoke(false);
        }

        public void OnCloseClicked()
        {
            Close();
            PanelClosed?.Invoke(true);
        }

        private void Close()
        {
            if(_delayedClose != null)
                StopCoroutine(_delayedClose);

            _delayedClose = StartCoroutine(DealyedCloseState());
            Extentions.DisableGroup(_canvasGroup);
            CanvasPanelOpened?.Invoke(false);
        }

        private void OnUpgradeClicked()
        {
            _cellBuyer.TryUpgrade(_cell);
            OpenMenu(_cell);
        }

        private void OpenMenu(Cell cell)
        {
            if (_delayedClose != null)
                StopCoroutine(_delayedClose);

            if (_cell == null)
            {
                CanvasPanelOpened?.Invoke(true);
                CellPanelOpened?.Invoke(cell);
            }

            HasOpened = true;
            IsOpen = true;
            Extentions.EnableGroup(_canvasGroup);
            _cell = cell;
            cell.CellPriceView.HideStonePricePanel();

            DisableText();
            UpdatePrice(cell, out AntHouse house);

            _currentAntHouse = house;
            _antStatView.UpdateInfo(_currentAntHouse.CurrentAntsCount, _currentAntHouse.AntsPerLevel, _currentAntHouse.IsMaxLevel, _currentAntHouse.houseSprites._houseStats);
            _resourcePerSecView.UpdatInfo(_currentAntHouse.ResourcePerSec, _currentAntHouse.Level, _currentAntHouse.IsMaxLevel, _currentAntHouse.houseSprites._houseStats);


            EnableButtons();
            ChangeUpgradeButtonColor();
            ChangeStarsState(house);

            PanelOpened?.Invoke();
        }

        private IEnumerator DealyedCloseState()
        {
            if(_currentAntHouse.IsMaxLevel)
                _cell.CellPriceView.HideStonePricePanel();

            _cell = null;
            IsOpen = false;

            yield return new WaitForSecondsRealtime(0.35f);

            HasOpened = false;
            _delayedClose = null;
        }

        private void UpdatePrice(Cell cell, out AntHouse house)
        {

            if (cell.DiggersHouse.gameObject.activeSelf)
            {
                _price.text = cell.DiggersHouse.NextLevelPrice.ToString();
                _diggersNest.SetActive(true);
                house = cell.DiggersHouse;
            }
            else
            {
                _price.text = cell.LoaderHouse.NextLevelPrice.ToString();
                _loadersNest.SetActive(true);
                house = cell.LoaderHouse;
            }
        }

        private void EnableButtons()
        {
            _upgradeButton.gameObject.SetActive(true);
        }

        private void DisableText()
        {
            _diggersNest.SetActive(false);
            _loadersNest.SetActive(false);
        }

        private void ChangeStarsState(AntHouse house)
        {
            switch (house.Level)
            {
                case 1:
                    _star1.sprite = _deactiveStar;
                    _star2.sprite = _deactiveStar;
                    _star3.sprite = _deactiveStar;
                    break;
                case 2:
                    _star1.sprite = _activeStar;
                    _star2.sprite = _deactiveStar;
                    _star3.sprite = _deactiveStar;
                    break;
                case 3:
                    _star1.sprite = _activeStar;
                    _star2.sprite = _activeStar;
                    _star3.sprite = _deactiveStar;
                    break;
                case 4:
                    _star1.sprite = _activeStar;
                    _star2.sprite = _activeStar;
                    _star3.sprite = _activeStar;
                    _upgradeButton.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public bool CanBuy()
        {
            return _currentAntHouse.CanBuy(_stoneWalletPresenter);
        }

        private void ChangeUpgradeButtonColor()
        {
            if (_currentAntHouse == null)
                return;

            if (_currentAntHouse.CanBuy(_stoneWalletPresenter) == false)
                _price.color = Color.grey;
            else
                _price.color = Color.white;
        }
    }
}
