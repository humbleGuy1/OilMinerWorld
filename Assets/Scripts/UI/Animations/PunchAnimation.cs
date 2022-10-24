using UnityEngine;
using DG.Tweening;

namespace Assets.Scripts
{
    public class PunchAnimation : MonoBehaviour
    {
        private Vector3 _punch = new Vector3(1.2f, 1.2f, 1.2f);

        private const float Duration = 0.3f;

        private void OnEnable()
        {
            transform.DOPunchScale(_punch, Duration);
        }
    }
}
