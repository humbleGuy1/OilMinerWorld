using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using DG.Tweening;

public class CellsAnimator : MonoBehaviour
{
    [SerializeField] private Transform _centralPoint;

    public void Animate(List<Cell> cells)
    {
        StartCoroutine(Animating(cells));
    }

    private void MoveY(Cell cell)
    {
        if (cell.IsActivated)
            return;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(cell.transform.DOMoveY(1f, 0.2f));
        sequence.Append(cell.transform.DOMoveY(0, 0.4f));
    }

    private IEnumerator Animating(List<Cell> cells)
    {
        Collider[] colliders;
        int maxRadius = 6;
        float radius = 0.25f;

        while (radius <= maxRadius)
        {
            var waitForSeconds = new WaitForSeconds(0.025f);

            colliders = Physics.OverlapSphere(_centralPoint.position, radius);

            foreach (var collider in colliders)
            {
                if(collider.TryGetComponent(out Cell cell) && cells.Contains(cell))
                {
                    MoveY(cell);
                    cell.Activate();
                }
            }

            radius += 0.25f;

            yield return waitForSeconds;
        }
    }
}
