using UnityEngine;

namespace Assets.Scripts
{
    public class OnFocusHandler : MonoBehaviour
    {
        private void OnEnable()
        {
            Application.focusChanged += OnFocusChanged;
        }

        private void OnDisable()
        {
            Application.focusChanged -= OnFocusChanged;            
        }

        protected virtual void OnFocusAppChanged(bool focus) { }

        private void OnFocusChanged(bool focus)
        {
            OnFocusAppChanged(focus);

            if(focus)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;
        }
    }
}
