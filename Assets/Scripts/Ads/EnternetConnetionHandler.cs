using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class EnternetConnetionHandler : MonoBehaviour
    {
        [SerializeField] private string[] _urls;

        private const float Delay = 1f;

        public bool EnternetAccess { get; private set; } = false;

        [Obsolete]
        private void OnEnable()
        {
            StartCoroutine(TestConnection());
        }

        [Obsolete]
        public IEnumerator TestConnection(Action<bool> callback)
        {
            foreach (string url in _urls)
            {
                UnityWebRequest request = UnityWebRequest.Get(url);
                yield return request.SendWebRequest();

                if(request.isNetworkError == false)
                {
                    EnternetAccess = true;
                    callback(true);
                    yield break;
                }
            }

            callback(false);
            EnternetAccess = false;
        }

        [Obsolete]
        private IEnumerator TestConnection()
        {
            var wait = new WaitForSecondsRealtime(Delay);

            while (true)
            {
                foreach (string url in _urls)
                {
                    UnityWebRequest request = UnityWebRequest.Get(url);
                    yield return request.SendWebRequest();

                    if (request.isNetworkError == false)
                    {
                        EnternetAccess = true;
                        yield break;
                    }
                }

                EnternetAccess = false;
                yield return wait;
            }
        }
    }
}
