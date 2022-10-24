#if UNITY_EDITOR
using Assets.Scripts;
using UnityEngine;

public class TopHexHider : MonoBehaviour
{
    [SerializeField] private CellTopHex[] _hexArray;

    public void Hide()
    {
        foreach(var hex in _hexArray)
        {
            hex.Disable();
        }
    }

    public void Show()
    {
        foreach (var hex in _hexArray)
        {
            hex.Enable();
        }
    }

    public void ShowSprite()
    {
        foreach (var hex in _hexArray)
        {
            hex.EnableSprite();
        }
    }

    public void HideSprite()
    {
        foreach (var hex in _hexArray)
        {
            hex.DisableSprite();
        }
    }
}
#endif
