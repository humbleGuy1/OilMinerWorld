using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using PathCreation;
using UnityEngine;
using Zenject;

public abstract class AntHouse : GUIDObject, IBlockable
{
    [SerializeField] private Ant _ant;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Cell _cell;
    [SerializeField] private AntsRecover _antsRecover;
    [SerializeField] private AntHouseUpgradeView _antHouseUpgradeView;

    [SerializeField] protected int DefaultAntCount;

    private List<Ant> _ants = new List<Ant>();
    private WalletPresenter _wallet;
    private Transform _pathRoot;
    private Shop _shop;
    private AntCreator _antCreator;
    private int _1LevelUpPrice = 300;
    private int _2LevelUpPrice = 600;
    private int _3LevelUpPrice = 900;

    protected IReadOnlyList<Ant> Ants => _ants;
    protected float DefaultPositionY { get; private set; } = 0.18f;

    [field: SerializeField] public AddingResourceAnimation AddingResourceAnimation { get; private set; }
    [field: SerializeField] public float ResourcePerSec { get; protected set; }
    [field:SerializeField] public HouseSprites houseSprites { get; private set; }
    public Blockable Blockable { get; private set; } = new Blockable();
    public Cell Cell => _cell;
    public int Level { get; private set; } = 1;
    public int NextLevelPrice { get; private set; }
    public int CurrentAntsCount { get; private set; }
    public int AvailableAntsCount => GetAvailableCount();
    public int MaxLevel { get; } = 4;
    public bool IsMaxLevel => Level >= MaxLevel;
    public int AntsPerLevel => DefaultAntCount;

    public event Action LevelIncreased;
    public event Action<int> Upgraded;
    public event Action<Ant> AntRemoved;
    public event Action<Transform> LevelIncreasedCellPosition;

    [Inject]
    private void Construct(Shop shop)
    {
        _shop = shop;
    }

    public void Initialize(Transform pathRoot, WalletPresenter wallet, AntCreator antCreator)
    {
        LoadLevel();
        _antCreator = antCreator;
        Blockable.SetCell(_cell);
        _wallet = wallet;
        _pathRoot = pathRoot;
        _antHouseUpgradeView.UpgradeModel(Level);

        StartCoroutine(Working());
    }

    public void SetPrice(int multiply)
    {
        _1LevelUpPrice *= multiply;
        _2LevelUpPrice *= multiply;
        _3LevelUpPrice *= multiply;

        var save = new AntHouseSaveData(GUID);

        if (save.HasSave)
        {
            SaveLevel();
            SwitchCurrentPrice();
        }
        else
        {
            NextLevelPrice = _1LevelUpPrice;
            LoadLevel();
        }
    }

    public void IncreaseLevel()
    {
        SoundHandler.Instance.PlayUpgradeSound();
        Level++;
        SwitchCurrentPrice();

        var save = new AntHouseSaveData(GUID);
        save.Load();
        save.Level = Level;
        save.Save();

        _antHouseUpgradeView.UpgradeModel(Level);
        Upgraded?.Invoke(Level);
        LevelIncreased?.Invoke();
        LevelIncreasedCellPosition?.Invoke(_cell.transform);
    }

    public bool IsValidate(WalletPresenter stoneWalletPresenter)
    {
        if (NextLevelPrice <= stoneWalletPresenter.Value && Level < MaxLevel)
            return true;

        return false;
    }

    public bool CanBuy(WalletPresenter stoneWalletPresenter)
    {
        if (NextLevelPrice <= stoneWalletPresenter.Value && gameObject.activeSelf && Level < MaxLevel)
            return true;

        return false;
    }

    public void AddAnt(Ant ant)
    {
        CurrentAntsCount++;
        _ants.Add(ant);
        int antIndex = _ants.Count - 1;
        ant.Initialize(Cell, _wallet, this, _shop, antIndex);
        StartCoroutine(SendAntToHome(ant));
    }

    public void RemoveAnt(Ant ant)
    {
        CurrentAntsCount--;

        if (CurrentAntsCount < 0)
            CurrentAntsCount = 0;
        _ants.RemoveAt(ant.Index);
        _antCreator.Create(_ant.Type, this, _cell);
    }

    public bool TryCreateAnts()
    {
        if (CurrentAntsCount >= AvailableAntsCount && gameObject.activeSelf == false)
            return false;

        if(Cell.CellState == CellData.CellState.Opened)
        {
            for (int i = CurrentAntsCount; i < AvailableAntsCount; i++)
            {
                Ant ant = Instantiate(_ant, transform.position + Vector3.up * 0.15f, Quaternion.identity);
                AddAnt(ant);
            }
        }

        return Cell.CellState == CellData.CellState.Opened;
    }

    protected abstract IEnumerator Working();

    protected IEnumerator SendAntToHome(Ant ant)
    {
        Cell cellUnderAnt = GetCellUnder(ant.transform);

        if (cellUnderAnt != Cell)
        {
            var pathFinder = new PathFinder(this);
            pathFinder.Find(cellUnderAnt, Cell);

            yield return new WaitUntil(() => pathFinder.PathFinded);

            var pathToHome = new List<Cell>();
            pathToHome.AddRange(pathFinder.Path);
            pathToHome.RemoveAt(pathToHome.Count - 1);

            Cell cellBeforeHome = pathToHome[pathToHome.Count - 1];
            Vector3 cellSide = Cell.GetNearestSidePosition(cellBeforeHome.transform.position);

            var pointFullPath = new List<Vector3>();
            pointFullPath.Add(ant.transform.position);
            pointFullPath.AddRange(ConvertToPositions(pathToHome, DefaultPositionY));
            pointFullPath.Add(new Vector3(cellSide.x, DefaultPositionY, cellSide.z));

            ant.StartMove(CreateVertexPath(new BezierPath(pointFullPath, false, PathSpace.xyz)));
        }
        else
        {
            ant.AddToHome();
        }
    }

    protected List<Vector3> ConvertToPositions(List<Cell> cells, float defaultPositionY, float defaultRandomOffset = 0.2f)
    {
        var positions = new List<Vector3>();

        foreach (Cell cell in cells)
        {
            float randomOffset = defaultRandomOffset;

            if (cell == cells[cells.Count - 1])
                randomOffset = 0f;

            positions.Add(new Vector3(cell.transform.position.x + UnityEngine.Random.Range(-randomOffset, randomOffset), defaultPositionY,
                cell.transform.position.z + UnityEngine.Random.Range(-randomOffset, randomOffset)));
        }

        return positions;
    }

    protected Cell GetCellUnder(Transform transform)
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 10f, _layerMask.value))
            if (hit.collider.TryGetComponent(out Cell cell))
                return cell;

        return _cell;
        //throw new System.InvalidOperationException();
    }

    protected VertexPath CreateVertexPath(BezierPath bezierPath)
    {
        return new VertexPath(bezierPath, _pathRoot);
    }

    private int GetAvailableCount()
    {
        //if (Level == 2)
        //    return _defaultAntCount + 5;
        //else if (Level == 3)
        //    return _defaultAntCount + 10;
        //else if (Level == 4)
        //    return _defaultAntCount + 15;

        //return _defaultAntCount;

        return DefaultAntCount * Level;
    }

    private void SwitchCurrentPrice()
    {
        switch (Level)
        {
            case 1:
                NextLevelPrice = _1LevelUpPrice;
                break;
            case 2:
                NextLevelPrice = _2LevelUpPrice;
                break;
            case 3:
                NextLevelPrice = _3LevelUpPrice;
                break;
        }
    }

    private void SaveLevel()
    {
        var save = new AntHouseSaveData(GUID);
        save.Load();
        save.Level = Level;
        save.Save();
    }

    private void LoadLevel()
    {
        var save = new AntHouseSaveData(GUID);

        if (save.HasSave == false)
        {
            Level = 1;
        }
        else
        {
            save.Load();
            Level = save.Level;
        }
    }
}
