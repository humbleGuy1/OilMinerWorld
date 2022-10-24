using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class AnalyticsMoneyProperty : MonoBehaviour
    {
        private LeafWalletPresenter _leafWallet;
        private StoneWalletPresenter _stoneWallet;
        private Analytics _analytics;

        [Inject]
        private void Construct(LeafWalletPresenter leafWallet, StoneWalletPresenter stoneWallet)
        {
            _leafWallet = leafWallet;
            _stoneWallet = stoneWallet;
        }

        private void Awake()
        {
            _analytics = Singleton<Analytics>.Instance;
        }

        private void OnEnable()
        {
            _analytics.EventSent += OnSentEvent;
        }

        private void OnDisable()
        {
            _analytics.EventSent -= OnSentEvent;
        }

        private void OnSentEvent()
        {
            YandexAppMetricaUserProfile userProfile = new YandexAppMetricaUserProfile();
            userProfile.Apply(YandexAppMetricaAttribute.CustomNumber("current_soft_leaves").WithValue(_leafWallet.Value));
            userProfile.Apply(YandexAppMetricaAttribute.CustomNumber("current_soft_stones").WithValue(_stoneWallet.Value));

            AppMetrica.Instance.SetUserProfileID(new DuckyID().Value());
            AppMetrica.Instance.ReportUserProfile(userProfile);
        }
    }
}
