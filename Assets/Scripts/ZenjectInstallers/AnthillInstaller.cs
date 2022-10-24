using Assets.Scripts;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Agava.Anthill.ZenjectInstallers
{
    public class AnthillInstaller : MonoInstaller
    {
        [SerializeField] private Assets.Scripts.Anthill _anthill;
        [SerializeField] private Cell[] _cells;
        [SerializeField] private Region[] _regions;
        [SerializeField] private AntHouse[] _antHouses;

        public override void InstallBindings()
        {
            BindCellList();
            BindAntHouseList();
            BindRegions();
            BindAnthill();
        }

        private void BindAnthill() =>
            Container
                .BindInterfacesTo<Assets.Scripts.Anthill>()
                .FromInstance(_anthill)
                .AsSingle();

        private void BindRegions() =>
            Container
                .Bind<Region[]>()
                .FromInstance(_regions)
                .AsSingle();

        private void BindAntHouseList()
        {
            var antHouseList = new AntHouseList(_antHouses);

            Container
                .Bind<AntHouseList>()
                .FromInstance(antHouseList)
                .AsSingle();
        }

        private void BindCellList()
        {
            var cellList = new CellList(_cells);

            Container
                .Bind<CellList>()
                .FromInstance(cellList)
                .AsSingle();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetupFields();
        }

        [ContextMenu(nameof(SetupFields))]
        private void SetupFields()
        {
            _anthill = FindObjectOfType<Assets.Scripts.Anthill>();
            _cells = FindObjectsOfType<Cell>(false);
            _regions = FindObjectsOfType<Region>(false);
            _antHouses = FindObjectsOfType<AntHouse>(false);

            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}
