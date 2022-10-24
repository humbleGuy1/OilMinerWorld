using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private ParticleSystem _particleSystem;

    public float Value { get; private set; }
    public StatsType Type {get; private set; }

    public Stat(StatsType statsType)
    {
        Type = statsType;
    }

    public void Upgrade(float value)
    {
        Value = value;

        _particleSystem.Play();
    }
}

public enum StatsType
{
    Speed,
    Strength,
    Income
}
