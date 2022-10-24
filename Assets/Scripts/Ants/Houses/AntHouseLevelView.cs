using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class AntHouseLevelView
{
    [SerializeField] private UpgradeModelView _model;

    [field: SerializeField] public int Level { get; private set; }


    public void Enable(bool playAnimation)
    {
        _model.Animate(playAnimation);
    }
}
