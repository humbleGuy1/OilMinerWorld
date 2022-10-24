using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _hit;

        private Enemy _enemy;

        private const float Delay = 1f;
        private const string Die = "Die";
        private const string Attack = "Attack";

        private void OnEnable()
        {
            _enemy = GetComponent<Enemy>();
            _enemy.Attacked += OnAttacked;
            _enemy.Damaged += OnDamaged;
            _enemy.Die += OnDie;
        }

        private void OnDisable()
        {
            _enemy.Attacked -= OnAttacked;
            _enemy.Damaged -= OnDamaged;
            _enemy.Die -= OnDie;
        }

        private void OnAttacked()
        {
            _animator.SetTrigger(Attack);
        }

        private void OnDamaged()
        {
            _hit.Play();
        }

        private void OnDie()
        {
            _animator.SetTrigger(Die);
            Invoke(nameof(Disable), Delay);
        }

        private void Disable()
        {
            _animator.enabled = false;
            enabled = false;
        }
    }
}
