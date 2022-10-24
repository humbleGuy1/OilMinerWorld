using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class CellBuyer : MonoBehaviour
    {
        private LeafWalletPresenter _leafWallet;
        private StoneWalletPresenter _stoneWallet;

        [Inject]
        private void Construct(LeafWalletPresenter leafWallet, StoneWalletPresenter stoneWallet)
        {
            _leafWallet = leafWallet;
            _stoneWallet = stoneWallet;
        }

        public void TryUpgrade(Cell cell)
        {
            if (cell.DiggersHouse.CanBuy(_stoneWallet))
            {
                _stoneWallet.SpendResource(cell.DiggersHouse.NextLevelPrice);
                cell.DiggersHouse.IncreaseLevel();
            }
            else if (cell.LoaderHouse.CanBuy(_stoneWallet))
            {
                _stoneWallet.SpendResource(cell.LoaderHouse.NextLevelPrice);
                cell.LoaderHouse.IncreaseLevel();
            }
        }

        public void TryBuy(Cell cell)
        {
            if (cell.Price > _leafWallet.Value)
                return;

            _leafWallet.SpendResource(cell.Price);
            cell.StartDigging();
        }
    }
}
