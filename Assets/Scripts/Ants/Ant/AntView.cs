using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AntView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _dieFX;

    private Animator _animator;

    public const string Dig = "Dig";
    public const string Load = "Load";
    public const string Run = "Run";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Play(string animation, float speed)
    {
        _animator.speed = speed;
        _animator.Play(animation);
    }

    public void Stop()
    {
        _animator.StopPlayback();
    }

    public void OnDie()
    {
        //_dieFX.Play();
        _animator.enabled = false;
    }

    public float GetCurrentAnimationDuration()
    {
        return _animator.GetCurrentAnimatorClipInfo(0).Length;
    }

    public void Hide()
    {
        Stop();
        gameObject.SetActive(false);
    }
}
