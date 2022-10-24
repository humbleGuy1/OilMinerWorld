using System;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts
{
    public static class TimeUtils
    {
        public const string LastSaveTime = "LastSaveTime";

        public static void SetLastDateTime(string key, DateTime value)
        {
            string converted = value.ToString("u", CultureInfo.InvariantCulture);
            PlayerPrefs.SetString(key, converted);
        }

        public static DateTime GetDateTime(string key, DateTime defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                string stored = PlayerPrefs.GetString(key);
                DateTime result = DateTime.ParseExact(stored, "u", CultureInfo.InvariantCulture);
                return result;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
