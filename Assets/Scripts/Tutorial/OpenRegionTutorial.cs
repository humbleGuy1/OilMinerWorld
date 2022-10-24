using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class OpenRegionTutorial : MonoBehaviour
{
    [SerializeField] private OneTimeShow _finger;
    [SerializeField] private Region _region;

    private void OnEnable()
    {
        _region.StarsCollected += OnStarsCollected;
    }

    private void OnStarsCollected()
    {
        _finger.Show();
    }
}
