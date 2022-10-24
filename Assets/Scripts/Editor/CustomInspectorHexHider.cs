using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TopHexHider))]
[CanEditMultipleObjects]
public class CustomInspectorHexHider : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TopHexHider topHexHider = (TopHexHider)target;

        if (GUILayout.Button("Show"))
        {
            topHexHider.Show();
        }

        if (GUILayout.Button("Hide"))
        {
            topHexHider.Hide();
        }

        if (GUILayout.Button("HideSprite"))
        {
            topHexHider.HideSprite();
        }

        if (GUILayout.Button("ShowSprite"))
        {
            topHexHider.ShowSprite();
        }
    }
}
