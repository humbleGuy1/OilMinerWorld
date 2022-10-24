using UnityEngine;
using System.Linq;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public class LoaderHouse : AntHouse
    {
        private List<Cell> _targets = new List<Cell>();

        public void AddTarget(Cell cell)
        {
            if(_targets.Contains(cell) == false)
                _targets.Add(cell);
        }

        public void SetTargets(IEnumerable targets1, IEnumerable targets2 = null)
        {
            foreach (Cell item in targets1)
            {
                if(item.Region == Cell.Region)
                    _targets.Add(item);
            }

            if(targets2 != null)
            {
                foreach (Cell item in targets2)
                {
                    if (item.Region == Cell.Region)
                        _targets.Add(item);
                }

            }
        }

        protected override IEnumerator Working()
        {
            while (true)
            {
                yield return new WaitUntil(()=> Blockable.IsBlocked == false);

                yield return new WaitUntil(() => Ants.Any(loader => loader.CurrentState == AntState.WaitingWork)
                    && _targets.Any(target => target.CellState == CellState.Opened || target.CellState == CellState.DeadEnemy)
                    && _targets.Any(target => target.Food.PartsCount > target.LoadersCount));

                Cell nearTarget = ChooseTarget();

                if (nearTarget == null)
                    continue;

                int targetLoaderCount = nearTarget.Food.PartsCount - nearTarget.LoadersCount;
                IEnumerable<Ant> allLoadersForWork = Ants.Where(loader => loader.CurrentState == AntState.WaitingWork);
                var loadersForWork = new List<Ant>();

                foreach (Ant loader in allLoadersForWork)
                {
                    if (targetLoaderCount == loadersForWork.Count)
                        break;

                    loadersForWork.Add(loader);
                    loader.SetTarget(nearTarget);
                    nearTarget.CellAntsHolder.AddLoader((Loader)loader);
                }

                if (loadersForWork.Count > 0)
                    StartCoroutine(SendAntsToWork(loadersForWork, nearTarget));

                Cell ChooseTarget()
                {
                    var nearTargets = _targets
                        .Where(target => (target.CellState == CellState.Opened && target.Food.gameObject.activeSelf) || target.CellState == CellState.DeadEnemy)
                        .OrderBy(target => Vector3.Distance(transform.position, target.transform.position)).ToList();

                    Cell[] twoNearCandidates = new Cell[2];
                    
                    for (int i = 0; i < twoNearCandidates.Length; i++)
                    {
                        twoNearCandidates[i] = nearTargets.FirstOrDefault(target => target.Food.PartsCount > target.LoadersCount);
                        nearTargets.Remove(twoNearCandidates[i]);
                    }

                    if (twoNearCandidates[0] != null && twoNearCandidates[1] != null)
                    {
                        if ((transform.position - twoNearCandidates[1].transform.position).magnitude < 7 && twoNearCandidates[1].Food.PartsCount < twoNearCandidates[0].Food.PartsCount)
                            return twoNearCandidates[1];
                    }

                    

                    return twoNearCandidates[0];
                }
            }
        }

        private IEnumerator SendAntsToWork(List<Ant> loaders, Cell target)
        {
            yield return new WaitUntil(() => Blockable.IsBlocked == false);
            var pathFinder = new PathFinder(this);

            pathFinder.Find(Cell, target);

            yield return new WaitUntil(() => pathFinder.PathFinded);

            var pathFromHomeToTarget = new List<Cell>();
            pathFromHomeToTarget.AddRange(pathFinder.Path);
            pathFromHomeToTarget.RemoveAt(pathFromHomeToTarget.Count - 1);

            var pathFromTargetToHome = new List<Cell>();
            pathFromTargetToHome.AddRange(pathFromHomeToTarget);
            pathFromTargetToHome.Reverse();
            pathFromTargetToHome.RemoveAll(cell => cell == Cell);

            Cell cellBeforeHome = target;

            if (pathFromTargetToHome.Count > 0)
                cellBeforeHome = pathFromTargetToHome[pathFromTargetToHome.Count - 1];

            Vector3 homeCellSide = Cell.GetNearestSidePosition(cellBeforeHome.transform.position);
            homeCellSide = new Vector3(homeCellSide.x, DefaultPositionY, homeCellSide.z);

            for (int i = 0; i < loaders.Count; i++)
            {
                var loader = loaders[i];
                pathFinder.Find(GetCellUnder(loader.transform), Cell);

                yield return new WaitUntil(() => pathFinder.PathFinded);

                var pathFromAntToHome = new List<Cell>();
                pathFromAntToHome.AddRange(pathFinder.Path);
                pathFromAntToHome.RemoveAt(0);

                var pointFullPath = new List<Vector3>();
                pointFullPath.Add(loader.transform.position);
                pointFullPath.AddRange(ConvertToPositions(pathFromAntToHome, DefaultPositionY));
                pointFullPath.AddRange(ConvertToPositions(pathFromHomeToTarget, DefaultPositionY));

                Vector3 targetPositionWithDefaultY = new Vector3(target.transform.position.x, DefaultPositionY, target.transform.position.z);
                Vector3 direction = (targetPositionWithDefaultY - pointFullPath[pointFullPath.Count - 1]).normalized;
                float minDistanceToNextPoint = 0.3f;
                float maxDistanceToNextPoint = 0.5f;
                float distanceToNextPoint = (target.Food.MaxPartsCount - target.Food.RenderingPartsCount) / (float)target.SlicedHex.MaxPartsCount + minDistanceToNextPoint;
                distanceToNextPoint = Mathf.Clamp(distanceToNextPoint, minDistanceToNextPoint, maxDistanceToNextPoint);
                pointFullPath.Add(direction * distanceToNextPoint + pointFullPath[pointFullPath.Count - 1]);

                Resource part = target.Food.FindNearestPart(pointFullPath[pointFullPath.Count - 1]);

                if (part == null)
                    yield break;

                loader.SetTargetPiece(part);
                pointFullPath.Add(new Vector3(part.Root.position.x, DefaultPositionY, part.Root.position.z));
                pointFullPath.AddRange(ConvertToPositions(pathFromTargetToHome, DefaultPositionY));
                pointFullPath.Add(homeCellSide);

                loader.StartMove(CreateVertexPath(new BezierPath(pointFullPath, false, PathSpace.xyz)));
            }
        }
    }
}
