using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ŅellMaterialChanger))]
[CanEditMultipleObjects]
public class CustomInspectorCellMaterialChanger : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ŅellMaterialChanger cellMaterialChanger = (ŅellMaterialChanger)target;

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
