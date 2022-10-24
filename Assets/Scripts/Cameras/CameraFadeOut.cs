using UnityEngine;

namespace Assets.Scripts
{
    public class CameraFadeOut : CamerasSwitcher
    {
        [SerializeField] private LocalMapLoader _localMap;

        protected override void Init()
        {
            EnableCamera(_mainAnthillMapCamera1);
        }
    }
}
