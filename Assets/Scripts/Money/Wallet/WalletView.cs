using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void Render(float money)
    {
        _text.text = $"{Mathf.RoundToInt(money)}";
    }
}
