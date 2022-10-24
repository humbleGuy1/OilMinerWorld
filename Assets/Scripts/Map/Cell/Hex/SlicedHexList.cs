using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SlicedHexList : MonoBehaviour
    {
        [SerializeField] private List<SlicedHex> _slicedHexs;

        public SlicedHex GetSlicedHexByDifficult(int difficult)
        {
            if (_slicedHexs.Count <= difficult)
                throw new ArgumentOutOfRangeException(nameof(difficult));

            return _slicedHexs[difficult];
        }
    }
}
