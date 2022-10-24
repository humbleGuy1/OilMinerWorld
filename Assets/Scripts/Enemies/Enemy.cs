using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected Cell _cell;
        [SerializeField] private Transform _rootModel;
        [SerializeField] private Food _enemyPiece;
        [SerializeField] private SkinnedMeshRenderer _meshModel;
        [SerializeField] private EnemyFightButton _enemyFightButton;

        protected int Reward = 150;
        protected bool CanAttack;
        protected WalletPresenter Wallet;

        private Ant _antTarget;
        private bool _isOpen = false;
        private bool _isBusy = false;
        private bool _hasTarget = false;
        private int _health = 5;
        private Coroutine _attacking;

        private const float WaitAnimation = 0.5f;
        private const float WaitDeadAnimation = 0.3f;
        private const float DelayBetweenAttack = 5f;

        public EnemyFightButton EnemyFightButton => _enemyFightButton;
        public bool IsDead { get; private set; }

        public event Action Die;
        public event Action Damaged;
        public event Action Attacked;
        public event Action Activated;

        protected void OnEnable()
        {
            if(_cell != null)
                _cell.Opened += OnActivate;

            _enemyFightButton.FightCliked += TakeDamage;
            _enemyFightButton.Disable();
            _enemyPiece.SetCell(_cell);
        }

        private void OnDisable()
        {
            _cell.Opened -= OnActivate;
            _enemyFightButton.FightCliked -= TakeDamage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isOpen == false || _cell.CellState == CellState.DeadEnemy || _cell.CellState == CellState.EatenEnemy || IsDead)
                return;

            if (other.TryGetComponent(out Ant ant) && _isBusy == false)
                _attacking = StartCoroutine(Attacking(ant));
        }

        public void TakeDamage()
        {
            _health--;
            Damaged?.Invoke();

            if (_health <= 0 && IsDead ==false)
                OnDie();
        }

        public virtual void OnActivation() { }
        public virtual void OnDeath() { }
        public virtual void OnDisapear() { }

        protected void OnActivate()
        {
            if (_cell.CellState == CellState.DeadEnemy || _cell.CellState == CellState.EatenEnemy)
            {
                OnDead();
                return;
            }

            Activated?.Invoke();
            _meshModel.enabled = true;
            _isOpen = true;
            _enemyFightButton.Enable();
            OnActivation();
        }

        public void OnDie()
        {
            IsDead = true;
            _enemyFightButton.Disable();
            //_cell.SetEnemyFood(_enemyPiece);
            //_cell.ChangeState(CellState.DeadEnemy);
            _cell.ResetState();
            Die?.Invoke();
            enabled = false;
            //Invoke(nameof(ChangeModel), WaitDeadAnimation);
            OnDeath();

            StartCoroutine(GoinDownThrowTheGround());
        }

        private void OnDead()
        {
            IsDead = true;
            _enemyPiece.RemoveParts();
            _enemyFightButton.Disable();
            //_meshModel.enabled = false;
            enabled = false;
            _isOpen = false;
        }

        private void Attack()
        {
            _antTarget.Die();
        }

        private void ChangeModel()
        {
            _enemyPiece.transform.rotation = _rootModel.rotation;
            _enemyPiece.gameObject.SetActive(true);
            _meshModel.enabled = false;
        }

        private void LookAtTarget()
        {
            if (_antTarget != null)
            {
                _rootModel.transform.LookAt(_antTarget.transform);
                _rootModel.localRotation = Quaternion.Euler(0, _rootModel.localRotation.y, 0);
            }
        }

        private IEnumerator Rotating()
        {
            var wait = new WaitForFixedUpdate();

            while (_hasTarget)
            {
                LookAtTarget();
                yield return wait;
            }
        }

        private IEnumerator Attacking(Ant ant)
        {
            _isBusy = true;
            var wait = new WaitForSeconds(DelayBetweenAttack);
            var waitAnimation = new WaitForSeconds(WaitAnimation);

            yield return new WaitUntil(() => CanAttack);

            _hasTarget = true;
            _antTarget = ant;
            Coroutine rotating = StartCoroutine(Rotating());
            yield return waitAnimation;
            Attacked?.Invoke();
            yield return waitAnimation;
            Attack();
            _hasTarget = false;
            StopCoroutine(rotating);
            yield return wait;
            _isBusy = false;
        }

        private IEnumerator GoinDownThrowTheGround()
        {

            yield return new WaitForSeconds(3);
            float elapsedTime = 0f;
            float duration = 0.5f;
            Vector3 targetPosition = transform.position + Vector3.down * 5f;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, 1f * Time.deltaTime);

                yield return null;
            }

            OnDisapear();
            gameObject.SetActive(false);
        }
    }
}
