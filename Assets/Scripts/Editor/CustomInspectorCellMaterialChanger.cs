using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ÑellMaterialChanger))]
[CanEditMultipleObjects]
public class CustomInspectorCellMaterialChanger : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ÑellMaterialChanger cellMaterialChanger = (ÑellMaterialChanger)target;

        if (GUILayout.Button("ColorizeCells"))
        {
            cellMaterialChanger.ColorizeCells();
        }

        if (GUILayout.Button("ColorizeRocks"))
        {
            cellMaterialChanger.ColorizeRocks();
        }

        if (GUILayout.Button("ColorizeInfiniteRocks"))
        {
            cellMaterialChanger.ColoRizeInfiniteRock();
        }
    }
}
