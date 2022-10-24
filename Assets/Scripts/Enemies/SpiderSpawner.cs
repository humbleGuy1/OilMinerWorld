using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using Assets.Scripts;
using NSubstitute.Core;
using UnityEngine;

public class SpiderSpawner : MonoBehaviour
{
    [SerializeField] private Spider _spider;
    [SerializeField] private List<LoaderHouse> _loaderHouses;
    [SerializeField] private SpiderPointerHandler _pointerHandler;

    private WalletPresenter _wallet;
    private List<IBlockable> _blockables;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        _wallet = FindObjectOfType<WalletPresenter>();
    }

    public void Init(List<LoaderHouse> loaderHouse)
    {
        _loaderHouses = loaderHouse;
        _blockables = GetComponentsInChildren<MonoBehaviour>().OfType<IBlockable>().ToList();
    }

    public void SpawnRandom(SpiderCounter spiderCounter)
    {
        IBlockable blockable = _blockables.FirstOrDefault(blockable => blockable.Blockable.IsBlocked == false);

        if(blockable != null)
            SpawnToCell(blockable.Blockable.Cell, spiderCounter);
    }

    public void SpawnToCell(Cell cell, SpiderCounter spiderCounter)
    {
        if (cell.IsBlocked || cell.CellState == CellData.CellState.Opened == false)
            return;

        Spider spider = Instantiate(_spider, cell.transform.position, _spider.transform.rotation);

        foreach (var loaderHouse in _loaderHouses)
        {
            loaderHouse.AddTarget(cell);
        }

        spider.EnemyFightButton.Enable();
        spider.Init(cell, spiderCounter, _wallet);
        StartCoroutine(SpawningAnimation(spider, cell));
        _pointerHandler.SpawnPointer(cell);
    }

    public bool IsRegionBlocked()
    {
        return _blockables.FirstOrDefault(blockable => blockable.Blockable.IsBlocked == false) == default;
    }

    private IEnumerator SpawningAnimation(Spider spider, Cell cell)
    {
        float elapsedTime = 0f;
        float dropTime = 3f;
        Vector3 targetPosition = spider.transform.position+Vector3.up*0.5f;
        spider.transform.position += Vector3.up * 10f;
        Vector3 initialPos = spider.transform.position;
        float speedMultiplier = 1f;

        while (elapsedTime<dropTime)
        {
            if (spider.IsDead)
                speedMultiplier = 4f;

            elapsedTime += Time.deltaTime* speedMultiplier;

            spider.transform.position = Vector3.Lerp(initialPos, targetPosition, elapsedTime / dropTime);

            yield return null;
        }

        spider.EnableAttack();
    }
}
