using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using TMPro;
using UnityEngine;

public class RemoteTest : MonoBehaviour
{
    [SerializeField] private TMP_Text _isRemoteReady;
    [SerializeField] private TMP_Text _rewordDuration;
    [SerializeField] private TMP_Text _rewordCooldown;
    [SerializeField] private TMP_Text _adsFirst;
    [SerializeField] private TMP_Text _adsDefault;
    [SerializeField] private TMP_Text _adsCooldown;

    private void Start()
    {
        _isRemoteReady.text = $"{GameAnalytics.IsRemoteConfigsReady()}";
        _rewordDuration.text = $"{RemoteConfig.RewardConfig.Duration}";
        _rewordCooldown.text = $"{RemoteConfig.RewardConfig.Cooldown}";

        _adsFirst.text = $"{RemoteConfig.InterConfig.FirstAppearTime}";
        _adsDefault.text = $"{RemoteConfig.InterConfig.DefaultAppearTime}";
        _adsCooldown.text = $"{RemoteConfig.InterConfig.Cooldown}";
    }
}
