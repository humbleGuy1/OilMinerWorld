using UnityEngine;

namespace Assets.Scripts
{
    public class UpgradeMenuButtons : MonoBehaviour
    {
        [SerializeReference] private UpgradeMenu _upgradeMenu;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && _upgradeMenu.HasOpened)
                _upgradeMenu.OnCloseClicked();
        }
    }
}
