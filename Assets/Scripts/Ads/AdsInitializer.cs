using UnityEngine;

namespace Assets.Scripts
{
    public class AdsInitializer : MonoBehaviour
    {
        public const string MaxSdkKey = "sXK51_hTX1ce0ptcag_5fW3rThP28uRBkXv7iQsIDUH12xNyOtgztuyWpHpzTDTxK7EJIDZtxiVV1oSBExdZqb";
        public const string RewardInterstitialAdUnitId = "eb7c1a7d70ee1927";
        public const string InterstitialAdUnitId = "aa0c17ee3a96e956";
        public const string BannerAdUnitId = "5f57aeec16fff843";

        [SerializeField] private Ads _ads;

        private void OnEnable()
        {
            _ads.Started += Init;
        }

        private void OnDisable()
        {
            _ads.Started -= Init;            
        }

        private void Init()
        {
            MaxSdk.SetSdkKey(MaxSdkKey);
            MaxSdk.SetUserId("user");
            MaxSdk.InitializeSdk();
        }
    }
}
