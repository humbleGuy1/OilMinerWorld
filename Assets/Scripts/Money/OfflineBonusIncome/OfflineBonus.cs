using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class OfflineBonus : MonoBehaviour
    {
        [SerializeField] private TMP_Text _incomeBonusText;
        [SerializeField] private OfflineBonusButton _offlineBonusButtons;
        [Header("Settings")]
        [SerializeField] private float _delaySeconds = 120f;
        [SerializeField] private int _devide = 10;
        [SerializeField] private int _secondsInDay = 86000;

        public int Bonus { get; private set; } = 0;

        public void Activate()
        {
            if (PlayerPrefs.HasKey(TimeUtils.LastSaveTime) == false)
                return;

            if(SpentTwoMinutesLastDate() == false)
                return;

            _offlineBonusButtons.Enable();
            ShoReward();
            Bonus = GetOfflineBonus();
        }

        public int GetOfflineBonus()
        {
            int secondsSpan = GetSecondsSpent();
            secondsSpan = Mathf.Clamp(secondsSpan, 0, _secondsInDay);
            int offlineBonus = secondsSpan / _devide;

            return offlineBonus;
        }

        public void ShoReward(string custom = null)
        {
            _incomeBonusText.text = GetOfflineBonus().ToString();

            if (custom != null)
                _incomeBonusText.text = custom;
        }

        private bool SpentTwoMinutesLastDate()
        {
            int secondsSpan = GetSecondsSpent();
            return secondsSpan > _delaySeconds;
        }

        private int GetSecondsSpent()
        {
            DateTime lastSaveTime = TimeUtils.GetDateTime(TimeUtils.LastSaveTime, DateTime.UtcNow);
            TimeSpan timeSpent = DateTime.UtcNow - lastSaveTime;
            int secondsSpent = (int)timeSpent.TotalSeconds;

            return secondsSpent;
        }
    }
}
