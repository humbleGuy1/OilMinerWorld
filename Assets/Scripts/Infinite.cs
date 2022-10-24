using Assets.Scripts;
using DG.Tweening;
using UnityEngine;

public class Infinite : MonoBehaviour
{
    [SerializeField] private float _duration;

    public void Enable()
    {
        gameObject.SetActive(true);
        //transform.DORotate(new Vector3(0, 360f, 0), _duration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
