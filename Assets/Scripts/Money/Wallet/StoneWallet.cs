using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class StoneWallet : Wallet
{
    private const string SaveKey = "SaveStoneWallet";

    public StoneWallet(int value) : base(value, SaveKey) { }
}
