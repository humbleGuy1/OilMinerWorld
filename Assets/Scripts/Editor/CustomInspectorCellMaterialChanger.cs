using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(�ellMaterialChanger))]
[CanEditMultipleObjects]
public class CustomInspectorCellMaterialChanger : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        �ellMaterialChanger cellMaterialChanger = (�ellMaterialChanger)target;

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
