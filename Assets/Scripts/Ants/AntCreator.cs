using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class AntCreator : MonoBehaviour
    {
        [SerializeField] private Digger _diggerTemplate;
        [SerializeField] private Loader _loaderTemplate;
        [SerializeField] private Transform _diggersContainer;
        [SerializeField] private Transform _loadersContainer;

        private Anthill _anthill;

        public Cell QueenCell { get; private set; }

        private void OnDestroy()
        {
            for (int i = 0; i < _anthill.AllCells.Count; i++)
            {
                Cell cell = _anthill.AllCells[i];
                CellData.CellType cellType = _anthill.AllCells[i].CellType;

                if (cellType == CellData.CellType.LoaderHouse || cellType == CellData.CellType.DiggersHouse)
                {
                    _anthill.AllCells[i].Opened -= OnAntHouseLevelIncrease;
                    cell.LoaderHouse.LevelIncreased -= OnAntHouseLevelIncrease;
                    cell.DiggersHouse.LevelIncreased -= OnAntHouseLevelIncrease;
                }
            }
        }

        public void Init(Anthill anthill, Cell queenCell)
        {
            _anthill = anthill;
            QueenCell = queenCell;

            for (int i = 0; i < _anthill.AllCells.Count; i++)
            {
                Cell cell = _anthill.AllCells[i];
                CellData.CellType cellType = _anthill.AllCells[i].CellType;

                if (cellType == CellData.CellType.LoaderHouse || cellType == CellData.CellType.DiggersHouse)
                {
                    _anthill.AllCells[i].Opened += OnAntHouseLevelIncrease;
                    cell.LoaderHouse.LevelIncreased += OnAntHouseLevelIncrease;
                    cell.DiggersHouse.LevelIncreased += OnAntHouseLevelIncrease;
                }
            }

            CreateAnts(_anthill.AntHouses);
        }

        public void Create(AntType type, AntHouse house, Cell spawnCell)
        {
            if (house.CurrentAntsCount >= house.AvailableAntsCount)
                return;

            Transform container = type == AntType.Loader ? _loadersContainer : _diggersContainer;
            Ant template = TemplateBy(type);
            Ant ant = Instantiate(template, spawnCell.transform.position + Vector3.up * 0.15f, Quaternion.identity, container);

            house.AddAnt(ant);
        }

        private void OnAntHouseLevelIncrease()
            => CreateAnts(_anthill.AntHouses);

        private void CreateAnts(IReadOnlyList<AntHouse> houses)
        {
            foreach (AntHouse house in houses)
            {
                house.TryCreateAnts();
            }
        }

        private Ant TemplateBy(AntType type)
        {
            if (type == AntType.Digger)
                return _diggerTemplate;

            return _loaderTemplate;
        }
    }
}
