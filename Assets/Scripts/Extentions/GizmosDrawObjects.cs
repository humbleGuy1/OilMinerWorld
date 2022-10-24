#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public class GizmosDrawObjects : MonoBehaviour
    {
        private List<Cell> _cells = new List<Cell>();

        private void OnValidate()
        {
            _cells.Clear();
            _cells.AddRange(FindObjectsOfType<Cell>());
        }

        private void OnDrawGizmos()
        {
            if (_cells.Count <= 0)
                return;

            for (int i = 0; i < _cells.Count; i++)
            {
                Cell cell = _cells[i];

                if (cell.OpenOnStart)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(cell.transform.position + Vector3.up, 0.2f);
                }

                switch (cell.CellType)
                {
                    case CellType.Default:
                        Gizmos.color = Color.gray;
                        Gizmos.DrawCube(cell.transform.position + Vector3.up, Vector3.one / 10);
                        break;
                    case CellType.Food:
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawSphere(cell.transform.position + Vector3.up, 0.15f);
                        break;
                    case CellType.Queen:
                        Gizmos.color = Color.white;
                        Gizmos.DrawSphere(cell.transform.position + Vector3.up, 0.1f);
                        break;
                    case CellType.DiggersHouse:
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(cell.transform.position + Vector3.up, 0.1f);
                        break;
                    case CellType.LoaderHouse:
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawSphere(cell.transform.position + Vector3.up, 0.1f);
                        break;
                    case CellType.Enemy:
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(cell.transform.position + Vector3.up, 0.13f);
                        break;
                }

                if (cell.DiggingDifficult == 2)
                    Gizmos.color = Color.green;
                else if (cell.DiggingDifficult == 1)
                    Gizmos.color = Color.black;
                else if (cell.DiggingDifficult == 0)
                    Gizmos.color = Color.white;

                Gizmos.DrawCube(cell.transform.position + Vector3.up, Vector3.one / 8);
            }
        }
    }
}
#endif
