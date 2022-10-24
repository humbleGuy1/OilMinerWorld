using Assets.Scripts;
using UnityEngine;
using Zenject;

namespace Agava.Anthill.ZenjectInstallers
{
    public class FoodInstaller : MonoInstaller
    {
        [SerializeField] private FoodRegrowButton _foodRegrowButtonTemplate;

        public override void InstallBindings()
        {
            BindFoodRegrowButtonTemplate();
        }

        private void BindFoodRegrowButtonTemplate() =>
            Container
                .Bind<FoodRegrowButton>()
                .FromInstance(_foodRegrowButtonTemplate)
                .AsSingle();
    }
}
