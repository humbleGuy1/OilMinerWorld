using UnityEngine;
using System.Collections;
using Google.Play.Review;
using Assets.Scripts;

public class GooglePlayReview : MonoBehaviour
{
    [SerializeField] private EnternetConnetionHandler _enternetConnetionHandler;

    private const string RateUsKey = "RateUsKey";
    private const float Delay = 30f;

    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
    private Coroutine _startReview;

    private void OnEnable()
    {
        _reviewManager = new ReviewManager();
        
        if (PlayerPrefs.HasKey(RateUsKey) == false && _enternetConnetionHandler.EnternetAccess)
            Invoke(nameof(Launch), Delay);
    }

    private void Launch()
    {
        if (_startReview != null)
            return;

        PlayerPrefs.GetString(RateUsKey, true.ToString());

        _startReview = StartCoroutine(StartReview());
    }

    private IEnumerator StartReview()
    {
        yield return RequestReviewInfo();
        yield return LaunchReview();
    }

    private IEnumerator RequestReviewInfo()
    {
        var requestFlowOperation = _reviewManager.RequestReviewFlow();

        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError(requestFlowOperation.Error.ToString());
            yield break;
        }

        _playReviewInfo = requestFlowOperation.GetResult();
    }

    private IEnumerator LaunchReview()
    {
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);

        yield return launchFlowOperation;
        _playReviewInfo = null;

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError(launchFlowOperation.Error.ToString());
            yield break;
        }
    }
}
