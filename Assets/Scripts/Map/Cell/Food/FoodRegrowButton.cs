using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class FoodRegrowButton : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;

        public Food Food { get; private set; }

        public void Init(Food food)
        {
            Food = food;
            Clicked?.Invoke();
            Destroy(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
            Destroy(gameObject);
        }


    }
}
