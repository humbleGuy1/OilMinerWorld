using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PathCreation;

namespace Assets.Scripts
{
    public class DiggersHouse : AntHouse
    {
        private List<Cell> _targets = new List<Cell>();

        public int DiggersForWork => Ants.Count(digger => digger.CurrentState == AntState.WaitingWork);

        public void AddTarget(Cell cell)
        {
            if (_targets.Contains(cell))
                throw new System.InvalidOperationException();

            _targets.Add(cell);
        }

        public void RemoveTarget(Cell cell)
        {
            if (_targets.Contains(cell) == false)
                return;

            _targets.Remove(cell);
        }

        protected override IEnumerator Working()
        {
            while (true)
            {
                yield return new WaitUntil(() => Blockable.IsBlocked == false);

                yield return new WaitUntil(() => Ants.Any(digger => digger.CurrentState == AntState.WaitingWork)
                    && _targets.Any(target => target.SlicedHex.PartsCount > target.DiggersCount));

                _targets = _targets.OrderBy(cell => cell.SlicedHex.IsInfinite).ToList();
                Cell target = _targets.FirstOrDefault(target => target.SlicedHex.PartsCount > target.DiggersCount);

                if (target == null)
                    continue;

                int targetDiggerCount = target.SlicedHex.PartsCount - target.DiggersCount;
                IEnumerable<Ant> allDiggersForWork = Ants.Where(digger => digger.CurrentState == AntState.WaitingWork);
                var diggersForWork = new List<Ant>();

                foreach (Ant digger in allDiggersForWork)
                {
                    if (targetDiggerCount == diggersForWork.Count)
                        break;

                    diggersForWork.Add(digger);
                    digger.SetTarget(target);
                    target.CellAntsHolder.AddDigger((Digger)digger);
                }

                if (target == null)
                    continue;

                if (diggersForWork.Count > 0)
                    StartCoroutine(SendAntsToWork(diggersForWork, target));
            }
        }


        //Оптимизировать :путь считаеся для каждого муравья отдельно, лучше один раз посчитать от норы до ресурса
        private IEnumerator SendAntsToWork(List<Ant> diggers, Cell target)
        {
            yield return new WaitUntil(() => Blockable.IsBlocked == false);

            var pathFinder = new PathFinder(this);
            var diggersByDistance = diggers.OrderBy(digger => Vector3.Distance(digger.transform.position, target.transform.position)).ToList();
            Cell commonCell = GetCellUnder(diggersByDistance[0].transform);

            pathFinder.Find(commonCell, target);

            yield return new WaitUntil(() => pathFinder.PathFinded);

            var pathFromCommonCellToTarget = new List<Cell>();
            pathFromCommonCellToTarget.AddRange(pathFinder.Path);
            pathFromCommonCellToTarget.RemoveAt(pathFromCommonCellToTarget.Count - 1);

            Cell cellBeforeTarget = pathFromCommonCellToTarget[pathFromCommonCellToTarget.Count - 1];

            pathFinder.Find(cellBeforeTarget, Cell);

            yield return new WaitUntil(() => pathFinder.PathFinded);

            var pathFromCommonCellToHome = new List<Cell>();
            pathFromCommonCellToHome.AddRange(pathFinder.Path);
            pathFromCommonCellToHome.RemoveAt(pathFromCommonCellToHome.Count - 1);

            Cell cellBeforeHome = target;

            if (pathFromCommonCellToHome.Count > 0)
                cellBeforeHome = pathFromCommonCellToHome[pathFromCommonCellToHome.Count - 1];


            Vector3 homeCellSide = Cell.GetNearestSidePosition(cellBeforeHome.transform.position);
            homeCellSide = new Vector3(homeCellSide.x, DefaultPositionY, homeCellSide.z);

            for (int i = diggersByDistance.Count - 1; i >= 0; i--)
            {
                Digger digger = (Digger)diggersByDistance[i];

                pathFinder.Find(GetCellUnder(digger.transform), commonCell);

                yield return new WaitUntil(() => pathFinder.PathFinded);

                var pathFromAntToCommonCell = new List<Cell>();
                pathFromAntToCommonCell.AddRange(pathFinder.Path);
                pathFromAntToCommonCell.RemoveAt(0);

                Vector3 sidePosition = target.GetNearestSidePosition(cellBeforeTarget.transform.position);
                sidePosition = new Vector3(sidePosition.x, DefaultPositionY, sidePosition.z);

                var pointFullPath = new List<Vector3>();
                pointFullPath.Add(digger.transform.position);
                pointFullPath.AddRange(ConvertToPositions(pathFromAntToCommonCell, DefaultPositionY));
                pointFullPath.AddRange(ConvertToPositions(pathFromCommonCellToTarget, DefaultPositionY));

                Vector3 targetPositionWithDefaultY = new Vector3(target.transform.position.x, DefaultPositionY, target.transform.position.z);
                Vector3 direction = (targetPositionWithDefaultY - pointFullPath[pointFullPath.Count - 1]).normalized;
                float minDistanceToNextPoint = 0.3f;
                float maxDistanceToNextPoint = 0.5f;
                float distanceToNextPoint = (target.SlicedHex.MaxPartsCount - target.SlicedHex.RenderingPartsCount) / (float)target.SlicedHex.MaxPartsCount + minDistanceToNextPoint;
                distanceToNextPoint = Mathf.Clamp(distanceToNextPoint, minDistanceToNextPoint, maxDistanceToNextPoint);
                pointFullPath.Add(direction * distanceToNextPoint + pointFullPath[pointFullPath.Count - 1]);

                Resource part = target.SlicedHex.FindNearestPart(pointFullPath[pointFullPath.Count - 1]);

                if (part == null)
                    yield break;


                digger.SetTargetPiece(part);
                //pointFullPath.Add(new Vector3(target.transform.position.x, DefaultPositionY, target.transform.position.z));
                pointFullPath.Add(new Vector3(part.Root.position.x, DefaultPositionY, part.Root.position.z));

                pointFullPath.Add(sidePosition);
                pointFullPath.AddRange(ConvertToPositions(pathFromCommonCellToHome, DefaultPositionY));
                pointFullPath.Add(homeCellSide);

                digger.StartMove(CreateVertexPath(new BezierPath(pointFullPath, false, PathSpace.xyz)));
            }
        }
    }
}
