using System;
using UnityEngine;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    [Serializable]
    public class CellSave : SavedObject<CellSave>
    {
        [SerializeField] public CellState CellState;

        public CellSave(string guid) : base(guid) { }

        protected override void OnLoad(CellSave loadedObject)
        {
            CellState = loadedObject.CellState;
        }
    }
}
