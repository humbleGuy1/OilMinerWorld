using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RateUs : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Sprite _activeStar;
        [Header("Buttons")]
        [SerializeField] private Button _submit;
        [SerializeField] private Button _notNow;
        [Header("Buttons - Stars")]
        [SerializeField] private Button _star1;
        [SerializeField] private Button _star2;
        [SerializeField] private Button _star3;
        [SerializeField] private Button _star4;
        [SerializeField] private Button _star5;

        private const float Delay = 30f;
        private const string RateUsKey = "RateUs";

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey(RateUsKey))
                Destroy(gameObject);
            else
                Invoke(nameof(Show), Delay);

            _submit.onClick.AddListener(OnSubmit);
            _notNow.onClick.AddListener(OnNotNowClicked);

            _star1.onClick.AddListener(On1StarClicked);
            _star2.onClick.AddListener(On2StarClicked);
            _star3.onClick.AddListener(On3StarClicked);
            _star4.onClick.AddListener(On4StarClicked);
            _star5.onClick.AddListener(On5StarClicked);
        }

        private void OnDisable()
        {
            _submit.onClick.RemoveListener(OnSubmit);
            _notNow.onClick.RemoveListener(OnNotNowClicked);

            _star1.onClick.RemoveListener(On1StarClicked);
            _star2.onClick.RemoveListener(On2StarClicked);
            _star3.onClick.RemoveListener(On3StarClicked);
            _star4.onClick.RemoveListener(On4StarClicked);
            _star5.onClick.RemoveListener(On5StarClicked);
        }

        private void Show()
        {
            _canvas.enabled = true;
        }

        private void On1StarClicked()
        {
            ShowButtons();
            _star1.image.sprite = _activeStar;
        }

        private void On2StarClicked()
        {
            ShowButtons();
            _star1.image.sprite = _activeStar;
            _star2.image.sprite = _activeStar;
        }

        private void On3StarClicked()
        {
            ShowButtons();
            _star1.image.sprite = _activeStar;
            _star2.image.sprite = _activeStar;
            _star3.image.sprite = _activeStar;
        }

        private void On4StarClicked()
        {
            ShowButtons();
            _star1.image.sprite = _activeStar;
            _star2.image.sprite = _activeStar;
            _star3.image.sprite = _activeStar;
            _star4.image.sprite = _activeStar;
        }

        private void On5StarClicked()
        {
            ShowButtons();
            _star1.image.sprite = _activeStar;
            _star2.image.sprite = _activeStar;
            _star3.image.sprite = _activeStar;
            _star4.image.sprite = _activeStar;
            _star5.image.sprite = _activeStar;
        }

        private void OnSubmit()
        {
            PlayerPrefs.SetString(RateUsKey, "done");
            OpenMarket();
            Destroy(gameObject);
        }

        private void OnNotNowClicked()
        {
            PlayerPrefs.SetString(RateUsKey, "canceled");
            Destroy(gameObject);
        }

        private void ShowButtons()
        {
            _submit.gameObject.SetActive(true);
            _star1.interactable = false;
            _star2.interactable = false;
            _star3.interactable = false;
            _star4.interactable = false;
            _star5.interactable = false;
        }

        private void OpenMarket()
        {
            Application.OpenURL("market://details?id=org.Agava.MyAntFarm");
        }
    }
}
