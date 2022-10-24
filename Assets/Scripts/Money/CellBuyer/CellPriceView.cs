using System.Collections;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class CellPriceView : MonoBehaviour
{
    [Header("Stone(Level Up) Price")]
    [SerializeField] private GameObject _stonePricePanel;
    [Header("Leaf Price")]
    [SerializeField] private TMP_Text _leafPrice;
    [SerializeField] private GameObject _leafPricePanel;
    [SerializeField] private Color _openForSaleColor;
    [SerializeField] private Color _closeForSaleColor;

    private AntHouse _antHouse;

    public bool IsActive => _leafPricePanel.gameObject.activeSelf;

    public void Init(AntHouse antHouse)
    {
        _antHouse = antHouse;
    }

    public void ShowLeafPricePanel() => _leafPricePanel.SetActive(true);
    public void HideLeafPricePanel() => _leafPricePanel.SetActive(false);

    public void ShowStonePricePanel()
    {
        if(UpgradeMenu.IsOpen == false)
            _stonePricePanel.SetActive(true);
    }

    public void HideStonePricePanel() => _stonePricePanel.SetActive(false);
    public void RenderCellPrice(int price) => _leafPrice.text = price.ToString();
    public void RenderOpenForSale() => RenderPriceColor(openForSale: true);
    public void RenderCloseForSale() => RenderPriceColor(openForSale: false);

    private void RenderPriceColor(bool openForSale) => _leafPrice.color = openForSale ? _openForSaleColor : _closeForSaleColor;

    private IEnumerator Waiting()
    {
        yield return new WaitUntil(() => UpgradeMenu.IsOpen == false);

        _stonePricePanel.SetActive(true);
    }
}
