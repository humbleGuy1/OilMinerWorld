using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class UpgradeTutorial : MonoBehaviour
{
    [SerializeField] private OneTimeShow _oneTimeShow;
    [SerializeField] private UpgradeMenu _upgradeMenu;

    private void OnEnable()
    {
        _upgradeMenu.PanelOpened += OnPanelShown;
    }

    private void OnDisable()
    {
        _upgradeMenu.PanelOpened -= OnPanelShown;
    }

    private void OnPanelShown()
    {
        if (_upgradeMenu.CanBuy() == false)
            return;

        if (_oneTimeShow.WasShown())
            _upgradeMenu.PanelOpened -= OnPanelShown;

        _oneTimeShow.Show();
    }
}
