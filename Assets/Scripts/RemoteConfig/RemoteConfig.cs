using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Assets.Scripts;
using GameAnalyticsSDK;
using IJunior.TypedScenes;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class RemoteConfig
{
    private static bool _isInititate;
    public static void Init()
    {
        if (_isInititate)
            return;

        InterConfig.Init();
        RewardConfig.Init();
        _isInititate = true;
    }

    public static class InterConfig
    {
        public const string FirstAppear = "InterFirst";
        public const string DefaultAppear = "InterDefaultAppear";
        public const string CooldownConfig = "InterCooldown";

        public static float FirstAppearTime { get; private set; } = 240f;
        public static float DefaultAppearTime { get; private set; } = 180f;
        public static float Cooldown { get; private set; } = 60f;

        public static void Init()
        {
            FirstAppearTime = Config.GetValue(FirstAppear, FirstAppearTime);
            DefaultAppearTime = Config.GetValue(DefaultAppear, DefaultAppearTime);
            Cooldown = Config.GetValue(CooldownConfig, Cooldown);
        }
    }

    public static class RewardConfig
    {
        public const string CooldownConfig = "RewardCooldown";
        public const string DurationConfig = "RewardDuration";

        public static float Cooldown { get; private set; } = 120f;
        public static float Duration { get; private set; } = 60f;

        public static void Init()
        {
            Cooldown = Config.GetValue(CooldownConfig, Cooldown);
            Duration = Config.GetValue(DurationConfig, Duration);
        }
    }

    public class Config
    {
        public static float GetValue(string configName, float defaultValue)
        {
            string configValue = GameAnalytics.GetRemoteConfigsValueAsString(configName, $"{defaultValue}");

            if (configValue.Length <= 0 || float.TryParse(configValue, out float value) == false)
                return defaultValue;

            return value;
        }
    }
}


