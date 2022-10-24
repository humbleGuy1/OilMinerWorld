using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts
{
    public class AntHouseUpgradeView : MonoBehaviour
    {
        [SerializeField] private List<AntHouseLevelView> _antHouseLevelModels;
        [SerializeField] private StarsHandler _starsHandler;

        public void UpgradeModel(int level)
        {
            AntHouseLevelView antHouseLevelView = _antHouseLevelModels.FirstOrDefault(model => model.Level == level);

            if (antHouseLevelView != default)
            {
                for (int i = 0; i < level; i++)
                {
                    _antHouseLevelModels[i].Enable(false);
                }

                antHouseLevelView.Enable(true);
            }
        }
    }
}
