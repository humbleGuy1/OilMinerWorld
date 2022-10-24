using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderFinder : MonoBehaviour
{
    [SerializeField] private Outline[] _activeOutlines;

    private void OnValidate()
    {
        //_activeOutlines = FindObjectsOfType<Outline>();
    }

    [ContextMenu("Duplicate")]
    public void Duplicate()
    {
        foreach(var outline in _activeOutlines)
        {
            var bottomOutline = Instantiate(outline, outline.transform);
            bottomOutline.transform.position = Vector3.down * 0.5f;
        }
    }
}
