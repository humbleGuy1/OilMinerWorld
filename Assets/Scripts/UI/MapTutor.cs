using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MapTutor : MonoBehaviour
    {
        [SerializeField] private GameObject _fingerClick;
        [SerializeField] private AnthillChooseButtons _chooseButtons;

        private const string FirstTransition = "FirstTransition";
        private const int True = 1;

        private void OnEnable()
        {
            if(PlayerPrefs.GetInt(FirstTransition) == True)
                enabled = false;
            else
                Invoke(nameof(ShowTutor), 1f);
        }

        private void ShowTutor()
        {
            _chooseButtons.AnthillClicked += HideTutor;
            _fingerClick.SetActive(true);
        }

        private void HideTutor()
        {
            _fingerClick.SetActive(false);

            PlayerPrefs.SetInt(FirstTransition, True);
            _chooseButtons.AnthillClicked -= HideTutor;
        }
    }
}
