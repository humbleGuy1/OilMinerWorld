using System;

namespace Assets.Scripts
{
    public abstract class Ads : OnFocusHandler
    {
        public event Action Started;

        private void OnEnable()
        {
            Started?.Invoke();
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        protected abstract void Subscribe();
        protected abstract void UnSubscribe();
    }
}
