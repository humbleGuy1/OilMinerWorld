using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FictionalHouseStatsView : MonoBehaviour
{
    [SerializeField] private HouseStatsView _houseStatsView;

    public void UpdatInfo(float defaultResPerSec ,int houseLevel, bool isMaxed, Sprite sprite)
    {
        float currentValue = defaultResPerSec * houseLevel;
        _houseStatsView.UpdateInfo(currentValue, defaultResPerSec, isMaxed, sprite);
    }
}
