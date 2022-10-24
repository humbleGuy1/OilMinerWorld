using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Zenject;
using static Assets.Scripts.CellData;
using TMPro;
using ModestTree;

namespace Assets.Scripts
{
    [RequireComponent(typeof(AntCreator))]
    public class Anthill : MonoBehaviour, IInitializable
    {
        [SerializeField] private Transform _pathRoot;
        [SerializeField] private int _levelNumber;
        [SerializeField] private SpiderSpawnerHandler _spiderSpawnerHandler;
        [SerializeField] private Tutorial _tutorial;

        private Region[] _regions;
        private AntHouseList _antHouseList;
        private CellList _cellList;
        private AntCreator _antCreator;
        private EndGame _endGame;

        public IReadOnlyList<Cell> DefaultCells => _cellList.DefaultCells;
        public IReadOnlyList<Cell> AllCells => _cellList.AllCells;
        public IReadOnlyList<AntHouse> AntHouses => _antHouseList.AllHouses;
        public int LevelNumber => _levelNumber;

        [Inject]
        private void Construct(LeafWalletPresenter leafWallet, StoneWalletPresenter stoneWallet, EndGame endGame,
            CellList cellList, AntHouseList antHouseList, Region[] regions)
        {
            _cellList = cellList;
            _antHouseList = antHouseList;
            _regions = regions;
            _endGame = endGame;

            _antCreator = GetComponent<AntCreator>();

            InitializeDiggerHouses(stoneWallet);
            InitializeLoaderHouses(leafWallet);

            foreach (var cell in _cellList.AllCells)
            {
                cell.BecameDiggable += OnCellBecameDiggable;
                cell.Digged += OnCellDigged;
            }

            _endGame.AnthillDone += OnGameEnd;
        }

        private void OnDestroy()
        {
            foreach (var cell in _cellList.AllCells)
            {
                cell.BecameDiggable -= OnCellBecameDiggable;
                cell.Digged -= OnCellDigged;
            }

            _endGame.AnthillDone -= OnGameEnd;
        }

        private void Start()
        {
            foreach (var region in _regions)
            {
                region.Initialize();
            }

            _spiderSpawnerHandler.Init(_regions.ToList());

            Cell cellQueen = _cellList.AllCells.FirstOrDefault(queen => queen.Queen.gameObject.activeSelf);
            _antCreator.Init(this, cellQueen);

            _tutorial.TryStartTutorial();
        }
    

        public void Initialize()
        {
            //foreach (var region in _regions)
            //    region.Initialize();

            //Cell cellQueen = _cellList.AllCells.FirstOrDefault(queen => queen.Queen.gameObject.activeSelf);
            //_antCreator.Init(this, cellQueen);
        }

        private void InitializeLoaderHouses(LeafWalletPresenter leafWallet)
        {
            foreach (var loaderHouse in _antHouseList.LoaderHouses)
            {
                loaderHouse.Initialize(_pathRoot, leafWallet, _antCreator);
            }
        }

        private void InitializeDiggerHouses(StoneWalletPresenter stoneWallet)
        {
            foreach (var diggerHouse in _antHouseList.DiggersHouses)
                diggerHouse.Initialize(_pathRoot, stoneWallet, _antCreator);
        }

        private void OnGameEnd()
        {
            Cell[] filtered = _cellList.DefaultCells.Where(cell => cell.CellState == CellState.Unlocked && cell.CellState == CellState.Locked).ToArray();
            for (int i = 0; i < filtered.Length; i++)
                filtered[i].Block();
        }

        private void OnCellDigged(Cell cell)
        {
            foreach (var diggerHouse in _antHouseList.DiggersHouses)
                diggerHouse.RemoveTarget(cell);
        }

        private void OnCellBecameDiggable(Cell cell)
        {

            var diggerHouses = cell.Region.DiggerHouses.Where(house => house.Cell.CellState == CellData.CellState.Opened)
                .OrderBy(house => Vector3.Distance(house.transform.position, cell.transform.position)).ToList();
            int targetDiggers = cell.SlicedHex.PartsCount - cell.DiggersCount;
            int currentDiggers = 0;

            for (int i = 0; i < diggerHouses.Count; i++)
            {
                currentDiggers += diggerHouses[i].DiggersForWork;
                diggerHouses[i].AddTarget(cell);

                if (currentDiggers >= targetDiggers)
                    break;
            }
        }
    }
}
