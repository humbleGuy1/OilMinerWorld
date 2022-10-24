using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class HouseUpgradeButton : MonoBehaviour
{
    [SerializeField] private Cell _cell;

    public void OnPointerDown()
    {
        InputRoot inputRoot = FindObjectOfType<InputRoot>();

        if (inputRoot != null)
            inputRoot.ClickOn(_cell);
    }
}
