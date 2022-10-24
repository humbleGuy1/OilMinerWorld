using UnityEngine;

namespace Assets.Scripts
{
    public class SaveAntsProgress : MonoBehaviour
    {
        private const string AntsProgress = "AntsProgress";

        public static void Save(int count, int level)
        {
            PlayerPrefs.SetInt($"{level}AntsProgress", count);
        }
    }
}
