using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;
using TMPro;
using static UnityEngine.UI.CanvasScaler;
using System.Linq;
using NSubstitute.Core;

namespace Assets.Scripts
{
    [SelectionBase]
    public class Region : GUIDObject
    {
        [SerializeField] private int _starsToUnlock;
        [SerializeField] private bool _tutorial = false;
        [SerializeField] private CellPriceUpdate _cellPriceUpdate;
        [SerializeField] private RegionSave.RegionState _regionState;
        [SerializeField] private List<Cell> _cells;
        [SerializeField] private List<DiggersHouse> _diggerHouses;
        [SerializeField] private CellsAnimator _cellsAnimator;
        [SerializeField] private List<LoaderHouse> _loaderHouses;
        [SerializeField] private SpiderSpawner _spiderSpawner;
        [SerializeField] private int _index;

        public int Index => _index;
        private RegionViewGoal _regionViewGoal;
        private StarGoal _starsGoal;
        private int _defaulHexPrice = 100;
        private int _priceHexes => _defaulHexPrice + 300*(_index-1);

        private int _upgradeMultiply => _index;
        public float PieceMultiply => (_index-1)*0.2f;

        public SpiderSpawner SpiderSpawner => _spiderSpawner;
        public RegionSave.RegionState RegiondState => _regionState;
        public IReadOnlyList<DiggersHouse> DiggerHouses => _diggerHouses;
        public IReadOnlyList<Cell> Cells => _cells;

        public event Action Opened;
        public event Action StarsCollected;

        private void OnValidate()
        {
            //_diggerHouses.Clear();
            //_loaderHouses.Clear();
            //var diggerCells = _cells.FindAll(cell => cell.CellType == CellData.CellType.DiggersHouse);

            //foreach (var cell in diggerCells)
            //{
            //    _diggerHouses.Add(cell.DiggersHouse);
            //}

            //var loaderCells = _cells.FindAll(cell => cell.CellType == CellData.CellType.LoaderHouse);

            //foreach (var cell in loaderCells)
            //{
            //    _loaderHouses.Add(cell.LoaderHouse);
            //}
        }

        [Inject]
        private void Construct(StarGoal starGoal)
        {
            _starsGoal = starGoal;

            _cellPriceUpdate.Initialize(this);

            _regionViewGoal = GetComponentInChildren<RegionViewGoal>();

            int hexPrice = _priceHexes;

            if (_index > 1)
                hexPrice -= 100;

            foreach (var cell in _cells)
                cell.Initialize(hexPrice, PieceMultiply, _upgradeMultiply, this);
        }

        public void Initialize()
        {
            foreach (var cell in _cells)
                cell.SetupState();

            _spiderSpawner.Init(_loaderHouses);

            SetTargetsToLoaderHouse();

            var save = new RegionSave(GUID);
            save.Load();
            
            if (save.HasSave)
                _regionState = save.CurrentRegionState;

            if (_regionState == RegionSave.RegionState.Blocked)
            {
                _starsGoal.StarCollected += OnStarsCountUpdated;
                OnStarsCountUpdated();
                Block();
            }
            else
            {
                Unblock();
            }
        }

        private void SetTargetsToLoaderHouse()
        {
            var foodCells = _cells.Where(cell => cell.CellType == CellData.CellType.Food).ToArray();
            var enemyCells = _cells.Where(cell => cell.CellType == CellData.CellType.Enemy).ToArray();

            foreach (var loaderHouse in _loaderHouses)
            {
                loaderHouse.SetTargets(foodCells, enemyCells);
            }
        }

        private void Block()
        {
            for (int i = 0; i < _cells.Count; i++)
                _cells[i].Block();
        }

        public void Unblock()
        {
            if (_tutorial)
                return;

            for (int i = 0; i < _cells.Count; i++)
            {
                _cells[i].Unblock();

                if (_cells[i].CellType == CellData.CellType.DiggersHouse && _cells[i].CellState != CellData.CellState.Opened)
                {
                    foreach (var neighborCells in _cells[i].NeighborCells.GetNeighborCells())
                    {
                        if (neighborCells.Region == this)
                            neighborCells.Unlock();
                    }

                    _cells[i].Open();
                }
            }

            if(_cellsAnimator != null)
                _cellsAnimator.Animate(_cells);
            
            _regionState = RegionSave.RegionState.Unblocked;
            var save = new RegionSave(GUID);
            save.Load();
            save.CurrentRegionState = _regionState;
            save.Save();

            Opened?.Invoke();
        }

        private void OnStarsCountUpdated()
        {
            if (_tutorial)
                return;

            _regionViewGoal.SetValue(_starsGoal.StarsCount, _starsToUnlock);

            if (_starsGoal.StarsCount >= _starsToUnlock)
            {
                //Unblock();
                StarsCollected?.Invoke();
                _regionViewGoal.SwitchToCompletedState();
                _starsGoal.StarCollected -= OnStarsCountUpdated;
                //_regionViewGoal.HideCanvas();
            }

            //if (_regionState == RegionSave.RegionState.Unblocked)
                //_regionViewGoal.HideCanvas();
        }
    }
}
