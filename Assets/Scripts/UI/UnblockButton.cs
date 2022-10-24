using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class UnblockButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _icon;

        private Collider _collider;

        public bool Activated { get; private set; } = false;

        public event Action Clicked;

        private void OnEnable()
        {
            _collider = GetComponent<Collider>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_icon.gameObject.activeSelf)
            {
                Clicked?.Invoke();
                OnActivated();
                _icon.enabled = false;
            }
        }

        public void OnActivated()
        {
            Activated = true;
        }

        public void HideButton()
        {
            _collider.enabled = false;
            _icon.gameObject.SetActive(false);
        }

        public void ShowButton()
        {
            if (Activated == false)
            {
                _collider.enabled = true;
                _icon.gameObject.SetActive(true);
            }
        }
    }
}
