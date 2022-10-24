using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CellObject : MonoBehaviour
    {
        [SerializeField] private List<ObjectByCellType> _objects;

        public void Setup(CellData.CellType type)
        {
            foreach (ObjectByCellType item in _objects)
                item.Object.SetActive(item.CellType == type);
        }

        [Serializable]
        private class ObjectByCellType
        {
            [field: SerializeField] public CellData.CellType CellType;
            [field: SerializeField] public GameObject Object;
        }
    }
}
