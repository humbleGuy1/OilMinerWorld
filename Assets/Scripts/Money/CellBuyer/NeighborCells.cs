using UnityEngine;
using System.Collections.Generic;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public class NeighborCells : MonoBehaviour
    {
        private List<Cell> _neighborCells;
        private int _regionIndex;

        public void Init(int regionIndex)
        {
            _regionIndex = regionIndex;
        }

        public IReadOnlyList<Cell> GetNeighborCells()
        {
            if (_neighborCells != null)
                return _neighborCells;

            _neighborCells = new List<Cell>();
            Collider[] neighborColliders = Physics.OverlapBox(transform.position, Vector3.one * 0.5f, Quaternion.identity);

            foreach (Collider collider in neighborColliders)
                if (collider.TryGetComponent(out Cell cell) && cell != this && cell && cell.Region.Index == _regionIndex)
                    _neighborCells.Add(cell);

            return _neighborCells;
        }

        public void UnlockNeighborCells(int unlockRadius, int regionIndexFrom)
        {
            if (unlockRadius <= 0)
                return;

            foreach (Cell cell in GetNeighborCells())
            {
                if (cell.CellState == CellState.Locked || cell.CellState == CellState.Unlocked)
                {
                    if (CheckNeighborhoodOfNeigbors(cell))
                    {
                        if(cell.Region.Index == regionIndexFrom)
                            cell.Unlock(unlockRadius);
                        continue;
                    }

                    cell.CellView.RenderLockWithTopHex();
                }
            }

            bool CheckNeighborhoodOfNeigbors(Cell cell)
            {
                foreach (Cell neighbor in cell.NeighborCells.GetNeighborCells())
                    if ((neighbor.CellType == CellType.Default || neighbor.CellType == CellType.Enemy)
                      && (neighbor.CellState == CellState.Opened || neighbor.CellState == CellState.DeadEnemy || neighbor.CellState == CellState.EatenEnemy))
                        return true;

                return false;
            }
        }
    }
}
