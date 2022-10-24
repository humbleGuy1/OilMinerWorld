using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public class PathFinder
    {
        private List<Cell> _path = new List<Cell>();
        private MonoBehaviour _root;

        public IReadOnlyList<Cell> Path => _path;
        public bool PathFinded { get; private set; }

        public PathFinder(MonoBehaviour root)
        {
            _root = root;
        }

        public PathFinder Find(Cell startCell, Cell endCell)
        {
            PathFinded = false;

            _path.Clear();
            _path.Add(startCell);
            _path.Add(startCell);
            _root.StartCoroutine(FindPath(startCell, endCell));

            return this;
        }

        private IEnumerator FindPath(Cell startCell, Cell endCell)
        {
            Cell lastAvailableCell = startCell;
            List<Cell> deadEndCell = new List<Cell>();

            while (lastAvailableCell != endCell)
            {
                Vector3 directionToEndCell = (endCell.transform.position - lastAvailableCell.transform.position).normalized;

                IReadOnlyList<Cell> neighborCells = lastAvailableCell.NeighborCells.GetNeighborCells();
                neighborCells = neighborCells.Where(neighbor => _path.Contains(neighbor) == false && deadEndCell.Contains(neighbor) == false
                    && (((neighbor.CellState == CellState.Opened || neighbor.CellState == CellState.DeadEnemy || neighbor.CellState == CellState.EatenEnemy) && (neighbor.CellType == CellType.Default || neighbor.CellType == CellType.Enemy)) || neighbor == endCell)).ToList();
                neighborCells = neighborCells.OrderBy(neighbor => Vector3.Angle(neighbor.transform.position - lastAvailableCell.transform.position, directionToEndCell)).ToList();

                if (neighborCells.Count == 0)
                {
                    deadEndCell.Add(lastAvailableCell);
                    _path.RemoveAt(_path.Count - 1);
                    lastAvailableCell = _path[_path.Count - 1];
                    continue;
                }

                lastAvailableCell = neighborCells.Contains(endCell) ? neighborCells.First(cell => cell == endCell) : neighborCells[0];

                if (lastAvailableCell != null)
                    _path.Add(lastAvailableCell);

                yield return null;
            }

            PathFinded = true;
        }
    }
}
