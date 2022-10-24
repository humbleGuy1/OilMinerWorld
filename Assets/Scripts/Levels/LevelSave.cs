using UnityEngine;

namespace Assets.Scripts
{
    public class LevelSave : SavedObject<LevelSave>
    {
        [SerializeField] public bool Done = false;

        public LevelSave(string guid) : base(guid) { }

        protected override void OnLoad(LevelSave loadedObject)
        {
            Done = loadedObject.Done;
        }
    }
}
