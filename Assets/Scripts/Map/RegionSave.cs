using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class RegionSave : SavedObject<RegionSave>
    {
        [SerializeField] public RegionState CurrentRegionState;
        [SerializeField] public int Coast = 10;

        public RegionSave(string guid) : base(guid) { }

        protected override void OnLoad(RegionSave loadedObject)
        {
            CurrentRegionState = loadedObject.CurrentRegionState;
            Coast = loadedObject.Coast;
        }

        public enum RegionState
        {
            Blocked,
            Unblocked
        }
    }
}
