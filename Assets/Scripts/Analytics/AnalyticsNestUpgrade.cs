using UnityEngine;
using System.Linq;

namespace Assets.Scripts
{
    public class AnalyticsNestUpgrade : MonoBehaviour
    {
        [SerializeField] private AntHouse[] _antHouse;
        private Analytics _analytics;

        private void OnValidate()
        {
            var antshouses = FindObjectsOfType<AntHouse>();
            _antHouse = antshouses.Where(house => house.gameObject.activeSelf).ToArray();
        }


        private void OnEnable()
        {
            _analytics = Singleton<Analytics>.Instance;
            foreach (AntHouse house in _antHouse)
                house.Upgraded -= OnNestUpgraded;
        }

        private void OnDisable()
        {
            foreach (AntHouse house in _antHouse)
                house.Upgraded -= OnNestUpgraded;            
        }

        private void OnNestUpgraded(int level)
        {
            _analytics.OnNestUpgraded(level);
        }
    }
}
