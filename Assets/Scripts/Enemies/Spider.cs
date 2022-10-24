using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Spider : Enemy
{
    private SpiderCounter _spiderCounter;

    public void Init(Cell cell, SpiderCounter spiderCounter, WalletPresenter wallet)
    {
        _spiderCounter = spiderCounter;
        _cell = cell;
        Wallet = wallet;
        OnEnable();
        OnActivate();
    }

    public void EnableAttack()
    {
        CanAttack = true;
    }

    public override void OnActivation()
    {
        _cell.IsBlocked = true;
        _cell.LoaderHouse.Blockable.Block();
        _cell.DiggersHouse.Blockable.Block();
    }

    public override void OnDeath()
    {
        _cell.LoaderHouse.Blockable.UnBlock();
        _cell.DiggersHouse.Blockable.UnBlock();
        Wallet.AddResource(Reward);
        _spiderCounter.Deacrease();
    }

    public override void OnDisapear()
    {
        _cell.IsBlocked = false;
    }
}
