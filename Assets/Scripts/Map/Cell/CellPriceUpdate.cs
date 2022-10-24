using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class CellPriceUpdate : MonoBehaviour
    {
        private Region _region;
        private UpgradeMenu _upgradeMenu;
        private LeafWalletPresenter _leafWalletPresenter;
        private StoneWalletPresenter _stoneWalletPresenter;

        private List<Cell> _tempCellsToChangeView = new List<Cell>();

        [Inject]
        private void Construct(LeafWalletPresenter leafWallet, StoneWalletPresenter stoneWallet, UpgradeMenu upgradeMenu)
        {
            _upgradeMenu = upgradeMenu;
            _leafWalletPresenter = leafWallet;
            _stoneWalletPresenter = stoneWallet;

            _upgradeMenu.CanvasPanelOpened += OnCanvasPanelOpened;
            _leafWalletPresenter.ValueChanged += OnLeafWalletValueChanged;
            _stoneWalletPresenter.ValueChanged += OnStoneWalletValueChanged;
        }

        private void OnDestroy()
        {
            _upgradeMenu.CanvasPanelOpened -= OnCanvasPanelOpened;
            _leafWalletPresenter.ValueChanged -= OnLeafWalletValueChanged;
            _stoneWalletPresenter.ValueChanged -= OnStoneWalletValueChanged;

            foreach (var cell in _region.Cells)
            {
                cell.Opened -= OnStoneWalletValueChanged;
                cell.Unlocked -= OnLeafWalletValueChanged;
                cell.LoaderHouse.LevelIncreased -= OnStoneWalletValueChanged;
                cell.DiggersHouse.LevelIncreased -= OnStoneWalletValueChanged;
            }
        }

        private void Start()
        {
            OnLeafWalletValueChanged();
            OnStoneWalletValueChanged();
        }

        public void Initialize(Region region)
        {
            if (_region != null)
                throw new InvalidOperationException("Already initialized");

            _region = region;

            foreach (var cell in _region.Cells)
            {
                cell.Opened += OnStoneWalletValueChanged;
                cell.Unlocked += OnLeafWalletValueChanged;
                cell.LoaderHouse.LevelIncreased += OnStoneWalletValueChanged;
                cell.DiggersHouse.LevelIncreased += OnStoneWalletValueChanged;
            }
        }

        private void OnCanvasPanelOpened(bool active)
        {
            ChangeViewPrice(active);
            OnStoneWalletValueChanged();
        }

        private void ChangeViewPrice(bool active)
        {
            if(_tempCellsToChangeView.Count == 0)
                _tempCellsToChangeView = _region.Cells.Where(view => view.CellPriceView.IsActive).ToList();

            for (int i = 0; i < _tempCellsToChangeView.Count; i++)
            {
                CellPriceView cellPriceView = _tempCellsToChangeView[i].CellPriceView;

                if (active)
                    cellPriceView.HideLeafPricePanel();
                else
                    cellPriceView.ShowLeafPricePanel();
            }

            if (active == false)
                _tempCellsToChangeView.Clear();
        }

        private void OnLeafWalletValueChanged()
        {
            for (int i = 0; i < _region.Cells.Count; i++)
            {
                Cell cell = _region.Cells[i];

                if (cell.CellState == CellData.CellState.Unlocked)
                {
                    if (cell.Price <= _leafWalletPresenter.Value)
                        cell.CellPriceView.RenderOpenForSale();
                    else
                        cell.CellPriceView.RenderCloseForSale();
                }

                cell.CellPriceView.RenderCellPrice(cell.Price);
            }
        }

        private void OnStoneWalletValueChanged()
        {
            for (int i = 0; i < _region.Cells.Count; i++)
            {
                Cell cell = _region.Cells[i];

                if(cell.CellState == CellData.CellState.Opened == false)
                    cell.CellPriceView.HideStonePricePanel();
                else if(cell.CellType == CellData.CellType.DiggersHouse && cell.DiggersHouse.IsValidate(_stoneWalletPresenter))
                    cell.CellPriceView.ShowStonePricePanel();
                else if(cell.CellType == CellData.CellType.LoaderHouse && cell.LoaderHouse.IsValidate(_stoneWalletPresenter))
                    cell.CellPriceView.ShowStonePricePanel();
                else
                    cell.CellPriceView.HideStonePricePanel();
            }
        }
    }
}
