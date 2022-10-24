using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class AntHouseSaveData : SavedObject<AntHouseSaveData>
    {
        [SerializeField] public int Level;

        public AntHouseSaveData(string guid) : base(guid) { }

        protected override void OnLoad(AntHouseSaveData loadedObject)
        {
            if(loadedObject!= null)
                Level = loadedObject.Level;
        }
    }
}
