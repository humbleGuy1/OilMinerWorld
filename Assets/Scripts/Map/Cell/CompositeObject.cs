using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public abstract class CompositeObject : MonoBehaviour
    {
        [SerializeField] protected List<Resource> _initialParts = new List<Resource>();
        [SerializeField] protected List<Resource> _renderingParts = new List<Resource>();
        [SerializeField] protected List<Resource> _availableParts = new List<Resource>();
        [SerializeField] protected Cell _cell;
        [SerializeField] private bool _isInfinite;

        public event Action<int, int> PartsChanged;

        public bool IsInfinite => _isInfinite;
        public bool HasParts => _renderingParts.Count > 0;
        public int MaxPartsCount => _initialParts.Count;
        public int PartsCount => _renderingParts.Count;
        public int RenderingPartsCount => _renderingParts.Count;

        public Resource FindNearestPart(Vector3 position)
        {
            Resource part;

            if (_isInfinite)
            {
                part = GetRandomPart();
                part = Instantiate(part, part.transform.position, part.transform.rotation);
                part.SetInfinitePrice();

                return part;
            }

            _availableParts = _availableParts.Where(part => part != null).OrderBy(part => Vector3.Distance(part.Root.position, position)).ToList();

            part = _availableParts.FirstOrDefault();

            if (part != null)
                _availableParts.Remove(part);
            else
                part = _renderingParts.FirstOrDefault();

            return part;
        }

        public void SetMultiplyPrice(float multiply, CellData.CellDifficult difficulty = CellData.CellDifficult.Ligth)
        {
            for (int i = 0; i < _initialParts.Count; i++)
            {
                _initialParts[i].SetMultiply(multiply, difficulty);
            }
        }

        public abstract void RemovePart(Resource removablePart);

        public void RemoveParts()
        {
            _initialParts.Clear();
            _renderingParts.Clear();
            _availableParts.Clear();
        }

        public bool HasPart(Resource part)
        {
            return _renderingParts.Contains(part);
        }

        public void ReturnPart(Resource returnablePart, bool hasPart)
        {
            if (hasPart)
                _renderingParts.Add(returnablePart);

            _availableParts?.Add(returnablePart);
            _renderingParts = _renderingParts.Where(part => part != null).ToList();

            OnPartsChanged();
        }

        protected void FillPartsLists()
        {
            _availableParts.AddRange(_initialParts);
            _renderingParts.AddRange(_initialParts);
        }

        protected void OnPartsChanged()
        {
            PartsChanged?.Invoke(MaxPartsCount, _renderingParts.Count);
        }

        private Resource GetRandomPart()
        {
            int index = UnityEngine.Random.Range(0, _initialParts.Count);
            Resource part = _initialParts[index];
            return part;
        }
    }
}
