using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private WalletPresenter _wallet;
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _x1Button;
    [SerializeField] private Button _x2Button;
    [SerializeField] private Button _x4Button;
    [SerializeField] private Color _pressedColor;
    [SerializeField] private Color _pressedShadowColor;

    private Color _defaultColor;
    private Color _defaultShadowColor;
    private Image _imageX1;
    private Image _imageX2;
    private Image _imageX4;
    private Shadow _shadowX1;
    private Shadow _shadowX2;
    private Shadow _shadowX4;

    private void Awake()
    {
        _imageX1 = _x1Button.GetComponent<Image>();
        _imageX2 = _x2Button.GetComponent<Image>();
        _imageX4 = _x4Button.GetComponent<Image>();

        _shadowX1 = _x1Button.GetComponent<Shadow>();
        _shadowX2 = _x2Button.GetComponent<Shadow>();
        _shadowX4 = _x4Button.GetComponent<Shadow>();

        _defaultColor = _imageX1.color;
        _defaultShadowColor = _shadowX1.effectColor;
    }

    private void Start()
    {
        OnX1ButtonClick();
    }

    private void OnEnable()
    {
        _addButton.onClick.AddListener(OnAddButtonClick);
        _x1Button.onClick.AddListener(OnX1ButtonClick);
        _x2Button.onClick.AddListener(OnX2ButtonClick);
        _x4Button.onClick.AddListener(OnX4ButtonClick);
    }

    private void OnDisable()
    {
        _addButton.onClick.RemoveListener(OnAddButtonClick);
        _x1Button.onClick.RemoveListener(OnX1ButtonClick);
        _x2Button.onClick.RemoveListener(OnX2ButtonClick);
        _x4Button.onClick.RemoveListener(OnX4ButtonClick);
    }

    private void OnAddButtonClick()
    {
        _wallet.AddResource(99999);
    }

    private void OnX1ButtonClick()
    {
        Time.timeScale = 1;

        _imageX1.color = _pressedColor;
        _imageX2.color = _defaultColor;
        _imageX4.color = _defaultColor;

        _shadowX1.effectColor = _pressedShadowColor;
        _shadowX2.effectColor = _defaultShadowColor;
        _shadowX4.effectColor = _defaultShadowColor;
    }    
    
    private void OnX2ButtonClick()
    {
        Time.timeScale = 2;

        _imageX1.color = _defaultColor;
        _imageX2.color = _pressedColor;
        _imageX4.color = _defaultColor;

        _shadowX1.effectColor = _defaultShadowColor;
        _shadowX2.effectColor = _pressedShadowColor;
        _shadowX4.effectColor = _defaultShadowColor;
    }

    private void OnX4ButtonClick()
    {
        Time.timeScale = 4;

        _imageX1.color = _defaultColor;
        _imageX2.color = _defaultColor;
        _imageX4.color = _pressedColor;

        _shadowX1.effectColor = _defaultShadowColor;
        _shadowX2.effectColor = _defaultShadowColor;
        _shadowX4.effectColor = _pressedShadowColor;
    }
}
