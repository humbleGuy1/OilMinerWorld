namespace Assets.Scripts
{
    public class LeafWallet : Wallet
    {
        private const string SaveKey = "SaveLeafWallet";

        public LeafWallet(int value) : base(value, SaveKey) { }
    }
}
