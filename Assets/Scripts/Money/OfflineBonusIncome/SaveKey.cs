using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class SaveKey : MonoBehaviour
    {
        [System.Obsolete]
        private void OnEnable()
        {
            Application.DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus == false)
                Save();
        }

        private void Save()
        {
            TimeUtils.SetLastDateTime(TimeUtils.LastSaveTime, DateTime.UtcNow);
        }
    }
}
