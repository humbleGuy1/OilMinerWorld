using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public class Food : CompositeObject
    {
        [SerializeField] private bool _isRegrowing = true;

        public event Action Regrowed;
        public event Action StartEating;
        public event Action<Food> Eaten;

        public Vector3 CellPosition => _cell.transform.position;

        public void SetCell(Cell cell)
        {
            _cell = cell;
        }

        public override void RemovePart(Resource removablePart)
        {
            _renderingParts.Remove(removablePart);

            if (_initialParts.Count - _renderingParts.Count == 1)
                StartEating?.Invoke();

            if (_renderingParts.Count <= 0 && _isRegrowing)
                StartCoroutine(WaitingRegrow());

            if(_renderingParts.Count <= 0 && _cell.CellState == CellState.DeadEnemy)
                _cell.ResetState();
        }

        public void Regrow()
        {
            FillPartsLists();
            SetActivePieces(true);

            foreach (FoodPart piece in _initialParts)
                piece.SetWaitingRegrowState(false);

            Regrowed?.Invoke();
        }

        public void SetActivePieces(bool state)
        {
            foreach (FoodPart piece in _initialParts)
                piece.gameObject.SetActive(state);
        }

        private IEnumerator WaitingRegrow()
        {
            yield return new WaitUntil(() => _initialParts.Where(piece => (piece as FoodPart).IsWaitingRegrow == true).ToList().Count == _initialParts.Count);
            Eaten?.Invoke(this);
        }
    }
}
