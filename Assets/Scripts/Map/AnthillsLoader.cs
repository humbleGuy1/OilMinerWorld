using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AnthillsLoader : MonoBehaviour
    {
        [SerializeField] private bool _test = false;
        [SerializeField] private AnthillSwitcher _anthillSwitcher;
        [SerializeField] private Button _mapTestButton;
        [SerializeField] private GameObject _hideImageUntilLoaded;

        private void OnEnable()
        {
            if(_hideImageUntilLoaded != null )
                _hideImageUntilLoaded.SetActive(true);

            var level1 = new LevelSave(LevelsGUIDData.Level1GUID);
            level1.Load();


            _mapTestButton.onClick.AddListener(OnMapClick);

            _anthillSwitcher.AnthillFirst += OnFirstAnthillChose;
            _anthillSwitcher.AnthillSecond += OnSecondAnthillChose;
            _anthillSwitcher.AnthillThird += OnThirdAnthillChose;
            _anthillSwitcher.AnthillFourth += OnFourthAnthillChose;
            _anthillSwitcher.AnthillFifth += OnFifthAnthillChose;
            _anthillSwitcher.AnthillSixth += OnSixthAnthillChose;



            if(level1.Done == false && _test == false)
            {
                OnFirstAnthillChose();

                return;
            }

            if (_hideImageUntilLoaded != null)
                _hideImageUntilLoaded.SetActive(false);
        }

        private void OnDisable()
        {
            _mapTestButton.onClick.RemoveListener(OnMapClick);

            _anthillSwitcher.AnthillFirst -= OnFirstAnthillChose;
            _anthillSwitcher.AnthillSecond -= OnSecondAnthillChose;
            _anthillSwitcher.AnthillThird -= OnThirdAnthillChose;
            _anthillSwitcher.AnthillFourth -= OnFourthAnthillChose;
            _anthillSwitcher.AnthillFifth -= OnFifthAnthillChose;
            _anthillSwitcher.AnthillSixth -= OnSixthAnthillChose;
        }

        private void OnMapClick()
        {
            LocalMap.Load();
        }

        private void OnFirstAnthillChose()
        {
            Map_1.Load();
        }

        private void OnSecondAnthillChose()
        {
            Map_2.Load();
        }

        private void OnThirdAnthillChose()
        {

        }

        private void OnFourthAnthillChose()
        {

        }

        private void OnFifthAnthillChose()
        {

        }

        private void OnSixthAnthillChose()
        {

        }
    }
}
