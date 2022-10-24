#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class RevertingPrefabs : MonoBehaviour
    {

        [ContextMenu(nameof(RevertAddedGameObject))]
        private void RevertAddedGameObject()
        {
            PrefabUtility.RevertObjectOverride(this, InteractionMode.AutomatedAction);
        }

        [ContextMenu(nameof(RevertPrefabInstanceAll))]
        private void RevertPrefabInstanceAll()
        {
            PrefabUtility.RevertPrefabInstance(gameObject);
        }
    }
}
#endif
