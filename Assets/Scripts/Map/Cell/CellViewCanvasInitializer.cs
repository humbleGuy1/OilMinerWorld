using UnityEngine;

namespace Assets.Scripts
{
    public class CellViewCanvasInitializer : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Canvas _canvas;

        public void Initialize()
        {
            _canvas.worldCamera = Camera.main;
        }
#endif
    }
}
