using Assets.Scripts;
using UnityEngine;

public class MenuHint : MonoBehaviour
{
    [SerializeField] private GameObject _finger;
    [SerializeField] private UpgradeMenu _upgradeMenu;
    [SerializeField] private AntHouse _house;
    [SerializeField] private Transform _closeButton;
    [SerializeField] private Transform _buyButton;

    private Vector3 _offset = new Vector3(50, -80);

    private void OnEnable()
    {
        _house.LevelIncreased += StartCloseHint;
        _upgradeMenu.PanelClosed += StopCloseHint;
        _upgradeMenu.PanelOpened += StartBuyHint;
    }

    public void StartCloseHint()
    {
        if (_house.Level == 2)
        {
            _closeButton.gameObject.SetActive(true);
            _finger.transform.position = _closeButton.position + _offset;
            _finger.SetActive(true);
        }
    }

    public void StartBuyHint()
    {
        if (_house.Level <= 1)
        {
            _closeButton.gameObject.SetActive(false);
            _finger.transform.position = _buyButton.position + _offset;
            _finger.SetActive(true);
        }
    }

    private void StopCloseHint(bool moving)
    {
        _finger.SetActive(false);
    }
}
