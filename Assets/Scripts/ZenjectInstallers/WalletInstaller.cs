using Assets.Scripts;
using UnityEngine;
using Zenject;

namespace Agava.Anthill.ZenjectInstallers
{
    public class WalletInstaller : MonoInstaller
    {
        [SerializeField] private LeafWalletPresenter _leafWallet;
        [SerializeField] private StoneWalletPresenter _stoneWallet;

        public override void InstallBindings()
        {
            BindLeafWallet();
            BindStoneWallet();
        }

        private void BindStoneWallet() =>
            Container
                .Bind<StoneWalletPresenter>()
                .FromInstance(_stoneWallet)
                .AsSingle();

        private void BindLeafWallet() =>
            Container
                .Bind<LeafWalletPresenter>()
                .FromInstance(_leafWallet)
                .AsSingle();
    }
}
