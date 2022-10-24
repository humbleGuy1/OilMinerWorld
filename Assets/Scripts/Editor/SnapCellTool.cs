using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace Assets.Scripts
{
    [EditorTool("Cell Snap Move", typeof(Cell))]
    public class SnapCellTool : EditorTool
    {
        private List<Transform> _oldSelectedTransforms = new List<Transform>();
        private Hex[] _allHexInScene;

        public override void OnToolGUI(EditorWindow window)
        {
            if (Application.isPlaying)
                return;

            var selectedTransforms = new List<Transform>();
            targets?.ToList().ForEach(cell => selectedTransforms.Add(((Cell)cell).transform));

            var hookedCell = (Selection.activeTransform?.GetComponent<Cell>() != null ?
                Selection.activeTransform : selectedTransforms[selectedTransforms.Count - 1]).GetComponent<Cell>();

            if (selectedTransforms.SequenceEqual(_oldSelectedTransforms) == false)
            {
                PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(hookedCell.gameObject);
                _allHexInScene = prefabStage != null ? prefabStage.prefabContentsRoot.GetComponentsInChildren<Hex>() : FindObjectsOfType<Hex>();

                _oldSelectedTransforms = selectedTransforms;
            }

            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(hookedCell.transform.position, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(selectedTransforms.ToArray(), "Move with custom snap tool");

                var movedPosition = newPosition - hookedCell.transform.position;
                MoveCells(selectedTransforms, movedPosition);
                MoveWithSnapping(selectedTransforms, hookedCell, newPosition);
            }
        }

        private void MoveWithSnapping(List<Transform> selectedTransforms, Cell hookedCell, Vector3 newPosition)
        {
            Vector3 bestPosition = newPosition;
            float closestDistance = float.PositiveInfinity;

            foreach (var hex in _allHexInScene)
            {
                if (ContainsIn(selectedTransforms, child: hex.transform))
                    continue;

                foreach (var localCenterSidePosition in hex.LocalCenterSidesPositions)
                {
                    foreach (var hookedLocalCenterSidePosition in hookedCell.Hex.LocalCenterSidesPositions)
                    {
                        Vector3 targetPosition = hex.transform.position + localCenterSidePosition -
                            (hookedCell.Hex.transform.position + hookedLocalCenterSidePosition - hookedCell.transform.position);
                        float distance = Vector3.Distance(targetPosition, newPosition);

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            bestPosition = targetPosition;
                        }
                    }
                }
            }

            var movedPosition = (closestDistance < 0.5f ? bestPosition : newPosition) - hookedCell.transform.position;
            MoveCells(selectedTransforms, movedPosition);
        }

        private void MoveCells(List<Transform> cells, Vector3 movedPosition)
        {
            cells.ForEach(cell => cell.transform.position = cell.transform.position + movedPosition);
        }

        private bool ContainsIn(List<Transform> parents, Transform child)
        {
            foreach (var parent in parents)
                if (child.parent.transform == parent)
                    return true;

            return false;
        }
    }
}
