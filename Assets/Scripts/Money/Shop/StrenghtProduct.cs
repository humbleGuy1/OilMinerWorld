using UnityEngine;

namespace Assets.Scripts
{
    public class StrenghtProduct : Product
    {
        private bool _isBoosted = false;

        private const int Boost = 5;

        public override void Buy()
        {
            if (_isBoosted == false)
                base.Buy();
        }

        public void BoostStrenght()
        {
            _isBoosted = true;
            ProductValue.Boost(Boost);
            OnProductBoosted();
        }

        public void OnBoostSpeedEnd()
        {
            _isBoosted = false;
            ProductValue.StopBoost(Boost);
            OnProductBoosted();
        }
    }
}
