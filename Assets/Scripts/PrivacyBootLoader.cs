using System;
using System.Net;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using IJunior.TypedScenes;

namespace Assets.Scripts
{
    public class PrivacyBootLoader : MonoBehaviour
    {
        [SerializeField] private EnternetConnetionHandler _enternetConnetionHandler;
 
        private const string GDPRKey = "GDPRKey";
        private const float WaitTimeSec = 0.5f;

        public GeoData GeoDataResult = new GeoData();

        [Obsolete]
        private void OnEnable()
        {
            if (PlayerPrefs.HasKey(GDPRKey))
                Load();
            else
                StartCoroutine(WaitingConnection());
        }

        [Obsolete]
        private void Init(bool access)
        {
            if (access)
                StartCoroutine(DetectCounrty());
            else
                Load();
        }

        private void Show()
        {
            SimpleGDPR.ShowDialog(new TermsOfServiceDialog().
                SetTermsOfServiceLink("https://playducky.com/tos").
                SetPrivacyPolicyLink("https://playducky.com/privacypolicy"),
                TermsOfServiceDialogClosed);
        }

        private void Load()
        {
            Boot_2_Ads.Load();
        }

        private void TermsOfServiceDialogClosed()
        {
            PlayerPrefs.SetString(GDPRKey, true.ToString());
            Load();
        }

        [Obsolete]
        private bool IsEuropeZone()
        {
            if (GeoDataResult.continentCode == "EU")
                return true;

            return false;
        }

        [Obsolete]
        private IEnumerator DetectCounrty()
        {
            string ipAdress = new WebClient().DownloadString("https://api.ipify.org");
            string requetsData = new WebClient().DownloadString($"http://ip-api.com/json/{ipAdress}?fields=status,continentCode");

            yield return new WaitForSeconds(WaitTimeSec);

            if (requetsData == null)
                Debug.Log("error");
            else
                GeoDataResult = JsonUtility.FromJson<GeoData>(requetsData);

            if (IsEuropeZone())
                Show();
            else
                Load();
        }

        [Obsolete]
        private IEnumerator WaitingConnection()
        {
            var wait = new WaitForSeconds(WaitTimeSec);
            yield return _enternetConnetionHandler.TestConnection(callback => Init(callback));
        }
    }

    public class GeoData
    {
        public string status;
        public string continentCode;
    }
}
