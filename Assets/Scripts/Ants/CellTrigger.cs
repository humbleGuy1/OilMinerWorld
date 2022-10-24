using UnityEngine;

namespace Assets.Scripts
{
    public class CellTrigger : MonoBehaviour
    {
        public Cell LastTriggeredCell { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Cell cell))
                LastTriggeredCell = cell;
        }
    }
}
