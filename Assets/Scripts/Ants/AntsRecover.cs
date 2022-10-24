using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class AntsRecover : MonoBehaviour
    {
        private Coroutine _coroutine;
        private AntCreator _antCreator;
        private AntHouse _antHouse;
        private int _count;

        private const float Delay = 15f;
        private const float WaitTime = 1f;

        public void Init(AntCreator antCreator, AntHouse antHouses)
        {
            _antHouse = antHouses;
            _antCreator = antCreator;

            //_antHouse.AntRemoved += OnAntRemoved;
        }

        private void OnDestroy()
        {
            //_antHouse.AntRemoved -= OnAntRemoved;
        }

        private void OnAntRemoved(Ant ant)
        {
            _count++;
            _coroutine ??= StartCoroutine(DelayedRecoveyAnt(ant, _antHouse, _antCreator.QueenCell));
        }

        private IEnumerator DelayedRecoveyAnt(Ant ant, AntHouse antHouse, Cell queenCell)
        {
            int result = _count;
            var wait = new WaitForSecondsRealtime(WaitTime);
            yield return new WaitForSeconds(Delay);

            for (int i = 0; i < _count; i++)
            {
                _antCreator.Create(ant.Type, antHouse, queenCell);
                yield return wait;
            }

            _count -= result;
            _coroutine = null;

            if (_count > 0)
                _coroutine = StartCoroutine(DelayedRecoveyAnt(ant, antHouse, queenCell));
        }
    }
}
