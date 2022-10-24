using UnityEngine;

namespace Assets.Scripts
{
    public class DontDestrouOnLoad : MonoBehaviour
    {
        [System.Obsolete]
        private void OnEnable()
        {
            Application.DontDestroyOnLoad(gameObject);
        }
    }
}
