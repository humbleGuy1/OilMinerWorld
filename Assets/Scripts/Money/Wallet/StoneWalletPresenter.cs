public class StoneWalletPresenter : WalletPresenter
{
    protected override Wallet Create() => new StoneWallet(StartValue);
}
