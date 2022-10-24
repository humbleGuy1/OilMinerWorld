using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class OpenRegionButton : MonoBehaviour
{
    [SerializeField] private Region _region;
    [SerializeField] private Image _background;

    private bool _canUnblock;

    private void OnEnable()
    {
        _region.StarsCollected += OnStarsCollected;
    }

    public void OnButtonClick()
    {
        if (_canUnblock)
        {
            _region.Unblock();
            _region.StarsCollected -= OnStarsCollected;
            _background.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Analytics.Instance.OnRegionOpen("region_open", _region.Index);
        }
    }

    private void OnStarsCollected()
    {
        _canUnblock = true;
    }
}
