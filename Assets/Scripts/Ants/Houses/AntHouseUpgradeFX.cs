using UnityEngine;

namespace Assets.Scripts
{
    public class AntHouseUpgradeFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Transform _particleRoot;
        [SerializeField] private LoaderHouse _loaderHouse;
        [SerializeField] private DiggersHouse _digitalHouse;

        private ParticleSystem _currentParticle;

        private void OnEnable()
        {
            _currentParticle = Instantiate(_particleSystem, _particleRoot.position, Quaternion.identity, _particleRoot);

            _loaderHouse.LevelIncreased += OnAnyHouseUpgraded;
            _digitalHouse.LevelIncreased += OnAnyHouseUpgraded;
        }

        private void OnDisable()
        {
            _loaderHouse.LevelIncreased -= OnAnyHouseUpgraded;
            _digitalHouse.LevelIncreased -= OnAnyHouseUpgraded;            
        }

        private void OnAnyHouseUpgraded()
        {
            PlayParticle();
        }

        private void PlayParticle()
        {
            _currentParticle.Play();
        }
    }
}
