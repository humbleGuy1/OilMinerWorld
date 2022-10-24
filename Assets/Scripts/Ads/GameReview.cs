using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class GameReview : MonoBehaviour
{
//#if UNITY_ANDROID
    private const string RateUsKey = "RateUsKey";
    private const float Delay = 10f;

    [SerializeField] private GooglePlayReview _googlePlayReview;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey(RateUsKey) == false)
            Invoke(nameof(Launch), Delay);
    }

//#endif

    public void Launch()
    {
//#if UNITY_ANDROID
        //_googlePlayReview.Launch();
        PlayerPrefs.GetString(RateUsKey, true.ToString());
//#elif UNITY_IOS
  //      Device.RequestStoreReview();
//#endif
    }
}
