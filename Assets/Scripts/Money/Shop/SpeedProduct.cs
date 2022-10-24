namespace Assets.Scripts
{
    public class SpeedProduct : Product
    {
        private bool _isBoosted = false;

        private const int Boost = 3;

        public override void Buy()
        {
            if (_isBoosted == false)
                base.Buy();
        }

        public void BoostSpeed()
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
