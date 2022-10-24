using UnityEngine;
using IJunior.TypedScenes;
using GameAnalyticsSDK;
using System.Collections;

public class Boot : MonoBehaviour
{
    private const float Delay = 3f;
    private SaveDeletion _saveDeletion = new SaveDeletion();

    private void OnEnable()
    {
        Invoke(nameof(LoadMap), Delay);
    }

    public void LoadMap()
    {
        GameAnalytics.Initialize();
        //LocalMap.Load();
        _saveDeletion.Delete();
        StartCoroutine(WaitingRemoteConfig());
        //if (GameAnalytics.IsRemoteConfigsReady())
        //    RemoteConfig.Init();

        //Map_2.Load();
    }

    private IEnumerator WaitingRemoteConfig()
    {
        float elapsedTime = 0f;
        float maxWaitTime = 5f;

        while (GameAnalytics.IsRemoteConfigsReady() == false && elapsedTime< maxWaitTime)
        {
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        if(GameAnalytics.IsRemoteConfigsReady())
            RemoteConfig.Init();

        Map_2.Load();
    }
}
