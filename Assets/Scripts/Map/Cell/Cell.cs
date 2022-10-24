using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using static Assets.Scripts.CellData;
using DG.Tweening;

namespace Assets.Scripts
{
    public class Cell : GUIDObject
    {
        [SerializeField] private Food _food;
        [SerializeField] private Food _enemyFood;
        [SerializeField] private CellFX _cellFX;
        [SerializeField] private CellObject _object;
        [SerializeField] private CellType _cellType;
        [SerializeField] private CellDifficult _cellDifficult;
        [SerializeField] private RockActivator _rockActivator;
        [SerializeField] private bool _test;

        private Coroutine _waitDigging;
        private bool _isActivated;

        private CellState _initialState;
        private const float Delay = 0.05f;
        private const int UnlockRadius = 1;

        [HideInInspector] public bool IsBlocked;
        [field: SerializeField] public CellState CellState { get; private set; }
        [field: SerializeField] public Hex Hex { get; private set; }
        [field: SerializeField] public Queen Queen { get; private set; }
        [field: SerializeField] public CellView CellView { get; private set; }
        [field: SerializeField] public LoaderHouse LoaderHouse { get; private set; }
        [field: SerializeField] public DiggersHouse DiggersHouse { get; private set; }
        [field: SerializeField] public NeighborCells NeighborCells { get; private set; }
        [field: SerializeField] public CellPriceView CellPriceView { get; private set; }
        [field: SerializeField] public CellAntsHolder CellAntsHolder { get; private set; }
        [field: SerializeField] public Food Food { get; private set; }
        [field: SerializeField] public bool OpenOnStart { get; private set; }
        public Region Region { get; private set; }
        public CellType CellType => _cellType;
        public SlicedHex SlicedHex { get; private set; }
        public int Price { get; private set; }
        public int DiggingDifficult => (int)_cellDifficult;
        public int DiggersCount => CellAntsHolder.DiggersCount;
        public int LoadersCount => CellAntsHolder.LoadersCount;
        public bool IsDigging => CellState == CellState.Digging;
        public bool CanStartDig => CellState == CellState.Unlocked;
        public bool IsActivated => _isActivated;

        public event Action Opened;
        public event Action Unlocked;
        public event Action<Cell> Digging;
        public event Action<Cell> Digged;
        public event Action<Cell> BecameDiggable;
        public event Action<Cell> FirstTimeDigged;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _object.Setup(_cellType);
            Food = CellType == CellType.Enemy ? _enemyFood : _food;

            if(CellType == CellType.Default)
                gameObject.name = $"Cell";
            else
                gameObject.name = $"Cell_{CellType}";

            //CellView.RenderBlock();

            //ActivateRocks();
        }

        public void ActivateRocks()
        {
            _rockActivator.Activate(_cellDifficult);
        }
#endif
        private void OnApplicationFocus(bool focus)
        {
            var cellSave = new CellSave(GUID);
            cellSave.Load();
            cellSave.CellState = CellState;
            cellSave.Save();
        }

        public void Initialize(int price, float pieceMulty, int upgradeMultiply, Region region)
        {
            Region = region;
            NeighborCells.Init(Region.Index);
            CellView.Init((int)_cellDifficult);
            var save = new CellSave(GUID);

            if (save.HasSave || OpenOnStart)
            {
                save.Load();
                CellState = OpenOnStart ? CellState.Opened : save.CellState;
                save.CellState = CellState;
                save.Save();
            }

            Price = price;
            LoaderHouse.SetPrice(upgradeMultiply);
            DiggersHouse.SetPrice(upgradeMultiply);
            CellView.RenderLock();
            SlicedHex = CellView.GetHex();
            SlicedHex.SetMultiplyPrice(pieceMulty, _cellDifficult);
            //Food.SetMultiplyPrice(pieceMulty);
            //Food.Test(pieceMulty, Region.Index);
            _object.Setup(_cellType);
            _initialState = CellState;
        }

        public void SetEnemyFood(Food food)
        {
            Food = food;
        }

        public void SetCellType(CellData.CellType cellType)
        {
            _cellType = cellType;
        }

        public void ResetState()
        {
            CellState = _initialState;
        }

        public void SetupState()
        {
            switch (CellState)
            {
                case CellState.Opened:
                    Open();
                    break;
                case CellState.Unlocked:
                    Unlock();
                    break;
                case CellState.Blocked:
                    Block();
                    break;
                case CellState.Digging:
                    Unlock();
                    Invoke(nameof(StartDigging), Delay);
                    break;
                case CellState.DeadEnemy:
                case CellState.EatenEnemy:
                    _cellType = CellType.Default;
                    Open();
                    break;
            }
        }

        public Vector3 GetNearestSidePosition(Vector3 position)
        {
            var sides = Hex.LocalCenterSidesPositions.OrderBy(side => Vector3.Distance(side + Hex.transform.position, position)).ToList();
            return sides[0] + Hex.transform.position;
        }

        public void Unlock(int radius = 0)
        {
            CellState = CellState.Unlocked;
            CellView.RenderUnlock();
            _cellFX.OnCellUnlocked();
            NeighborCells.UnlockNeighborCells(radius - 1, Region.Index);
            Unlocked?.Invoke();
        }

        public void StartDigging()
        {
            ChangeState(CellState.Digging);
            Digging?.Invoke(this);
            CellView.RenderDigging();
            _waitDigging = StartCoroutine(WaitDigging());
        }

        public void ChangeState(CellState state)
        {
            CellState = state;

            var cellSave = new CellSave(GUID);
            cellSave.Load();
            cellSave.CellState = CellState;
            cellSave.Save();
        }

        public void Block()
        {
            ChangeState(CellState.Blocked);
            CellView.RenderBlock();
        }

        public void Unblock()
        {
            if (CellState == CellState.Blocked || CellState == CellState.Locked)
            {
                ChangeState(CellState.Locked);
                CellView.RenderUnblocked();
                _cellFX.OnCellUnlocked();

                foreach (Cell neighbor in NeighborCells.GetNeighborCells())
                {
                    if (neighbor.CellState == CellState.Opened && neighbor.CellType == CellType.Default)
                    {
                        Unlock();
                        CellView.RenderLockWithTopHex();
                    }
                }
            }

            _initialState = CellState;
        }

        public void Open()
        {
            bool alreadyOpened = CellState == CellState.Opened;

            if (CellState != CellState.DeadEnemy && CellState != CellState.EatenEnemy)
                ChangeState(CellState.Opened);

            CellView.RenderUnlock();
            CellView.RenderOpen();

            if (SlicedHex != null)
                SlicedHex.DestroyAllPart();

            NeighborCells.UnlockNeighborCells(UnlockRadius, Region.Index);

            if (alreadyOpened == false)
                FirstTimeDigged?.Invoke(this);

            _cellFX.OnCellOpened();
            Opened?.Invoke();
            Digged?.Invoke(this);

            _initialState = CellState;
        }

        public void Activate()
        {
            _isActivated = true;
        }

        private IEnumerator WaitDigging()
        {
            BecameDiggable?.Invoke(this);
            transform.DOScale(1.13f, 0.13f).OnComplete(ScaleBack);

            if (SlicedHex.IsInfinite)
                SlicedHex.AcitvateInfinite();

            yield return new WaitUntil(() => SlicedHex.HasParts == false);
            Open();
        }

        private void ScaleBack()
        {
            transform.DOScale(1f, 0.13f);
        }
    }
}
