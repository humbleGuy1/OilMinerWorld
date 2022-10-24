using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [SerializeField] private Color _color;

    [field: SerializeField] public float NearZoomLimit { get; private set; } = -13f;
    [field: SerializeField] public float FarZoomLimit { get; private set; } = 4f;
    [field: SerializeField] public Transform Camera { get; private set; }
    [field: SerializeField] public float Left { get; private set; }
    [field: SerializeField] public float Right { get; private set; }
    [field: SerializeField] public float Top { get; private set; }
    [field: SerializeField] public float Bottom { get; private set; }


    //private void OnDrawGizmos()
    //{
    //    //Vector3 leftTop = new Vector3(Left, 0, Top);
    //    //Vector3 leftBottom = new Vector3(Left, 0, Bottom);
    //    //Vector3 rightTop = new Vector3(Right, 0, Top);
    //    //Vector3 rightBottom = new Vector3(Right, 0, Bottom);

    //    //Gizmos.color = _color;
    //    //Gizmos.DrawLine(leftTop, rightTop);
    //    //Gizmos.DrawLine(rightTop, rightBottom);
    //    //Gizmos.DrawLine(rightBottom, leftBottom);
    //    //Gizmos.DrawLine(leftBottom, leftTop);
    //}
}
