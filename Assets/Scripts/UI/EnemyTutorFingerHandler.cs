using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EnemyTutorFingerHandler : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Image _finger;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _tapToKill;
        [SerializeField] private GameObject _speedTextHint;
        [SerializeField] private GameObject _strenghtTextHint;
        [SerializeField] private GameObject _incomeTextHint;
        [SerializeField] private GameObject _tapLeafHint;

        private void OnEnable()
        {
            _enemy.Die += OnEnemyDie;
            _enemy.Activated += OnEnemyActivated;
        }

        private void OnDisable()
        {
            _enemy.Die -= OnEnemyDie;            
            _enemy.Activated -= OnEnemyActivated;
        }

        private void OnEnemyActivated()
        {
            _finger.enabled = true;
            _animator.enabled = true;
            _tapLeafHint.gameObject.SetActive(false);
            _speedTextHint.gameObject.SetActive(false);
            _strenghtTextHint.gameObject.SetActive(false);
            _incomeTextHint.gameObject.SetActive(false);
            _tapToKill.SetActive(true);
        }

        private void OnEnemyDie()
        {
            gameObject.SetActive(false);
            _tapToKill.SetActive(false);
        }
    }
}
