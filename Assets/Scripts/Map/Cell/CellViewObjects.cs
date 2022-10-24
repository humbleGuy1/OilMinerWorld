using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CellViewObjects 
{
    [SerializeField] private List<GameObject> _renderObjects;

    public void EnableRenderObjects()
    {
        foreach (var renderObject in _renderObjects)
        {
            renderObject.SetActive(true);
        }
    }

    public void DisableRenderObjects()
    {
        foreach (var renderObject in _renderObjects)
        {
            renderObject.SetActive(false);
        }
    }
}
