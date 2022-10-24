using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class CellsInitializer : MonoBehaviour
    {
#if UNITY_EDITOR
        [ContextMenu(nameof(InitializeCellViewCanvas))]
        private void InitializeCellViewCanvas()
        {
            CellViewCanvasInitializer[] cellViewInitializer = FindObjectsOfType<CellViewCanvasInitializer>();

            foreach (CellViewCanvasInitializer initalizer in cellViewInitializer)
                initalizer.Initialize();
        }

        [ContextMenu("Generate GUID")]
        private void GenerateGUID()
        {
            IReadOnlyCollection<Cell> cells = FindObjectsOfType<Cell>(false);

            foreach (Cell cell in cells)
            {
                cell.LoaderHouse.RegenerateGUID();
                cell.DiggersHouse.RegenerateGUID();
            }
        }
#endif
    }
}
