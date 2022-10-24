using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using System.Linq;

public class SpiderSpawnerHandler : MonoBehaviour
{
    private List<Region> _regions = new List<Region>();
    private int _openRegionCounter;
    private int _openRegionToStartSpawnSpiders = 2;
    private SpiderCounter _spiderCounter = new SpiderCounter();
    
    public bool IsActive { get; private set; }

    public void Init(List<Region> regions)
    {
        //_regions = regions;

        //foreach (var region in _regions)
        //{
        //    region.Opened += ActivateSpawners;

        //    if (region.RegiondState == RegionSave.RegionState.Unblocked)
        //        ActivateSpawners();
        //}
    }

    private void OnEnable()
    {
        if(_regions.Count > 0)
        {
            foreach (var region in _regions)
            {
                region.Opened += ActivateSpawners;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var region in _regions)
        {
            region.Opened -= ActivateSpawners;
        }
    }

    private void ActivateSpawners()
    {
        _openRegionCounter++;

        if (_openRegionCounter > _openRegionToStartSpawnSpiders && IsActive ==false)
        {
            IsActive = true;
            
            StartCoroutine(SpiderSpawning());
        }
    }

    private IEnumerator SpiderSpawning()
    {
        while (IsActive)
        {
            yield return new WaitUntil(() => _spiderCounter.Counter < 1);

            _spiderCounter.Increase();

            yield return new WaitForSeconds(5);

            var openRegions = _regions.Where(region => region.RegiondState == RegionSave.RegionState.Unblocked).ToList();
            var notBlockedRegions = openRegions.Where(region => region.SpiderSpawner.IsRegionBlocked() == false).ToList();

            if(notBlockedRegions.Count>0)
            {
                int index = Random.Range(0, notBlockedRegions.Count);
                notBlockedRegions[index].SpiderSpawner.SpawnRandom(_spiderCounter);
            }
        }
    }
}
