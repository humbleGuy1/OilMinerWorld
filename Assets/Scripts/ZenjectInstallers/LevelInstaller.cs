using Assets.Scripts;
using UnityEngine;
using Zenject;

namespace Agava.Anthill.ZenjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private EndGame _endGame;
        [SerializeField] private Shop _shop;
        [SerializeField] private StarGoal _starGoal;
        [SerializeField] private UpgradeMenu _upgradeMenu;
        [SerializeField] private AddCurrencyObjectsAnimation _addCurrencyObjectsAnimation;

        public override void InstallBindings()
        {
            BindAddCurrencyAnimation();
            BindShop();
            BindEndGame();
            BindStarGoal();
            BindUpgradeMenu();
        }

        private void BindUpgradeMenu() =>
            Container
                .Bind<UpgradeMenu>()
                .FromInstance(_upgradeMenu)
                .AsSingle();

        private void BindStarGoal() =>
            Container
                .Bind<StarGoal>()
                .FromInstance(_starGoal)
                .AsSingle();

        private void BindEndGame() =>
            Container
                .Bind<EndGame>()
                .FromInstance(_endGame)
                .AsSingle();

        private void BindShop() =>
            Container
                .Bind<Shop>()
                .FromInstance(_shop)
                .AsSingle();

        private void BindAddCurrencyAnimation() =>
            Container
                .Bind<AddCurrencyObjectsAnimation>()
                .FromInstance(_addCurrencyObjectsAnimation)
                .AsSingle();
    }
}
