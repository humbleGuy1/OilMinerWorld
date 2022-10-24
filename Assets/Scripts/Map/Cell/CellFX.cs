using UnityEngine;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public class CellFX : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Cell _cell;

        private bool _isTargetPoint;
        private ParticleSystem _currentParticle;

        public void OnCellUnlocked()
        {
            if(_cell.CellType == CellType.LoaderHouse || _cell.CellType == CellType.DiggersHouse || _cell.CellType == CellType.Food || _cell.SlicedHex.IsInfinite)
                _isTargetPoint = true;

            if(_currentParticle == null && _isTargetPoint)
            {
                _currentParticle = Instantiate(_particleSystem, _parent.position, Quaternion.identity, _parent);
                _currentParticle.Play();
            }
        }

        public void OnCellOpened()
        {
            if(_currentParticle)
                _currentParticle.Stop();
        }
    }
}
