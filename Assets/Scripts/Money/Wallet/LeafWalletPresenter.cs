namespace Assets.Scripts
{
    public class LeafWalletPresenter : WalletPresenter
    {
        protected override Wallet Create() => new LeafWallet(StartValue);
    }
}
