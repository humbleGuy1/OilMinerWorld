using UnityEngine;

namespace Assets.Scripts
{
    public class CellTopHex : MonoBehaviour
    {
        [SerializeField] private GameObject _hexSprite;

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void DisableSprite()
        {
            _hexSprite.SetActive(false);
        }

        public void EnableSprite()
        {
            _hexSprite.SetActive(true);
        }
    }
}
