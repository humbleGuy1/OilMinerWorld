using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace Assets.Scripts
{
    [EditorTool("Region Snap Move", typeof(Region))]
    public class SnapRegionTool : EditorTool
    {
        private List<Transform> _oldSelectedTransforms = new List<Transform>();
        private Hex[] _allHexInScene;

        public override void OnToolGUI(EditorWindow window)
        {
            if (Application.isPlaying)
                return;

            var selectedTransforms = new List<Transform>();
            targets?.ToList().ForEach(region => selectedTransforms.Add(((Region)region).transform));

            var hookedRegion = (Selection.activeTransform?.GetComponent<Region>() != null ?
                Selection.activeTransform : selectedTransforms[selectedTransforms.Count - 1]).GetComponent<Region>();

            if (selectedTransforms.SequenceEqual(_oldSelectedTransforms) == false)
            {
                PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(hookedRegion.gameObject);
                _allHexInScene = prefabStage != null ? prefabStage.prefabContentsRoot.GetComponentsInChildren<Hex>() : FindObjectsOfType<Hex>();

                _oldSelectedTransforms = selectedTransforms;
            }

            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(hookedRegion.transform.position, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTransforms.ToArray(), "Move with custom snap tool");

                var movedPosition = newPosition - hookedRegion.transform.position;
                MoveRegions(selectedTransforms, movedPosition);
                MoveWithSnapping(selectedTransforms, hookedRegion, newPosition);
            }
        }

        private void MoveWithSnapping(List<Transform> selectedTransforms, Region hookedRegion, Vector3 newPosition)
        {
            Transform bestTransform = hookedRegion.transform;
            Vector3 bestPosition = newPosition;
            float closestDistance = float.PositiveInfinity;

            foreach (var hex in _allHexInScene)
            {
                if (ContainsIn(selectedTransforms, child: hex.transform))
                    continue;

                foreach (var localCenterSidePosition in hex.LocalCenterSidesPositions)
                {
                    foreach (var cell in hookedRegion.Cells)
                    {
                        foreach (var hookedLocalCenterSidePosition in cell.Hex.LocalCenterSidesPositions)
                        {
                            Vector3 targetPosition = hex.transform.position + localCenterSidePosition -
                                (cell.Hex.transform.position + hookedLocalCenterSidePosition - cell.transform.position);
                            float distance = Vector3.Distance(targetPosition, cell.transform.position);

                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                bestPosition = targetPosition;
                                bestTransform = cell.transform;
                            }
                        }
                    }
                }
            }

            var movedPosition = closestDistance < 0.5f ? bestPosition - bestTransform.position
                                                       : newPosition - hookedRegion.transform.position;

            MoveRegions(selectedTransforms, movedPosition);
        }

        private void MoveRegions(List<Transform> regions, Vector3 movedPosition)
        {
            regions.ForEach(region => region.transform.position = region.transform.position + movedPosition);
        }

        private bool ContainsIn(List<Transform> parents, Transform child)
        {
            foreach (Transform region in parents)
                foreach (Transform cell in region)
                    if (child.parent.transform == cell)
                        return true;

            return false;
        }
    }
}
