using System.Collections;
using Assets.Scripts;
using UnityEngine;

public class SpiderPointerHandler : MonoBehaviour
{
    [SerializeField] private WindowSpiderPointer _windowSpiderPointerPrefab;

    public void SpawnPointer(Cell targetCell)
    {
        var prefab = Instantiate(_windowSpiderPointerPrefab, transform);
        prefab.Initialize(targetCell.transform.position);
        StartCoroutine(DisableDelay(targetCell, prefab));
    }

    private IEnumerator DisableDelay(Cell cell, WindowSpiderPointer prefab)
    {
        while (cell.IsBlocked)
        {
            yield return null;
        }

        prefab.Disable();
    }
}
