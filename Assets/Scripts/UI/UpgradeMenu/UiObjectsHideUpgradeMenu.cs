using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class UiObjectsHideUpgradeMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup[] _canvasGroupsUi;
        [SerializeField] private UpgradeMenu _upgradeMenu;

        private List<CanvasGroup> _alreadyOffCanvases = new List<CanvasGroup>();

        private void OnEnable()
        {
            _upgradeMenu.CanvasPanelOpened += OnUpgradeActivated;
        }

        private void OnDisable()
        {
            _upgradeMenu.CanvasPanelOpened -= OnUpgradeActivated;            
        }

        private void OnUpgradeActivated(bool activate)
        {
            if (activate)
            {
                for (int i = 0; i < _canvasGroupsUi.Length; i++)
                {
                    if (_canvasGroupsUi[i].alpha == 1)
                        Extentions.DisableGroup(_canvasGroupsUi[i]);
                    else
                        _alreadyOffCanvases.Add(_canvasGroupsUi[i]);
                }
            }
            else
            {
                for (int i = 0; i < _canvasGroupsUi.Length; i++)
                    if (_canvasGroupsUi[i].alpha == 0 && _alreadyOffCanvases.Contains(_canvasGroupsUi[i]) == false)
                        Extentions.EnableGroup(_canvasGroupsUi[i]);

                _alreadyOffCanvases.Clear();
            }
        }
    }
}
