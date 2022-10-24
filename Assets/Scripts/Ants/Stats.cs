using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

[Serializable]
public class Stats
{
    private List<Stat> _stats;

    [SerializeField] private Speed _speed = new Speed();
    [SerializeField] private Strength _strength = new Strength();
    [SerializeField] private Income _income = new Income();

    public Speed Speed => _speed;
    public Strength Strengh => _strength;
    public Income Income => _income;

    public Stats()
    {
        _stats = new List<Stat>()
        {
            _speed,
            _strength,
            _income
        };
    }

    public void Upgrade(Product product)
    {
        Stat stat = _stats.FirstOrDefault(stat => stat.Type == product.StatsType);

        if (stat != default)
            stat.Upgrade(product.ProductValue.Value);
    }
}
