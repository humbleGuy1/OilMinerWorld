#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class TopHexEditor : MonoBehaviour
    {
        private List<CellTopHex> _cells = new List<CellTopHex>();

        [ContextMenu(nameof(HideTopHex))]
        private void HideTopHex()
        {
            _cells.Clear();
            _cells.AddRange(FindObjectsOfType<CellTopHex>());

            foreach (CellTopHex cell in _cells)
                cell.gameObject.SetActive(false);
        }

        [ContextMenu(nameof(ShowTopHex))]
        private void ShowTopHex()
        {
            _cells.AddRange(FindObjectsOfType<CellTopHex>());

            foreach (CellTopHex cell in _cells)
                cell.gameObject.SetActive(true);

            _cells.Clear();
        }
    }
}
#endif
