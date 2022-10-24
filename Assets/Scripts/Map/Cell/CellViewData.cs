using UnityEngine;

namespace Assets.Scripts
{
    public class CellViewData : MonoBehaviour
    {
        public enum HexType
        {
            Sliced,
            Whole
        }

        public enum TextState
        {
            Disabled,
            OnlyPrice,
            OnlyCancel
        }

    }
}
